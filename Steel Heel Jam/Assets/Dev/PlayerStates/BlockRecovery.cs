using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockRecovery : BasicState
{
    public BlockRecovery()
    {
        Debug.Log("block recovery");
        timeToChangingState = .15f;
        canPlayerControlMove = false;
        canPlayerControlRotate = false;
        canAttack = false;
        canDodgeRoll = false;
        canBlock = false;
        stateToChangeTo = new Idle();
    }
}
