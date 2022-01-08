using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Rhythm;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public delegate void OnNoteHitListener(RhythmTrack.Note note, float score);

[DefaultExecutionOrder(-10000)]
public class RhythmGamePlayer : MonoBehaviour
{
    public static RhythmGamePlayer Instance { get; private set; }
    
    public float Duration => goodTrackSource.clip.length;
    public float Position => currentPlaybackTime;
    
    public RhythmTrack startTrack;
    public AudioSource goodTrackSource;
    public AudioSource badTrackSource;

    public float noteOffset;
    public float noteShowDuration;
    public float playbackSpeed = 1f;
    public AnimationCurve scoringCurve;

    public PlayStats Stats => stats;
    
    public IReadOnlyList<DisplayNote> DisplayedNotes => currentDisplayNotes;
    
    public event Action<RhythmTrack.Note, float> OnNoteHitListener;
    public event Action OnNoteMissListener;
    
    public bool isVRPlayerPresent = false;
    
    private RhythmTrack currentTrack;
    private int currentTrackNote;
    private float currentAudioMix;
    private float targetAudioMix;
    
    private readonly List<RhythmTrack.Note> currentNotes = new();
    private readonly HashSet<RhythmTrack.Note> hitNotes = new();
    private readonly List<DisplayNote> currentDisplayNotes = new();
    private readonly List<Callback> playCallbacks = new();
    private PlayStats stats;

    private float currentPlaybackTime;
    private float lastSourceTime;

    private void Awake()
    {
        Instance = this;
    }

    public void AddCallback(float time, Action action)
    {
        playCallbacks.Add(new Callback
        {
            time = time,
            action = action
        });
    }

    public void PlayTrack(RhythmTrack track)
    {
        DOTween.Kill(this);
        
        currentTrack = track;
        currentTrackNote = 0;
        stats = new PlayStats();
        
        currentNotes.Clear();
        hitNotes.Clear();

        PlayClip(badTrackSource, track.badAudioClip);
        PlayClip(goodTrackSource, track.goodAudioClip);
        
        MixGoodBadAudio(1f);
        targetAudioMix = 1f;
    }

    private void LateUpdate()
    {
        if(Keyboard.current.digit1Key.wasPressedThisFrame)
            Hit(0);
        
        if(Keyboard.current.digit2Key.wasPressedThisFrame)
            Hit((DrumChannel) 1);
        
        if(Keyboard.current.digit3Key.wasPressedThisFrame)
            Hit((DrumChannel) 2);
        
        if(Keyboard.current.digit4Key.wasPressedThisFrame)
            Hit((DrumChannel) 3);
    }

    private void UpdateCurrentTime()
    {
        var newSourceTime = goodTrackSource.time;

        if (goodTrackSource.isPlaying) 
            currentPlaybackTime += Time.deltaTime;

        if (lastSourceTime != newSourceTime)
        {
            var diff = Mathf.Abs(lastSourceTime - newSourceTime);
            lastSourceTime = newSourceTime;

            if (diff < 0.05f)
                currentPlaybackTime = newSourceTime;
        }
    }

    public bool GetScore(DrumChannel channel, out float score, out RhythmTrack.Note note)
    {
        score = 0;
        note = new RhythmTrack.Note();
        
        if(scoringCurve.length == 0)
            return false;

        var currentTime = currentPlaybackTime + noteOffset;
        var noteOnOffset = scoringCurve.keys[0].time;
        var noteOffOffset = scoringCurve.keys[scoringCurve.length - 1].time;
        var hasHit = false;

        score = 0;
        
        // быдлокод, оптимизировать если надо
        for (var i = 0; i < currentNotes.Count; i++)
        {
            note = currentNotes[i];
            
            if(note.channel != channel)
                continue;

            if (note.onTime + noteOnOffset <= currentTime && 
                note.offTime + noteOffOffset >= currentTime)
            {
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

                break;
            }
        }

        return hasHit;
    }
    
    /// <summary>
    /// Ударить по ноте
    /// </summary>
    /// <param name="channel">Канал на котором находится нота</param>
    public void Hit(DrumChannel channel)
    {
        if (currentTrack != startTrack)
        {
            PlayTrack(startTrack);
        }

        if (GetScore(channel, out var score, out var note))
        {
            if (hitNotes.Add(note))
            {
                OnNoteHit(note, score);
                hitNotes.Add(note);
                return;
            }
        }
        
        OnNoteMiss();
    }

    private void OnNoteMiss()
    {
        stats.misses++;
        stats.combo = 0;

        AnimateAudioMix(0f);
        OnNoteMissListener?.Invoke();
        
        Debug.Log("Note Missed");
    }

    private void OnNoteHit(RhythmTrack.Note note, float score)
    {
        stats.normalizedScore += score;
        stats.hits++;
        stats.combo++;

        if (stats.combo > stats.maxCombo)
            stats.maxCombo = stats.combo;

        if (score == 1f)
            stats.perfectHits++;
        
        AnimateAudioMix(1f);
        OnNoteHitListener?.Invoke(note, score);

        Debug.Log("HitNote: " + note.channel + ", score: " + score);
    }

    private void Update()
    {
        if (isVRPlayerPresent)
            UpdateDisplayNotes();

        UpdateCurrentTime();
        UpdateCallbacks();
    }

    private void UpdateCallbacks()
    {
        for (var i = playCallbacks.Count - 1; i >= 0; i--)
        {
            var callback = playCallbacks[i];

            if (currentPlaybackTime > callback.time)
            {
                callback.action?.Invoke();
                playCallbacks.RemoveAt(i);
            }
        }
    }

    private void UpdateDisplayNotes()
    {
        if (currentTrack == null)
            return;
        
        var currentTime = currentPlaybackTime + noteOffset;
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
            var leftPosition = Mathf.InverseLerp(0f, channelCount, (float) note.channel);
            var rightPosition = Mathf.InverseLerp(0f, channelCount, (float) note.channel + 1);
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
        public DrumChannel channel;
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
        public int combo;
        public int maxCombo;
    }
    
    private struct Callback
    {
        public float time;
        public Action action;
    }
}
