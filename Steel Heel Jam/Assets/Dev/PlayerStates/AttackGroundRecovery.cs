using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackGroundRecovery : BasicState
{
    public AttackGroundRecovery(float recovery)
    {
        timeToChangingState = recovery;
        canPlayerControlMove = false;
        canPlayerControlRotate = false;
        canAttack = true;
        canDodgeRoll = true;
        canBlock = true;
        updateMovement = true;
        moveSpeedMultiplier = 1.6f;
        extraFallGravityMultiplier = .8f;
        countDodgeRollCooldown = false;
        animationState = AnimationState.DodgeRoll; // TODO: CHANGE THIS
        stateToChangeTo = new Idle();
    }


    public override void Update(PlayerStatus status)
    {
        base.Update(status);

        //status.movement.SetVelocityToMoveSpeedTimesFowardDirection();

        //if (!status.movement.grounded && status.movement.wasGrounded)
        //{
        //    status.SetPlayerStateImmediately(new DodgeRollFall());
        //}
    }
}
