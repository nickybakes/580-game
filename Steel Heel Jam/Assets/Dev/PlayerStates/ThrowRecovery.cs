using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowRecovery : BasicState
{
    public ThrowRecovery()
    {
        timeToChangeState = .15f;
        canPlayerControlMove = false;
        canPlayerControlRotate = false;
        canAttack = false;
        canDodgeRoll = false;
        canBlock = false;
        updateMovement = true;
        moveSpeedMultiplier = 0.1f;
        countDodgeRollCooldown = false;
        visual = VisualChild.Recovery;
        botPickStrategyOnExit = true;

        animationState = AnimationState.Throw_01;
        stateToChangeTo = new Idle();
    }
}