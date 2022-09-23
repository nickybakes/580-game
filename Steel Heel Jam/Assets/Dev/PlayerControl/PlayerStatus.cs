using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerChild
{
    Hitbox = 2,
    Visuals = 4
}

public enum Tag
{
    Player
}


[RequireComponent(typeof(PlayerMovement))]
[RequireComponent(typeof(PlayerCombat))]
public class PlayerStatus : MonoBehaviour
{
    private float stamina = 100f;

    private BasicState currentPlayerState;

    [HideInInspector]
    public PlayerMovement movement;

    [HideInInspector]
    public PlayerCombat combat;

    private PlayerVisuals visuals;

    public int playerNumber;

    public int PlayerNumber { get { return playerNumber; } }

    public BasicState CurrentPlayerState { get { return currentPlayerState; }}

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

        movement.UpdateManual(currentPlayerState.updateMovement, currentPlayerState.canPlayerControlMove, currentPlayerState.canPlayerControlRotate);

        combat.UpdateManual(currentPlayerState.canAttack, currentPlayerState.canDodgeRoll, currentPlayerState.canBlock);


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
            Vector3 knockbackDir = (collisionPos - hitboxPos).normalized;
            knockback = knockback * (2 + stamina / 100);
            Vector3 knockbackVelocity = new Vector3(knockbackDir.x * knockback, knockbackHeight, knockbackDir.z * knockback);
            movement.grounded = false;

            attackingPlayerStatus.combat.weaponState.gotAHit = true;

            SetPlayerStateImmediately(new ImpactStun(attackingPlayerStatus, knockbackVelocity));
            currentPlayerState.stateToChangeTo.timeToChangeState = hitstun;
        }

    }
}
