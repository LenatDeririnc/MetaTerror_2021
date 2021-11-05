using Infrastructure.Core;
using Infrastructure.StateMachine.Interfaces;
using UnityEngine;

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
            _game.stateMachine.Enter<LoadLevelState>();
        }

        public void Exit() {}
    }
}