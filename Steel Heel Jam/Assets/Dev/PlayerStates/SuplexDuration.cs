using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuplexDuration : BasicState
{
    private PlayerStatus victim;
    public SuplexDuration(PlayerStatus _victim)
    {
        timeToChangeState = 0; // Until grounded.
        moveSpeedMultiplier = 5f;
        canPlayerControlMove = false;
        canPlayerControlRotate = false;
        canAttack = false;
        canDodgeRoll = false;
        canBlock = false;
        updateMovement = true; // False for now.
        countAttackCooldown = false;
        extraFallGravityMultiplier = 2f;
        animationState = AnimationState.SuplexDuration_01;
        stateToChangeTo = new Idle();

        victim = _victim;
    }

    public override void Update(PlayerStatus status)
    {
        base.Update(status);

        victim.movement.velocity = status.movement.velocity;

        status.visuals.RotateModelFlyingThroughAir();

        // If player is grounded and moving downwards.
        if (status.movement.grounded && status.movement.velocity.y < 0)
        {
            // Set to AttackAirRecovery.
            status.SetPlayerStateImmediately(new SuplexRecovery());

            Vector3 launchDir = status.transform.forward;

            launchDir.x *= -1;
            launchDir.z *= -1;

            float launchTopDownSpeed = 20;

            // Set suplexed state to knockback.
            Knockback knockback = new Knockback(new Vector3(launchDir.x * launchTopDownSpeed, 50, launchDir.z * launchTopDownSpeed));
            knockback.timeToChangeState = 1.0f;
            victim.SetPlayerStateImmediately(knockback);
            // Add GetSuplex Method to do values for knockback, damage, etc.
        }
    }

    public override void OnExitThisState(BasicState nextState, PlayerStatus status)
    {
        base.OnExitThisState(nextState, status);

        status.visuals.SetModelRotationX(0);

        // Play ground sound. Louder when moving faster.
        float vol = -status.movement.velocity.y / 50;
        AudioManager audioManager = GameObject.FindObjectOfType<AudioManager>();

        audioManager.Play("crunch", vol / 3);
        audioManager.Play("punch", vol);
        audioManager.Play("landing", vol / 2);
        audioManager.Play("orchestraHitLong");
    }

    // Give initial arc direction.
    public override void OnEnterThisState(BasicState prevState, PlayerStatus status)
    {
        base.OnEnterThisState(prevState, status);

        // Set velocity to arc. Moves opposite of ActualForwardDirection.
        Vector3 arc = -status.movement.ActualFowardDirection * moveSpeedMultiplier;
        arc.y = 50.0f; // Height of arc.

        status.movement.velocity = arc;
    }
}
