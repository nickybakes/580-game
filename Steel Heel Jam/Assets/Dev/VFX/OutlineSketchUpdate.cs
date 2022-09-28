using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class OutlineSketchUpdate : MonoBehaviour
{

    public SkinnedMeshRenderer innerOutlineMesh;
    public SkinnedMeshRenderer outerOutlineMesh;

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

    void UpdateOutline()
    {
        if (!innerOutlineMesh || !outerOutlineMesh)
            return;

        innerOutlineMesh.material.SetFloat("_PanX", Random.value);
        innerOutlineMesh.material.SetFloat("_PanY", Random.value);

        outerOutlineMesh.material.SetFloat("_PanX", Random.value);
        outerOutlineMesh.material.SetFloat("_PanY", Random.value);
    }
}
