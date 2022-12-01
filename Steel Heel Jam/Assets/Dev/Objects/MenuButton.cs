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

    [HideInInspector]
    public new BoxCollider2D collider;

    public Vector2 LocalPosition
    {
        get
        {
            Transform currentTransform = transform;
            Vector2 currentPosition = transform.localPosition;
            while (!currentTransform.parent.gameObject.CompareTag("Canvas"))
            {
                currentTransform = currentTransform.parent;
                currentPosition += (Vector2)currentTransform.localPosition;
            }

            return currentPosition;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        rect = GetComponent<RectTransform>();
        collider = GetComponent<BoxCollider2D>();
        cursorsHoveringThisButton = new List<PlayerCursor>();

        buttonSelects = new MenuButton[4]
        {
            upSelect,
            rightSelect,
            downSelect,
            leftSelect,
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

    /*
public enum SnapDirection
{
    Up,
    Right,
    Down,
    Left,
}
    */

    public MenuButton GetProperNextSelect(SnapDirection direction)
    {
        SnapDirection[][] orders = new SnapDirection[4][];
        orders[(int)SnapDirection.Up] = new[] { SnapDirection.Right, SnapDirection.Left };
        orders[(int)SnapDirection.Right] = new[] { SnapDirection.Up, SnapDirection.Down };
        orders[(int)SnapDirection.Down] = new[] { SnapDirection.Right, SnapDirection.Left };
        orders[(int)SnapDirection.Left] = new[] { SnapDirection.Up, SnapDirection.Down };

        MenuButton nextSelect = buttonSelects[(int)direction];

        if (nextSelect != null && !nextSelect.gameObject.activeInHierarchy)
        {
            nextSelect = nextSelect.GetNextSelect(direction);
        }
        else
        {
            return nextSelect;
        }

        //if we cant find any in that direction
        //look for an alternate button close by
        if (nextSelect == null)
        {
            MenuButton requestedNextSelect = buttonSelects[(int)direction];

            if (requestedNextSelect != null)
            {
                return requestedNextSelect.GetClosestButton(requestedNextSelect.GetNextSelect(orders[(int)direction][0]), requestedNextSelect.GetNextSelect(orders[(int)direction][1]));
            }
        }

        return nextSelect;
    }

    private MenuButton GetClosestButton(MenuButton a, MenuButton b)
    {
        if (a == null && b != null)
            return b;
        else if (a != null && b == null)
            return a;
        else if (a == null && b == null)
            return null;

        if (Vector2.Distance(LocalPosition, a.LocalPosition) <= Vector2.Distance(LocalPosition, b.LocalPosition))
            return a;

        return b;
    }

    private MenuButton GetNextSelect(SnapDirection direction)
    {
        MenuButton nextSelect = buttonSelects[(int)direction];
        if (nextSelect != null && !nextSelect.gameObject.activeInHierarchy)
        {
            return nextSelect.GetNextSelect(direction);
        }
        else if (nextSelect != null && nextSelect.gameObject.activeInHierarchy)
        {
            return nextSelect;
        }
        return null;
    }
}
