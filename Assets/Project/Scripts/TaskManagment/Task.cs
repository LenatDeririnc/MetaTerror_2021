using System;
using Player.Interfaces;
using UnityEngine;

namespace TaskManagment
{
    public class Task : MonoBehaviour, IInteractable
    {
        private TaskManager _manager;
        
        [SerializeField] private bool _isWorking = true;

        public bool IsWorking
        {
            get => _isWorking;
            set
            {
                _isWorking = value;
                TaskContainer.OnUpdateAction?.Invoke(this);
            }
        }
        
        public static Action OnFixAction;
        
        private void Start()
        {
            _manager = TaskManager.Instance;
            _manager.container.UpdateTask(this);
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