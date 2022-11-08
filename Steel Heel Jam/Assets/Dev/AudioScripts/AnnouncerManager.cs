using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnnouncerManager : MonoBehaviour
{
    /// <summary>
    /// 0 is top priority.
    /// </summary>
    enum Priority
    {
        HeelFire,
        Buff,
        Elimination,
        SpotlightSpawn,
        Block,
        ElbowDrop,
        Damage
    }

    // Use to see if the line to play has priority.
    private Sound currentLine;
    private Priority currentPriority;

    // Use to prevent the same clip from inside a sound to play twice in a row.
    // If no previous clip, set to -1.
    private int previousClip = -1;

    
}
