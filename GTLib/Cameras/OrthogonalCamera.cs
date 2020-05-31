using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace GTLib.Cameras
{
    public class OrthogonalCamera:Camera
    {
        public OrthogonalCamera(Vector3 eye, Vector3 focus, Vector3 up) : base(eye, focus, up)
        {
        }
    }
}
