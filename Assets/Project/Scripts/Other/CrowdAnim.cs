using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class CrowdAnim : MonoBehaviour
{
    public Animator animator;

    public float minAnimationTime = 5f;
    public float maxAnimationTime = 30f;

    private float animationTimer;

    private void Awake()
    {
        if (!animator)
        {
            animator = GetComponent<Animator>();
        } 
        
        animator.SetFloat("Offset", Random.value);
    }

    private void FixedUpdate()
    {
        animationTimer -= Time.deltaTime;

        if (animationTimer <= 0)
        {
            animator.SetInteger("Anim", Random.Range(0, 3));
            animationTimer = Mathf.Lerp(minAnimationTime, maxAnimationTime, Random.value);
        }
    }
}
