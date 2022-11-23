using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 0 is top priority.
/// </summary>
public enum Priority
{
    MatchEnd,
    HeelFire,
    Buff,
    Elimination,
    SpotlightSpawn,
    Block,
    ElbowDrop,
    Damage
}


public class AnnouncerManager : MonoBehaviour
{
    public static AnnouncerManager anm;

    private void Awake()
    {
        anm = this;    
    }

    // Use to see if the line to play has priority.
    private Sound currentLine;
    private Priority currentPriority;

    public static void PlayLine(string name, Priority p)
    {
        anm.Play(name, p);
    }

    private void Play(string name, Priority p)
    {
        // If the currentLine is not playing (it has finished)
        if (!AudioManager.aud.IsPlaying(currentLine))
        {
            currentLine = null;
        }

        // If no VO is playing, play as normal, update currentPriority.
        if (currentLine == null)
        {
            currentPriority = p;
            currentLine = AudioManager.aud.Find(name);

            AudioManager.aud.Play(name);
        }
        // If what's about to play is greater priority, stop current, update it, and allow to play and update priority.
        else if (currentPriority > p)
        {
            AudioManager.aud.Stop(currentLine.name);
            currentLine = AudioManager.aud.Find(name);
            AudioManager.aud.Play(name);
            currentPriority = p;
        }
        // CurrentLine has priority, do nothing. (Else do not allow clip to play.)
        // Stretch goal, add to queue if high enough priority.
    }
}
