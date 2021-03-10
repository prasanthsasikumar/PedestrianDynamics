using System.Collections.Generic;
using UnityEngine;
using Avatar = LightBuzz.Kinect4Azure.Avateering.Avatar;


namespace LightBuzz.Kinect4Azure
{
    public class Demo_Kinect4Azure_Avateering : MonoBehaviour
    {
        [SerializeField] private Configuration _configuration;
        [SerializeField] private UniformImage _image;
        [SerializeField] private Avatar[] _avatars;

        private KinectSensor sensor;

        private void Start()
        {
            sensor = KinectSensor.Create(_configuration);

            if (sensor == null)
            {
                Debug.LogError("Sensor not connected!");
                return;
            }

            sensor.Open();
        }

        private void Update()
        {
            if (sensor == null || !sensor.IsOpen) return;

            Frame frame = sensor.Update();

            if (frame != null)
            {
                if (frame.ColorFrameSource != null)
                {
                    _image.Load(frame.ColorFrameSource);
                }

                if (frame.BodyFrameSource != null)
                {
                    UpdateAvatars(frame.BodyFrameSource.Bodies);
                }
            }
        }

        private void OnDestroy()
        {
            sensor?.Close();
        }

        public void DoTPose()
        {
            foreach (Avatar item in _avatars)
            {
                item.DoTPose();
            }
        }

        private void UpdateAvatars(IList<Body> bodies)
        {
            if (bodies == null || bodies.Count == 0) return;
            if (_avatars == null || _avatars.Length == 0) return;

            Body body = bodies.Closest();

            foreach (Avatar avatar in _avatars)
            {
                avatar.Update(body);

                _image.FlipHorizontally = avatar.Flip;
            }
        }
    }
}
