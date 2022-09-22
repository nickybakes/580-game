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
    // Start is called before the first frame update
    void Start()
    {
        _input = GetComponent<StarterAssetsInputs>();
        _status = GetComponent<PlayerStatus>();
        _itemManagerObject = GameObject.Find("ItemManager");
        _itemManager = _itemManagerObject.GetComponent<ItemManager>();
        itemPickedUp = false;
    }

    // Update is called once per frame
    void Update()
    {
        PickUpItem();
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
                    _itemManager.deleteItemAndRemoveFromList(i);
                }
            }
        }
    }
}
