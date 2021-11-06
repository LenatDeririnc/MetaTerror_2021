using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Common.UI.MainMenu.Buttons
{
    public class StartGameButton : MonoBehaviour
    {
        private string SceneName;
        public Action OnPressAction;

        protected virtual void Start()
        {
            OnPressAction = () => { SceneManager.LoadSceneAsync(SceneName); };
        }

        public void OnPressButton()
        {
            OnPressAction();
        }
    }
}