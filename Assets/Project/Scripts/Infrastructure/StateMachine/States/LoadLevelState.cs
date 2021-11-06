﻿using Common.SceneManagement.ScriptableObjects;
using Infrastructure.Core;
using Infrastructure.StateMachine.Interfaces;

namespace Infrastructure.StateMachine.States
{
    public class LoadLevelState : IPayloadedState<Scene>, IState
    {
        private readonly Game _game;

        public LoadLevelState(Game game)
        {
            _game = game;
        }

        public void Exit()
        { }

        public void Enter(Scene scene) => 
            _game.sceneProvider.Load(scene, OnLoaded);

        public void Enter() => 
            _game.sceneProvider.Load(_game.mainScene, OnLoaded);

        public void OnLoaded() => 
            _game.stateMachine.Enter<RegisterLevelServicesState>();
    }
}