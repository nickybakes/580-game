using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackGroundRecovery : BasicState
{
    public AttackGroundRecovery(float recovery, bool canCombo)
    {
        timeToChangingState = recovery;
        canPlayerControlMove = false;
        canPlayerControlRotate = false;
        canAttack = canCombo ? true : false;
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
        if (timeToChangingState != 0)
        {
            timeInThisState += Time.deltaTime;
            if (timeInThisState >= timeToChangingState)
            {
                changeStateNow = true;
            }
        }
    }
}
