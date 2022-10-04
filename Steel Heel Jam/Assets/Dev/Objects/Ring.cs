using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ring : MonoBehaviour
{
    Transform tr;

    private Vector3 startingSize;
    private Vector3 targetSize;
    private float resizeMagnitude;

    private float time;
    private float resizeTimer;
    
    // Start is called before the first frame update
    void Start()
    {
        tr = transform;

        ResizeRing(5, 0.2f);
    }

    // Update is called once per frame
    void Update()
    {
        if (resizeTimer > 0)
        {
            float size = resizeMagnitude * Time.deltaTime * time;
            //print(size);
            
        }

        transform.localScale = Vector3.Lerp(transform.localScale, targetSize, time * Time.deltaTime);
        print(transform.localScale);
    }

    /// <summary>
    /// Resizes the ring to the specified size.
    /// </summary>
    /// <param name="radius">The new radius of the ring.</param>
    /// <param name="time">The time it takes for the ring to fully resize.</param>
    public void ResizeRing(float size, float time)
    {
        startingSize = tr.localScale;
        targetSize = new Vector3(size, tr.localScale.y, size);
        //resizeMagnitude = targetSize - startingSize;

        this.time = time;
        resizeTimer = time;
    }
}
