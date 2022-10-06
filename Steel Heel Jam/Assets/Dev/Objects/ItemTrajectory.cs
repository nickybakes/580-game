using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ItemTrajectory : MonoBehaviour
{
    public bool isThrown;
    public float chargeAmount;
    [SerializeField]
    private float chargeAmountMultiplier = 20f;
    [SerializeField]
    private float aimAssistMultiplier = 300f;

    private bool wasThrown;

    private Transform tr;
    private Rigidbody rb;

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
            if (collision.gameObject.CompareTag(Tag.Player.ToString()))
            {
                PlayerStatus hitPlayerStatus = collision.gameObject.GetComponent<PlayerStatus>();
                hitPlayerStatus.GetHitByThrowable(transform.position, collision.transform.position, 10, 5 * chargeAmount, 5 * chargeAmount/*, thrower*/);
            }

            wasThrown = false;
            isThrown = false;
        }
    }

    private void InitialThrow()
    {
        Vector3 initialDirection = thrower.movement.ActualFowardDirection;
        initialDirection.x *= chargeAmount * chargeAmountMultiplier;
        initialDirection.z *= chargeAmount * chargeAmountMultiplier;
        initialDirection.y = 5 * chargeAmount; //chargeamt from 0-3

        rb.velocity = new Vector3(0,0,0);
        rb.AddForce(initialDirection, ForceMode.Impulse);

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

        // Add small impulse every update to course correct.
        Vector3 correctedDirection = chargeAmount * chargeAmountMultiplier * (target.transform.position - transform.position).normalized;
        Vector3 forceToAdd = correctedDirection - rb.transform.forward.normalized;

        rb.AddForce(forceToAdd * aimAssistMultiplier * Time.deltaTime, ForceMode.Force);
    }
}
