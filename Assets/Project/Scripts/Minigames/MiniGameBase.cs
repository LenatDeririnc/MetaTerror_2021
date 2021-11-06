using System;
using DG.Tweening;
using UnityEngine;

namespace Minigames
{
    public abstract class MiniGameBase : MonoBehaviour, IMiniGame
    {
        public CanvasGroup group;
        
        public bool IsGameRunning { get; private set; }
        
        private Action<MiniGameResult> onCompleteListener;

        protected virtual void Awake()
        {
            group.alpha = 0f;
            group.interactable = false;
        }

        public void StartNewGame(Action<MiniGameResult> onCompleteListener)
        {
            group.DOFade(1f, 0.5f);
            group.interactable = true;
            
            IsGameRunning = true;
            this.onCompleteListener = onCompleteListener;
            OnGameStarted();
        }

        public void AbortGame()
        {
            FinishGame(MiniGameResult.Abort);
        }

        protected void FinishGame(MiniGameResult result, float delay = 0.25f)
        {
            if(!IsGameRunning)
                return;
            
            group.interactable = false;
            IsGameRunning = false;

            DOTween.Sequence()
                .AppendInterval(delay)
                .AppendCallback(() =>
                {
                    onCompleteListener?.Invoke(result);
                    onCompleteListener = null;
                    OnGameFinished();
                })
                .Append(group.DOFade(0f, 0.5f));
        }
        
        protected abstract void OnGameFinished();
        protected abstract void OnGameStarted();
    }
}