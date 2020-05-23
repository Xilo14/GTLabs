using GTLib.Interfaces;
using GTLib.Scenes;
using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace GTLib.Drawers
{
    public class GTDrawer : IGTDrawing
    {
        public IGTHavingPrimitives2D scene { get; set; }
        public Bitmap bitmap { get; set; }

        public GTDrawer(IGTHavingPrimitives2D scene,Bitmap bitmap)
        {
            this.scene = scene;
            this.bitmap = bitmap;
        }
        public GTDrawer() : this(new Scene2D(),new Bitmap(EnvVar.STANDART_BMP_WIDTH, EnvVar.STANDART_BMP_HEIGHT)) { }
        public GTDrawer(IGTHavingPrimitives2D scene) : this(scene, new Bitmap(EnvVar.STANDART_BMP_WIDTH, EnvVar.STANDART_BMP_HEIGHT)) { }
        public GTDrawer(Bitmap bitmap) : this(new Scene2D(), bitmap) { }


    }
}
