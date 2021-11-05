using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.UI;

namespace Rhythm.UI
{
    public class RhythmBarField : MonoBehaviour
    {
        public int channel;

        public Image background;
        public ParticleSystem hitParticles;

        public Color backgroundColor;
        public Color barColor;
        
        public RhythmGamePlayer rhythmGame;
        public GameObject barPrefab;

        public float appearThreshold = 0.8f;

        private ObjectPool<GameObject> barPool;
        private List<GameObject> instantiatedBars;
        private RectTransform rectTransform;

        private void Awake()
        {
            background.color = backgroundColor;
            rectTransform = GetComponent<RectTransform>();
            instantiatedBars = new List<GameObject>();
            
            barPool = new ObjectPool<GameObject>(
                createFunc: () =>
                {
                    var bar = Instantiate(barPrefab, rectTransform);
                    bar.GetComponent<Image>().color = barColor;
                    return bar;
                },
                actionOnGet: o => o.SetActive(true),
                actionOnRelease: o => o.SetActive(false),
                actionOnDestroy: Destroy
            );
        }

        private void OnEnable()
        {
            rhythmGame.OnNoteHitListener += OnNoteHit;
        }

        private void OnDisable()
        {
            rhythmGame.OnNoteHitListener -= OnNoteHit;
        }

        private void OnNoteHit(RhythmTrack.Note note, float score)
        {
            if(note.channel != channel)
                return;
            
            hitParticles.Play(true);
        }

        private void Update()
        {
            if(!rhythmGame)
                return;

            rhythmGame.GetScore(channel, out var score, out _);
            background.color = Color.Lerp(backgroundColor, barColor, score);
            
            var notes = rhythmGame.DisplayedNotes;

            for (var i = 0; i < instantiatedBars.Count; i++)
                barPool.Release(instantiatedBars[i]);

            instantiatedBars.Clear();

            for (var i = 0; i < notes.Count; i++)
            {
                var note = notes[i];
                
                if(note.channel != channel)
                    continue;
                
                var bar = barPool.Get();
                instantiatedBars.Add(bar);

                var rect = bar.GetComponent<RectTransform>();
                rect.SetAsFirstSibling();
                rect.anchorMin = new Vector2(0f, note.startPosition);
                rect.anchorMax = new Vector2(1f, note.endPosition);

                var image = bar.GetComponent<Image>();
                var imageColor = image.color;
                imageColor.a = Mathf.InverseLerp(1f, appearThreshold, note.startPosition);
                image.color = imageColor;

                bar.SetActive(rect.rect.height > 0f);
            }
        }
    }
}