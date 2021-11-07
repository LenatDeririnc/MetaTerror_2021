using DG.Tweening;
using Rhythm;
using UnityEngine;

public class Drum : MonoBehaviour
{
    public DrumChannel channel;
    public SkinnedMeshRenderer renderer;
    public AnimationCurve hitCurve;

    public void Hit()
    {
        if(RhythmGamePlayer.Instance) 
            RhythmGamePlayer.Instance.Hit(channel);

        DOTween.Kill(this);
        DOTween.To(() => 0f, b => { renderer.SetBlendShapeWeight(0, b * 100f); }, 1f, 
                hitCurve[hitCurve.length - 1].time)
            .SetEase(hitCurve)
            .SetTarget(this);
    }
}
