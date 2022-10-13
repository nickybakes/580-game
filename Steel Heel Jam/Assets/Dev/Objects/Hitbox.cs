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
    public float hitstun;
    public float radius;
    public float length;
    public float duration;
    public int playerNumber;

    public PlayerStatus playerStatus;

    /// <summary>
    /// A reference to this object's transform. Use this instead of GetComponent<Transform>()
    /// </summary>
    public Transform tr;

    // Start is called before the first frame update
    void Start()
    {
        gameObject.SetActive(true);

        playerStatus = transform.parent.GetComponent<PlayerStatus>();
        tr = transform;
    }

    // Update is called once per frame
    void Update()
    {
        if(duration >= 0)
            duration -= Time.deltaTime;

        if (duration <= 0)
        {
            gameObject.SetActive(false);
        }
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == Tag.Player.ToString())
        {
            PlayerStatus player = other.GetComponent<PlayerStatus>();

            if (playerNumber == player.PlayerNumber) return;

            player.GetHit(
                playerStatus.transform.position,
                other.transform.position,
                damage,
                knockback,
                knockbackHeight,
                hitstun,
                playerStatus
                );
        }
    }

    // private void OnDrawGizmos()
    // {
    //     Gizmos.color = Color.red;
    //     Gizmos.DrawSphere(transform.position, radius);
    // }
}
