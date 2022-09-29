using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class GlobalInputManager : MonoBehaviour
{

    public PlayerToken[] playerTokens;

    public GameObject playerPrefab;

    public GameObject cursorPrefab;

    void Start()
    {
        playerTokens = new PlayerToken[8];
    }


    public void OnPlayerJoined(PlayerInput input)
    {
        PlayerToken token = input.GetComponent<PlayerToken>();
        int playerNumber = getLowestPlayerNumber();
        if (playerNumber != -1)
        {
            token.playerNumber = playerNumber;
            playerTokens[playerNumber - 1] = token;

            GameObject player = Instantiate(playerPrefab);
            token.SetUpPlayerPrefab(player);
        }
        else
        {
            Destroy(input.gameObject);
        }
    }

    private int getLowestPlayerNumber()
    {
        for (int i = 0; i < playerTokens.Length; i++)
        {
            if (playerTokens[i] == null)
                return i + 1;
        }

        return -1;
    }

}
