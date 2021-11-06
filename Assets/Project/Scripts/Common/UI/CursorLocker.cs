using Common.Components;
using UnityEngine;

namespace Common.UI
{
    public class CursorLocker : Singleton<CursorLocker>
    {
        [SerializeField] private bool isLocked = true;

        protected override void BeforeRegister()
        {
            SetSettings(false, true);
        }

        private void Start()
        {
            SetLocked(isLocked);
        }

        public static void SetLocked(bool locked)
        {
            if (Instance == null)
                return;

            Instance.isLocked = locked;
            Cursor.lockState = locked ? CursorLockMode.Locked : CursorLockMode.None;
            Cursor.visible = !locked;
        }
    }
}