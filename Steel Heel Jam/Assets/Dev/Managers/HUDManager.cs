using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUDManager : MonoBehaviour
{

    public GameObject headerPanel;

    private Canvas canvas;

    public GameObject headerPrefab;

    public List<PlayerHeader> headers;


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
    }
}
