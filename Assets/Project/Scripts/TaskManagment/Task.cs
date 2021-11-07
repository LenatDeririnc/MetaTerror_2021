using System;
using Minigames;
using Player;
using Player.Interfaces;
using UI;
using UnityEngine;

namespace TaskManagment
{
    public class Task : MonoBehaviour, IInteractable
    {
        [SerializeField] private Animator _animator;
        [SerializeField] private string FixAnimatorTrigger = "OnFix";
        [SerializeField] private string BreakAnimatorTrigger = "OnBreak";
        [SerializeField] private SpriteRenderer _sprite;
        [SerializeField] private MiniGameType _miniGameType;
        
        [SerializeField] private bool _isWorking = true;

        private TaskManager _manager;
        public Sprite Sprite => _sprite.sprite;

        public static Action<Task> ONBreakAction;
        public static Action<Task> ONFixAction;
        public static Action<Task> ONUpdateStateAction;
        public static Action<Task> ONSpriteUpdate;

        public bool IsWorking
        {
            get => _isWorking;
            set
            {
                _isWorking = value;
                _sprite.enabled = !_isWorking;
                TaskContainer.OnUpdateAction?.Invoke(this);
            }
        }

        protected virtual void Start()
        {
            _manager = TaskManager.Instance;
            _manager.container.UpdateTask(this);
            BreaksUICount.Instance.AppendIcon(this);
            ONSpriteUpdate += BreaksUICount.Instance.UpdateIcon;
            UpdateSprite();
        }

        private void UpdateSprite()
        {
            _sprite.enabled = !IsWorking;
            ONSpriteUpdate?.Invoke(this);
        }

        
        public virtual void Break()
        {
            IsWorking = false;
            UpdateSprite();
            ONBreakAction?.Invoke(this);
            ONUpdateStateAction?.Invoke(this);
            
            if (_animator != null)
                _animator.SetTrigger(BreakAnimatorTrigger);
        }
        
        
        public virtual void Fix()
        {
            if (IsWorking)
                return;
            
            print($"{name} fixed");

            IsWorking = true;
            UpdateSprite();
            ONFixAction?.Invoke(this);
            ONUpdateStateAction?.Invoke(this);
            
            if (_animator != null)
                _animator.SetTrigger(FixAnimatorTrigger);
        }

        public void OnInteract(Organizer organizer)
        {
            if(IsWorking)
                return;
            
            organizer.IsControllable = false;
            MiniGameManager.Instance.StartNewGame(_miniGameType, result =>
            {
                Fix();
                organizer.IsControllable = true;
            });
        }
    }
}