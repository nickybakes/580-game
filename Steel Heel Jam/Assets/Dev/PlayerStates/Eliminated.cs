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

    public override void OnEnterThisState(BasicState prevState, PlayerStatus status)
    {
        base.OnEnterThisState(prevState, status);

        //this 'disables' players colliding with the dead body
        status.gameObject.GetComponent<CharacterController>().radius = .17f;
        status.gameObject.GetComponent<CharacterController>().height = .5f;
        status.gameObject.GetComponent<CharacterController>().center =  new Vector3(0, .25f, 0);
    }
}
