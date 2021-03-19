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
    public Slider scanArealength, scanAreabreadth, exitAreaLength;
    public Toggle enablePointCloud, EnableLogging;
    public TMP_Dropdown timeScale;
    public Transform scanArea, exitArea;

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
        save.exitAreaPositionX = exitArea.position.x;
        save.exitAreaPositionY = exitArea.position.y;
        save.exitAreaPositionZ = exitArea.position.z;
        save.exitAreaRotationX = exitArea.rotation.x;
        save.exitAreaRotationY = exitArea.rotation.y;
        save.exitAreaRotationZ = exitArea.rotation.z;
        save.exitAreaRotationW = exitArea.rotation.w;
        save.exitAreaScaleX = exitArea.localScale.x;
        save.exitAreaScaleY = exitArea.localScale.y;
        save.exitAreaScaleZ = exitArea.localScale.z;
        return save;
    }

    public void SetConfigState(Save save)
    {
        scanArealength.value = save.scanArealength;
        scanAreabreadth.value = save.scanAreabreadth;
        exitAreaLength.value = save.exitAreaLength;
        enablePointCloud.isOn = save.enablePointCloud;
        EnableLogging.isOn = save.enableLogging;
        timeScale.value = save.timeScale;
        scanArea.position = new Vector3(save.scanAreaPositionX, save.scanAreaPositionY, save.scanAreaPositionZ);
        scanArea.localRotation = new Quaternion(save.scanAreaRotationX, save.scanAreaRotationY, save.scanAreaRotationZ, save.scanAreaRotationW);
        scanArea.localScale = new Vector3(save.scanAreaScaleX, save.scanAreaScaleY, save.scanAreaScaleZ);
        exitArea.position = new Vector3(save.exitAreaPositionX, save.exitAreaPositionY, save.exitAreaPositionZ); 
        exitArea.rotation = new Quaternion(save.exitAreaRotationX, save.exitAreaRotationY, save.exitAreaRotationZ, save.exitAreaRotationW);
        exitArea.localScale = new Vector3(save.exitAreaScaleX, save.exitAreaScaleY, save.exitAreaScaleZ);
    }

    public void SaveConfiguration()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/PD_Config.save");
        bf.Serialize(file, GetConfigState());
        file.Close();
        Debug.Log("Configuration Saved");
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
}

[System.Serializable]
public class Save
{
    [SerializeField]
    public float scanArealength, scanAreabreadth, exitAreaLength;
    [SerializeField]
    public bool enablePointCloud, enableLogging;
    [SerializeField]
    public int timeScale;
    [SerializeField]
    public float scanAreaPositionX, scanAreaPositionY, scanAreaPositionZ, exitAreaPositionX, exitAreaPositionY, exitAreaPositionZ, scanAreaScaleX, scanAreaScaleY, scanAreaScaleZ, exitAreaScaleX, exitAreaScaleY, exitAreaScaleZ;
    [SerializeField]
    public float scanAreaRotationX, scanAreaRotationY, scanAreaRotationZ, scanAreaRotationW, exitAreaRotationX, exitAreaRotationY, exitAreaRotationZ, exitAreaRotationW;
}
