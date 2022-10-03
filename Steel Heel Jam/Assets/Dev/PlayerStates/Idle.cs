using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Idle : BasicState
{


    public Idle()
    {
        timeToChangeState = 0;
        canPlayerControlMove = true;
        canPlayerControlRotate = true;
        updateMovement = true;
        canPickUp = true;
        animationState = AnimationState.Idle;
    }

}
