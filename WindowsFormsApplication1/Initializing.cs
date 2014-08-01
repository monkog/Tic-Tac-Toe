using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

namespace WindowsFormsApplication1
{
    public partial class Initializing : Form
    {
        private int tick = 0;
        Matrix rotateMatrix;
        GraphicsPath right, upper, down, small, small2, gp;
        Region yin;

        public Initializing()
        {
            InitializeComponent();
            timer.Start();
            gp = new GraphicsPath();
            gp.AddEllipse(0, 0, 256, 256);
            BackColor = Color.Red;

            right = new GraphicsPath();
            upper = new GraphicsPath();
            small = new GraphicsPath();
            down = new GraphicsPath();
            small2 = new GraphicsPath();

            right.AddArc(3, 3, 250, 250, 90, -180);
            upper.AddEllipse(60, 3, 125, 125);
            down.AddEllipse(60, 128, 125, 125);
            small.AddEllipse(115, 53, 25, 25);
            small2.AddEllipse(115, 175, 25, 25);

            Region = new Region(gp);
            Region.Exclude(right);
            Region.Union(upper);
            Region.Exclude(down);
            Region.Exclude(small);
            Region.Union(small2);

            MainMenuStrip = null;
            ShowInTaskbar = false;
            FormBorderStyle = FormBorderStyle.None;
            Show();
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            if (tick >= 200)
                Close();

            tick++;

            rotateMatrix = new Matrix();
            rotateMatrix.RotateAt(1, new PointF(128,128));
            right.Transform(rotateMatrix);
            upper.Transform(rotateMatrix);
            down.Transform(rotateMatrix);
            small2.Transform(rotateMatrix);
            small.Transform(rotateMatrix);

            yin = new Region(gp);
            yin.Exclude(right);
            yin.Union(upper);
            yin.Exclude(down);
            yin.Exclude(small);
            yin.Union(small2);
            Region = yin;
        }
    }
}
