using Infrastructure.Core;
using Infrastructure.StateMachine.Interfaces;
using Services.Input;
using Services.SceneLoad;

namespace Infrastructure.StateMachine.States
{
    public class RegisterLevelServicesState : IState
    {
        private Game _game;

        public RegisterLevelServicesState(Game game)
        {
            _game = game;
        }

        public void Exit()
        { }

        public void Enter()
        {
            _game.serviceManager.RegisterService<SceneService>();
            _game.stateMachine.Enter<GameLoopState>();
        }
    }
}