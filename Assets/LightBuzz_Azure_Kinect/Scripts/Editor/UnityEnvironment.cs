using System;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace LightBuzz.Kinect4Azure
{
    [InitializeOnLoadAttribute]
    public class UnityEnvironment : MonoBehaviour
    {
        /// <summary>
        /// The names of the Azure Kinect binaries that need to be copied in the root directory.
        /// </summary>
        public static readonly string[] KinectBinaries = new string[]
        {
            "cublas64_100.dll",
            "cudart64_100.dll",
            "cudnn64_7.dll",
            "onnxruntime.dll",
            "dnn_model_2_0.onnx"
        };

        /// <summary>
        /// The asset folder.
        /// </summary>
        public static readonly string AssetFolder = "LightBuzz_Azure_Kinect";

        /// <summary>
        /// The native C++ plugins folder.
        /// </summary>
        public static readonly string PluginsFolder = Path.Combine(AssetFolder, "Plugins", "x86_64");

        /// <summary>
        /// The LightBuzz K4A version file.
        /// </summary>
        public static readonly string VersionFile = "version.txt";

        /// <summary>
        /// Copies the required Azure Kinect binaries to the root folder.
        /// </summary>
        static UnityEnvironment()
        {
            if (Application.platform != RuntimePlatform.WindowsEditor) return;

            string assetFolder = Path.Combine(Application.dataPath, AssetFolder);
            string binariesFolder = Path.Combine(Application.dataPath, PluginsFolder);
            string rootFolder = Directory.GetParent(Application.dataPath).FullName;

            string versionFileCurrent = Path.Combine(assetFolder, VersionFile);
            string versionFilePrevious = Path.Combine(rootFolder, VersionFile);

            bool isNewVersion = true;
            bool errorOccurred = false;

            if (File.Exists(versionFileCurrent) && File.Exists(versionFilePrevious))
            {
                string versionCurrent = File.ReadAllText(versionFileCurrent);
                string versionPrevious = File.ReadAllText(versionFilePrevious);

                if (versionCurrent == versionPrevious)
                {
                    isNewVersion = false;
                }
            }
            else
            {
                Debug.LogWarning("Version file does not exist under " + versionFileCurrent);
                Debug.LogWarning("Consider importing the LightBuzz K4A SDK package again.");
            }

            foreach (string file in KinectBinaries)
            {
                string origin = Path.Combine(binariesFolder, file);
                string destination = Path.Combine(rootFolder, file);

                if (isNewVersion && File.Exists(origin))
                {
                    try
                    {
                        File.Copy(origin, destination, true);
                    }
                    catch (IOException ex)
                    {
                        // Binaries already exist and they are in use.
                    }
                    catch (Exception ex)
                    {
                        Debug.LogError("Could not copy K4A binary to parent folder: " + origin);
                        Debug.LogError(ex);
                        Debug.Log("Try to copy the binaries from the Plugins/x86_64 folder to the root folder manually.");
                        Debug.Log("Binaries folder: " + binariesFolder);
                        Debug.Log("Root folder: " + rootFolder);

                        errorOccurred = true;
                    }
                }
            }

            if (!errorOccurred)
            {
                File.Copy(versionFileCurrent, versionFilePrevious, true);
            }

            Debug.Log("Successfully initialized Unity environment for Kinect!");
        }

        [MenuItem("LightBuzz/Kinect for Azure/Support")]
        static void Support()
        {
            Application.OpenURL("https://lightbuzz.com/support?product=Azure%20Kinect%20for%20Unity3D");
        }

        [MenuItem("LightBuzz/Kinect for Azure/Rate")]
        static void Rate()
        {
            Application.OpenURL("http://u3d.as/1FBs#review");
        }

        [MenuItem("LightBuzz/Kinect for Azure/Documentation")]
        static void Documentation()
        {
            Application.OpenURL("https://lightbuzz.com/tools/azure-kinect-unity/documentation");
        }
    }
}
