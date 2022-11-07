using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackAirDuration : BasicState
{

    public AttackAirDuration()
    {
        canPlayerControlMove = true;
        canPlayerControlRotate = true;
        canAttack = false;
        canDodgeRoll = false;
        canBlock = false;
        updateMovement = true;
        countAttackCooldown = false;
        extraFallGravityMultiplier = 2f;
        animationState = AnimationState.AttackAirDuration_01;
        visual = VisualChild.Attack;
        stateToChangeTo = new AttackAirRecovery();
    }

    public override void Update(PlayerStatus status)
    {
        base.Update(status);

        if (status.movement.grounded) status.SetPlayerStateImmediately(stateToChangeTo);
    }

    public override void OnEnterThisState(BasicState prevState, PlayerStatus status)
    {
        base.OnEnterThisState(prevState, status);

        status.movement.velocity.y -= 8f;

        status.combat.weaponState.AirAttack();
    }

    public override void OnExitThisState(BasicState nextState, PlayerStatus status)
    {
        base.OnExitThisState(nextState, status);

        status.combat.weaponState.ForceEndAttack();

        if (nextState is AttackAirRecovery)
        {
            ((AttackAirRecovery)nextState).velocityYAirAttack = -status.movement.velocity.y;
            Debug.Log(-status.movement.velocity.y);
            // If traveling fast enough downwards, creates a decal and shakes camera.
            if (-status.movement.velocity.y > 50)
            {
                CameraManager.cam.ShakeCamera(Mathf.Clamp01((-status.movement.velocity.y - 41f) / 70f) * .6f);
            }

            GameObject decal = VisualsManager.SpawnDecal(DecalName.Crack_01, status.transform.position);

            float scale = Mathf.Clamp((-status.movement.velocity.y - 36f) / 53f, .3f, 1);
            decal.transform.localScale = new Vector3(scale, scale, 1);

            GameObject particle = VisualsManager.SpawnParticle(ParticleName.AirAttackLand_01, status.transform.position);

            float scale2 = Mathf.Clamp((-status.movement.velocity.y - 39f) / 53f, .5f, 2f);
            particle.transform.localScale = new Vector3(scale2, scale2, scale2);

            // Play ground sound. Louder when moving faster.
            float vol = -status.movement.velocity.y / 75;

            AudioManager.aud.Play("crunch", vol / 3);
            AudioManager.aud.Play("punch", vol);
            AudioManager.aud.Play("landing", vol / 2);
        }


    }
}
