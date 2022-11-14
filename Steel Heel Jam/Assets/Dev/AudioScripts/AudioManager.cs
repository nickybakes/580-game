using UnityEngine.Audio;
using UnityEngine;
using System;
using UnityEditor.Rendering;
using UnityEngine.Rendering;
using System.Collections;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioMixerGroup masterMixerGroup;
    [SerializeField] private AudioMixerGroup musicMixerGroup;
    [SerializeField] private AudioMixerGroup soundEffectsMixerGroup;
    [SerializeField] private AudioMixerGroup voiceOverMixerGroup;
    [SerializeField] private AudioMixerGroup ambientMixerGroup;
    [Space(10)] //For ease of reading in Inspector.

    public Sound[] sounds;

    public static AudioManager aud;

    // Start is called before the first frame update
    private void Awake()
    {
        aud = this;

        //if (instance == null)
        //    instance = this;
        //else
        //{
        //    Destroy(gameObject);
        //    return;
        //}

        //DontDestroyOnLoad(gameObject);

        foreach (Sound s in sounds)
        {
            for (int i = 0; i < s.clips.Length; i++)
            {
                s.source = gameObject.AddComponent<AudioSource>();
                s.source.clip = s.clips[i];
            }

            //All clips grouped together will have the same volume, pitch, loop status.
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;

            switch (s.audioType)
            {
                case Sound.AudioTypes.Music:
                    s.source.outputAudioMixerGroup = musicMixerGroup;
                    break;
                case Sound.AudioTypes.SoundEffect:
                    s.source.outputAudioMixerGroup = soundEffectsMixerGroup;
                    break;
                case Sound.AudioTypes.VoiceOver:
                    s.source.outputAudioMixerGroup = voiceOverMixerGroup;
                    break;
                case Sound.AudioTypes.Ambient:
                    s.source.outputAudioMixerGroup = ambientMixerGroup;
                    break;
            }

            if (s.playOnAwake)
                s.source.Play();
        }
    }

    #region Parameter variations for Play()
    /// <summary>
    /// Plays audioclip. Chooses randomly from clips. Sets volume, pitch.
    /// Only one to use s.previousClip, useful for VO lines.
    /// </summary>
    /// <param name="name">Name of audioclip</param>
    public void Play(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }

        //chooses from list before playing.
        int randClip = UnityEngine.Random.Range(0, s.clips.Length);

        if (s.clips.Length > 1)
        {
            while (randClip == s.previousClip)
            {
                randClip = UnityEngine.Random.Range(0, s.clips.Length);
            }
        }

        s.source.clip = s.clips[randClip];
        s.previousClip = randClip;

        if (s.randStartPos)
            s.source.time = UnityEngine.Random.Range(0, s.source.clip.length);

        s.source.Play();
    }

    /// <summary>
    /// Plays audioclip. Chooses randomly from clips. Sets volume, pitch.
    /// </summary>
    /// <param name="name">Name of audioclip</param>
    public void Play(string name, float volume)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }

        //chooses from list before playing.
        s.source.clip = s.clips[UnityEngine.Random.Range(0, s.clips.Length)];
        s.source.volume = volume;

        if (s.randStartPos)
            s.source.time = UnityEngine.Random.Range(0, s.source.clip.length);

        s.source.Play();
    }

    /// <summary>
    /// Plays audioclip. Chooses randomly from clips. Sets volume, pitch.
    /// </summary>
    /// <param name="name">Name of audioclip</param>
    public void Play(string name, float pitch1 = 1, float pitch2 = 1)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }

        //chooses from list before playing.
        s.source.clip = s.clips[UnityEngine.Random.Range(0, s.clips.Length)];
        s.source.pitch = UnityEngine.Random.Range(pitch1, pitch2);

        if (s.randStartPos)
            s.source.time = UnityEngine.Random.Range(0, s.source.clip.length);

        s.source.Play();
    }

    /// <summary>
    /// Plays audioclip. Chooses randomly from clips. Sets volume, pitch.
    /// </summary>
    /// <param name="name">Name of audioclip</param>
    public void Play(string name, float volume, float pitch1 = 1, float pitch2 = 1)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }

        //chooses from list before playing.
        s.source.clip = s.clips[UnityEngine.Random.Range(0, s.clips.Length)];
        s.source.volume = volume;
        s.source.pitch = UnityEngine.Random.Range(pitch1, pitch2);

        if (s.randStartPos)
            s.source.time = UnityEngine.Random.Range(0, s.source.clip.length);

        s.source.Play();
    }
    #endregion

    /// <summary>
    /// Stops audioclip.
    /// </summary>
    /// <param name="name">Name of audioclip</param>
    public void Stop(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }

        s.source.Stop();
    }

    /// <summary>
    /// Finds audioclip. (Useful for changing volume/pitch)
    /// </summary>
    /// <param name="name">Name of audioclip</param>
    /// <returns>Returns Sound clip.</returns>
    public Sound Find(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return null;
        }
        return s;
    }

    /// <summary>
    /// Finds if a specified clip is currently playing.
    /// </summary>
    /// <returns>Returns true if the clip is playing, else false.</returns>
    public bool IsPlaying(Sound s)
    {
        if (s == null)
            return false;

        if (s.source.isPlaying)
            return true;
        else
            return false;
    }

    /// <summary>
    /// Fade in or out audio, based on whether speed is + or -.
    /// </summary>
    /// <param name="name">Name of sound clip.</param>
    /// <param name="speed">Speed to fade at. </param>
    /// <param name="maxMinVol">Either the max or min volume, depending on if you're fading in or out. (between 0 and 1)</param>
    public void Fade(string name, float speed, float maxMinVol)
    {
        Sound s = Find(name);

        // For fadeIn OR fadeOut
        if ((speed > 0 && s.source.volume < maxMinVol) || (speed < 0 && s.source.volume > maxMinVol))
            s.source.volume += speed * Time.deltaTime;
    }

    public void UpdateMixerVolume()
    {
        masterMixerGroup.audioMixer.SetFloat("MasterVolume", Mathf.Log10(AudioOptionsManager.masterVolume) * 20);
        musicMixerGroup.audioMixer.SetFloat("MusicVolume", Mathf.Log10(AudioOptionsManager.musicVolume) * 20);
        soundEffectsMixerGroup.audioMixer.SetFloat("SoundEffectsVolume", Mathf.Log10(AudioOptionsManager.soundEffectsVolume) * 20);
        voiceOverMixerGroup.audioMixer.SetFloat("VoiceOverVolume", Mathf.Log10(AudioOptionsManager.voiceOverVolume) * 20);
        ambientMixerGroup.audioMixer.SetFloat("AmbientVolume", Mathf.Log10(AudioOptionsManager.ambientVolume) * 20);
    }
}
