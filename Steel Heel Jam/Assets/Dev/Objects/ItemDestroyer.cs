using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDestroyer : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other) {
        if(other.CompareTag(Tag.PickUp.ToString())){
            GameManager.game.itemManager.itemsOnGround.Remove(other.gameObject);
            Destroy(other.gameObject);
        }
    }
}
