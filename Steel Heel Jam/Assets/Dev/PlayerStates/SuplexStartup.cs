using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;

public class SuplexStartup : BasicState
{
    public SuplexStartup()
    {
        timeToChangeState = 1.0f;
        canPlayerControlMove = false;
        canPlayerControlRotate = false;
        canAttack = false;
        canDodgeRoll = false;
        canBlock = false;
        updateMovement = true;
        countAttackCooldown = false;
        animationState = AnimationState.AttackGroundStartup_01;
        stateToChangeTo = new SuplexMissRecovery();
    }

    // Give forward walk direction and speed.
    public override void OnEnterThisState(BasicState prevState, PlayerStatus status)
    {
        base.OnEnterThisState(prevState, status);

        //status.movement.SetVelocityToMoveSpeedTimesFowardDirection();
    }
}
