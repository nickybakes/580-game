using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerToken : MonoBehaviour
{

    public static Color32[] colors = {new Color32(255, 40, 40, 255), new Color32(17, 61, 255, 255), new Color32(255, 246, 9, 255), new Color32(0, 188, 37, 255),
            new Color32(255, 119, 0, 255), new Color32(135, 19, 193, 255), new Color32(255, 0, 238, 255), new Color32(2, 214, 221, 255), new Color32(87,87,87, 255)};

    public int playerNumber;

    public StarterAssetsInputs playerPrefabInputsComp;

    public void SetUpPlayerPrefab(GameObject player)
    {
        playerPrefabInputsComp = player.GetComponent<StarterAssetsInputs>();
        PlayerStatus status = player.GetComponent<PlayerStatus>();

        player.transform.GetChild(0).GetComponent<MeshRenderer>().material.color = colors[playerNumber - 1];
        player.transform.GetChild(1).GetComponent<MeshRenderer>().material.color = colors[playerNumber - 1];

        status.playerNumber = playerNumber;
    }

    public void OnMove(InputValue value)
    {
        playerPrefabInputsComp.OnMove(value);
    }

    public void OnJump(InputValue value)
    {
        playerPrefabInputsComp.OnJump(value);
    }

    public void OnDodgeRoll(InputValue value)
    {
        playerPrefabInputsComp.OnDodgeRoll(value);
    }

    public void OnPickUp(InputValue value)
    {
        playerPrefabInputsComp.OnPickUp(value);
    }

    public void OnAttack(InputValue value)
    {
        playerPrefabInputsComp.OnAttack(value);
    }

    public void OnBlock(InputValue value)
    {
        playerPrefabInputsComp.OnBlock(value);
    }

    public void OnThrow(InputValue value)
    {
        playerPrefabInputsComp.OnThrow(value);
    }
}
