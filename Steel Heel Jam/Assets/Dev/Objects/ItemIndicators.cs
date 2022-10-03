using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemIndicators : MonoBehaviour
{
    private ItemManager itemManagerScript;

    // Start is called before the first frame update
    void Start()
    {
        // Since both are prefabs...
        itemManagerScript = GameObject.FindGameObjectWithTag("ItemManager").GetComponent<ItemManager>();
    }

    // Update is called once per frame
    void Update()
    {
        //for (int i = 0; i < itemManagerScript.itemsOnGround.Count; i++)
        //{
        //    if (!itemManagerScript.itemsOnGround[i].GetComponent<Item>().isOnScreen)
        //    {
        //        // Create a new GUI arrow.

        //        // Calculate direction to point arrow.
        //        NormalVectorToItem();

        //        // Calculate edge of screen.
        //    }
        //}
    }

    /// <summary>
    /// Calculates the normal vec3 from the middle of the camera projection to the item.
    /// </summary>
    /// <returns>Normalized vector 3 pointing towards the item.</returns>
    private Vector3 NormalVectorToItem()
    {
        return new Vector3();
    }
}
