using System;
using Common.SceneManagement;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Common.Components
{
    public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        protected static int InstanceCount = 0;
        protected bool IsDestroyWithGameObject = false;
        protected bool ClearInstanceOnLoadScene = false;

        private static T _instance;
        public static T Instance
        {
            get
            {
                _instance ??= FindObjectOfType<T>();
                if (_instance == null)
                    Debug.Log("Singleton<" + typeof(T) + "> instance has been not found.");
                return _instance;
            }
        }

        protected Action OnDestroyAction;

        protected virtual void BeforeRegister() {}

        protected void SetSettings(bool isDestroyWhtiGameObject = false, bool clearInstanceOnLoadScene = false)
        {
            IsDestroyWithGameObject = isDestroyWhtiGameObject;
            ClearInstanceOnLoadScene = clearInstanceOnLoadScene;
        }

        public static void OnLoadNewScene()
        {
            SceneProvider.OnLoadScene -= OnLoadNewScene;
            InstanceCount = 0;
        }

        protected void Init()
        {
            BeforeRegister();

            SceneProvider.OnLoadScene += OnLoadNewScene;
            InstanceCount += 1;

            if (InstanceCount == 1)
            {
                if (ClearInstanceOnLoadScene)
                    _instance = null;
                else
                {
                    transform.parent = null;
                    DontDestroyOnLoad(this);
                }
            }

            _instance ??= this as T;
            OnDestroyAction += () => DestroySelf(IsDestroyWithGameObject ? (Object) gameObject : this);
            
            if (_instance != this)
            {
                OnDestroyAction?.Invoke();
                return;
            }

            AfterRegister();
        }

        protected void Awake()
        {
            Init();
        }
        
        protected virtual void AfterRegister() {}

        private void DestroySelf(Object target)
        {
            OnDestroyAction -= () => DestroySelf(target);
            
            if (Application.isPlaying)
                Destroy(target);
            else
                DestroyImmediate(target);
        }
    }
}