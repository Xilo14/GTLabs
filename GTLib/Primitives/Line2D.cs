using System;
using System.Collections.Generic;
using System.Text;
using GTLib.Interfaces;

namespace GTLib.Primitives
{
    class Line2D: Primitive2D
    {
        Dot2D start { get; set; }
        Dot2D finish { get; set; }
    }
}
