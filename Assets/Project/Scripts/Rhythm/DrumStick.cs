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

    private bool isParticlePlaying = false;

    private void Awake()
    {
        lastPosition = tipTransform.position;

        clickForParticle.action.started += _ => OnPlay();
        clickForParticle.action.canceled += _ => OffPlay();
    }

    private void OffPlay()
    {
        isParticlePlaying = false;
        particle.Stop();
    }

    private void OnPlay()
    {
        isParticlePlaying = true;
        particle.Play();
    }

    private void Update()
    {
        var newPosition = tipTransform.position;

        if (isParticlePlaying)
        {
            particle.SetPosition(particlePosition.position);
        }

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
