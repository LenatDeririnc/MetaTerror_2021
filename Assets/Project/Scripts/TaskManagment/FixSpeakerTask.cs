using System;
using DG.Tweening;
using UnityEngine;

namespace TaskManagment
{
    public class FixSpeakerTask : Task
    {
        public ParticleSystem sparkParticles;
        
        public Rigidbody leftConnectorRigidBody;
        public Rigidbody rightConnectorRigidBody;

        public Transform leftTargetPosition;
        public Transform rightTargetPosition;

        protected void Awake()
        {
            if (IsWorking)
            {
                Fix();
            }
            else
            {
                Break();
            }
        }

        private void Spark()
        {
            if (sparkParticles)
                sparkParticles.Play();
        }
        
        [ContextMenu("Break")]
        public override void Break()
        {
            base.Break();

            leftConnectorRigidBody.isKinematic = false;
            rightConnectorRigidBody.isKinematic = false;
            leftConnectorRigidBody.WakeUp();
            rightConnectorRigidBody.WakeUp();

            Spark();
        }

        [ContextMenu("Fix")]
        public override void Fix()
        {
            base.Fix();
            
            leftConnectorRigidBody.isKinematic = true;
            rightConnectorRigidBody.isKinematic = true;

            InsertJack(leftConnectorRigidBody, leftTargetPosition);
            InsertJack(rightConnectorRigidBody, rightTargetPosition);
            DOTween.Sequence().InsertCallback(0.5f, Spark);
        }

        private void InsertJack(Rigidbody r, Transform target)
        {
            var hoverPosition = target.position - target.right * 0.5f;

            r.isKinematic = true;
            r.transform.DORotateQuaternion(target.rotation, 0.5f)
                .SetTarget(this);

            DOTween.Sequence()
                .Append(r.transform.DOMove(hoverPosition, 0.5f))
                .Append(r.transform.DOMove(target.position, 0.2f))
                .SetTarget(this);
        }
    }
}