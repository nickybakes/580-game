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

    private AudioManager audioManger;

    void Start()
    {
        ReInitializeCursors();
        audioManger = FindObjectOfType<AudioManager>();
        MenuManager.menu = FindObjectOfType<MenuManager>();
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

                MenuManager.menu.SolidifyCharacterDisplay(token.playerNumber);
            }
        }
    }


    public void OnPlayerJoined(PlayerInput input)
    {
        if (DoesThisInputAlreadyExist(input))
            return;

        Debug.Log(input.currentControlScheme);

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

                audioManger.Play("controllerOn", 0.6f, 1.4f);
                
                MenuManager.menu.SolidifyCharacterDisplay(playerNumber);
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
