using GTLib.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace GTLib.Primitives
{
    public class Circle2D: Primitive2D
    {
        public Dot2D Center { get; set; }
        public int Radius { get; set; }
    }
}
