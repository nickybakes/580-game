using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DodgeRoll : BasicState
{
    public DodgeRoll()
    {
        timeToChangeState = .45f;
        canPlayerControlMove = false;
        canPlayerControlRotate = false;
        canAttack = false;
        canDodgeRoll = false;
        canBlock = false;
        updateMovement = true;
        moveSpeedMultiplier = 1.8f;
        extraFallGravityMultiplier = .8f;
        countDodgeRollCooldown = false;
        isInvincibleToAttacks = true;
        botPickStrategyOnExit = true;

        visual = VisualChild.DodgeRoll;

        animationState = AnimationState.DodgeRoll;
        stateToChangeTo = new DodgeRollRecovery();
    }


    public override void Update(PlayerStatus status)
    {
        base.Update(status);

        status.movement.SetVelocityToMoveSpeedTimesFowardDirection();

        if (!status.movement.grounded && status.movement.wasGrounded && timeToChangeState - timeInThisState >= .1f)
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

    public override void OnEnterThisState(BasicState prevState, PlayerStatus status)
    {
        base.OnEnterThisState(prevState, status);

        if (status.buffs[(int)Buff.SpeedySubversion])
        {
            moveSpeedMultiplier *= 2.0f;
            timeToChangeState /= 2.0f;

            AudioManager.aud.Play("roll", 1.5f, 1.5f); // Higher pitched for faster rolling.
        }
        else
        {
            AudioManager.aud.Play("roll");
        }
    }

}
