using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuplexStartup : BasicState
{
    public SuplexStartup()
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
        stateToChangeTo = new SuplexDuration();
    }

    public override void OnEnterThisState(BasicState prevState, PlayerStatus status)
    {
        base.OnEnterThisState(prevState, status);
        Debug.Log("got to the startup.");
    }
}
