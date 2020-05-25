using GTLib.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace GTLib.Primitives
{
    public class Circle2D: Primitive2D
    {
        public Dot2D Center { get; set; }
        public double Radius { get; set; }

        public Circle2D(Dot2D center,int radius)
        {
            this.Center = center;
            this.Radius = radius;
        }
    }
}
