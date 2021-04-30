using LightBuzz.Kinect4Azure;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScanAreaTracker : MonoBehaviour
{
    private List<Body> bodiesEnteredScanArea, trackedBodiesInsideScanArea;
    private GameObject[] exits;

    public Text NumberOfPeopleinScanArea;
    public FlowMonitor flowMonitor;
    public ApplicationManager applicationManager;
    public int algorithm;
    
    void Start() 
    {
        bodiesEnteredScanArea = new List<Body>();
        trackedBodiesInsideScanArea = new List<Body>();
    }

    private void OnTriggerEnter(Collider other)
    {
        uint bodyId = other.transform.parent.transform.parent.gameObject.GetComponent<Stickman>().id;
        foreach(Body trackedBody in applicationManager.GetTrackedBodies())
        {
            if(trackedBody.ID == bodyId)
            {
                flowMonitor.AddTrackingInfo(bodyId, other.transform.position, System.DateTime.Now, true);
                bodiesEnteredScanArea.Add(trackedBody);
                if(algorithm == 6) trackedBodiesInsideScanArea.Add(trackedBody);
                return;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        uint bodyId = other.transform.parent.transform.parent.gameObject.GetComponent<Stickman>().id;
        foreach (Body trackedBody in applicationManager.GetTrackedBodies())
        {
            if (trackedBody.ID == bodyId)
            {
                if (algorithm == 6) trackedBodiesInsideScanArea.Remove(trackedBody);
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
       // trackedBodies = ;
        if (applicationManager.GetTrackedBodies() == null) return;

        foreach (Body trackedBody in applicationManager.GetTrackedBodies())
        {
            if (algorithm == 1)//MainOne
            {
                if (!BodyNotEnteredBefore(trackedBody.ID)) return;
                if (IsPointInside(trackedBody.Joints[JointType.Pelvis].Position, trackedBody.ID)) trackedBodiesInsideScanArea.Add(trackedBody);
            } else if (algorithm == 2)
            {
                if (IsPointInside(trackedBody.Joints[JointType.Pelvis].Position, trackedBody.ID)) trackedBodiesInsideScanArea.Add(trackedBody);
            } else if(algorithm == 3)
            {
                if (this.GetComponent<BoxCollider>().bounds.Contains(trackedBody.Joints[JointType.Pelvis].Position) && !CheckIfAreaOverlappingWithExits(trackedBody.Joints[JointType.Pelvis].Position))
                {
                    trackedBodiesInsideScanArea.Add(trackedBody);
                }
            }else if(algorithm == 4)
            {
                if (!BodyNotEnteredBefore(trackedBody.ID)) return;
                if (CheckIfPointInside(trackedBody.Joints[JointType.Pelvis].Position, trackedBody.ID)) trackedBodiesInsideScanArea.Add(trackedBody); Debug.Log("4");
            }
            else if (algorithm == 5)
            {
                if (CheckIfPointInside(trackedBody.Joints[JointType.Pelvis].Position, trackedBody.ID)) trackedBodiesInsideScanArea.Add(trackedBody); Debug.Log("5");
            }
            else if (algorithm == 6)
            {
                trackedBodiesInsideScanArea.Add(trackedBody);Debug.Log("6");
            }
        }

        NumberOfPeopleinScanArea.text = "In Scan space : " + trackedBodiesInsideScanArea.Count;
    }

    bool BodyNotEnteredBefore(uint id)
    {
        foreach(Body body in bodiesEnteredScanArea)
        {
            if (body.ID == id) return true;
        }
        return false;
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

    private bool CheckIfPointInside(Vector3 point, uint id)
    {
        Vector3 center = this.transform.position;
        Vector3 half = this.transform.localScale / 2;
        Vector3 dx = Vector3.Normalize(this.transform.position);

        Vector3 d = point - center;
        bool inside = Mathf.Abs(Vector3.Dot(d, dx)) <= half.x;/* &&
                      Mathf.Abs(Vector3.Dot(d, dy)) <= half.y &&
                      Mathf.Abs(Vector3.Dot(d, dz)) <= half.z;*/

        //Debug.Log(inside);
        //Debug.Log(Mathf.Abs(Vector3.Dot(d, dx)) +":"+ half.x+"   :   " + id);

        return inside;
    }

    bool IsPointInside(Vector3 point, uint id)
    {
        Vector3 half = this.transform.localScale / 2;
        Vector3 localPos = transform.InverseTransformPoint(point);

        //Debug.Log(Mathf.Abs(localPos.y) + " : " + half.y + "   :   " + id);
        if (Mathf.Abs(localPos.x) < half.x && Mathf.Abs(localPos.y) < half.y && Mathf.Abs(localPos.z) < half.z)
            return true;
        else
            return false;
    }
}
