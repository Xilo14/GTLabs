using OpenTK;
using System;
using System.Collections.Generic;
using System.Text;

namespace GTLib.Light
{
    public class DirectLight : Light
    {
        public Vector3 LightVector { get ; set ; } = new Vector3(0,0,-1);
    }
}
