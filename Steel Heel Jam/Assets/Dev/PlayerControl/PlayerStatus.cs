using System.Collections;
using System.Collections.Generic;
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
    /// <summary>
    /// The stamina value for the player. Stamina is consumed for actions and is lost upon being hit, being the heel, or being outside of the ring.
    /// When a player's stamina is empty and they are knocked out of the zone, they are eliminated.
    /// </summary>
    private float stamina = 100f;
    /// <summary>
    /// The player's current maximum stamina value.
    /// </summary>
    private float maxStamina = 100f;
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

    [SerializeField] private const float OOBStaminaDamage = 20f;

    [SerializeField] private const float OOBMaxStaminaDamage = 10f;

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

    /// <summary>
    /// Gives you the current moveSpeed of the character (base move speed multiplied by the current state's move speed multiplier)
    /// </summary>
    /// <value>the current moveSpeed of the character</value>
    public float CurrentMoveSpeed { get { return movement.moveSpeed * currentPlayerState.moveSpeedMultiplier; } }

    // Start is called before the first frame update
    void Start()
    {
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
                ReduceMaxStamina(OOBMaxStaminaDamage);
            }
        }

        movement.UpdateManual(currentPlayerState.updateMovement, currentPlayerState.canPlayerControlMove, currentPlayerState.canPlayerControlRotate);

        combat.UpdateManual(currentPlayerState.canAttack, currentPlayerState.canDodgeRoll, currentPlayerState.canBlock, currentPlayerState.canPickUp);

        currentPlayerState.Update(this);

        if (currentPlayerState.changeStateNow)
            SetPlayerStateImmediately(currentPlayerState.stateToChangeTo);

    }

    public void SetPlayerStateImmediately(BasicState state)
    {
        currentPlayerState.OnExitThisState(state, this);
        state.OnEnterThisState(currentPlayerState, this);

        visuals.EnableVisual(state.visual);

        currentPlayerState = state;
    }

    public void GetHit(Vector3 hitboxPos, Vector3 collisionPos, float damage, float knockback, float knockbackHeight, float hitstun, PlayerStatus attackingPlayerStatus)
    {
        if(IsBlocking)
        {
            attackingPlayerStatus.SetPlayerStateImmediately(new BlockedStun());
            attackingPlayerStatus.movement.velocity = attackingPlayerStatus.transform.position - transform.position;
            SetPlayerStateImmediately(new Idle());
            return;
        }

        if (!IsDodgeRolling)
        {
            ReduceStamina(damage);

            Vector3 knockbackDir = (collisionPos - hitboxPos).normalized;
            knockback = knockback * (2 + stamina / 100);
            Vector3 knockbackVelocity = new Vector3(knockbackDir.x * knockback, knockbackHeight, knockbackDir.z * knockback);
            movement.grounded = false;

            attackingPlayerStatus.combat.weaponState.gotAHit = true;

            SetPlayerStateImmediately(new ImpactStun(attackingPlayerStatus, knockbackVelocity));
            currentPlayerState.stateToChangeTo.timeToChangeState = hitstun;
        }

    }

    /// <summary>
    /// Reduces the player's stamina. This value will never go below 0.
    /// </summary>
    /// <param name="value">The value to decrease the stamina value by.</param>
    private void ReduceStamina(float value)
    {
        stamina -= value;

        if (stamina < 0) stamina = 0;
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
