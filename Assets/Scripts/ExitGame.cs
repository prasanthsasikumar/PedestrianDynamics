using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitGame : MonoBehaviour
{
    public void QuitGame()
    {
        GameObject.FindObjectOfType<FlowMonitor>().PrintTrajectory();
        Application.Quit();
    }
}
