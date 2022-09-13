using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(PlayerInput))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Player")]
    [Tooltip("Move speed of the character in m/s")]
    public float moveSpeed = 2.0f;

    [Tooltip("How fast the character turns to face movement direction")]
    [Range(0.0f, 0.3f)]
    public float rotationSmoothTime = 0.12f;

    [Tooltip("Acceleration and deceleration")]
    public float speedChangeRate = 10.0f;

    public AudioClip landingAudioClip;
    public AudioClip[] footstepAudioClips;

    [Range(0, 1)]
    public float FootstepAudioVolume = 0.5f;

    [Space(10)]
    [Tooltip("The height the player can jump")]
    public float jumpHeight = 1.2f;

    [Tooltip("The character uses its own gravity value. The engine default is -9.81f")]
    public float gravity = -15.0f;

    [Tooltip("Platformer jumps feel better when they fall faster after their apex. This multiplies gravity whe nthe player is falling")]
    public float fallGravityMultiplier = 2;

    [Space(10)]
    [Tooltip(
        "Time required to pass before being able to jump again. Set to 0f to instantly jump again"
    )]
    public float jumpTimeout = 0.50f;

    [Tooltip(
        "Time required to pass before entering the fall state. Useful for walking down stairs"
    )]
    public float fallTimeout = 0.15f;

    [Tooltip(
    "The amount of time you can be off the edge of a platform while still being able to count as grounded"
    )]
    public float roadRunnerTimeMax = 0.15f;

    [Tooltip(
        "How quickly to change the current velocity of the player (while in the air) to the inputed direction/velocity of the player"
        )]
    public float airSpeedChangeAmount = 1;

    //if you jump using the road runner time, then disbale the ability to jump with road runner time until you land
    private bool roadRunnerJumpAvailable;

    [Tooltip(
    "The amount of time spent in the air, resets to 0 when you land."
    )]
    private float timeInAir;

    [Header("Player Grounded")]
    [Tooltip(
        "If the character is grounded or not. Not part of the CharacterController built in grounded check"
    )]
    public bool grounded = true;

    //if the player was grounded in the previous frame
    private bool wasGrounded = true;

    [Tooltip("Useful for rough ground")]
    public float groundedOffset = -0.14f;

    [Tooltip(
        "The radius of the grounded check. Should match the radius of the CharacterController"
    )]
    public float groundedRadius = 0.28f;

    [Tooltip("What layers the character uses as ground")]
    public LayerMask groundLayers;

    // player
    private float _speed;
    private float _targetRotation = 0.0f;

    //stores direction and speed of where the player is moving in each axis
    private Vector3 velocity;
    private float _rotationVelocity;
    private float _terminalVelocity = 53.0f;

    // timeout deltatime
    private float _jumpTimeoutDelta;
    private float _fallTimeoutDelta;


    private PlayerInput _playerInput;
    private CharacterController _controller;
    private StarterAssetsInputs _input;

    private const float _threshold = 0.01f;

    private bool IsCurrentDeviceMouse
    {
        get
        {
            return _playerInput.currentControlScheme == "KeyboardMouse";
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        _controller = GetComponent<CharacterController>();
        _input = GetComponent<StarterAssetsInputs>();
        _playerInput = GetComponent<PlayerInput>();

        // reset our timeouts on start
        _jumpTimeoutDelta = jumpTimeout;
        _fallTimeoutDelta = fallTimeout;
        velocity = new Vector3();
    }

    public void UpdateManual(bool updateMovement, bool controlMovement, bool controlRotation)
    {
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
            Move();
        }
    }

    public void ControlMovement(Vector3 inputDirection)
    {
        //while you are grounded, you have immediate control of your movement
        if (grounded)
        {
            velocity.x = moveSpeed * inputDirection.x;
            velocity.z = moveSpeed * inputDirection.z;
        }
        else
        {
            //while in the air, you have less control of your movement
            //to do this, we find the difference between your current velocity and your requested velocity (what you are inputting)
            //then, over time, we 'slowly' interpolate toward your requested velocity
            //how quickly this is (how much control you have over your character) is proportional to the airSpeedChangeAmount variable
            float differenceX = velocity.x - (moveSpeed * inputDirection.x);
            float differenceZ = velocity.z - (moveSpeed * inputDirection.z);
            Vector2 speedDecayDirection = new Vector2(differenceX, differenceZ) / moveSpeed;

            velocity.x = velocity.x - speedDecayDirection.x * airSpeedChangeAmount * Time.deltaTime;
            velocity.z = velocity.z - speedDecayDirection.y * airSpeedChangeAmount * Time.deltaTime;
        }
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

        Vector3 targetDirection = Quaternion.Euler(0.0f, _targetRotation, 0.0f) * Vector3.forward;
    }

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
            // reset the jump timeout timer
            _jumpTimeoutDelta = jumpTimeout;

        if (grounded)
        {
            //reset time in air
            timeInAir = 0;
            roadRunnerJumpAvailable = true;

            // reset the fall timeout timer
            _fallTimeoutDelta = fallTimeout;

            // stop our velocity dropping infinitely when grounded
            if (velocity.y < 0.0f)
            {
                velocity.y = -10f;
            }

            // Jump
            if (controlMovement && _input.jump && !_input.wasJumping && _jumpTimeoutDelta <= 0.0f)
            {
                // the square root of H * -2 * G = how much velocity needed to reach desired height
                velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
                grounded = false;
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

            // allow player to Jump with road runner time
            if (controlMovement && roadRunnerJumpAvailable && timeInAir < roadRunnerTimeMax && _input.jump && !_input.wasJumping && _jumpTimeoutDelta <= 0.0f)
            {
                // the square root of H * -2 * G = how much velocity needed to reach desired height
                velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
                roadRunnerJumpAvailable = false;
            }



            // fall timeout
            if (_fallTimeoutDelta >= 0.0f)
            {
                _fallTimeoutDelta -= Time.deltaTime;
            }

            if (timeInAir >= roadRunnerTimeMax)
            {
                // if we are not grounded, do not jump
                _input.jump = false;
            }
        }

        // apply gravity over time if under terminal (multiply by delta time twice to linearly speed up over time)
        if (velocity.y < _terminalVelocity)
        {
            if (velocity.y < 0)
            {
                //when falling, make gravity stronger (multiply it by a multiplier value) to give a better feel to jumps
                velocity.y += gravity * fallGravityMultiplier * Time.deltaTime;
            }
            else
            {
                velocity.y += gravity * Time.deltaTime;
            }
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

    private void OnDrawGizmosSelected()
    {
        Color transparentGreen = new Color(0.0f, 1.0f, 0.0f, 0.35f);
        Color transparentRed = new Color(1.0f, 0.0f, 0.0f, 0.35f);

        if (grounded)
            Gizmos.color = transparentGreen;
        else
            Gizmos.color = transparentRed;

        // when selected, draw a gizmo in the position of, and matching radius of, the grounded collider
        Gizmos.DrawSphere(
            new Vector3(
                transform.position.x,
                transform.position.y - groundedOffset,
                transform.position.z
            ),
            groundedRadius
        );
    }

    // private void OnFootstep(AnimationEvent animationEvent)
    // {
    //     if (animationEvent.animatorClipInfo.weight > 0.5f)
    //     {
    //         if (FootstepAudioClips.Length > 0)
    //         {
    //             var index = Random.Range(0, FootstepAudioClips.Length);
    //             AudioSource.PlayClipAtPoint(
    //                 FootstepAudioClips[index],
    //                 transform.TransformPoint(_controller.center),
    //                 FootstepAudioVolume
    //             );
    //         }
    //     }
    // }

    // private void OnLand(AnimationEvent animationEvent)
    // {
    //     if (animationEvent.animatorClipInfo.weight > 0.5f)
    //     {
    //         AudioSource.PlayClipAtPoint(
    //             LandingAudioClip,
    //             transform.TransformPoint(_controller.center),
    //             FootstepAudioVolume
    //         );
    //     }
    // }
}
