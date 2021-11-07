using System;
using System.Collections.Generic;
using System.Linq;
using Common.Extensions;
using Services.Audio.Extensions;
using UI;
using UnityEngine;
using Random = UnityEngine.Random;

namespace TaskManagment
{
    public class TaskContainer
    {
        public HashSet<Task> workingTasks;
        public HashSet<Task> destroyedTasks;
        
        private AudioClip _breakSoundEffect;
        
        public static Action<Task> OnUpdateAction;

        public TaskContainer(AudioClip breakSoundEffect)
        {
            ClearSets();
            OnUpdateAction = UpdateTask;
            _breakSoundEffect = breakSoundEffect;
        }

        public void ClearSets()
        {
            workingTasks = new HashSet<Task>();
            destroyedTasks = new HashSet<Task>();
        }

        public void BreakRandomTask()
        {
            if (workingTasks.Count <= 0)
                return;
            
            MainCanvas.Instance.scorePanel.Blink();
            int random = Random.Range(0, workingTasks.Count);
            BreakTask(random);
        }

        public void BreakTask(int index)
        {
            var task = workingTasks.ElementAt(index);
            task.Break();
            _breakSoundEffect.PlayOnce(0);
            Debug.Log($"{task.name} has broken");
        }

        public void UpdateTask(Task task)
        {
            bool isWorking = task.IsWorking;

            HashSet<Task> hashSetToAdd;
            HashSet<Task> hashSetToRemove;

            if (isWorking)
            {
                hashSetToAdd = workingTasks;
                hashSetToRemove = destroyedTasks;
            }
            else
            {
                hashSetToAdd = destroyedTasks;
                hashSetToRemove = workingTasks;
            }

            hashSetToAdd.Add(task);
            hashSetToRemove.RemoveIfContains(task);
            
            Debug.Log($"works: \"{workingTasks.Count}\", broked: \"{destroyedTasks.Count}\"");
            
            BreaksUICount.Instance.UpdateCount();
        }
    }
}