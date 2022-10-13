using System.Linq;
using UnityEngine;

public enum EquipState
{
    DefaultState,
    TestCubeState
};

public struct Attack
{
    public float damageMultiplier;
    public float knockbackMultiplier;
    public float knockbackHeightMultiplier;
    public float hitstunMultiplier;
    public float radiusMultiplier;
    public float heightMultiplier;
    public float startupMultiplier;
    public float durationMultiplier;
    public float recoveryMultiplier;
    public float forwardSpeedModifierMultiplier;

    /// <summary>
    /// Creates an instance of an Attack struct.
    /// </summary>
    public Attack(
        float _damageMultiplier,
        float _knockbackMultiplier,
        float _knockbackHeightMultiplier,
        float _hitstunMultiplier,
        float _radiusMultiplier,
        float _heightMultiplier,
        float _startupMultiplier,
        float _durationMultiplier,
        float _recoveryMultiplier,
        float _forwardSpeedModifierMultiplier
        )
    {
        damageMultiplier = _damageMultiplier;
        knockbackMultiplier = _knockbackMultiplier;
        knockbackHeightMultiplier = _knockbackHeightMultiplier;
        hitstunMultiplier = _hitstunMultiplier;
        radiusMultiplier = _radiusMultiplier;
        heightMultiplier = _heightMultiplier;
        startupMultiplier = _startupMultiplier;
        durationMultiplier = _durationMultiplier;
        recoveryMultiplier = _recoveryMultiplier;
        forwardSpeedModifierMultiplier = _forwardSpeedModifierMultiplier;
    }
}

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

    private float knockback = 5;

    private float knockbackHeight = 20;

    private float hitstun = .75f;

    private float radius = 1;

    private float height = 10;

    private float startup = 0.1f; //TIME IS IN SECONDS

    private float duration = 0.2f;

    private float recovery = 0.35f;

    private float forwardSpeedModifier = 1.4f;

    public int maxComboCount;
    public int currentComboCount = 0;
    protected Attack[] combo;
    public Attack currentAttack;
    public Attack airAttack;

    public float staminaCost = 5f;

    [SerializeField] public GameObject hitbox;
    private Hitbox hitboxScript;
    private CapsuleCollider hitboxCollider;

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
            return startup * currentAttack.startupMultiplier;
        }
    }

    public float Duration
    {
        get
        {
            return duration * currentAttack.durationMultiplier;
        }
    }

    public float Recovery
    {
        get
        {
            return recovery * currentAttack.recoveryMultiplier;
        }
    }

    public float ForwardSpeedModifier
    {
        get
        {
            return forwardSpeedModifier * currentAttack.forwardSpeedModifierMultiplier;
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
        InitializeAttacks();
    }

    //**********
    // Methods
    //**********
    protected void SetupHitboxReferences(GameObject _hitbox)
    {
        // Initialize hitbox references
        hitboxScript = hitbox.GetComponent<Hitbox>();
        hitboxCollider = hitbox.GetComponent<CapsuleCollider>();
    }

    protected virtual void InitializeAttacks()
    {
        combo = new Attack[]
        {
            new Attack(
                1.0f, // Damage Multiplier
                1.0f, // Knockback Multiplier
                1.0f, // Knockback Height Multiplier
                1.0f, // Hitstun Multiplier
                1.0f, // Radius Multiplier
                1.0f, // Height Multiplier
                1.0f, // Startup Multiplier
                1.0f, // Duration Multiplier
                1.0f, // Recovery Multiplier
                1.0f  // Forward Speed Multiplier
                )
        };

        maxComboCount = combo.Length;
    }

    protected virtual void InitializeAirAttack()
    {
        airAttack = new Attack(
            1.0f, // Damage Multiplier
                1.0f, // Knockback Multiplier
                1.0f, // Knockback Height Multiplier
                1.0f, // Hitstun Multiplier
                1.0f, // Radius Multiplier
                1.0f, // Height Multiplier
                1.0f, // Startup Multiplier
                1.0f, // Duration Multiplier
                1.0f, // Recovery Multiplier
                1.0f  // Forward Speed Multiplier
                );
    }

    //protected virtual void SetInitialHit()
    //{
    //    damageMultiplier = 1.0f;
    //    knockbackMultiplier = 1.0f;
    //    knockbackHeightMultiplier = 1.0f;
    //    hitstunMultiplier = 1.0f;
    //    radiusMultiplier = 1.0f;
    //    heightMultiplier = 1.0f;
    //    startupMultiplier = 1.0f;
    //    durationMultiplier = 1.0f;
    //    recoveryMultiplier = 1.0f;
    //}

    //public virtual void UpdateValues()
    //{
    //    SetInitialHit();
    //}

    /// <summary>
    /// Activates the hitbox prefab attached to the player.
    /// </summary>
    public virtual void Attack()
    {
        gotAHit = false;

        currentAttack = combo[currentComboCount];

        currentComboCount += 1;

        if (currentComboCount > maxComboCount) currentComboCount = 0;

        LoadHitbox();

        hitbox.SetActive(true);
    }


    public virtual void AirAttack()
    {
        gotAHit = false;

        LoadAirHitbox();

        hitbox.SetActive(true);
    }

    public virtual void ForceEndAttack()
    {
        hitbox.SetActive(false);
    }

    /// <summary>
    /// Sets the hitbox values.
    /// </summary>
    /// <returns>A reference to the hitbox script.</returns>
    protected virtual Hitbox LoadHitbox()
    {
        // Set up hitbox values
        hitboxScript.damage = damage * currentAttack.damageMultiplier;
        hitboxScript.knockback = knockback * currentAttack.knockbackMultiplier;
        hitboxScript.knockbackHeight = knockbackHeight * currentAttack.knockbackHeightMultiplier;
        hitboxScript.hitstun = hitstun * currentAttack.hitstunMultiplier;
        hitboxScript.radius = radius * currentAttack.radiusMultiplier; // Radius is only passed through for gizmo drawing
        hitboxScript.duration = duration * currentAttack.durationMultiplier;
        hitboxScript.playerNumber = playerNumber;

        // Resize hitbox
        hitboxCollider.radius = radius * currentAttack.radiusMultiplier;
        hitboxCollider.height = height * currentAttack.heightMultiplier;
        hitboxScript.tr.localPosition = new Vector3(0, 1, 1 + (radius * currentAttack.radiusMultiplier) / 2); // Experimental
        hitboxScript.tr.GetChild(0).localScale = new Vector3(hitboxCollider.radius * 2, hitboxCollider.height, hitboxCollider.radius * 2);

        return hitboxScript;
    }

    /// <summary>
    /// Sets the hitbox values for the air attack.
    /// </summary>
    /// <returns>A reference to the hitbox script.</returns>
    protected virtual Hitbox LoadAirHitbox()
    {
        // Set up hitbox values
        hitboxScript.damage = damage * airAttack.damageMultiplier;
        hitboxScript.knockback = knockback * airAttack.knockbackMultiplier;
        hitboxScript.knockbackHeight = knockbackHeight * airAttack.knockbackHeightMultiplier;
        hitboxScript.hitstun = hitstun * airAttack.hitstunMultiplier;
        hitboxScript.radius = radius * airAttack.radiusMultiplier; // Radius is only passed through for gizmo drawing
        hitboxScript.duration = duration * airAttack.durationMultiplier;
        hitboxScript.playerNumber = playerNumber;

        // Resize hitbox
        hitboxCollider.radius = radius * airAttack.radiusMultiplier * 5;
        hitboxCollider.height = hitboxCollider.radius * 2;
        hitboxScript.tr.localPosition = new Vector3(0, 0, 0);
        hitboxScript.tr.GetChild(0).localScale = new Vector3(hitboxCollider.radius * 2, hitboxCollider.height, hitboxCollider.radius * 2);

        return hitboxScript;
    }
}
