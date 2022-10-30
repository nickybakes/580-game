using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AudioOptionsManager : MonoBehaviour
{
    public static float masterVolume {get; private set; }
    public static float musicVolume { get; private set; }
    public static float soundEffectsVolume { get; private set; }
    public static float voiceOverVolume { get; private set; }
    public static float ambientVolume { get; private set; }

    [SerializeField] private TextMeshPro masterSliderText;
    [SerializeField] private TextMeshPro musicSliderText;
    [SerializeField] private TextMeshPro soundEffectSliderText;
    [SerializeField] private TextMeshPro voiceOverSliderText;
    [SerializeField] private TextMeshPro ambientSliderText;


    //https://www.youtube.com/watch?v=LfU5xotjbPw
    
    public void OnMasterSliderValueChange(float value)
    {
        masterVolume = value;
        //masterSliderText.text = ((int)(value * 100)).ToString();
        AudioManager.aud.UpdateMixerVolume();
    }

    public void OnMusicSliderValueChange(float value)
    {
        musicVolume = value;
        //musicSliderText.text = ((int)(value * 100)).ToString();
        AudioManager.aud.UpdateMixerVolume();
    }

    public void OnSoundEffectSliderValueChange(float value)
    {
        soundEffectsVolume = value;
        //soundEffectSliderText.text = ((int)(value * 100)).ToString();
        AudioManager.aud.UpdateMixerVolume();
    }

    public void OnVoiceOverSliderValueChange(float value)
    {
        voiceOverVolume = value;
        //voiceOverSliderText.text = ((int)(value * 100)).ToString();
        AudioManager.aud.UpdateMixerVolume();
    }

    public void OnAmbientSliderValueChange(float value)
    {
        ambientVolume = value;
        //ambientSliderText.text = ((int)(value * 100)).ToString();
        AudioManager.aud.UpdateMixerVolume();
    }
}
