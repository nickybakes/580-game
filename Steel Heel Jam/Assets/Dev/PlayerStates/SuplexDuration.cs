using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuplexDuration : BasicState
{
    public SuplexDuration()
    {
        timeToChangeState = 0; // Until grounded.
        canPlayerControlMove = false;
        canPlayerControlRotate = false;
        canAttack = false;
        canDodgeRoll = false;
        canBlock = false;
        updateMovement = true;
        countAttackCooldown = false;
        animationState = AnimationState.Knockback; // Will be Suplex
        stateToChangeTo = new Idle();
    }

    // Update the arc of the suplex.
    public override void Update(PlayerStatus status)
    {
        base.Update(status);


        status.movement.SetVelocityToMoveSpeedTimesFowardDirection();
    }

    // Give initial arc direction.
    public override void OnEnterThisState(BasicState prevState, PlayerStatus status)
    {
        base.OnEnterThisState(prevState, status);

        status.movement.SetVelocityToMoveSpeedTimesFowardDirection();
    }
}
