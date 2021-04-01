using AwesomeCharts;
using LightBuzz.Kinect4Azure;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FlowMonitor : MonoBehaviour
{
    private float density, flow;
    private List<Body> listOfBodiesInScanArea;
    private int deltaTime = 2;
    private List<TrackedBody> trackedBodies;
    private List<DataPoint> datapoints;
    private LineDataSet set1;
    private ExitAreaTracking[] exitAreas;

    public Text densityText;
    public LineChart chart;
    public GameObject scanArea;
    public StickmanManager stickmanManager;
    public TMP_Dropdown timeScale;

    // Start is called before the first frame update
    void Start()
    {
        trackedBodies = new List<TrackedBody>();
        datapoints = new List<DataPoint>();
        exitAreas = (ExitAreaTracking[])GameObject.FindObjectsOfType(typeof(ExitAreaTracking));
        Debug.Log("Flow Check every " + (timeScale.value+1) + "s");
        InvokeRepeating("TrackFlow", 2.0f, timeScale.value+1);
        ConfigChart(); 
        set1 = new LineDataSet();

        datapoints.Add(new DataPoint(0, 1));
        datapoints.Add(new DataPoint(1, 0));
        datapoints.Add(new DataPoint(2, 0));//Remove this below too. Done due to an error drawing the chart in UI
        datapoints.Add(new DataPoint(3, 0));
        datapoints.Add(new DataPoint(4, 0));
        datapoints.Add(new DataPoint(5, 0));
        //AddChartData();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TrackFlow()
    {
        //Get the number of skeltons that are being tracked. Density calculated by number of people in the tracked area. Flow tracked by how much people move in a given time
        listOfBodiesInScanArea = scanArea.GetComponent<ScanAreaTracker>().GetListOfBodiesthatAreInScanArea();
        density = listOfBodiesInScanArea.Count / (scanArea.transform.localScale.x * scanArea.transform.localScale.z);
        densityText.text = "Density:" + density + "pp/m^2. Area: " + scanArea.transform.localScale.y * scanArea.transform.localScale.z + "m^2";
        Debug.Log("Density is " + density + "/m^2. Scan Area is " + scanArea.transform.localScale.x * scanArea.transform.localScale.z, DLogType.Density);


        //Get skeltons in tracking area and store thier position
        foreach (Body body in listOfBodiesInScanArea)
        {
            Vector3 trackedBodyPoint = body.Joints[JointType.Pelvis].Position;            
            foreach (TrackedBody trackedBody in trackedBodies)
            {
                if (trackedBody.bodyId == body.ID)//Already being tracked, add position information
                {
                    trackedBody.positions.Add(trackedBodyPoint);
                    trackedBody.timestamp.Add(Time.time * 1000);
                }
            }                

            //update the datapoint entry For graph
            foreach (DataPoint datapoint in datapoints.ToArray())
            {
                //remove matching one and add the new one.
                if(datapoint.density == trackedBodies.Count)
                {
                    datapoints.Remove(datapoint);
                    datapoints.Add(new DataPoint(trackedBodies.Count, trackedBodies));
                }
            }

        }

        //List the speed of people moving in space.
        foreach (TrackedBody trackedBody in trackedBodies)
        {
            //If it has been a while since the last update, the body is long gone. So we can stop logging its speed.
            if ((Time.time * 1000 - trackedBody.timestamp[trackedBody.timestamp.Count - 1]) < (timeScale.value + 1) && trackedBody.AverageSpeed() > 0)
            {
                Debug.Log("Speed of Body " + trackedBody.bodyId + " is " + trackedBody.AverageSpeed() + "m/s and Density is " + density, DLogType.Speed);
                //Debug.Log("Speed of Body " + trackedBody.bodyId + " is " + trackedBody.GetSpeed() + "m/s and Density is " + density, DLogType.Speed);
            }
        }

        foreach (ExitAreaTracking exitArea in exitAreas)
        {
           // Debug.Log(exitArea.gameObject.name + " - flow rate : " + exitArea.GetNumberofPeopleExited(), DLogType.FlowRate);
        }

        //Draw the Graph
        chart.GetChartData().DataSets.Clear();
        set1.Clear();
        //Debug.Log("Current Density and Speed");
        foreach (DataPoint datapoint in datapoints)
        {
            set1.AddEntry(new LineEntry(datapoint.density, datapoint.speed*100f));
            if (datapoint.speed> 0.000123f) {
                //Debug.Log("Density : " + datapoint.density + " Speed : " + datapoint.speed);
            }
        }
        chart.GetChartData().DataSets.Add(set1);
        chart.SetDirty();
    }

    public void AddTrackingInfo(uint bodyId, Vector3 position, float time)
    {
        bool foundBodyInList = false;
        foreach (TrackedBody trackedBody in trackedBodies)
        {
            if (trackedBody.bodyId == bodyId)
            {
                trackedBody.positions.Add(position);
                trackedBody.timestamp.Add(time);
                foundBodyInList = true;
            }
        }
        if (!foundBodyInList)//New body. Add to list
        {
            TrackedBody newbody = new TrackedBody(bodyId, position, time);
            trackedBodies.Add(newbody);
        }
    }

    private void ConfigChart()
    {
        chart.Config.ValueIndicatorSize = 17;

        chart.XAxis.DashedLine = true;
        //chart.XAxis.LineThickness = 1;
        chart.XAxis.LabelColor = Color.white;
        //chart.XAxis.LabelSize = 18;

        chart.YAxis.DashedLine = true;
        //chart.YAxis.LineThickness = 1;
        chart.YAxis.LabelColor = Color.white;
        //chart.YAxis.LabelSize = 16;
    }

    private void AddChartData()
    {
        LineDataSet set1 = new LineDataSet();
        set1.AddEntry(new LineEntry(0.5f, 0.5f));
        set1.AddEntry(new LineEntry(0.7f, 0.8f));
        //set1.AddEntry(new LineEntry(30, 62));
        //set1.AddEntry(new LineEntry(50, 46));
        //set1.AddEntry(new LineEntry(70, 31));
        //set1.AddEntry(new LineEntry(90, 20));

        set1.LineColor = new Color32(54, 105, 126, 255);
        set1.FillColor = new Color32(54, 105, 126, 110);

        chart.GetChartData().DataSets.Add(set1);
        //chart.GetChartData().DataSets.Add(set2);

        chart.SetDirty();
    }
}

public class TrackedBody
{
    public uint bodyId;
    public List<Vector3> positions;
    public List<float> timestamp;

    public TrackedBody(uint bodyId, Vector3 position, float time)
    {
        this.bodyId = bodyId;
        positions = new List<Vector3>();
        timestamp = new List<float>();
        positions.Add(position);
        timestamp.Add(time);
    }

    public float AverageSpeed()
    {
        if (positions.Count > 1)
        {
            return (Vector3.Distance(positions[0], positions[positions.Count - 1])) / (timestamp[timestamp.Count - 1] - timestamp[0]) * 1000;
        }
        else
        {
            return 0;
        }

    }

    public float GetSpeed()
    {
        if (positions.Count > 1)
        {
            return (Vector3.Distance(positions[positions.Count - 2], positions[positions.Count - 1])) / (timestamp[timestamp.Count - 1] - timestamp[timestamp.Count - 2]) * 1000;
        }
        else
        {
            return 0;
        }
    }

}

public class FlowClass
{

}

public class DataPoint
{
    public int density;
    public float speed = 0f;

    public DataPoint(int density, List<TrackedBody> trackedBodies)
    {
        this.density = density;
        foreach (TrackedBody trackedBody in trackedBodies)
        {
            speed += trackedBody.AverageSpeed();
        }
        speed = speed / trackedBodies.Count;
    }
    public DataPoint(int density, float speed)
    {
        this.density = density;
        this.speed = speed; 
    }

    public override bool Equals(System.Object obj)
    {
        if (obj == null)
            return false;
        DataPoint data = obj as DataPoint;
        if (this.density == data.density)
        {
            return true;
        }
        return false;
    }

    public bool Equals(DataPoint data)
    {
        if (this.density == data.density)
        {
            return true;
        }
        return false;

    }
}