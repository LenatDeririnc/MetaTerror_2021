using Infrastructure.Core;
using Infrastructure.StateMachine.Interfaces;

namespace Infrastructure.StateMachine.States
{
    public class GameLoopState : IState
    {
        private readonly Game _game;
        private readonly GameStateMachine _stateMachine;
        
        public GameLoopState(Game game)
        {
            _game = game;
            _stateMachine = game.stateMachine;
        }

        public void Exit()
        { }

        public void Enter()
        {
            _game.loadingCurtain.Hide();
        }
    }
}