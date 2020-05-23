using GTLib.Interfaces;
using GTLib.Primitives;
using System;
using System.Collections.Generic;
using System.Text;

namespace GTLib.Scenes
{
    public class Scene2D : Scene,IGTHavingPrimitives2D
    {
        public List<Primitive2D> _elements;

        public List<Primitive2D> GetElements()
        {
            return _elements;
        }
        public Scene2D()
        {
            _elements = new List<Primitive2D>();
        }
    }
}
