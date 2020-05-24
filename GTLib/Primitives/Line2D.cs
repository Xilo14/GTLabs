﻿using System;
using System.Collections.Generic;
using System.Text;
using GTLib.Interfaces;

namespace GTLib.Primitives
{
    public class Line2D : Primitive2D
    {
        public Dot2D start { get; set; }
        public Dot2D finish { get; set; }

        public Line2D(Dot2D start, Dot2D finish)
        {
            this.start = start;
            this.finish = finish;
        }
        public Line2D() : this(new Dot2D(), new Dot2D()) { }

    }
}
