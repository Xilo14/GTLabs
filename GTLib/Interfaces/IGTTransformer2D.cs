using System;
using System.Collections.Generic;
using System.Text;
using GTLib.Primitives;

namespace GTLib.Interfaces
{
    interface IGTTransformer2D
    {
        public void Transform(Primitive2D primitive);
    }
}
