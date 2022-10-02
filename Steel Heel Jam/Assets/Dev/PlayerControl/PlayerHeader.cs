using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerHeader : MonoBehaviour
{

    private PlayerStatus playerStatus;

    private Canvas canvas;

    private Transform tr;

    private Transform playerTransform;

    public TextMeshProUGUI playerNumberText;
    public GameObject heelText;
    public GameObject dangerText;

    public Image playerNumberBackground;
    public Image staminaBarFill;

    private RectTransform staminaBarFillRect;
    public Image staminaBarBorder;

    private RectTransform staminaBarBorderRect;

    public Image staminaBarBackground;

    private RectTransform staminaBarBackgroundRect;

    private bool blinkDangerText;
    private float dangerBlinkTime;
    private const float dangerBlinkTimeMax = .15f;
    private const float staminaDangerThreshold = .12f;

    public PlayerStatus Status{
        get{ return playerStatus; }
    }



    public void Setup(PlayerStatus status, Canvas c)
    {
        playerStatus = status;
        canvas = c;
        playerStatus.playerHeader = this;
        playerTransform = playerStatus.transform;
        tr = transform;
        playerNumberText.text = "" + playerStatus.PlayerNumber;
        playerNumberBackground.color = PlayerToken.colors[playerStatus.PlayerNumber - 1];
        staminaBarFill.color = PlayerToken.colors[playerStatus.PlayerNumber - 1];

        staminaBarFillRect = staminaBarFill.GetComponent<RectTransform>();
        staminaBarBorderRect = staminaBarBorder.GetComponent<RectTransform>();
        staminaBarBackgroundRect = staminaBarBackground.GetComponent<RectTransform>();
    }

    public void UpdateStaminaBar()
    {
        float width = staminaBarBackgroundRect.rect.width;
        float staminaPercentage = playerStatus.stamina / PlayerStatus.deafaultMaxStamina;
        float staminaMaxPercentage = playerStatus.maxStamina / PlayerStatus.deafaultMaxStamina;

        //decreases bars from the center
        // staminaBarFillRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, width * staminaPercentage);
        // staminaBarBorderRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, width * staminaMaxPercentage);

        //DECREASES BARS FROM THE LEFT SIDE
        staminaBarFillRect.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, 0, width * staminaPercentage);
        staminaBarBorderRect.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, 0, width * staminaMaxPercentage);

        if (staminaPercentage <= staminaDangerThreshold)
        {
            blinkDangerText = true;
        }
        else
        {
            blinkDangerText = false;
            dangerText.SetActive(false);
        }
    }

    public void SetHeel(bool enabled)
    {
        heelText.SetActive(enabled);
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 position = Camera.main.WorldToScreenPoint(playerTransform.position);
        position.y -= 42 * canvas.scaleFactor;
        tr.position = position;

        if (blinkDangerText)
        {
            dangerBlinkTime += Time.deltaTime;
            if (dangerBlinkTime > dangerBlinkTimeMax)
            {
                dangerBlinkTime = 0;
                dangerText.SetActive(!dangerText.activeSelf);
            }
        }
    }
}
