using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlaybackAlgorithmSelector : MonoBehaviour
{
    public void ValueSelected(bool value)
    {
        //UncheckALL
        //public List<PlaybackAlgorithmSelector> list =  ;
        if (!value) return;


        foreach (PlaybackAlgorithmSelector obj in GameObject.FindObjectsOfType<PlaybackAlgorithmSelector>())
        {
            if (obj.gameObject.name != this.name) obj.gameObject.GetComponent<Toggle>().isOn = false;
        }
        this.GetComponent<Toggle>().isOn = true;

        //Debug.Log(this.name.Substring(this.name.Length - 1));
        GameObject.FindObjectOfType<ScanAreaTracker>().algorithm = int.Parse(this.name.Substring(this.name.Length - 1));
    }
}
