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

    private float currentStamina;
    public float maxStamina = 5;
    public float staminaRegenRate = .5f;
    private bool isRunning = false;

    public Rigidbody2D rb;
    public Animator animator;

    private Vector2 moveDirection;
    private Vector2 lastMoveDirection; // Store the last non-zero movement direction


    void Start()
    {
        currentStamina = maxStamina;
    }

    void Update()
    {
        ProcessInputs();
    }

    void FixedUpdate()
    {
        Move();
        RegenerateStamina();

    }

    void ProcessInputs()
    {
        // Handle keyboard input
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        // Check if the "Shift" key is held down to adjust the movement speed
        if (Input.GetKey(KeyCode.LeftShift) && currentStamina > 0)
        {
            isRunning = true;
            currentStamina -= Time.deltaTime;  // Consume stamina while running
            Debug.Log(currentStamina);
        }
        else
        {
            isRunning = false;
        }

        // Adjust movement speed based on running state
        moveSpeed = isRunning ? baseMoveSpeed * 3f : baseMoveSpeed;


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

    void RegenerateStamina()
    {
        // Regenerate stamina over time if not running
        if (!isRunning && currentStamina < maxStamina)
        {
            currentStamina += staminaRegenRate * Time.deltaTime;

            // Clamp the stamina to the maximum value
            currentStamina = Mathf.Clamp(currentStamina, 0f, maxStamina);
        }
    }
}