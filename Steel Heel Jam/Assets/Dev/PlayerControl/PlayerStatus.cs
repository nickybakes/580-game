using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatus : MonoBehaviour
{
    private float stamina = 100f;
    public enum EquipState { DefaultState, TestCubeState };
    public EquipState currentEquipState;

    public BasicState currentPlayerState;

    // Start is called before the first frame update
    void Start()
    {
        currentEquipState = EquipState.DefaultState;
        currentPlayerState = new Idle();
    }

    // Update is called once per frame
    void Update()
    {
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
