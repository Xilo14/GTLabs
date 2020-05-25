using GTLib.Interfaces;
using GTLib.Scenes;
using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Diagnostics;
using GTLib.Primitives;
using GTLib.Elements;

namespace GTLib.Drawers
{
    public abstract class GTDrawer : IGTDrawing
    {
        public IGTHavingPrimitives2D scene { get; set; }
        public Bitmap bitmap { get; set; }

        public GTDrawer(IGTHavingPrimitives2D scene, Bitmap bitmap)
        {
            this.scene = scene;
            this.bitmap = bitmap;
        }
        public GTDrawer() : this(new Scene2D(), new Bitmap(EnvVar.STANDART_BMP_WIDTH, EnvVar.STANDART_BMP_HEIGHT)) { }
        public GTDrawer(IGTHavingPrimitives2D scene) : this(scene, new Bitmap(EnvVar.STANDART_BMP_WIDTH, EnvVar.STANDART_BMP_HEIGHT)) { }
        public GTDrawer(Bitmap bitmap) : this(new Scene2D(), bitmap) { }

        public virtual void Draw()
        {
            
        }

        public virtual UInt32 DrawWithMetric()
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            this.Draw();

            stopWatch.Stop();
            //return (UInt32)stopWatch.ElapsedTicks;
            double seconds = (double)stopWatch.ElapsedTicks / (double)Stopwatch.Frequency;
            var nanoseconds = seconds * 1000000000;
            return (UInt32)nanoseconds;
        }




    }
    
}
