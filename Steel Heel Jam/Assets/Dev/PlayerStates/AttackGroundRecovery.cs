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
        animationState = AnimationState.AttackGroundRecovery_01;
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

        moveSpeedMultiplier = Mathf.Lerp(prevSpeedModifier, 0, Mathf.Clamp01(timeInThisState / (timeToChangeState * 0.25f)));
        status.movement.SetVelocityToMoveSpeedTimesFowardDirection();
    }

    public override void OnEnterThisState(BasicState prevState, PlayerStatus status)
    {
        base.OnEnterThisState(prevState, status);

        prevSpeedModifier = prevState.moveSpeedMultiplier;

        if (status.combat.weaponState.CanCombo)
            canCombo = true;

        // Lose stamina if missed
        if (!status.combat.weaponState.gotAHit)
        {
            status.ReduceStamina(status.combat.weaponState.staminaCost);
        }
        // If hit and is heel, remove heel status
        else if (status.isHeel)
        {
            status.isHeel = false;

            if (status.playerHeader)
            {
                status.playerHeader.SetHeel(false);
            }

        }
    }

    public override void OnExitThisState(BasicState nextState, PlayerStatus status)
    {
        base.OnExitThisState(nextState, status);

        if (!(nextState is AttackGroundStartup))
        {
            status.combat.weaponState.currentComboCount = 0;
            status.combat.attackCooldown = 0;
        }
    }
}
