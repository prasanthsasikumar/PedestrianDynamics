using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpdateTextOnSliderChange : MonoBehaviour
{
    public Text text;
    public void UpdateText(float value)
    {
        text.text = value.ToString();
    }

    public void Start()
    {
        text.text = this.GetComponent<Slider>().value.ToString() + " m";
        //also set the size of the block from here
    }

}
