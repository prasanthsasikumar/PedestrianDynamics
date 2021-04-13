using LightBuzz.Kinect4Azure;
using LightBuzz.Kinect4Azure.Video;
using Microsoft.Azure.Kinect.Sensor;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class ApplicationManager : MonoBehaviour
{
    [SerializeField] private Configuration _configuration;
    [SerializeField] private PointCloud _pointCloud;
    [SerializeField] private StickmanManager _stickmanManager;
    [SerializeField] private MediaBarPlayer mediaBarPlayer;

    [Tooltip("The rotation and zoom speed when using the left/right/top/down arrow keys or the mouse wheel.")]
    [Range(0.0f, 1.0f)]
    [SerializeField] private float _speed = 0.5f;

    private KinectSensor _sensor;
    private List<Body> trackedBodies;
    private VideoRecorder _recorder;
    private bool isRecording = false;
    public BGRA[] pointCloudColorArray;
    public Short3[] pointCloudDepthArray;

    private void Start()
    {
        _sensor = KinectSensor.Create(_configuration);

        if (_sensor == null)
        {
            Debug.LogError("Sensor not connected!");
            return;
        }

        _sensor.Open();

        Camera.main.transform.LookAt(_pointCloud.transform.position);

        _recorder = new VideoRecorder(new VideoConfiguration
        {
            Path = Path.Combine(Application.persistentDataPath, "Video"),
            ColorResolution = _sensor.Configuration.ColorResolution.Size(),
            DepthResolution = _sensor.Configuration.DepthMode.Size(),
            RecordColor = true,
            RecordDepth = false,
            RecordBody = true,
            RecordFloor = false,
            RecordIMU = false
        });
        _recorder.OnRecordingStarted += OnRecordingStarted;
        _recorder.OnRecordingStopped += OnRecordingStopped;
        _recorder.OnRecordingCompleted += OnRecordingCompleted;
    }


    private void OnDestroy()
    {
        _sensor?.Close();
    }

    private void Update()
    {
        if (_sensor == null || !_sensor.IsOpen) return;

        Frame frame = _sensor.Update();
        if (mediaBarPlayer.IsPlaying)
        {
            frame = mediaBarPlayer.Update();
        }

        if (frame != null)
        {
            var pointCloudDepth = frame.DepthFrameSource?.PointCloud;
            var pointCloudColor = frame.ColorFrameSource?.PointCloud;
            var bodies = frame.BodyFrameSource?.Bodies;
            trackedBodies = new List<Body>();
            trackedBodies = frame.BodyFrameSource?.Bodies;
            pointCloudColorArray = pointCloudColor;
            pointCloudDepthArray = pointCloudDepth;

            _pointCloud.Load(pointCloudColor, pointCloudDepth);
            _stickmanManager.Load(bodies);
        }

        if(isRecording) _recorder?.Update(frame);
    }

    public List<Body> GetTrackedBodies()
    {
        return trackedBodies;
    }

    private void LateUpdate()
    {
        Vector3 cameraPosition = Camera.main.transform.localPosition;
        Vector3 originPosition = _pointCloud.transform.position;
        float angle = _speed * 100.0f * Time.deltaTime;

        if (Input.GetKey(KeyCode.RightArrow))
        {
            Camera.main.transform.RotateAround(originPosition, Vector3.up, angle);
        }

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            Camera.main.transform.RotateAround(originPosition, Vector3.down, angle);
        }

        if (Input.GetKey(KeyCode.UpArrow))
        {
            Camera.main.transform.RotateAround(originPosition, Vector3.right, angle);
        }

        if (Input.GetKey(KeyCode.DownArrow))
        {
            Camera.main.transform.RotateAround(originPosition, Vector3.left, angle);
        }

        if (Input.mouseScrollDelta != Vector2.zero)
        {
            Camera.main.transform.localPosition = new Vector3(cameraPosition.x, cameraPosition.y, cameraPosition.z + Input.mouseScrollDelta.y * _speed);
        }
    }

    private void OnRecordingCompleted()
    {
        Debug.Log("Recording completed");        
    }

    private void OnRecordingStopped()
    {
        Debug.Log("Recording stopped");        
    }

    private void OnRecordingStarted()
    {
        Debug.Log("Recording started");        
    }

    public void StartStopRecording()
    {
        isRecording = !isRecording;

        if (isRecording)
            _recorder.Start();
        else
            _recorder.Stop();
    }

    public void StartStopPlayback(bool state)
    {
        if(!state) mediaBarPlayer.Stop();
        else
        {
            mediaBarPlayer.LoadVideo(_recorder.Configuration.Path);
            mediaBarPlayer.Play();
        }
    }
}

