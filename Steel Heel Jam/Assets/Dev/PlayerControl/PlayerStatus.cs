using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerChild
{
    Hitbox = 2
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
    public EquipState currentEquipState;

    public BasicState currentPlayerState;

    public PlayerMovement movement;

    public PlayerCombat combat;

    public int playerNumber;

    public int PlayerNumber { get { return playerNumber; } }

    // Start is called before the first frame update
    void Start()
    {
        currentEquipState = EquipState.DefaultState;
        currentPlayerState = new Idle();
        movement = GetComponent<PlayerMovement>();
        combat = GetComponent<PlayerCombat>();
    }

    // Update is called once per frame
    void Update()
    {
        movement.UpdateManual(currentPlayerState.updateMovement, currentPlayerState.canPlayerControlMove, currentPlayerState.canPlayerControlRotate, currentPlayerState.moveSpeedMultiplier);

        combat.UpdateManual(currentPlayerState.canAttack, currentPlayerState.canDodgeRoll, currentPlayerState.canBlock);

        currentPlayerState.grounded = movement.grounded;
        currentPlayerState.wasGrounded = movement.wasGrounded;
        currentPlayerState.Update();

        if (currentPlayerState.changeStateNow)
            currentPlayerState = currentPlayerState.stateToChangeTo;

        switch (currentEquipState)
        {
            case EquipState.DefaultState:
                //Debug.Log("Default State");
                break;
            case EquipState.TestCubeState:
                //Debug.Log("TestCubeState");
                break;
        }
    }

    public void GetHit(Vector3 hitboxPos, Vector3 collisionPos, float damage, float knockback, float knockbackHeight, float hitstun)
    {

        Vector3 knockbackDir = (collisionPos - hitboxPos).normalized;
        knockback = knockback * (2 + stamina / 100);
        movement.velocity = new Vector3(knockbackDir.x * knockback, knockbackHeight, knockbackDir.z * knockback);
        movement.grounded = false;

        currentPlayerState = new ImpactStun();
        currentPlayerState.stateToChangeTo.timeToChangingState = hitstun;
    }
}
