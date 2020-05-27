using System;
using System.Collections.Generic;
using System.Text;

namespace GTLib.Primitives
{
    public class Line3D:Primitive3D
    {
        public Line3D(Dot3D start, Dot3D finish)
        {
            this.start = start;
            this.finish = finish;
        }

        public Line3D() : this(new Dot3D(), new Dot3D())
        {
        }

        public Dot3D start { get; set; }
        public Dot3D finish { get; set; }
    }
}
