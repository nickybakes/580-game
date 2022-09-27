using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerItemPickUpThrow : MonoBehaviour
{
    private StarterAssetsInputs _input;
    private PlayerStatus _status;
    private GameObject _itemManagerObject;
    private ItemManager _itemManager;
    bool itemPickedUp;
    bool readyToThrow;
    float forceMultiplier;
    float forceMultiplierLimit;
    string pickedUpItemTag;
    // Start is called before the first frame update
    void Start()
    {
        _input = GetComponent<StarterAssetsInputs>();
        _status = GetComponent<PlayerStatus>();
        _itemManagerObject = GameObject.Find("ItemManager");
        _itemManager = _itemManagerObject.GetComponent<ItemManager>();
        itemPickedUp = false;
        readyToThrow = false;
        forceMultiplier = 0;
        forceMultiplierLimit = 500;
    }

    // Update is called once per frame
    void Update()
    {
        PickUpItem();
        ChargeAndThrowItem();
    }

    void PickUpItem()
    {
        if(_input.pickUpPressed && itemPickedUp == false)
        {
            Debug.Log("Pick up pressed");
            for (int i = 0; i < _itemManager.itemsOnGround.Count; i++)
            {
                float pickUpDistance = Vector3.Distance(transform.position, _itemManager.itemsOnGround[i].transform.position);
                if (pickUpDistance <=2)
                {
                    Debug.Log("item picked up");
                    itemPickedUp = true;
                    pickedUpItemTag = _itemManager.DeleteItemAndRemoveFromList(i);
                    _input.pickUpPressed = false;
                }
            }
        }
    }

    void ChargeAndThrowItem()
    {
        // Charge up throw
        if (_input.throwIsHeld && itemPickedUp == true && forceMultiplier < forceMultiplierLimit)
        {
            forceMultiplier += 500 * Time.deltaTime;
            Debug.Log(forceMultiplier);
            readyToThrow = true;
        }

        // Throw Item
        if (_input.throwIsHeld == false && itemPickedUp == true && readyToThrow == true)
        {

            /*if (forceMultiplier > 10)
            {
                rb.AddForce(player.transform.forward * forceMultiplier);
                this.transform.parent = null;
                GetComponent<Rigidbody>().useGravity = true;
                GetComponent<BoxCollider>().enabled = true;
                itemPickedUp = false;

                forceMultiplier = 0;
                readyToThrow = false;

                // Since item has been thrown, set playerStatus enum back to default
                _playerStatus.currentEquipState = EquipState.DefaultState;
            }*/

            itemPickedUp = false;
            readyToThrow = false;
            _itemManager.SpawnThrownItem(pickedUpItemTag, this.transform, forceMultiplier);
            forceMultiplier = 0;
        }
    }
}
