using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;

public class GrabStartup : BasicState
{
    public GrabStartup()
    {
        timeToChangeState = 1.0f;
        canPlayerControlMove = false;
        canPlayerControlRotate = false;
        canAttack = false;
        canDodgeRoll = false;
        canBlock = false;
        updateMovement = true;
        alternateFriction = true;
        countAttackCooldown = false;
        animationState = AnimationState.GrabStartup_01;
        stateToChangeTo = new GrabDuration();
    }

    public override void OnEnterThisState(BasicState prevState, PlayerStatus status)
    {
        base.OnEnterThisState(prevState, status);

        status.movement.SetTheSetForwardDirection();
    }
}
