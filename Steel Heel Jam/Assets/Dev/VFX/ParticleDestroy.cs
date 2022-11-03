using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleDestroy : MonoBehaviour
{
    private new Transform transform;

    void Start()
    {
        transform = gameObject.transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.childCount == 0)
            Destroy(gameObject);
    }
}
