using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackGroundRecovery : BasicState
{

    private float prevSpeedModifier;

    public AttackGroundRecovery()
    {
        canPlayerControlMove = false;
        canPlayerControlRotate = false;
        canAttack = false;
        canDodgeRoll = false;
        canBlock = false;
        updateMovement = true;
        countAttackCooldown = false;
        visual = VisualChild.Recovery;
        animationState = AnimationState.AttackGroundRecovery;
        stateToChangeTo = new Idle();
    }

    private bool canCombo;


    public override void Update(PlayerStatus status)
    {
        base.Update(status);

        //adds a very tiny cooldown at the end of attack duration and start of recovery
        if (timeInThisState < .1f && canCombo)
        {
            canAttack = true;
        }

        moveSpeedMultiplier = Mathf.Lerp(prevSpeedModifier, 0, timeInThisState / timeToChangeState);
        status.movement.SetVelocityToMoveSpeedTimesFowardDirection();
    }

    public override void OnEnterThisState(BasicState prevState, PlayerStatus status)
    {
        base.OnEnterThisState(prevState, status);

        prevSpeedModifier = prevState.moveSpeedMultiplier;

        if (status.combat.weaponState.CanCombo)
            canCombo = true;
    }

    public override void OnExitThisState(BasicState nextState, PlayerStatus status)
    {
        base.OnExitThisState(nextState, status);

        if (changeStateNow)
        {
            status.combat.weaponState.currentComboCount = 0;
            status.combat.attackCooldown = 0;
        }
    }
}
