using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MenuButtonType
{
    StartMatch,

}

public class MenuButton : MonoBehaviour
{

    public MenuButtonType buttonType;

    public GameObject buttonHoverObject;

    [HideInInspector]
    private List<PlayerCursor> cursorsHoveringThisButton;


    // Start is called before the first frame update
    void Start()
    {
        cursorsHoveringThisButton = new List<PlayerCursor>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter2D(Collider2D other)
    {
        buttonHoverObject.SetActive(true);

        if (other.CompareTag(Tag.UICursor.ToString()))
        {
            PlayerCursor cursor = other.gameObject.GetComponent<PlayerCursor>();
            cursorsHoveringThisButton.Add(cursor);
        }

    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag(Tag.UICursor.ToString()))
        {
            PlayerCursor cursor = other.gameObject.GetComponent<PlayerCursor>();
            cursorsHoveringThisButton.Remove(cursor);

            if (cursorsHoveringThisButton.Count == 0)
            {
                buttonHoverObject.SetActive(false);
            }
        }
    }
}
