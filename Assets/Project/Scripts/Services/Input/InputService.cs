using Services.Input.Interfaces;

namespace Services.Input
{
    public class InputService : IService
    {
        public static IInputVariation CurrentInput;

        public void RegisterService()
        {
            // if (!Application.isEditor)
            // TODO: Сделать другую инпут систему
            
            CurrentInput = new DebugInputVariation();
        }
    }
}