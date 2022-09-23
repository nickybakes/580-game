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
        animationState = AnimationState.AttackGroundStartup; // TODO: CHANGE THIS
        stateToChangeTo = new AttackGroundRecovery(recovery);
    }


    public override void Update(PlayerStatus status)
    {
        base.Update(status);

        if (changeStateNow)
        {
            status.combat.weaponState.Attack();
            status.movement.SetVelocityToMoveSpeedTimesFowardDirection(status.combat.weaponState.ForwardDisplacement);
        }
    }
}
