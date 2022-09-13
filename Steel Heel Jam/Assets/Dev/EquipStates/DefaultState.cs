using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EquipState
{
    DefaultState,
    TestCubeState
};

/// <summary>
/// The base class for Weapon States
/// </summary>
public class DefaultState
{
    //*********
    // Fields
    //*********
    protected int playerNumber;
    private float damage = 20;
    protected float damageMultiplier;
    private float knockback = 15;
    protected float knockbackMultiplier;
    private float radius = 5;
    protected float radiusMultiplier;
    private float startup = 0.25f; //TIME IS IN SECONDS
    protected float startupMultiplier;
    private float duration = 0.1f;
    protected float durationMultiplier;
    private float recovery = 0.15f;
    protected float recoveryMultiplier;
    private float forwardDisplacement = 1;
    protected float forwardDisplacementMultiplier;
    public int comboCount = 3;
    //private float backwardDisplacement;
    //protected float backwardDisplacementMultiplier;

    // NOTE MAKE COMBO SYSTEM
    // NOTE DEFINE PRIVATES

    public float Startup
    {
        get
        {
            return startup * startupMultiplier;
        }
    }

    public float Recovery
    {
        get
        {
            return recovery * recoveryMultiplier;
        }
    }

    //***************
    // Constructor
    //***************
    public DefaultState(int _playerNumber)
    {
        playerNumber = _playerNumber;
    }

    //**********
    // Methods
    //**********
    public virtual void Attack()
    {
        //TURN ON THE HITBOX
    }


}
