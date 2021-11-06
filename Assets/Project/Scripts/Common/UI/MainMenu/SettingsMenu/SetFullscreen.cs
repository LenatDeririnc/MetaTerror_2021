using UnityEngine;
using UnityEngine.UI;

namespace Common.UI.MainMenu.SettingsMenu
{
    public class SetFullscreen : MonoBehaviour
    {
        private static string PlayerPrefsFullscreenKey = "isFullscreen";

        [SerializeField] private Toggle toggle;
    
        void Start()
        {
            if (toggle is null)
                return;

            if (!PlayerPrefs.HasKey(PlayerPrefsFullscreenKey))
                return;

            toggle.isOn = PlayerPrefs.GetString(PlayerPrefsFullscreenKey) == true.ToString();
        }

        public void Set(bool state)
        {
            Screen.fullScreen = state;

            if (state)
                Screen.fullScreenMode = FullScreenMode.FullScreenWindow;

            PlayerPrefs.SetString(PlayerPrefsFullscreenKey, state.ToString());
        }
    }
}