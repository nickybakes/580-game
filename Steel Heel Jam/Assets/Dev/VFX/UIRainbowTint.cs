using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIRainbowTint : MonoBehaviour
{

    private Image image;


    public float speed = 1;

    public float saturation = 1;
    public float value = 1;

    // Start is called before the first frame update
    void Start()
    {
        image = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        image.color = Color.HSVToRGB(Mathf.PingPong(Time.time * speed, 1), saturation, value);
    }
}
