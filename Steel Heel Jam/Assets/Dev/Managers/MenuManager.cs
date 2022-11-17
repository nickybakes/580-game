using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{

    public static MenuManager menu;

    public GameObject HowToPlayMenu;
    public GameObject JoinMenu;

    public MenuCharacterDisplay[] characterDisplays;

    // Start is called before the first frame update
    void Start()
    {
        Shader.SetGlobalFloat("ringRadius", 3000);

        MenuManager.menu = this;
    }

    // Update is called once per frame
    void Update()
    {

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
