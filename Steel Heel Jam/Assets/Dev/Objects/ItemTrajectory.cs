using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemTrajectory : MonoBehaviour
{
    public bool isTargetedThrow;
    public bool isThrown;
    public float chargeAmount;

    private bool wasThrown;
    private Vector3 throwDirection;
    private float gravity = 0.01f;
    public float startingVelocityY = 1;

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
        throwDirection.y += gravity * Time.deltaTime;

        tr.position = new Vector3(tr.position.x + (throwDirection.x * chargeAmount / 200), tr.position.y + (throwDirection.y / 200), tr.position.z + (throwDirection.z * chargeAmount / 200));
    }

    private void UpdateTargetedThrow()
    {
        
    }

    private Vector3 GetInitialDirection()
    {
        wasThrown = true;

        Vector3 vec3 = thrower.movement.ActualFowardDirection;
        vec3.y = startingVelocityY;
        return vec3;
    }
}
