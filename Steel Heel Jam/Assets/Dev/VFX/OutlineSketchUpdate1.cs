using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class OutlineSketchUpdate1 : MonoBehaviour
{

    public MeshRenderer[] innerOutlineMeshes;
    // public MeshRenderer innerOutlineMesh2;
    public MeshRenderer[] outerOutlineMeshes;
    // public MeshRenderer outerOutlineMesh2;

    private float time;
    public float maxFrameTime = .2f;


    // Start is called before the first frame update
    void Start()
    {
        time = 0;
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;

        if (time >= maxFrameTime)
        {
            time = 0;
            UpdateOutline();
        }
    }

    public void SetBaseColor(int playerNumber)
    {
        foreach (MeshRenderer m in innerOutlineMeshes)
        {
            m.material.SetColor("_BaseColor", PlayerToken.colors[playerNumber - 1]);
        }
        // outerOutlineMesh.material.SetColor("_BaseColor", PlayerToken.colors[playerNumber - 1]);
    }

    void UpdateOutline()
    {
        foreach (MeshRenderer m in innerOutlineMeshes)
        {
            m.material.SetFloat("_PanX", Random.value);
            m.material.SetFloat("_PanY", Random.value);
        }

        foreach (MeshRenderer m in outerOutlineMeshes)
        {
            m.material.SetFloat("_PanX", Random.value);
            m.material.SetFloat("_PanY", Random.value);
        }
    }
}
