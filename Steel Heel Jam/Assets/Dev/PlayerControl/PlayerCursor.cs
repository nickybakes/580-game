using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public enum SnapDirection
{
    Up,
    Down,
    Left,
    Right
}
public class PlayerCursor : MonoBehaviour
{
    public TextMeshProUGUI playerNumberText;

    public Image cursorSprite;

    public int playerNumber;

    private float moveSpeed = .65f;

    private float aspectRatio = 16f / 9f;

    [HideInInspector] public CursorInputs _input;

    private Vector2 inputDirection;

    private RectTransform rect;

    private Vector2 velocity;

    public Vector2 normalizedPosition;

    private RectTransform canvasRect;

    public MenuButton highlightedButton;

    public float lerpToPositionTimeMax;

    public float lerpToPositionTimeCurrent;

    private Vector2 lerpToPosition;

    private Vector2 lerpStartPosition;

    private float lerpToSpeed = 50f;

    private MenuCustomizationPanel customizationPanel;

    private bool customizationMovementInput;

    public bool IsCustomizing
    {
        get
        {
            return customizationPanel != null;
        }
        set
        {
            if (value)
            {
                velocity = Vector2.zero;
                customizationPanel = MenuManager.menu.customizationPanels[playerNumber - 1];
                customizationPanel.OpenPanel();
                LerpToCustomizationPanel();
            }
            else
            {
                rect.localScale = Vector3.one;
                customizationPanel.ClosePanel();
                customizationPanel = null;
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        _input = GetComponent<CursorInputs>();
        rect = GetComponent<RectTransform>();
        canvasRect = FindObjectOfType<Canvas>().GetComponent<RectTransform>();
        velocity = new Vector2();
    }

    // Update is called once per frame
    void Update()
    {
        if (IsCustomizing)
        {
            if (lerpToPositionTimeCurrent > 0)
            {
                velocity = Vector2.zero;

                float percentage = Mathf.Max(lerpToPositionTimeCurrent / lerpToPositionTimeMax, 0);

                normalizedPosition = Vector2.Lerp(lerpToPosition, lerpStartPosition, percentage);

                transform.localScale = new Vector3(percentage, percentage, percentage);

                lerpToPositionTimeCurrent -= Time.deltaTime;

                if (lerpToPositionTimeCurrent <= 0)
                    transform.localScale = Vector3.zero;
            }
            else
            {
                if (_input.customize)
                {
                    _input.customize = false;
                }

                if (_input.back && !_input.wasBacking)
                {
                    _input.back = false;
                    customizationPanel.DiscardChanges();
                    IsCustomizing = false;
                }

                if (_input.accept && !_input.wasAccepting)
                {
                    _input.accept = false;
                    customizationPanel.SaveChanges();
                    IsCustomizing = false;
                }

                if (_input.randomize && !_input.wasRandomize)
                {
                    customizationPanel.RandomizeNotSavedImmediately();
                    _input.randomize = false;
                }

                if (!customizationMovementInput)
                {
                    if (_input.customizeMove.x < -.7f)
                    {
                        customizationPanel.ChangeValue(true);
                        customizationMovementInput = true;
                    }
                    else if (_input.customizeMove.x > .7f)
                    {
                        customizationPanel.ChangeValue(false);
                        customizationMovementInput = true;
                    }

                    if (_input.customizeMove.y < -.7f)
                    {
                        customizationPanel.SwitchCategory(false);
                        customizationMovementInput = true;
                    }
                    else if (_input.customizeMove.y > .7f)
                    {
                        customizationPanel.SwitchCategory(true);
                        customizationMovementInput = true;
                    }

                }
                else
                {
                    if (_input.customizeMove.magnitude < .5f)
                    {
                        customizationMovementInput = false;
                    }
                }
            }
        }
        else
        {
            if (_input.back && MenuManager.menu)
            {
                AppManager.app.RemovePlayerToken(playerNumber);
            }

            if (lerpToPositionTimeCurrent > 0)
            {
                velocity = Vector2.zero;

                normalizedPosition = Vector2.Lerp(lerpToPosition, lerpStartPosition, lerpToPositionTimeCurrent / lerpToPositionTimeMax);

                lerpToPositionTimeCurrent -= Time.deltaTime;

                if (lerpToPositionTimeCurrent <= 0)
                {
                    normalizedPosition = lerpToPosition;
                }

            }
            else
            {
                inputDirection = _input.move.normalized;
                aspectRatio = canvasRect.rect.width / canvasRect.rect.height;
                inputDirection = new Vector2(inputDirection.x, inputDirection.y * aspectRatio);
                velocity = inputDirection * moveSpeed;
            }

            if (_input.snapState.isSnapping)
            {
                // Logic for snapping to direction here
                if (highlightedButton != null)
                {
                    MenuButton b = highlightedButton.buttonSelects[(int)_input.snapState.snapDirection];

                    if (b != null)
                    {
                        LerpToButton(b);
                    }
                }
            }

            if (_input.customize && !_input.wasCustomize && MenuManager.menu)
            {
                _input.customize = false;
                IsCustomizing = true;
            }

            if (_input.randomize && !_input.wasRandomize && MenuManager.menu)
            {
                MenuManager.menu.customizationPanels[playerNumber - 1].RandomizeSaveImmediately();
                _input.randomize = false;
            }
        }


        Move();
    }

    private Vector2 NormalizePosition(Vector2 position)
    {
        return new Vector2(position.x / canvasRect.rect.width + .5f, position.y / canvasRect.rect.height + .5f);
    }

    private void LerpToButton(MenuButton b)
    {
        Vector2 buttonNormalizedPosition = NormalizePosition(b.rect.anchoredPosition);
        lerpToPosition = buttonNormalizedPosition;
        lerpStartPosition = normalizedPosition;

        CalculateLerpTime(lerpToPosition, 50, .05f);
    }

    private void LerpToCustomizationPanel()
    {
        Vector2 panelNormalizedPosition = NormalizePosition(customizationPanel.rect.anchoredPosition);
        lerpToPosition = panelNormalizedPosition;
        lerpStartPosition = normalizedPosition;

        CalculateLerpTime(lerpToPosition, 10, .2f);
    }

    private void CalculateLerpTime(Vector2 position, float lerpToSpeed, float minTime)
    {
        float distance = Vector2.Distance(position, normalizedPosition);

        lerpToPositionTimeMax = Mathf.Max(distance / lerpToSpeed, minTime);
        lerpToPositionTimeCurrent = lerpToPositionTimeMax;
    }

    public void ReturnToDefaultLocation(int defaultLocation)
    {
        if (!rect)
            rect = GetComponent<RectTransform>();

        switch (defaultLocation)
        {
            case (0):
                normalizedPosition = new Vector2(.1f, 1 - (.2f + (.075f * (playerNumber - 1))));
                break;

            case (1):
                normalizedPosition = new Vector2(.2f + (.075f * (playerNumber - 1)), .7f);
                break;
            case (2):
                normalizedPosition = new Vector2(.2f + (.075f * (playerNumber - 1)), .25f);
                break;

            case (3):
                normalizedPosition = new Vector2(.2f + (.075f * (playerNumber - 1)), .12f);
                break;
        }
    }

    private void Move()
    {
        normalizedPosition += velocity * Time.deltaTime;
        normalizedPosition.x = Mathf.Max(Mathf.Min(1, normalizedPosition.x), 0);
        normalizedPosition.y = Mathf.Max(Mathf.Min(1, normalizedPosition.y), 0);
        rect.localPosition = new Vector2(
            (normalizedPosition.x - .5f) * canvasRect.rect.width,
            (normalizedPosition.y - .5f) * canvasRect.rect.height
        );
    }
}
