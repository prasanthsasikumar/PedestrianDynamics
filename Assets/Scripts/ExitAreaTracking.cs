using LightBuzz.Kinect4Azure;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExitAreaTracking : MonoBehaviour
{
    public enum AttachedToSide
    {
        Right = 0,
        Left = 1,
        FrontSide = 2,
        BackSide = 3
    }

    private List<uint> peopleExited, peopleEntered;

    public Text entryStatus,exitStatus;
    public ScanAreaTracker ScanArea;
    public FlowMonitor flowMonitor;
    public AttachedToSide thisSide = AttachedToSide.FrontSide;
    
    // Start is called before the first frame update
    void Start()
    {
        peopleExited = new List<uint>();
        peopleEntered = new List<uint>();
    }

    // Update is called once per frame
    void Update()
    {
        //Logging number of people exited.
        entryStatus.text = "Entered : " + peopleEntered.Count;
        exitStatus.text = "Exited : " + peopleExited.Count;
    }

    private void OnTriggerEnter(Collider other)
    {        
        //Debug.Log(other.transform.parent.transform.parent.gameObject);
        //Parent of this collider is the bone joint. It's parent is the skelton. Getting the id from the skelton.
        uint bodyID = other.transform.parent.transform.parent.gameObject.GetComponent<Stickman>().id;
        
        if (!peopleExited.Contains(bodyID))
        {
            //Exiting scan area = people in scannig area hiting collider 
            if (CheckIfBodyIsInScanArea(bodyID) && !peopleEntered.Contains(bodyID))
            {
                peopleExited.Add(bodyID);
                flowMonitor.AddTrackingInfo(bodyID,transform.position, System.DateTime.Now, false);
                Debug.Log("Body " + bodyID + " exited via "+ thisSide, DLogType.ScanArea); //at " + Time.time);
                //Debug.Log(Vector3.Distance(other.transform.position, ScanArea.transform.position));
                //Debug.Log(Vector3.Distance(this.transform.position, ScanArea.transform.position));
            }
            else //Comming inside scan area = people in not in scannig area hitiing collider 
            {
                if (!peopleEntered.Contains(bodyID))
                {
                    peopleEntered.Add(bodyID);
                    Debug.Log("Body " + bodyID + " entered via " + thisSide,DLogType.ScanArea);// at " + Time.time);
                    //Debug.Log(Vector3.Distance(other.transform.position, ScanArea.transform.position));
                    //Debug.Log(Vector3.Distance(this.transform.position, ScanArea.transform.position));
                }
            }

        }
    }

    public bool CheckIfBodyIsInScanArea(uint bodyId)
    {
        foreach(Body trackedBody in ScanArea.GetListOfBodiesthatAreInScanArea())
        {
            if (trackedBody.ID == bodyId) return true;
        }        
        return false;
    }

    public int GetNumberofPeopleExited()
    {
        return peopleExited.Count;
    }
    public int GetNumberofPeopleEntered()
    {
        return peopleEntered.Count;
    }
}
