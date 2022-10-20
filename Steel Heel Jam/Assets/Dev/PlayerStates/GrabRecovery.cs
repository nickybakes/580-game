using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabRecovery : BasicState
{
    public GrabRecovery()
    {
        timeToChangeState = 1.0f;
        moveSpeedMultiplier = 0.1f;
        canPlayerControlMove = false;
        canPlayerControlRotate = false;
        canAttack = false;
        canDodgeRoll = false;
        canBlock = false;
        updateMovement = true;
        countAttackCooldown = false;
        animationState = AnimationState.BlockRecovery;
        stateToChangeTo = new Idle();
    }

    public override void Update(PlayerStatus status)
    {
        base.Update(status);

        status.movement.SetVelocityToMoveSpeedTimesFowardDirection();
    }
}
