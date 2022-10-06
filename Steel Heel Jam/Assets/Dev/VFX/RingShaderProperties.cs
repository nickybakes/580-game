using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RingShaderProperties : MonoBehaviour
{

    public Vector2 ringPosition = new Vector2(0, 0);
    public float ringRadius = 40;

    // Start is called before the first frame update
    void Start()
    {
    }

    public void RetrieveRingProperties(){
        ringPosition = Shader.GetGlobalVector("ringPosition");
        ringRadius = Shader.GetGlobalFloat("ringRadius");
    }

    public void UpdateRingProperties()
    {
        Shader.SetGlobalVector("ringPosition", ringPosition);
        Shader.SetGlobalFloat("ringRadius", ringRadius);
    }

    // Update is called once per frame
    void Update()
    {
        UpdateRingProperties();
    }
}
