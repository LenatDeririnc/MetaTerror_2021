using Common.SceneManagement.ScriptableObjects;
using Services.SceneLoad;
using UnityEngine;

namespace GameCore.UI.MainMenu
{
    public class StartGameButton : Common.UI.MainMenu.Buttons.StartGameButton
    {
        [SerializeField] private Scene scene;
        
        protected override void Start()
        {
            OnPressAction = () =>
            {
                SceneService.sceneProvider.Load(scene);
            };
        }
    }
}