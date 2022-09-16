using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knockback : BasicState
{


    public Knockback()
    {
        timeToChangingState = 0;
        canPlayerControlMove = false;
        canPlayerControlRotate = false;
        canAttack = false;
        canDodgeRoll = false;
        canBlock = false;
        updateMovement = true;
        animationState = AnimationState.Knockback;
        stateToChangeTo = new Idle();
    }

}
