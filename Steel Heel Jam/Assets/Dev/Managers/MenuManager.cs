using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{

    public static MenuManager menu;

    public GameObject HowToPlayMenu;
    public GameObject JoinMenu;

    public MenuCharacterDisplay[] characterDisplays;

    public MenuCustomizationPanel[] customizationPanels;

    private bool characterDisplaysInitialized;

    // Start is called before the first frame update
    void Start()
    {
        Shader.SetGlobalFloat("ringRadius", 3000);

        MenuManager.menu = this;
    }

    // Update is called once per frame
    void Update()
    {
        if (!characterDisplaysInitialized)
        {
            characterDisplaysInitialized = true;
            for (int i = 0; i < AppManager.app.playerTokens.Length; i++)
            {
                if (AppManager.app.playerTokens[i] != null)
                {
                    characterDisplays[i].currentVisualPrefs = AppManager.app.playerTokens[i].visualPrefs;
                    characterDisplays[i].SolidDisplay();
                }
            }
        }
    }

    public void SolidifyCharacterDisplay(int playerNumber)
    {
        characterDisplays[playerNumber - 1].SolidDisplay();
    }

    public void HologramCharacterDisplay(int playerNumber)
    {
        characterDisplays[playerNumber - 1].HologramDisplay();
    }

    //these are temporary functions
    public void ShowHowToPlay()
    {
        HowToPlayMenu.SetActive(true);
    }

    public void ShowJoinScreen()
    {
        JoinMenu.SetActive(true);
    }

    public void HideHowToPlay()
    {
        HowToPlayMenu.SetActive(false);
    }

    public void HideJoinScreen()
    {
        JoinMenu.SetActive(false);
    }
}
