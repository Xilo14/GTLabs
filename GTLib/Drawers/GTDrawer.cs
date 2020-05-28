using System.Diagnostics;
using GTLib.Interfaces;
using GTLib.Scenes;

namespace GTLib.Drawers
{
    public abstract class GTDrawer : IGTDrawing
    {
        public Scene2D Scene2D { get; set; }

        public virtual void Draw()
        {
        }

        public virtual uint DrawWithMetric()
        {
            var stopWatch = new Stopwatch();
            stopWatch.Start();

            Draw();

            stopWatch.Stop();
            //return (UInt32)stopWatch.ElapsedTicks;
            var seconds = stopWatch.ElapsedTicks / (double) Stopwatch.Frequency;
            var nanoseconds = seconds * 1000000000;
            return (uint) nanoseconds;
        }
    }
}