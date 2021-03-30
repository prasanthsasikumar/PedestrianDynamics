using LightBuzz.Kinect4Azure;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScanAreaTracker : MonoBehaviour
{
    private List<uint> bodiesCurrentlyInScanArea;
    public Text NumberOfPeopleinScanArea;

    public FlowMonitor flowMonitor;

    // Start is called before the first frame update
    void Start() 
    {
        bodiesCurrentlyInScanArea = new List<uint>();
    }

    private void Update()
    {

        NumberOfPeopleinScanArea.text = "In Scan space : " + bodiesCurrentlyInScanArea.Count;
    }


    private void OnTriggerEnter(Collider other)
    {
        uint personId = other.transform.parent.transform.parent.gameObject.GetComponent<Stickman>().id;
        if (!bodiesCurrentlyInScanArea.Contains(personId))
        {
            bodiesCurrentlyInScanArea.Add(personId);
            flowMonitor.AddTrackingInfo(personId, other.transform.position, Time.time);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        uint personId = other.transform.parent.transform.parent.gameObject.GetComponent<Stickman>().id;
        if (bodiesCurrentlyInScanArea.Contains(personId))
        {
            bodiesCurrentlyInScanArea.Remove(personId);
        }
    }
    public List<uint> GetListOfBodiesthatAreInScanArea()
    {
        //Debug.Log(peopleAboutToExit.Count);
        return bodiesCurrentlyInScanArea;
    }
}
