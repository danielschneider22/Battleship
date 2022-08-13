using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DelayStartAnimation : MonoBehaviour
{
    public float waitTime;
    private float waitTimer;

    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        
        if(waitTimer > waitTime && animator.enabled == false)
        {
            animator.enabled = true;
        } else if (animator.enabled == false)
        {
            waitTimer += Time.deltaTime;
        }
    }
}
