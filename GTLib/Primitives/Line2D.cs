using System;
using System.Collections.Generic;
using System.Text;
using GTLib.Interfaces;

namespace GTLib.Primitives
{
    class Line2D:IGTDrawable
    {
        Dot2D start { get; set; }
        Dot2D finish { get; set; }
    }
}
