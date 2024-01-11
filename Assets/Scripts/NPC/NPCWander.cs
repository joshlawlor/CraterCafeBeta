using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

public class NPCWander : MonoBehaviour
{
    [SerializeField]
    private Transform[] wanderPoints;

    [SerializeField]
    private float wanderSpeed = 1.5f;

    private int currentWanderIndex = 0;

    public Animator animator;

    private Collider2D pathfinderCollider;


    private void Start()
    {
        pathfinderCollider = transform.Find("PathFinder").GetComponent<Collider2D>();

        StartCoroutine(Wander());
    }
    private IEnumerator Wander()
    {
        while (true)
        {
            Vector3 targetPosition = wanderPoints[currentWanderIndex].position;

            // Check if the next position is clear
            if (IsPositionClear(targetPosition))
            {
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
            }
            else
            {
                // If the next position is not clear, wait for a short time
                yield return new WaitForSeconds(0.5f);
            }

            yield return null;
        }
    }

    private bool IsPositionClear(Vector3 position)
    {
        // Get the Tilemap component for the "Furniture" layer
        Tilemap furnitureTilemap = GameObject.Find("FurnitureTilemap").GetComponent<Tilemap>();

        // Get the tile at the specified position
        Vector3Int cellPosition = furnitureTilemap.WorldToCell(position);
        TileBase tile = furnitureTilemap.GetTile(cellPosition);

        // If there is no tile at the position, the area is clear
        return tile == null;
    }

}

