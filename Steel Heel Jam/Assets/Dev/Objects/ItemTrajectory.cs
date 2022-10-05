using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemTrajectory : MonoBehaviour
{
    public bool isTargetedThrow;
    public bool isThrown;
    public float chargeAmount;
    private float chargeAmountMultiplier = 0.1f;

    private bool wasThrown;
    private Vector3 throwDirection;
    //private float gravity = -0.1f;

    private Transform tr;

    public PlayerStatus target;
    public PlayerStatus thrower;

    // Start is called before the first frame update
    void Start()
    {
        tr = transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (isThrown)
        {
            if (!wasThrown)
                throwDirection = GetInitialDirection();

            if (isTargetedThrow)
            {
                UpdateTargetedThrow();
            }
            else
            {
                UpdateStraightThrow();
            }

        }
    }

    private void UpdateStraightThrow()
    {

        //transform.position = thrower.movement.ActualFowardDirection * chargeAmount;
        //Debug.Log(thrower.movement.ActualFowardDirection);
        //throwDirection.y += gravity * Time.deltaTime;

        tr.position = new Vector3(tr.position.x + (throwDirection.x * chargeAmount * chargeAmountMultiplier), tr.position.y/* + (throwDirection.y)*/, tr.position.z + (throwDirection.z * chargeAmount * chargeAmountMultiplier));
    }

    private void UpdateTargetedThrow()
    {
        
    }

    private Vector3 GetInitialDirection()
    {
        wasThrown = true;
        return thrower.movement.ActualFowardDirection;
    }
}
