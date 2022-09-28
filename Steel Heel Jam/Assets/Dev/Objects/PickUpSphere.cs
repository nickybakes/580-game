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
            //Debug.Log(other.gameObject.name);
            // Change Players current equip state to equip state for this object
            switch(other.gameObject.name)
            {
                case "TestCube(Clone)":
                    playerCombat.weaponState = new TestCubeState(playerStatus.playerNumber, playerCombat._hitbox);
                    break;

                case "Saber(Clone)":
                    playerCombat.weaponState = new Saber(playerStatus.playerNumber, playerCombat._hitbox);
                    break;

                case "BoomBox(Clone)":
                    playerCombat.weaponState = new BoomBox(playerStatus.playerNumber, playerCombat._hitbox);
                    break;

                case "ExposiveBarrel(Clone)":
                    playerCombat.weaponState = new ExplosiveBarrel(playerStatus.playerNumber, playerCombat._hitbox);
                    break;

                case "SteelChair(Clone)":
                    playerCombat.weaponState = new SteelChair(playerStatus.playerNumber, playerCombat._hitbox);
                    break;

                case "Boomerang(Clone)":
                    playerCombat.weaponState = new Boomerang(playerStatus.playerNumber, playerCombat._hitbox);
                    break;

                case "Gauntlets(Clone)":
                    playerCombat.weaponState = new Gauntlets(playerStatus.playerNumber, playerCombat._hitbox);
                    break;

                case "HomingMissile(Clone)":
                    playerCombat.weaponState = new HomingMissile(playerStatus.playerNumber, playerCombat._hitbox);
                    break;
            }

            // Destroy the object on the ground
            Destroy(other.gameObject);

            // Set Player to pick up state
            playerStatus.SetPlayerStateImmediately(new ItemPickUp());
        }
    }

    private void TurnOffSphere()
    {
        gameObject.SetActive(false);
    }
}
