using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public enum ItemType
    {
        Saber,
        BoomBox,
        ExplosiveBarrel,
        SteelChair,
        LeadPipe,
        Gauntlets,
        Ladder,
        BaseballBat
    }

    public ItemType itemType;
}
