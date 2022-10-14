using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class DecalFade : MonoBehaviour
{

    private const float timeBeforeFadeMax = 15;

    private const float timeDuringFadeMax = 6;

    private bool fading;

    private float currentStateTime;

    private DecalProjector decalProjector;

    // Start is called before the first frame update
    void Start()
    {
        decalProjector = GetComponent<DecalProjector>();
    }

    // Update is called once per frame
    void Update()
    {
        currentStateTime += Time.deltaTime;
        if (fading)
        {
            decalProjector.fadeFactor = 1 - (currentStateTime / timeDuringFadeMax);;

            if (currentStateTime >= timeDuringFadeMax)
            {
                Destroy(gameObject);
            }
        }
        else
        {
            if (currentStateTime >= timeBeforeFadeMax)
            {
                currentStateTime = 0;
                fading = true;
            }
        }
    }
}
