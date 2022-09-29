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

    private float moveSpeed = .1f;

    private CursorInputs _input;

    private Vector2 inputDirection;


    // Start is called before the first frame update
    void Start()
    {
        _input = GetComponent<CursorInputs>();
    }

    // Update is called once per frame
    void Update()
    {
        inputDirection = _input.move.normalized;
    }
}
