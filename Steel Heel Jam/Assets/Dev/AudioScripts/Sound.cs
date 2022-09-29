using UnityEngine.Audio;
using UnityEngine;

[System.Serializable]
public class Sound
{
    public enum AudioTypes {
        Music,
        SoundEffect,
        VoiceOver 
    }
    public AudioTypes audioType;

    public string name;

    public AudioClip[] clips;

    [Range(0f, 1f)]
    public float volume;
    [Range(.1f, 3f)]
    public float pitch;

    public bool loop;
    public bool playOnAwake;

    [HideInInspector]
    public AudioSource source;
}
