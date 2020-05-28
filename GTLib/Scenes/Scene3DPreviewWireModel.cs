using System;
using System.Collections.Generic;
using GTLib.Cameras;
using GTLib.Elements;
using GTLib.Interfaces;
using GTLib.Primitives;

namespace GTLib.Scenes
{
    public class Scene3DPreviewWireModel : Scene3D
    {
        public WireModel Model { get; set; }
        public Scene3DPreviewWireModel(WireModel model,Camera camera):base(camera)
        {
            Model = model;
        }
        public Scene3DPreviewWireModel(WireModel model):base() { }
        public Scene3DPreviewWireModel() : this(new WireModel()) { }


        public override List<Primitive> GetElements()
        {
            throw new NotImplementedException();
        }
        public override List<Primitive3D> Get3DElements()
        {
            return new List<Primitive3D>() { Model };
        }
    }
}