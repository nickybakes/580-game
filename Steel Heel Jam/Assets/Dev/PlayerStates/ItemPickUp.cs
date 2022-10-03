using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickUp : BasicState
{
    public ItemPickUp()
    {
        timeToChangeState = 0.2f;
        canPlayerControlMove = false;
        canPlayerControlRotate = false;
        updateMovement = true;
        animationState = AnimationState.Idle;
        canPickUp = false;

        stateToChangeTo = new ItemPickUpRecovery();
    }

    public override void OnEnterThisState(BasicState prevState, PlayerStatus status)
    {
        base.OnEnterThisState(prevState, status);

        status.movement.velocity = status.movement.velocity.normalized * 0;
    }
}
