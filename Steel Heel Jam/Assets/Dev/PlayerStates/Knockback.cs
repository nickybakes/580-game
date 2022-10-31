using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knockback : BasicState
{

    private Vector3 velocity;

    private const float maxTimeGrounded = .3f;

    private float timeGrounded;

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

        velocity = _velocity;
    }

    public override void OnEnterThisState(BasicState prevState, PlayerStatus status)
    {
        base.OnEnterThisState(prevState, status);

        status.movement.velocity = velocity;
        status.movement.grounded = false;
    }

    public override void OnExitThisState(BasicState nextState, PlayerStatus status)
    {
        base.OnExitThisState(nextState, status);

        status.visuals.SetModelRotationX(0);
    }

    public override void Update(PlayerStatus status)
    {
        base.Update(status);

        status.visuals.RotateModelFlyingThroughAir();

        if (status.movement.grounded)
        {
            timeGrounded += Time.deltaTime;
            if(timeGrounded >= maxTimeGrounded){
                status.SetPlayerStateImmediately(stateToChangeTo);
            }
        }
    }

}
