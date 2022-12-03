using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public enum Scenes
{
    MENU_InitApp = 0,
    MENU_TempJoinScreen = 1,
    MAP_Demo_01 = 2
}

public struct GameSettings
{
    // Basic Settings
    public int cameraShake;
    public bool spotlight;
    public int buffsBeforeHeelFire;
    public bool fastRing;

    // Weapon Toggles
    public bool baseballBat;
    public bool boombox;
    public bool boxingGlove;
    public bool explosiveBarrel;
    public bool ladder;
    public bool leadPipe;
    public bool sabre;
    public bool steelChair;

    public GameSettings(
        int cameraShake,
        bool spotlight,
        int buffsBeforeHeelFire,
        bool fastRing,
        bool baseballBat,
        bool boombox,
        bool boxingGlove,
        bool explosiveBarrel,
        bool ladder,
        bool leadPipe,
        bool sabre,
        bool steelChair
        )
    {
        this.cameraShake = cameraShake;
        this.spotlight = spotlight;
        this.buffsBeforeHeelFire = buffsBeforeHeelFire;
        this.fastRing = fastRing;

        this.baseballBat = baseballBat;
        this.boombox = boombox;
        this.boxingGlove = boxingGlove;
        this.explosiveBarrel = explosiveBarrel;
        this.ladder = ladder;
        this.leadPipe = leadPipe;
        this.sabre = sabre;
        this.steelChair = steelChair;
    }
}

public struct AudioSettings
{
    public int master;
    public int music;
    public int announcer;
    public int sfx;
    public int ambience;

    public AudioSettings(int master, int music, int announcer, int sfx, int ambience)
    {
        this.master = master;
        this.music = music;
        this.announcer = announcer;
        this.sfx = sfx;
        this.ambience = ambience;
    }
}


public class AppManager : MonoBehaviour
{

    public static AppManager app;

    public GameSettings gameSettings;
    public AudioSettings audioSettings;

    private Scenes currentScene;
    private Scenes previousScene;

    [HideInInspector]
    public bool ableToRemovePlayer;

    [HideInInspector]
    /// <summary>
    /// Player number of the current champion. -1 if no champion yet!
    /// </summary>
    public int currentChampion = -1;

    public Scenes CurrentScene
    {
        get { return currentScene; }
    }

    public Scenes PreviousScene
    {
        get { return previousScene; }
    }

    public int TokenAmount
    {
        get
        {
            int amount = 0;
            for (int i = 0; i < playerTokens.Length; i++)
            {
                if (playerTokens[i])
                    amount++;
            }
            return amount;
        }
    }

    public PlayerToken[] playerTokens;

    // Start is called before the first frame update
    void Start()
    {
        app = this;
        currentScene = Scenes.MENU_InitApp;
        DontDestroyOnLoad(gameObject);

        playerTokens = new PlayerToken[8];

        gameSettings = new GameSettings(5, true, 2, false, true, true, true, true, true, true, true, true);

        audioSettings = new AudioSettings(10, 10, 10, 10, 10);


        if (SceneManager.GetActiveScene() == SceneManager.GetSceneByBuildIndex((int)Scenes.MENU_InitApp))
            SwitchToScene(Scenes.MENU_TempJoinScreen);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void RemovePlayerToken(PlayerToken token)
    {
        if (!ableToRemovePlayer)
            return;

        if (AudioManager.aud)
        {
            AudioManager.aud.Play("controllerOff");
        }

        for (int i = 0; i < playerTokens.Length; i++)
        {
            if (playerTokens[i] == token)
            {
                PlayerInput.Destroy(playerTokens[i].gameObject);
                playerTokens[i] = null;
                return;
            }
        }
    }

    public void RemovePlayerToken(int playerNumber)
    {
        if (!ableToRemovePlayer)
            return;

        RemovePlayerToken(playerTokens[playerNumber - 1]);
        if (TokenAmount == 0)
        {
            MenuManager.menu.ReturnToTitleScreen();
        }
    }

    public void ClearPlayerTokens()
    {
        for (int i = 0; i < playerTokens.Length; i++)
        {
            if (playerTokens[i] != null)
                PlayerInput.Destroy(playerTokens[i].gameObject);
        }
        playerTokens = new PlayerToken[8];
    }

    public void SwitchToScene(Scenes s)
    {
        if (currentScene == Scenes.MENU_TempJoinScreen && s == Scenes.MENU_TempJoinScreen)
        {
            ClearPlayerTokens();
        }

        StartCoroutine(StartLoadProcess(s));

        previousScene = currentScene;
        currentScene = s;
    }

    private IEnumerator StartLoadProcess(Scenes s)
    {
        float time = 1f;
        if (currentScene == Scenes.MENU_InitApp)
            time = 0;
        yield return new WaitForSecondsRealtime(time);
        LoadScene(s);
    }

    private void LoadScene(Scenes s)
    {
        CheckIfLoadingDone(SceneManager.LoadSceneAsync((int)s, LoadSceneMode.Single));
    }

    private IEnumerator CheckIfLoadingDone(AsyncOperation operation)
    {
        if (!operation.isDone)
        {
            yield return null;
        }
    }
}
