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
    //**********
    // Fields
    //**********
    public int playerNumber;
    private float damage = 20;
    [SerializeField] protected float damageMultiplier = 1;
    private float knockback = 5;
    [SerializeField] protected float knockbackMultiplier = 1;
    private float knockbackHeight = 20;
    [SerializeField] protected float knockbackHeightMultiplier = 1;
    private float hitstun = .75f;
    [SerializeField] protected float hitstunMultiplier = 1;
    private float radius = 1;
    [SerializeField] protected float radiusMultiplier = 1;
    private float startup = 0.15f; //TIME IS IN SECONDS
    [SerializeField] protected float startupMultiplier = 1;
    private float duration = 0.15f;
    [SerializeField] protected float durationMultiplier = 1;
    private float recovery = 0.15f;
    [SerializeField] protected float recoveryMultiplier = 1;
    private float forwardDisplacement = 1;
    [SerializeField] protected float forwardDisplacementMultiplier = 1;
    [SerializeField] public int comboCount = 3;
    //private float backwardDisplacement;
    //protected float backwardDisplacementMultiplier;

    [SerializeField] public GameObject hitbox;
    private Hitbox hitboxScript;
    private SphereCollider hitboxCollider;

    // NOTE MAKE COMBO SYSTEM
    // NOTE DEFINE PRIVATES
    // NOTE MOVE MULTIPLIERS TO CONSTRUCTOR

    public float Startup
    {
        get
        {
            return startup * startupMultiplier;
        }
    }

    public float Duration
    {
        get
        {
            return duration * durationMultiplier;
        }
    }

    public float Recovery
    {
        get
        {
            return recovery * recoveryMultiplier;
        }
    }

    public float ForwardDisplacement
    {
        get
        {
            return forwardDisplacement * forwardDisplacementMultiplier;
        }
    }

    //***************
    // Constructor
    //***************
    /// <summary>
    /// Generates an instance of a DefaultState object.
    /// </summary>
    /// <param name="_playerNumber">The number of the controlling player.</param>
    /// <param name="_hitbox">A reference to the player's hitbox prefab.</param>
    public DefaultState(int _playerNumber, GameObject _hitbox)
    {
        playerNumber = _playerNumber;
        hitbox = _hitbox;

        // Initialize hitbox references
        hitboxScript = hitbox.GetComponent<Hitbox>();
        hitboxCollider = hitbox.GetComponent<SphereCollider>();
    }

    //**********
    // Methods
    //**********
    /// <summary>
    /// Activates the hitbox prefab attached to the player.
    /// </summary>
    public virtual void Attack()
    {
        hitboxScript.duration = duration * durationMultiplier;
        hitbox.SetActive(true);
    }

    /// <summary>
    /// Sets the hitbox size and duration. Must be called each time the equip state is changed.
    /// </summary>
    /// <returns>A reference to the hitbox script.</returns>
    public virtual Hitbox InitHitbox()
    {
        // Set up hitbox values
        hitboxScript.damage = damage * damageMultiplier;
        hitboxScript.knockback = knockback * knockbackMultiplier;
        hitboxScript.knockbackHeight = knockbackHeight * knockbackHeightMultiplier;
        hitboxScript.hitstun = hitstun * hitstunMultiplier;
        hitboxScript.radius = radius * radiusMultiplier; // Radius is only passed through for gizmo drawing
        hitboxScript.playerNumber = playerNumber;
        //hitboxScript.duration = duration * durationMultiplier;

        // Resize hitbox
        hitboxCollider.radius = radius * radiusMultiplier;
        hitbox.transform.localPosition = new Vector3(0, 1, 1 + (radius * radiusMultiplier) / 2); // Experimental

        return hitboxScript;
    }
}
