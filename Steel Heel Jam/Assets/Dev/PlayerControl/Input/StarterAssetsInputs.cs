using UnityEngine;
using UnityEngine.InputSystem;

public class StarterAssetsInputs : MonoBehaviour
{
    [Header("Character Input Values")]
    public Vector2 move;

    // private bool jump;

    //true if the player was holding jump in the previous frame
    // private bool wasJumping;

    private float timeSinceJumpWasPressed;
    private float timeSinceJumpWasPressedMax = .24f;

    private float timeSinceAttackWasPressed;
    private float timeSinceAttackWasPressedMax = .24f;

    // Combat Controls
    public bool attack;
    public bool pickUpPressed;
    public bool wasPickUpPressed;

    public bool throwIsHeld;
    public bool throwWasHeld;

    public bool dodgeRoll;

    public bool wasDodgeRolling;

    public bool block;
    public bool wasBlocking;

    [Header("Movement Settings")]
    public bool analogMovement;

    // [Header("Mouse Cursor Settings")]
    // public bool cursorLocked = true;
    // public bool cursorInputForLook = true;

    public bool Jump
    {
        get
        {
            return timeSinceJumpWasPressed < timeSinceJumpWasPressedMax;
        }

        set
        {
            if (value)
            {
                timeSinceJumpWasPressed = 0;
            }
            else
            {
                timeSinceJumpWasPressed = timeSinceJumpWasPressedMax;
            }
        }
    }

    public bool Attack
    {
        get
        {
            return timeSinceAttackWasPressed < timeSinceAttackWasPressedMax;
        }

        set
        {
            if (value)
            {
                timeSinceAttackWasPressed = 0;
            }
            else
            {
                timeSinceAttackWasPressed = timeSinceAttackWasPressedMax;
            }
        }
    }

    private void Update()
    {
        if (timeSinceJumpWasPressed <= timeSinceJumpWasPressedMax)
            timeSinceJumpWasPressed += Time.deltaTime;

        if (timeSinceAttackWasPressed <= timeSinceAttackWasPressedMax)
            timeSinceAttackWasPressed += Time.deltaTime;
    }

    public void OnMove(InputValue value)
    {
        MoveInput(value.Get<Vector2>());
    }

    public void OnJump(InputValue value)
    {
        JumpInput(value.isPressed);
    }

    public void OnDodgeRoll(InputValue value)
    {
        DodgeRollInput(value.isPressed);
    }

    public void OnPickUp(InputValue value)
    {
        PickUpInput(value.isPressed);
    }

    public void OnAttack(InputValue value)
    {
        AttackInput(value.isPressed);
    }

    public void OnBlock(InputValue value)
    {
        BlockInput(value.isPressed);
    }

    public void BlockInput(bool newBlockState)
    {
        wasBlocking = block;
        block = newBlockState;
    }

    public void AttackInput(bool newAttackState)
    {
        if (newAttackState)
            Attack = true;
    }

    public void PickUpInput(bool newPickUpState)
    {
        wasPickUpPressed = pickUpPressed;
        pickUpPressed = newPickUpState;
    }

    public void OnThrow(InputValue value)
    {
        /*if (value.isPressed) // Key pressed
        {
            throwIsHeld = true;
        }
        if (value.isPressed == false) // Key released
        {
            throwIsHeld = false;
        }*/
        ThrowInput(value.isPressed);
    }

    public void ThrowInput(bool newThrowState)
    {
        throwWasHeld = throwIsHeld;
        throwIsHeld = newThrowState;
    }

    public void MoveInput(Vector2 newMoveDirection)
    {
        move = newMoveDirection;
    }

    public void JumpInput(bool newJumpState)
    {
        // wasJumping = jump;
        // jump = newJumpState;
        if (newJumpState)
            Jump = true;
    }

    public void DodgeRollInput(bool newDodgeRollState)
    {
        wasDodgeRolling = dodgeRoll;
        dodgeRoll = newDodgeRollState;
    }

    // private void OnApplicationFocus(bool hasFocus)
    // {
    //     SetCursorState(cursorLocked);
    // }

    // private void SetCursorState(bool newState)
    // {
    //     Cursor.lockState = newState ? CursorLockMode.Locked : CursorLockMode.None;
    // }
}