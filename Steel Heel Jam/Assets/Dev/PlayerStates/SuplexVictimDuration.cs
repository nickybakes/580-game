using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuplexVictimDuration : BasicState
{
    public SuplexVictimDuration()
    {
        timeToChangeState = 0; // Until parent grounded.
        canPlayerControlMove = false;
        canPlayerControlRotate = false;
        canAttack = false;
        canDodgeRoll = false;
        canBlock = false;
        updateMovement = false;
        countAttackCooldown = false;
        isInvincibleToAttacks = true;
        isInvincibleToRing = true;

        animationState = AnimationState.SuplexVictimDuration_01;
        stateToChangeTo = new Idle();
    }

    public override void Update(PlayerStatus status)
    {
        base.Update(status);

        status.visuals.RotateModelFlyingThroughAir();
    }

    public override void OnExitThisState(BasicState nextState, PlayerStatus status)
    {
        base.OnExitThisState(nextState, status);

        status.gameObject.transform.parent = null;
        status.visuals.SetModelRotationX(0);

        status.ReduceStamina(status.suplexStaminaDamage);
    }
}
