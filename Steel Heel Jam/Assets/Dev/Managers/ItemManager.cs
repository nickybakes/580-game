using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    public List<GameObject> itemsOnGround;
    public List<GameObject> itemsToSpawn = new List<GameObject>();
    float spawnTimer;
    float spawnTimerMax;
    float spawnCap = 20f;

    // Start is called before the first frame update
    void Start()
    {
        spawnTimer = 0f;
        spawnTimerMax = 5f;
        //spawnTimerMax = 1f;
        //itemsOnGround = new List<GameObject>();
        for (int i = 0; i < 7; i++)
        {
            //SpawnRandomItemCenter(20);
        }

        // Turn off disabled items
        GameSettings itemSettings = AppManager.app.gameSettings;
        List<GameObject> itemsToDisable = new List<GameObject>();
        if (!itemSettings.sabre) itemsToDisable.Add(itemsToSpawn[0]);
        if (!itemSettings.boxingGlove) itemsToDisable.Add(itemsToSpawn[1]);
        if (!itemSettings.boombox) itemsToDisable.Add(itemsToSpawn[2]);
        if (!itemSettings.baseballBat) itemsToDisable.Add(itemsToSpawn[3]);
        if (!itemSettings.ladder) itemsToDisable.Add(itemsToSpawn[4]);
        if (!itemSettings.steelChair) itemsToDisable.Add(itemsToSpawn[5]);
        if (!itemSettings.explosiveBarrel) itemsToDisable.Add(itemsToSpawn[6]);
        if (!itemSettings.leadPipe) itemsToDisable.Add(itemsToSpawn[7]);
        foreach (GameObject item in itemsToDisable)
        {
            itemsToSpawn.Remove(item);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.game.gameWon || GameManager.game.dontUpdateGameplay)
            return;

        spawnTimer += Time.deltaTime;

        if (itemsOnGround.Count >= spawnCap)
        {
            spawnTimer = 0;
        }

        if (spawnTimer >= spawnTimerMax + (10 * Mathf.Clamp01(GameManager.game.gameTime / GameManager.game.maxGameTime)))
        {
            SpawnRandomItem();
            spawnTimer = 0;
        }
    }

    void SpawnRandomItemCenter(float radius)
    {
        int randomItemIndex = Random.Range(0, itemsToSpawn.Count);
        //int randomXCoordinate = r.Next(-40, 40);
        //int randomZCoordinate = r.Next(-40, 40);
        Vector3 center = Vector3.Lerp(GameManager.game.gameSceneSettings.minItemSpawnBoundaries, GameManager.game.gameSceneSettings.maxItemSpawnBoundaries, .5f);
        float xMin = Mathf.Max(center.x - radius, GameManager.game.gameSceneSettings.minItemSpawnBoundaries.x);
        float xMax = Mathf.Min(center.x + radius, GameManager.game.gameSceneSettings.maxItemSpawnBoundaries.x);
        float zMin = Mathf.Max(center.z - radius, GameManager.game.gameSceneSettings.minItemSpawnBoundaries.z);
        float zMax = Mathf.Min(center.z + radius, GameManager.game.gameSceneSettings.maxItemSpawnBoundaries.z);
        float randomXCoordinate = Random.Range(xMin, xMax);
        float randomZCoordinate = Random.Range(zMin, zMax);
        Vector3 spawnLocation = new Vector3(randomXCoordinate, Random.Range(GameManager.game.gameSceneSettings.minItemSpawnBoundaries.y, GameManager.game.gameSceneSettings.maxItemSpawnBoundaries.y), randomZCoordinate);
        GameObject newItem = Instantiate(itemsToSpawn[randomItemIndex], spawnLocation, Quaternion.identity);
        itemsOnGround.Add(newItem);
    }

    void SpawnRandomItem()
    {
        if (itemsToSpawn.Count == 0) return;
        int randomItemIndex = Random.Range(0, itemsToSpawn.Count);
        //int randomXCoordinate = r.Next(-40, 40);
        //int randomZCoordinate = r.Next(-40, 40);
        float radius = (GameManager.game.ringScript.tr.localScale.x / 2) + 8;
        float xMin = Mathf.Max(GameManager.game.ringScript.transform.position.x - radius, GameManager.game.gameSceneSettings.minItemSpawnBoundaries.x);
        float xMax = Mathf.Min(GameManager.game.ringScript.transform.position.x + radius, GameManager.game.gameSceneSettings.maxItemSpawnBoundaries.x);
        float zMin = Mathf.Max(GameManager.game.ringScript.transform.position.z - radius, GameManager.game.gameSceneSettings.minItemSpawnBoundaries.z);
        float zMax = Mathf.Min(GameManager.game.ringScript.transform.position.z + radius, GameManager.game.gameSceneSettings.maxItemSpawnBoundaries.z);
        float randomXCoordinate = Random.Range(xMin, xMax);
        float randomZCoordinate = Random.Range(zMin, zMax);
        Vector3 spawnLocation = new Vector3(randomXCoordinate, Random.Range(GameManager.game.gameSceneSettings.minItemSpawnBoundaries.y, GameManager.game.gameSceneSettings.maxItemSpawnBoundaries.y), randomZCoordinate);
        GameObject newItem = Instantiate(itemsToSpawn[randomItemIndex], spawnLocation, Quaternion.identity);
        itemsOnGround.Add(newItem);
    }

    /*public string DeleteItemAndRemoveFromList(int indexToDelete)
    {
        string itemTag = itemsOnGround[indexToDelete].tag;
        Destroy(itemsOnGround[indexToDelete]);
        itemsOnGround.RemoveAt(indexToDelete);
        return itemTag;
    }*/

    /*public void SpawnThrownItem(string itemTag, Transform playerTransform, float forceMultiplier)
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
        Vector3 targetPos = new Vector3(0, 5, 0);
        Vector3 throwDirection = targetPos - thrownItem.GetComponent<Rigidbody>().transform.position;
        throwDirection.Normalize();
        thrownItem.GetComponent<Rigidbody>().AddForce(throwDirection * forceMultiplier);
        itemsOnGround.Add(thrownItem);
    }*/
}
