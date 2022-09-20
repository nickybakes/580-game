using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackGroundStartup : BasicState
{
    public AttackGroundStartup(float startup, float recovery)
    {
        timeToChangingState = startup;
        canPlayerControlMove = false;
        canPlayerControlRotate = false;
        canAttack = false;
        canDodgeRoll = false;
        canBlock = false;
        updateMovement = true;
        moveSpeedMultiplier = 1.6f;
        extraFallGravityMultiplier = .8f;
        countDodgeRollCooldown = false;
        animationState = AnimationState.DodgeRoll; // TODO: CHANGE THIS
        stateToChangeTo = new AttackGroundRecovery(recovery);
        stateToChangeTo.timeToChangingState = recovery;
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
