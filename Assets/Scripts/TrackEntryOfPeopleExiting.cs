using LightBuzz.Kinect4Azure;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackEntryOfPeopleExiting : MonoBehaviour
{
    private List<uint> peopleAboutToExit;

    // Start is called before the first frame update
    void Start()
    {
        peopleAboutToExit = new List<uint>();
    }


    private void OnTriggerEnter(Collider other)
    {
        uint personId = other.transform.parent.transform.parent.gameObject.GetComponent<Stickman>().id;
        if (!peopleAboutToExit.Contains(personId))
        {
            peopleAboutToExit.Add(personId);
        }
    }

    public List<uint> GetPeopleInTrackingSpace()
    {
        //Debug.Log(peopleAboutToExit.Count);
        return peopleAboutToExit;
    }
}
