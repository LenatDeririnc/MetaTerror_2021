using Infrastructure.Core;
using Infrastructure.StateMachine.Interfaces;
using Services.Input;

namespace Infrastructure.StateMachine.States
{
    public class BootstrapState : IState
    {
        private readonly Game _game;

        public BootstrapState(Game game)
        {
            _game = game;
        }

        public void Enter()
        {
            _game.stateMachine.Enter<RegisterLevelServicesState>();
        }

        public void Exit() {}
    }
}