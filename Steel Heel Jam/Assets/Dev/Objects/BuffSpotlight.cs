using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffSpotlight : MonoBehaviour
{
    [SerializeField] public float speed = 3.0f;
    [SerializeField] public float targetDistanceMax = 10.0f;

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
        float x = Random.Range(0, 2) * Time.deltaTime;
        float z = Random.Range(0, 2) * Time.deltaTime;

        tr.position = new Vector3(tr.position.x + x, tr.position.y, tr.position.z + z);
    }

    private void MoveTowardTargetPlayer()
    {
        tr.position += (targetPlayerPosition.position - tr.position).normalized * speed * Time.deltaTime;
    }

    private bool ShouldContinueTargetingPlayer()
    {
        if (Vector3.Distance(targetPlayerPosition.position, tr.position) > targetDistanceMax) return false;

        return true;
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
