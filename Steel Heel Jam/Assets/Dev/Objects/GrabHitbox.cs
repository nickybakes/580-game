using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GrabHitbox : MonoBehaviour
{
    public PlayerStatus playerStatus;
    private List<GameObject> playersWithinBounds;

    // Start is called before the first frame update
    void Start()
    {
        playersWithinBounds = new List<GameObject>();
        playerStatus = transform.parent.GetComponent<PlayerStatus>();
    }

    // Update is called once per frame
    void Update()
    {
        if (playersWithinBounds.Count > 0)
            playersWithinBounds = playersWithinBounds.OrderBy(x => Vector3.Distance(playerStatus.transform.position, x.transform.position)).ToList();
    }

    public void TrySuplex()
    {
        if (playersWithinBounds.Count == 0)
            return;
        
        PlayerStatus g = playersWithinBounds[0].GetComponent<PlayerStatus>();

        // Make player grabbed unable to move and a child of the grabber.
        g.transform.SetParent(playerStatus.transform);
        // Grabbed player's position is overlapping grabber's position, will be fixed in animation.
        g.transform.position = playerStatus.transform.position;

        g.SetPlayerStateImmediately(new SuplexVictimStartup());
        playerStatus.SetPlayerStateImmediately(new SuplexStartup(g));
    }

    private void OnTriggerEnter(Collider other)
    {
        // Checks if colliding with a player, ignoring own player number.
        if (other.CompareTag(Tag.Player.ToString()) && other.GetComponent<PlayerStatus>().PlayerNumber != playerStatus.PlayerNumber)
        {
            playersWithinBounds.Add(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(Tag.Player.ToString()))
        {
            playersWithinBounds.Remove(other.gameObject);
        }
    }
}
