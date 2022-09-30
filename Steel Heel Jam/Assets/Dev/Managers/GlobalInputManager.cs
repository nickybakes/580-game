using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class GlobalInputManager : MonoBehaviour
{

    public GameObject playerPrefab;

    public GameObject cursorPrefab;

    public GameObject cursorPanel;

    public bool joinFromMenu;

    void Start()
    {
        ReInitializeCursors();
    }

    public void ReInitializeCursors()
    {
        for (int i = 0; i < AppManager.app.playerTokens.Length; i++)
        {
            if (AppManager.app.playerTokens[i] != null)
            {
                PlayerToken token = AppManager.app.playerTokens[i];
                token.input.SwitchCurrentActionMap("Cursor");

                Debug.Log(token.playerNumber);

                GameObject cursor = Instantiate(cursorPrefab, cursorPanel.transform);
                token.SetUpCursorPrefab(cursor);
            }
        }
    }


    public void OnPlayerJoined(PlayerInput input)
    {
        if (DoesThisInputAlreadyExist(input))
            return;

        PlayerToken token = input.GetComponent<PlayerToken>();
        token.input = input;
        int playerNumber = getLowestPlayerNumber();
        if (playerNumber != -1)
        {
            if (joinFromMenu)
            {
                input.SwitchCurrentActionMap("Cursor");
                token.playerNumber = playerNumber;
                AppManager.app.playerTokens[playerNumber - 1] = token;

                GameObject cursor = Instantiate(cursorPrefab, cursorPanel.transform);
                token.SetUpCursorPrefab(cursor);
            }
            else
            {
                input.SwitchCurrentActionMap("Player");
                token.playerNumber = playerNumber;
                AppManager.app.playerTokens[playerNumber - 1] = token;

                GameObject player = Instantiate(playerPrefab);
                token.SetUpPlayerPrefab(player);
            }
        }
        else
        {
            PlayerInput.Destroy(input.gameObject);
        }
    }

    private bool DoesThisInputAlreadyExist(PlayerInput input)
    {
        for (int i = 0; i < AppManager.app.playerTokens.Length; i++)
        {
            if (AppManager.app.playerTokens[i] != null)
            {
                if (AppManager.app.playerTokens[i].input == input)
                    return true;
            }
        }
        return false;
    }

    private int getLowestPlayerNumber()
    {
        for (int i = 0; i < AppManager.app.playerTokens.Length; i++)
        {
            if (AppManager.app.playerTokens[i] == null)
                return i + 1;
        }
        return -1;
    }

}
