using Infrastructure.Components;
using UnityEngine;

namespace Rhythm
{
    public class VRPlayerBootstrapper : MonoBehaviour
    {
        public GameObject vrDrumPlayer;
        public GameObject animatedDrumPlayer;
        
        public void Start()
        {
            RhythmGamePlayer.Instance.isVRPlayerPresent = GameBootstrapper.Instance.VR_mode;

            if(!GameBootstrapper.Instance.VR_mode)
            {
                animatedDrumPlayer.SetActive(true);
                vrDrumPlayer.SetActive(false);
                RhythmGamePlayer.Instance
                    .PlayTrack(RhythmGamePlayer.Instance.startTrack);
            }
            else
            {
                animatedDrumPlayer.SetActive(false);
                vrDrumPlayer.SetActive(true);
            }
        }
    }
}