using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockRecovery : BasicState
{
    public BlockRecovery()
    {
        timeToChangeState = .7f;
        canPlayerControlMove = true;
        canPlayerControlRotate = true;
        moveSpeedMultiplier = .1f;
        canAttack = false;
        canDodgeRoll = false;
        canBlock = false;
        updateMovement = true;
        countBlockCooldown = false;
        stateToChangeTo = new Idle();
        visual = VisualChild.Recovery;

    }

    public override void Update(PlayerStatus status)
    {
        base.Update(status);

        moveSpeedMultiplier = Mathf.Clamp(Mathf.Lerp(-.5f, 1, timeInThisState/timeToChangeState), 0, 1); 

    }
}
