using System;
using Player.Interfaces;
using UnityEngine;

namespace TaskManagment
{
    public class Task : MonoBehaviour, IInteractable
    {
        private TaskManager _manager;
        
        [SerializeField] private bool _isWorking = true;
        public bool IsWorking => _isWorking;

        public static Action<Task> OnUpdateAction;
        
        private void Start()
        {
            _manager = TaskManager.Instance;
            OnUpdateAction += _manager.container.UpdateTask;
            OnUpdateAction(this);
        }

        public void Break()
        {
            _isWorking = false;
            OnUpdateAction(this);
        }

        public void Fix()
        {
            if (_isWorking)
                return;
            
            print($"{name} fixed");
            
            _isWorking = true;
            OnUpdateAction(this);
        }

        public void OnInteract()
        {
            Fix();
        }
    }
}