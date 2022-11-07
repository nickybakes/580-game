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

    private float timeIdleInFinalRing;

    // Start is called before the first frame update
    void Awake()
    {
        tr = transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (!finalRing && ringResizeTimeCurrent <= ringResizeTimeMax)
        {
            ringResizeTimeCurrent += Time.deltaTime;

            tr.localScale = Vector3.Lerp(startingSize, targetSize, ringResizeTimeCurrent / ringResizeTimeMax);

            UpdateRingShaderProperties();
        }
        else if (!finalRing)
        {
            finalRing = true;
        }

        if (finalRing && GameManager.game.GetTotalActivityTime() <= 0)
        {
            timeIdleInFinalRing += Time.deltaTime;
            if (timeIdleInFinalRing > 7)
            {
                Vector3 currentScale = tr.localScale;
                float newScale = Mathf.Max(currentScale.x - (Time.deltaTime * .7f), 0);
                
                tr.localScale = new Vector3(newScale, currentScale.y, newScale);

                UpdateRingShaderProperties();
            }
        }
        else if (finalRing)
        {
            timeIdleInFinalRing = 0;
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
