using System.Linq;
using UnityEngine;

public enum AnimationModifier
{
    None,
    CarryOverHead,
    RightHandFist,
    FistsOverHead,
};

public enum AttackAnimation
{
    Punch_01,
    Punch_02,
    Punch_03,
    Swipe_01,
    Swipe_02,
    Stab_01,
    SwipeHeavy_01,
    SmashHeavy_01
}

public enum AttackParticle
{
    LeftHook,
    RightHook,
    Smash,
    Thrust,
    AirAttack,
    AnimationDefault = -1
}

public enum AttackDirection
{
    Horizontal = 0,
    Vertical = 1,
    Forward = 2
}

public struct Attack
{
    public float damageMultiplier;
    public float knockbackMultiplier;
    public float knockbackHeightMultiplier;
    public float timeInKnockbackMultiplier;
    public float radiusMultiplier;
    public float heightMultiplier;
    public float startupMultiplier;
    public float durationMultiplier;
    public float recoveryMultiplier;
    public float forwardSpeedModifierMultiplier;
    public AttackAnimation animation;
    public AttackDirection attackDirection;
    public AttackParticle attackParticle;

    /// <summary>
    /// Creates an instance of an Attack struct.
    /// </summary>
    public Attack(
        float _damageMultiplier = 1.0f,
        float _knockbackMultiplier = 1.0f,
        float _knockbackHeightMultiplier = 1.0f,
        float _timeInKnockbackMultiplier = 1.0f,
        float _radiusMultiplier = 1.0f,
        float _heightMultiplier = 1.0f,
        float _startupMultiplier = 1.0f,
        float _durationMultiplier = 1.0f,
        float _recoveryMultiplier = 1.0f,
        float _forwardSpeedModifierMultiplier = 1.0f,
        AttackAnimation _animation = AttackAnimation.Punch_03,
        AttackDirection _attackDirection = AttackDirection.Forward,
        AttackParticle _attackParticle = AttackParticle.AnimationDefault
        )
    {
        damageMultiplier = _damageMultiplier;
        knockbackMultiplier = _knockbackMultiplier;
        knockbackHeightMultiplier = _knockbackHeightMultiplier;
        timeInKnockbackMultiplier = _timeInKnockbackMultiplier;
        radiusMultiplier = _radiusMultiplier;
        heightMultiplier = _heightMultiplier;
        startupMultiplier = _startupMultiplier;
        durationMultiplier = _durationMultiplier;
        recoveryMultiplier = _recoveryMultiplier;
        forwardSpeedModifierMultiplier = _forwardSpeedModifierMultiplier;
        attackDirection = _attackDirection;
        animation = _animation;
        attackParticle = _attackParticle;
    }
}

/// <summary>
/// The base class for Weapon States
/// </summary>
public class DefaultState
{
    public static AttackParticle[] defaultAttackParticles = {AttackParticle.LeftHook, AttackParticle.RightHook, AttackParticle.LeftHook, AttackParticle.LeftHook, AttackParticle.RightHook, AttackParticle.Thrust, AttackParticle.LeftHook, AttackParticle.Smash};

    //**********
    // Fields
    //**********
    public int playerNumber;
    private float damage = 10;

    private float knockback = 5;

    private float knockbackHeight = 20;

    private float timeInKnockback = .75f;

    private float radius = 1;

    private float height = 0.5f;

    private float startup = 0.1f; //TIME IS IN SECONDS

    private float duration = 0.2f;

    private float recovery = 0.35f;

    [SerializeField] protected float recoveryMultiplier = 1;
    private float forwardSpeedModifier = 0.8f;
    [SerializeField] protected float forwardSpeedModifierMultiplier = 1;
    public int maxComboCount;

    public int currentComboCount = 0;
    public Attack[] combo;
    public Attack currentAttack;
    public Attack airAttack;

    public float staminaCost = 5f;

    [SerializeField] public GameObject hitbox;
    public Hitbox hitboxScript;
    private CapsuleCollider hitboxCollider;

    /// <summary>
    /// A layer of animation to override parts of the base animations.
    /// This is for things like having their hands above their heads to
    /// carrey heavy objects, having only their fist closed, etc.
    /// </summary>
    public AnimationModifier animationModifier = AnimationModifier.None;

    /// <summary>
    /// set this to true for when doing a single attack, we got a hit.
    /// this is used to tell the player's state to slow down their forward displacement to make the impact
    /// feel more IMPACTFUL!!
    /// </summary>
    public bool gotAHit;

    public bool explodeOnHit;

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

    /// <summary>
    /// Gets an AnimationState for a specific attack and section of the attack
    /// </summary>
    /// <param name="attack">The attack type, ie Punch_03, SmashHeavy_01</param>
    /// <param name="section">The section of the attack: 0 = startup, 1 = during, 2 = recovery</param>
    /// <returns></returns>
    public static AnimationState GetAttackAnimation(AttackAnimation attack, int section)
    {
        return (AnimationState)26 + ((int)attack * 3) + section;
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

    public virtual void InitializeAttacks()
    {
        combo = new Attack[]
        {
            new Attack(),
            new Attack()
        };

        maxComboCount = combo.Length;
        airAttack = new Attack();
    }

    public virtual void InitializeAirAttack()
    {
        airAttack = new Attack( // *** 1st Hit ***
                1.0f, // Damage Multiplier
                0.5f, // Knockback Multiplier
                0.5f, // Knockback Height Multiplier
                1.0f, // Hitstun Multiplier
                1.0f, // Radius Multiplier
                1.0f, // Height Multiplier
                1.0f, // Startup Multiplier
                1.0f, // Duration Multiplier
                1.0f, // Recovery Multiplier
                1.0f  // Forward Speed Multiplier
                );
    }

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

        currentAttack = airAttack;

        LoadAirHitbox();

        hitbox.SetActive(true);
    }

    public virtual void AirAttackLand(float extraSizeMultiplier = 1, float extraKnockbackMultiplier = 1)
    {
        gotAHit = false;

        currentAttack = airAttack;

        LoadAirHitbox(extraSizeMultiplier, extraKnockbackMultiplier);

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
        hitboxScript.timeInKnockback = timeInKnockback * currentAttack.timeInKnockbackMultiplier;
        hitboxScript.radius = radius * currentAttack.radiusMultiplier; // Radius is only passed through for gizmo drawing
        hitboxScript.height = height * currentAttack.heightMultiplier;
        hitboxScript.duration = duration * currentAttack.durationMultiplier;
        hitboxScript.playerNumber = playerNumber;
        hitboxScript.airAttack = false;

        // Resize hitbox
        hitboxCollider.radius = radius * currentAttack.radiusMultiplier;
        hitboxCollider.height = height * currentAttack.heightMultiplier;
        hitboxCollider.direction = (int)currentAttack.attackDirection;

        //hitboxScript.tr.localPosition = new Vector3(0, 1, 1 + (radius * currentAttack.radiusMultiplier) / 2); // Experimental
        hitboxScript.tr.localPosition = new Vector3(0, 1, 1 + (
            currentAttack.attackDirection == AttackDirection.Forward
            ? hitboxCollider.height / 2
            : (radius * currentAttack.radiusMultiplier) / 2)
            );

        float y = (hitboxCollider.height / 2) > hitboxCollider.radius ? hitboxCollider.height : hitboxCollider.radius;

        return hitboxScript;
    }

    /// <summary>
    /// Sets the hitbox values for the air attack.
    /// </summary>
    /// <returns>A reference to the hitbox script.</returns>
    protected virtual Hitbox LoadAirHitbox(float extraSizeMultiplier = 1, float extraKnockbackMultipler = 1)
    {
        // Set up hitbox values
        hitboxScript.damage = damage * airAttack.damageMultiplier;
        hitboxScript.knockback = knockback * airAttack.knockbackMultiplier * extraKnockbackMultipler;
        hitboxScript.knockbackHeight = knockbackHeight * airAttack.knockbackHeightMultiplier * extraKnockbackMultipler;
        hitboxScript.timeInKnockback = timeInKnockback * airAttack.timeInKnockbackMultiplier;
        hitboxScript.radius = radius * airAttack.radiusMultiplier; // Radius is only passed through for gizmo drawing
        hitboxScript.duration = duration * airAttack.durationMultiplier;
        hitboxScript.playerNumber = playerNumber;

        hitboxCollider.direction = 2;

        // Resize hitbox
        hitboxCollider.radius = radius * airAttack.radiusMultiplier * 1.5f * extraSizeMultiplier;
        hitboxCollider.height = 3;
        hitboxScript.tr.localPosition = new Vector3(0, 0, 0);
        hitboxScript.airAttack = true;

        return hitboxScript;
    }
}
