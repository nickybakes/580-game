using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuplexMissRecovery : BasicState
{
    public SuplexMissRecovery()
    {
        timeToChangeState = 1.0f;
        canPlayerControlMove = false;
        canPlayerControlRotate = false;
        canAttack = false;
        canDodgeRoll = false;
        canBlock = false;
        updateMovement = true;
        countAttackCooldown = false;
        animationState = AnimationState.AttackGroundRecovery_01;
        stateToChangeTo = new Idle();
    }
}
