using System;
using System.Collections.Generic;
using Common.AssetManagement;
using Common.Components;
using TaskManagment;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class BreaksUICount : Singleton<BreaksUICount>
    {
        [SerializeField] private GameObject iconSample;

        private Dictionary<Task, GameObject> _tasksUi;
        private AssetProvider _assetProvider;

        protected override void BeforeRegister()
        {
            SetSettings(false, true);
        }

        protected override void AfterRegister()
        {
            _assetProvider = new AssetProvider();
            _tasksUi = new Dictionary<Task, GameObject>();
        }

        public void AppendIcon(Task task)
        {
            if (iconSample == null)
                throw new Exception("no icon reference");

            Sprite sprite = task.Sprite;
            
            GameObject instanceObject = _assetProvider.Instantiate(iconSample, transform);
            Image image = instanceObject.GetComponent<Image>();

            if (image == null)
                throw new Exception("no image reference on icon");

            image.sprite = sprite;
            
            instanceObject.SetActive(false);
            _tasksUi[task] = instanceObject;
        }

        public void UpdateIcon(Task task)
        {
            GameObject spriteObject = _tasksUi[task];
            spriteObject.SetActive(!task.IsWorking);
        }
    }
}