using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;

public class SuplexStartup : BasicState
{
    public SuplexStartup()
    {
        timeToChangeState = 1.0f;
        canPlayerControlMove = false;
        canPlayerControlRotate = false;
        canAttack = false;
        canDodgeRoll = false;
        canBlock = false;
        updateMovement = false;
        countAttackCooldown = false;
        animationState = AnimationState.AttackGroundStartup_01;
        stateToChangeTo = new Idle();
    }

    public override void Update(PlayerStatus status)
    {
        base.Update(status);

        if (timeInThisState >= 0.1f)
        {
            //GrabHitbox grabHitbox = new GrabHitbox();
            //grabHitbox.TrySuplex();
            // Create/SetActive hitbox in front of player.
            // If then this hits another player, send to other Suplex states.
        }
        // If nothing is grabbed, currently sends back to Idle state. Will send to SuplexRecovery state.

        //status.movement.SetVelocityToMoveSpeedTimesFowardDirection();
    }
}
