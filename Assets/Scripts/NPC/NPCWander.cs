using UnityEngine;
using Pathfinding;
using System.Collections; // Add this line for IEnumerator

public class NPCWander : MonoBehaviour
{
    public AIPath aiPath;
    public float wanderRadius = 10f;
    public float wanderInterval = 5f;

    public Animator animator;

    private void Start()
    {
        // Get the Animator component
        animator = GetComponent<Animator>();

        // Get the AIPath component
        aiPath = GetComponent<AIPath>();

        if (aiPath == null)
        {
            Debug.LogError("AIPath component not found on NPC.");
            return;
        }

        // Start the wandering coroutine
        StartCoroutine(Wander());
    }

    private IEnumerator Wander()
    {
        while (true)
        {
            // Ensure aiPath is not null before accessing its properties
            if (aiPath != null)
            {
                // Generate a random point within the wanderRadius
                Vector2 randomDirection = Random.insideUnitCircle.normalized * wanderRadius;
                Vector3 targetPosition = transform.position + new Vector3(randomDirection.x, randomDirection.y, 0f);

                // Set the AIPath's destination to the random point
                aiPath.destination = targetPosition;
            }

            // Wait for the specified interval before generating a new destination
            yield return new WaitForSeconds(wanderInterval);
        }
    }

    private void FixedUpdate()
    {
        // Ensure aiPath is not null before accessing its properties
        if (aiPath != null)
        {
            // Calculate move direction
            Vector3 moveDirection = (aiPath.destination - transform.position).normalized;

            // Set animator parameters
            animator.SetFloat("Horizontal", moveDirection.x);
            animator.SetFloat("Vertical", moveDirection.y);
            animator.SetFloat("Speed", aiPath.velocity.magnitude);
        }
    }
}