using GTLib.Primitives;
using System.Collections.Generic;

namespace GTLib.Elements
{
    public class Quadrangle2D : Element2D
    {
        private Dot2D _a;

        private Dot2D _b;

        private Dot2D _c;

        private Dot2D _d;

        public Quadrangle2D(Dot2D a, Dot2D b, Dot2D c, Dot2D d)
        {
            Primitives = new List<Primitive2D>
            {
                new Line2D(a, b),
                new Line2D(b, c),
                new Line2D(c, d),
                new Line2D(d, a)
            };
            DeclarativePrimitives = new List<Primitive2D>
            {
                a, b, c, d
            };
            this.a = a;
            this.b = b;
            this.c = c;
            this.d = d;
        }

        public Dot2D a
        {
            get => _a;
            set
            {
                _a = value;
                ((Line2D)Primitives[0]).start = _a;
                ((Line2D)Primitives[3]).finish = _a;
                DeclarativePrimitives[0] = _a;
            }
        }
        public Dot2D b
        {
            get => _b;
            set
            {
                _b = value;
                ((Line2D)Primitives[0]).finish = _b;
                ((Line2D)Primitives[1]).start = _b;
                DeclarativePrimitives[1] = _b;
            }
        }
        public Dot2D c
        {
            get => _c;
            set
            {
                _c = value;
                ((Line2D)Primitives[1]).finish = _c;
                ((Line2D)Primitives[2]).start = _c;
                DeclarativePrimitives[2] = _c;
            }
        }
        public Dot2D d
        {
            get => _d;
            set
            {
                _d = value;
                ((Line2D)Primitives[2]).finish = _c;
                ((Line2D)Primitives[3]).start = _c;
                DeclarativePrimitives[3] = _c;
            }
        }
    }
}