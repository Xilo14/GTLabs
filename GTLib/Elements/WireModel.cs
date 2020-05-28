using System;
using System.Collections.Generic;
using System.Numerics;
using System.Runtime.Intrinsics;
using System.Text;
using GTLib.Primitives;

namespace GTLib.Elements
{
    public class WireModel : Element3D
    {
        public new List<Line3D> Primitives { get; protected set; } = new List<Line3D>();
        public new List<Dot3D> DeclarativePrimitives { get; protected set; } = new List<Dot3D>();

        public void AddWire(Line3D wire)
        {
            //if (Primitives.Contains(wire)) return;

            Primitives.Add(wire);

            //if (!DeclarativePrimitives.Contains(wire.start))
                DeclarativePrimitives.Add(wire.start);

            //if (!DeclarativePrimitives.Contains(wire.finish))
                DeclarativePrimitives.Add(wire.finish);
        }

        public WireModel(Dot3D position) : base(position)
        {
            AddWire(new Line3D(
                new Dot3D(0,3,0),
                new Dot3D(0, 0, 0)));
            AddWire(new Line3D(
                new Dot3D(0, 0, 3),
                new Dot3D(0, 0, 0)));
            AddWire(new Line3D(
                new Dot3D(3, 0, 0),
                new Dot3D(0, 0, 0)));
        }
        public WireModel() : this(new Dot3D(0,0,0)) { }
    }
}
