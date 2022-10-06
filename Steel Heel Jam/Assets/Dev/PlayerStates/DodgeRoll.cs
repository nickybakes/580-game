using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DodgeRoll : BasicState
{


    public DodgeRoll()
    {
        timeToChangeState = .3f;
        canPlayerControlMove = false;
        canPlayerControlRotate = false;
        canAttack = false;
        canDodgeRoll = false;
        canBlock = false;
        updateMovement = true;
        moveSpeedMultiplier = 1.7f;
        extraFallGravityMultiplier = .8f;
        countDodgeRollCooldown = false;

        visual = VisualChild.DodgeRoll;

        animationState = AnimationState.DodgeRoll;
        stateToChangeTo = new DodgeRollRecovery();
    }


    public override void Update(PlayerStatus status)
    {
        base.Update(status);

        status.movement.SetVelocityToMoveSpeedTimesFowardDirection();

        if (!status.movement.grounded && status.movement.wasGrounded)
        {
            status.SetPlayerStateImmediately(new DodgeRollFall());
        }
    }

    public override void OnExitThisState(BasicState nextState, PlayerStatus status)
    {
        base.OnExitThisState(nextState, status);

        if (nextState is DodgeRollFall)
        {
            status.ReduceStamina(status.dodgeRollStaminaDamage/2);
        }
        else
        {
            status.ReduceStamina(status.dodgeRollStaminaDamage);
        }


        status.movement.velocity = status.movement.velocity.normalized * status.movement.CurrentMoveSpeed;
    }

}
