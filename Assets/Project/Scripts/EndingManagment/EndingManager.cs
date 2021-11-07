using Common.Components;
using Common.Components.Interfaces;
using Common.SceneManagement.ScriptableObjects;
using Common.Tools;
using Common.UI;
using Services.SceneLoad;
using UI;
using UnityEngine;
using UnityEngine.Playables;

namespace EndingManagment
{
    public class EndingManager : Singleton<EndingManager>, ICoroutineRunner
    {
        [SerializeField] private bool startOnLoadScene = false;
        [SerializeField] private bool debugInfo = false;
        [SerializeField] private float timerInSeconds = 6 * 60;
        [SerializeField] private Scene scene;
        [SerializeField] private PlayableDirector _director;

        protected override void BeforeRegister()
        {
            SetSettings(true, true);
        }

        protected override void AfterRegister()
        {
            
        }

        private void Start()
        {
            if (startOnLoadScene)
            {
                RhythmGamePlayer.Instance.AddCallback(timerInSeconds, OnTimerEnd);
            }
        }
        
        public void StartEnd()
        {
            _director.Play();
        }

        private void OnTimerEnd()
        {
            print("end!");
            StartEnd();
        }
        
        public void HideGameView()
        {
            MainCanvas.Instance?.blackScreen.SetActive(true);
        }
        
        public void EndGame()
        {
            SceneService.sceneProvider.Load(scene);
            CursorLocker.SetLocked(false);
        }
    }
}