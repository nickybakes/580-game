using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIOffScreenIndicator : MonoBehaviour
{
    [HideInInspector]
    public Transform transformToTrackTo;

    public float distanceFromEdgeOfScreen;

    public GameObject graphicToShow;

    public Transform arrowToRotate;

    private new Transform transform;

    private RectTransform canvasRect;

    private float aspectRatio = 16f / 9f;

    void Start()
    {
        transform = gameObject.transform;
        canvasRect = HUDManager.hud.canvas.GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        if (transformToTrackTo)
        {
            Vector2 directionToPoint = Camera.main.WorldToViewportPoint(transformToTrackTo.position);
            bool onScreen = directionToPoint.x > 0 && directionToPoint.x < 1 && directionToPoint.y > 0 && directionToPoint.y < 1;

            if (!onScreen)
            {
                if (!graphicToShow.activeSelf)
                    graphicToShow.SetActive(true);

                directionToPoint.x -= .5f;
                directionToPoint.y -= .5f;
                directionToPoint *= 2;

                Rect rect = canvasRect.rect;

                aspectRatio = rect.width / rect.height;

                directionToPoint.y *= aspectRatio;

                directionToPoint.Normalize();

                Vector2 screenPoint = directionToPoint;

                if (Mathf.Abs(screenPoint.y) > Mathf.Abs(screenPoint.x))
                {
                    float yMultiply = 1 / Mathf.Abs(screenPoint.y);
                    screenPoint *= yMultiply;
                }
                else
                {
                    float xMultiply = 1 / Mathf.Abs(screenPoint.x);
                    screenPoint *= xMultiply;
                }

                float x = rect.width / 2;
                float y = rect.height / 2;

                transform.position = new Vector3(x + screenPoint.x * (x - distanceFromEdgeOfScreen), y + screenPoint.y * (y - distanceFromEdgeOfScreen), 0);

                if (directionToPoint.x < 0)
                {
                    arrowToRotate.localScale = new Vector3(-1, 1, 1);
                }
                else if (directionToPoint.x > 0)
                {
                    arrowToRotate.localScale = new Vector3(1, 1, 1);
                }
                arrowToRotate.rotation = Quaternion.Euler(new Vector3(transform.rotation.x, transform.rotation.y, Mathf.Rad2Deg * Mathf.Asin(directionToPoint.y)));

            }
            else if (graphicToShow.activeSelf)
            {
                graphicToShow.SetActive(false);
            }
        }
    }
}
