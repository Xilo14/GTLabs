using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using GTLib;
using GTLib.Cameras;
using GTLib.Drawers;
using GTLib.Primitives;
using GTLib.Scenes;
using GTLib.Tranformers;
using GTLib.Renderers;

namespace Renderer
{
    public partial class MainForm : Form
    {
        private Scene3DPreviewWireModel _scene3d;
        private Scene2D _scene2d;
        private Bitmap _bitmap;
        private GTDrawerSlow _drawerSlow;
        private RendererPreviewWireModel _renderer;

        TransformerScale transScale = new TransformerScale();

        private Point moveStart;
        private long _lastTick;

        public MainForm()
        {
            InitializeComponent();
            DoubleBuffered = true;


            _bitmap = new Bitmap(this.Width, this.Height);

            _scene2d = new Scene2D();
            _scene3d = new Scene3DPreviewWireModel();
            _scene3d.Camera = new OrthogonalCamera()
            {
                CursNormal = new Dot3D(0, 0, 0),
                Position = new Dot3D(3, 0, 0),
                Top = new Dot3D(3, 1, 0)
            };

            _drawerSlow = new GTDrawerSlow(_scene2d, _bitmap);
            _renderer = new RendererPreviewWireModel(_scene3d, _scene2d);
        }
        private void MouseWheelHandler(object sender, MouseEventArgs e)
        {
            
            if (e.Delta > 0)
            {

                transScale.ScaleIndex *= 1.1;

            }

            if (e.Delta < 0)
            {
                transScale.ScaleIndex *= 0.909;
            }
            
        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();


            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {

                WaveFrontParser wfp = new WaveFrontParser();
                var objModel = wfp.Parse(openFileDialog.FileName);

                _scene3d.Model = GTLib.FileParsers.WaveFront
                    .Converter.ObjModelToWireModel(objModel);

                uint nsrend = _renderer.RenderWithMetric();
                _drawerSlow.Scene2D = _renderer.Scene2D;


                //var transRotate = new TransformerRotate();
                //transRotate.Degree = 180;
                //transRotate.Center = new Dot2D(
                //    this.Width / 2,
                //    this.Height / 2);
                //transRotate.Transform(_scene2d);



                uint nsdraw = _drawerSlow.DrawWithMetric();



                this.Text = (1000000000 / nsrend).ToString() + "      " + 1000000000 / nsdraw;

                this.BackgroundImage = _drawerSlow.Bitmap;
                this.MouseWheel += this.MouseWheelHandler;
                var thread = new Thread(Cycle);
                thread.Start();
            }

        }



        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                moveStart = new Point(e.X, e.Y);
            }
        }

        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            if ((e.Button & MouseButtons.Left) != 0)
            {
                Point deltaPos = new Point(e.X - moveStart.X, e.Y - moveStart.Y);
                RotateDot3D(_scene3d.Camera.Position, deltaPos.X, deltaPos.Y);
                RotateDot3D(_scene3d.Camera.Top, deltaPos.X, deltaPos.Y);

                
            }
        }
        private void RotateDot3D(Dot3D dot3d, double degx, double degy)
        {
            var rady = (degy/100) * (Math.PI / 180);
            var radx = (degx/100) * (Math.PI / 180);

            Vector3 vector3 = new Vector3(
                (float)dot3d.X,
                (float)dot3d.Y,
                (float)dot3d.Z);

            Matrix4x4 matrixY = new Matrix4x4(
                (float)Math.Cos(radx), 0, (float)Math.Sin(radx), 0,
                0, 1, 0, 0,
                -(float)Math.Sin(radx), 0, (float)Math.Cos(radx), 0,
                0, 0, 0, 0);

            Matrix4x4 matrixX = new Matrix4x4(
                1, 0, 0, 0,
                0, (float)Math.Cos(rady), -(float)Math.Sin(rady), 0,
                0, (float)Math.Sin(rady), (float)Math.Cos(rady), 0,
                0, 0, 0, 0);

            //Matrix4x4 matrixZ = new Matrix4x4(
            //    (float)Math.Cos(radx), -(float)Math.Sin(radx), 0, 0,
            //    (float)Math.Sin(radx), (float)Math.Cos(radx), 0, 0,
            //    0, 0, 1, 0,
            //    0, 0, 0, 0);

            vector3 = Vector3.Transform(vector3, matrixY);
            vector3 = Vector3.Transform(vector3, matrixX);
            dot3d.X = vector3.X;
            dot3d.Y = vector3.Y;
            dot3d.Z = vector3.Z;
        }

        private void Cycle()
        {
            DateTime lastUpdate = DateTime.Now;
            while (true)
            {
                _renderer.Scene2D.Get2DElements().Clear();
                _renderer.Render();
                _drawerSlow.Scene2D = _renderer.Scene2D;
                _scene2d = _renderer.Scene2D;

                
                
                transScale.Transform(_scene2d);

                var transMove = new TransformerMove();
                transMove.MoveX = this.Width / 2;
                transMove.MoveY = this.Height / 2;
                transMove.Transform(_scene2d);

                UInt32 ns = _drawerSlow.DrawWithMetric();
                Debug.WriteLine(ns);

                Invoke(new Action(() =>
                {
                    lock (this)
                    {
                        
                        var currentTicks = System.Environment.TickCount64;

                        //this.BackgroundImage = null;
                        if (this.BackgroundImage != _drawerSlow.Bitmap)
                            this.BackgroundImage = _drawerSlow.Bitmap;
                        this.Refresh();
                        
                        if ((DateTime.Now - lastUpdate).Seconds > 0)
                        {
                            this.Text = "Lab1 Graphics " +
                                        "Elapset time: " + ns + "нс"
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
                //Thread.Sleep(1000 / 30);

            }
        }
    }
}
