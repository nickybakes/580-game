using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;

public class ItemThrowing : BasicState
{
    public ItemThrowing()
    {
        timeToChangeState = 999f; // Only change state on button release.
        canPlayerControlMove = false;
        canPlayerControlRotate = true;
        updateMovement = true;
        canAttack = false;
        canDodgeRoll = true;
        canBlock = true;
        moveSpeedMultiplier = 1.0f;
        //animationState = AnimationState.Throwing;
        //visual = VisualChild.Throw;

        stateToChangeTo = new ThrowRecovery();
    }

    public override void Update(PlayerStatus status)
    {
        base.Update(status);

        // Decrease movespeed to a cap. (2 seconds to lerp from 1.0 to 0.2 movespeed.)
        moveSpeedMultiplier = Mathf.Lerp(1.0f, 0.2f, timeInThisState / 2.0f);

        status.movement.SetVelocityToMoveSpeedTimesFowardDirection();

        // Needs a way to see when the input is released to change to Idle state.
        if (/*input is released*/ true)
        {
            status.SetPlayerStateImmediately(new Idle());
        }
    }

    public override void OnEnterThisState(BasicState prevState, PlayerStatus status)
    {
        base.OnEnterThisState(prevState, status);

        status.movement.velocity = status.movement.velocity.normalized * status.movement.CurrentMoveSpeed;
    }
}
