using DG.Tweening;
using Rhythm;
using UnityEngine;

public class MusicLights : MonoBehaviour
{
    public DrumChannel channel;
    public Light light;
    public float lightFadeDuration = 0.5f;

    private float originalIntensity;

    private void Awake()
    {
        originalIntensity = light.intensity;
        light.intensity = 0;
    }

    private void OnEnable()
    {
        RhythmGamePlayer.Instance.OnNoteHitListener += OnNoteHit;
    }

    private void OnDisable()
    {
        RhythmGamePlayer.Instance.OnNoteHitListener -= OnNoteHit;
    }

    private void OnNoteHit(RhythmTrack.Note note, float score)
    {
        if(note.channel != channel)
            return;

        light.intensity = originalIntensity * score;
        light.DOKill();
        light.DOIntensity(0f, lightFadeDuration)
            .SetEase(Ease.InFlash);
    }
}
