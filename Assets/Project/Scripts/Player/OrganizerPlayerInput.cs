using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class OrganizerPlayerInput : MonoBehaviour
    {
        public Organizer organizer;
    
        public InputActionReference movement;
        public InputActionReference look;

        public void Update()
        {
            var lookValue = look.action.ReadValue<Vector2>();
            organizer.AddLookDirection(lookValue.x, lookValue.y);
        
            var movementValue = movement.action.ReadValue<Vector2>();
            var lookMovementValue = organizer.ForwardQuaternion * new Vector3(movementValue.x, 0, movementValue.y);

            organizer.SetMovementDirection(lookMovementValue);
        }
    }
}