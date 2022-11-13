using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffSpotlight : MonoBehaviour
{
    [SerializeField] public float targetingSpeed = 5f;
    [SerializeField] public float wanderingSpeed = 3.5f;
    [SerializeField] public float targetDistanceMax = 14.0f;

    [SerializeField] public float wanderDecisionCooldownMax = 10.0f;
    private float wanderDecisionCooldown;
    private Vector2 wanderDirection;

    /// <summary>
    /// All the players currently within the spotlight.
    /// </summary>
    private List<PlayerStatus> players = new List<PlayerStatus>();

    [HideInInspector]
    public Transform tr;

    private Transform targetPlayerPosition;

    private bool playerTargeted = false;

    // Start is called before the first frame update
    void Awake()
    {
        tr = GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        // If a player flexes in the spotlight, begin following them
        // NOTE: This prioritizes players in the order they entered the spotlight
        CheckFlexers();

        if (playerTargeted)
        {
            MoveTowardTargetPlayer();

            playerTargeted = ShouldContinueTargetingPlayer();
        }
        else
        {
            Wander();
        }
    }

    private void Wander()
    {
        tr.position += new Vector3(wanderDirection.x, 0, wanderDirection.y) * wanderingSpeed * Time.deltaTime;

        wanderDecisionCooldown += Time.deltaTime;

        if (wanderDecisionCooldown > wanderDecisionCooldownMax)
        {
            wanderDecisionCooldown = 0;

            DecideWanderDirection();
        }

        StayWithinBounds();
    }

    public void DecideWanderDirection()
    {
        float angle = Random.Range(0, Mathf.PI * 2);
        wanderDirection = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
    }

    private void CheckFlexers()
    {
        bool spotlightFilled = false;
        foreach (PlayerStatus status in players)
        {
            if (status.CurrentPlayerState is Flexing)
            {
                targetPlayerPosition = status.transform;
                playerTargeted = true;

                if (status.spotlight >= PlayerStatus.defaultMaxSpotlight)
                {
                    spotlightFilled = true;
                    status.ReduceSpotlightMeter(PlayerStatus.defaultMaxSpotlight);
                    VisualsManager.SpawnParticle(ParticleName.SpotlightBuffGet_01, status.transform.position);
                    status.GiveBuff();
                    status.isInSpotlight = false;
                    break;
                }
            }
        }

        if (spotlightFilled)
        {
            foreach (PlayerStatus status in players)
            {
                status.isInSpotlight = false;
            }
            players.Clear();
            GameManager.game.DespawnSpotlight();
        }
    }

    private void MoveTowardTargetPlayer()
    {
        Vector3 movement = (targetPlayerPosition.position - tr.position).normalized * targetingSpeed * Time.deltaTime;
        tr.position += new Vector3(movement.x, 0, movement.z);
    }

    private bool ShouldContinueTargetingPlayer()
    {
        if (Vector3.Distance(targetPlayerPosition.position, tr.position) > targetDistanceMax) return false;

        return true;
    }

    private void StayWithinBounds()
    {
        if ((tr.position.x > GameManager.game.gameSceneSettings.maxItemSpawnBoundaries.x && wanderDirection.x > 0) || (tr.position.x < GameManager.game.gameSceneSettings.minItemSpawnBoundaries.x && wanderDirection.x < 0))
        {
            wanderDirection.x *= -1;
            wanderDecisionCooldown = 0;
        }

        if ((tr.position.z > GameManager.game.gameSceneSettings.maxItemSpawnBoundaries.z && wanderDirection.y > 0) || (tr.position.z < GameManager.game.gameSceneSettings.minItemSpawnBoundaries.z && wanderDirection.y < 0))
        {
            wanderDirection.y *= -1;
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
