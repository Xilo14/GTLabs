using System.Numerics;

namespace GTLib.Cameras
{
    public class PerspectiveCamera : Camera
    {
        public PerspectiveCamera(Vector3 eye, Vector3 focus, Vector3 up) : base(eye, focus, up)
        {
        }
    }
}