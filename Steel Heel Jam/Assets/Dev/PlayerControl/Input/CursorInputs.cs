using UnityEngine;
using UnityEngine.InputSystem;

public class CursorInputs : MonoBehaviour
{
    [Header("Cursor Input Values")]
    public Vector2 move;

    public bool accept;
    public bool wasAccepting;

    private void Update()
    {

    }

    public void OnMove(InputValue value)
    {
        MoveInput(value.Get<Vector2>());
    }

    public void OnAccept(InputValue value)
    {
        AcceptInput(value.isPressed);
    }

    public void MoveInput(Vector2 newMoveDirection)
    {
        move = newMoveDirection;
    }

    public void AcceptInput(bool pressed)
    {
        wasAccepting = accept;
        accept = pressed;
    }
}