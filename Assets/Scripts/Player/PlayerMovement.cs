using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using Unity.VisualScripting;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class PlayerMovement : MonoBehaviour
{
    public float baseMoveSpeed = 2.5f;
    private float moveSpeed;
    public Rigidbody2D rb;
    public Animator animator;

    private Vector2 moveDirection;
    private Vector2 lastMoveDirection; // Store the last non-zero movement direction

    void Update()
    {
        ProcessInputs();
    }

    void FixedUpdate()
    {
        Move();
    }

    void ProcessInputs()
    {
        // Handle keyboard input
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        moveSpeed = Input.GetKey(KeyCode.LeftShift) ? baseMoveSpeed * 2.3f : baseMoveSpeed;


        // Combine keyboard movement
        moveDirection = new Vector2(moveX, moveY).normalized;

        // Update Animator parameters
        animator.SetFloat("Horizontal", moveDirection.x);
        animator.SetFloat("Vertical", moveDirection.y);
        animator.SetFloat("Speed", moveDirection.magnitude);

        // Update the last non-zero movement direction
        if (moveDirection != Vector2.zero)
        {
            lastMoveDirection = moveDirection;
        }
    }

    void Move()
    {
        // Move based on keyboard input
        rb.velocity = moveDirection * moveSpeed;

        // Set the walk animation direction based on the last non-zero movement direction
        if (lastMoveDirection != Vector2.zero)
        {
            animator.SetFloat("LastMoveDirectionX", lastMoveDirection.x);
            animator.SetFloat("LastMoveDirectionY", lastMoveDirection.y);
        }
    }
}