using System;
using Common.Components;
using Common.Components.Interfaces;
using Common.Tools;

namespace TaskManagment
{
    public class TaskManager : Singleton<TaskManager>, ICoroutineRunner
    {
        public float timerIntervalInSeconds = 10;
        public int minDestroyCount = 1;
        public int maxDestroyCount = 1;
        public TaskContainer container;
        
        private Timer _timer;

        protected override void BeforeRegister()
        {
            SetSettings(true, true);
        }

        protected override void AfterRegister()
        {
            container = new TaskContainer();
            _timer = new Timer(this, timerIntervalInSeconds, OnTimerEnd, debugInfo: true, name: "TaskManagerTimer");
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

    }
}