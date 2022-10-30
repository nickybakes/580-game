using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(StarterAssetsInputs))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Player")]
    [Tooltip("Move speed of the character in m/s")]
    public float moveSpeed = 13;

    /// <summary>
    /// Multiply the default moveSpeed by this, useful for temporarily increasing player's speed (such as while dodge rolling)
    /// </summary>
    private float moveSpeedMultiplier;

    [Tooltip("How fast the character turns to face movement direction")]
    [Range(0.0f, 0.3f)]
    public float rotationSmoothTime = 0.027f;

    [Tooltip("Acceleration and deceleration")]
    public float speedChangeRate = 16;

    [Space(10)]
    [Tooltip("The height the player can jump")]
    public float jumpHeight = 3.4f;

    [Tooltip("The character uses its own gravity value. The engine default is -9.81f")]
    public float gravity = -70;

    [Tooltip("Platformer jumps feel better when they fall faster after their apex. This multiplies gravity when the player is falling")]
    public float fallGravityMultiplier = 1.7f;

    private float extraFallMultiplier = 1;

    [Space(10)]
    [Tooltip(
        "Time required to pass before being able to jump again. Set to 0f to instantly jump again"
    )]
    public float jumpTimeout = 0.05f;

    [Tooltip(
        "Time required to pass before entering the fall state. Useful for walking down stairs"
    )]
    public float fallTimeout = 0.15f;

    [Tooltip(
    "The amount of time you can be off the edge of a platform while still being able to count as grounded"
    )]
    public float roadRunnerTimeMax = 0.12f;

    [Tooltip(
        "How quickly to change the current velocity of the player (while in the air) to the inputed direction/velocity of the player"
        )]
    public float airSpeedChangeAmount = 30;

    //if you jump using the road runner time, then disbale the ability to jump with road runner time until you land
    private bool roadRunnerJumpAvailable;

    [Tooltip(
    "The amount of time spent in the air, resets to 0 when you land."
    )]
    private float timeInAir;

    [Tooltip(
    "The amount of time spent grounded, resets to 0 when you land"
    )]
    private float timeGrounded;

    [Header("Player Grounded")]
    [Tooltip(
        "If the character is grounded or not. Not part of the CharacterController built in grounded check"
    )]
    public bool grounded = true;

    //if the player was grounded in the previous frame
    public bool wasGrounded = true;

    [Tooltip("Useful for rough ground")]
    public float groundedOffset = -0.17f;

    [Tooltip(
        "The radius of the grounded check. Should match the radius of the CharacterController"
    )]
    public float groundedRadius = 0.4f;

    [Tooltip("What layers the character uses as ground")]
    public LayerMask groundLayers;

    // player
    private float _speed;
    private float _targetRotation = 0.0f;

    //stores direction and speed of where the player is moving in each axis
    public Vector3 velocity;
    private float _rotationVelocity;
    private float _terminalVelocity = 53.0f;

    // timeout deltatime
    private float _jumpTimeoutDelta;
    private float _fallTimeoutDelta;


    new private Transform transform;

    private PlayerInput _playerInput;
    private CharacterController _controller;
    private StarterAssetsInputs _input;
    private PlayerStatus _status;

    private AudioManager _audioManager;

    /// <summary>
    /// a set forward direction we want to player to be moving toward
    /// </summary>
    private Vector3 setForwardDirection;


    private const float _threshold = 0.01f;

    private bool IsCurrentDeviceMouse
    {
        get
        {
            return _playerInput.currentControlScheme == "KeyboardMouse";
        }
    }

    /// <summary>
    /// Gives you the current moveSpeed of the character (base move speed multiplied by the current state's move speed multiplier)
    /// </summary>
    /// <value>the current moveSpeed of the character</value>
    public float CurrentMoveSpeed
    {
        get { return moveSpeed * moveSpeedMultiplier; }
    }

    public Vector3 ActualFowardDirection
    {
        get
        {
            if (_input.move != Vector2.zero)
                return new Vector3(_input.move.x, 0.0f, _input.move.y).normalized;
            else
                return transform.forward;
        }
    }

    public bool InputDirectionNotZero
    {
        get { return (_input.move.x != 0 || _input.move.y != 0); }
    }

    public float ActualTopDownSpeed
    {
        get { return new Vector2(velocity.x, velocity.z).magnitude; }
    }

    // public void SetStatus(PlayerStatus status){
    //     _status = status;
    // }

    // Start is called before the first frame update
    public void Start()
    {
        transform = gameObject.transform;

        _controller = GetComponent<CharacterController>();
        _input = GetComponent<StarterAssetsInputs>();
        _playerInput = GetComponent<PlayerInput>();
        _status = GetComponent<PlayerStatus>();
        _audioManager = FindObjectOfType<AudioManager>();

        // reset our timeouts on start
        _jumpTimeoutDelta = jumpTimeout;
        _fallTimeoutDelta = fallTimeout;
        velocity = new Vector3();
    }

    /// <summary>
    /// This is to be updated via the PlayerStatus script's Update method. The parameters should be recieved from the Player's
    /// current PlayerState
    /// </summary>
    /// <param name="updateMovement">True if we should overall update the player's movement (gravity, position with velocity, etc)</param>
    /// <param name="controlMovement">True if the player can affect their movement velocity and if they can jump with their inputs</param>
    /// <param name="controlRotation">True if the player can change the direction they are facing, regardless of their current velocity</param>
    public void UpdateManual(bool updateMovement, bool controlMovement, bool controlRotation, bool alternateFriction)
    {
        moveSpeedMultiplier = _status.CurrentPlayerState.moveSpeedMultiplier;
        extraFallMultiplier = _status.CurrentPlayerState.extraFallGravityMultiplier;
        if (updateMovement)
        {
            JumpAndGravity(controlMovement);
            GroundedCheck();
            // normalise input direction
            Vector3 inputDirection = new Vector3(_input.move.x, 0.0f, _input.move.y).normalized;
            if (controlMovement)
                ControlMovement(inputDirection);
            if (controlRotation)
                ControlRotation(inputDirection);
            if (alternateFriction)
                ControlMovement(Vector3.zero);

            Move();
        }
    }

    public void ControlMovement(Vector3 inputDirection)
    {
        //while you are grounded, you have immediate control of your movement
        if (grounded)
        {
            //if the player is not pressing anything, then this code decelerates the player quickly
            if (inputDirection.x == 0 && inputDirection.z == 0)
            {
                float decelerationAmount = 100;
                Vector2 topDownVelocity = new Vector2(velocity.x, velocity.z);
                float currentActualSpeed = topDownVelocity.magnitude;

                if (currentActualSpeed > .2f)
                {
                    Vector2 currentVelocityDirection = topDownVelocity / currentActualSpeed;
                    currentActualSpeed -= decelerationAmount * Time.deltaTime;

                    velocity.x = currentVelocityDirection.x * currentActualSpeed;
                    velocity.z = currentVelocityDirection.y * currentActualSpeed;
                }
                else
                {
                    velocity.x = 0;
                    velocity.z = 0;
                }
            }
            //if the player is holding a direction then they get immediate control of their player
            //but if their current actual speed is higher than their current set move speed then this code will slowly decelerate them 
            //to that current set move speed
            else
            {
                float decelerationAmount = 70;
                Vector2 topDownVelocity = new Vector2(velocity.x, velocity.z);
                float currentActualSpeed = topDownVelocity.magnitude;

                if (currentActualSpeed > CurrentMoveSpeed)
                {
                    Vector2 currentVelocityDirection = topDownVelocity / currentActualSpeed;
                    currentActualSpeed -= decelerationAmount * Time.deltaTime;

                    velocity.x = currentVelocityDirection.x * currentActualSpeed;
                    velocity.z = currentVelocityDirection.y * currentActualSpeed;
                }
                else
                {
                    velocity.x = CurrentMoveSpeed * inputDirection.x;
                    velocity.z = CurrentMoveSpeed * inputDirection.z;
                }
            }
        }
        else
        {
            float decelerationAmount = 10;
            Vector2 topDownVelocity = new Vector2(velocity.x, velocity.z);
            float currentActualSpeed = topDownVelocity.magnitude;
            Vector2 currentVelocityDirection = topDownVelocity / currentActualSpeed;

            //if the player is moving faster than their currentMoveSpeed, and they are holding i the same direction
            //then keep them going that direction, very slowly decelerate them
            if (Vector2.Dot(currentVelocityDirection, new Vector2(inputDirection.x, inputDirection.z)) > .9f && currentActualSpeed > CurrentMoveSpeed)
            {
                currentActualSpeed -= decelerationAmount * Time.deltaTime;

                velocity.x = currentVelocityDirection.x * currentActualSpeed;
                velocity.z = currentVelocityDirection.y * currentActualSpeed;
            }
            else
            {
                //while in the air, you have less control of your movement
                //to do this, we find the difference between your current velocity and your requested velocity (what you are inputting)
                //then, over time, we 'slowly' interpolate toward your requested velocity
                //how quickly this is (how much control you have over your character) is proportional to the airSpeedChangeAmount variable
                float differenceX = velocity.x - (CurrentMoveSpeed * inputDirection.x);
                float differenceZ = velocity.z - (CurrentMoveSpeed * inputDirection.z);
                Vector2 speedDecayDirection = new Vector2(differenceX, differenceZ) / CurrentMoveSpeed;
                if (CurrentMoveSpeed == 0)
                    speedDecayDirection = Vector2.zero;

                velocity.x = velocity.x - speedDecayDirection.x * airSpeedChangeAmount * Time.deltaTime;
                velocity.z = velocity.z - speedDecayDirection.y * airSpeedChangeAmount * Time.deltaTime;
            }

        }
    }

    /// <summary>
    /// Gets the forward vector of the player and makes their top down velocity equal to that times their current set move speed
    /// </summary>
    public void SetVelocityToMoveSpeedTimesFowardDirection()
    {
        Vector3 v = setForwardDirection * CurrentMoveSpeed;
        velocity = new Vector3(v.x, velocity.y, v.z);
    }

    public void SetVelocityToMoveSpeedTimesFowardDirection(float moveSpeed)
    {
        Vector3 v = setForwardDirection * moveSpeed;
        velocity = new Vector3(v.x, velocity.y, v.z);
    }

    public void SetTheSetForwardDirection()
    {
        transform.forward = ActualFowardDirection;
        setForwardDirection = ActualFowardDirection;
    }

    /// <summary>
    /// Sets velocity's x and z to 0, but keeps y (vertical movement, gravity, etc) unchanged
    /// </summary>
    public void SetTopDownVelocityToZero()
    {
        velocity.x = 0;
        velocity.z = 0;
    }

    public void ControlRotation(Vector3 inputDirection)
    {
        // note: Vector2's != operator uses approximation so is not floating point error prone, and is cheaper than magnitude
        // if there is a move input rotate player when the player is moving
        if (_input.move != Vector2.zero)
        {
            _targetRotation =
                Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg;
            float rotation = Mathf.SmoothDampAngle(
                transform.eulerAngles.y,
                _targetRotation,
                ref _rotationVelocity,
                rotationSmoothTime
            );

            // rotate to face input direction relative to camera position
            transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);
        }

    }

    /// <summary>
    /// should return the direction that player last moved in (not the direction they are visually facing)
    /// </summary>
    /// <returns></returns>
    // public Vector3 RotationToVector3(){
    //     return Quaternion.Euler(0.0f, _targetRotation, 0.0f) * Vector3.forward;
    // }

    private void GroundedCheck()
    {
        // set sphere position, with offset
        Vector3 spherePosition = new Vector3(
            transform.position.x,
            transform.position.y - groundedOffset,
            transform.position.z
        );

        wasGrounded = grounded;

        grounded = Physics.CheckSphere(
            spherePosition,
            groundedRadius,
            groundLayers,
            QueryTriggerInteraction.Ignore
        );
    }

    private void Move()
    {
        // move the player
        _controller.Move(new Vector3(
            velocity.x * Time.deltaTime,
                velocity.y * Time.deltaTime,
                velocity.z * Time.deltaTime
        ));
    }

    private Vector3 AdjustVelocityToDownwardSlope(Vector3 vel)
    {
        Ray ray = new Ray(transform.position, Vector3.down);

        if (Physics.Raycast(ray, out RaycastHit hitInfo, groundedRadius, groundLayers, QueryTriggerInteraction.Ignore))
        {
            Quaternion slopeRotation = Quaternion.FromToRotation(Vector3.up, hitInfo.normal);
            Vector3 adjustedVelocity = slopeRotation * vel;
            // adjustedVelocity.x = vel.x;
            // adjustedVelocity.z = vel.z;
            if (adjustedVelocity.y < 0)
                return adjustedVelocity;
        }

        return vel;
    }

    private void JumpAndGravity(bool controlMovement)
    {

        if (!wasGrounded && grounded)
            timeGrounded = 0;

        if (grounded)
        {
            // Reset double jump availability
            if (_status.buffs[(int)Buff.TopRopes])
            {
                _status.canDoubleJump = true;
            }

            //reset time in air
            timeInAir = 0;

            timeGrounded += Time.deltaTime;

            if (timeGrounded > .14)
                roadRunnerJumpAvailable = true;

            // reset the fall timeout timer
            _fallTimeoutDelta = fallTimeout;

            // stop our velocity dropping infinitely when grounded
            if (velocity.y < 0.0f)
            {
                velocity.y = -10f;
            }

            // Jump
            if (controlMovement && _input.Jump)
            {
                // the square root of H * -2 * G = how much velocity needed to reach desired height
                velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
                _input.Jump = false;
                grounded = false;
                roadRunnerJumpAvailable = false;
            }

            // jump timeout
            if (_jumpTimeoutDelta >= 0.0f)
            {
                _jumpTimeoutDelta -= Time.deltaTime;
            }
        }
        else
        {
            if (wasGrounded && velocity.y < 0)
                velocity.y = 0;

            //increment time in air
            timeInAir += Time.deltaTime;

            if (controlMovement && _input.Jump)
            {
                // allow player to Jump with road runner time
                if (roadRunnerJumpAvailable && timeInAir < roadRunnerTimeMax)
                {
                    // the square root of H * -2 * G = how much velocity needed to reach desired height
                    velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
                    roadRunnerJumpAvailable = false;
                    _input.Jump = false;
                }
                // Double jump if possible
                else if (_status.canDoubleJump)
                {
                    // the square root of H * -2 * G = how much velocity needed to reach desired height
                    velocity.y = Mathf.Sqrt((jumpHeight * 1.2f) * -2f * gravity);
                    _status.canDoubleJump = false;
                    _input.Jump = false;
                }
            }
            

            // fall timeout
            if (_fallTimeoutDelta >= 0.0f)
            {
                _fallTimeoutDelta -= Time.deltaTime;
            }
        }

        // apply gravity over time if under terminal (multiply by delta time twice to linearly speed up over time)
        if (velocity.y < 0)
        {
            //when falling, make gravity stronger (multiply it by a multiplier value) to give a better feel to jumps
            velocity.y += gravity * fallGravityMultiplier * extraFallMultiplier * Time.deltaTime;
        }
        else
        {
            velocity.y += gravity * Time.deltaTime;
        }
    }

    private static float ClampAngle(float lfAngle, float lfMin, float lfMax)
    {
        if (lfAngle < -360f)
            lfAngle += 360f;
        if (lfAngle > 360f)
            lfAngle -= 360f;
        return Mathf.Clamp(lfAngle, lfMin, lfMax);
    }

    // private void OnDrawGizmosSelected()
    // {
    //     Color transparentGreen = new Color(0.0f, 1.0f, 0.0f, 0.35f);
    //     Color transparentRed = new Color(1.0f, 0.0f, 0.0f, 0.35f);

    //     if (grounded)
    //         Gizmos.color = transparentGreen;
    //     else
    //         Gizmos.color = transparentRed;

    //     // when selected, draw a gizmo in the position of, and matching radius of, the grounded collider
    //     Gizmos.DrawSphere(
    //         new Vector3(
    //             transform.position.x,
    //             transform.position.y - groundedOffset,
    //             transform.position.z
    //         ),
    //         groundedRadius
    //     );
    // }

    //private void OnFootstep(AnimationEvent animationEvent)
    //{
    //    if (animationEvent.animatorClipInfo.weight > 0.5f)
    //    {
    //        _audioManager.Play("Footstep", 0.2f, 0.8f, 1.2f);
    //    }
    //}

    //private void OnLand(AnimationEvent animationEvent)
    //{
    //    if (animationEvent.animatorClipInfo.weight > 0.5f)
    //    {
    //        _audioManager.Play("Landing", 0.2f, 0.8f, 1.2f);
    //    }
    //}
}
