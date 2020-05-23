using GTLib.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace GTLib.Primitives
{
    class Circle2D: Primitive2D
    {
        Dot2D Center { get; set; }
        int Radius { get; set; }
    }
}
