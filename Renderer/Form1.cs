using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GTLib;
using GTLib.Drawers;
using GTLib.Primitives;
using GTLib.Scenes;
using GTLib.Tranformers;

namespace Renderer
{
    public partial class Form1 : Form
    {
        private Scene3D _scene3d;
        private Scene2D _scene2d;
        private Bitmap _bitmap;
        private GTDrawerSlow _drawerSlow;

        public Form1()
        {
            InitializeComponent();

            _bitmap = new Bitmap(this.Width, this.Height);
            _scene3d = new Scene3D();

        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();


            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {

                WaveFrontParser wfp = new WaveFrontParser();
                _scene3d.model = wfp.Parse(openFileDialog.FileName);

                var stopWatch = new Stopwatch();
                stopWatch.Start();

                _scene2d = _scene3d.Render();

                stopWatch.Stop();
                //return (UInt32)stopWatch.ElapsedTicks;
                var seconds = stopWatch.ElapsedTicks / (double)Stopwatch.Frequency;
                var nanoseconds = seconds * 1000000000;

                _drawerSlow = new GTDrawerSlow(_scene2d, _bitmap)
                { CurrentAlgForLine = GTDrawerSlow.AlgsForLine.Luke };
                var trans = new TransformerScale();
                trans.ScaleIndex = 260;
                trans.Transform(_scene2d);
                var transMove = new TransformerMove();
                transMove.MoveX = this.Width / 2;
                transMove.MoveY = this.Height / 2;
                transMove.Transform(_scene2d);

                var transRotate = new TransformerRotate();
                transRotate.Degree = 180;
                transRotate.Center = new Dot2D(
                    this.Width / 2,
                    this.Height / 2);
                transRotate.Transform(_scene2d);

                
                var ns = _drawerSlow.DrawWithMetric();

                this.Text = (1000000000/nanoseconds).ToString() + "      " + 1000000000 / ns;

                this.BackgroundImage = _drawerSlow.Bitmap;
            }
        }
    }
}
