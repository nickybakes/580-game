using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIRainbowText : MonoBehaviour
{

    private TextMeshProUGUI text;


    public float speed = 1;

    public float saturation = 1;
    public float value = 1;

    private float alpha;

    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
        alpha = text.color.a;
    }

    // Update is called once per frame
    void Update()
    {
        Color color = Color.HSVToRGB(Mathf.PingPong(Time.time * speed, 1), saturation, value);
        color.a = alpha;
        text.color = color;
    }
}
