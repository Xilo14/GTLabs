using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using GTLib.Elements;
using GTLib.Primitives;

namespace GTLib.Scenes
{
    public class Scene3D
    {
        public Model model;


        public Scene2D Render()
        {
            var scene2d = new Scene2D();
            
            foreach (var face in model.f)
            {
                scene2d.AddElement(new Triangle2D(
                    new Dot2D(model.v[(int)(face[0].X)-1].X, model.v[(int)(face[0].X)-1].Y), 
                    new Dot2D(model.v[(int)(face[1].X)-1].X, model.v[(int)(face[1].X)-1].Y), 
                    new Dot2D(model.v[(int)(face[2].X)-1].X, model.v[(int)(face[2].X)-1].Y)));

            }

            return scene2d;
        }
    }
}
