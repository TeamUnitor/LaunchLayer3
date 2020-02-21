using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderHandler : MonoBehaviour
{
    private Text text_fadeDuration;

    // Start is called before the first frame update
    void Start()
    {
        text_fadeDuration = GetComponent<Text>();    
    }

    public void textUpdate(float value)
    {
        text_fadeDuration.text = value.ToString("N3");
    }
}
