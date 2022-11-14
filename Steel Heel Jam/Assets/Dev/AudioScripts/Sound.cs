using UnityEngine.Audio;
using UnityEngine;

[System.Serializable]
public class Sound
{
    public enum AudioTypes {
        Music,
        SoundEffect,
        VoiceOver,
        Ambient
    }
    public AudioTypes audioType;

    public string name;

    // Use to prevent the same clip from inside a sound to play twice in a row.
    // If no previous clip, set to -1.
    [HideInInspector]
    public int previousClip = -1;

    public AudioClip[] clips;

    [Range(0f, 1f)]
    public float volume;
    [Range(.1f, 3f)]
    public float pitch;

    public bool loop;
    public bool randStartPos;
    public bool playOnAwake;

    [HideInInspector]
    public AudioSource source;
}
