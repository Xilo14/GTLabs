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
using GTLib.Tranformers;

namespace Lab1Dvor
{
    public partial class MainForm : Form
    {
        private Bitmap _bitmap;
        private Scene2D _scene2d;
        private GTDrawerSlow _drawerSlow;

        //We save these figures for further manipulation 
        private Triangle2D _dynamicTriangle;
        private Triangle2D _dynamicTriangle2;
        private Triangle2D _dynamicTriangle3;

        private Line2D _dynamicLine;
        private Line2D _dynamicLine2;

        private SimpleText2D _text2D;

        //Transformers
        private TransformerRotate _transRotate;
        private TransformerMove _transMove;
        private TransformerScale _transScale;

        //For count fps in WinForms
        private long _lastTick = System.Environment.TickCount64;

        private Random _rnd = new Random(14);

        public MainForm()
        {
            InitializeComponent();

            _bitmap = new Bitmap(this.Width, this.Height);
            _scene2d = new Scene2D();
            _drawerSlow = new GTDrawerSlow(_scene2d, _bitmap)
            { CurrentAlgForLine = GTDrawerSlow.AlgsForLine.Luke };


            //Adding different elements to scene
            //Dots
            _scene2d.AddElement(new Dot2D(34, 23));
            _scene2d.AddElement(new Dot2D(344, 243));
            _scene2d.AddElement(new Dot2D(14, 323));

            //Lines
            _scene2d.AddElement(_dynamicLine = new Line2D(new Dot2D(187, 213), new Dot2D(227, 213)));
            _scene2d.AddElement(_dynamicLine2 = new Line2D(new Dot2D(187, 213), new Dot2D(267, 213)));
            _scene2d.AddElement(new Line2D(
                new Dot2D(213, 12),
                new Dot2D(54, 76)));

            //Triangles
            _scene2d.AddElement(_dynamicTriangle = new Triangle2D(
                new Dot2D(137, 65),
                new Dot2D(215, 67),
                new Dot2D(187, 213)));
            _scene2d.AddElement(_dynamicTriangle2 = new Triangle2D(
                new Dot2D(137, 65),
                new Dot2D(215, 67),
                new Dot2D(187, 213)));
            _scene2d.AddElement(_dynamicTriangle3 = new Triangle2D(
                new Dot2D(37, 35),
                new Dot2D(215, 167),
                new Dot2D(287, 213)));
            for (int i = 0; i < 5; i++)
                _scene2d.AddElement(new Triangle2D(
                    new Dot2D(_rnd.Next(0, this.Width), _rnd.Next(0, this.Height)),
                    new Dot2D(_rnd.Next(0, this.Width), _rnd.Next(0, this.Height)),
                    new Dot2D(_rnd.Next(0, this.Width), _rnd.Next(0, this.Height))));

            //Circles
            _scene2d.AddElement(new Circle2D(
                new Dot2D(_rnd.Next(200, this.Width - 200), _rnd.Next(200, this.Height - 200)),
                    _rnd.Next(20, 200)));

            //Texts
            _scene2d.AddElement(_text2D = new SimpleText2D("ЮАД")
            {
                ScaleIndex = 20,
                X = 187,
                Y = 213
            });

            



            //Initialize transformers
            _transRotate = new TransformerRotate(new Dot2D(_bitmap.Width/2,_bitmap.Height/2));
            _transMove = new TransformerMove();
            _transScale = new TransformerScale();
        }

        private void MainForm_KeyDown(object sender, KeyEventArgs e)
        {
            Debug.WriteLine(e.KeyCode);
            switch (e.KeyCode)
            {
                case Keys.Up:
                    _transMove.MoveX = 0;
                    _transMove.MoveY = -5;
                    _transMove.Transform(_scene2d);
                    break;

                case Keys.Down:
                    _transMove.MoveX = 0;
                    _transMove.MoveY = 5;
                    _transMove.Transform(_scene2d);
                    break;

                case Keys.Left:
                    _transMove.MoveX = -5;
                    _transMove.MoveY = 0;
                    _transMove.Transform(_scene2d);
                    break;

                case Keys.Right:
                    _transMove.MoveX = 5;
                    _transMove.MoveY = 0;
                    _transMove.Transform(_scene2d);
                    break;

                case Keys.NumPad4:
                    _transRotate.Degree = -1;
                    _transRotate.Transform(_scene2d);
                    break;

                case Keys.NumPad6:
                    _transRotate.Degree = 1;
                    _transRotate.Transform(_scene2d);
                    break;

                case Keys.PageUp:
                    _transScale.ScaleIndex = 1.1;
                    _transScale.Transform(_scene2d);
                    break;

                case Keys.Next://PageDown
                    _transScale.ScaleIndex = 0.909;
                    _transScale.Transform(_scene2d);
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
            DateTime lastUpdate = DateTime.Now;
            while (true)
            {
                UInt32 ns = _drawerSlow.DrawWithMetric();
                Debug.WriteLine(ns);

                //Test modify triangle cord #uncomment
                //DynamicTriangle.a.X = rnd.Next(100, 200);


                Invoke(new Action(() =>
                {
                    lock (this)
                    {
                        var currentTicks = System.Environment.TickCount64;

                        this.BackgroundImage = null;
                        this.BackgroundImage = _drawerSlow.Bitmap;

                        if ((DateTime.Now - lastUpdate).Seconds > 0)
                        {
                            this.Text = "Lab1 Graphics " +
                                        "Elapset time: "+ ns + "нс"
                                        + "  Potential FPS:" + 1000000000 / ns
                                        + "  FPS:" + (int)(1000 / (double)(currentTicks - _lastTick));
                            lastUpdate = DateTime.Now;
                        }
                        _lastTick = currentTicks;
                    }
                }));
                //We set 60 fps, but often frames falling to 20-30, Why?
                //Because GC want to clear our bytes array. If we use
                //Array.Clear this program eat all memory. Need fix it.
                Thread.Sleep(1000 / 60);

            }
        }
    }
}
