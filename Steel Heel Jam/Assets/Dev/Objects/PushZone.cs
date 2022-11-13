using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushZone : MonoBehaviour
{

    public Vector2 topDownDirection = new Vector2(0, 1);

    public float pushForce = 5;


    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag(Tag.Player.ToString()))
        {
            PlayerStatus player = other.GetComponent<PlayerStatus>();

            player.movement.velocity = new Vector3(topDownDirection.x * pushForce, 0, topDownDirection.y * pushForce);
        }
        if (other.CompareTag(Tag.PickUp.ToString()))
        {
            GameObject pickupObject = other.gameObject;

            Rigidbody rb = pickupObject.GetComponent<Rigidbody>();

            if (rb != null)
            {
                rb.AddForce(new Vector3(topDownDirection.x * pushForce, 8, topDownDirection.y * pushForce));
            }
        }
    }
}
