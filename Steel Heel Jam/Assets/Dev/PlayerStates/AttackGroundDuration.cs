using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackGroundDuration : BasicState
{
    private float angle = 0.8f;
    private float dist = 10;

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

        List<PlayerStatus> potentialTargets = status.combat.ReturnPotentialTargets(angle, dist);

        // If there is someone in front of the player in the angle, adjust the players rotation
        if (potentialTargets.Count != 0)
        {
            Vector3 newForwardDirection = (
                potentialTargets[0].transform.position - status.transform.position
            ).normalized;
            status.transform.forward = newForwardDirection;
            status.movement.SetTheSetForwardDirection(); // Set new forward direction.
        }

        status.combat.weaponState.Attack();
        status.movement.SetVelocityToMoveSpeedTimesFowardDirection();

        status.audioManager.Play("swing", 0.8f, 1.2f);
    }

    public override void OnExitThisState(BasicState nextState, PlayerStatus status)
    {
        base.OnExitThisState(nextState, status);
        status.combat.weaponState.ForceEndAttack();
        if (!(nextState is AttackGroundRecovery))
        {
            status.combat.weaponState.currentComboCount = 0;
            status.combat.attackCooldown = 0;
            status.combat.ResumeWeaponAnimationModifier();
        }
    }
}
