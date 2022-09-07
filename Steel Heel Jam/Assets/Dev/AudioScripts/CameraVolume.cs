using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraVolume : MonoBehaviour
{
    private Slider slider;

    public void ChangeMasterVolume()
    {
        slider = GetComponent<Slider>();
        AudioListener.volume = slider.value;
    }
}
