using GTLib.Primitives;
using System;
using System.Collections.Generic;
using System.Text;

namespace GTLib.Elements
{
    public abstract class Element3D : Primitive3D
    {
        public List<Primitive3D> Primitives { get; protected set; }
        public List<Primitive3D> DeclarativePrimitives { get; protected set; }

        public Dot3D Position { get; set; }
        public Dot3D Curs { get; set; }
        public Element3D(Dot3D position)
        {
            Position = position;
        }
        public Element3D()
        {
            Position = new Dot3D(0,0,0);
        }
    }
}
