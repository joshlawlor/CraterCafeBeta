using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCMovement : MonoBehaviour
{
    // Array of waypoints to walk from one to the next one
    [SerializeField]
    private Vector3[] waypoints;

    // Walk speed that can be set in Inspector
    [SerializeField]
    private float moveSpeed = 2f;
    public Animator animator;

    // Index of current waypoint from which Enemy walks
    // to the next one
    private int waypointIndex = 0;
    private NPCInteract npcInteract;


    // Use this for initialization
    private void Start()
    {

        // Set position of Enemy as position of the first waypoint
        transform.position = waypoints[waypointIndex];
        npcInteract = GetComponent<NPCInteract>();

    }

    // Update is called once per frame
    private void FixedUpdate()
    {

        // Move Enemy
        Move();

    }

    // Method that actually make Enemy walk
    private void Move()
    {
        // If Enemy didn't reach last waypoint it can move
        // If enemy reached last waypoint then it stops
        if (waypointIndex <= waypoints.Length - 1)
        {
            Vector3 targetPosition = waypoints[waypointIndex];

            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

            Vector3 moveDirection = (targetPosition - transform.position).normalized;

            animator.SetFloat("Horizontal", moveDirection.x);
            animator.SetFloat("Vertical", moveDirection.y);
            animator.SetFloat("Speed", moveDirection.magnitude);

            if (transform.position == targetPosition)
            {
                waypointIndex++;

                // Check if the NPC has reached the final waypoint
                if (waypointIndex == waypoints.Length)
                {
                    // Trigger the animator to set Order to true
                    npcInteract.EnableOrderPopUp();

                }
            }
        }
    }
}
