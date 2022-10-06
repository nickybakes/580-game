using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Idle : BasicState
{


    public Idle()
    {
        timeToChangeState = 0;
        canPlayerControlMove = true;
        canPlayerControlRotate = true;
        updateMovement = true;
        canPickUp = true;
        animationState = AnimationState.Idle;
    }

    private AnimationState DetermineAnimationState(PlayerStatus status)
    {
        AnimationState animState = AnimationState.Idle;
        if (status.movement.grounded && status.movement.InputDirectionNotZero)
        {
            animState = AnimationState.Run;
        }

        if (!status.movement.grounded && status.movement.velocity.y > 0)
        {
            animState = AnimationState.Jump;
        }

        if (!status.movement.grounded && status.movement.velocity.y <= 0)
        {
            animState = AnimationState.Fall;
        }
        return animState;
    }

    public override void OnEnterThisState(BasicState prevState, PlayerStatus status)
    {
        base.OnEnterThisState(prevState, status);

        animationState = DetermineAnimationState(status);
    }

    public override void Update(PlayerStatus status)
    {
        base.Update(status);

        status.SetAnimationState(DetermineAnimationState(status));
    }

}
