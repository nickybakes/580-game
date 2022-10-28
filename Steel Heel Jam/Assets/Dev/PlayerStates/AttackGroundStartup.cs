using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackGroundStartup : BasicState
{
    public AttackGroundStartup()
    {
        canPlayerControlMove = false;
        canPlayerControlRotate = false;
        canAttack = false;
        canDodgeRoll = false;
        canBlock = false;
        updateMovement = false;
        countAttackCooldown = false;
        animationState = AnimationState.AG_Punch_03_S;
        stateToChangeTo = new AttackGroundDuration();
    }

    public override void OnEnterThisState(BasicState prevState, PlayerStatus status)
    {
        base.OnEnterThisState(prevState, status);

        status.combat.PauseWeaponAnimationModifier();
    }

    public override void OnExitThisState(BasicState nextState, PlayerStatus status)
    {
        base.OnExitThisState(nextState, status);

        if (!(nextState is AttackGroundDuration))
        {
            status.combat.weaponState.currentComboCount = 0;
            status.combat.attackCooldown = 0;
            status.combat.ResumeWeaponAnimationModifier();
        }
    }
}
