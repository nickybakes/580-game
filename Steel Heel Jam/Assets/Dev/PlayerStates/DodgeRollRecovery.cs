using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DodgeRollRecovery : BasicState
{


    public DodgeRollRecovery()
    {
        timeToChangingState = .15f;
        canPlayerControlMove = false;
        canPlayerControlRotate = false;
        canAttack = false;
        canDodgeRoll = false;
        canBlock = false;
        updateMovement = true;
        moveSpeedMultiplier = 1.6f;
        countDodgeRollCooldown = false;
        animationState = AnimationState.DodgeRoll;
        stateToChangeTo = new Idle();
    }

    public override void Update(PlayerStatus status)
    {
        base.Update(status);

        moveSpeedMultiplier = Mathf.Lerp(1.6f, .4f, timeInThisState/timeToChangingState); 

        status.movement.SetVelocityToMoveSpeedTimesFowardDirection();

        if (!status.movement.grounded && status.movement.wasGrounded)
        {
            status.SetPlayerStateImmediately(new DodgeRollFall());
        }
    }

}
