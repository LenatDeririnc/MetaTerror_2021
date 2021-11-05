using Common.SceneManagement;
using Infrastructure.Core;

namespace Services.SceneLoad
{
    public class SceneService : IService
    {
        private readonly SceneProvider _sceneProvider;
        private readonly Game _game;

        public SceneService(Game game)
        {
            _game = game;
            _sceneProvider = game.sceneProvider;
        }

        public void RegisterService()
        {
            _sceneProvider.Load(_game.mainScene);
        }
    }
}