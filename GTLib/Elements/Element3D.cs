using GTLib.Primitives;
using System;
using System.Collections.Generic;
using System.Text;

namespace GTLib.Elements
{
    public abstract class Element3D : Primitive3D
    {
        public double X { set; get; } = 0;
        public double Y { set; get; } = 0;
        public double Z { set; get; } = 0;
    }
}
