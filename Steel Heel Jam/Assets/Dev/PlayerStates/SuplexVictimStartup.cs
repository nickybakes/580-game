using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuplexVictimStartup : BasicState
{
    public SuplexVictimStartup()
    {
        timeToChangeState = 0.5f;
        canPlayerControlMove = false;
        canPlayerControlRotate = false;
        canAttack = false;
        canDodgeRoll = false;
        canBlock = false;
        updateMovement = false;
        countAttackCooldown = false;
        animationState = AnimationState.BlockRecovery;
        stateToChangeTo = new SuplexVictimDuration();
    }
}
