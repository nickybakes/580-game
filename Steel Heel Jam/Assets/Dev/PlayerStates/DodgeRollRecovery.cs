using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DodgeRollRecovery : BasicState
{


    public DodgeRollRecovery()
    {
        timeToChangingState = .15f;
        canPlayerControlMove = false;
        canPlayerControlRotate = false;
        canAttack = false;
        canDodgeRoll = false;
        canBlock = false;
        updateMovement = true;
        moveSpeedMultiplier = .4f;
        animationState = AnimationState.DodgeRoll;
        stateToChangeTo = new Idle();
    }

}
