using LightBuzz.Kinect4Azure;
using System;
using System.IO;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;

namespace LightBuzz.Kinect4Azure.Avateering_Editor
{
    public class BuildEventsEditor : MonoBehaviour
    {
        /// <summary>
        /// Copies the required Azure Kinect binaries to the build folder.
        /// </summary>
        /// <param name="target">The build target platform.</param>
        /// <param name="pathToBuiltProject">The path to the executable project.</param>
        [PostProcessBuildAttribute(1)]
        public static void OnPostprocessBuild(BuildTarget target, string pathToBuiltProject)
        {
            string bianriesFolder = Path.Combine(Application.dataPath, "LightBuzz_Azure_Kinect", "Plugins", "x86_64");
            string buildFolder = Path.GetDirectoryName(pathToBuiltProject);

            for (int i = 0; i < UnityEnvironment.KinectBinaries.Length; i++)
            {
                string name = UnityEnvironment.KinectBinaries[i];
                string origin = Path.Combine(bianriesFolder, name);
                string destination = Path.Combine(buildFolder, name);

                if (File.Exists(origin))
                {
                    try
                    {
                        File.Copy(origin, destination, true);
                    }
                    catch (Exception ex)
                    {
                        Debug.LogError("Could not copy K4A binary to Build folder: " + origin);
                        Debug.LogError(ex);
                    }
                }
            }

            Debug.Log("Successfully copied K4A binaries to Build folder!");
        }
    }
}
