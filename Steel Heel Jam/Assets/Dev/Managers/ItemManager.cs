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
        spawnTimerMax = 5f;
        //spawnTimerMax = 1f;
        //itemsOnGround = new List<GameObject>();
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
        int randomItemIndex = r.Next(0, itemsToSpawn.Count);
        //int randomXCoordinate = r.Next(-40, 40);
        //int randomZCoordinate = r.Next(-40, 40);
        float radius = GameManager.game.ringScript.tr.localScale.x/2;
        Debug.Log(radius);
        float randomXCoordinate = NextFloat(GameManager.game.ringScript.transform.position.x - radius - 2f, GameManager.game.ringScript.transform.position.x + radius - 2f);
        float randomZCoordinate = NextFloat(GameManager.game.ringScript.transform.position.z - radius - 2f, GameManager.game.ringScript.transform.position.z + radius - 2f);
        Vector3 spawnLocation = new Vector3(randomXCoordinate, 1, randomZCoordinate);
        GameObject newItem = Instantiate(itemsToSpawn[randomItemIndex], spawnLocation, Quaternion.identity);
        itemsOnGround.Add(newItem);
    }

    private float NextFloat(float min, float max)
    {
        double val = (r.NextDouble() * (max - min) + min);
        return (float)val;
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
