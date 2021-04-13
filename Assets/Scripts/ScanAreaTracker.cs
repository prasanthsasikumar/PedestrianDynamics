using LightBuzz.Kinect4Azure;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScanAreaTracker : MonoBehaviour
{
    private List<Body> trackedBodies, trackedBodiesInsideScanArea;
    private GameObject[] exits;

    public Text NumberOfPeopleinScanArea;
    public FlowMonitor flowMonitor;
    
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
                flowMonitor.AddTrackingInfo(bodyId, other.transform.position, System.DateTime.Now, true);
               // trackedBodiesInsideScanArea.Add(trackedBody);
                Debug.Log("Adding TrackedBody " + trackedBodiesInsideScanArea.Count, DLogType.Logic);
                return;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        uint bodyId = other.transform.parent.transform.parent.gameObject.GetComponent<Stickman>().id;
        foreach (Body trackedBody in trackedBodies)
        {
            if (trackedBody.ID == bodyId)
            {
                //trackedBodiesInsideScanArea.Remove(trackedBody);
                Debug.Log("Removing TrackedBody " + trackedBodiesInsideScanArea.Count, DLogType.Logic);
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
        trackedBodies = GameObject.FindObjectOfType<ApplicationManager>().GetTrackedBodies();
        if (trackedBodies == null) return;

        foreach (Body trackedBody in trackedBodies)
        {
            if (this.GetComponent<BoxCollider>().bounds.Contains(trackedBody.Joints[JointType.Pelvis].Position) && !CheckIfAreaOverlappingWithExits(trackedBody.Joints[JointType.Pelvis].Position))
            {
                trackedBodiesInsideScanArea.Add(trackedBody);
            }
        }

        NumberOfPeopleinScanArea.text = "In Scan space : " + trackedBodiesInsideScanArea.Count;
    }

    //NOT USED
    bool ColliderContainsPoint(Transform ColliderTransform, Vector3 Point)
    {
        Vector3 localPos = ColliderTransform.InverseTransformPoint(Point);
        if (Mathf.Abs(localPos.x) < 0.5f && Mathf.Abs(localPos.y) < 0.5f && Mathf.Abs(localPos.z) < 0.5f)
            return true;
        else
            return false;
    }

    private bool CheckIfAreaOverlappingWithExits(Vector3 position)
    {
        exits = null;
        exits =  GameObject.FindGameObjectsWithTag("exit");
        foreach (GameObject exit in exits)
        {
            if (exit.GetComponent<BoxCollider>().bounds.Contains(position)) return true;
        }
        return false;
    }
}
