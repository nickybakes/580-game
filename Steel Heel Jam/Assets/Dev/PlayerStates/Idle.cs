using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Idle : BasicState
{

    public Idle()
    {
        timeToChangingState = 0;
        canPlayerMove = true;
        canPlayerSpin = true;
        animationState = AnimationState.Idle;
    }

}
