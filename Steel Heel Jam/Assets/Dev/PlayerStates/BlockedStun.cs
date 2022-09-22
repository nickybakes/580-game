using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockedStun : BasicState
{
    /// <summary>
    /// This is the state a player will enter when their attack is blocked
    /// </summary>
    // Start is called before the first frame update
    public BlockedStun()
    {
        Debug.Log("block recovery");
        timeToChangingState = 1f;
        canPlayerControlMove = false;
        canPlayerControlRotate = false;
        canAttack = false;
        canDodgeRoll = false;
        canBlock = false;
        stateToChangeTo = new Idle();
    }
}
