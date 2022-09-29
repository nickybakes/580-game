using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpSphere : MonoBehaviour
{
    public PlayerCombat playerCombat;
    public PlayerStatus playerStatus;

    // Start is called before the first frame update
    void Start()
    {
        playerCombat = transform.parent.GetComponent<PlayerCombat>();
        playerStatus = transform.parent.GetComponent<PlayerStatus>();
    }

    private void OnEnable()
    {
        Invoke("TurnOffSphere", 0.1f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == Tag.PickUp.ToString())
        {
            Item itemScript = other.GetComponent<Item>();
            switch (itemScript.itemType)
            {
                case Item.ItemType.TestCube:
                    playerCombat.weaponState = new TestCubeState(playerStatus.playerNumber, playerCombat._hitbox);
                    break;

                case Item.ItemType.Saber:
                    playerCombat.weaponState = new Saber(playerStatus.playerNumber, playerCombat._hitbox);
                    break;

                case Item.ItemType.BoomBox:
                    playerCombat.weaponState = new BoomBox(playerStatus.playerNumber, playerCombat._hitbox);
                    break;

                case Item.ItemType.ExplosiveBarrel:
                    playerCombat.weaponState = new ExplosiveBarrel(playerStatus.playerNumber, playerCombat._hitbox);
                    break;

                case Item.ItemType.SteelChair:
                    playerCombat.weaponState = new SteelChair(playerStatus.playerNumber, playerCombat._hitbox);
                    break;

                case Item.ItemType.Boomerang:
                    playerCombat.weaponState = new Boomerang(playerStatus.playerNumber, playerCombat._hitbox);
                    break;

                case Item.ItemType.Gauntlets:
                    playerCombat.weaponState = new Gauntlets(playerStatus.playerNumber, playerCombat._hitbox);
                    break;

                case Item.ItemType.HomingMissile:
                    playerCombat.weaponState = new HomingMissile(playerStatus.playerNumber, playerCombat._hitbox);
                    break;
            }

            // Destroy the object on the ground
            //Destroy(other.gameObject);

            // If the player is not in unarmed equip state, the item they are currently holding need to drop before new item is picked up.
            if (playerCombat.hasItemEquiped == true)
            {

                Item itemToDrop = playerStatus.transform.GetComponent<Item>();
                //GameObject itemToDrop = playerStatus.transform.GetChild(4).gameObject;
                //itemToDrop.transform.position = playerStatus.transform.position;
                //itemToDrop.transform.parent = null;
                //itemToDrop.transform.gameObject.SetActive(true);
            }

            // Make object inactive and a child of the player
            other.gameObject.SetActive(false);
            other.transform.SetParent(playerStatus.transform);
            playerCombat.hasItemEquiped = true;

            // Set Player to pick up state
            playerStatus.SetPlayerStateImmediately(new ItemPickUp());
        }
    }

    private void TurnOffSphere()
    {
        gameObject.SetActive(false);
    }
}
