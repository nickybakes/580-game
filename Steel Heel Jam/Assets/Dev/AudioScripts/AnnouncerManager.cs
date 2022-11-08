using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 0 is top priority.
/// </summary>
public enum Priority
{
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

    private void Start()
    {
        anm = this;    
    }

    // Use to see if the line to play has priority.
    private Sound currentLine;
    private Priority currentPriority;

    // Use to prevent the same clip from inside a sound to play twice in a row.
    // If no previous clip, set to -1.
    private int previousClip = -1;

    public static void PlayLine(Priority p)
    {
        anm.Play(p);
    }

    private void Play(Priority p)
    {
        if (currentLine == null)
        {

        }
    }

}
