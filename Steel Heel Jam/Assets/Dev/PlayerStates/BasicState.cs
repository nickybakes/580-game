using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AnimationState {
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
    public float timeToChangingState;

    private float timeInThisState;

    public bool updateMovement;

    public bool canPlayerControlMove;
    
    public bool canPlayerControlRotate;

    public AnimationState animationState;

    public BasicState stateToChangeTo;

    public bool changeStateNow;

    public BasicState(){
        timeToChangingState = 0;
        updateMovement = true;
        canPlayerControlMove = true;
        canPlayerControlRotate = true;
        animationState = AnimationState.Idle;
    }

    public virtual void Update(){
        if(timeToChangingState != 0){
            timeInThisState += Time.deltaTime;
            if(timeInThisState >= timeToChangingState){
                changeStateNow = true;
            }
        }
    }
}
