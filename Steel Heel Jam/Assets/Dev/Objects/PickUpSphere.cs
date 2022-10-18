using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpSphere : MonoBehaviour
{
    public PlayerCombat playerCombat;
    public PlayerStatus playerStatus;
    public Transform pickUpLocation;

    private List<GameObject> itemsWithinBounds;

    // Start is called before the first frame update
    void Start()
    {
        itemsWithinBounds = new List<GameObject>();
        playerCombat = transform.parent.GetComponent<PlayerCombat>();
        playerStatus = transform.parent.GetComponent<PlayerStatus>();
        pickUpLocation = transform.parent.Find("PickUpLocation");
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < itemsWithinBounds.Count; i++)
        {
            if (!itemsWithinBounds[i].activeSelf)
            {
                itemsWithinBounds.RemoveAt(i);
                if (itemsWithinBounds.Count == 0 && playerStatus.playerHeader)
                    playerStatus.playerHeader.SetPickup(false);
                i--;
            }
        }
    }

    public void TryPickup()
    {
        if (itemsWithinBounds.Count == 0)
            return;

        GameObject g = itemsWithinBounds[0];

        Item itemScript = g.GetComponent<Item>();
        switch (itemScript.itemType)
        {
            case Item.ItemType.TestCube:
                playerCombat.weaponState = new TestCubeState(playerStatus.playerNumber, playerCombat._hitbox);
                if (playerStatus.playerHeader)
                    playerStatus.playerHeader.SetWeaponText("Test Item");
                break;

            case Item.ItemType.Saber:
                playerCombat.weaponState = new Saber(playerStatus.playerNumber, playerCombat._hitbox);
                if (playerStatus.playerHeader)
                    playerStatus.playerHeader.SetWeaponText("Saber");
                break;

            case Item.ItemType.BoomBox:
                playerCombat.weaponState = new BoomBox(playerStatus.playerNumber, playerCombat._hitbox);
                if (playerStatus.playerHeader)
                    playerStatus.playerHeader.SetWeaponText("Boom Box");
                break;

            case Item.ItemType.ExplosiveBarrel:
                playerCombat.weaponState = new ExplosiveBarrel(playerStatus.playerNumber, playerCombat._hitbox);
                if (playerStatus.playerHeader)
                    playerStatus.playerHeader.SetWeaponText("Barrel");
                break;

            case Item.ItemType.SteelChair:
                playerCombat.weaponState = new SteelChair(playerStatus.playerNumber, playerCombat._hitbox);
                if (playerStatus.playerHeader)
                    playerStatus.playerHeader.SetWeaponText("Steel Chair");
                break;

            case Item.ItemType.Boomerang:
                playerCombat.weaponState = new Boomerang(playerStatus.playerNumber, playerCombat._hitbox);
                if (playerStatus.playerHeader)
                    playerStatus.playerHeader.SetWeaponText("Boomerang");
                break;

            case Item.ItemType.Gauntlets:
                playerCombat.weaponState = new Gauntlets(playerStatus.playerNumber, playerCombat._hitbox);
                if (playerStatus.playerHeader)
                    playerStatus.playerHeader.SetWeaponText("Boxer Gloves");
                break;

            case Item.ItemType.HomingMissile:
                playerCombat.weaponState = new HomingMissile(playerStatus.playerNumber, playerCombat._hitbox);
                if (playerStatus.playerHeader)
                    playerStatus.playerHeader.SetWeaponText("Homing Missile");
                break;
        }

        // Destroy the object on the ground
        //Destroy(other.gameObject);

        // If the player is not in unarmed equip state, the item they are currently holding need to drop before new item is picked up.
        if (playerCombat.equippedItem != null)
        {
            playerCombat.equippedItem.transform.parent = null;
            playerCombat.equippedItem.SetActive(true);
        }

        // Make object inactive and a child of the player
        g.transform.SetParent(playerStatus.transform);
        g.transform.position = pickUpLocation.transform.position;

        g.SetActive(false);
        playerCombat.equippedItem = g;

        // Set Player to pick up state
        playerStatus.SetPlayerStateImmediately(new ItemPickUp());

        itemsWithinBounds.RemoveAt(0);
        if (itemsWithinBounds.Count == 0 && playerStatus.playerHeader)
            playerStatus.playerHeader.SetPickup(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(Tag.PickUp.ToString()))
        {
            if (!itemsWithinBounds.Contains(other.gameObject))
            {
                if (playerStatus.playerHeader)
                    playerStatus.playerHeader.SetPickup(true);

                itemsWithinBounds.Add(other.gameObject);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(Tag.PickUp.ToString()))
        {
            itemsWithinBounds.Remove(other.gameObject);
            if (itemsWithinBounds.Count == 0 && playerStatus.playerHeader)
                playerStatus.playerHeader.SetPickup(false);
        }
    }
}
