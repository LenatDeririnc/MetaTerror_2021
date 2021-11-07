using System;
using Services.Input;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class OrganizerPlayerInput : MonoBehaviour
    {
        public Organizer organizer;

        public PlayerControls Controls;

        private void OnEnable()
        {
            Controls = InputService.CurrentInput.Controls;
            if (Controls == null)
                return;
            
            Controls.Enable();

            var organizer = Controls.Organizer;
            
            organizer.Interact.performed += HandleInteract;
        }

        private void OnDisable()
        {
            if (Controls == null)
                return;
            
            Controls.Disable();
            
            var organizer = Controls.Organizer;
            
            organizer.Interact.performed -= HandleInteract;
        }

        private void Update()
        {
            if (!organizer.IsControllable)
            {
                organizer.SetMovementDirection(Vector3.zero);
                return;
            }

            var organizerControls = Controls.Organizer;
            
            var lookValue = organizerControls.Look.ReadValue<Vector2>();
            organizer.AddLookDirection(lookValue.x, lookValue.y);
        
            var movementValue = organizerControls.Movement.ReadValue<Vector2>();
            var lookMovementValue = organizer.ForwardQuaternion * new Vector3(movementValue.x, 0, movementValue.y);

            organizer.SetMovementDirection(lookMovementValue);
        }

        private void HandleInteract(InputAction.CallbackContext obj)
        {
            InteractRaycaster.ONInteractInput?.Invoke();
        }
    }
}