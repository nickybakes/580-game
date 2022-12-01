using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;


public class MenuToggle : MonoBehaviour
{

    public bool value;

    public Image checkmarkImage;

    public UnityEvent onValueChange;


    // Start is called before the first frame update
    void Start()
    {
        UpdateImage();
    }

    public void UpdateImage()
    {
        checkmarkImage.gameObject.SetActive(value);
    }

    public void Toggle()
    {
        value = !value;
        UpdateImage();
        onValueChange.Invoke();
    }
}
