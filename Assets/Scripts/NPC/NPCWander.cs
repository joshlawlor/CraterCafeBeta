using System.Collections;
using UnityEngine;

public class NPCWander : MonoBehaviour
{
    [SerializeField]
    private Transform[] wanderPoints;

    [SerializeField]
    private float wanderSpeed = 1.5f;

    private int currentWanderIndex = 0;

    public Animator animator;


    private void Start()
    {
        StartCoroutine(Wander());
    }

    private IEnumerator Wander()
    {
        while (true)
        {
            Vector3 targetPosition = wanderPoints[currentWanderIndex].position;

            transform.position = Vector3.MoveTowards(transform.position, targetPosition, wanderSpeed * Time.deltaTime);

            Vector3 moveDirection = (targetPosition - transform.position).normalized;

            animator.SetFloat("Horizontal", moveDirection.x);
            animator.SetFloat("Vertical", moveDirection.y);
            animator.SetFloat("Speed", moveDirection.magnitude);

            if (transform.position == targetPosition)
            {
                // Move to the next wander point
                currentWanderIndex = (currentWanderIndex + 1) % wanderPoints.Length;


                // Pause between waypoints
                yield return new WaitForSeconds(2f);
            }

            yield return null;
        }
    }
}
