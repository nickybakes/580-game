using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DecalName
{
    Crack_01
}

public class VisualsManager : MonoBehaviour
{

    private static VisualsManager vis;

    public GameObject[] decals;

    // Start is called before the first frame update
    void Start()
    {
        vis = this;
    }

    public static GameObject SpawnDecal(DecalName decal, Vector3 pos)
    {
        GameObject g = GameObject.Instantiate(vis.decals[(int) decal]);
        g.transform.position = pos;

        return g;
    }
}
