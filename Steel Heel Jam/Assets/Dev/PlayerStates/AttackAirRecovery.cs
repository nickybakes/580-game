using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackAirRecovery : BasicState
{

    public float velocityYAirAttack;
    public AttackAirRecovery()
    {
        timeToChangeState = .8f;
        canPlayerControlMove = false;
        canPlayerControlRotate = false;
        canAttack = false;
        canDodgeRoll = false;
        canBlock = false;
        updateMovement = false;
        countAttackCooldown = false;
        visual = VisualChild.Recovery;
        animationState = AnimationState.Eliminated;
        stateToChangeTo = new Idle();
    }

    public override void Update(PlayerStatus status)
    {
        base.Update(status);

        if (timeInThisState > .2)
        {
            status.combat.weaponState.ForceEndAttack();
        }
    }

    public override void OnExitThisState(BasicState nextState, PlayerStatus status)
    {
        base.OnExitThisState(nextState, status);

        status.combat.weaponState.ForceEndAttack();
    }

    public override void OnEnterThisState(BasicState prevState, PlayerStatus status)
    {
        base.OnEnterThisState(prevState, status);

        float fallHeightMultiplier = .75f + Mathf.Clamp((velocityYAirAttack - 43f) / 20f, 0f, 10f);

        status.combat.weaponState.AirAttackLand(fallHeightMultiplier, fallHeightMultiplier);
    }
}
