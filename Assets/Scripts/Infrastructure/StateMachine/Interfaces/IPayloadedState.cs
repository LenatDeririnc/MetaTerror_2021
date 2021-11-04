namespace Infrastructure.StateMachine.Interfaces
{
    public interface IPayloadedState<TPayload> : IExitableState
    {
        void Enter(TPayload payload);
    }
}