using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AnimationState
{
    Idle,
    Run,
    Jump,
    Fall,
    DodgeRoll,
    DodgeRollFall,
    DodgeRollRecovery,
    Block,
    BlockRecovery,
    Knockback,
    Eliminated,
    ImpactStun,
    Pickup_01,
    Throw_01,
    ThrowCharging_01,
    Taunt_01,
    GrabStartup_01,
    GrabDuration_01,
    SuplexStartup_01,
    SuplexDuration_01,
    SuplexRecovery_01,
    SuplexVictimStartup_01,
    SuplexVictimDuration_01,
    AttackAirStartup_01,
    AttackAirDuration_01,
    AttackAirRecovery_01,
    AG_Punch_01_S, //ID = 26
    AG_Punch_01_D,
    AG_Punch_01_R,
    AG_Punch_02_S,
    AG_Punch_02_D,
    AG_Punch_02_R,
    AG_Punch_03_S,
    AG_Punch_03_D,
    AG_Punch_03_R,
    AG_Swipe_01_S,
    AG_Swipe_01_D,
    AG_Swipe_01_R,
    AG_Swipe_02_S,
    AG_Swipe_02_D,
    AG_Swipe_02_R,
    AG_Stab_01_S,
    AG_Stab_01_D,
    AG_Stab_01_R,
    AG_HeavySwipe_01_S,
    AG_HeavySwipe_01_D,
    AG_HeavySwipe_01_R,
    AG_HeavySmash_01_S,
    AG_HeavySmash_01_D,
    AG_HeavySmash_01_R,
}

/// <summary>
/// The base class for Player States
/// </summary>
public class BasicState
{
    /// <summary>
    /// The amount of time to stay in this state before automatically changing to the next state. 0 means no limit
    /// </summary>
    public float timeToChangeState = 0;

    protected float timeInThisState;

    public bool updateMovement = true;
    public bool alternateFriction = false;

    public bool canPlayerControlMove = true;

    public bool canPlayerControlRotate = true;

    public float moveSpeedMultiplier = 1;

    public float extraFallGravityMultiplier = 1;

    public bool canAttack = true;

    public bool canDodgeRoll = true;

    /// <summary>
    /// True if we should increment our attack cool down timer while in this state.
    /// Basically, only have this be true if this state is not related to attacking
    /// </summary>
    public bool countAttackCooldown = true;

    /// <summary>
    /// True if we should increment our block cool down timer while in this state.
    /// Basically, only have this be true if this state is not related to blocking
    /// </summary>
    public bool countBlockCooldown = true;

    /// <summary>
    /// True if we should increment our dodge roll cool down timer while in this state.
    /// Basically, only have this be true if this state is not related to DodgeRolling
    /// </summary>
    public bool countDodgeRollCooldown = true;

    public bool canBlock = true;

    /// <summary>
    /// False for all but idle.
    /// (this way it doesn't need to be explicitly stated in every status.)
    /// </summary>
    public bool canPickUp = false;

    public bool canThrow = false;

    public bool isInvincibleToAttacks = false;
    public bool isInvincibleToRing = false;


    public AnimationState animationState = AnimationState.Idle;

    public BasicState stateToChangeTo;

    public bool changeStateNow;

    public VisualChild visual = VisualChild.None;

    public BasicState()
    {
        //if we need to set anything up for states to work, we can do it here
        //i used to put the default values for fields in here, but i moved that to above so
        //we could use base call of constructor if we need to set things up
        //such as initialize array, get references to stuff, etc
    }

    public virtual void Update(PlayerStatus status)
    {
        if (timeToChangeState != 0)
        {
            timeInThisState += Time.deltaTime;
            if (timeInThisState >= timeToChangeState)
            {
                changeStateNow = true;
            }
        }
    }

    /// <summary>
    /// This is called when we change TO this state
    /// </summary>
    /// <param name="nextState">the previous state we were in before this one</param>
    public virtual void OnEnterThisState(BasicState prevState, PlayerStatus status)
    {

    }

    /// <summary>
    /// This is called when we change FROM this state
    /// </summary>
    /// <param name="nextState">the state to come after this one</param>
    public virtual void OnExitThisState(BasicState nextState, PlayerStatus status)
    {

    }


}
