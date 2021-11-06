using System;
using System.Collections;
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
        [SerializeField] private bool debugInfo = false;
        [SerializeField] private float timerInSeconds = 6 * 60;
        [SerializeField] private Scene scene;
        [SerializeField] private PlayableDirector _director;
        private Timer _endingTimer;

        protected override void BeforeRegister()
        {
            SetSettings(true, true);
        }

        protected override void AfterRegister()
        {
            _endingTimer = new Timer(this, timerInSeconds, OnTimerEnd, debugInfo: debugInfo, name: $"{name}_endingTimer");
        }

        private void Start()
        {
            _endingTimer.Play();
        }

        private void OnTimerEnd()
        {
            print("end!");
            _director.Play();
        }
        
        public void HideGameView()
        {
            MainCanvas.Instance?.panel.SetActive(true);
        }
        
        public void EndGame()
        {
            SceneService.sceneProvider.Load(scene);
            CursorLocker.SetLocked(false);
        }
    }
}