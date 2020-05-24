using GTLib.Primitives;
using System;
using System.Collections.Generic;
using System.Text;

namespace GTLib.Elements
{
    class Triangle2D:Element2D
    {
        public Dot2D a { get; set; }
        public Dot2D b { get; set; }
        public Dot2D c { get; set; }

        public Triangle2D(Dot2D a,Dot2D b,Dot2D c)
        {
            this.Primitives = new List<Primitive2D>();
            this.Primitives.Add(new Line2D(a,b));
            this.Primitives.Add(new Line2D(b,c));
            this.Primitives.Add(new Line2D(c,a));
            this.a = a;
            this.b = b;
            this.c = c;
        }
    }
}
