using UnityEngine;
using UnityEngine.UI;

namespace Rhythm
{
    public class TrackPositionDisplay : MonoBehaviour
    {
        public Image barImage;
        public Transform barPosition;

        private void Awake()
        {
            barImage.fillAmount = 0f;
        }

        private void Update()
        {
            var player = RhythmGamePlayer.Instance;
            
            if(!player && player.Duration != 0)
                return;

            barImage.fillAmount = player.Position / player.Duration;
            var position = barPosition.transform.localPosition;
            position.x = ((RectTransform) barPosition.parent).rect.width * barImage.fillAmount;
            barPosition.transform.localPosition = position;
        }
    }
}