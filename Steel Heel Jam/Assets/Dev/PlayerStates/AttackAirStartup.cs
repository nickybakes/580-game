using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackAirStartup : BasicState
{
    public AttackAirStartup()
    {
        timeToChangeState = 0.25f;
        canPlayerControlMove = false;
        canPlayerControlRotate = false;
        canAttack = false;
        canDodgeRoll = false;
        canBlock = false;
        updateMovement = false;
        countAttackCooldown = false;
        animationState = AnimationState.Eliminated;
        stateToChangeTo = new AttackAirDuration();
    }

    public override void Update(PlayerStatus status)
    {
        base.Update(status);
    }

    public override void OnEnterThisState(BasicState prevState, PlayerStatus status)
    {
        base.OnEnterThisState(prevState, status);
    }

    public override void OnExitThisState(BasicState nextState, PlayerStatus status)
    {
        base.OnExitThisState(nextState, status);
    }
}