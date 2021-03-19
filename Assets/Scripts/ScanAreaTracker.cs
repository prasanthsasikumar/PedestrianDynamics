using LightBuzz.Kinect4Azure;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScanAreaTracker : MonoBehaviour
{
    private List<uint> bodiesThatEnteredScanArea, bodiesCurrentlyInScanArea;

    public FlowMonitor flowMonitor;

    // Start is called before the first frame update
    void Start()
    {
        bodiesThatEnteredScanArea = new List<uint>();//REMOVE THIS EVENTUALLY!!!!!!!!!!!!!!
        bodiesCurrentlyInScanArea = new List<uint>();
    }


    private void OnTriggerEnter(Collider other)
    {
        uint personId = other.transform.parent.transform.parent.gameObject.GetComponent<Stickman>().id;
        if (!bodiesThatEnteredScanArea.Contains(personId))
        {
            bodiesThatEnteredScanArea.Add(personId);
            flowMonitor.AddTrackingInfo(personId, other.transform.position, Time.time);
        }
        if (!bodiesCurrentlyInScanArea.Contains(personId))
        {
            bodiesCurrentlyInScanArea.Add(personId);
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

    public List<uint> GetListOfBodiesthatEnteredScanArea()
    {
        //Debug.Log(peopleAboutToExit.Count);
        return bodiesThatEnteredScanArea;
    }
    public List<uint> GetListOfBodiesthatAreInScanArea()
    {
        //Debug.Log(peopleAboutToExit.Count);
        return bodiesCurrentlyInScanArea;
    }
}
