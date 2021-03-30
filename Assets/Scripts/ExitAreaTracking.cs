using LightBuzz.Kinect4Azure;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExitAreaTracking : MonoBehaviour
{

    private List<uint> peopleExited, peopleEntered;

    public Text entryStatus,exitStatus;
    public ScanAreaTracker InfrontOfExit;
    public FlowMonitor flowMonitor;

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
        //Debug.Log(other.transform.parent.transform.parent.gameObject.GetComponent<Stickman>().id);
        uint personId = other.transform.parent.transform.parent.gameObject.GetComponent<Stickman>().id;
        if(!peopleExited.Contains(personId))
        {
            //Entering inside the building
            if (InfrontOfExit.GetListOfBodiesthatAreInScanArea().Contains(personId) && !peopleEntered.Contains(personId))
            {
                peopleExited.Add(personId);
                flowMonitor.AddTrackingInfo(personId,transform.position,Time.time);
                Debug.Log("Body " + personId + " exited", DLogType.ScanArea); //at " + Time.time);
            }
            else
            {
                if (!peopleEntered.Contains(personId))
                {
                    peopleEntered.Add(personId);
                    Debug.Log("Body " + personId + " entered",DLogType.ScanArea);// at " + Time.time);
                }
            }

        }
    }

    public int GetNumberofPeopleExited()
    {
        return peopleExited.Count;
    }
}
