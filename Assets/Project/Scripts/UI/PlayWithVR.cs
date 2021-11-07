using GameCore.UI.MainMenu;
using Infrastructure.Components;

namespace UI
{
    public class PlayWithVR : StartGameButton
    {
        protected override void Start()
        {
            base.Start();
            OnPressAction += () => { GameBootstrapper.Instance.VR_mode = true; };
        }
    }
}