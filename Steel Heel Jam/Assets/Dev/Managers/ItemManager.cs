using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class ItemManager : MonoBehaviour
{
    public List<GameObject> itemsOnGround;
    List<GameObject> itemsToSpawn;
    public GameObject testCube;
    float spawnTimer;
    float spawnTimerMax;
    Random r = new Random();

    // Start is called before the first frame update
    void Start()
    {
        spawnTimer = 0f;
        spawnTimerMax = 10f;
        itemsOnGround = new List<GameObject>();
        itemsToSpawn = new List<GameObject>();
        // Add any new objects added to the game to the list below
        itemsToSpawn.Add(testCube);
    }

    // Update is called once per frame
    void Update()
    {
        // spawnTimer += Time.deltaTime;
        // if (spawnTimer >= spawnTimerMax)
        // {
        //     spawnRandomItem();
        //     spawnTimer = 0;
        // }
    }

    void spawnRandomItem()
    {
        int randomItemIndex = r.Next(0, itemsToSpawn.Count - 1);
        int randomXCoordinate = r.Next(-40, 40);
        int randomZCoordinate = r.Next(-40, 40);
        Vector3 spawnLocation = new Vector3(randomXCoordinate, 1, randomZCoordinate);
        GameObject newItem = Instantiate(itemsToSpawn[randomItemIndex], spawnLocation, Quaternion.identity);
        itemsOnGround.Add(newItem);
    }

    public void deleteItemAndRemoveFromList(int indexToDelete)
    {
        Destroy(itemsOnGround[indexToDelete]);
        itemsOnGround.RemoveAt(indexToDelete);
    }
}
