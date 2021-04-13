using LightBuzz.Kinect4Azure;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrameRecordings : ScriptableObject
{
    [SerializeField]
    public List<Frame> frames = new List<Frame>();
    public List<float> frameTimes = new List<float>();
}
