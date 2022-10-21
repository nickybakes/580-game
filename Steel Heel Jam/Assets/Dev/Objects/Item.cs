using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    PlayerStatus targetPlayer;
    public enum ItemType
    {
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
