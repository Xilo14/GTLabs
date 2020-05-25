using System;
using System.Collections.Generic;
using System.Text;
using GTLib.Interfaces;

namespace GTLib.Primitives
{
    public class Dot2D: Primitive2D
    {
        public double X { get; set; }
        public double Y { get; set; }

        public Dot2D(double X, double Y)
        {
            this.X = X;
            this.Y = Y;
        }
        public Dot2D() : this(0,0) { }
    }
}
