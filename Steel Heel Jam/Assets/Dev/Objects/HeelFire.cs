using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeelFire : MonoBehaviour
{
    [SerializeField] PlayerStatus playerStatus;
    int playerNumber;
    [SerializeField] float damage;
    [SerializeField] float knockback;
    [SerializeField] float knockbackHeight;
    [SerializeField] float timeInKnockback;

    // Start is called before the first frame update
    void Start()
    {
        playerNumber = playerStatus.playerNumber;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == Tag.Player.ToString())
        {
            PlayerStatus player = other.GetComponent<PlayerStatus>();

            if (playerNumber == player.PlayerNumber) return;

            player.GetHitByExplosive(
                playerStatus.transform.position,
                player.transform.position,
                damage,
                knockback,
                knockbackHeight,
                timeInKnockback,
                playerStatus
                );
        }
    }

    //private void OnTriggerStay(Collider other)
    //{
    //    if (other.tag == Tag.Player.ToString())
    //    {
    //        PlayerStatus player = other.GetComponent<PlayerStatus>();

    //        if (playerNumber == player.PlayerNumber) return;

    //        player.GetHitByExplosive(
    //            playerStatus.transform.position,
    //            player.transform.position,
    //            damage,
    //            knockback,
    //            knockbackHeight,
    //            timeInKnockback,
    //            playerStatus
    //            );
    //    }
    //}
}
