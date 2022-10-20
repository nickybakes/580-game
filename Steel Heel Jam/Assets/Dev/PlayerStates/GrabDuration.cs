using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabDuration : BasicState
{
    public GrabDuration()
    {
        timeToChangeState = 1.0f;
        moveSpeedMultiplier = 0.1f;
        canPlayerControlMove = false;
        canPlayerControlRotate = false;
        canAttack = false;
        canDodgeRoll = false;
        canBlock = false;
        updateMovement = true;
        countAttackCooldown = false;
        animationState = AnimationState.AttackGroundStartup_01;
        stateToChangeTo = new GrabRecovery();
    }

    public override void Update(PlayerStatus status)
    {
        base.Update(status);

        status.movement.SetVelocityToMoveSpeedTimesFowardDirection();
    }

    // Give forward walk direction and speed.
    public override void OnEnterThisState(BasicState prevState, PlayerStatus status)
    {
        base.OnEnterThisState(prevState, status);

        status.movement.SetTheSetForwardDirection();
        status.movement.SetVelocityToMoveSpeedTimesFowardDirection();

        status.combat.grabHitbox.TrySuplex();
    }
}
