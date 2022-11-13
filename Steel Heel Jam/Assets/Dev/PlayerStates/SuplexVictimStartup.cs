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
        isInvincibleToAttacks = true;
        isInvincibleToRing = true;

        animationState = AnimationState.SuplexVictimStartup_01;
        stateToChangeTo = new SuplexVictimDuration();
    }

    public override void OnEnterThisState(BasicState prevState, PlayerStatus status)
    {
        base.OnEnterThisState(prevState, status);

        status.transform.GetChild((int)PlayerChild.RingDecal).gameObject.SetActive(false);
    }

    public override void OnExitThisState(BasicState nextState, PlayerStatus status)
    {
        base.OnExitThisState(nextState, status);

        if (!(nextState is SuplexVictimDuration))
        {
            status.transform.GetChild((int)PlayerChild.RingDecal).gameObject.SetActive(true);
        }
    }
}
