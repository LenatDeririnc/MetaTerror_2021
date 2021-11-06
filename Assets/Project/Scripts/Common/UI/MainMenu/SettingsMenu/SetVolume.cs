using UnityEngine;
using UnityEngine.UI;

namespace Common.UI.MainMenu.SettingsMenu
{
    public class SetVolume : MonoBehaviour
    {
        private static string PlayerPrefsVolumeKey = "GameVolume";

        [SerializeField] private Slider Slider;

        private void Start()
        {
            if (Slider == null)
                return;

            if (!PlayerPrefs.HasKey(PlayerPrefsVolumeKey)) return;

            var currentSliderValue = PlayerPrefs.GetFloat(PlayerPrefsVolumeKey);
            Slider.SetValueWithoutNotify(currentSliderValue);
        }

        public void Set(float volume)
        {
            AudioListener.volume = volume;
            PlayerPrefs.SetFloat(PlayerPrefsVolumeKey, volume);
        }
    }
}