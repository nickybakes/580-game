using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class GlobalInputManager : MonoBehaviour
{
    public void OnPlayerJoined(PlayerInput input)
    {
        Debug.Log(input);
    }

}
