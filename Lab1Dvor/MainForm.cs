using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Lab1Dvor
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
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

                case Keys.Add:
                    break;

                //case Keys.Down:
                //    break;

                //case Keys.Down:
                //    break;
            }
        }
    }
}
