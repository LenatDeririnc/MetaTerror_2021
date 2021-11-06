using System;
using UnityEngine;

public class DrumAiPlayer : MonoBehaviour
{
    public Animator animator;

    public HandRig leftHand;
    public HandRig rightHand;

    public Transform leftStickIdle;
    public Transform rightStickIdle;

    public void UpdatePosition()
    {
        
    }
    
    [Serializable]
    public struct HandRig
    {
        public Transform handRoot;
        public DrumStick stick;
        public AvatarIKGoal goal;
    }
}
