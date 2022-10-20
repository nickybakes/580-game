using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuplexStartup : BasicState
{
    public SuplexStartup(PlayerStatus victim)
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
        stateToChangeTo = new SuplexDuration(victim);

    }
}
