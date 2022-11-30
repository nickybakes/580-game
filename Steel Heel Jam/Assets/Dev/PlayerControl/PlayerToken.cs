using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerToken : MonoBehaviour
{

    public static Color32[] colors = {new Color32(255, 40, 40, 255), new Color32(17, 61, 255, 255), new Color32(255, 246, 9, 255), new Color32(0, 188, 37, 255),
            new Color32(250, 96, 0, 255), new Color32(101, 14, 158, 255), new Color32(255, 0, 179, 255), new Color32(2, 214, 221, 255), new Color32(87,87,87, 255)};

    public int playerNumber;

    public StarterAssetsInputs playerPrefabInputsComp;

    public CursorInputs cursorPrefabInputsComp;

    public PlayerInput input;

    public CharacterVisualPrefs visualPrefs;

    public bool playerPrefsSet;

    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    void OnDestroy()
    {
        Debug.Log("Player Token " + playerNumber + " destroyed.");
        if (cursorPrefabInputsComp != null)
        {
            MenuManager.menu.HologramCharacterDisplay(playerNumber);
            Destroy(cursorPrefabInputsComp.gameObject);
        }

        if (playerPrefabInputsComp != null)
        {
            Destroy(playerPrefabInputsComp.gameObject);
        }
    }

    public PlayerCursor SetUpCursorPrefab(GameObject cursor)
    {
        cursorPrefabInputsComp = cursor.GetComponent<CursorInputs>();
        PlayerCursor cursorScript = cursor.GetComponent<PlayerCursor>();

        cursorScript.playerNumberText.text = "P" + playerNumber;
        cursorScript.cursorSprite.color = colors[playerNumber - 1];
        cursorScript.playerNumber = playerNumber;

        cursorScript.ReturnToDefaultLocation(3);

        if (MenuManager.menu)
        {
            if (!playerPrefsSet)
            {
                visualPrefs = MenuManager.menu.characterDisplays[playerNumber - 1].currentVisualPrefs;
                playerPrefsSet = true;
            }
            else
            {
                MenuManager.menu.characterDisplays[playerNumber - 1].SetVisualPrefs(visualPrefs);
            }
        }

        return cursorScript;
    }

    public PlayerStatus SetUpPlayerPrefab(GameObject player)
    {
        playerPrefabInputsComp = player.GetComponent<StarterAssetsInputs>();
        PlayerStatus status = player.GetComponent<PlayerStatus>();

        OutlineSketchUpdate outline = player.GetComponentInChildren<OutlineSketchUpdate>();

        outline.SetPlayerNumberIndex(playerNumber);
        outline.SetSkinTone(visualPrefs.skinToneIndex);

        PlayerHairController hairController = player.GetComponentInChildren<PlayerHairController>();
        hairController.SetHairPrefs(visualPrefs.hairStyleIndex, visualPrefs.hairColorIndex, playerNumber);

        player.GetComponentInChildren<PlayerRingDecal>().SetTint(playerNumber);

        // player.transform.GetChild((int)PlayerChild.Model).GetChild(0).GetComponent<MeshRenderer>().material.SetColor("_BaseColor", colors[playerNumber - 1]);
        // player.transform.GetChild((int)PlayerChild.Model).GetChild(1).GetComponent<MeshRenderer>().material.SetColor("_BaseColor", colors[playerNumber - 1]);

        status.playerNumber = playerNumber;

        return status;
    }

    public void OnPause(InputValue value)
    {
        playerPrefabInputsComp.OnPause(value);
    }

    public void OnDebugRestartGame(InputValue value)
    {
        // playerPrefabInputsComp.OnPause(value);

        // if (value.isPressed)
        // {
        //     Debug.Log("Restart Game");
        //     AppManager.app.SwitchToScene(Scenes.MENU_TempJoinScreen);
        // }
    }

    public void OnDebugStartGame1(InputValue value)
    {
        // playerPrefabInputsComp.OnPause(value);
        // if (value.isPressed)
        // {
        //     Debug.Log("Start Game 1");
        //     AppManager.app.SwitchToScene(Scenes.MAP_Demo_01);
        // }
    }

    public void OnMove(InputValue value)
    {
        playerPrefabInputsComp.OnMove(value);
    }

    public void OnCursorMove(InputValue value)
    {
        cursorPrefabInputsComp.OnMove(value);
    }

    public void OnCursorSnap(InputValue value)
    {
        cursorPrefabInputsComp.OnSnap(value);
    }

    public void OnAccept(InputValue value)
    {
        cursorPrefabInputsComp.OnAccept(value);
    }

    public void OnCursorBack(InputValue value)
    {
        cursorPrefabInputsComp.OnBack(value);
    }

    public void OnCustomize(InputValue value)
    {
        cursorPrefabInputsComp.OnCustomize(value);
    }

    public void OnRandomize(InputValue value)
    {
        cursorPrefabInputsComp.OnRandomize(value);
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
