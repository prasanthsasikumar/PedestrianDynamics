using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleSize : MonoBehaviour
{
    public bool enableAutoScale = false;

    private AutoScale[] scales;

    public void ChangeZAxis(float value)
    {
        transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, value);
        LogArea();
    }

    public void ChangeXAxis(float value)
    {
        transform.localScale = new Vector3(value, transform.localScale.y, transform.localScale.z);
        LogArea();
    }

    public void ChangeYAxis(float value)
    {        
        transform.localScale = new Vector3(transform.localScale.x, value, transform.localScale.z);
        LogArea();
    }

    public void LogArea()
    {
        Debug.Log("Area of " + this.name + " is :" + transform.localScale,DLogType.ScanArea);
        if (enableAutoScale) AutoScale();                      //Remove this outside the log area. 
    }

    public void AutoScale()
    {
        scales = this.GetComponents<AutoScale>();
        foreach(AutoScale obj in scales)
        {
            obj.ScaleToParent();
        }
    }


}
