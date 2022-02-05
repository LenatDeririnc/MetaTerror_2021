using UnityEngine;
using UnityEngine.InputSystem;

public class DrumStick : MonoBehaviour
{
    public Transform tipTransform;
    public LayerMask drumLayerMask = Physics.DefaultRaycastLayers;
    public float minHitSpeed = 1f;
    public ParticleBuilder particle;

    private Vector3 lastPosition;
    
    public InputActionReference clickForParticle;
    public Transform particlePosition;

    private void Awake()
    {
        lastPosition = tipTransform.position;

        clickForParticle.action.started += _ => particle.Play(particlePosition.position);
        clickForParticle.action.canceled += _ => particle.Stop();
    }

    private void Update()
    {
        var newPosition = tipTransform.position;

        if (RhythmGamePlayer.Instance && Physics.Linecast(lastPosition, newPosition, 
            out var hitInfo, drumLayerMask, QueryTriggerInteraction.Collide))
        {
            var speed = Vector3.Distance(lastPosition, newPosition) / Time.deltaTime;

            if (speed > minHitSpeed)
            {
                var drum = hitInfo.collider.GetComponentInParent<Drum>();

                if (drum)
                    drum.Hit();
            }
        }
        
        // Debug.DrawLine(lastPosition, newPosition, Color.magenta, 0.25f);
        lastPosition = newPosition;
    }
}
