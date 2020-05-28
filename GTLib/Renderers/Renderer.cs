using System.Diagnostics;
using GTLib.Interfaces;
using GTLib.Scenes;

namespace GTLib.Renderers
{
    public abstract class Renderer:IGTRender
    {
        public Scene3D Scene3D { get; set; }

        public virtual void Render()
        {
        }
        public virtual uint RenderWithMetric()
        {
            var stopWatch = new Stopwatch();
            stopWatch.Start();

            Render();

            stopWatch.Stop();
            //return (UInt32)stopWatch.ElapsedTicks;
            var seconds = stopWatch.ElapsedTicks / (double)Stopwatch.Frequency;
            var nanoseconds = seconds * 1000000000;
            return (uint)nanoseconds;
        }

    }
}