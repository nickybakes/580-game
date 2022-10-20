using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum VisualChild
{

    None = -1,
    Block = 0,
    DodgeRoll = 1,
    Stun = 2,
    Recovery = 3,
    Knockback = 4
}

public class PlayerVisuals
{

    private PlayerStatus status;

    private Transform tr;

    private Animator animator;

    private Transform modelTransform;

    private AnimationState currentAnimationState;

    private GameObject blockSphere;
    private GameObject dodgeRollSphere;
    private GameObject stunSphere;
    private GameObject recoverySphere;
    private GameObject knockbackSphere;

    public PlayerVisuals(PlayerStatus _status, Transform _tr)
    {
        tr = _tr;
        status = _status;

        animator = tr.GetComponentInChildren<Animator>();
        modelTransform = tr.GetChild((int)PlayerChild.Model);

        blockSphere = tr.GetChild((int)PlayerChild.Visuals).GetChild((int)VisualChild.Block).gameObject;
        dodgeRollSphere = tr.GetChild((int)PlayerChild.Visuals).GetChild((int)VisualChild.DodgeRoll).gameObject;
        stunSphere = tr.GetChild((int)PlayerChild.Visuals).GetChild((int)VisualChild.Stun).gameObject;
        recoverySphere = tr.GetChild((int)PlayerChild.Visuals).GetChild((int)VisualChild.Recovery).gameObject;
        knockbackSphere = tr.GetChild((int)PlayerChild.Visuals).GetChild((int)VisualChild.Knockback).gameObject;
    }

    /// <summary>
    /// Rotatethe character model on the X axis
    /// </summary>
    /// <param name="degrees">Angle in degrees</param>
    public void SetModelRotationX(float degrees)
    {
        modelTransform.localRotation = Quaternion.Euler(degrees, 0, 0);
    }

    /// <summary>
    /// When the character is flying through the air (like in the knockback state or suplex state)
    /// this will angle the character model so their body is pointed in the direction they are moving head first
    /// </summary>
    public void RotateModelFlyingThroughAir()
    {
        float topDownSpeed = status.movement.ActualTopDownSpeed;

        float angle = 0;

        if (topDownSpeed == 0)
        {
            if (status.movement.velocity.y < 0)
            {
                angle = 90;
            }
        }
        else
        {
            angle = Mathf.Rad2Deg * Mathf.Atan(status.movement.velocity.y / topDownSpeed);
        }

        SetModelRotationX(-90 + angle);
    }

    public void SetAnimationState(AnimationState state)
    {
        if (currentAnimationState == state)
            return;

        currentAnimationState = state;

        animator.SetTrigger(state.ToString());
        // animator.SetLayerWeight()
    }

    public void EnableVisual(VisualChild vc)
    {
        ClearAll();
        switch (vc)
        {
            case (VisualChild.Block):
                Block();
                break;
            case (VisualChild.DodgeRoll):
                DodgeRoll();
                break;
            case (VisualChild.Stun):
                Stun();
                break;
            case (VisualChild.Knockback):
                Knockback();
                break;
            case (VisualChild.Recovery):
                Recovery();
                break;
        }
    }

    private void ClearAll()
    {
        blockSphere.SetActive(false);
        dodgeRollSphere.SetActive(false);
        stunSphere.SetActive(false);
        recoverySphere.SetActive(false);
        knockbackSphere.SetActive(false);
    }

    private void Block()
    {
        blockSphere.SetActive(true);
    }

    private void DodgeRoll()
    {
        dodgeRollSphere.SetActive(true);
    }

    private void Stun()
    {
        stunSphere.SetActive(true);
    }

    private void Recovery()
    {
        recoverySphere.SetActive(true);
    }

    private void Knockback()
    {
        knockbackSphere.SetActive(true);
    }
}
