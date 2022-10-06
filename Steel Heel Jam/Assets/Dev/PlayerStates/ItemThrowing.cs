using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
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
        animationState = AnimationState.ThrowCharging_01;
        //visual = VisualChild.Throw;


        stateToChangeTo = new ThrowRecovery();
    }

    public override void Update(PlayerStatus status)
    {
        base.Update(status);

        // Decrease movespeed to a cap. (2 seconds to lerp from 1.0 to 0.2 movespeed.)
        //moveSpeedMultiplier = Mathf.Lerp(1.0f, 0.2f, timeInThisState / 2.0f);

        //status.movement.SetVelocityToMoveSpeedTimesFowardDirection();
    }

    public override void OnEnterThisState(BasicState prevState, PlayerStatus status)
    {
        base.OnEnterThisState(prevState, status);

        //status.movement.velocity = status.movement.velocity.normalized * moveSpeedMultiplier;
        // status.movement.SetTopDownVelocityToZero();
    }
}
