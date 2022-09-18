using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : BasicState
{
    public Block()
    {
        Debug.Log("block");
        timeToChangingState = .25f;
        canPlayerControlMove = false;
        canPlayerControlRotate = false;
        canAttack = false;
        canDodgeRoll = false;
        canBlock = false;
        updateMovement = true;
        stateToChangeTo = new BlockRecovery();

        // For testing
        /*canPlayerControlMove = true;
        canPlayerControlRotate = true;
        canAttack = true;
        updateMovement = true;*/
    }
}
