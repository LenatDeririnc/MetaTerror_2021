using System;
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
            Controls = new PlayerControls();
            Controls.Enable();

            var organizer = Controls.Organizer;
            
            organizer.Interact.performed += HandleInteract;
        }

        private void OnDisable()
        {
            Controls.Disable();
            
            var organizer = Controls.Organizer;
            
            organizer.Interact.performed -= HandleInteract;
        }

        private void Update()
        {
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