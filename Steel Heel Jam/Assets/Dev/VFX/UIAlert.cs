using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class UIAlert : MonoBehaviour
{
    [HideInInspector]
    public Transform transformToTrackTo;

    /// <summary>
    /// How long the message should stay still before moving towards its target
    /// and getting destroyed.
    /// Make it 0 to make the alert pop up and stay up forever
    /// </summary>
    public float timeToStayStillMax = 1;
    private float timeToStayStillCurrent;

    /// <summary>
    /// moves this many screens per second (about)
    /// </summary>
    public float moveSpeed = 1.5f;

    public TextMeshProUGUI[] textObjects;

    private float timeToMoveMax;
    private float timeToMoveCurrent;


    private Vector2 startingPositionOnScreen;

    private new Transform transform;

    private RectTransform rectTransform;

    void Start()
    {
        transform = gameObject.transform;
        rectTransform = GetComponent<RectTransform>();

        startingPositionOnScreen = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (timeToStayStillMax != 0)
        {
            if (timeToStayStillCurrent < timeToStayStillMax)
            {
                timeToStayStillCurrent += Time.deltaTime;
            }
            else
            {
                if (timeToMoveCurrent == 0)
                {
                    if (transformToTrackTo)
                    {
                        Vector2 trackingSceenPoint = Camera.main.WorldToViewportPoint(transformToTrackTo.position);

                        Vector2 thisScreenPoint = Camera.main.ScreenToViewportPoint(rectTransform.position);

                        float distance = Vector2.Distance(trackingSceenPoint, thisScreenPoint);

                        timeToMoveMax = Mathf.Max(distance / moveSpeed, .4f);
                    }
                    else
                    {
                        timeToMoveMax = .4f;
                    }
                }

                timeToMoveCurrent += Time.deltaTime;

                if (transformToTrackTo)
                {
                    transform.position = Vector2.Lerp(startingPositionOnScreen, Camera.main.WorldToScreenPoint(transformToTrackTo.position), timeToMoveCurrent / timeToMoveMax);
                }

                float scale = (timeToMoveMax - timeToMoveCurrent) / timeToMoveMax;
                rectTransform.localScale = new Vector3(scale, scale, scale);

                if (timeToMoveCurrent > timeToMoveMax)
                {
                    Destroy(gameObject);
                }
            }
        }
    }
}
