using System.Collections;
using UnityEngine;
using Pathfinding;

public class NPCExitScene : MonoBehaviour
{
    public AIPath aiPath;
    public Vector3 doorPoint;
    public Vector3 exitPoint;
    public Collider2D npcCollider;
    public Animator animator;

    private bool usePathfinding = true;

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

        // Start the exit coroutine
        StartCoroutine(ExitScene());
    }

    private IEnumerator ExitScene()
    {
        // Move towards the door point using pathfinding
        aiPath.destination = doorPoint;

        while ((transform.position - doorPoint).sqrMagnitude > 0.01f)
        {
            // Calculate move direction while using pathfinding
            Vector3 moveDirection = aiPath.velocity.normalized;

            // Set animator parameters
            animator.SetFloat("Horizontal", moveDirection.x);
            animator.SetFloat("Vertical", moveDirection.y);
            animator.SetFloat("Speed", aiPath.velocity.magnitude);

            yield return null;
        }

        // NPC has reached the door point, disable collider
        npcCollider.enabled = false;

        // Stop using pathfinding and move towards the exit point using transform
        usePathfinding = false;

        while ((transform.position - exitPoint).sqrMagnitude > 0.01f)
        {
            // Move towards the exit point using transform
            transform.position = Vector3.MoveTowards(transform.position, exitPoint, 3 * Time.deltaTime);

            // Calculate move direction
            Vector3 moveDirection = (exitPoint - transform.position).normalized;

            // Set animator parameters
            animator.SetFloat("Horizontal", moveDirection.x);
            animator.SetFloat("Vertical", moveDirection.y);
            animator.SetFloat("Speed", 3); // Set a constant speed

            yield return null;
        }

        // NPC has reached the exit point, destroy the NPC GameObject
        Destroy(gameObject);
    }

    private void FixedUpdate()
    {
        // Check if pathfinding should be used
        aiPath.enabled = usePathfinding;
    }
}
