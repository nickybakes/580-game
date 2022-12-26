using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuplexRecovery : BasicState
{
    public SuplexRecovery()
    {
        timeToChangeState = .8f;
        canPlayerControlMove = false;
        canPlayerControlRotate = false;
        canAttack = false;
        canDodgeRoll = false;
        canBlock = false;
        updateMovement = false;
        countAttackCooldown = false;
        visual = VisualChild.Recovery;
        animationState = AnimationState.SuplexRecovery_01;
        stateToChangeTo = new Idle();
        botPickStrategyOnExit = true;
    }
}
