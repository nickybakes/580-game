using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabDuration : BasicState
{
    private float angle = 0.7f;
    private float dist = 10;

    public GrabDuration()
    {
        timeToChangeState = 0.3f;
        moveSpeedMultiplier = 1.5f;
        canPlayerControlMove = false;
        canPlayerControlRotate = false;
        canAttack = false;
        canDodgeRoll = false;
        canBlock = false;
        updateMovement = true;
        countAttackCooldown = false;
        animationState = AnimationState.GrabDuration_01;
        stateToChangeTo = new GrabRecovery();
    }

    public override void Update(PlayerStatus status)
    {
        base.Update(status);

        status.combat.grabHitbox.TrySuplex();

        status.movement.SetVelocityToMoveSpeedTimesFowardDirection();
    }

    // Give forward walk direction and speed.
    public override void OnEnterThisState(BasicState prevState, PlayerStatus status)
    {
        base.OnEnterThisState(prevState, status);

        // Set GrabHitbox active
        status.combat.grabHitbox.gameObject.SetActive(true);

        List<PlayerStatus> potentialTargets = status.combat.ReturnPotentialTargets(angle, dist);

        // If there is someone in front of the player in the angle, adjust the players rotation
        if (potentialTargets.Count != 0)
        {
            Vector3 newForwardDirection = (potentialTargets[0].transform.position - status.transform.position).normalized;
            status.transform.forward = newForwardDirection;
            status.movement.SetTheSetForwardDirection(); // Set new forward direction.
        }

        status.movement.SetVelocityToMoveSpeedTimesFowardDirection();

        status.combat.grabHitbox.TrySuplex();

        AudioManager.aud.Play("grab", 0.8f, 1.2f);
    }

    public override void OnExitThisState(BasicState nextState, PlayerStatus status)
    {
        base.OnExitThisState(nextState, status);

        // Reduce stamina for missed grab.
        if (nextState is GrabRecovery)
            status.ReduceStamina(status.missedGrabStaminaDamage);

        // Set GrabHitbox inactive
        status.combat.grabHitbox.gameObject.SetActive(false);
    }
}
