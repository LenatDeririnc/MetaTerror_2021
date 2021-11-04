using Common.Components;
using Common.Components.Interfaces;
using Common.SceneManagement.ScriptableObjects;
using Infrastructure.Core;
using Infrastructure.StateMachine.States;

namespace Infrastructure.Components
{
    public class GameBootstrapper : Singleton<GameBootstrapper>, ICoroutineRunner
    {
        public Scene mainScene;
        public LoadingCurtain loadingCurtain;
        private Game _game;

        protected override void BeforeRegister() =>
            SetSettings(true);

        protected override void AfterRegister()
        {
            _game = new Game(this, loadingCurtain, mainScene);
            _game.stateMachine.Enter<BootstrapState>();
        }
    }
}
