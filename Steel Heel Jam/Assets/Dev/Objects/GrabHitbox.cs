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
        this.gameObject.SetActive(false);
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

        PlayerStatus victim = playersWithinBounds[0].GetComponent<PlayerStatus>();

        if (victim.CurrentPlayerState.isInvincibleToAttacks || victim.eliminated || playerStatus.eliminated)
            return;

        // Make player grabbed unable to move and a child of the grabber.
        victim.transform.SetParent(playerStatus.transform);
        // Grabbed player's position is overlapping grabber's position, will be fixed in animation.
        victim.transform.position = playerStatus.transform.position;

        Vector3 forward = playerStatus.transform.forward;

        victim.transform.forward = new Vector3(forward.x, 0, forward.z);

        victim.SetPlayerStateImmediately(new SuplexVictimStartup());
        playerStatus.SetPlayerStateImmediately(new SuplexStartup(victim));

        // Removes victim from grabHitbox.
        playersWithinBounds.Remove(victim.gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (playerStatus == null)
            return;
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
