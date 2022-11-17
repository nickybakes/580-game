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

    [HideInInspector] public MenuButton highlightedButton;

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
        if (_input.back)
        {
            AppManager.app.RemovePlayerToken(playerNumber);
        }

        inputDirection = _input.move.normalized;
        aspectRatio = canvasRect.rect.width / canvasRect.rect.height;
        inputDirection = new Vector2(inputDirection.x, inputDirection.y * aspectRatio);
        velocity = inputDirection * moveSpeed;
        Move();

        if (_input.snapState.isSnapping)
        {
            // Logic for snapping to direction here
            if (highlightedButton != null)
            {
                //rect.anchoredPosition = highlightedButton.buttonSelects[(int)_input.snapState.snapDirection].rect.anchoredPosition;
            }
        }

        if (_input.customizeLeft && !_input.wasCustomizeLeft)
        {
            int skinToneIndex = AppManager.app.playerTokens[playerNumber - 1].visualPrefs.skinToneIndex;
            if (skinToneIndex == 0)
                skinToneIndex = 15;
            else
                skinToneIndex--;

            AppManager.app.playerTokens[playerNumber - 1].visualPrefs.skinToneIndex = skinToneIndex;

            MenuManager.menu.characterDisplays[playerNumber - 1].SetSkinToneIndex(skinToneIndex);
            _input.customizeLeft = false;
        }
        if (_input.customizeRight && !_input.wasCustomizeRight)
        {
            int skinToneIndex = AppManager.app.playerTokens[playerNumber - 1].visualPrefs.skinToneIndex;
            if (skinToneIndex == 15)
                skinToneIndex = 0;
            else
                skinToneIndex++;

            AppManager.app.playerTokens[playerNumber - 1].visualPrefs.skinToneIndex = skinToneIndex;

            MenuManager.menu.characterDisplays[playerNumber - 1].SetSkinToneIndex(skinToneIndex);
            _input.customizeRight = false;
        }
    }

    public void ReturnToDefaultLocation()
    {
        if (!rect)
            rect = GetComponent<RectTransform>();

        normalizedPosition = new Vector2(.1f, 1 - (.2f + (.075f * (playerNumber - 1))));
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
