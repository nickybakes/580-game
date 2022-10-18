using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffSpotlight : MonoBehaviour
{
    [SerializeField] public float speed = 3.0f;
    [SerializeField] public float targetDistanceMax = 10.0f;

    [SerializeField] public float wanderDecisionCooldownMax = 10.0f;
    private float wanderDecisionCooldown;
    private Vector2 wanderDirection;

    /// <summary>
    /// All the players currently within the spotlight.
    /// </summary>
    private List<PlayerStatus> players = new List<PlayerStatus>();

    private Transform tr;

    private Transform targetPlayerPosition;

    private bool playerTargeted = false;

    // Start is called before the first frame update
    void Start()
    {
        tr = GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        if (playerTargeted)
        {
            MoveTowardTargetPlayer();

            playerTargeted = ShouldContinueTargetingPlayer();
        } 
        else 
        {
            Wander();

            // If a player flexes in the spotlight, begin following them
            // NOTE: This prioritizes players in the order they entered the spotlight
            // TODO: Maybe decide based on distance from the center of the spotlight
            foreach (PlayerStatus status in players)
            {
                if (status.CurrentPlayerState is Rest)
                {
                    targetPlayerPosition = status.transform;
                    playerTargeted = true;
                }
            }
        }
    }

    private void Wander()
    {
        tr.position += new Vector3(wanderDirection.x, 0, wanderDirection.y) * Time.deltaTime;

        wanderDecisionCooldown += Time.deltaTime;

        if (wanderDecisionCooldown > wanderDecisionCooldownMax)
        {
            wanderDecisionCooldown = 0;

            wanderDirection = DecideWanderDirection();
            print(wanderDirection);
        }

        StayWithinBounds();
    }

    private Vector2 DecideWanderDirection()
    {
        return new Vector2(Random.Range(-1, 2), Random.Range(-1, 2));
    }

    private void MoveTowardTargetPlayer()
    {
        Vector3 movement = (targetPlayerPosition.position - tr.position).normalized * speed * Time.deltaTime;
        tr.position += new Vector3(movement.x, 0, movement.z);
    }

    private bool ShouldContinueTargetingPlayer()
    {
        if (Vector3.Distance(targetPlayerPosition.position, tr.position) > targetDistanceMax) return false;

        return true;
    }

    private void StayWithinBounds()
    {
        if (tr.position.x > 35)
        {
            wanderDirection.x = -1;
            wanderDecisionCooldown = 0;
        }
        else if (tr.position.x < -35)
        {
            wanderDirection.x = 1;
            wanderDecisionCooldown = 0;
        }

        if (tr.position.z > 17)
        {
            // Z axis
            wanderDirection.y = -1;
            wanderDecisionCooldown = 0;
        }
        else if(tr.position.z < -9)
        {
            // Z axis
            wanderDirection.y = 1;
            wanderDecisionCooldown = 0;
        }
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(Tag.Player.ToString()))
        {
            PlayerStatus status = other.gameObject.GetComponent<PlayerStatus>();

            status.isInSpotlight = true;

            // Add the player to the list of contained players
            players.Add(status);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(Tag.Player.ToString()))
        {
            PlayerStatus status = other.gameObject.GetComponent<PlayerStatus>();

            status.isInSpotlight = false;

            // Remove the player from the list of contained players
            players.Remove(status);
        }
    }
}
