using GTLib.Primitives;
using System;
using System.Collections.Generic;
using System.Text;
using GTLib.Light;

namespace GTLib.Scenes
{
    public class Scene3DSimple : Scene3D
    {
        public DirectLight DirectLight = new DirectLight();
        private List<Primitive3D> _elements;

        public Scene3DSimple()
        {
            _elements = new List<Primitive3D>();
        }
        public override List<Primitive3D> Get3DElements()
        {
            return _elements;
        }

        public override List<Primitive> GetElements()
        {
            throw new NotImplementedException();
        }
        public void AddElement(Primitive3D el)
        {
            _elements.Add(el);
        }
    }
}
