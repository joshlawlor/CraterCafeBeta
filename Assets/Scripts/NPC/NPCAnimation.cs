using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCAnimation : MonoBehaviour
{
    public Animator animator;

    private void Start()
    {
        // Get the Animator component
        animator = GetComponent<Animator>();
        
        if (animator == null)
        {
            Debug.LogError("Animator component not found on NPC.");
        }
    }

    public void UpdateAnimation(Vector3 targetPosition, float moveSpeed)
    {
        // Calculate move direction
        Vector3 moveDirection = (targetPosition - transform.position).normalized;
    Debug.Log("UpdateAnimation Ran");
        // Set animator parameters
        if (animator != null)
        {
            animator.SetFloat("Horizontal", moveDirection.x);
            animator.SetFloat("Vertical", moveDirection.y);
            animator.SetFloat("Speed", moveSpeed);
        }
    }
}
