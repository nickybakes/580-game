using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ring : MonoBehaviour
{
    float startingRadius = 30.0f;
    float currenRadius = 30.0f;

    private Transform effTransform;

    // Start is called before the first frame update
    void Start()
    {
        effTransform = transform;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
