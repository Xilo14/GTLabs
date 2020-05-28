using System.Collections.Generic;
using GTLib.Primitives;

namespace GTLib.Interfaces
{
    public interface IGTHavingPrimitives3D:IGTHavingPrimitives
    {
        public List<Primitive3D> Get3DElements();
    }
}