# PedestrianDynamics [Massey University]

   [![N|Solid](https://github.com/prasanthsasikumar/PedestrianDynamics/blob/main/Assets/Resources/MasseyLogo.png)](https://www.massey.ac.nz/)

[![N|Solid](https://github.com/prasanthsasikumar/PedestrianDynamics/blob/main/Assets/Resources/frg.png)](https://fireresearchgroup.com/)

[![Build Status](https://travis-ci.org/joemccann/dillinger.svg?branch=master)](https://github.com/prasanthsasikumar/PedestrianDynamics)

Unity Application that uses a kinect device to track movemnet of people in a give space. Using UI, the tracking space can be adjusted along with other parameters.

## Modules
  * Configuration Manager - Lets the user save and load all the UI parameters as external configuration file.
  * Record/Playback - Lets users record and playback body tracking information without capturing RGB information. (Ethics applications). Recording function has the ability to capture a point cloud frame to get the reference.
  * Scan Area - Users can use the sliders to adjust the size and rotation of the scanning area.
  * Exit areas - Users can chose to track exit areas from the scan space.
  * Density and Area - shows the realtime density and area of the scanning area.
  * Navigation - User can adjust the scan area by clicking and dragging the scaning box. Right click and WASD lets the user move in the 3D space. 
  * Data Logging - Data logged to a csv file under the following labels:

## Logged Data labels
| **Option** | **Unit** | **Description** |
| --- | --- | --- |
| `ScanArea` | `m^2` | Area of the space being scanned. |
| `Speed` | `m/s` | Speed of each body moving in the scanning area.|
| `Density` | `pp/m^2` | Number of people / Scan Area. |
| `AverageSpeed` | `m/s` | Average speed of the body during the total duration of its movement. |
| `FlowRate` | `pp/s` | Number of people exiting through a side |
| `Trajectory` | `Position` | Position of the body during each second|

### REQUIREMENTS
- Microsoft Azure Kinect 
- Unity 2020 and above to build.[Executable for windows packaged]
- Kinect SDK

### Support
Working Video can be found here - https://youtu.be/S6yG16kHJk4


### Downloads(Source code)
- Please find the source code here - https://github.com/prasanthsasikumar/PedestrianDynamics/
- Issues can be reported here - https://github.com/prasanthsasikumar/PedestrianDynamics//issues/new


License
----

Massey University - Digital Reasearch Lab [Reserved] 


