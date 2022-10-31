using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuplexStartup : BasicState
{

    private PlayerStatus victim;
    public SuplexStartup(PlayerStatus _victim)
    {
        timeToChangeState = 0.5f;
        canPlayerControlMove = false;
        canPlayerControlRotate = false;
        canAttack = false;
        canDodgeRoll = false;
        canBlock = false;
        updateMovement = false;
        countAttackCooldown = false;
        animationState = AnimationState.SuplexStartup_01;
        stateToChangeTo = new SuplexDuration(_victim);
        isInvincibleToAttacks = true;
        isInvincibleToRing = true;
        victim = _victim;
    }

    public override void OnExitThisState(BasicState nextState, PlayerStatus status)
    {
        base.OnExitThisState(nextState, status);

        if(!(nextState is SuplexDuration)){
            victim.SetPlayerStateImmediately(new Idle());
        }

    }
}
