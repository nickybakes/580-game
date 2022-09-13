using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Idle : BasicState
{

    public Idle()
    {
        timeToChangingState = 0;
        canPlayerControlMove = true;
        canPlayerControlRotate = true;
        animationState = AnimationState.Idle;
    }

}
