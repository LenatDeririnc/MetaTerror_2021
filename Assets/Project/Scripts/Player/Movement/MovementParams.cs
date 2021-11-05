using System;

namespace Player.Movement
{
    [Serializable]
    public struct MovementParams
    {
        public float normalAcceleration;
        public float reactiveAcceleration;
        public float reactiveAccelerationAngle;
        public float airAcceleration;

        public float jumpVelocity;
        public float jumpControlDuration;
    
        public float maxSpeed;
        public float maxAirSpeed;

        public static MovementParams GetDefault()
        {
            return new MovementParams
            {
                normalAcceleration = 10f,
                reactiveAcceleration = 25f,
                reactiveAccelerationAngle = 0.25f,
                airAcceleration = 2.5f,

                jumpVelocity = 4f,
                jumpControlDuration = 0.25f,
            
                maxSpeed = 1f,
                maxAirSpeed = 5f
            };
        }
    }
}