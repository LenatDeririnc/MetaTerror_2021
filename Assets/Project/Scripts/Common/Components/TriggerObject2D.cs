using UnityEngine;
using UnityEngine.Events;

namespace Common.Components
{
    public class TriggerObject2D : MonoBehaviour
    {
        [SerializeField] bool HideMesh = true;

        [SerializeField] private LayerMask m_objectMask = 1;

        private void Start()
        {
            if (HideMesh)
                HideRenderMesh();
        }

        public UnityEvent<Collider2D> OnEnter;
        public UnityEvent<Collider2D> OnStay;
        public UnityEvent<Collider2D> OnExit;

        private void TriggerAction(Collider2D other, UnityEvent<Collider2D> @event)
        {
            if (@event == null)
                return;

            if (((1 << other.gameObject.layer) & m_objectMask) != 0)
                @event.Invoke(other);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            TriggerAction(other, OnEnter);
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            TriggerAction(other, OnExit);
        }

        private void OnTriggerStay2D(Collider2D other)
        {
            TriggerAction(other, OnStay);
        }

        private void HideRenderMesh()
        {
            var mesh = GetComponent<MeshRenderer>();
            
            if (mesh == null)
                return;
            
            mesh.enabled = false;
        }

    }
}