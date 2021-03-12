using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleSize : MonoBehaviour
{
    public void ChangeZAxis(float value)
    {
        Debug.Log("Area of "+ this.name + " is :" + transform.localScale);
        transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, value);
    }

    public void ChangeXAxis(float value)
    {
        Debug.Log("Area of " + this.name + " is :" + transform.localScale);
        transform.localScale = new Vector3(value, transform.localScale.y, transform.localScale.z);
    }

    public void ChangeYAxis(float value)
    {
        Debug.Log("Area of " + this.name + " is :" + transform.localScale);
        transform.localScale = new Vector3(transform.localScale.x, value, transform.localScale.z);
    }
}
