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

    private float timeInThisState;

    public bool updateMovement = true;

    public bool canPlayerControlMove = true;

    public bool canPlayerControlRotate = true;

    public float moveSpeedMultiplier = 1;

    public bool canAttack = true;

    public bool canDodgeRoll = true;

    public bool canBlock = true;

    public AnimationState animationState = AnimationState.Idle;

    public BasicState stateToChangeTo;

    public bool changeStateNow;

    public bool grounded;
    public bool wasGrounded;

    /// <summary>
    /// The 'custom' arrays are for specific info that a specific state might need. 
    /// </summary>
    // public bool[] customBools;
    // public float[] customFloats;
    // public int[] customInts;

    public BasicState()
    {
        //if we need to set anything up for states to work, we can do it here
        //i used to put the default values for fields in here, but i moved that to above so
        //we could use base call of constructor if we need to set things up
        //such as initialize array, get references to stuff, etc
    }

    public virtual void Update()
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

    /// <summary>
    /// Immediately goes to the "stateToChangeTo" state
    /// </summary>
    public void InterruptState()
    {
        changeStateNow = true;
    }

    /// <summary>
    /// Immediately goes to state within the parameter
    /// </summary>
    /// <param name="state">The state to immediately go to</param>
    public void InterruptState(BasicState state)
    {
        stateToChangeTo = state;
        changeStateNow = true;
    }
}
