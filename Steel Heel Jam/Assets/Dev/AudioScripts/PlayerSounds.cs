using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSounds : MonoBehaviour
{
    /// <summary>
    /// Used in animation events for run animation.
    /// </summary>
    private void OnFootstep()
    {
        AudioManager.aud.Play("footstep", 0.2f, 0.8f, 1.2f);
    }
}
