using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuplexStartup : BasicState
{
    public SuplexStartup()
    {
        timeToChangeState = 0.1f;
        canPlayerControlMove = false;
        canPlayerControlRotate = false;
        canAttack = false;
        canDodgeRoll = false;
        canBlock = false;
        updateMovement = false;
        countAttackCooldown = false;
        animationState = AnimationState.Idle;
        stateToChangeTo = new SuplexDuration();
    }
}
