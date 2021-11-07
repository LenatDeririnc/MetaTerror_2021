using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Management;

namespace Rhythm
{
    public class VRPlayer : MonoBehaviour
    {
        public Transform recenterTarget;
        public Transform cameraTransform;
        public Transform rigRootTransform;
        
        public InputActionReference recenter;

        private void OnEnable()
        {
            RhythmGamePlayer.Instance.isVRPlayerPresent = true;
            StartCoroutine(Init());
            
            recenter.asset.Enable();
        }

        private IEnumerator Init()
        {
            yield return XRGeneralSettings.Instance.Manager.InitializeLoader();
            XRGeneralSettings.Instance.Manager.StartSubsystems();
        }

        private void Update()
        {
            if (recenter.action.WasPressedThisFrame())
            {   
                var diff = cameraTransform.position - rigRootTransform.position;
                rigRootTransform.position = recenterTarget.position - diff;
            }
        }

        private void OnDisable()
        {
            RhythmGamePlayer.Instance.isVRPlayerPresent = false;
            
            if (XRGeneralSettings.Instance.Manager.isInitializationComplete)
            {
                XRGeneralSettings.Instance.Manager.StopSubsystems();
                XRGeneralSettings.Instance.Manager.DeinitializeLoader();
            }
        }
    }
}