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

    private PlayerStatus ownerStatus;
    private int playerNumber = -1;

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

    public void Init(PlayerStatus _ownerStatus, bool damageOwnerToo)
    {
        ownerStatus = _ownerStatus;
        //spawn the explosion particle (Nick)
        if (!damageOwnerToo)
        {
            //set its color to a player specifically if its not gonna damage them (Nick)
            playerNumber = _ownerStatus.playerNumber;
        }

        AudioManager.aud.Play("explosion", 0.8f, 1.2f);
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
                ownerStatus
                );
        }
    }
}
