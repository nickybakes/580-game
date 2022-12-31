using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{

    public static MenuManager menu;

    public GameObject[] menuPanels;

    private Stack<GameObject> menuStack;

    public MenuCharacterDisplay[] characterDisplays;

    public MenuCustomizationPanel[] customizationPanels;

    public GameObject titleLogo;

    [HideInInspector]
    public PlayerCursor[] cursors;

    private bool characterDisplaysInitialized;

    // Start is called before the first frame update
    void Start()
    {
        AppManager.app.ableToRemovePlayer = true;

        Shader.SetGlobalFloat("ringRadius", 3000);

        MenuManager.menu = this;

        foreach (GameObject g in menuPanels)
        {
            g.SetActive(false);
        }

        if (AppManager.app.TokenAmount == 0)
        {
            ReturnToTitleScreen();
            AudioManager.aud.Play("VO_GameLaunch");
        }
        else
        {
            PassTitleScreen();
        }
    }

    public void PassTitleScreen()
    {
        titleLogo.SetActive(false);
        foreach (GameObject g in menuPanels)
        {
            g.SetActive(false);
        }
        menuStack = new Stack<GameObject>();
        menuStack.Push(menuPanels[1]);
        menuStack.Peek().SetActive(true);
    }

    public void ReturnToTitleScreen()
    {
        titleLogo.SetActive(true);
        foreach (GameObject g in menuPanels)
        {
            g.SetActive(false);
        }
        menuPanels[0].SetActive(true);
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


    public void PlayAnnouncerAudioAdjustWithInterrupt()
    {
        AudioManager.aud.Play("VO_SettingAdjust");
    }

    public void PlaySoundEffectsAdjust()
    {
        string soundName = "SFX_Adjust_0";

        float rng = Random.value;
        if (rng > .7f)
        {
            if (Random.value > .5f)
                soundName += "2";
            else
                soundName += "3";
        }
        else
            soundName += "1";
            
        AudioManager.aud.Play(soundName);
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
        AppManager.app.ableToRemovePlayer = false;
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

    #region audio controls
    public void LoadAudioSetting(MenuNumberInput setting)
    {
        switch (setting.ID)
        {
            case (0):
                setting.value = AppManager.app.audioSettings.master;
                break;
            case (1):
                setting.value = AppManager.app.audioSettings.music;
                break;
            case (2):
                setting.value = AppManager.app.audioSettings.announcer;
                break;
            case (3):
                setting.value = AppManager.app.audioSettings.sfx;
                break;
            case (4):
                setting.value = AppManager.app.audioSettings.ambience;
                break;
        }
        setting.UpdateValueText();
        AudioManager.aud.UpdateMixerVolume();
    }

    public void SaveAudioSetting(MenuNumberInput setting)
    {
        switch (setting.ID)
        {
            case (0):
                AppManager.app.audioSettings.master = setting.value;
                break;
            case (1):
                AppManager.app.audioSettings.music = setting.value;
                break;
            case (2):
                AppManager.app.audioSettings.announcer = setting.value;
                break;
            case (3):
                AppManager.app.audioSettings.sfx = setting.value;
                break;
            case (4):
                AppManager.app.audioSettings.ambience = setting.value;
                break;
        }
        AudioManager.aud.UpdateMixerVolume();
    }

    #endregion

    #region stuff for options controls
    public void LoadCameraShake(MenuNumberInput setting)
    {
        setting.value = AppManager.app.gameSettings.cameraShake;
        setting.UpdateValueText();
    }
    public void LoadSpotlight(MenuToggle setting)
    {
        setting.value = AppManager.app.gameSettings.spotlight;
        setting.UpdateImage();
    }
    public void LoadBuffsBeforeHeelFire(MenuNumberInput setting)
    {
        setting.value = AppManager.app.gameSettings.buffsBeforeHeelFire;
        setting.UpdateValueText();
    }
    public void LoadFastRing(MenuToggle setting)
    {
        setting.value = AppManager.app.gameSettings.fastRing;
        setting.UpdateImage();
    }
    public void LoadBotAmount(MenuNumberInput setting)
    {
        setting.value = AppManager.app.gameSettings.botAmount;
        setting.UpdateValueText();
    }
    public void LoadBotsOnly(MenuToggle setting)
    {
        setting.value = AppManager.app.gameSettings.botsOnly;
        setting.UpdateImage();
    }
    public void LoadBaseballBat(MenuToggle setting)
    {
        setting.value = AppManager.app.gameSettings.baseballBat;
        setting.UpdateImage();
    }
    public void LoadBoombox(MenuToggle setting)
    {
        setting.value = AppManager.app.gameSettings.boombox;
        setting.UpdateImage();
    }
    public void LoadBoxingGlove(MenuToggle setting)
    {
        setting.value = AppManager.app.gameSettings.boxingGlove;
        setting.UpdateImage();
    }
    public void LoadExplosiveBarrel(MenuToggle setting)
    {
        setting.value = AppManager.app.gameSettings.explosiveBarrel;
        setting.UpdateImage();
    }
    public void LoadLadder(MenuToggle setting)
    {
        setting.value = AppManager.app.gameSettings.ladder;
        setting.UpdateImage();
    }
    public void LoadLeadPipe(MenuToggle setting)
    {
        setting.value = AppManager.app.gameSettings.leadPipe;
        setting.UpdateImage();
    }
    public void LoadSabre(MenuToggle setting)
    {
        setting.value = AppManager.app.gameSettings.sabre;
        setting.UpdateImage();
    }
    public void LoadSteelChair(MenuToggle setting)
    {
        setting.value = AppManager.app.gameSettings.steelChair;
        setting.UpdateImage();
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
    public void ChangeBotAmount(int value)
    {
        AppManager.app.gameSettings.botAmount = value;
    }
    public void ToggleBotsOnly(bool value)
    {
        AppManager.app.gameSettings.botsOnly = value;
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
