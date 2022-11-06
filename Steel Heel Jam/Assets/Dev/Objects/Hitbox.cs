using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hitbox : MonoBehaviour
{
    //**********
    // Fields
    //**********
    public float damage;
    public float knockback;
    public float knockbackHeight;
    public float timeInKnockback;
    public float radius;
    public float height;
    public float duration;
    public int playerNumber;

    public bool airAttack;

    public PlayerStatus playerStatus;

    /// <summary>
    /// A reference to this object's transform. Use this instead of GetComponent<Transform>()
    /// </summary>
    public Transform tr;

    // Start is called before the first frame update
    void Start()
    {
        playerStatus = transform.parent.GetComponent<PlayerStatus>();
        tr = transform;

        gameObject.SetActive(false);
    }

    // Update is called once per frame
    //void Update()
    //{
        // if(duration >= 0)
        //     duration -= Time.deltaTime;

        // if (duration <= 0)
        // {
        //     gameObject.SetActive(false);
        // }
    //}

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(Tag.Player.ToString()))
        {
            PlayerStatus player = other.GetComponent<PlayerStatus>();

            if (playerNumber == player.PlayerNumber) return;

            if (airAttack)
            {
                player.GetHitByElbowDrop(
                    playerStatus.transform.position,
                    player.transform.position,
                    damage,
                    knockback,
                    knockbackHeight,
                    timeInKnockback,
                    playerStatus,
                    false
                    );
            }
            else
            {
                player.GetHitByMelee(
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
    }

    // private void OnDrawGizmos()
    // {
    //     Gizmos.color = Color.red;
    //     Gizmos.DrawSphere(transform.position, radius);
    // }
}
