using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{

    public static MenuManager menu;

    public GameObject HowToPlayMenu;
    public GameObject JoinMenu;

    public GameObject[] menuPanels;

    private Stack<GameObject> menuStack;

    public MenuCharacterDisplay[] characterDisplays;

    public MenuCustomizationPanel[] customizationPanels;

    [HideInInspector]
    public PlayerCursor[] cursors;

    private bool characterDisplaysInitialized;

    // Start is called before the first frame update
    void Start()
    {
        Shader.SetGlobalFloat("ringRadius", 3000);

        MenuManager.menu = this;

        foreach (GameObject g in menuPanels)
        {
            g.SetActive(false);
        }

        menuStack = new Stack<GameObject>();
        menuStack.Push(menuPanels[1]);
        menuStack.Peek().SetActive(true);
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

    public void StartGame()
    {
        AppManager.app.SwitchToScene(Scenes.MAP_Demo_01);
    }

    //closes application window or ends Unity editor playing
    public void QuitGame()
    {
#if UNITY_EDITOR
        // Application.Quit() does not work in the editor so
        // UnityEditor.EditorApplication.isPlaying need to be set to false to end the game
        UnityEditor.EditorApplication.isPlaying = false;
#else
         Application.Quit();
#endif
    }

    public static void GoBack()
    {
        menu.menuStack.Pop().SetActive(false);
        menu.menuStack.Peek().SetActive(true);
    }

    /*
    0. title
    1. join
    2. how to play
    3. settings
    */
    public static void GoToMenu(int menuIndex)
    {
        menu.menuStack.Peek().SetActive(false);
        menu.menuStack.Push(menu.menuPanels[menuIndex]);
        menu.menuStack.Peek().SetActive(true);
    }

    public bool OnJoinScreen()
    {
        return menuStack.Peek() == menuPanels[1];
    }

    public void SaveAllCustomizePanels()
    {
        for (int i = 0; i < AppManager.app.playerTokens.Length; i++)
        {
            if (AppManager.app.playerTokens[i] && cursors[i].IsCustomizing)
            {
                customizationPanels[i].SaveChanges();
                customizationPanels[i].ClosePanel();
                cursors[i].IsCustomizing = false;
            }
        }
    }

    /// <summary>
    /// Switches the current menu with a different one
    /// Useful for toggling between different pages of the same general menu
    /// like the how to play that switches between Gamepad and Keyboard controls
    /// </summary>
    /// <param name="menuIndex"></param>
    public static void SwitchMenu(int menuIndex)
    {
        GoBack();
        GoToMenu(menuIndex);
    }

    #region stuff for options controls
    public void LoadCameraShake(MenuNumberInput setting)
    {
        setting.value = AppManager.app.gameSettings.cameraShake;
    }
    public void LoadSpotlight(MenuToggle setting)
    {
        setting.value = AppManager.app.gameSettings.spotlight;
    }
    public void LoadBuffsBeforeHeelFire(MenuNumberInput setting)
    {
        setting.value = AppManager.app.gameSettings.buffsBeforeHeelFire;
    }
    public void LoadFastRing(MenuToggle setting)
    {
        setting.value = AppManager.app.gameSettings.fastRing;
    }
    public void LoadBaseballBat(MenuToggle setting)
    {
        setting.value = AppManager.app.gameSettings.baseballBat;
    }
    public void LoadBoombox(MenuToggle setting)
    {
        setting.value = AppManager.app.gameSettings.boombox;
    }
    public void LoadBoxingGlove(MenuToggle setting)
    {
        setting.value = AppManager.app.gameSettings.boxingGlove;
    }
    public void LoadExplosiveBarrel(MenuToggle setting)
    {
        setting.value = AppManager.app.gameSettings.explosiveBarrel;
    }
    public void LoadLadder(MenuToggle setting)
    {
        setting.value = AppManager.app.gameSettings.ladder;
    }
    public void LoadLeadPipe(MenuToggle setting)
    {
        setting.value = AppManager.app.gameSettings.leadPipe;
    }
    public void LoadSabre(MenuToggle setting)
    {
        setting.value = AppManager.app.gameSettings.sabre;
    }
    public void LoadSteelChair(MenuToggle setting)
    {
        setting.value = AppManager.app.gameSettings.steelChair;
    }
    public void ChangeCameraShake(int value)
    {
        AppManager.app.gameSettings.cameraShake = value;
    }
    public void ToggleSpotlight(bool value)
    {
        AppManager.app.gameSettings.spotlight = value;
    }
    public void ChangeBuffsBeforeHeelFire(int value)
    {
        AppManager.app.gameSettings.buffsBeforeHeelFire = value;
    }
    public void ToggleFastRing(bool value)
    {
        AppManager.app.gameSettings.fastRing = value;
    }
    public void ToggleBaseballBat(bool value)
    {
        AppManager.app.gameSettings.baseballBat = value;
    }
    public void ToggleBoombox(bool value)
    {
        AppManager.app.gameSettings.boombox = value;
    }
    public void ToggleBoxingGlove(bool value)
    {
        AppManager.app.gameSettings.boxingGlove = value;
    }
    public void ToggleExplosiveBarrel(bool value)
    {
        AppManager.app.gameSettings.explosiveBarrel = value;
    }
    public void ToggleLadder(bool value)
    {
        AppManager.app.gameSettings.ladder = value;
    }
    public void ToggleLeadPipe(bool value)
    {
        AppManager.app.gameSettings.leadPipe = value;
    }
    public void ToggleSabre(bool value)
    {
        AppManager.app.gameSettings.sabre = value;
    }
    public void ToggleSteelChair(bool value)
    {
        AppManager.app.gameSettings.steelChair = value;
    }
    #endregion
}
