using UnityEngine;
using UnityEngine.InputSystem;

public class StarterAssetsInputs : MonoBehaviour
{
    [Header("Character Input Values")]
    public Vector2 move;
    public Vector2 look;
    public bool jump;
    public bool sprint;
    public bool pickUpPutDownPressed;
    public bool throwIsHeld;

    [Header("Movement Settings")]
    public bool analogMovement;

    [Header("Mouse Cursor Settings")]
    public bool cursorLocked = true;
    public bool cursorInputForLook = true;

    public void OnMove(InputValue value)
    {
        MoveInput(value.Get<Vector2>());
    }

    public void OnLook(InputValue value)
    {
        if (cursorInputForLook)
        {
            LookInput(value.Get<Vector2>());
        }
    }

    public void OnJump(InputValue value)
    {
        JumpInput(value.isPressed);
    }

    public void OnPickUpPutDown(InputValue value)
    {
        /*if(pickUpPutDownPressed == false)
        {
            pickUpPutDownPressed = true;
        }
        else if(pickUpPutDownPressed == true)
        {
            pickUpPutDownPressed = false;
        }*/
        PickUpPutDownInput(value.isPressed);

    }

    public void PickUpPutDownInput(bool newPickUpPutDownState)
    {
        pickUpPutDownPressed = newPickUpPutDownState;
    }

    public void OnThrow(InputValue value)
    {
        if(value.isPressed) // Key pressed
        {
            throwIsHeld = true;
        }
        if(value.isPressed == false) // Key released
        {
            throwIsHeld = false;
        }
    }

    public void MoveInput(Vector2 newMoveDirection)
    {
        move = newMoveDirection;
    }

    public void LookInput(Vector2 newLookDirection)
    {
        look = newLookDirection;
    }

    public void JumpInput(bool newJumpState)
    {
        jump = newJumpState;
    }

    private void OnApplicationFocus(bool hasFocus)
    {
        SetCursorState(cursorLocked);
    }

    private void SetCursorState(bool newState)
    {
        Cursor.lockState = newState ? CursorLockMode.Locked : CursorLockMode.None;
    }
}