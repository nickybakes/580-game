using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ring : MonoBehaviour
{
    public Transform tr;

    private Vector3 startingSize;
    private Vector3 targetSize;

    private float ringResizeTimeCurrent;
    private float ringResizeTimeMax;

    private bool finalRing;

    private bool finalRingShrinking;

    private float timeInFinalRing;

    // Start is called before the first frame update
    void Awake()
    {
        tr = transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (ringResizeTimeCurrent <= ringResizeTimeMax)
        {
            ringResizeTimeCurrent += Time.deltaTime;

            tr.localScale = Vector3.Lerp(startingSize, targetSize, ringResizeTimeCurrent / ringResizeTimeMax);

            UpdateRingShaderProperties();
        }
        else if (!finalRing)
        {
            finalRing = true;
        }

        if (finalRing)
        {
            timeInFinalRing += Time.deltaTime;
        }

        if (finalRing && !finalRingShrinking)
        {
            if (timeInFinalRing > 7)
            {
                if (GameManager.game.GetTotalActivityTime() <= 0)
                {
                    finalRingShrinking = true;
                    ResizeRing(0, 15);
                }
            }
        }


    }

    public void UpdateRingShaderProperties()
    {
        Shader.SetGlobalVector("ringPosition", new Vector2(tr.position.x, tr.position.z));
        Shader.SetGlobalFloat("ringRadius", tr.localScale.x);
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

        ringResizeTimeCurrent = 0;
        ringResizeTimeMax = time;
    }
}