using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenDirectory : MonoBehaviour
{
    string itemPath;

    private void Start()
    {
        itemPath = Application.persistentDataPath;
    }

    public void ShowExplorer()
    {
        itemPath = itemPath.Replace(@"/", @"\");   // explorer doesn't like front slashes
        System.Diagnostics.Process.Start("explorer.exe", "/select," + itemPath);
    }
}
