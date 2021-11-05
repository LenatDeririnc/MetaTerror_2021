using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Rhythm;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class RhythmGamePlayer : MonoBehaviour
{
    public RhythmTrack startTrack;
    public AudioSource goodTrackSource;
    public AudioSource badTrackSource;

    public float noteOffset;
    public float noteShowDuration;
    public float playbackSpeed = 1f;
    public AnimationCurve scoringCurve;

    public PlayStats Stats => stats;
    
    public IReadOnlyList<DisplayNote> DisplayedNotes => currentDisplayNotes;

    private RhythmTrack currentTrack;
    private int currentTrackNote;
    private float currentAudioMix;
    private float targetAudioMix;
    
    private readonly List<RhythmTrack.Note> currentNotes = new();
    private readonly HashSet<RhythmTrack.Note> hitNotes = new();
    private readonly List<DisplayNote> currentDisplayNotes = new();
    private PlayStats stats;

    public void PlayTrack(RhythmTrack track)
    {
        DOTween.Kill(this);
        
        currentTrack = track;
        currentTrackNote = 0;
        targetAudioMix = -1f;
        stats = new PlayStats();
        
        currentNotes.Clear();
        hitNotes.Clear();

        PlayClip(badTrackSource, track.badAudioClip);
        PlayClip(goodTrackSource, track.goodAudioClip);
        MixGoodBadAudio(1f);
    }

    private void LateUpdate()
    {
        if(Keyboard.current.digit1Key.wasPressedThisFrame)
            Hit(0);
        
        if(Keyboard.current.digit2Key.wasPressedThisFrame)
            Hit(1);
        
        if(Keyboard.current.digit3Key.wasPressedThisFrame)
            Hit(2);
        
        if(Keyboard.current.digit4Key.wasPressedThisFrame)
            Hit(3);
    }

    public void OnEnable()
    {
        PlayTrack(startTrack);
    }
    
    /// <summary>
    /// Ударить по ноте
    /// </summary>
    /// <param name="channel">Канал на котором находится нота</param>
    public void Hit(int channel)
    {
        if(scoringCurve.length == 0)
            return;
        
        var currentTime = goodTrackSource.time + noteOffset;
        var noteOnOffset = scoringCurve.keys[0].time;
        var noteOffOffset = scoringCurve.keys[scoringCurve.length - 1].time;
        var hasHit = false;
        float score;
        
        for (var i = 0; i < currentNotes.Count; i++)
        {
            var note = currentNotes[i];
            
            if(note.channel != channel)
                continue;

            if (note.onTime + noteOnOffset <= currentTime && 
                note.offTime + noteOffOffset >= currentTime)
            {
                if (hitNotes.Contains(note))
                {
                    // ты чё дурак, уже ударял по этой ноте, никаких очков
                    Debug.Log("Repeat hit");
                    break;
                }
                
                hitNotes.Add(note);
                hasHit = true;

                // ударил в ноту
                if (note.onTime <= currentTime && note.offTime >= currentTime)
                {
                    score = scoringCurve.Evaluate(0f);
                }
                // ударил раньше
                else if (note.onTime > currentTime)
                {
                    score = scoringCurve.Evaluate(currentTime - note.onTime);
                }
                // ударил позже
                else
                {
                    score = scoringCurve.Evaluate(currentTime - note.offTime);
                }
                
                OnNoteHit(note, score);
            }
        }

        if (!hasHit)
        {
            OnNoteMiss();
        }
    }

    private void OnNoteMiss()
    {
        stats.misses++;

        AnimateAudioMix(0f);
        
        Debug.Log("Note Missed");
    }

    private void OnNoteHit(RhythmTrack.Note note, float score)
    {
        stats.normalizedScore += score;
        stats.hits++;

        if (score == 1f)
            stats.perfectHits++;
        
        AnimateAudioMix(1f);

        Debug.Log("HitNote: " + note.channel + ", score: " + score);
    }

    private void Update()
    {
        UpdateDisplayNotes();
    }

    private void UpdateDisplayNotes()
    {
        var currentTime = goodTrackSource.time + noteOffset;
        var curveEnd = scoringCurve[scoringCurve.length - 1].time;

        var noteStartTime = currentTime + noteShowDuration;
        var noteEndTime = currentTime - curveEnd;

        for (var i = currentNotes.Count - 1; i >= 0; i--)
        {
            var note = currentNotes[i];

            if (note.offTime <= noteEndTime)
            {
                if (!hitNotes.Contains(note))
                    OnNoteMiss();

                currentNotes.RemoveAt(i);
            }
        }
        
        while (currentTrackNote < currentTrack.trackNotes.Length)
        {
            var note = currentTrack.trackNotes[currentTrackNote];

            if (note.onTime <= noteStartTime)
            {
                currentNotes.Add(note);
                currentTrackNote++;
            }
            else
            {
                break;
            }
        }

        currentDisplayNotes.Clear();
        
        for (var i = 0; i < currentNotes.Count; i++)
        {
            var note = currentNotes[i];
            
            currentDisplayNotes.Add(new DisplayNote
            {
                originalNote = note,
                channel = note.channel,
                startPosition = Mathf.InverseLerp(0, noteShowDuration, note.onTime - currentTime),
                endPosition = Mathf.InverseLerp(0, noteShowDuration, note.offTime - currentTime)
            });
        }
    }

    private void PlayClip(AudioSource source, AudioClip c)
    {
        source.Stop();
        source.clip = c;
        source.pitch = playbackSpeed;
        source.time = 0f;
        source.Play();
    }

    private void MixGoodBadAudio(float goodAudio)
    {
        currentAudioMix = goodAudio;
        goodTrackSource.volume = goodAudio;
        badTrackSource.volume = 1f - goodAudio;
    }

    private void AnimateAudioMix(float goodAudio)
    {
        if(targetAudioMix == goodAudio)
            return;

        targetAudioMix = goodAudio;
        DOTween.Kill(this);
        DOTween.To(() => currentAudioMix, MixGoodBadAudio, goodAudio, 0.15f)
            .SetTarget(this);
    }
    
    private void OnDrawGizmos()
    {
        if(currentTrack == null)
            return;
        
        var channelCount = currentTrack.trackNotes
            .DistinctBy(n => n.channel)
            .Count();

        var bottomLeft = transform.position;
        var bottomRight = bottomLeft + Vector3.right;
        var topLeft = bottomLeft + Vector3.up;
        var topRight = bottomRight + Vector3.up;
        
        Gizmos.DrawLine(bottomLeft, bottomRight);
        Gizmos.DrawLine(topLeft, topRight);
        Gizmos.DrawLine(bottomLeft, topLeft);
        Gizmos.DrawLine(bottomRight, topRight);

        for (var i = 1; i < channelCount; i++)
        {
            var position = Mathf.InverseLerp(0f, channelCount, i);
            var topPosition = Vector3.Lerp(topLeft, topRight, position);
            var bottomPosition = Vector3.Lerp(bottomLeft, bottomRight, position);
            
            Gizmos.DrawLine(topPosition, bottomPosition);
        }

        Vector3 GetPoint(float x, float y)
        {
            var topLeftRight = Vector3.Lerp(topLeft, topRight, x);
            var bottomLeftRight = Vector3.Lerp(bottomLeft, bottomRight, x);

            return Vector3.Lerp(bottomLeftRight, topLeftRight, y);
        }

        for (var i = 0; i < DisplayedNotes.Count; i++)
        {
            var note = DisplayedNotes[i];
            var leftPosition = Mathf.InverseLerp(0f, channelCount, note.channel);
            var rightPosition = Mathf.InverseLerp(0f, channelCount, note.channel + 1);
            var topPosition = note.endPosition;
            var bottomPosition = note.startPosition;

            var noteTopLeft = GetPoint(leftPosition, topPosition);
            var noteBottomRight = GetPoint(rightPosition, bottomPosition);

            var noteBounds = new Bounds();
            noteBounds.SetMinMax(noteTopLeft, noteBottomRight);

            if (hitNotes.Contains(note.originalNote))
            {
                Gizmos.color = new Color(0f, 1f, 0, 0.25f);
            }
            else
            {
                Gizmos.color = new Color(1f, 0f, 0, 0.25f);
            }
            
            
            Gizmos.DrawCube(noteBounds.center, noteBounds.size);
        }
    }
    
    [Serializable]
    public struct DisplayNote
    {
        public RhythmTrack.Note originalNote;
        public int channel;
        public float startPosition;
        public float endPosition;
    }
    
    [Serializable]
    public struct PlayStats
    {
        public float normalizedScore;
        public int perfectHits;
        public int hits;
        public int misses;
    }
}
