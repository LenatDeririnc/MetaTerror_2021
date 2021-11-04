﻿using UnityEngine;

namespace Common.Components
{
    public class HideMeshOnStart : MonoBehaviour
    {
        public MeshRenderer target;
        
        private void Start()
        {
            if (target != null)
                target.enabled = false;
        }
    }
}