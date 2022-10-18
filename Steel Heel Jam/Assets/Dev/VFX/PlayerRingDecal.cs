using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class PlayerRingDecal : MonoBehaviour
{

    private DecalProjector decal;

    // Start is called before the first frame update
    void Start()
    {
        decal = GetComponent<DecalProjector>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetTint(int playerNumber){
        decal = GetComponent<DecalProjector>();
        decal.material = new Material(decal.material);
        decal.material.SetColor("_Tint", PlayerToken.colors[playerNumber - 1]);
    }
}
