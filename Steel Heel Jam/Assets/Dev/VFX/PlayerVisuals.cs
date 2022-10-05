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
        modelTransform = animator.transform;

        blockSphere = tr.GetChild((int)PlayerChild.Visuals).GetChild((int)VisualChild.Block).gameObject;
        dodgeRollSphere = tr.GetChild((int)PlayerChild.Visuals).GetChild((int)VisualChild.DodgeRoll).gameObject;
        stunSphere = tr.GetChild((int)PlayerChild.Visuals).GetChild((int)VisualChild.Stun).gameObject;
        recoverySphere = tr.GetChild((int)PlayerChild.Visuals).GetChild((int)VisualChild.Recovery).gameObject;
        knockbackSphere = tr.GetChild((int)PlayerChild.Visuals).GetChild((int)VisualChild.Knockback).gameObject;
    }

/// <summary>
/// Do this later, make the model rotate while in knockout state, so it looks
/// like its drilling through the air
/// </summary>
/// <param name="radians">Angle in radians</param>
    public void SetModelRotationX(float radians)
    {
        Vector3 center = modelTransform.position + new Vector3(0, modelTransform.localScale.y, 0);

        // modelTransform.RotateAround(center, Vector3.right, angle);

    }

    public void SetAnimationState(AnimationState state)
    {
        if (currentAnimationState == state)
            return;

        currentAnimationState = state;

        animator.SetTrigger(state.ToString());
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
