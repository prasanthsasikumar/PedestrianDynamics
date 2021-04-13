using LightBuzz.Kinect4Azure;
using Microsoft.Azure.Kinect.Sensor;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ConfigurationManager : MonoBehaviour
{
    private Save save;
    public bool loadSnapshot { get; set; }
    public Slider scanArealength, scanAreabreadth, exitAreaLength;
    public Toggle enablePointCloud, EnableLogging, leftExit, rightExit, frontExit, rearExit;
    public TMP_Dropdown timeScale;
    public Transform scanArea;
    public ApplicationManager applicationManager;
    public PointCloud _pointCloud;

    public void Start()
    {
        LoadConfiguration();
    }

    public Save GetConfigState()
    {
        save = new Save();
        save.scanArealength = scanArealength.value;
        save.scanAreabreadth = scanAreabreadth.value;
        save.exitAreaLength = exitAreaLength.value;
        save.enablePointCloud = enablePointCloud.isOn;
        save.enableLogging = EnableLogging.isOn;
        save.leftExit = leftExit.isOn;
        save.rightExit = rightExit.isOn;
        save.frontExit = frontExit.isOn;
        save.rearExit = rearExit.isOn;
        save.timeScale = timeScale.value;
        save.scanAreaPositionX = scanArea.position.x;
        save.scanAreaPositionY = scanArea.position.y;
        save.scanAreaPositionZ = scanArea.position.z;
        save.scanAreaRotationX = scanArea.localRotation.x;
        save.scanAreaRotationY = scanArea.localRotation.y;
        save.scanAreaRotationZ = scanArea.localRotation.z;
        save.scanAreaRotationW = scanArea.localRotation.w;
        save.scanAreaScaleX = scanArea.localScale.x;
        save.scanAreaScaleY = scanArea.localScale.y;
        save.scanAreaScaleZ = scanArea.localScale.z;
        return save;
    }

    public void SetConfigState(Save save)
    {
        scanArealength.value = save.scanArealength;
        scanAreabreadth.value = save.scanAreabreadth;
        exitAreaLength.value = save.exitAreaLength;
        enablePointCloud.isOn = save.enablePointCloud;
        EnableLogging.isOn = save.enableLogging;
        leftExit.isOn = save.leftExit;
        rightExit.isOn = save.rightExit;
        frontExit.isOn = save.frontExit;
        rearExit.isOn = save.rearExit;
        timeScale.value = save.timeScale;
        scanArea.position = new Vector3(save.scanAreaPositionX, save.scanAreaPositionY, save.scanAreaPositionZ);
        scanArea.localRotation = new Quaternion(save.scanAreaRotationX, save.scanAreaRotationY, save.scanAreaRotationZ, save.scanAreaRotationW);
        scanArea.localScale = new Vector3(save.scanAreaScaleX, save.scanAreaScaleY, save.scanAreaScaleZ);
    }

    public SnapShot GetSnapShot()
    {
        SnapShot snap = new SnapShot();
        snap.pointCloudColor = new SnapColorFrame[applicationManager.pointCloudColorArray.Length];
        snap.pointCloudDepth = new SnapDepthFrame[applicationManager.pointCloudDepthArray.Length];
        for (int i=0;i<applicationManager.pointCloudColorArray.Length; i++)
        {
            SnapColorFrame snapColorFrame = new SnapColorFrame();
            snapColorFrame.A = applicationManager.pointCloudColorArray[i].A;
            snapColorFrame.R = applicationManager.pointCloudColorArray[i].R;
            snapColorFrame.G = applicationManager.pointCloudColorArray[i].G;
            snapColorFrame.B = applicationManager.pointCloudColorArray[i].B;
            snapColorFrame.Value = applicationManager.pointCloudColorArray[i].Value;
            snap.pointCloudColor[i] = snapColorFrame;
        }
        for (int i = 0; i < applicationManager.pointCloudDepthArray.Length; i++)
        {
            SnapDepthFrame snapDepthFrame = new SnapDepthFrame();
            snapDepthFrame.X = applicationManager.pointCloudDepthArray[i].X;
            snapDepthFrame.Y = applicationManager.pointCloudDepthArray[i].Y;
            snapDepthFrame.Z = applicationManager.pointCloudDepthArray[i].Z;
            snap.pointCloudDepth[i] = snapDepthFrame;
        }
        return snap;
    }


    public void SetSnapShot(SnapShot snap)
    {
        BGRA[] color = new BGRA[snap.pointCloudColor.Length];
        Short3[] depth = new Short3[snap.pointCloudDepth.Length];
        for (int i = 0; i < snap.pointCloudColor.Length; i++)
        {
            BGRA bgra = new BGRA(snap.pointCloudColor[i].B, snap.pointCloudColor[i].G, snap.pointCloudColor[i].R, snap.pointCloudColor[i].A);
            color[i] = bgra;
        }
        for (int i = 0; i < snap.pointCloudDepth.Length; i++)
        {
            Short3 xyz = new Short3(snap.pointCloudDepth[i].X, snap.pointCloudDepth[i].Y, snap.pointCloudDepth[i].Z);
            depth[i] = xyz;
        }
        _pointCloud.Load(color, depth);
    }

    public void SaveConfiguration()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/PD_Config.save");
        bf.Serialize(file, GetConfigState());
        file.Close();        
        Debug.Log("Configuration Saved");
    }

    public void SavePointCloud(bool value)
    {
        if (!value) return;
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/Video/PointCloud.save");
        bf.Serialize(file, GetSnapShot());
        file.Close();

        BinaryFormatter bf_config = new BinaryFormatter();
        FileStream file_Config = File.Create(Application.persistentDataPath + "/Video/PD_Config.save");
        bf_config.Serialize(file_Config, GetConfigState());
        file_Config.Close();
        Debug.Log("Snapshot and Configuration Saved");
    }

    public void LoadConfiguration()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Open(Application.persistentDataPath + "/PD_Config.save", FileMode.Open);
        Save save = (Save)bf.Deserialize(file);
        file.Close();
        SetConfigState(save);
        Debug.Log("Configuration Loaded");
    }

    public void LoadPoinCloud(bool value)
    {
        if (!value) return;

        if (loadSnapshot)
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/Video/PointCloud.save", FileMode.Open);
            SnapShot snapShot = (SnapShot)bf.Deserialize(file);
            file.Close();
            SetSnapShot(snapShot);
        }

        BinaryFormatter bf_config = new BinaryFormatter();
        FileStream file_config = File.Open(Application.persistentDataPath + "/Video/PD_Config.save", FileMode.Open);
        Save save = (Save)bf_config.Deserialize(file_config);
        file_config.Close();
        SetConfigState(save);
        Debug.Log("Snapshot and Configuration Loaded");
    }
}

[System.Serializable]
public class Save
{
    [SerializeField]
    public float scanArealength, scanAreabreadth, exitAreaLength;
    [SerializeField]
    public bool enablePointCloud, enableLogging, leftExit, rightExit, frontExit, rearExit;
    [SerializeField]
    public int timeScale;
    [SerializeField]
    public float scanAreaPositionX, scanAreaPositionY, scanAreaPositionZ, scanAreaScaleX, scanAreaScaleY, scanAreaScaleZ;
    [SerializeField]
    public float scanAreaRotationX, scanAreaRotationY, scanAreaRotationZ, scanAreaRotationW;
}

[System.Serializable]
public class SnapShot
{
    [SerializeField]
    public SnapColorFrame[] pointCloudColor;
    [SerializeField]
    public SnapDepthFrame[] pointCloudDepth;
}

[System.Serializable]
public class SnapColorFrame
{
    [SerializeField]
    public byte A, R, G, B;
    [SerializeField]
    public int Value;
}
[System.Serializable]
public class SnapDepthFrame
{
    [SerializeField]
    public short X, Y, Z;
}