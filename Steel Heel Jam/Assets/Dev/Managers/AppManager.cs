using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum Scenes
{
    MENU_InitApp = 0,
    MENU_TempJoinScreen = 1,
    MAP_Demo_01 = 2
}

public class AppManager : MonoBehaviour
{

    private Scenes currentScene;

    // Start is called before the first frame update
    void Start()
    {
        currentScene = Scenes.MENU_InitApp;
        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SwitchToScene(Scenes s){
        SceneManager.LoadSceneAsync((int) s, LoadSceneMode.Additive);
        SceneManager.UnloadSceneAsync((int) currentScene);
    }
}
