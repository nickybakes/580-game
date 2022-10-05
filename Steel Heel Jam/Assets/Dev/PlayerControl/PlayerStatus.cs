using UnityEngine;

public enum PlayerChild
{
    Model = 0,
    Hitbox = 1,
    Visuals = 3,
    PickUpSphere = 4
}

public enum Tag
{
    Player,
    PickUp
}


[RequireComponent(typeof(PlayerMovement))]
[RequireComponent(typeof(PlayerCombat))]
public class PlayerStatus : MonoBehaviour
{

    public const float deafaultMaxStamina = 100f;


    /// <summary>
    /// The stamina value for the player. Stamina is consumed for actions and is lost upon being hit, being the heel, or being outside of the ring.
    /// When a player's stamina is empty and they are knocked out of the zone, they are eliminated.
    /// </summary>
    public float stamina = deafaultMaxStamina;
    /// <summary>
    /// The player's current maximum stamina value.
    /// </summary>
    public float maxStamina = deafaultMaxStamina;
    /// <summary>
    /// The lowest a player's maximum stamina can get.
    /// </summary>
    [SerializeField] private const float MinMaxStamina = 20f;

    private BasicState currentPlayerState;

    [HideInInspector]
    public PlayerMovement movement;

    [HideInInspector]
    public PlayerCombat combat;

    private PlayerVisuals visuals;

    public int playerNumber;

    public int PlayerNumber { get { return playerNumber; } }

    public BasicState CurrentPlayerState { get { return currentPlayerState; } }

    public bool attackBlocked = false;

    [SerializeField] public float missedBlockStaminaDamage = 20f;

    [SerializeField] public float dodgeRollStaminaDamage = 16f;

    /// <summary>
    /// A boolean that represents if the player is outside of the ring.
    /// </summary>
    private bool isOOB = false;

    /// <summary>
    /// The rate at which stamina is lost when out of bounds.
    /// </summary>
    [SerializeField] private const float OOBStaminaLossCooldownMax = 1f;

    /// <summary>
    /// The current timer for losing stamina when out of bounds.
    /// </summary>
    private float OOBStaminaLossCooldown;

    /// <summary>
    /// The damage to stamina that the player takes every interval while out of bounds.
    /// </summary>
    [SerializeField] private const float OOBStaminaDamage = 10f;

    /// <summary>
    /// The loss of max stamina that the player accrues every interval while out of bounds.
    /// </summary>
    [SerializeField] private const float OOBMaxStaminaDamage = 5f;

    /// <summary>
    /// The rate at which stamina is regained when not active.
    /// </summary>
    [SerializeField] private const float StaminaRegenCooldownMax = 1f;

    /// <summary>
    /// The current timer for regenerating stamina.
    /// </summary>
    private float staminaRegenCooldown;

    /// <summary>
    /// The amount of stamina restored every interval when not active.
    /// </summary>
    [SerializeField] private const float PassiveStaminaRegen = 5f;

    public float totalDamageTaken;

    public float recentDamageTaken;

    public float recentDamageTakenMax;

    public float recentDamageTimeCurrent;

    public bool eliminated;

    public float timeOfEliminiation;

    public PlayerHeader playerHeader;

    private new Transform transform;

    /// <summary>
    /// Use this to check if the player is currently dodging when you want to hit them with an attack
    /// </summary>
    /// <value>True if the player is currently dodging on this frame</value>
    public bool IsDodgeRolling
    {
        get
        {
            return (currentPlayerState is DodgeRoll);
        }
    }

    public bool IsBlocking
    {
        get
        {
            return (currentPlayerState is Block);
        }
    }

    public Transform GetTransform
    {
        get { return transform; }
    }

    /// <summary>
    /// Gives you the current moveSpeed of the character (base move speed multiplied by the current state's move speed multiplier)
    /// </summary>
    /// <value>the current moveSpeed of the character</value>
    public float CurrentMoveSpeed { get { return movement.moveSpeed * currentPlayerState.moveSpeedMultiplier; } }

    // Start is called before the first frame update
    void Start()
    {
        transform = gameObject.transform;
        currentPlayerState = new Idle();
        movement = GetComponent<PlayerMovement>();
        combat = GetComponent<PlayerCombat>();

        movement.Start();
        combat.Start();

        visuals = new PlayerVisuals(this, transform);
    }

    // Update is called once per frame
    void Update()
    {
        //fixes the null ref exception when recompiling in the Editor
#if UNITY_EDITOR
        if (currentPlayerState == null)
            currentPlayerState = new Idle();
#endif

        // Prevents player movement on game start countdown.
        if (GameManager.game.countdownTime > 0)
            return;

        movement.UpdateManual(currentPlayerState.updateMovement, currentPlayerState.canPlayerControlMove, currentPlayerState.canPlayerControlRotate, currentPlayerState.alternateFriction);

        combat.UpdateManual(currentPlayerState.canAttack, currentPlayerState.canDodgeRoll, currentPlayerState.canBlock, currentPlayerState.canPickUp, currentPlayerState.canThrow);

        currentPlayerState.Update(this);

        if (currentPlayerState.changeStateNow)
            SetPlayerStateImmediately(currentPlayerState.stateToChangeTo);

        // If the player is out of bounds . . .
        if (isOOB)
        {
            // Reduce the timer for OOB stamina loss
            OOBStaminaLossCooldown -= Time.deltaTime;

            // If the timer for OOB stamina loss runs out . . .
            if (OOBStaminaLossCooldown < 0)
            {
                // Reset the timer for OOB stamina loss and decrease stamina & maximum stamina
                OOBStaminaLossCooldown = OOBStaminaLossCooldownMax;
                ReduceStamina(OOBStaminaDamage);
                //ReduceMaxStamina(OOBMaxStaminaDamage);
            }
        }

        if (combat.ActedRecently || isOOB)
        {
            staminaRegenCooldown = StaminaRegenCooldownMax;
        }
        else
        {
            staminaRegenCooldown -= Time.deltaTime;

            if (staminaRegenCooldown <= 0)
            {
                IncreaseStamina(PassiveStaminaRegen);
                staminaRegenCooldown = StaminaRegenCooldownMax;
            }
        }

        if (recentDamageTimeCurrent > 0)
        {
            recentDamageTimeCurrent -= Time.deltaTime;

            recentDamageTaken = Mathf.Max(0, Mathf.Lerp(0, recentDamageTakenMax, recentDamageTimeCurrent / 10f));
        }
    }

    public void SetPlayerStateImmediately(BasicState state)
    {
        if(eliminated)
            return;
            
        currentPlayerState.OnExitThisState(state, this);
        state.OnEnterThisState(currentPlayerState, this);

        visuals.SetAnimationState(state.animationState);
        visuals.EnableVisual(state.visual);

        currentPlayerState = state;
    }

    public void SetAnimationState(AnimationState state)
    {
        visuals.SetAnimationState(state);
    }

    public void GetHit(Vector3 hitboxPos, Vector3 collisionPos, float damage, float knockback, float knockbackHeight, float hitstun, PlayerStatus attackingPlayerStatus)
    {
        if (eliminated)
            return;

        if (IsBlocking)
        {
            attackingPlayerStatus.SetPlayerStateImmediately(new BlockedStun());
            attackingPlayerStatus.movement.velocity = attackingPlayerStatus.transform.position - transform.position;
            SetPlayerStateImmediately(new Idle());
            attackBlocked = true;
            return;
        }

        if (!IsDodgeRolling)
        {
            Vector3 knockbackDir = (collisionPos - hitboxPos).normalized;
            knockback = knockback * (2 + stamina / deafaultMaxStamina);

            float staminaRatio = (maxStamina - stamina) * 0.2f;

            float staminaRatioX = staminaRatio;
            float staminaRatioZ = staminaRatio;

            if (knockbackDir.x < 0) staminaRatioX = -staminaRatio;
            else if (knockbackDir.x == 0) staminaRatioX = 0;

            if (knockbackDir.z < 0) staminaRatioZ = -staminaRatio;
            else if (knockbackDir.z == 0) staminaRatioZ = 0;

            Vector3 knockbackVelocity = new Vector3(knockbackDir.x * knockback + staminaRatioX, knockbackHeight + staminaRatio, knockbackDir.z * knockback + staminaRatioZ);
            movement.grounded = false;
            print(knockbackVelocity.magnitude);
            ReduceStamina(damage);
            totalDamageTaken += damage;

            recentDamageTaken += damage;
            recentDamageTakenMax = recentDamageTaken;
            recentDamageTimeCurrent = 10f;

            if (totalDamageTaken > 200f && recentDamageTaken > 30f)
            {
                ReduceMaxStamina(damage);
            }

            attackingPlayerStatus.combat.weaponState.gotAHit = true;

            SetPlayerStateImmediately(new ImpactStun(attackingPlayerStatus, knockbackVelocity));
            currentPlayerState.stateToChangeTo.timeToChangeState = hitstun;
        }

    }

    /// <summary>
    /// Increases the player's stamina. This value will never go above the max.
    /// </summary>
    /// <param name="value">The value to increase the stamina value by.</param>
    public void IncreaseStamina(float value)
    {
        stamina += value;

        if (stamina > maxStamina) stamina = maxStamina;

        if (playerHeader != null)
            playerHeader.UpdateStaminaBar();
    }

    /// <summary>
    /// Reduces the player's stamina. This value will never go below 0.
    /// </summary>
    /// <param name="value">The value to decrease the stamina value by.</param>
    public void ReduceStamina(float value)
    {
        stamina -= value;

        if (stamina < 0) stamina = 0;

        if (playerHeader != null)
            playerHeader.UpdateStaminaBar();

        if (!eliminated && stamina == 0 && isOOB)
        {
            GameManager.game.EliminatePlayer(this);
        }
    }

    /// <summary>
    /// Reduces the player's maximum stamina. If the value would be decreased past the minimum-maximum value, it will be set to the minimum-maximum.
    /// </summary>
    /// <param name="value">The value to decrease the maximum stamina by.</param>
    private void ReduceMaxStamina(float value)
    {
        maxStamina -= value;

        if (maxStamina < MinMaxStamina) maxStamina = MinMaxStamina;

        if (stamina > maxStamina)
        {
            stamina = maxStamina;
        }

        if (playerHeader != null)
            playerHeader.UpdateStaminaBar();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Ring")
        {
            isOOB = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Ring")
        {
            isOOB = true;
            OOBStaminaLossCooldown = OOBStaminaLossCooldownMax;
        }
    }
}
