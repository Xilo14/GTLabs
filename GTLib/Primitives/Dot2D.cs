using System;
using System.Collections.Generic;
using System.Text;
using GTLib.Interfaces;

namespace GTLib.Primitives
{
    public class Dot2D: Primitive2D
    {
        public int X { get; set; }
        public int Y { get; set; }

        public Dot2D(int X,int Y)
        {
            this.X = X;
            this.Y = Y;
        }
        public Dot2D() : this(0,0) { }
    }
}
