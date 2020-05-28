using System;
using System.Collections.Generic;
using System.Text;
using GTLib.Primitives;

namespace GTLib.Elements
{
    public class Quadrangle3D : Element3D
    {
        private Dot3D _a;

        private Dot3D _b;

        private Dot3D _c;

        private Dot3D _d;

        public Quadrangle3D(Dot3D a, Dot3D b, Dot3D c, Dot3D d)
        {
            Primitives = new List<Primitive3D>
            {
                new Line3D(a, b),
                new Line3D(b, c),
                new Line3D(c, d),
                new Line3D(d, a)
            };
            DeclarativePrimitives = new List<Primitive3D>
            {
                a, b, c, d
            };
            this.a = a;
            this.b = b;
            this.c = c;
            this.d = d;
        }

        public Dot3D a
        {
            get => _a;
            set
            {
                _a = value;
                ((Line3D)Primitives[0]).start = _a;
                ((Line3D)Primitives[3]).finish = _a;
                DeclarativePrimitives[0] = _a;
            }
        }
        public Dot3D b
        {
            get => _b;
            set
            {
                _b = value;
                ((Line3D)Primitives[0]).finish = _b;
                ((Line3D)Primitives[1]).start = _b;
                DeclarativePrimitives[1] = _b;
            }
        }
        public Dot3D c
        {
            get => _c;
            set
            {
                _c = value;
                ((Line3D)Primitives[1]).finish = _c;
                ((Line3D)Primitives[2]).start = _c;
                DeclarativePrimitives[2] = _c;
            }
        }
        public Dot3D d
        {
            get => _d;
            set
            {
                _d = value;
                ((Line3D)Primitives[2]).finish = _c;
                ((Line3D)Primitives[3]).start = _c;
                DeclarativePrimitives[3] = _c;
            }
        }
    }
}
