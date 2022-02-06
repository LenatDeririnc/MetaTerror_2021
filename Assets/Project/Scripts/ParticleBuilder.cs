using UnityEngine;

public class ParticleBuilder : MonoBehaviour
{
    [SerializeField] private GameObject particleObjectAsset;
    private Transform _particleTransform;
    private GameObject _particle;
    private ParticleSystem _particleSystem;

    private void Awake()
    {
        _particle = Instantiate(particleObjectAsset);
        _particleSystem = _particle.GetComponent<ParticleSystem>();
        _particleTransform = _particle.transform;
    }

    public void SetPosition(Vector3 position)
    {
        _particleTransform.position = position;
    }
    
    public void Play()
    {
        _particleSystem.Play();
    }

    public void Play(Vector3 position)
    {
        _particleTransform.position = position;
        _particleSystem.Play();
    }

    public void LocalPlay(Vector3 localPosition)
    {
        _particleTransform.localPosition = localPosition;
        _particleSystem.Play();
    }

    public void Stop()
    {
        _particleSystem.Stop();
    }
    
}