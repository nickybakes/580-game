using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ImpactStun : BasicState
{

    private PlayerStatus attackingPlayer;

    private bool moveVictimWithAttacker;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="_attackingPlayer">the attacking player that caused this impact stun. use only for MELEE hits, otherwise just set to null</param>
    /// <param name="knockbackVelocity"></param>
    public ImpactStun(PlayerStatus _attackingPlayer, Vector3 knockbackVelocity, bool _moveVictimWithAttacker)
    {
        timeToChangeState = .2f;
        canPlayerControlMove = false;
        canPlayerControlRotate = false;
        canAttack = false;
        canDodgeRoll = false;
        canBlock = false;
        updateMovement = false;
        animationState = AnimationState.ImpactStun;
        stateToChangeTo = new Knockback(knockbackVelocity);
        moveVictimWithAttacker = _moveVictimWithAttacker;
        animationState = AnimationState.ImpactStun;

        visual = VisualChild.Stun;

        attackingPlayer = _attackingPlayer;
    }

    public override void OnEnterThisState(BasicState prevState, PlayerStatus status)
    {
        base.OnEnterThisState(prevState, status);
        status.movement.velocity = Vector3.zero;

        // If the attacking player has hit the ground with their elbow drop, play VO line.
        if (attackingPlayer.CurrentPlayerState.animationState == AnimationState.AttackAirRecovery_01 ||
            attackingPlayer.CurrentPlayerState.animationState == AnimationState.AttackAirDuration_01)
        {
            // If traveling fast enough downwards, plays VO line.
            if (-attackingPlayer.movement.velocity.y > 60)
            {
                AudioManager.aud.Play("elbowDrop");
            }
        }
    }

    public override void Update(PlayerStatus status)
    {
        base.Update(status);

        if (moveVictimWithAttacker && attackingPlayer != null && (attackingPlayer.CurrentPlayerState is AttackGroundDuration || attackingPlayer.CurrentPlayerState is AttackGroundRecovery))
            status.movement.velocity = attackingPlayer.movement.velocity;
        else
            status.movement.velocity = Vector3.zero;
    }

}
