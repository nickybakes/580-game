using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerCursor : MonoBehaviour
{
    public TextMeshProUGUI playerNumberText;

    public Image cursorSprite;

    public int playerNumber;

    private float moveSpeed = .65f;

    private float aspectRatio = 16f / 9f;

    private CursorInputs _input;

    private Vector2 inputDirection;

    private RectTransform rect;

    private Vector2 velocity;

    public Vector2 normalizedPosition;

    private RectTransform canvasRect;

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
        aspectRatio = canvasRect.rect.width/canvasRect.rect.height;
        inputDirection = new Vector2(inputDirection.x, inputDirection.y * aspectRatio);
        velocity = inputDirection * moveSpeed;
        Move();
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
