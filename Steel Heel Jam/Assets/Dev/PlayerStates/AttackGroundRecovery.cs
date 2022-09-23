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
        countAttackCooldown = false;
        animationState = AnimationState.AttackGroundRecovery; // TODO: CHANGE THIS
        stateToChangeTo = new Idle();
    }


    public override void Update(PlayerStatus status)
    {
        base.Update(status);

    }
}
