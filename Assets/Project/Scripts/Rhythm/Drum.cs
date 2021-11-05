using Rhythm;
using UnityEngine;

public class Drum : MonoBehaviour
{
    public DrumChannel channel;
    public Animator animator;

    public void Hit()
    {
        if(RhythmGamePlayer.Instance) 
            RhythmGamePlayer.Instance.Hit(channel);

        if (animator)
            animator.SetTrigger("Hit");
    }
}
