using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MenuButton : MonoBehaviour
{
    public GameObject[] buttonHoverObjects;

    [HideInInspector]
    public List<PlayerCursor> cursorsHoveringThisButton;

    [HideInInspector] public RectTransform rect;

    public UnityEvent submitAction;

    [HideInInspector]
    public MenuButton[] buttonSelects;
    public MenuButton upSelect;
    public MenuButton downSelect;
    public MenuButton leftSelect;
    public MenuButton rightSelect;

    // Start is called before the first frame update
    void Start()
    {
        rect = GetComponent<RectTransform>();
        cursorsHoveringThisButton = new List<PlayerCursor>();

        buttonSelects = new MenuButton[4]
        {
            upSelect,
            downSelect,
            leftSelect,
            rightSelect
        };
    }

    // Update is called once per frame
    void Update()
    {
        bool invokeSubmit = false;
        foreach (PlayerCursor cursor in cursorsHoveringThisButton)
        {
            if (cursor._input.accept && !cursor._input.wasAccepting)
            {
                invokeSubmit = true;
                cursor._input.accept = false;
                break;
            }
        }

        if (invokeSubmit)
        {
            submitAction.Invoke();
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {

        if (other.CompareTag(Tag.UICursor.ToString()))
        {
            PlayerCursor cursor = other.gameObject.GetComponent<PlayerCursor>();
            AddCursor(cursor);
        }

    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag(Tag.UICursor.ToString()))
        {
            PlayerCursor cursor = other.gameObject.GetComponent<PlayerCursor>();
            RemoveCursor(cursor);
        }
    }

    public void AddCursor(PlayerCursor cursor)
    {
        foreach (GameObject g in buttonHoverObjects)
        {
            g.SetActive(true);
        }
        cursor.highlightedButton = this;
        cursorsHoveringThisButton.Add(cursor);
    }

    public void RemoveCursor(PlayerCursor cursor)
    {
        cursor.highlightedButton = null;
        cursorsHoveringThisButton.Remove(cursor);

        if (cursorsHoveringThisButton.Count == 0)
        {
            foreach (GameObject g in buttonHoverObjects)
            {
                g.SetActive(false);
            }
        }

    }
}
