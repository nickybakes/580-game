using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    private Transform tr;

    [SerializeField] public float explosionDurationMax;
    private float explosionDuration;

    [SerializeField] private float damage;
    [SerializeField] private float knockback;
    [SerializeField] private float knockbackHeight;
    [SerializeField] private float timeInKnockback;

    public PlayerStatus playerStatus;
    public int playerNumber;

    private void Awake()
    {
        tr = transform;
    }

    // Update is called once per frame
    void Update()
    {
        explosionDuration += Time.deltaTime;
        if (explosionDuration > explosionDurationMax) Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(Tag.Player.ToString()))
        {
            PlayerStatus player = other.GetComponent<PlayerStatus>();

            if (playerNumber == player.PlayerNumber) return;

            player.GetHitByExplosive(
                tr.position,
                player.transform.position,
                damage,
                knockback,
                knockbackHeight,
                timeInKnockback,
                playerStatus
                );

            print("BOOM");
        }
    }
}
