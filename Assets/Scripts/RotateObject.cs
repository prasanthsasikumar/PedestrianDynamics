using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RotateObject : MonoBehaviour
{
    public Transform ObjectToRotate;
    public TMP_Dropdown axisDropDown;
    private float rotateValue;

    // Preserve the original and current orientation
    private float previousValue = 0f;
  

    public void RotateNow()
    {
        rotateValue = this.GetComponent<Scrollbar>().value;

        // How much we've changed
        float delta = rotateValue - this.previousValue;

        if (axisDropDown.value == 0)
        {
            ObjectToRotate.Rotate(Vector3.up * delta * 360);
        }
        else if (axisDropDown.value == 1)
        {
            ObjectToRotate.Rotate(Vector3.right * delta * 360);
        }
        else
        {
            ObjectToRotate.Rotate(Vector3.forward * delta * 360);
        }

        // Set our previous value for the next change
        this.previousValue = rotateValue;
    }
}