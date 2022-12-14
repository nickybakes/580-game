using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DecalName
{
    Crack_01
}

public enum ParticleName
{
    SpotlightBuffGet_01,
    AirAttackLand_01,
    Explosion_01,
    Explosion_01_01,
    Explosion_01_02,
    Explosion_01_03,
    Explosion_01_04,
    Explosion_01_05,
    Explosion_01_06,
    Explosion_01_07,
    Explosion_01_08
}

public class VisualsManager : MonoBehaviour
{
    private static VisualsManager vis;

    public GameObject[] decals;

    public GameObject[] particles;

    // Start is called before the first frame update
    void Start()
    {
        vis = this;
    }

    public static GameObject SpawnDecal(DecalName decal, Vector3 pos)
    {
        GameObject g = GameObject.Instantiate(vis.decals[(int)decal]);
        g.transform.position = pos;

        return g;
    }

    public static GameObject SpawnParticle(ParticleName particle, Vector3 pos)
    {
        GameObject g = GameObject.Instantiate(vis.particles[(int)particle]);
        g.transform.position = pos;

        return g;
    }
}
