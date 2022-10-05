using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knockback : BasicState
{

    private Vector3 velocity;

    public Knockback(Vector3 _velocity)
    {
        timeToChangeState = 0;
        canPlayerControlMove = false;
        canPlayerControlRotate = false;
        canAttack = false;
        canDodgeRoll = false;
        canBlock = false;
        updateMovement = true;
        // alternateFriction = true;
        animationState = AnimationState.Knockback;
        stateToChangeTo = new Idle();

        visual = VisualChild.Knockback;

        velocity = _velocity;
    }

    public override void OnEnterThisState(BasicState prevState, PlayerStatus status)
    {
        base.OnEnterThisState(prevState, status);
        
        status.movement.velocity = velocity;
        status.movement.grounded = false;
    }

}
