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
        public float breakTimeIntervalInSeconds = 10;
        public float scoreTimeIntervalInSeconds = 1;
        public TaskContainer container;
        
        private Timer _breakTimer;
        private Timer _scoreTimer;
        private int gameScore = 0;

        protected override void BeforeRegister()
        {
            SetSettings(true, true);
        }

        protected override void AfterRegister()
        {
            container = new TaskContainer();
            _breakTimer = new Timer(this, breakTimeIntervalInSeconds, OnBreakTimerEnd, debugInfo: debugInfo, name: $"{name}_breakTimer");
            _scoreTimer = new Timer(this, scoreTimeIntervalInSeconds, OnScoreTimerEnd, debugInfo, $"{name}_scoreTimer");
            
            Task.OnFixAction += OnFixAnyTask;
        }

        private void OnDestroy()
        {
            Task.OnFixAction -= OnFixAnyTask;
        }

        private void Start()
        {
            _breakTimer.Play();
            _scoreTimer.Play();
        }

        public void BreakRandomTask()
        {
            container.BreakRandomTask();
        }

        public void OnFixAnyTask()
        { }

        public void OnBreakTimerEnd()
        {
            BreakRandomTask();
            _breakTimer.Restart();
        }

        public void OnScoreTimerEnd()
        {
            IncreaseScore();
            _scoreTimer.Restart();
        }

        private void IncreaseScore()
        {
            gameScore += gameScoreModifier * container.workingTasks.Count;
            ScoreSetter.UpdateScoreAction?.Invoke(gameScore);
        }
    }
}