using System.Collections;
using UnityEngine;
using UnityEngine.XR.Management;

namespace Rhythm
{
    public class VRPlayer : MonoBehaviour
    {
        private void Awake()
        {
            RhythmGamePlayer.Instance.isVRPlayerPresent = true;
            StartCoroutine(Init());
        }

        private IEnumerator Init()
        {
            yield return XRGeneralSettings.Instance.Manager.InitializeLoader();
            XRGeneralSettings.Instance.Manager.StartSubsystems();
        }
    }
}