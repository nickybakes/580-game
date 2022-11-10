using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSceneSettings : MonoBehaviour
{

    //this script will contain the references to the specific player spawn locations
    //and item spawn locations

    public Transform playerSpawnsFor2;
    public Transform playerSpawnsFor3;
    public Transform playerSpawnsFor4;
    public Transform playerSpawnsFor5;
    public Transform playerSpawnsFor6;
    public Transform playerSpawnsFor7;
    public Transform playerSpawnsFor8;
    public Transform ringCenters;
    public Transform heelSpotlightStart;

    public Vector3 minItemSpawnBoundaries = new Vector3(-100, 0, -100);
    public Vector3 maxItemSpawnBoundaries = new Vector3(100, 100, 100);

    /// <summary>
    /// Used for the intro sequence to show the map's name
    /// </summary>
    public GameObject mapNameAlert;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
