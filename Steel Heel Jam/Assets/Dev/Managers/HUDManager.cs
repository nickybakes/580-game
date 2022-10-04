using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUDManager : MonoBehaviour
{

    public GameObject headerPanel;

    private Canvas canvas;

    public GameObject headerPrefab;

    public List<PlayerHeader> headers;

    private ItemManager itemManagerScript;
    //public GameObject arrowPrefab;
    //public List<GameObject> arrows;


    public void CreatePlayerHeader(PlayerStatus status)
    {
        if (!canvas)
            canvas = GetComponent<Canvas>();

        GameObject headerObject = Instantiate(headerPrefab, headerPanel.transform);
        PlayerHeader header = headerObject.GetComponent<PlayerHeader>();
        headers.Add(header);
        header.Setup(status, canvas);
    }

    public void RemoveHeader(PlayerStatus status)
    {
        PlayerHeader header = null;
        foreach(PlayerHeader h in headers){
            if(h.Status == status){
                header = h;
            }
        }

        if(header != null){
            headers.Remove(header);
            Destroy(header.gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        canvas = GetComponent<Canvas>();
        // Since both are prefabs...
        itemManagerScript = GameObject.FindGameObjectWithTag("ItemManager").GetComponent<ItemManager>();
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < itemManagerScript.itemsOnGround.Count; i++)
        {
            if (!itemManagerScript.itemsOnGround[i].GetComponent<Item>().isOnScreen)
            {
                // Create a new GUI arrow.


                // Calculate direction to point arrow.
                //Vector3 itemVector = NormalVectorToItem(i);
                //Debug.Log("Normal direction is: " + itemVector.x + ", " + itemVector.z);

                // Calculate edge of screen.
            }
        }
    }


    /// <summary>
    /// Calculates the normal vec3 from the middle of the camera projection to the item.
    /// </summary>
    /// <returns>Normalized vector 3 pointing towards the item.</returns>
    private Vector3 NormalVectorToItem(int i)
    {
        var ray = Camera.main.ScreenPointToRay(new Vector2(Screen.height / 2, Screen.width / 2));
        RaycastHit hitPoint;

        if (Physics.Raycast(ray, out hitPoint, 100.0f))
        {
            return (hitPoint.point - itemManagerScript.itemsOnGround[i].transform.position).normalized;
        }

        return new Vector3();
    }
}
