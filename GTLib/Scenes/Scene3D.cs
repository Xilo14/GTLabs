using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using GTLib.Cameras;
using GTLib.Elements;
using GTLib.Interfaces;
using GTLib.Primitives;

namespace GTLib.Scenes
{
    public abstract class Scene3D : Scene, IGTHavingPrimitives3D
    {
        public abstract List<Primitive3D> Get3DElements();
        public Camera Camera { get; set; }

        public Scene3D(Camera camera)
        {
            Camera = camera;
        }
        public Scene3D() : this(new Camera()) { }


    }
}
