using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUDManager : MonoBehaviour
{

    public GameObject headerPanel;

    private Canvas canvas;

    public GameObject headerPrefab;


    public void CreatePlayerHeader(GameObject player)
    {
        if (!canvas)
            canvas = GetComponent<Canvas>();
            
        GameObject headerObject = Instantiate(headerPrefab, headerPanel.transform);
        PlayerHeader header = headerObject.GetComponent<PlayerHeader>();
        header.Setup(player.GetComponent<PlayerStatus>(), canvas);
    }

    // Start is called before the first frame update
    void Start()
    {
        canvas = GetComponent<Canvas>();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
