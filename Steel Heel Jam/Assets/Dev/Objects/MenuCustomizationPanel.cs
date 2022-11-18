using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

public class MenuCustomizationPanel : MonoBehaviour
{
    [Range(1, 8)]
    public int playerNumber = 1;

    public Image border;

    public TextMeshProUGUI valueDisplay;
    public TextMeshProUGUI limitDisplay;

    public Animator arrowAnimator;

    public GameObject[] categories;
    public int[] limits;

    private int currentCategory;

    private int currentValue;

    // Start is called before the first frame update
    void Start()
    {
        border.color = PlayerToken.colors[playerNumber - 1];
    }

    // Update is called once per frame
    void Update()
    {

    }

    public int GetCurrentCategory()
    {
        return currentCategory;
    }

    public void SwitchCategory(bool up, int valueToFill)
    {
        categories[currentCategory].SetActive(false);
        if (up)
        {
            arrowAnimator.SetTrigger("Up");
            if (currentCategory == 0)
                currentCategory = categories.Length - 1;
            else
                currentCategory--;
        }
        else
        {
            arrowAnimator.SetTrigger("Down");
            if (currentCategory == categories.Length - 1)
                currentCategory = 0;
            else
                currentCategory++;
        }

        categories[currentCategory].SetActive(true);
        valueDisplay.text = "" + (currentValue + 1);
        limitDisplay.text = "/" + limits[currentCategory];
    }

    public int ChangeValue(bool decrease)
    {
        if (decrease)
        {
            arrowAnimator.SetTrigger("Left");
            if (currentValue == 0)
                currentValue = limits[currentCategory] - 1;
            else
                currentValue--;
        }
        else
        {
            arrowAnimator.SetTrigger("Right");
            if (currentValue == limits[currentCategory] - 1)
                currentValue = 0;
            else
                currentValue++;
        }

        valueDisplay.text = "" + (currentValue + 1);

        return currentValue;
    }
}
