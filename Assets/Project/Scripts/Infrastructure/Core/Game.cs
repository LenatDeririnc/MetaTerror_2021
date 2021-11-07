using Common.Components;
using Common.Components.Interfaces;
using Common.SceneManagement;
using Common.SceneManagement.ScriptableObjects;
using Infrastructure.StateMachine;
using Services;

namespace Infrastructure.Core
{
    public class Game
    {
        public Scene mainScene;
        public GameStateMachine stateMachine;
        public ServiceManager serviceManager;
        public LoadingCurtain loadingCurtain;
        public SceneProvider sceneProvider;

        public Game(ICoroutineRunner coroutineRunner, LoadingCurtain curtain)
        {
            loadingCurtain = curtain;
            loadingCurtain.SetCoroutineRunner(coroutineRunner);
            sceneProvider = new SceneProvider(coroutineRunner, loadingCurtain);
            serviceManager = new ServiceManager(this);
            stateMachine = new GameStateMachine(this);
        }
    }
}