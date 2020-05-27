using System.Collections.Generic;
using GTLib.Primitives;

namespace GTLib.Elements
{
    public abstract class Element2D : Primitive2D
    {
        public List<Primitive2D> Primitives { get; protected set; }
        public List<Primitive2D> DeclarativePrimitives { get; protected set; }
    }
}