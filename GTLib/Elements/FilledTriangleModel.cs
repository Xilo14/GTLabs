using GTLib.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;

namespace GTLib.Elements
{
    public class FilledTriangleModel : Element3D
    {
        //что делать?
        private Boolean isNormalizedPrimitives;
        private Boolean isNormalizedDeclarativePrimitives;

        private List<Primitive3D> _primitives = new List<Primitive3D>();
        public new List<Primitive3D> Primitives
        {
            get
            {
                if (!isNormalizedPrimitives)
                    this.NormalizePrimitives();
                return _primitives;
            }

        }

        private List<Primitive3D> _declarativePrimitives = new List<Primitive3D>();
                public new List<Primitive3D> DeclarativePrimitives
        {
            get
            {
                if (!isNormalizedDeclarativePrimitives)
                    this.NormalizeDeclarativePrimitives();
                return _declarativePrimitives;

            }
        }

        public void Normalize()
        {
            NormalizePrimitives();
            NormalizeDeclarativePrimitives();
        }
        public void NormalizePrimitives()
        {
            _primitives = new List<Primitive3D>(_primitives.Distinct());
            isNormalizedPrimitives = true;
        }
        public void NormalizeDeclarativePrimitives()
        {
            _declarativePrimitives = new List<Primitive3D>(_declarativePrimitives.Distinct());
            isNormalizedDeclarativePrimitives = true;
        }

        public FilledTriangleModel(Vector3 position) : base(position) { }
        public FilledTriangleModel() : this(new Vector3(0, 0, 0)) { }

        public void AddTriangle(Triangle3D triangle)
        {
            Primitives.Add(triangle);
            DeclarativePrimitives.Add(triangle.a);
            DeclarativePrimitives.Add(triangle.b);
            DeclarativePrimitives.Add(triangle.c);
            isNormalizedPrimitives = false;
            isNormalizedDeclarativePrimitives = false;
        }
    }
}
