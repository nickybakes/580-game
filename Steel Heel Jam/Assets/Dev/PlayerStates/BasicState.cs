using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AnimationState {
    Idle,
    Run,
    Jump,
    Fall,
    DodgeRoll
}

public class BasicState
{
    /// <summary>
    /// awdhawfadawo
    /// </summary>
    public float timeToChangingState;

    private float timeInThisState;

    public bool canPlayerMove;
    
    public bool canPlayerSpin;

    public AnimationState animationState; 

    public BasicState(){
        timeToChangingState = 0;
        canPlayerMove = true;
        canPlayerSpin = true;
        animationState = AnimationState.Idle;
    }

    public virtual void Update(){
        if(timeToChangingState != 0){
            timeInThisState += Time.deltaTime;
            if(timeInThisState >= timeToChangingState){
                //change state
            }
        }
    }
}
