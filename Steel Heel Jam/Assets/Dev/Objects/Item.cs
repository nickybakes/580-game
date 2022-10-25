using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    Saber,
    BoomBox,
    ExplosiveBarrel,
    SteelChair,
    LeadPipe,
    Gauntlets,
    Ladder,
    BaseballBat,
    Unarmed = -1
}

public class Item : MonoBehaviour
{
    public ItemType itemType;
}
