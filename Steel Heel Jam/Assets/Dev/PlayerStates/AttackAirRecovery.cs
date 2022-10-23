using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackAirRecovery : BasicState
{
    public AttackAirRecovery()
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
        animationState = AnimationState.Eliminated;
        stateToChangeTo = new Idle();
    }
}
