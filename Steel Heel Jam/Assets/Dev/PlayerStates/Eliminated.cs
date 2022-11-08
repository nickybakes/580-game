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
        alternateFriction = true;

        animationState = AnimationState.Eliminated;
    }

    public override void Update(PlayerStatus status)
    {
        base.Update(status);
        status.visuals.SetAnimationState(AnimationState.Eliminated);
    }
}
