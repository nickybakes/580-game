using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DodgeRollFall : BasicState
{


    public DodgeRollFall()
    {
        timeToChangeState = 0;
        canPlayerControlMove = false;
        canPlayerControlRotate = false;
        canAttack = true;
        canDodgeRoll = false;
        canBlock = false;
        updateMovement = true;
        moveSpeedMultiplier = 2.1f;
        extraFallGravityMultiplier = .6f;
        countDodgeRollCooldown = false;
        animationState = AnimationState.DodgeRollFall;
        stateToChangeTo = new Idle();
    }

    
    public override void Update(PlayerStatus status) {
        base.Update(status);

        extraFallGravityMultiplier = Mathf.Lerp(.6f, 1, Mathf.Min(1, timeInThisState/.4f));

        status.movement.SetVelocityToMoveSpeedTimesFowardDirection();

        if(status.movement.grounded) {
            status.SetPlayerStateImmediately(new Idle());
        }
    }

}
