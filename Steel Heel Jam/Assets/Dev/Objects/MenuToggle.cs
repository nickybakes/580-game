using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;


public class MenuToggle : MonoBehaviour
{
    public bool value;

    public Image checkmarkImage;
    public Animator animator;

    public UnityEvent<bool> onValueChange;

    public UnityEvent<MenuToggle> onAwake;


    // Start is called before the first frame update
    void Start()
    {
        UpdateImage();
    }

    private void Awake()
    {
        onAwake.Invoke(this);
    }

    public void UpdateImage()
    {
        checkmarkImage.gameObject.SetActive(value);
    }

    public void Toggle()
    {
        value = !value;
        UpdateImage();
        onValueChange.Invoke(value);
        animator.SetBool("Bouncing", value);
    }
}
