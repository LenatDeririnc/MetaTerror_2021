using Common.Components;
using Common.Components.Interfaces;
using Common.SceneManagement.ScriptableObjects;
using Infrastructure.Core;
using Infrastructure.StateMachine.States;

namespace Infrastructure.Components
{
    public class GameBootstrapper : Singleton<GameBootstrapper>, ICoroutineRunner
    {
        public LoadingCurtain loadingCurtain;
        private Game _game;
        public bool VR_mode = false;

        protected override void BeforeRegister() =>
            SetSettings(true);

        protected override void AfterRegister()
        {
            _game = new Game(this, loadingCurtain);
            _game.stateMachine.Enter<BootstrapState>();
        }
    }
}
