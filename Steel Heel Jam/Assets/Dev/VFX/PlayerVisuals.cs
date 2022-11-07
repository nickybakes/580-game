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
    Flexing = 4,
    Attack = 5,
}

public class PlayerVisuals
{

    private static GameObject[] visualChildren;

    private PlayerStatus status;

    private Transform tr;

    private Animator animator;

    private Transform modelTransform;
    private SkinnedMeshRenderer modelMeshRenderer;

    private PlayerWeaponVisual weaponHolder;

    private AnimationState currentAnimationState;

    private GameObject blockSphere;
    private GameObject dodgeRollSphere;
    private GameObject stunSphere;
    private GameObject recoverySphere;
    private GameObject knockbackSphere;
    private GameObject attackVisual;
    private AttackParticle currentAttackParticle = AttackParticle.LeftHook;
    private VisualChild currentVisual = VisualChild.Block;

    public PlayerVisuals(PlayerStatus _status, Transform _tr)
    {
        tr = _tr;
        status = _status;

        animator = tr.GetComponentInChildren<Animator>();
        modelTransform = tr.GetChild((int)PlayerChild.Model);
        modelMeshRenderer = modelTransform.GetChild(0).GetChild(1).GetComponent<SkinnedMeshRenderer>();

        weaponHolder = modelTransform.GetComponentInChildren<PlayerWeaponVisual>();


        blockSphere = tr.GetChild((int)PlayerChild.Visuals).GetChild((int)VisualChild.Block).gameObject;
        dodgeRollSphere = tr.GetChild((int)PlayerChild.Visuals).GetChild((int)VisualChild.DodgeRoll).gameObject;
        stunSphere = tr.GetChild((int)PlayerChild.Visuals).GetChild((int)VisualChild.Stun).gameObject;
        recoverySphere = tr.GetChild((int)PlayerChild.Visuals).GetChild((int)VisualChild.Recovery).gameObject;
        knockbackSphere = tr.GetChild((int)PlayerChild.Visuals).GetChild((int)VisualChild.Flexing).gameObject;
        attackVisual = tr.GetChild((int)PlayerChild.Visuals).GetChild((int)VisualChild.Attack).gameObject;

        if (visualChildren == null)
            visualChildren = new GameObject[] { blockSphere, dodgeRollSphere, stunSphere, recoverySphere, knockbackSphere, attackVisual };
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
    }

    public void SetAnimationModifier(AnimationModifier mod)
    {
        for (int i = 1; i < animator.layerCount; i++)
        {
            animator.SetLayerWeight(i, 0);
        }

        if (mod != AnimationModifier.None)
        {
            animator.SetLayerWeight((int)mod, 1);
        }
    }

    public void ShowWeaponVisual(ItemType item)
    {
        weaponHolder.ShowWeapon(item);
    }

    public void EnableIFrames()
    {
        modelMeshRenderer.material.SetFloat("_I_Frames_Blink", 1);
    }

    public void DisableIFrames()
    {
        modelMeshRenderer.material.SetFloat("_I_Frames_Blink", 0);
    }

    public void SetAttackParticle(AttackParticle particle)
    {
        attackVisual.transform.GetChild((int)currentAttackParticle).gameObject.SetActive(false);
        attackVisual.transform.GetChild((int)particle).gameObject.SetActive(true);
        currentAttackParticle = particle;
    }

    public void EnableVisual(VisualChild vc)
    {
        visualChildren[(int)currentVisual].SetActive(false);
        if (vc != VisualChild.None)
        {
            visualChildren[(int)vc].SetActive(true);
            currentVisual = vc;
        }

    }
}
