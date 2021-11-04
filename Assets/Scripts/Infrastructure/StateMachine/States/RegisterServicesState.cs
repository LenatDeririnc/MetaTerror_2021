using Infrastructure.Core;
using Infrastructure.StateMachine.Interfaces;
using Services;
using Services.Input;
using Services.SceneLoad;

namespace Infrastructure.StateMachine.States
{
    public class RegisterServicesState : IState
    {
        private Game _game;

        public RegisterServicesState(Game game)
        {
            _game = game;
        }

        public void Exit()
        { }

        public void Enter()
        {
            _game.serviceManager.RegisterService<InputService>();
            _game.serviceManager.RegisterService<SceneService>();
            _game.stateMachine.Enter<GameLoopState>();
        }
    }
}