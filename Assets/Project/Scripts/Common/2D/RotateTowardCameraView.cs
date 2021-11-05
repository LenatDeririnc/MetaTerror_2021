using System;
using UnityEngine;

namespace Common._2D
{
    public class RotateTowardCameraView : MonoBehaviour
    {
        private Transform _transform;

        private Camera _currentCamera;
        Camera CurrentCamera
        {
            get
            {
                _currentCamera ??= Camera.main;
                return _currentCamera;
            }
        }

        private void Awake()
        {
            _transform = transform;
        }

        private void Update()
        {
            if (CurrentCamera == null)
                return;
            
            _transform.forward = -CurrentCamera.transform.forward;
        }
    }
}