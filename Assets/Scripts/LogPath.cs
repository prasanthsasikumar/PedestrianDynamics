using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LogPath : MonoBehaviour
{
    public string filePath;
    public string filePathFull;
    // Start is called before the first frame update
    void Start()
    {
        filePath = Application.persistentDataPath;
        filePathFull = System.IO.Path.Combine(filePath, "PedestrainDynamics" + "." + System.DateTime.Now.ToString("yyyy.MM.dd.HH.mm.ss") + ".csv");
        this.GetComponent<Text>().text = filePathFull;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
