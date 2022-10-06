using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImpactStun : BasicState
{

    private PlayerStatus attackingPlayer;

/// <summary>
/// 
/// </summary>
/// <param name="_attackingPlayer">the attacking player that caused this impact stun. use only for MELEE hits, otherwise just set to null</param>
/// <param name="knockbackVelocity"></param>
    public ImpactStun(PlayerStatus _attackingPlayer, Vector3 knockbackVelocity)
    {
        timeToChangeState = .2f;
        canPlayerControlMove = false;
        canPlayerControlRotate = false;
        canAttack = false;
        canDodgeRoll = false;
        canBlock = false;
        updateMovement = false;
        animationState = AnimationState.Knockback;
        stateToChangeTo = new Knockback(knockbackVelocity);

        animationState = AnimationState.ImpactStun;

        visual = VisualChild.Stun;

        attackingPlayer = _attackingPlayer;
    }

    public override void OnEnterThisState(BasicState prevState, PlayerStatus status)
    {
        base.OnEnterThisState(prevState, status);
        status.movement.velocity = Vector3.zero;
    }

    public override void Update(PlayerStatus status)
    {
        base.Update(status);

        if (attackingPlayer != null && (attackingPlayer.CurrentPlayerState is AttackGroundDuration || attackingPlayer.CurrentPlayerState is AttackGroundRecovery))
            status.movement.velocity = attackingPlayer.movement.velocity;
        else
            status.movement.velocity = Vector3.zero;
    }

}
