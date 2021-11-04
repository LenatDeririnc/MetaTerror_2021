using System;
using System.Collections;
using UnityEngine;

namespace Common.Components
{
    public class LoadingCurtain : MonoBehaviour
    {
        public CanvasGroup canvasGroup;
        public float fadeSpeed = 0.03f;

        public void Show(Action callback) =>
            StartCoroutine(FadeOut(callback));

        public void Hide() =>
            StartCoroutine(FadeIn());

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