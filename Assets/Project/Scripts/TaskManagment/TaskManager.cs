using System;
using Common.Components;
using Common.Components.Interfaces;
using Common.Tools;
using Common.UI;
using UnityEngine;

namespace TaskManagment
{
    public class TaskManager : Singleton<TaskManager>, ICoroutineRunner
    {
        [SerializeField] private bool debugInfo = true;
        [SerializeField] private int gameScoreModifier = 10;
        public float timerIntervalInSeconds = 10;
        public TaskContainer container;
        
        private Timer _timer;
        private int gameScore = 0;

        protected override void BeforeRegister()
        {
            SetSettings(true, true);
        }

        protected override void AfterRegister()
        {
            container = new TaskContainer();
            _timer = new Timer(this, timerIntervalInSeconds, OnTimerEnd, debugInfo: debugInfo, name: name);
            
            Task.OnFixAction += OnFix;
        }

        private void OnDestroy()
        {
            Task.OnFixAction -= OnFix;
        }

        private void Start()
        {
            _timer.Play();
        }

        public void OnTimerEnd()
        {
            _timer.Restart();
            BreakRandomTask();
        }

        public void BreakRandomTask()
        {
            container.BreakRandom();
        }

        public void OnFix()
        {
            gameScore += gameScoreModifier * container.workingTasks.Count;
            ScoreSetter.UpdateScoreAction?.Invoke(gameScore);
        }

    }
}