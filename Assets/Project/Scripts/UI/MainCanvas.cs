using Common.Components;
using UnityEngine;

namespace UI
{
    public class MainCanvas : Singleton<MainCanvas>
    {
        public GameObject panel;
        public ScoreSetterWithColor scorePanel;

        protected override void BeforeRegister()
        {
            SetSettings(true, true);
        }
    }
}