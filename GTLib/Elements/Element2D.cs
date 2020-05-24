using System;
using System.Collections.Generic;
using System.Text;
using GTLib.Primitives;

namespace GTLib.Elements
{
    public abstract class Element2D:Primitive2D
    {
        public List<Primitive2D> Primitives { get; protected set; }
    }
}
