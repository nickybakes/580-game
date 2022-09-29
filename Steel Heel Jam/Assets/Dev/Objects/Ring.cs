using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ring : MonoBehaviour
{
    [SerializeField] public float startingRadius = 30.0f;
    private float radius;

    private float resizeTimer;

    // Start is called before the first frame update
    void Start()
    {
        radius = startingRadius;
    }

    // Update is called once per frame
    void Update()
    {
        if (resizeTimer > 0)
        {
            float size = ((radius * 2) - transform.localScale.x) * Time.deltaTime;
            transform.localScale = new Vector3(size, transform.localScale.y, size);
        }
    }

    /// <summary>
    /// Resizes the ring to the specified size.
    /// </summary>
    /// <param name="radius">The new radius of the ring.</param>
    /// <param name="time">The time it takes for the ring to fully resize.</param>
    public void ResizeRing(float radius, float time)
    {
        this.radius = radius;
        resizeTimer = time;
    }
}
