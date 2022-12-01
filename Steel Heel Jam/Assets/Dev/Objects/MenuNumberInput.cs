using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;
using UnityEngine.UI;

public class MenuNumberInput : MonoBehaviour
{
    public bool wrapAround = true;

    public int value = 5;

    public int min = 0;

    public int max = 10;

    public int interval = 1;

    public TextMeshProUGUI valueText;

    public UnityEvent<int> onValueChange;

    public UnityEvent<MenuNumberInput> onAwake;

    // Start is called before the first frame update
    void Start()
    {
        UpdateValueText();
    }

    private void Awake()
    {
        onAwake.Invoke(this);
    }

    public void UpdateValueText()
    {
        valueText.text = "" + value;
    }

    public void IncrementValue()
    {
        value += interval;
        if (wrapAround)
        {
            if (value > max)
            {
                value = min;
            }
        }
        else
        {
            if (value > max)
            {
                value = max;
            }
        }
        UpdateValueText();
        onValueChange.Invoke(value);
    }

    public void DecrementValue()
    {
        value -= interval;
        if (wrapAround)
        {
            if (value < min)
            {
                value = max;
            }
        }
        else
        {
            if (value < min)
            {
                value = min;
            }
        }
        UpdateValueText();
        onValueChange.Invoke(value);
    }
}
