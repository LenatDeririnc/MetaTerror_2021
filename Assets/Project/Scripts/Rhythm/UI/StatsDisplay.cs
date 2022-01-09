using System;
using DG.Tweening;
using DG.Tweening.Core;
using TMPro;
using UnityEngine;

namespace Rhythm
{
    public class StatsDisplay : MonoBehaviour
    {
        public TMP_Text scoreText;
        public TMP_Text hitPercentText;

        public Color positiveColor;
        public Color negativeColor;

        private float currentScore;
        private float currentHitPercent = 1f;

        private RhythmGamePlayer.PlayStats lastStats;
        
        private void Update()
        {
            if(!RhythmGamePlayer.Instance)
                return;

            var stats = RhythmGamePlayer.Instance.Stats;

            if (stats.score != lastStats.score)
            {
                Increment(scoreText, currentScore, stats.score, s =>
                {
                    currentScore = s;
                    scoreText.text = Mathf.FloorToInt(s).ToString();
                });
            }

            if (stats.hits != lastStats.hits || 
                stats.misses != lastStats.misses)
            {
                var hitPercent = stats.hits > 0 ? stats.hits / (float) (stats.hits + stats.misses) : 1f;
                Increment(hitPercentText, currentHitPercent, hitPercent, s =>
                {
                    currentHitPercent = s;
                    hitPercentText.SetText("{0:00.0}%", hitPercent * 100f);
                });
            }
            
            lastStats = stats;
        }

        private void Increment(TMP_Text text, float currentAmount, float newAmount, DOSetter<float> onUpdate)
        {
            if(currentAmount == newAmount)
                return;
            
            text.rectTransform.DOKill();
            text.DOKill();

            DOTween.To(() => currentAmount, onUpdate, newAmount, 0.5f)
                .SetEase(Ease.InOutSine)
                .SetTarget(text);

            if (currentAmount < newAmount)
            {
                text.rectTransform.pivot = new Vector2(0.5f, 0.3f);
                text.rectTransform.DOPivot(new Vector2(0.5f, 0.5f), 0.1f);
                text.color = positiveColor;
                text.DOColor(Color.white, 0.15f);
            }
            else
            {
                text.color = negativeColor;
                text.DOColor(Color.white, 0.15f);
                text.rectTransform.pivot = new Vector2(0.5f, 0.7f);
                text.rectTransform.DOPivot(new Vector2(0.5f, 0.5f), 0.1f);
            }
        }
    }
}