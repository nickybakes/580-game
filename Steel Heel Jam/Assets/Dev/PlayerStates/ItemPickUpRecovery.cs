using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickUpRecovery : BasicState
{

    public ItemPickUpRecovery()
    {
        timeToChangeState = .15f;
        canPlayerControlMove = false;
        canPlayerControlRotate = false;
        canAttack = false;
        canDodgeRoll = false;
        canBlock = false;
        updateMovement = true;
        moveSpeedMultiplier = 0.1f;
        countDodgeRollCooldown = false;
        visual = VisualChild.Recovery;

        animationState = AnimationState.Idle;
        stateToChangeTo = new Idle();
    }

    public override void Update(PlayerStatus status)
    {
        base.Update(status);

        status.movement.SetVelocityToMoveSpeedTimesFowardDirection();
    }

    public override void OnEnterThisState(BasicState prevState, PlayerStatus status)
    {
        base.OnEnterThisState(prevState, status);

        status.movement.velocity = status.movement.velocity.normalized * status.movement.CurrentMoveSpeed;
    }
}
