using ThreeDISevenZeroR.SensorKit;
using UnityEngine;

namespace Player.Movement
{
    public class MovingObject : MonoBehaviour
    {
        [Header("Components")]
        public Rigidbody objectRigidbody;
    
        [Header("Ground detection")]
        public CastSensor groundSensor;
        public float groundTargetHeight;

        [Header("Settings")]
        public MovementParams movementParams = MovementParams.GetDefault();

        public Vector2 InputDirection;
    
        private bool jumpActive;
        private float jumpControlTimer;

        private bool isGrounded;
        private Vector3 groundVelocity;
        private bool canUnstickFromGroundNextFrame;
        private Vector3 accumulatedPunch;

        public void Punch(Vector3 velocity, bool canUnstick = true)
        {
            accumulatedPunch += velocity;

            if (canUnstick)
                canUnstickFromGroundNextFrame = true;
        }

        public void Jump(float force = 1f)
        {
            if (isGrounded && !jumpActive)
            {
                jumpActive = true;
                jumpControlTimer = movementParams.jumpControlDuration * force;
                canUnstickFromGroundNextFrame = true;
            }
        }

        public void AbortJump()
        {
            jumpActive = false;
            jumpControlTimer = 0f;
        }

        private void FixedUpdate()
        {
            UpdateSensors();
            ApplyAccumulatedPunch();

            var velocityOnGround = objectRigidbody.velocity - groundVelocity;
            var localVelocity = WorldToLocalVelocity(velocityOnGround);

            UpdateGroundDetection();

            var flatVelocity = new Vector2(localVelocity.x, localVelocity.z);

            if (isGrounded)
            {
                var targetVelocity = InputToGroundVelocity(InputDirection);
                UpdateGroundVelocity(ref flatVelocity, targetVelocity);
            }
            else
            {
                var acceleration = InputToAirAcceleration(InputDirection);
                UpdateAirVelocity(ref flatVelocity, acceleration);
            }

            localVelocity.x = flatVelocity.x;
            localVelocity.z = flatVelocity.y;

            UpdateJumpVelocity(ref localVelocity.y);
            StickToGround(ref localVelocity.y);

            localVelocity = Vector3.ClampMagnitude(localVelocity, movementParams.maxAirSpeed);
        
            var worldVelocity = LocalToWorldVelocity(localVelocity);
            objectRigidbody.velocity = worldVelocity + groundVelocity;
        }

        private void UpdateSensors()
        {
            groundSensor.UpdateSensor();
        }

        private void ApplyAccumulatedPunch()
        {
            if (accumulatedPunch != Vector3.zero)
            {
                objectRigidbody.velocity += accumulatedPunch;
                accumulatedPunch = Vector3.zero;
            }
        }

        private void UpdateGroundDetection()
        {
            if (groundSensor.HasHit)
            {
                var groundRigidbody = groundSensor.HitCollider.attachedRigidbody;
                var groundRigidbodyVelocity = groundRigidbody 
                    ? groundRigidbody.velocity
                    : Vector3.zero;

                var velocityDiff = WorldToLocalVelocity(groundRigidbodyVelocity - objectRigidbody.velocity);
                var nextFrameHeightDiff = groundTargetHeight - 
                                          groundSensor.RayHit.distance + 
                                          velocityDiff.y * Time.fixedDeltaTime;
            
                var isBottomReached = nextFrameHeightDiff > 0;
                var isFalling = velocityDiff.y >= 0;

                if (!isGrounded && isFalling && isBottomReached)
                {
                    isGrounded = true;
                } 
                else if (canUnstickFromGroundNextFrame)
                {
                    isGrounded = false;
                }

                groundVelocity = isGrounded 
                    ? groundRigidbodyVelocity 
                    : Vector3.zero;
            }
            else
            {
                isGrounded = false;
                groundVelocity = Vector3.zero;
            }
        
            objectRigidbody.useGravity = !isGrounded;
            canUnstickFromGroundNextFrame = false;
        }

        private void StickToGround(ref float velocity)
        {
            if (isGrounded)
            {
                var heightDiff = groundTargetHeight - groundSensor.RayHit.distance;
                velocity = heightDiff / Time.fixedDeltaTime;
            }
        }
    
        private void UpdateJumpVelocity(ref float velocity)
        {
            if (isGrounded || !jumpActive)
                return;
        
            velocity = movementParams.jumpVelocity;
            jumpControlTimer -= Time.deltaTime;

            if (jumpControlTimer <= 0)
                jumpActive = false;
        }

        private void UpdateGroundVelocity(ref Vector2 velocity, Vector2 targetVelocity)
        {
            targetVelocity = Vector2.ClampMagnitude(targetVelocity, movementParams.maxSpeed);

            var reactivity = 1f;
        
            if (targetVelocity != Vector2.zero)
            {
                var reactivityAngle = Mathf.LerpUnclamped(90f, 0f, Vector2.Dot(velocity, targetVelocity));
                reactivity = Mathf.InverseLerp(0, movementParams.reactiveAccelerationAngle, reactivityAngle);
            }

            var velocityChangeSpeed = Mathf.Lerp(
                movementParams.normalAcceleration, 
                movementParams.reactiveAcceleration, 
                reactivity);
        
            velocity = Vector2.MoveTowards(velocity, targetVelocity, velocityChangeSpeed * Time.fixedDeltaTime);
        }

        private void UpdateAirVelocity(ref Vector2 velocity, Vector2 deltaVelocity)
        {
            var oldMagnitude = velocity.magnitude;
            velocity = Vector2.ClampMagnitude(velocity + deltaVelocity * Time.fixedDeltaTime, 
                Mathf.Max(oldMagnitude, movementParams.maxAirSpeed));
        }

        private Vector2 InputToGroundVelocity(Vector2 input) => new(
            input.x * movementParams.maxSpeed, 
            input.y * movementParams.maxSpeed);
        private Vector2 InputToAirAcceleration(Vector2 input) => input * movementParams.airAcceleration;

        // Методы оставлены чёб не переписывать, убрал трансформации
        private Vector3 LocalToWorldVelocity(Vector3 localVelocity) => localVelocity;
        private Vector3 WorldToLocalVelocity(Vector3 worldVelocity) => worldVelocity;
    }
}
