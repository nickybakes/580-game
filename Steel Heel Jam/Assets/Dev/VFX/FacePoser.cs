using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FacePoser : MonoBehaviour
{

    private static float[] eyePositionsX = { .37f, .57f, .8f };
    private static float[] eyePositionsY = { 0.75f, .5f };

    public SkinnedMeshRenderer meshRenderer;
    public Material[] materials;
    public bool useMaterialList;

    private float blinkTime;
    private float blinkTimeMax;

    private float blinkHoldTime;
    private float blinkHoldTimeMax;

    private int currentEyeL = 4;
    private int currentEyeR = 4;
    private int currentMouth = 3;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (blinkHoldTime > 0)
        {
            blinkHoldTime -= Time.deltaTime;

            if (blinkHoldTime <= 0)
            {
                ReturnFromBlink();
            }
        }
        else
        {
            blinkTime -= Time.deltaTime;

            if (blinkTime <= 0)
            {
                ForceBlink();
            }
        }
    }





    public void ForceBlink()
    {
        blinkHoldTimeMax = Random.Range(.2f, .6f);
        blinkHoldTime = blinkHoldTimeMax;
        blinkTimeMax = Random.Range(.5f, 6);
        blinkTime = blinkTimeMax;
        int l = currentEyeL;
        int r = currentEyeR;
        SetEyes(5);
        currentEyeL = l;
        currentEyeR = r;
    }

    public void ReturnFromBlink()
    {
        SetEyeR(currentEyeR);
        SetEyeL(currentEyeL);
    }

    public void ForceUpdateMaterial()
    {
        SetEyeR(currentEyeR);
        SetEyeL(currentEyeL);
    }


    public void SetEyeR(int eyeIndex)
    {
        currentEyeR = eyeIndex;
        SetPropEyeR(eyePositionsX[eyeIndex % 3], eyePositionsY[eyeIndex / 3]);
    }

    public void SetEyeL(int eyeIndex)
    {
        currentEyeL = eyeIndex;
        SetPropEyeL(eyePositionsX[eyeIndex % 3], eyePositionsY[eyeIndex / 3]);
    }


    public void SetEyes(int eyeIndex)
    {
        currentEyeL = eyeIndex;
        currentEyeR = eyeIndex;

        SetPropEyeR(eyePositionsX[eyeIndex % 3], eyePositionsY[eyeIndex / 3]);
        SetPropEyeL(eyePositionsX[eyeIndex % 3], eyePositionsY[eyeIndex / 3]);
    }

    private void SetPropMouth(float x, float y)
    {
        SetMaterialProperty("_Mouth_Offset", x, y);
    }

    private void SetropPupilR(float x, float y)
    {
        SetMaterialProperty("_Pupil_Offset_R", x, y);
    }

    private void SetPropPupilL(float x, float y)
    {
        SetMaterialProperty("_Pupil_Offset_L", x, y);
    }

    private void SetPropEyeR(float x, float y)
    {
        SetMaterialProperty("_Eye_Offset_R", x, y);
    }

    private void SetPropEyeL(float x, float y)
    {
        SetMaterialProperty("_Eye_Offset_L", x, y);
    }

    private void SetMaterialProperty(string property, float x, float y)
    {
        if (useMaterialList)
        {
            foreach (Material m in materials)
            {
                m.SetVector(property, new Vector2(x, y));
            }
        }
        else if (meshRenderer)
        {
            meshRenderer.material.SetVector(property, new Vector2(x, y));
        }
    }
}
