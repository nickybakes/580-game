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

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
