using Common.Components;
using Services.SceneLoad;
using UnityEngine;

namespace UI
{
    public class MainCanvas : Singleton<MainCanvas>
    {
        public GameObject panel;

        protected override void BeforeRegister()
        {
            SetSettings(true, true);
        }
    }
}