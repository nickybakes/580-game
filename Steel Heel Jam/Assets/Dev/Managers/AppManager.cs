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


public class AppManager : MonoBehaviour
{

    public static AppManager app;

    private Scenes currentScene;
    private Scenes previousScene;

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
                else
                    return amount;
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


        if(SceneManager.GetActiveScene() == SceneManager.GetSceneByBuildIndex((int) Scenes.MENU_InitApp))
            SwitchToScene(Scenes.MENU_TempJoinScreen);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void RemovePlayerToken(PlayerToken token)
    {
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
        RemovePlayerToken(playerTokens[playerNumber - 1]);
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
        // CheckIfLoadingDone(SceneManager.LoadSceneAsync((int)s, LoadSceneMode.Single));
        // if (currentScene == Scenes.MENU_InitApp)
        // {
        //     SceneManager.LoadScene((int)s, LoadSceneMode.Single);
        // }
        // else
        // {
        //     //we will need a proper async scene switch in order to have a good looking
        //     //loading transition thing
        //     SceneManager.LoadScene((int)s, LoadSceneMode.Single);
        //     // SceneManager.UnloadSceneAsync((int)currentScene);
        // }
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
