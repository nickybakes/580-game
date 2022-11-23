using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHairController : MonoBehaviour
{
    public Material[] hairMaterials;

    public int[] hairMaterialIndices;

    public GameObject[] hairObjectParents;
    public MeshRenderer[] hairMeshes;

    public MeshRenderer[] hairOutlineMeshes;

    private Material outlineMaterial;


    // Start is called before the first frame update
    void Start()
    {

        for (int i = 0; i < hairMaterials.Length; i++)
        {
            hairMaterials[i] = new Material(hairMaterials[i]);
        }
    }

    private float outlineTime;

    void Update()
    {
        outlineTime += Time.deltaTime;

        if (outlineTime >= .2)
        {
            outlineTime = 0;
            outlineMaterial.SetFloat("_PanX", Random.value);
            outlineMaterial.SetFloat("_PanY", Random.value);
        }

    }

    public void SetHairPrefs(int hairStyle, int hairColor, int playerNumber)
    {
        Material hairMaterial = new(hairMaterials[hairMaterialIndices[hairStyle]]);
        hairMaterial.SetFloat("_Player_Index", playerNumber - 1);
        hairMaterial.SetFloat("_Hair_Color", hairColor);

        hairMeshes[hairStyle].material = hairMaterial;

        for (int i = 0; i < hairObjectParents.Length; i++)
        {
            hairObjectParents[i].SetActive(false);
        }

        hairObjectParents[hairStyle].SetActive(true);
        outlineMaterial = new(hairOutlineMeshes[hairStyle].material);
        hairOutlineMeshes[hairStyle].material = outlineMaterial;

    }
}
