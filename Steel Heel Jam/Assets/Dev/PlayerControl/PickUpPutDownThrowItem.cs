using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PickUpPutDownThrowItem : MonoBehaviour
{
    private Transform pickUpLocation;
    private Transform player;

    public float pickUpDistance;
    public float forceMultiplier;
    public float forceMultiplierLimit;

    public bool readyToThrow;
    public bool itemPickedUp;

    private Rigidbody rb;

    public StarterAssetsInputs _input;
    public PlayerStatus _playerStatus;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        player = GameObject.Find("PlayerPrefab").transform;
        pickUpLocation = GameObject.Find("PickUpLocation").transform;
    }

    // Update is called once per frame
    void Update()
    {
        // Pick Up Item
        pickUpDistance = Vector3.Distance(player.position, transform.position);
        if (pickUpDistance <= 2)
        {
            if (_input.pickUpPutDownPressed && itemPickedUp == false && pickUpLocation.childCount < 1)
            {
                Debug.Log("test");
                GetComponent<Rigidbody>().useGravity = false;
                GetComponent<BoxCollider>().enabled = false;
                this.transform.position = pickUpLocation.position;
                this.transform.parent = GameObject.Find("PickUpLocation").transform;

                itemPickedUp = true;
                forceMultiplier = 0;

                // Check tag of picked up Item and set playerstatus enum accordingly
                switch(this.tag)
                {
                    case "TestCube":
                        _playerStatus.currentEquipState = PlayerStatus.EquipState.TestCubeState;
                        break;
                }
            }
        }

        // Charge Up Throw
        if (_input.throwIsHeld && itemPickedUp == true && readyToThrow == true && forceMultiplier < forceMultiplierLimit)
        {
            forceMultiplier += 500 * Time.deltaTime;
        }

        // Throw Item
        if (_input.throwIsHeld == false && itemPickedUp == true)
        {
            readyToThrow = true;

            if (forceMultiplier > 10)
            {
                rb.AddForce(player.transform.forward * forceMultiplier);
                this.transform.parent = null;
                GetComponent<Rigidbody>().useGravity = true;
                GetComponent<BoxCollider>().enabled = true;
                itemPickedUp = false;

                forceMultiplier = 0;
                readyToThrow = false;

                // Since item has been thrown, set playerStatus enum back to default
                _playerStatus.currentEquipState = PlayerStatus.EquipState.DefaultState;
            }

            forceMultiplier = 0;
        }
    }
}