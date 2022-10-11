using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ItemTrajectory : MonoBehaviour
{
    public bool isThrown;
    public float chargeAmount;
    [SerializeField]
    private float chargeAmountMultiplier = 80f;

    private bool wasThrown;

    private Transform tr;
    private Rigidbody rb;

    private Vector3 velocity;
    private Vector3 currentMoveDirection;
    private float speed;

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

        // isThrown only happens the first frame throw is released.
        if (isThrown && !wasThrown)
        {
            InitialThrow();
            wasThrown = true;
        }
        if (wasThrown && target != null)
        {
            UpdateTargetedThrow();
        }

        //Debug.Log(wasThrown);

        // Set isThrown to false when it's "grounded" or being held.
        //if (rb.velocity.magnitude < 0.01f)
        //{
        //    isThrown = false;
        //    wasThrown = false;
        //}
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Prevent collision with thrower
        if (collision.gameObject != throwerObject)
        {
            // Checks for collision with enemy.
            if (isThrown && collision.gameObject.CompareTag(Tag.Player.ToString()))
            {
                PlayerStatus hitPlayerStatus = collision.gameObject.GetComponent<PlayerStatus>();
                hitPlayerStatus.GetHitByThrowable(transform.position, collision.transform.position, 10, 12 * chargeAmount + 3, 13 * chargeAmount + 7, thrower);
            }

            wasThrown = false;
            isThrown = false;
            rb.useGravity = true;
        }
    }

    private void InitialThrow()
    {

        if (target != null)
        {
            Vector3 initialDirection = (thrower.movement.ActualFowardDirection + (Vector3.up * 2)).normalized;

            currentMoveDirection = initialDirection;
            speed = 15 * chargeAmount + 20;

            rb.useGravity = false;
        }
        else
        {
            Vector3 initialDirection = (thrower.movement.ActualFowardDirection + (Vector3.up * 2)).normalized;

            initialDirection.x *= chargeAmount * chargeAmountMultiplier + 20;
            initialDirection.z *= chargeAmount * chargeAmountMultiplier + 20;
            initialDirection.y = 5 * chargeAmount + 4; //chargeamt from 0-3

            rb.velocity = new Vector3(0,0,0);
            rb.AddForce(initialDirection, ForceMode.Impulse);
        }
        // rb.velocity = new Vector3(0,0,0);
        // rb.AddForce(initialDirection, ForceMode.Impulse);

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

        velocity = speed * currentMoveDirection;

        tr.position += velocity * Time.deltaTime;

        // Add small impulse every update to course correct.
        // Vector3 correctedDirection = chargeAmount * chargeAmountMultiplier * (target.transform.position - transform.position).normalized;
        // Vector3 forceToAdd = correctedDirection - rb.transform.forward.normalized;


        // rb.AddForce(forceToAdd * aimAssistMultiplier * Time.deltaTime, ForceMode.Force);
    }
}
