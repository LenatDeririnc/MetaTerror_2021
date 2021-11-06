using System;
using Minigames;
using Player;
using Player.Interfaces;
using UnityEngine;

namespace TaskManagment
{
    public class Task : MonoBehaviour, IInteractable
    {
        [SerializeField] private SpriteRenderer _sprite;
        [SerializeField] private MiniGameType _miniGameType;
        
        [SerializeField] private bool _isWorking = true;

        private TaskManager _manager;

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
        
        public static Action OnFixAction;
        
        protected virtual void Start()
        {
            _manager = TaskManager.Instance;
            _manager.container.UpdateTask(this);
            UpdateSprite();
        }

        private void UpdateSprite()
        {
            _sprite.enabled = !IsWorking;
        }

        
        public virtual void Break()
        {
            IsWorking = false;
            UpdateSprite();
        }
        
        
        public virtual void Fix()
        {
            if (IsWorking)
                return;
            
            print($"{name} fixed");

            IsWorking = true;
            UpdateSprite();
            OnFixAction?.Invoke();
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