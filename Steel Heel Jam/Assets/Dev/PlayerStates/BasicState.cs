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
    AttackGroundStartup,
    AttackGroundDuration,
    AttackGroundRecovery,
    Knockback,
    Eliminated,
}

//public enum PlayerState
//{
//    Idle,
//    Run,
//    Jump,
//    Fall,
//    DodgeRoll,
//    ImpactStun,
//    HitStun,
//    Knockback,
//}

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
