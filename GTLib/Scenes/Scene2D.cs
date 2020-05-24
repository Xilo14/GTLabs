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
        public void AddElement(Primitive2D el)
        {
            _elements.Add(el);
        }
        public void AddElement(IEnumerable<Primitive2D> els)
        {
            _elements.AddRange(els);
        }
        public void DeleteElement(Primitive2D el)
        {
            _elements.Remove(el);
        }
        public void DeleteElement(IEnumerable<Primitive2D> els)
        {
            foreach(var el in els)
                _elements.Remove(el);
        }

    }
}
