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
    Knockback,
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
    /// awdhawfadawo
    /// </summary>
    public float timeToChangingState = 0;

    protected float timeInThisState;

    public bool updateMovement = true;

    public bool canPlayerControlMove = true;

    public bool canPlayerControlRotate = true;

    public float moveSpeedMultiplier = 1;

    public float extraFallGravityMultiplier = 1;

    public bool canAttack = true;

    public bool canDodgeRoll = true;

    /// <summary>
    /// True if we should increment our block cool down timer while in this state.
    /// Basically, only have this be true if this state is not related to blocking
    /// </summary>
    public bool countBlockdown = true;

    /// <summary>
    /// True if we should increment our dodge roll cool down timer while in this state.
    /// Basically, only have this be true if this state is not related to DodgeRolling
    /// </summary>
    public bool countDodgeRollCooldown = true;

    public bool canBlock = true;

    public AnimationState animationState = AnimationState.Idle;

    public BasicState stateToChangeTo;

    public bool changeStateNow;

    public BasicState()
    {
        //if we need to set anything up for states to work, we can do it here
        //i used to put the default values for fields in here, but i moved that to above so
        //we could use base call of constructor if we need to set things up
        //such as initialize array, get references to stuff, etc
    }

    public virtual void Update(PlayerStatus status)
    {
        if (timeToChangingState != 0)
        {
            timeInThisState += Time.deltaTime;
            if (timeInThisState >= timeToChangingState)
            {
                changeStateNow = true;
            }
        }
    }
}
