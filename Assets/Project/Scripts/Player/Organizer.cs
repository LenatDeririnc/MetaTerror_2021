using Player.Movement;
using UnityEngine;

namespace Player
{
    public class Organizer : MonoBehaviour
    {
        [Header("Components")]
        public Transform cameraTransform;
        public MovingObject movingObject;
    
        [Header("Third Person")]
        public Transform thirdPersonModel;
        public Animator thirdPersonAnimator;
        public float thirdPersonRotationSpeed = 10f;

        [Header("First Person")]
        public Transform firstPersonModel;
        public float firstPersonRotationSpeed = 20f;
        public float firstPersonViewBobMinVelocity = 0.1f;
        public float firstPersonViewBobMaxVelocity = 2f;

        private float lookHorizontal;
        private float lookVertical;
        private Vector3 cameraOriginalPosition;

        public Quaternion LookQuaternion => Quaternion.Euler(lookVertical, lookHorizontal, 0);
        public Quaternion ForwardQuaternion => Quaternion.Euler(0, lookHorizontal, 0);

        public void SetMovementDirection(Vector3 direction)
        {
            movingObject.InputDirection = new Vector3(direction.x, direction.z);
        }

        public void AddLookDirection(float horizontal, float vertical)
        {
            lookHorizontal = Mathf.Repeat(lookHorizontal + horizontal, 360f);
            lookVertical = Mathf.Clamp(lookVertical + vertical, -90f, 90f);
        }

        private void LateUpdate()
        {
            UpdateAnimations();
        }

        private void UpdateAnimations()
        {
            var lookDirection = LookQuaternion;
            var forwardDirection = ForwardQuaternion;

            var firstPersonPosition = cameraTransform.position;
            var firstPersonRotation = Quaternion.Slerp(firstPersonModel.rotation, lookDirection,
                Time.deltaTime * firstPersonRotationSpeed);

            var velocity3d = movingObject.objectRigidbody.velocity;
            var velocity2dMagnitude = new Vector2(velocity3d.x, velocity3d.z).magnitude;
            var bobAmount = Mathf.InverseLerp(
                firstPersonViewBobMinVelocity, 
                firstPersonViewBobMaxVelocity, 
                velocity2dMagnitude);
        
            // Тряска рукой. Возможно лучше передавать в аниматор, но вроде выглядит неплохо
            firstPersonPosition += lookDirection * new Vector3(
                Mathf.Cos(Time.time * 7.5f) * 0.02f, 
                Mathf.Sin(Time.time * 15f) * 0.02f) * bobAmount;
        
            cameraTransform.rotation = lookDirection;
            firstPersonModel.rotation = firstPersonRotation;
            firstPersonModel.position = firstPersonPosition;
            thirdPersonModel.rotation = Quaternion.Slerp(thirdPersonModel.rotation, forwardDirection,
                Time.deltaTime * thirdPersonRotationSpeed);
        
            var localVelocity = Quaternion.Inverse(forwardDirection) * velocity3d;
            thirdPersonAnimator.SetFloat("Velocity", velocity2dMagnitude);
            thirdPersonAnimator.SetFloat("ForwardVelocity", localVelocity.z);
            thirdPersonAnimator.SetFloat("SideVelocity", localVelocity.x);
        }
    }
}
