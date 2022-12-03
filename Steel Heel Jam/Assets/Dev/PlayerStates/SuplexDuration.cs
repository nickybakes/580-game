using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class SuplexDuration : BasicState
{
    private PlayerStatus victim;
    public SuplexDuration(PlayerStatus _victim)
    {
        timeToChangeState = 7.5f; // Until grounded.
        moveSpeedMultiplier = 5f;
        canPlayerControlMove = false;
        canPlayerControlRotate = false;
        canAttack = false;
        canDodgeRoll = false;
        canBlock = false;
        updateMovement = true;
        countAttackCooldown = false;
        isInvincibleToAttacks = true;
        isInvincibleToRing = true;

        extraFallGravityMultiplier = 2f;
        animationState = AnimationState.SuplexDuration_01;
        stateToChangeTo = new SuplexRecovery();

        victim = _victim;
    }

    public override void Update(PlayerStatus status)
    {
        base.Update(status);

        victim.movement.velocity = status.movement.velocity;
        victim.GetTransform.position = status.GetTransform.position;

        status.visuals.RotateModelFlyingThroughAir();

        // If player is grounded and moving downwards.
        if (status.movement.grounded && status.movement.velocity.y < 0)
        {
            // Set to AttackAirRecovery.
            status.SetPlayerStateImmediately(new SuplexRecovery());
        }
    }

    public override void OnExitThisState(BasicState nextState, PlayerStatus status)
    {
        base.OnExitThisState(nextState, status);

        status.visuals.SetModelRotationX(0);

        // Play ground sound. Louder when moving faster.
        float vol = -status.movement.velocity.y / 50;

        AudioManager.aud.Play("crunch", vol / 6);
        AudioManager.aud.Play("punch", vol);
        AudioManager.aud.Play("landing", vol / 2);
        AudioManager.aud.Play("debris", vol / 3);
        AudioManager.aud.Play("orchestraHitLong");

        GameObject decal = VisualsManager.SpawnDecal(DecalName.Crack_01, status.transform.position);

        float scale = Mathf.Clamp((-status.movement.velocity.y - 36f) / 53f, .3f, 1);
        decal.transform.localScale = new Vector3(scale, scale, 1);

        GameObject particle = VisualsManager.SpawnParticle(ParticleName.AirAttackLand_01, status.transform.position);

        float scale2 = Mathf.Clamp((-status.movement.velocity.y - 39f) / 53f, .5f, 2f);
        particle.transform.localScale = new Vector3(scale2, scale2, scale2);

        Vector3 launchDir = status.transform.forward;

        launchDir.x *= -1;
        launchDir.z *= -1;

        float staminaMultiplier = (100 - victim.stamina) * 0.2f;
        float launchTopDownSpeed = 20 + staminaMultiplier;
        float launchHeight = 50 + staminaMultiplier;

        // Set suplexed state to knockback.
        Knockback knockback = new Knockback(new Vector3(launchDir.x * launchTopDownSpeed, launchHeight, launchDir.z * launchTopDownSpeed));
        knockback.timeToChangeState = 1.0f;
        victim.playerLastHitBy = status;
        victim.SetPlayerStateImmediately(knockback);

        status.totalDamageDealt += status.suplexStaminaDamage;
        victim.totalDamageTaken += status.suplexStaminaDamage;
        status.IncreaseSpotlightMeter(20);

        CameraManager.cam.ShakeCamera(.7f);
    }

    // Give initial arc direction.
    public override void OnEnterThisState(BasicState prevState, PlayerStatus status)
    {
        base.OnEnterThisState(prevState, status);

        // Set velocity to arc. Moves opposite of ActualForwardDirection.
        Vector3 arc = -status.transform.forward * moveSpeedMultiplier;

        arc.y = 50.0f; // Height of arc.

        status.movement.velocity = arc;
    }
}
