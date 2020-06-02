using OpenTK;
using System;
using System.Collections.Generic;
using System.Text;

namespace GTLib.Light
{
    class DirectLight : Light
    {
        private Vector3 _lightVector;

        public Vector3 LightVector { get => _lightVector; set => _lightVector = value; }
    }
}
