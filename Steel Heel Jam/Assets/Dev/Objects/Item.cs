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
}
