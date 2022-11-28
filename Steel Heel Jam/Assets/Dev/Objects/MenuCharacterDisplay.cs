using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct CharacterVisualPrefs
{
    public int skinToneIndex;
    public int hairStyleIndex;
    public int hairColorIndex;

    public CharacterVisualPrefs(int _skinToneIndex = 0, int _hairStyleIndex = 0, int _hairColorIndex = 0)
    {
        skinToneIndex = _skinToneIndex;
        hairStyleIndex = _hairStyleIndex;
        hairColorIndex = _hairColorIndex;
    }
}

public class MenuCharacterDisplay : MonoBehaviour
{


    [Range(1, 8)]
    public int playerNumber = 1;

    public Material enabledPlayerMaterial;
    public Material disabledPlayerMaterial;
    public Material outlinePlayerMaterial;

    public SkinnedMeshRenderer characterMesh;
    public SkinnedMeshRenderer outlineMesh;

    public CharacterVisualPrefs currentVisualPrefs;

    public Material[] hairMaterialsSolid;
    public Material[] hairMaterialsHologram;

    public int[] hairMaterialIndices;

    public GameObject[] hairObjectParents;
    public MeshRenderer[] hairMeshes;
    public MeshRenderer[] hairOutlineMeshes;

    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        enabledPlayerMaterial = new Material(enabledPlayerMaterial);
        disabledPlayerMaterial = new Material(disabledPlayerMaterial);

        FacePoser facePoser = GetComponentInChildren<FacePoser>();
        facePoser.materials = new[] { enabledPlayerMaterial, disabledPlayerMaterial };
        facePoser.ForceUpdateMaterial();

        for (int i = 0; i < hairMaterialsSolid.Length; i++)
        {
            hairMaterialsSolid[i] = new Material(hairMaterialsSolid[i]);
            hairMaterialsHologram[i] = new Material(hairMaterialsHologram[i]);
        }

        // disabledPlayerMaterial.SetColor("_Tint", PlayerToken.colors[playerNumber - 1]);
        enabledPlayerMaterial.SetFloat("_Player_Index", playerNumber - 1);
        disabledPlayerMaterial.SetFloat("_Player_Index", playerNumber - 1);
        for (int i = 0; i < hairMaterialsSolid.Length; i++)
        {
            hairMaterialsSolid[i].SetFloat("_Player_Index", playerNumber - 1);
            hairMaterialsHologram[i].SetFloat("_Player_Index", playerNumber - 1);
        }

        if (AppManager.app.playerTokens[playerNumber - 1] && AppManager.app.playerTokens[playerNumber - 1].playerPrefsSet)
        {
            SetVisualPrefs(AppManager.app.playerTokens[playerNumber - 1].visualPrefs);

            SolidDisplay();
        }
        else
        {
            RandomizePrefs(10);

            UpdateAllMeshes();

            SolidDisplay();
        }

        animator = GetComponentInChildren<Animator>();
        animator.SetInteger("PlayerNumber", playerNumber);

    }

    private float outlineTime;

    void Update()
    {
        outlineTime += Time.deltaTime;

        if (outlineTime >= .2)
        {
            outlineTime = 0;
            for (int i = 0; i < hairOutlineMeshes.Length; i++)
            {
                hairOutlineMeshes[i].material.SetFloat("_PanX", Random.value);
                hairOutlineMeshes[i].material.SetFloat("_PanY", Random.value);
            }
            outlineMesh.material.SetFloat("_PanX", Random.value);
            outlineMesh.material.SetFloat("_PanY", Random.value);
        }

    }

    public void RandomizePrefs(int skinToneLimit)
    {
        currentVisualPrefs = new CharacterVisualPrefs(Random.Range(0, skinToneLimit), Random.Range(0, 7), Random.Range(0, 16));
    }

    public void SetVisualPrefs(CharacterVisualPrefs prefs)
    {
        currentVisualPrefs = prefs;
        UpdateAllMeshes();
    }

    public void UpdateAllMeshes()
    {
        disabledPlayerMaterial.SetFloat("_Skin_Tone", currentVisualPrefs.skinToneIndex);
        enabledPlayerMaterial.SetFloat("_Skin_Tone", currentVisualPrefs.skinToneIndex);

        for (int i = 0; i < hairMaterialsSolid.Length; i++)
        {
            hairMaterialsSolid[i].SetFloat("_Hair_Color", currentVisualPrefs.hairColorIndex);
            hairMaterialsHologram[i].SetFloat("_Hair_Color", currentVisualPrefs.hairColorIndex);
        }

        for (int i = 0; i < hairObjectParents.Length; i++)
        {
            hairObjectParents[i].SetActive(false);
        }

        hairObjectParents[currentVisualPrefs.hairStyleIndex].SetActive(true);
    }

    public void HologramDisplay()
    {
        if (characterMesh == null)
            return;

        characterMesh.material = disabledPlayerMaterial;
        outlineMesh.gameObject.SetActive(false);

        for (int i = 0; i < hairMeshes.Length; i++)
        {
            hairMeshes[i].material = hairMaterialsHologram[hairMaterialIndices[i]];
            hairOutlineMeshes[i].gameObject.SetActive(false);
        }
    }

    public void SolidDisplay()
    {
        characterMesh.material = enabledPlayerMaterial;
        outlineMesh.gameObject.SetActive(true);

        for (int i = 0; i < hairMeshes.Length; i++)
        {
            hairMeshes[i].material = hairMaterialsSolid[hairMaterialIndices[i]];
            hairOutlineMeshes[i].gameObject.SetActive(true);
        }
    }
}
