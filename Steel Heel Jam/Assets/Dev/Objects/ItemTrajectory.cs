using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ItemTrajectory : MonoBehaviour
{
    private float chargeAmountMultiplier = 10f;
    private float minThrowSpeed = 10f;

    private Transform tr;
    private Rigidbody rb;

    private Vector3 velocity;
    private Vector3 currentMoveDirection;
    private float speed;

    public bool isFirstFrameOfThrow;
    public bool isMidAir;
    public float chargeAmount;
    public PlayerStatus target;
    public PlayerStatus thrower;
    public GameObject throwerObject;

    // Start is called before the first frame update
    void Start()
    {
        tr = transform;
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {

        if (isFirstFrameOfThrow)
        {
            InitialThrow();
            isFirstFrameOfThrow = false;
            isMidAir = true;
        }
        else if (isMidAir && target != null)
        {
            UpdateTargetedThrow();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Prevent collision with thrower
        if (collision.gameObject != throwerObject)
        {
            // Checks for collision with enemy.
            if (isMidAir && collision.gameObject.CompareTag(Tag.Player.ToString()))
            {
                PlayerStatus hitPlayerStatus = collision.gameObject.GetComponent<PlayerStatus>();
                hitPlayerStatus.GetHitByThrowable(transform.position, collision.transform.position, 10, 12 * chargeAmount + 3, 13 * chargeAmount + 7, thrower);
            }

            isMidAir = false;
            rb.useGravity = true;
        }
    }

    private void InitialThrow()
    {

        if (target != null)
        {
            Vector3 initialDirection = (thrower.movement.ActualFowardDirection + (Vector3.up * 2)).normalized;

            currentMoveDirection = initialDirection;
            speed = chargeAmountMultiplier * chargeAmount + minThrowSpeed;

            rb.useGravity = false;
        }
        else
        {
            Vector3 initialDirection = (thrower.movement.ActualFowardDirection /*+ (Vector3.up * 2)*/).normalized;

            initialDirection.x *= chargeAmount * chargeAmountMultiplier + minThrowSpeed; //chargeamt from 0-2
            initialDirection.z *= chargeAmount * chargeAmountMultiplier + minThrowSpeed;
            initialDirection.y = 4; // Set general height of non-targeted throw.

            rb.velocity = new Vector3(0,0,0);
            rb.AddForce(initialDirection, ForceMode.Impulse);
            rb.useGravity = true;
        }

        //transform.position = thrower.movement.ActualFowardDirection * chargeAmount;
        //Debug.Log(thrower.movement.ActualFowardDirection);

        //height -= Physics.gravity.y * Time.fixedDeltaTime;

        //tr.position = new Vector3(tr.position.x + (throwDirection.x * chargeAmount * chargeAmountMultiplier), tr.position.y, tr.position.z + (throwDirection.z * chargeAmount * chargeAmountMultiplier));

        // If it collides with a surface, set isThrown to false.
        //if ()
    }

    private void UpdateTargetedThrow()
    {
        // If the object hit the target.
        //if ((target.transform.position - transform.position).magnitude < 0.1f)
        //    isThrown = false;

        //Mathf.Lerp(1.0f, 0.2f, timeInThisState / 2.0f)
        //Vector3.Slerp();


        float slerpValue = 1 - Mathf.Pow(.25f, Time.deltaTime);

        Vector3 targetCenter = target.transform.position + (Vector3.up * 1.43f);

        Vector3 newDirection = Vector3.Slerp((targetCenter - tr.position).normalized, currentMoveDirection, slerpValue);

        currentMoveDirection = newDirection;

        rb.velocity = speed * currentMoveDirection;

        tr.position += rb.velocity * Time.deltaTime;
    }
}
