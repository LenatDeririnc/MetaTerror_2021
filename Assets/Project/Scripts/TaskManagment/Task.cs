using System;
using UnityEngine;

namespace TaskManagment
{
    public class Task : MonoBehaviour
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
            _isWorking = true;
            OnUpdateAction(this);
        }
    }
}