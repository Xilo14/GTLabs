using System;
using System.Collections.Generic;
using System.Text;

namespace GTLib.Primitives
{
    public class Dot3D:Primitive3D
    {
        public Dot3D(double x, double y,double z)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
        }

        public Dot3D() : this(0, 0,0) { }

        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }
    }
}
