using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eliminated : BasicState
{
    public Eliminated()
    {
        canPlayerControlMove = false;
        canPlayerControlRotate = false;
        canAttack = false;
        canDodgeRoll = false;
        canBlock = false;
        updateMovement = true;

        animationState = AnimationState.Eliminated;
    }
}
