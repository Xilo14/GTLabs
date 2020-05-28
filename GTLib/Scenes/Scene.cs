using GTLib.Interfaces;
using GTLib.Primitives;
using System.Collections.Generic;

namespace GTLib.Scenes
{
    public abstract class Scene : IGTHavingPrimitives
    {
        public abstract List<Primitive> GetElements();
    }
}