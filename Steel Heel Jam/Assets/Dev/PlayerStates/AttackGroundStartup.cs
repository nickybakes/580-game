using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackGroundStartup : BasicState
{
    public AttackGroundStartup(float startup, float recovery)
    {
        timeToChangingState = startup;
        canPlayerControlMove = false;
        canPlayerControlRotate = false;
        canAttack = false;
        canDodgeRoll = false;
        canBlock = false;
        updateMovement = false;
        countAttackCooldown = false;
        animationState = AnimationState.AttackGroundStartup;
        stateToChangeTo = new AttackGroundRecovery(recovery);
    }

    public override void OnExitThisState(BasicState nextState, PlayerStatus status)
    {
        status.combat.weaponState.Attack();
        status.movement.SetVelocityToMoveSpeedTimesFowardDirection(status.combat.weaponState.ForwardDisplacement);

        if (!status.combat.weaponState.CanCombo)
        {
            status.combat.attackCooldown = 0;
        }
    }
}
