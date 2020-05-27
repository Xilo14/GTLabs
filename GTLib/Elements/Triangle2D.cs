using System.Collections.Generic;
using GTLib.Primitives;

namespace GTLib.Elements
{
    public class Triangle2D : Element2D
    {
        private Dot2D _a;

        private Dot2D _b;

        private Dot2D _c;

        public Triangle2D(Dot2D a, Dot2D b, Dot2D c)
        {
            Primitives = new List<Primitive2D>
            {
                new Line2D(a, b),
                new Line2D(b, c),
                new Line2D(c, a)
            };
            DeclarativePrimitives = new List<Primitive2D>
            {
                a, b, c
            };
            this.a = a;
            this.b = b;
            this.c = c;
        }

        public Dot2D a
        {
            get => _a;
            set
            {
                _a = value;
                ((Line2D) Primitives[0]).start = _a;
                ((Line2D) Primitives[2]).finish = _a;
                DeclarativePrimitives[0] = _a;
            }
        }

        public Dot2D b
        {
            get => _b;
            set
            {
                _b = value;
                ((Line2D) Primitives[0]).finish = _b;
                ((Line2D) Primitives[1]).start = _b;
                DeclarativePrimitives[1] = _b;
            }
        }

        public Dot2D c
        {
            get => _c;
            set
            {
                _c = value;
                ((Line2D) Primitives[1]).finish = _c;
                ((Line2D) Primitives[2]).start = _c;
                DeclarativePrimitives[2] = _c;
            }
        }
    }
}