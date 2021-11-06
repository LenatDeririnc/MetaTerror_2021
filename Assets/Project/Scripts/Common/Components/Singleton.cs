using UnityEngine;

namespace Common.Components
{
    public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        protected static int InstanceCount = 0;
        protected bool IsDestroyWithGameObject = false;
        protected bool ClearInstanceOnLoadScene = false;

        private static T _instance;
        public static T Instance => _instance;

        private Object _hideObject;

        protected virtual void BeforeRegister() {}

        protected void SetSettings(bool isDestroyWhtiGameObject = false, bool clearInstanceOnLoadScene = false)
        {
            IsDestroyWithGameObject = isDestroyWhtiGameObject;
            ClearInstanceOnLoadScene = clearInstanceOnLoadScene;
        }

        private void Init()
        {
            BeforeRegister();

            DebugContainers.DebugCheck.InfiniteCycle(ref InstanceCount, 100);

            _hideObject = IsDestroyWithGameObject ? gameObject : this;
            
            if (_instance != null)
            {
                Destroy(_hideObject);
                return;
            }

            _instance = this as T;

            if (!ClearInstanceOnLoadScene)
                DontDestroyYourself();

            AfterRegister();
        }

        private void DontDestroyYourself()
        {
            transform.parent = null;
            DontDestroyOnLoad(this);
        }

        protected void Awake()
        {
            Init();
        }
        
        protected virtual void AfterRegister() {}
    }
}