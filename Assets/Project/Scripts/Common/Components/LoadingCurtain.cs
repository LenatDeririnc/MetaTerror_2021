using System;
using System.Collections;
using Common.Components.Interfaces;
using UnityEngine;

namespace Common.Components
{
    public class LoadingCurtain : MonoBehaviour, ICoroutineRunner
    {
        private ICoroutineRunner _coroutineRunner; 
        public CanvasGroup canvasGroup;
        public float fadeSpeed = 0.03f;

        private void Awake()
        {
            _coroutineRunner ??= this;
        }

        public void SetCoroutineRunner(ICoroutineRunner runner)
        {
            _coroutineRunner = runner;
        }

        public void Show(Action callback) =>
            _coroutineRunner.StartCoroutine(FadeOut(callback));

        public void Hide() =>
            _coroutineRunner.StartCoroutine(FadeIn());

        private IEnumerator FadeOut(Action callback)
        {
            gameObject.SetActive(true);
            
            while (canvasGroup.alpha < 1)
            {
                canvasGroup.alpha += fadeSpeed;
                yield return new WaitForSeconds(fadeSpeed);
            }

            callback();
        }

        private IEnumerator FadeIn()
        {
            while (canvasGroup.alpha > 0)
            {
                canvasGroup.alpha -= fadeSpeed;
                yield return new WaitForSeconds(fadeSpeed);
            }
            
            gameObject.SetActive(false);
        }
    }
}