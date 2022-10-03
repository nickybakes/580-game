using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    PlayerStatus targetPlayer;
    public enum ItemType
    {
        TestCube,
        Saber,
        BoomBox,
        ExplosiveBarrel,
        SteelChair,
        Boomerang,
        Gauntlets,
        HomingMissile
    }

    public ItemType itemType;
    public bool isOnScreen;

    // Disable this script when the GameObject moves out of the camera's view
    void OnBecameInvisible()
    {
        isOnScreen = false;
    }

    // Enable this script when the GameObject moves into the camera's view
    void OnBecameVisible()
    {
        isOnScreen = true;
    }

}
