using UnityEngine;

namespace Rhythm
{
    public class DrumStickTrigger : MonoBehaviour
    {
        [SerializeField] private DrumStick _drumStick;
        [SerializeField] private CapsuleCast _capsuleCast;
        
        public DrumStick DrumStick => _drumStick;
        private Transform _transform;

        private void Awake()
        {
            _transform = transform;
            _capsuleCast.Awake(_transform, OnCast, OnEndCast);
        }

        private void OnCast(RaycastHit hit)
        {
            var otherTrigger = hit.collider.GetComponent<DrumStickTrigger>();
            if (otherTrigger == null)
                return;

            DrumStick.particle.Play(hit.point);
        }

        private void OnEndCast()
        {
            DrumStick.particle.Stop();
        }

        private void Update()
        {
            _capsuleCast.Update();
        }
    }
}