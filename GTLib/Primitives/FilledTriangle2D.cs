using GTLib.Elements;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace GTLib.Primitives
{
    public class FilledTriangle2D : Primitive2D
    {
        public Color Color { get; set; }
        public Dot2D A { get; set; }
        public Dot2D B { get; set; }
        public Dot2D C { get; set; }


        public FilledTriangle2D(Dot2D a, Dot2D b, Dot2D c, Color color)
        {
            A = a;
            B = b;
            C = c;
            Color = color;
        }
    }
}
