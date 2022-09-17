using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DodgeRoll : BasicState
{


    public DodgeRoll()
    {
        timeToChangingState = .2f;
        canPlayerControlMove = false;
        canPlayerControlRotate = false;
        canAttack = false;
        canDodgeRoll = false;
        canBlock = false;
        updateMovement = true;
        moveSpeedMultiplier = 1.6f;
        countDodgeRollCooldown = false;
        animationState = AnimationState.DodgeRoll;
        stateToChangeTo = new DodgeRollRecovery();
    }

    
    public override void Update(PlayerStatus status) {
        base.Update(status);

        status.movement.SetVelocityToMoveSpeedTimesFowardDirection();
        
        if(!status.movement.grounded && status.movement.wasGrounded) {
            status.SetPlayerStateImmediately(new DodgeRollFall());
        }
    }

}
