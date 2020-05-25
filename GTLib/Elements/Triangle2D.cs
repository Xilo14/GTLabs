using GTLib.Primitives;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using System.Text;

namespace GTLib.Elements
{
    public class Triangle2D:Element2D
    {
        private Dot2D _a;

        public Dot2D a
        {
            get => _a;
            set
            {
                _a = value;
                ((Line2D)this.Primitives[0]).start = this._a;
                ((Line2D)this.Primitives[2]).finish = this._a;
            }

        }

        private Dot2D _b;
        public Dot2D b
        {
            get => _b;
            set
            {
                _b = value;
                ((Line2D)this.Primitives[0]).finish = this._b;
                ((Line2D)this.Primitives[1]).start = this._b;
            }

        }

        private Dot2D _c;
        public Dot2D c
        {
            get => _c;
            set
            {
                _c = value;
                ((Line2D)this.Primitives[1]).finish = this._c;
                ((Line2D)this.Primitives[2]).start = this._c;
            }

        }

        public Triangle2D(Dot2D a,Dot2D b,Dot2D c)
        {
            this.Primitives = new List<Primitive2D>
            {
                new Line2D(a, b),
                new Line2D(b, c),
                new Line2D(c, a)
            };
            this.a = a;
            this.b = b;
            this.c = c;
        }
    }
}
