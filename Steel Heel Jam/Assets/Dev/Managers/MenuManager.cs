using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{

    public GameObject HowToPlayMenu;
    public GameObject JoinMenu;
    // Start is called before the first frame update
    void Start()
    {
        Shader.SetGlobalFloat("ringRadius", 3000);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //these are temporary functions
    public void ShowHowToPlay(){
        HowToPlayMenu.SetActive(true);
    }

    public void ShowJoinScreen(){
        JoinMenu.SetActive(true);
    }

    public void HideHowToPlay(){
        HowToPlayMenu.SetActive(false);
    }

    public void HideJoinScreen(){
        JoinMenu.SetActive(false);
    }
}
