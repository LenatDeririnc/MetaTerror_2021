using DG.Tweening;
using Rhythm;
using UnityEngine;

public class Piano : Drum
{
    public Transform noteTransform;
    public float noteRotaion;
    
    public override void Hit()
    {
        base.Hit();

        noteTransform.DOKill();
        noteTransform.localRotation = Quaternion.identity;
        noteTransform.DOLocalRotate(new Vector3(noteRotaion, 0, 0), hitCurve[hitCurve.length - 1].time)
            .SetEase(hitCurve);
    }
}
