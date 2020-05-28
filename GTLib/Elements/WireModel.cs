using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.Intrinsics;
using System.Text;
using GTLib.Primitives;

namespace GTLib.Elements
{
    public class WireModel : Element3D
    {
        private Boolean isNormalizedPrimitives;
        private Boolean isNormalizedDeclarativePrimitives;

        private new List<Line3D> _primitives = new List<Line3D>();
        public new List<Line3D> Primitives
        {
            get
            {
                if(!isNormalizedPrimitives)
                    this.NormalizePrimitives();
                return _primitives;
            }
            
        } 
        private new List<Dot3D> _declarativePrimitives = new List<Dot3D>();

        public new List<Dot3D> DeclarativePrimitives
        {
            get
            {
                if (!isNormalizedDeclarativePrimitives)
                    this.NormalizeDeclarativePrimitives();
                return _declarativePrimitives;

            }
        }

        public void AddWire(Line3D wire)
        {
            //if (Primitives.Contains(wire)) return;

            _primitives.Add(wire);

            //if (!DeclarativePrimitives.Contains(wire.start))
            _declarativePrimitives.Add(wire.start);

            //if (!DeclarativePrimitives.Contains(wire.finish))
            _declarativePrimitives.Add(wire.finish);

            isNormalizedPrimitives = false;
            isNormalizedDeclarativePrimitives = false;
        }

        public void Normalize()
        {
            NormalizePrimitives();
            NormalizeDeclarativePrimitives();
        }
        public void NormalizePrimitives()
        {
            _primitives = new List<Line3D>(_primitives.Distinct());
            isNormalizedPrimitives = true;
        }
        public void NormalizeDeclarativePrimitives()
        {
            _declarativePrimitives = new List<Dot3D>(_declarativePrimitives.Distinct());
            isNormalizedDeclarativePrimitives = true;
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
