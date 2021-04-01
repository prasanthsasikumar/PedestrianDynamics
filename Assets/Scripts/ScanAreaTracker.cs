using LightBuzz.Kinect4Azure;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScanAreaTracker : MonoBehaviour
{
    private List<Body> trackedBodies, trackedBodiesInsideScanArea;
    public Text NumberOfPeopleinScanArea;
    

    public FlowMonitor flowMonitor;

    // Start is called before the first frame update
    void Start() 
    {
        trackedBodies = new List<Body>();
        trackedBodiesInsideScanArea = new List<Body>();
    }

    private void OnTriggerEnter(Collider other)
    {
        uint bodyId = other.transform.parent.transform.parent.gameObject.GetComponent<Stickman>().id;
        foreach(Body trackedBody in trackedBodies)
        {
            if(trackedBody.ID == bodyId)
            {
                flowMonitor.AddTrackingInfo(bodyId, other.transform.position, Time.time * 1000);
                return;
            }
        }
    }    

    public List<Body> GetListOfBodiesthatAreInScanArea()
    {
        return trackedBodiesInsideScanArea;
    }    
    
    private void Update()
    {
        trackedBodiesInsideScanArea = new List<Body>();//Clear the list every update
        trackedBodies = GameObject.FindObjectOfType<TrackPedestrians>().GetTrackedBodies();
        if (trackedBodies == null) return;

        foreach (Body trackedBody in trackedBodies)
        {
            if (this.GetComponent<BoxCollider>().bounds.Contains(trackedBody.Joints[JointType.Pelvis].Position))
            {
                trackedBodiesInsideScanArea.Add(trackedBody);
            }
        }

        NumberOfPeopleinScanArea.text = "In Scan space : " + trackedBodiesInsideScanArea.Count;
    }

    bool ColliderContainsPoint(Transform ColliderTransform, Vector3 Point)
    {
        Vector3 localPos = ColliderTransform.InverseTransformPoint(Point);
        if (Mathf.Abs(localPos.x) < 0.5f && Mathf.Abs(localPos.y) < 0.5f && Mathf.Abs(localPos.z) < 0.5f)
            return true;
        else
            return false;
    }
}
