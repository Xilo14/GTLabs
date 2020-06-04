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

namespace Demo_FilledTriangle
{
    public partial class MainForm : Form
    {
        private Scene3DSimple _scene3d;
        private Scene2D _scene2d;
        private Bitmap _bitmap;
        private GTDrawerSlow _drawerSlow;
        private RendererTriangleModels _renderer;

        TransformerScale transScale = new TransformerScale();

        private Point moveStart;
        private long _lastTick;

        public MainForm()
        {
            InitializeComponent();
            DoubleBuffered = true;
            this.Width = 800;
            this.Height = 800;
            _bitmap = new Bitmap(this.Width, this.Height);

            _scene2d = new Scene2D();
            _scene3d = new Scene3DSimple();
            _scene3d.Camera = new PerspectiveCamera(
                new Vector3(3, 0, 0),
                new Vector3(2, 0, 0),
                new Vector3(3, 1, 0));

            _drawerSlow = new GTDrawerSlow(_scene2d, _bitmap);
            _renderer = new RendererTriangleModels(_scene3d, _scene2d);


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
        private void MainForm_KeyDown(object sender, KeyEventArgs e)
        {
            Debug.WriteLine(e.KeyCode);//для проверки клавиш
            switch (e.KeyCode)
            {
                case Keys.W:
                    _scene3d.Camera.MoveAheadBack((float)0.1);
                    break;

                case Keys.A:
                    _scene3d.Camera.MoveLeftRight((float)0.1);
                    break;

                case Keys.S:
                    _scene3d.Camera.MoveAheadBack((float)-0.1);
                    break;

                case Keys.D:
                    _scene3d.Camera.MoveLeftRight((float)-0.1);
                    break;

                case Keys.ShiftKey:
                    _scene3d.Camera.MoveUpDown((float)0.1);
                    break;

                case Keys.ControlKey:
                    _scene3d.Camera.MoveUpDown((float)-0.1);
                    break;
            }
        }

        private void MainForm_Shown(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();


            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {

                WaveFrontParser wfp = new WaveFrontParser();
                var objModel = wfp.Parse(openFileDialog.FileName);


                GTLib.FileParsers.WaveFront.Converter.DrawNormal = true;

                _scene3d.AddElement(GTLib.FileParsers.WaveFront
                    .Converter.ObjFilledToTriangleModel(objModel));

                uint nsrend = _renderer.RenderWithMetric();
                _drawerSlow.Scene2D = _renderer.Scene2D;




                uint nsdraw = _drawerSlow.DrawWithMetric();



                this.Text = (1000000000 / nsrend).ToString() + "      " + 1000000000 / nsdraw;

                this.BackgroundImage = _drawerSlow.Bitmap;
                //this.MouseWheel += this.MouseWheelHandler;
                var thread = new Thread(Cycle);
                thread.Start();
            }

        }

        private void MainForm_MouseDown(object sender, MouseEventArgs e)
        {

            if (e.Button == MouseButtons.Left)
            {
                moveStart = new Point(e.X, e.Y);
            }
        }

        private void MainForm_MouseMove(object sender, MouseEventArgs e)
        {

            if ((e.Button & MouseButtons.Left) != 0)
            {
                Point deltaPos = new Point(e.X - moveStart.X, e.Y - moveStart.Y);
                _scene3d.Camera.RotateCamera(deltaPos.X, -deltaPos.Y);
                //_scene3d.Camera.Eye = RotateDot3D(_scene3d.Camera.Eye, deltaPos.X, deltaPos.Y);
                //_scene3d.Camera.Up = RotateDot3D(_scene3d.Camera.Up, deltaPos.X, deltaPos.Y);
                moveStart = new Point(e.X, e.Y);

            }
        }
        private Vector3 RotateDot3D(Vector3 dot3d, double degx, double degy)
        {
            var rady = (degy / 100) * (Math.PI / 180);
            var radx = (degx / 100) * (Math.PI / 180);

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
            return vector3;
        }

        private void Cycle()
        {
            DateTime lastUpdate = DateTime.Now;
            while (true)
            {
                _renderer.Scene2D.Get2DElements().Clear();
                var nsr = _renderer.RenderWithMetric();
                _drawerSlow.Scene2D = _renderer.Scene2D;
                _scene2d = _renderer.Scene2D;



                //transScale.Transform(_scene2d);

                //var transMove = new TransformerMove();
                //transMove.MoveX = this.Width / 2;
                //transMove.MoveY = this.Height / 2;
                //transMove.Transform(_scene2d);

                UInt32 nsd = _drawerSlow.DrawWithMetric();



                float l = 0;

                Debug.WriteLine(nsr + " - " +
                                nsd + " : " +
                                _scene2d.Get2DElements().Count +
                                "   " + l);
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
                                        "Elapsed render time: " + nsr + "нс"
                                        + "  Potential FPS:" + 1000000000 / nsr +
                                        "  Elapsed draw time: " + nsd + "нс"
                                        + "  Potential FPS:" + 1000000000 / nsd
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
