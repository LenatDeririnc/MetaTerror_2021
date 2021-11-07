using Services.Input.Interfaces;
using UnityEngine.InputSystem;

namespace Services.Input
{
    public class GlobalInput : IInputVariation
    {
        public PlayerControls Controls;

        public GlobalInput()
        {
            Controls = new PlayerControls();
            Controls.Enable();
        }
        
        public bool GetButtonDown()
        {
            var organizer = Controls.Organizer;
            return organizer.Interact.ReadValue<bool>();
        }
    }
}