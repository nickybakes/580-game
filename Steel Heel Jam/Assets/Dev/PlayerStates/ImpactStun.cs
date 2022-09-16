using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImpactStun : BasicState
{


    public ImpactStun()
    {
        timeToChangingState = .2f;
        canPlayerControlMove = false;
        canPlayerControlRotate = false;
        canAttack = false;
        canDodgeRoll = false;
        canBlock = false;
        updateMovement = false;
        animationState = AnimationState.Knockback;
        stateToChangeTo = new Knockback();
    }

}
