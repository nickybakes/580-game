using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{

    public static CameraManager cam;

    public float maxZoom = 12;

    public Vector3 highestPosition;

    public float mapShortestRadius = 30;

    public float playerRadiusZMultiplier = 5;

    public float angleZOffset;

    private new Transform transform;
    private Transform childTransform;

    private Vector3 previousPosition;
    private float previousZoom;


    /// <summary>
    /// This value defines the most amount of shake. This can be like adjustable in the options menu
    /// </summary>
    public float globalShakeMagnitude = 1.1f;

    private float currentShakePercentage;

    private float currentShakeTime;
    public float currentShakeTimeMax = .3f;

    private float currentShakeOffsetCos;
    private float currentShakeOffsetSin;


    public void UpdateCamera(List<PlayerStatus> alivePlayers, List<PlayerStatus> eliminatedPlayers)
    {
        if (alivePlayers == null)
            return;
        // Bounds playerBounds = new Bounds(Vector3.Lerp(minPosition, maxPosition, .5f), Vector3.one * 10);

        Vector3 boundsMin = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);
        Vector3 boundsMax = new Vector3(float.MinValue, float.MinValue, float.MinValue);

        List<Vector3> positions = new List<Vector3>(alivePlayers.Count + eliminatedPlayers.Count);

        foreach (PlayerStatus status in alivePlayers)
        {
            if (status.GetTransform != null)
            {
                positions.Add(status.GetTransform.position);
            }
        }

        foreach (PlayerStatus status in eliminatedPlayers)
        {
            if (status.GetTransform != null && (status.eliminated || status.waitingToBeEliminated))
            {
                float eliminationTimeAmount = GameManager.game.gameTime - status.timeOfEliminiation;

                if (eliminationTimeAmount <= 3)
                {
                    positions.Add(status.GetTransform.position);
                }
                else if (eliminationTimeAmount > 3 && eliminationTimeAmount < 6 && positions.Count > 0)
                {
                    positions.Add(Vector3.Lerp(status.GetTransform.position, positions[0], (GameManager.game.gameTime - status.timeOfEliminiation - 3) / 3f));
                }
            }
        }

        foreach (Vector3 pos in positions)
        {
            boundsMin.x = Mathf.Min(pos.x, boundsMin.x);
            boundsMin.y = Mathf.Min(pos.y, boundsMin.y);
            boundsMin.z = Mathf.Min(pos.z, boundsMin.z);

            boundsMax.x = Mathf.Max(pos.x, boundsMax.x);
            boundsMax.y = Mathf.Max(pos.y, boundsMax.y);
            boundsMax.z = Mathf.Max(pos.z, boundsMax.z);
        }


        //give the bounds some padding near the edges
        // playerBounds.Expand(new Vector3(2, 1, 2));

        Vector3 playerBoundsSize = new Vector3(Mathf.Abs(boundsMax.x - boundsMin.x), Mathf.Abs(boundsMax.y - boundsMin.y), Mathf.Abs(boundsMax.z - boundsMin.z));

        // playerBoundsSize += new Vector3(12, 1, 7);

        float playerRadius = Mathf.Pow(playerBoundsSize.x / 2, 2) + playerRadiusZMultiplier * Mathf.Pow(playerBoundsSize.z / 2, 2);

        // Vector3 normalizedPlayerBounds = new Vector3(playerBoundsSize.x / cameraBoundsSize.x, playerBoundsSize.y / cameraBoundsSize.y, playerBoundsSize.z / cameraBoundsSize.z);

        float zoomPercentage = playerRadius / (Mathf.Pow(mapShortestRadius, 2));

        float targetZoom = Mathf.Lerp(maxZoom, 0, zoomPercentage);

        Vector3 center = Vector3.Lerp(boundsMin, boundsMax, .5f);


        // float zoomAmount = Mathf.Abs(targetZoom - maxZoom);

        // float x = center.x;
        // float y = (maxZoom * Mathf.Sin(Mathf.Deg2Rad * 65)) + center.y;
        // float z = (zoomAmount * Mathf.Cos(Mathf.Deg2Rad * 65));

        // Vector3 targetPosition = new Vector3(Mathf.Clamp(x, minPosition.x, maxPosition.x), minPosition.y, Mathf.Clamp(z, minPosition.z, maxPosition.z));

        // Debug.Log(center);


        // Vector3 targetPosition = Vector3.Lerp(new Vector3(center.x, highestPosition.y, center.z), highestPosition, 0);
        Vector3 targetPosition = new Vector3(center.x, Mathf.Lerp(previousPosition.y, highestPosition.y + center.y, .1f), center.z);
        // float additionalOffset = Mathf.Clamp(angleZOffsetMultiplier*(targetPosition.z + highestPosition.z), 0, 100);
        targetPosition.z -= angleZOffset;

        currentShakeTime = Mathf.Max(0, currentShakeTime - Time.deltaTime);
        float shakeTimePercentage = currentShakeTime / currentShakeTimeMax;
        float shakeAmount = shakeTimePercentage * currentShakePercentage * globalShakeMagnitude;
        childTransform.localPosition = new Vector3(Mathf.Cos(Time.time * 86 + currentShakeOffsetCos) * shakeAmount, Mathf.Sin(Time.time * 86 + currentShakeOffsetSin) * shakeAmount, targetZoom);

        transform.position = targetPosition;

        previousPosition = targetPosition;
    }

    /// <summary>
    /// Shake the camera with a scalable magnitude.
    /// </summary>
    /// <param name="magnitudePercentage">How much the camera should shake. From 0 to 1, but could technically 
    /// be higher than 1 if wanted</param>
    public void ShakeCamera(float magnitudePercentage)
    {
        if (currentShakeTime == 0 || magnitudePercentage >= currentShakePercentage)
        {
            currentShakePercentage = magnitudePercentage;
        }
        currentShakeOffsetCos = Random.value * 3f;
        currentShakeOffsetSin = Random.value * 3f;

        float newShakeTimeMax = .125f + magnitudePercentage * .5f;
        currentShakeTime += newShakeTimeMax;
        currentShakeTimeMax = currentShakeTime;
    }

    // Start is called before the first frame update
    void Start()
    {
        CameraManager.cam = this;
        transform = gameObject.transform;
        childTransform = transform.GetComponentInChildren<Camera>().transform;
        previousPosition = new Vector3(highestPosition.x, highestPosition.y, highestPosition.z);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
