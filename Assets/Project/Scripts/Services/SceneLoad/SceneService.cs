using Common.SceneManagement;
using Infrastructure.Core;

namespace Services.SceneLoad
{
    public class SceneService : IService
    {
        public static SceneProvider sceneProvider;
        private readonly Game _game;

        public SceneService(Game game)
        {
            _game = game;
        }

        public void RegisterService()
        {
            sceneProvider = _game.sceneProvider;
        }
    }
}