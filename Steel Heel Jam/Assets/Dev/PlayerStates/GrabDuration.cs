using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabDuration : BasicState
{
    public GrabDuration()
    {
        timeToChangeState = 0.3f;
        moveSpeedMultiplier = 1.5f;
        canPlayerControlMove = false;
        canPlayerControlRotate = false;
        canAttack = false;
        canDodgeRoll = false;
        canBlock = false;
        updateMovement = true;
        countAttackCooldown = false;
        animationState = AnimationState.GrabDuration_01;
        stateToChangeTo = new GrabRecovery();
    }

    public override void Update(PlayerStatus status)
    {
        base.Update(status);

        status.combat.grabHitbox.TrySuplex();

        status.movement.SetVelocityToMoveSpeedTimesFowardDirection();
    }

    // Give forward walk direction and speed.
    public override void OnEnterThisState(BasicState prevState, PlayerStatus status)
    {
        base.OnEnterThisState(prevState, status);

        // Set GrabHitbox active
        status.combat.grabHitbox.gameObject.SetActive(true);

        status.movement.SetVelocityToMoveSpeedTimesFowardDirection();

        status.combat.grabHitbox.TrySuplex();
    }

    public override void OnExitThisState(BasicState nextState, PlayerStatus status)
    {
        base.OnExitThisState(nextState, status);

        // Set GrabHitbox inactive
        status.combat.grabHitbox.gameObject.SetActive(false);
    }
}
