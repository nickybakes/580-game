using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public class Rest : BasicState
{
    private float restStaminaIncreaseCooldown;
    private float restStaminaIncreaseCooldownMax = 1f;

    private const float RestStaminaRegen = 10f;

    public Rest()
    {
        timeToChangeState = 0; // Only change state on button release.
        canPlayerControlMove = false;
        canPlayerControlRotate = false;
        updateMovement = true;
        canAttack = false;
        canDodgeRoll = false;
        canBlock = false;
        alternateFriction = true;
        animationState = AnimationState.Taunt_01;
        visual = VisualChild.Knockback;

        stateToChangeTo = new Idle();
    }

    public override void Update(PlayerStatus status)
    {
        base.Update(status);

        restStaminaIncreaseCooldown += Time.deltaTime;

        if (restStaminaIncreaseCooldown >= restStaminaIncreaseCooldownMax)
        {
            restStaminaIncreaseCooldown = 0;

            status.IncreaseStamina(RestStaminaRegen);
        }
    }

    public override void OnEnterThisState(BasicState prevState, PlayerStatus status)
    {
        base.OnEnterThisState(prevState, status);
    }
}
