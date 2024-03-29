using UnityEngine;
using UnityEngine.UI;

using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class PlayerMovement : MonoBehaviour
{
    public float baseMoveSpeed = 2.5f;
    private float moveSpeed;
    private float currentStamina;
    public float maxStamina = 5;
    public float staminaRegenRate = 1.35f;
    public float runCost = 1;
    private bool isRunning = false;

    public float StaminaPercentage
    {
        get { return currentStamina / maxStamina; }
    }


    public Rigidbody2D rb;
    public Animator animator;
    public Image StaminaBar;

    private Vector2 moveDirection;
    private Vector2 lastMoveDirection; // Store the last non-zero movement direction


    void Start()
    {
        currentStamina = maxStamina;
    }

    void Update()
    {
        ProcessInputs();
        if (currentStamina == maxStamina)
        {
            StaminaBar.gameObject.SetActive(false);

        }
        if (currentStamina < 2.5 && currentStamina > 1.5)
        {
            StaminaBar.color = Color.yellow;

        }
        else if (currentStamina < 1.5)
        {
            StaminaBar.color = Color.red;

        }
        else
        {
            StaminaBar.color = Color.green;
        }

    }

    void FixedUpdate()
    {
        Move();
        RegenerateStamina();
        UpdateStaminaBar();

    }

    void ProcessInputs()
    {
        // Handle keyboard input
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        // Check if the "Shift" key is held down to adjust the movement speed
        if (Input.GetKey(KeyCode.LeftShift) && currentStamina > 0 && (moveDirection.x != 0 || moveDirection.y != 0))
        {
            isRunning = true;
            StaminaBar.gameObject.SetActive(true);
            currentStamina -= runCost * Time.deltaTime;  // Consume stamina while running
            StaminaBar.fillAmount = currentStamina / maxStamina; // Update stamina bar image width

        }
        else
        {
            isRunning = false;
        }

        // Adjust movement speed based on running state
        moveSpeed = isRunning ? baseMoveSpeed * 1.8f : baseMoveSpeed;


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

    void UpdateStaminaBar()
    {
        // Update the mask's size or position based on the stamina percentage
        float maskWidth = Mathf.Clamp01(currentStamina / maxStamina);
        StaminaBar.rectTransform.localScale = new Vector3(maskWidth, 1f, 1f);
    }

}