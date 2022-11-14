using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockedStun : BasicState
{
    /// <summary>
    /// This is the state a player will enter when their attack is blocked
    /// </summary>
    // Start is called before the first frame update
    public BlockedStun()
    {
        timeToChangeState = 1f;
        canPlayerControlMove = false;
        canPlayerControlRotate = false;
        canAttack = false;
        canDodgeRoll = false;
        canBlock = false;

        visual = VisualChild.Stun;

        animationState = AnimationState.ImpactStun;

        stateToChangeTo = new Idle();
    }

    public override void OnEnterThisState(BasicState prevState, PlayerStatus status)
    {
        base.OnEnterThisState(prevState, status);

        // Play sfx and VO line for player who blocked.
        AudioManager.aud.Play("blockedPunch");
        AnnouncerManager.PlayLine("block", Priority.Block);
    }
}
