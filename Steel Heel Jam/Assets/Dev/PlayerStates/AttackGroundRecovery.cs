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
        canDodgeRoll = false;
        canBlock = false;
        updateMovement = true;
        countAttackCooldown = false;
        animationState = AnimationState.AttackGroundRecovery;
        stateToChangeTo = new Idle();
    }


    public override void Update(PlayerStatus status)
    {
        base.Update(status);

    }
}
