using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuplexDuration : BasicState
{
    public SuplexDuration()
    {
        timeToChangeState = 0; // Until grounded.
        canPlayerControlMove = false;
        canPlayerControlRotate = false;
        canAttack = false;
        canDodgeRoll = false;
        canBlock = false;
        updateMovement = false; // False for now.
        countAttackCooldown = false;
        animationState = AnimationState.Knockback; // Will be Suplex
        stateToChangeTo = new Idle();
    }

    public override void Update(PlayerStatus status)
    {
        base.Update(status);


        if (status.movement.grounded)
        {
            // Set to AttackAirRecovery.
            status.SetPlayerStateImmediately(new AttackAirRecovery());

            // Set suplexed state to knockback.
            // Reference.
        }
    }

    // Give initial arc direction.
    public override void OnEnterThisState(BasicState prevState, PlayerStatus status)
    {
        base.OnEnterThisState(prevState, status);

        // Set velocity to arc.
    }
}
