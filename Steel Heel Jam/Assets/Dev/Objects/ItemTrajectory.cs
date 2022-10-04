using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemTrajectory : MonoBehaviour
{
    public bool isTargetedThrow;
    public bool isThrown;
    public float chargeAmount;

    private GameObject _item;

    public PlayerStatus target;
    public PlayerStatus thrower;

    // Start is called before the first frame update
    void Start()
    {
        if (transform.parent != null)
            _item = transform.parent.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (isThrown)
        {
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
        //Debug.Log(thrower.movement.ActualFowardDirection);

        
    }

    private void UpdateTargetedThrow()
    {
        
    }
}
