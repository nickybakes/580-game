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

    [HideInInspector] public RectTransform rect;

    private int currentCategory;

    private int currentValue;

    private Animator animator;

    public CharacterVisualPrefs visualPrefs;

    // Start is called before the first frame update
    void Start()
    {
        rect = GetComponent<RectTransform>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public int GetCurrentCategory()
    {
        return currentCategory;
    }

    public void SwitchCategory(bool up)
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
        UpdateDisplayNumberToPrefs();
        limitDisplay.text = "/" + limits[currentCategory];
    }

    private void UpdateDisplayNumberToPrefs()
    {
        switch (currentCategory)
        {
            case (0):
                valueDisplay.text = "" + (visualPrefs.skinToneIndex + 1);
                currentValue = visualPrefs.skinToneIndex;
                break;
            case (1):
                valueDisplay.text = "" + (visualPrefs.hairStyleIndex + 1);
                currentValue = visualPrefs.hairStyleIndex;
                break;
            case (2):
                valueDisplay.text = "" + (visualPrefs.hairColorIndex + 1);
                currentValue = visualPrefs.hairColorIndex;
                break;
        }
    }

    private void UpdateVisualPrefs()
    {
        switch (currentCategory)
        {
            case (0):
                visualPrefs.skinToneIndex = currentValue;
                break;
            case (1):
                visualPrefs.hairStyleIndex = currentValue;
                break;
            case (2):
                visualPrefs.hairColorIndex = currentValue;
                break;
        }
    }

    public void ChangeValue(bool decrease)
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

        UpdateVisualPrefs();

        MenuManager.menu.characterDisplays[playerNumber - 1].SetVisualPrefs(visualPrefs);

        UpdateDisplayNumberToPrefs();
    }

    public void RandomizeNotSavedImmediately()
    {
        visualPrefs = new CharacterVisualPrefs(Random.Range(0, 16), Random.Range(0, 7), Random.Range(0, 16));

        MenuManager.menu.characterDisplays[playerNumber - 1].SetVisualPrefs(visualPrefs);

        UpdateDisplayNumberToPrefs();
    }

    public void RandomizeSaveImmediately()
    {
        RandomizeNotSavedImmediately();
        SaveChanges();
    }

    public void SaveChanges()
    {
        MenuManager.menu.characterDisplays[playerNumber - 1].SetVisualPrefs(visualPrefs);
        AppManager.app.playerTokens[playerNumber - 1].visualPrefs = visualPrefs;
    }

    public void DiscardChanges()
    {
        visualPrefs = AppManager.app.playerTokens[playerNumber - 1].visualPrefs;
        MenuManager.menu.characterDisplays[playerNumber - 1].SetVisualPrefs(visualPrefs);
    }

    public void OpenPanel()
    {
        visualPrefs = MenuManager.menu.characterDisplays[playerNumber - 1].currentVisualPrefs;
        UpdateDisplayNumberToPrefs();
        border.color = PlayerToken.colors[playerNumber - 1];
        animator.SetTrigger("Open");
    }

    public void ClosePanel()
    {
        animator.SetTrigger("Close");
    }
}
