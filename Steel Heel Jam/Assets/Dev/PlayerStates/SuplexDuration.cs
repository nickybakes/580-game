using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuplexDuration : BasicState
{
    private PlayerStatus victim;
    public SuplexDuration(PlayerStatus _victim)
    {
        timeToChangeState = 0; // Until grounded.
        moveSpeedMultiplier = 0.5f;
        canPlayerControlMove = false;
        canPlayerControlRotate = false;
        canAttack = false;
        canDodgeRoll = false;
        canBlock = false;
        updateMovement = true; // False for now.
        countAttackCooldown = false;
        animationState = AnimationState.Idle; // Will be Suplex
        stateToChangeTo = new Idle();

        victim = _victim;
    }

    public override void Update(PlayerStatus status)
    {
        base.Update(status);

        // If player is grounded and moving downwards.
        if (status.movement.grounded && status.movement.velocity.y < 0)
        {
            // Set to AttackAirRecovery.
            status.SetPlayerStateImmediately(new AttackAirRecovery());

            // Set suplexed state to knockback.
            Knockback knockback = new Knockback(new Vector3(0, 50, 0));
            knockback.timeToChangeState = 1.0f;
            victim.SetPlayerStateImmediately(knockback); // Set straight up for testing.
            //victim.GetHit(victim.transform.position, status.transform.position, 10, 20, 25, 2, status);
        }
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
