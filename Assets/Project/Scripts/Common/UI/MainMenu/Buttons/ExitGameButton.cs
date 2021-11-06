using UnityEngine;

namespace Common.UI.MainMenu.Buttons
{
    public class ExitGameButton : MonoBehaviour
    {
        public void Exit()
        {
            Application.Quit();
        }
    }
}