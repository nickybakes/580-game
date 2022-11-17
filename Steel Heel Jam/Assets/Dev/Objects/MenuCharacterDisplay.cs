using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct CharacterVisualPrefs
{
    public int skinToneIndex;

    public CharacterVisualPrefs(int _skinToneIndex = -1)
    {
        skinToneIndex = _skinToneIndex;
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

    // Start is called before the first frame update
    void Start()
    {
        enabledPlayerMaterial = new Material(enabledPlayerMaterial);
        disabledPlayerMaterial = new Material(disabledPlayerMaterial);

        disabledPlayerMaterial.SetColor("_Tint", PlayerToken.colors[playerNumber - 1]);
        enabledPlayerMaterial.SetFloat("_Player_Index", playerNumber - 1);
        disabledPlayerMaterial.SetFloat("_Player_Index", playerNumber - 1);

        currentVisualPrefs = new CharacterVisualPrefs(Random.Range(0, 16));

        HologramDisplay();
    }

    public void SetSkinToneIndex(int skinToneIndex)
    {
        currentVisualPrefs.skinToneIndex = skinToneIndex;

        UpdateAllMeshes();
    }

    public void UpdateAllMeshes()
    {
        disabledPlayerMaterial.SetFloat("_Skin_Tone", currentVisualPrefs.skinToneIndex);
        enabledPlayerMaterial.SetFloat("_Skin_Tone", currentVisualPrefs.skinToneIndex);

    }

    public void HologramDisplay()
    {
        disabledPlayerMaterial.SetFloat("_Skin_Tone", currentVisualPrefs.skinToneIndex);
        characterMesh.material = disabledPlayerMaterial;
        outlineMesh.gameObject.SetActive(false);
    }

    public void SolidDisplay()
    {
        enabledPlayerMaterial.SetFloat("_Skin_Tone", currentVisualPrefs.skinToneIndex);
        characterMesh.material = enabledPlayerMaterial;
        outlineMesh.gameObject.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {

    }
}