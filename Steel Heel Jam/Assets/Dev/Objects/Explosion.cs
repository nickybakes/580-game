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

    public void Init(PlayerStatus _ownerStatus, bool damageOwnerToo, float knockbackScale = 1, float transformScale = 1)
    {
        ownerStatus = _ownerStatus;
        //spawn the explosion particle
        if (!damageOwnerToo)
        {
            //set its color to a player specifically if its not gonna damage them
            playerNumber = _ownerStatus.playerNumber;
            GameObject g = VisualsManager.SpawnParticle(ParticleName.Explosion_01 + playerNumber, tr.position);
            g.transform.localScale = g.transform.localScale * transformScale;
        }
        else
        {
            GameObject g = VisualsManager.SpawnParticle(ParticleName.Explosion_01, tr.position);
            g.transform.localScale = g.transform.localScale * transformScale;
        }

        transform.localScale = transform.localScale * transformScale;

        knockback = knockback * knockbackScale;
        knockbackHeight = knockbackHeight * knockbackScale;
        damage = damage * knockbackScale;
        timeInKnockback = timeInKnockback * knockbackScale;


        VisualsManager.SpawnDecal(DecalName.Crack_01, tr.position);

        CameraManager.cam.ShakeCamera(.5f);

        AudioManager.aud.Play("explosion", 0.8f, 1.2f);
        AudioManager.aud.Play("debris", .4f, 0.65f, 1.8f);
        AudioManager.aud.Play("crunch", .2f, 0.4f, .65f);
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
        if (other.CompareTag(Tag.PickUp.ToString()))
        {
            GameObject pickupObject = other.gameObject;
            // while (pickupObject.transform.parent != null)
            // {
            //     pickupObject = pickupObject.transform.parent.gameObject;
            // }

            Rigidbody rb = pickupObject.GetComponent<Rigidbody>();

            if (rb != null)
            {
                rb.AddExplosionForce(1500, tr.position, 15, 30);
            }
        }
    }
}
