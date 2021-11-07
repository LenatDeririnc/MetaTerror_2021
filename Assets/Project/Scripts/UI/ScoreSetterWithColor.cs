using Common.UI;
using DG.Tweening;
using UnityEngine;

namespace UI
{
    public class ScoreSetterWithColor : ScoreSetter
    {
        private Color currentColor = Color.white;

        [SerializeField] private Color goolColor = Color.white;
        [SerializeField] private Color badColor = Color.red;
        [SerializeField] private int blinkCount = 2;
        [SerializeField] private float interval;

        public void Blink()
        {
            DOTween.Sequence()
                .Append(_text.DOColor(badColor, interval))
                .Append(_text.DOColor(goolColor, interval))
                .SetLoops(blinkCount);
        }
    }
}