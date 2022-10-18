using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public class Rest : BasicState
{
    private float restStaminaIncreaseCooldown;
    private float restStaminaIncreaseCooldownMax = 1f;

    // MAINTAIN 2 to 1 Ratio
    private const float RestStaminaRegen = 10.0f;
    private const float RestSpotlightLoss = 5.0f;

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

        //restStaminaIncreaseCooldown += Time.deltaTime;

        //if (restStaminaIncreaseCooldown >= restStaminaIncreaseCooldownMax)
        //{
        //    restStaminaIncreaseCooldown = 0;

        //    status.IncreaseStamina(RestStaminaRegen);
        //}

        if (status.spotlight > 0 || status.isInSpotlight)
        {
            status.IncreaseStamina(RestStaminaRegen * Time.deltaTime);
            status.ReduceSpotlightMeter(RestSpotlightLoss * Time.deltaTime);
        }
    }

    public override void OnEnterThisState(BasicState prevState, PlayerStatus status)
    {
        base.OnEnterThisState(prevState, status);
    }
}
