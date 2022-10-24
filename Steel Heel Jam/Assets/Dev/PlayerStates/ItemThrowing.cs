using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemThrowing : BasicState
{
    public ItemThrowing()
    {
        timeToChangeState = 0; // Only change state on button release.
        canPlayerControlMove = false;
        canPlayerControlRotate = true;
        updateMovement = true;
        canAttack = false;
        canDodgeRoll = false;
        canBlock = false;
        alternateFriction = true;
        canThrow = true;
        animationState = AnimationState.ThrowCharging_01;
        //visual = VisualChild.Throw;


        stateToChangeTo = new ThrowRecovery();
    }

    public override void OnEnterThisState(BasicState prevState, PlayerStatus status)
    {
        base.OnEnterThisState(prevState, status);

        status.audioManager.Play("throw", 0.8f, 1.2f);
    }
}
