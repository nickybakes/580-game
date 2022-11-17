using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

public struct SnapState
{
    public bool isSnapping;
    public SnapDirection snapDirection;
}

public class CursorInputs : MonoBehaviour
{
    [Header("Cursor Input Values")]
    public Vector2 move;

    public bool accept;
    public bool back;
    public bool wasAccepting;

    public SnapState snapState;

    private void Update()
    {
        snapState.isSnapping = false;
    }

    public void OnMove(InputValue value)
    {
        MoveInput(value.Get<Vector2>());
    }

    public void OnSnap(InputValue value)
    {
        SnapInput(value.Get<Vector2>());
    }

    public void OnAccept(InputValue value)
    {
        AcceptInput(value.isPressed);
    }

    public void OnBack(InputValue value)
    {
        BackInput(value.isPressed);
    }

    public void MoveInput(Vector2 newMoveDirection)
    {
        move = newMoveDirection;
    }

    public void SnapInput(Vector2 snapDirection)
    {
        snapState.isSnapping = true;

        // This is terrible and should be changed if I can figure out how to translate D-PAD to integer
        if (snapDirection.y == 1)
        {
            snapState.snapDirection = SnapDirection.Up;
        }
        else if (snapDirection.y == -1)
        {
            snapState.snapDirection = SnapDirection.Down;
        }
        else if (snapDirection.x == -1)
        {
            snapState.snapDirection = SnapDirection.Left;
        }
        else if (snapDirection.x == 1)
        {
            snapState.snapDirection = SnapDirection.Right;
        }
    }

    public void AcceptInput(bool pressed)
    {
        wasAccepting = accept;
        accept = pressed;
    }

    public void BackInput(bool pressed)
    {
        back = pressed;
    }
}