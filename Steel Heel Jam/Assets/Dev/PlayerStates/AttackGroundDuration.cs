using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackGroundDuration : BasicState
{
    public AttackGroundDuration()
    {
        canPlayerControlMove = false;
        canPlayerControlRotate = false;
        canAttack = false;
        canDodgeRoll = false;
        canBlock = false;
        updateMovement = true;
        countAttackCooldown = false;
        animationState = AnimationState.AG_Punch_03_D;
        stateToChangeTo = new AttackGroundRecovery();
    }

    private bool gotAHitAlready;


    public override void Update(PlayerStatus status)
    {
        base.Update(status);

        // if(!gotAHitAlready && status.combat.weaponState.gotAHit){
        //     gotAHitAlready = true;
        //     moveSpeedMultiplier = moveSpeedMultiplier * .3f;
        // }

        status.movement.SetVelocityToMoveSpeedTimesFowardDirection();
    }

    public override void OnEnterThisState(BasicState prevState, PlayerStatus status)
    {
        base.OnEnterThisState(prevState, status);
        status.combat.weaponState.Attack();
        status.movement.SetVelocityToMoveSpeedTimesFowardDirection();
    }

    public override void OnExitThisState(BasicState nextState, PlayerStatus status)
    {
        base.OnExitThisState(nextState, status);
        status.combat.weaponState.ForceEndAttack();
    }
}
