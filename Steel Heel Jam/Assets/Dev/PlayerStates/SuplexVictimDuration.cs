using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuplexVictimDuration : BasicState
{
    public SuplexVictimDuration()
    {
        timeToChangeState = 0; // Until grounded. (or parent grounded?)
        canPlayerControlMove = false;
        canPlayerControlRotate = false;
        canAttack = false;
        canDodgeRoll = false;
        canBlock = false;
        updateMovement = false; // False for now.
        countAttackCooldown = false;
        animationState = AnimationState.Knockback; // Will be Suplexed
        stateToChangeTo = new Idle();

        // Set to different layer / temporarily disable collisions.
    }

    public override void OnExitThisState(BasicState nextState, PlayerStatus status)
    {
        base.OnExitThisState(nextState, status);

        status.gameObject.transform.parent = null;
    }
}
