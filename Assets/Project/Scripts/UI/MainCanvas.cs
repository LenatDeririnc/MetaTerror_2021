using Common.Components;
using Common.UI;
using UnityEngine;

namespace UI
{
    public class MainCanvas : Singleton<MainCanvas>
    {
        public GameObject blackScreen;
        public ScoreSetterWithColor scorePanel;
        public CanvasGroup endText;
        public CanvasGroup endScorePanel;

        protected override void BeforeRegister()
        {
            SetSettings(true, true);
        }
    }
}