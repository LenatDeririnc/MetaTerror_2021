using System;
using Player.Interfaces;
using UnityEngine;

namespace TaskManagment
{
    public class Task : MonoBehaviour, IInteractable
    {
        [SerializeField] private SpriteRenderer _sprite;
        
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
        
        private void Start()
        {
            _manager = TaskManager.Instance;
            _manager.container.UpdateTask(this);
            _sprite.enabled = !IsWorking;
        }

        public void Break()
        {
            IsWorking = false;
        }

        public void Fix()
        {
            if (IsWorking)
                return;
            
            print($"{name} fixed");

            IsWorking = true;
            OnFixAction?.Invoke();
        }

        public void OnInteract()
        {
            Fix();
        }
    }
}