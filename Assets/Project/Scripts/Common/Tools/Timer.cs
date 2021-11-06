using System;
using System.Collections;
using Common.Components.Interfaces;
using UnityEngine;

namespace Common.Tools
{
    public class Timer
    {
        public string name { get; }
        private float _currentTime;
        private readonly ICoroutineRunner _runner;
        private Coroutine _timerCoroutine;
        private readonly Action _callbackAction;
        private readonly bool _debugInfo;
        private float _startSeconds;

        private bool _isEnabled = true;
        
        public Timer(ICoroutineRunner runner, float startSeconds, Action callbackAction, bool debugInfo = false, string name = "Timer")
        {
            this.name = name;
            _runner = runner;
            _callbackAction = callbackAction;
            _debugInfo = debugInfo;
            _currentTime = startSeconds;
            _startSeconds = startSeconds;
        }

        public void Play()
        {
            _isEnabled = true;
            _timerCoroutine = _runner.StartCoroutine(TimerIEnumerator());
        }

        public void SetStartSeconds(float seconds)
        {
            _startSeconds = seconds;
        }
        
        public void Restart()
        {
            _currentTime = _startSeconds;
            Play();
        }

        public void Stop()
        {
            _runner.StopCoroutine(_timerCoroutine);
            _isEnabled = false;
        }
        
        public IEnumerator TimerIEnumerator()
        {
            while (_isEnabled && _currentTime > 0)
            {
                _currentTime -= Time.deltaTime;
                if (_debugInfo)
                    Debug.Log(ToString());
                yield return null;
            }
            
            if (_isEnabled)
                _callbackAction();
        }

        public override string ToString()
        {
            return $"{name}: {_currentTime}";
        }
    }
}