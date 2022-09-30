using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestShaderGlobalFloat : MonoBehaviour
{

    public Vector2 ringPosition = new Vector2(0, 0);
    public float ringRadius = 15;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        Shader.SetGlobalFloat("ringRadius", ringRadius);
        Shader.SetGlobalVector("ringPosition", ringPosition);
    }
}
