using Infrastructure.Core;
using Infrastructure.StateMachine.Interfaces;

namespace Infrastructure.StateMachine.States
{
    public class GameLoopState : IState
    {
        private readonly GameStateMachine _stateMachine;
        
        public GameLoopState(Game game) => 
            _stateMachine = game.stateMachine;

        public void Exit()
        { }

        public void Enter()
        { }
    }
}