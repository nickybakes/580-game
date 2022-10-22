using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public class Rest : BasicState
{
    //private float restStaminaIncreaseCooldown;
    //private float restStaminaIncreaseCooldownMax = 1f;

    private const float DefaultRestStaminaRegen = 7.0f;

    // MAINTAIN 2 to 1 Ratio
    private const float SpotlightRestStaminaRegen = 12.0f;
    private const float RestSpotlightLoss = 6.0f;

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

        if (status.spotlight > 0)
        {
            status.IncreaseStamina(SpotlightRestStaminaRegen * Time.deltaTime);
            status.ReduceSpotlightMeter(RestSpotlightLoss * Time.deltaTime);
        }
        else
        {
            status.IncreaseStamina(DefaultRestStaminaRegen * Time.deltaTime);
        }
    }

    public override void OnEnterThisState(BasicState prevState, PlayerStatus status)
    {
        base.OnEnterThisState(prevState, status);
    }
}
