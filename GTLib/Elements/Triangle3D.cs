using System.Collections.Generic;
using GTLib.Primitives;

namespace GTLib.Elements
{
    public class Triangle3D : Element3D
    {

        private Dot3D _a;

        private Dot3D _b;

        private Dot3D _c;

        public Triangle3D(Dot3D a, Dot3D b, Dot3D c)
        {
            Primitives = new List<Primitive3D>
                {
                    new Line3D(a, b),
                    new Line3D(b, c),
                    new Line3D(c, a)
                };
            DeclarativePrimitives = new List<Primitive3D>
                {
                    a, b, c
                };
            this.a = a;
            this.b = b;
            this.c = c;
        }

        public Dot3D a
        {
            get => _a;
            set
            {
                _a = value;
                ((Line3D)Primitives[0]).start = _a;
                ((Line3D)Primitives[2]).finish = _a;
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

    }
}