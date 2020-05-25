using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using GTLib.Drawers;
using GTLib.Elements;
using GTLib.Primitives;
using GTLib.Scenes;

namespace Lab1Dvor
{
    public partial class MainForm : Form
    {
        private Bitmap bitmap;
        private Scene2D scene2d;
        private GTDrawerSlow drawerSlow;
        private Triangle2D DynamicTriangle;
        private long _lastTick = System.Environment.TickCount64;
        private Random rnd = new Random(14);
        public MainForm()
        {
            InitializeComponent();
            bitmap = new Bitmap(this.Width, this.Height);
            scene2d = new Scene2D();
            drawerSlow = new GTDrawerSlow(scene2d, bitmap)
            { CurrentAlgForLine = GTDrawerSlow.AlgsForLine.Luke };

            scene2d.AddElement(new Dot2D(34, 23));
            scene2d.AddElement(new Dot2D(344, 243));
            scene2d.AddElement(new Dot2D(14, 323));

            scene2d.AddElement(new Line2D(
                new Dot2D(213, 12),
                new Dot2D(54, 76)));

            scene2d.AddElement(DynamicTriangle = new Triangle2D(
                new Dot2D(13, 65),
                new Dot2D(125, 67),
                new Dot2D(87, 213)));
            for (int i = 0; i < 100; i++)
                scene2d.AddElement(new Triangle2D(
                    new Dot2D(rnd.Next(0, this.Width), rnd.Next(0, this.Height)),
                    new Dot2D(rnd.Next(0, this.Width), rnd.Next(0, this.Height)),
                    new Dot2D(rnd.Next(0, this.Width), rnd.Next(0, this.Height))));



        }

        private void MainForm_KeyDown(object sender, KeyEventArgs e)
        {
            Debug.WriteLine(e.KeyCode);
            switch (e.KeyCode)
            {
                case Keys.Up:
                    break;

                case Keys.Down:
                    break;

                case Keys.Left:
                    break;

                case Keys.Right:
                    break;

                case Keys.NumPad4:
                    break;

                case Keys.NumPad6:
                    break;

                case Keys.PageUp:
                    break;

                case Keys.Next:
                    break;

                case Keys.Add:
                    break;

                case Keys.Subtract:
                    break;
            }
        }

        private void MainForm_Load(object sender, EventArgs e)
        {

        }

        private void MainForm_Shown(object sender, EventArgs e)
        {
            var thread = new Thread(Cycle);
            thread.Start();

        }

        private void Cycle()
        {
            DateTime _lastUpdate = DateTime.Now;
            while (true)
            {
                this.bitmap.Dispose();
                this.bitmap = new Bitmap(this.Width, this.Height);
                this.drawerSlow.bitmap = this.bitmap;

                UInt32 ns = drawerSlow.DrawWithMetric();
                Debug.WriteLine(ns);

                //Test modify triangle cord #uncomment
                DynamicTriangle.a.X = rnd.Next(100, 200);

                //

                //DynamicTriangle.a = new Dot2D(rnd.Next(100, 200), rnd.Next(100, 200));
                //DynamicTriangle.b = new Dot2D(rnd.Next(100, 200), rnd.Next(100, 200));
                //DynamicTriangle.c = new Dot2D(rnd.Next(100, 200), rnd.Next(100, 200));

                Invoke(new Action(() =>
                {
                    lock (this)
                    {
                        var _currentTicks = System.Environment.TickCount64;

                        this.BackgroundImage = drawerSlow.bitmap;

                        if ((DateTime.Now - _lastUpdate).Seconds > 0)
                        {
                            this.Text = "Lab1 Graphics " + ns + "нс"
                                        + "  Potential FPS:" + 1000000000 / ns
                                        + "  FPS:" + (int)(1000 / (double)(_currentTicks - _lastTick));
                            _lastUpdate = DateTime.Now;
                        }


                        _lastTick = _currentTicks;
                    }
                }));

                Thread.Sleep(1000 / 60);

            }
        }
    }
}
