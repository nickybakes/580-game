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
        alternateFriction = true;
        animationState = AnimationState.Pickup_01;

        stateToChangeTo = new Idle();
    }

    public override void OnEnterThisState(BasicState prevState, PlayerStatus status)
    {
        base.OnEnterThisState(prevState, status);

        // status.movement.SetTopDownVelocityToZero();

        AudioManager.aud.Play("pickup");
    }
}
