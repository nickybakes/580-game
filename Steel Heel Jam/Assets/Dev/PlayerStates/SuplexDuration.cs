using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuplexDuration : BasicState
{
    public SuplexDuration()
    {
        timeToChangeState = 0; // Until grounded.
        moveSpeedMultiplier = 0.5f;
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

        // If player is grounded and moving downwards.
        if (status.movement.grounded && status.movement.velocity.y < 0)
        {
            // Set to AttackAirRecovery.
            status.SetPlayerStateImmediately(new AttackAirRecovery());

            // Set suplexed state to knockback.
            // Reference.
            PlayerStatus g = status.transform.parent.GetComponent<PlayerStatus>();
            g.SetPlayerStateImmediately(new Knockback(new Vector3(0,10,0))); // Set straight up for testing.
        }
    }

    // Give initial arc direction.
    public override void OnEnterThisState(BasicState prevState, PlayerStatus status)
    {
        base.OnEnterThisState(prevState, status);
        Debug.Log("got to suplex duration");
        // Set velocity to arc. Moves opposite of ActualForwardDirection.
        Vector3 arc = -status.movement.ActualFowardDirection * moveSpeedMultiplier;
        arc.y = 10.0f; // Height of arc.

        status.movement.velocity = arc;
    }
}
