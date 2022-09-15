using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerChild
{
    Hitbox = 2
}

public class PlayerStatus : MonoBehaviour
{
    private float stamina = 100f;
    public EquipState currentEquipState;

    public BasicState currentPlayerState;

    public PlayerMovement movement;

    // Start is called before the first frame update
    void Start()
    {
        currentEquipState = EquipState.DefaultState;
        currentPlayerState = new Idle();
        movement = GetComponent<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        movement.UpdateManual(currentPlayerState.updateMovement, currentPlayerState.canPlayerControlMove, currentPlayerState.canPlayerControlRotate);

        switch (currentEquipState)
        {
            case EquipState.DefaultState:
                Debug.Log("Default State");
                break;
            case EquipState.TestCubeState:
                Debug.Log("TestCubeState");
                break;
        }
    }
}
