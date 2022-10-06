using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DodgeRollRecovery : BasicState
{


    public DodgeRollRecovery()
    {
        timeToChangeState = .15f;
        canPlayerControlMove = false;
        canPlayerControlRotate = false;
        canAttack = false;
        canDodgeRoll = false;
        canBlock = false;
        updateMovement = true;
        moveSpeedMultiplier = 1.6f;
        countDodgeRollCooldown = false;
        visual = VisualChild.Recovery;

        animationState = AnimationState.DodgeRollRecovery;
        stateToChangeTo = new Idle();
    }

    public override void Update(PlayerStatus status)
    {
        base.Update(status);

        moveSpeedMultiplier = Mathf.Lerp(1.6f, .4f, timeInThisState/timeToChangeState); 

        status.movement.SetVelocityToMoveSpeedTimesFowardDirection();

        if (!status.movement.grounded && status.movement.wasGrounded)
        {
            status.SetPlayerStateImmediately(new DodgeRollFall());
        }
    }

    public override void OnEnterThisState(BasicState prevState, PlayerStatus status)
    {
        base.OnEnterThisState(prevState, status);

        status.movement.velocity = status.movement.velocity.normalized * status.movement.CurrentMoveSpeed;
    }

}
