using System.Collections.Generic;
using Common.Components;
using TaskManagment;
using UnityEngine;

namespace UI
{
    public class BreaksUICount : Singleton<BreaksUICount>
    {
        [SerializeField] private List<GameObject> breakUIs;

        protected override void BeforeRegister()
        {
            SetSettings(false, true);
        }

        public void UpdateCount()
        {
            foreach (var breakUI in breakUIs)
            {
                breakUI.SetActive(false);
            }

            var count = TaskManager.Instance.container.destroyedTasks.Count;

            for (int i = 0; i < count; ++i)
            {
                breakUIs[i].SetActive(true);
            }
        }
    }
}