using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackAirDuration : BasicState
{
    public AttackAirDuration()
    {
        canPlayerControlMove = true;
        canPlayerControlRotate = true;
        canAttack = false;
        canDodgeRoll = false;
        canBlock = false;
        updateMovement = true;
        countAttackCooldown = false;
        extraFallGravityMultiplier = 2f;
        animationState = AnimationState.AttackAirDuration_01;
        stateToChangeTo = new AttackAirRecovery();
    }

    public override void Update(PlayerStatus status)
    {
        base.Update(status);

        if (status.movement.grounded) status.SetPlayerStateImmediately(stateToChangeTo);
    }

    public override void OnEnterThisState(BasicState prevState, PlayerStatus status)
    {
        base.OnEnterThisState(prevState, status);

        status.movement.velocity.y -= 10f;

        status.combat.weaponState.AirAttack();
    }

    public override void OnExitThisState(BasicState nextState, PlayerStatus status)
    {
        base.OnExitThisState(nextState, status);

        status.combat.weaponState.ForceEndAttack();
    }
}
