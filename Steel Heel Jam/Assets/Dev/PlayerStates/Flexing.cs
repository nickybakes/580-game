using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public class Flexing : BasicState
{
    //private float restStaminaIncreaseCooldown;
    //private float restStaminaIncreaseCooldownMax = 1f;

    private const float DefaultRestStaminaRegen = 7.0f;

    // MAINTAIN 2 to 1 Ratio
    private const float SpotlightRestStaminaRegen = 12.0f;
    private const float RestSpotlightLoss = 6.0f;

    private const float spotlightFillRate = 20.0f;

    public Flexing()
    {
        timeToChangeState = 0; // Only change state on button release.
        canPlayerControlMove = false;
        canPlayerControlRotate = true;
        updateMovement = true;
        canAttack = true;
        canDodgeRoll = true;
        canBlock = true;
        alternateFriction = true;
        animationState = AnimationState.Taunt_01;
        visual = VisualChild.Knockback;

        stateToChangeTo = new Idle();
    }

    public override void Update(PlayerStatus status)
    {
        base.Update(status);

        if (status.spotlight > 0 && !status.isInSpotlight && status.stamina < PlayerStatus.defaultMaxStamina)
        {
            status.IncreaseStamina((SpotlightRestStaminaRegen + (status.buffs[(int)Buff.PlotArmor] == true ? status.plotArmorAdditionalHeal : 0)) * Time.deltaTime);
            status.ReduceSpotlightMeter(RestSpotlightLoss * Time.deltaTime);
        }
        else if (status.isInSpotlight)
        {
            status.IncreaseStamina((SpotlightRestStaminaRegen + (status.buffs[(int)Buff.PlotArmor] == true ? status.plotArmorAdditionalHeal : 0)) * Time.deltaTime);
            status.IncreaseSpotlightMeter(spotlightFillRate * Time.deltaTime);

            if (status.spotlight == 100)
            {
                status.GiveBuff();
            }
        }
        else if (status.spotlight == 0)
        {
            status.IncreaseStamina((DefaultRestStaminaRegen + (status.buffs[(int)Buff.PlotArmor] == true ? status.plotArmorAdditionalHeal : 0)) * Time.deltaTime);
        }
    }

    public override void OnEnterThisState(BasicState prevState, PlayerStatus status)
    {
        base.OnEnterThisState(prevState, status);

        AudioManager.aud.Play("flexing");
    }

    public override void OnExitThisState(BasicState nextState, PlayerStatus status)
    {
        base.OnExitThisState(nextState, status);

        AudioManager.aud.Stop("flexing");
    }
}
