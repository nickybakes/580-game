using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabRecovery : BasicState
{
    public GrabRecovery()
    {
        timeToChangeState = 0.3f;
        moveSpeedMultiplier = 1.5f;
        canPlayerControlMove = false;
        canPlayerControlRotate = false;
        canAttack = false;
        canDodgeRoll = false;
        canBlock = false;
        updateMovement = true;
        countAttackCooldown = false;
        animationState = AnimationState.BlockRecovery;
        stateToChangeTo = new Idle();
        botPickStrategyOnExit = true;
    }

    public override void Update(PlayerStatus status)
    {
        base.Update(status);

        moveSpeedMultiplier = Mathf.Lerp(1.5f, 0.0f, (timeInThisState * 2));

        status.movement.SetVelocityToMoveSpeedTimesFowardDirection();
    }
}
