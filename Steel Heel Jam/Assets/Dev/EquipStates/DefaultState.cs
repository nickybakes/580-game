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
    private float damage = 10;
    [SerializeField] protected float damageMultiplier = 1;
    private float knockback = 5;
    [SerializeField] protected float knockbackMultiplier = 1;
    private float knockbackHeight = 20;
    [SerializeField] protected float knockbackHeightMultiplier = 1;
    private float hitstun = .75f;
    [SerializeField] protected float hitstunMultiplier = 1;
    private float radius = 1;
    [SerializeField] protected float radiusMultiplier = 1;
    private float startup = 0.1f; //TIME IS IN SECONDS
    [SerializeField] protected float startupMultiplier = 1;
    private float duration = 0.2f;
    [SerializeField] protected float durationMultiplier = 1;
    private float recovery = 0.35f;
    [SerializeField] protected float recoveryMultiplier = 1;
    private float forwardSpeedModifier = 1.4f;
    [SerializeField] protected float forwardSpeedModifierMultiplier = 1;
    [SerializeField] public int maxComboCount = 3;
    public int currentComboCount = 0;
    public float staminaCost = 5f;
    //private float backwardDisplacement;
    //protected float backwardDisplacementMultiplier;

    [SerializeField] public GameObject hitbox;
    private Hitbox hitboxScript;
    private SphereCollider hitboxCollider;

    /// <summary>
    /// set this to true for when doing a single attack, we got a hit.
    /// this is used to tell the player's state to slow down their forward displacement to make the impact
    /// feel more IMPACTFUL!!
    /// </summary>
    public bool gotAHit;


    // NOTE MAKE COMBO SYSTEM
    // NOTE DEFINE PRIVATES
    // NOTE MOVE MULTIPLIERS TO CONSTRUCTOR

    public bool CanCombo
    {
        get
        {
            return currentComboCount < maxComboCount;
        }
    }

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

    public float ForwardSpeedModifier
    {
        get
        {
            return forwardSpeedModifier * forwardSpeedModifierMultiplier;
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

        SetupHitboxReferences(_hitbox);
    }

    //**********
    // Methods
    //**********
    protected void SetupHitboxReferences(GameObject _hitbox)
    {
        // Initialize hitbox references
        hitboxScript = hitbox.GetComponent<Hitbox>();
        hitboxCollider = hitbox.GetComponent<SphereCollider>();
    }

    protected virtual void SetInitialHit()
    {
        damageMultiplier = 1.0f;
        knockbackMultiplier = 1.0f;
        knockbackHeightMultiplier = 1.0f;
        hitstunMultiplier = 1.0f;
        radiusMultiplier = 1.0f;
        startupMultiplier = 1.0f;
        durationMultiplier = 1.0f;
        recoveryMultiplier = 1.0f;
    }

    public virtual void UpdateValues()
    {
        SetInitialHit();
    }

    /// <summary>
    /// Activates the hitbox prefab attached to the player.
    /// </summary>
    public virtual void Attack()
    {
        gotAHit = false;

        currentComboCount += 1;

        InitHitbox();

        hitbox.SetActive(true);
    }


    public virtual void AirAttack()
    {
        gotAHit = false;

        SetInitialHit();

        InitAirHitbox();

        hitbox.SetActive(true);
    }

    public virtual void ForceEndAttack()
    {
        hitbox.SetActive(false);
    }

    /// <summary>
    /// Sets the hitbox size and duration.
    /// </summary>
    /// <returns>A reference to the hitbox script.</returns>
    protected virtual Hitbox InitHitbox()
    {
        // Set up hitbox values
        hitboxScript.damage = damage * damageMultiplier;
        hitboxScript.knockback = knockback * knockbackMultiplier;
        hitboxScript.knockbackHeight = knockbackHeight * knockbackHeightMultiplier;
        hitboxScript.hitstun = hitstun * hitstunMultiplier;
        hitboxScript.radius = radius * radiusMultiplier; // Radius is only passed through for gizmo drawing
        hitboxScript.duration = duration * durationMultiplier;
        hitboxScript.playerNumber = playerNumber;
        //hitboxScript.duration = duration * durationMultiplier;

        // Resize hitbox
        hitboxCollider.radius = radius * radiusMultiplier;
        hitbox.transform.localPosition = new Vector3(0, 1, 1 + (radius * radiusMultiplier) / 2); // Experimental
        hitbox.transform.GetChild(0).localScale = new Vector3(hitboxCollider.radius * 2, hitboxCollider.radius * 2, hitboxCollider.radius * 2);

        return hitboxScript;
    }

    protected virtual Hitbox InitAirHitbox()
    {
        // Set up hitbox values
        hitboxScript.damage = damage * damageMultiplier * 1.5f;
        hitboxScript.knockback = knockback * knockbackMultiplier * 1.5f;
        hitboxScript.knockbackHeight = knockbackHeight * knockbackHeightMultiplier * 1.5f;
        hitboxScript.hitstun = hitstun * hitstunMultiplier * 1.5f;
        hitboxScript.radius = radius * radiusMultiplier * 5; // Radius is only passed through for gizmo drawing
        hitboxScript.duration = 100f;
        hitboxScript.playerNumber = playerNumber;

        // Resize hitbox
        hitboxCollider.radius = radius * radiusMultiplier * 5;
        hitbox.transform.localPosition = new Vector3(0, 0, 0);
        hitbox.transform.GetChild(0).localScale = new Vector3(hitboxCollider.radius, hitboxCollider.radius, hitboxCollider.radius);

        return hitboxScript;
    }
}
