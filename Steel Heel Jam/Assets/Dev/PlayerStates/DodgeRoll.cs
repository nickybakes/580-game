using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DodgeRoll : BasicState
{


    public DodgeRoll()
    {
        timeToChangingState = .25f;
        canPlayerControlMove = false;
        canPlayerControlRotate = false;
        canAttack = false;
        canDodgeRoll = false;
        canBlock = false;
        updateMovement = true;
        moveSpeedMultiplier = 1.6f;
        extraFallGravityMultiplier = .8f;
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

    public override void OnExitThisState(BasicState nextState, PlayerStatus status)
    {
        base.OnExitThisState(nextState, status);

        status.movement.velocity = status.movement.velocity.normalized * status.movement.CurrentMoveSpeed;
    }

}
