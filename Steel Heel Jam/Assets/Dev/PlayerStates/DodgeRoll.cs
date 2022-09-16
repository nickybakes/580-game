using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DodgeRoll : BasicState
{


    public DodgeRoll()
    {
        timeToChangingState = .3f;
        canPlayerControlMove = false;
        canPlayerControlRotate = false;
        canAttack = false;
        canDodgeRoll = false;
        canBlock = false;
        updateMovement = true;
        moveSpeedMultiplier = 1.6f;
        animationState = AnimationState.DodgeRoll;
        stateToChangeTo = new Knockback();
    }

    
    public override void Update() {
        base.Update();

        if(!grounded && wasGrounded) {
            InterruptState(new Idle());
        }
    }

}
