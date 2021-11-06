using System.Linq;
using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace Minigames
{
    public class GenericMiniGame : MiniGameBase
    {
        public float segmentAngle;
        public Image[] segments;
        public Image arrow;
        public Color inactiveColor = Color.red;
        public Color activeColor = Color.green;

        public AudioSource effectSource;
        public AudioClip successClip;
        public AudioClip failedClip;
        
        public float cursorStartVelocity = 1f;
        public float cursorMaxVelocity = 30f;
        public float cursorVelocityAcceleration = 5f;

        public InputActionReference interactAction;
        
        private bool[] segmentClicked = new bool[0];
        private float[] segmentRotations = new float[0];

        private float cursorRotation;
        private float cursorVelocity;

        protected override void Awake()
        {
            base.Awake();
            
            StartNewGame(_ => {});
        }

        protected override void OnGameFinished()
        {
            
        }

        private void Update()
        {
            if(!IsGameRunning)
                return;

            cursorVelocity = Mathf.MoveTowards(cursorVelocity, cursorMaxVelocity,
                Time.deltaTime * cursorVelocityAcceleration);
            cursorRotation += cursorVelocity * Time.deltaTime;
            cursorRotation = Mathf.Repeat(cursorRotation, 360f);

            var hoverSegment = GetHoverSegment();

            if (interactAction.action.WasPressedThisFrame())
            {
                if (hoverSegment != NO_SEGMENT && !segmentClicked[hoverSegment])
                {
                    segments[hoverSegment].color = activeColor;
                    segmentClicked[hoverSegment] = true;
                    OnSegmentClicked(hoverSegment, segmentClicked.Count(b => !b));
                }
                else
                {
                    OnSegmentMiss();
                }
            }
            
            for (var i = 0; i < segments.Length; i++)
            {
                var c = segments[i].color;
                c.a = segmentClicked[i] || hoverSegment == i ? 0.5f : 0.25f;
                segments[i].color = c;
            }

            var arrowColor = arrow.color;
            arrowColor.a = hoverSegment != NO_SEGMENT ? 1f : 0.5f;
            arrow.color = arrowColor;
            arrow.transform.rotation = Quaternion.Euler(0, 0, -cursorRotation);
        }

        private void OnSegmentClicked(int segment, int segmentsLeft)
        {
            if (segmentsLeft == 0)
            {
                FinishGame(MiniGameResult.Success);
            }

            switch (segmentsLeft)
            {
                case 2: effectSource.pitch = 0.8f; break;
                case 1: effectSource.pitch = 0.9f; break;
                case 0: effectSource.pitch = 1f; break;
            }

            effectSource.PlayOneShot(successClip);
            segments[segment].transform.localScale = Vector3.one;
            segments[segment].transform.DOPunchScale(new Vector3(0.2f, 0.2f, 0.2f), 0.25f)
                .SetDelay(0.1f);
            
            arrow.transform.localScale = Vector3.one;
            arrow.transform.transform.DOPunchScale(new Vector3(0.1f, 0.1f, 0.1f), 0.25f);
        }

        private void OnSegmentMiss()
        {
            cursorVelocity = 0f;
            effectSource.pitch = 1f;
            effectSource.PlayOneShot(failedClip);
        }

        private int GetHoverSegment()
        {
            for (var i = 0; i < segments.Length; i++)
            {
                if (IsNear(segmentRotations[i], cursorRotation))
                    return i;
            }

            return NO_SEGMENT;
        }

        private bool IsNear(float segmentPosition, float cursorPosition)
        {
            var halfAngle = segmentAngle / 2f;
            return Mathf.Abs(segmentPosition - cursorPosition) < halfAngle || 
                   Mathf.Abs(segmentPosition - (cursorPosition + 360)) < halfAngle;
        }

        protected override void OnGameStarted()
        {
            cursorRotation = 0f;
            cursorVelocity = cursorStartVelocity;
            
            segmentRotations = new float[segments.Length];
            segmentClicked = new bool[segments.Length];
            
            var rotationPerSegment = 360f / segments.Length;
            var halfAngle = segmentAngle / 2f;
            var randomRotation = Random.Range(0f, 360f);

            for (var i = 0; i < segments.Length; i++)
            {
                var segment = segments[i];
                var rotation = randomRotation + i * rotationPerSegment + 
                               Mathf.Lerp(halfAngle, rotationPerSegment - halfAngle, Random.value);

                segment.transform.rotation = Quaternion.Euler(0, 0, -rotation);
                segmentRotations[i] = Mathf.Repeat(rotation, 360f);

                segment.color = inactiveColor;
            }
        }

        private const int NO_SEGMENT = -1;
    }
}