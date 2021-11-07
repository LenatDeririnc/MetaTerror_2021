using System;
using System.Collections.Generic;
using Infrastructure.Core;
using Infrastructure.StateMachine.Interfaces;
using Infrastructure.StateMachine.States;

namespace Infrastructure.StateMachine
{
    public class GameStateMachine
    {
        private readonly Dictionary<Type, IExitableState> _states;
        private IExitableState _activeState;

        public GameStateMachine(Game game)
        {
            _states = new Dictionary<Type, IExitableState>()
            {
                [typeof(BootstrapState)] = new BootstrapState(game),
                [typeof(GameLoopState)] = new GameLoopState(game),
                [typeof(RegisterLevelServicesState)] = new RegisterLevelServicesState(game),
            };
        }

        private TState State<TState>() where TState : class, IExitableState => 
            _states[typeof(TState)] as TState;

        private TState ChangeState<TState>() where TState : class, IExitableState
        {
            _activeState?.Exit();
            
            TState state = State<TState>();
            _activeState = state;
            
            return state;
        }

        public void Enter<TState>() where TState : class, IState
        {
            TState state = ChangeState<TState>();
            state.Enter();
        }

        public void Enter<TState, TPayload>(TPayload payload) where TState : class, IPayloadedState<TPayload>
        {
            TState state = ChangeState<TState>();
            state.Enter(payload);
        }
    }
}