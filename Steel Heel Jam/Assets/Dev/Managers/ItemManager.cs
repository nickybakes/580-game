using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class ItemManager : MonoBehaviour
{
    public List<GameObject> itemsOnGround;
    public List<GameObject> itemsToSpawn = new List<GameObject>();
    float spawnTimer;
    float spawnTimerMax;
    Random r = new Random();

    // Start is called before the first frame update
    void Start()
    {
        spawnTimer = 0f;
        spawnTimerMax = 10f;
        itemsOnGround = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
         spawnTimer += Time.deltaTime;
         if (spawnTimer >= spawnTimerMax)
         {
             SpawnRandomItem();
             spawnTimer = 0;
         }
    }

    void SpawnRandomItem()
    {
        int randomItemIndex = r.Next(0, itemsToSpawn.Count - 1);
        int randomXCoordinate = r.Next(-40, 40);
        int randomZCoordinate = r.Next(-40, 40);
        Vector3 spawnLocation = new Vector3(randomXCoordinate, 1, randomZCoordinate);
        GameObject newItem = Instantiate(itemsToSpawn[randomItemIndex], spawnLocation, Quaternion.identity);
        itemsOnGround.Add(newItem);
    }

    public string DeleteItemAndRemoveFromList(int indexToDelete)
    {
        string itemTag = itemsOnGround[indexToDelete].tag;
        Destroy(itemsOnGround[indexToDelete]);
        itemsOnGround.RemoveAt(indexToDelete);
        return itemTag;
    }

    public void SpawnThrownItem(string itemTag, Transform playerTransform, float forceMultiplier)
    {
        GameObject prefabToSpawn = null;
        for (int i = 0; i < itemsToSpawn.Count; i++)
        {
            if(itemsToSpawn[i].tag == itemTag)
            {
                prefabToSpawn = itemsToSpawn[i];
            }
        }
        GameObject thrownItem = Instantiate(prefabToSpawn, playerTransform.position, playerTransform.rotation);
        thrownItem.GetComponent<Rigidbody>().AddForce(thrownItem.GetComponent<Rigidbody>().transform.position * forceMultiplier);
    }
}
