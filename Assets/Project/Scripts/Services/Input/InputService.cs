using Services.Input.Interfaces;

namespace Services.Input
{
    public class InputService : IService
    {
        public static GlobalInput CurrentInput;

        public void RegisterService()
        {
            CurrentInput = new GlobalInput();
        }
    }
}