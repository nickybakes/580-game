using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackGroundStartup : BasicState
{
    public AttackGroundStartup()
    {
        canPlayerControlMove = false;
        canPlayerControlRotate = false;
        canAttack = false;
        canDodgeRoll = false;
        canBlock = false;
        updateMovement = false;
        countAttackCooldown = false;
        animationState = AnimationState.AttackGroundStartup;
        stateToChangeTo = new AttackGroundDuration();
    }
}
