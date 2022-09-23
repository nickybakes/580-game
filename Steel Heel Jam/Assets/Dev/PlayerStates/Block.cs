using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : BasicState
{
    public Block()
    {
        timeToChangeState = .25f;
        canPlayerControlMove = false;
        canPlayerControlRotate = false;
        canAttack = false;
        canDodgeRoll = false;
        canBlock = false;
        updateMovement = true;
        countBlockCooldown = false;
        stateToChangeTo = new BlockRecovery();

        visual = VisualChild.Block;

        // For testing
        /*canPlayerControlMove = true;
        canPlayerControlRotate = true;
        canAttack = true;
        updateMovement = true;*/
    }
}
