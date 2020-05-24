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
using GTLib.Primitives;
using GTLib.Scenes;

namespace Lab1Dvor
{
    public partial class MainForm : Form
    {
        private Bitmap bitmap;
        private Scene2D scene2d;
        private GTDrawerSlow drawerSlow;
        public MainForm()
        {
            InitializeComponent();
            bitmap = new Bitmap(this.Width, this.Height);
            scene2d = new Scene2D();
            drawerSlow = new GTDrawerSlow(scene2d,bitmap);

            scene2d.AddElement(new Dot2D(34,23));
            scene2d.AddElement(new Dot2D(344, 243));
            scene2d.AddElement(new Dot2D(14, 323));

            scene2d.AddElement(new Line2D(
                new Dot2D(213,12),
                new Dot2D(54,76) ));
        }

        private void MainForm_KeyDown(object sender, KeyEventArgs e)
        {
            Debug.WriteLine(e.KeyCode);
            switch(e.KeyCode)
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
            while (true)
            {
                UInt32 ms = drawerSlow.DrawWithMetric();
                Debug.WriteLine(ms);
                Invoke(new Action(() =>
                {
                    this.Text = "Lab1 Graphics " + ms + "мс";
                    this.BackgroundImage = drawerSlow.bitmap;
                    this.bitmap = new Bitmap(this.Width, this.Height);
                    this.drawerSlow.bitmap = this.bitmap;
                }));
                
                Thread.Sleep(500);
                
            }
        }
    }
}
