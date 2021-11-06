using System;
using Player.Interfaces;
using UnityEngine;

namespace Player
{
    public class InteractRaycaster : MonoBehaviour
    {
        [SerializeField] private Transform cameraTransform;
        [SerializeField] private float checkDistance;
        [SerializeField] private LayerMask interactMask;

        public Organizer organizer;
        public static Action<IInteractable> ONTargetOver;
        public static Action ONInteractInput;
        private IInteractable _interactableObject;

        private void OnEnable()
        {
            ONInteractInput += OnInteractio;
        }
        
        private void OnDisable()
        {
            ONInteractInput -= OnInteractio;
        }

        private void Update()
        {
            var ray = new Ray(cameraTransform.position, cameraTransform.forward * checkDistance);
            var isRaycast = Physics.Raycast(ray, out var hit, checkDistance, interactMask);
            
            if (!isRaycast)
            {
                _interactableObject = null;
                return;
            }

            _interactableObject = hit.collider.GetComponentInParent<IInteractable>();
            ONTargetOver?.Invoke(_interactableObject);
        }
        
        // NOTE: Оно почему-то колбечит отсюда эветн нажатия интеракта автоматически, если переименовать в OnInteract()
        // Почему - не ебу.
        private void OnInteractio()
        {
            if(organizer.IsControllable)
                _interactableObject?.OnInteract(organizer);
        }

        private void OnDrawGizmos()
        {
            if (cameraTransform == null)
                return;
            
            Gizmos.color = Color.red;
            Gizmos.DrawRay(cameraTransform.position, cameraTransform.forward * checkDistance);
        }
    }
}