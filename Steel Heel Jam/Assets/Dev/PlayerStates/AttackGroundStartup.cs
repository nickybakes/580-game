using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackGroundStartup : BasicState
{
    public AttackGroundStartup(float startup, float recovery, bool canCombo)
    {
        timeToChangingState = startup;
        canPlayerControlMove = false;
        canPlayerControlRotate = false;
        canAttack = false;
        canDodgeRoll = false;
        canBlock = false;
        updateMovement = false;
        moveSpeedMultiplier = 1.6f;
        extraFallGravityMultiplier = .8f;
        countDodgeRollCooldown = false;
        animationState = AnimationState.DodgeRoll; // TODO: CHANGE THIS
        stateToChangeTo = new AttackGroundRecovery(recovery, canCombo);
    }


    public override void Update(PlayerStatus status)
    {
        if (timeToChangingState != 0)
        {
            timeInThisState += Time.deltaTime;
            if (timeInThisState >= timeToChangingState)
            {
                status.combat.weaponState.Attack();
                status.movement.SetVelocityToMoveSpeedTimesFowardDirection(status.combat.weaponState.ForwardDisplacement);
                changeStateNow = true;
            }
        }
    }
}
