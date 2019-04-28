using System;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

namespace WindowsFormsApplication1
{
    public partial class Initializing : Form
    {
        private int _tick;
        private Matrix _rotateMatrix;
        private GraphicsPath _right, _upper, _down, _small, _small2, _gp;
        private Region _yin;

        public Initializing()
        {
            InitializeComponent();
            timer.Start();
            _gp = new GraphicsPath();
            _gp.AddEllipse(0, 0, 256, 256);
            BackColor = Color.Red;

            _right = new GraphicsPath();
            _upper = new GraphicsPath();
            _small = new GraphicsPath();
            _down = new GraphicsPath();
            _small2 = new GraphicsPath();

            _right.AddArc(3, 3, 250, 250, 90, -180);
            _upper.AddEllipse(60, 3, 125, 125);
            _down.AddEllipse(60, 128, 125, 125);
            _small.AddEllipse(115, 53, 25, 25);
            _small2.AddEllipse(115, 175, 25, 25);

            Region = new Region(_gp);
            Region.Exclude(_right);
            Region.Union(_upper);
            Region.Exclude(_down);
            Region.Exclude(_small);
            Region.Union(_small2);

            MainMenuStrip = null;
            ShowInTaskbar = false;
            FormBorderStyle = FormBorderStyle.None;
            Show();
        }

		private void TimerTick(object sender, EventArgs e)
        {
            if (_tick >= 200)
                Close();

            _tick++;

            _rotateMatrix = new Matrix();
            _rotateMatrix.RotateAt(1, new PointF(128,128));
            _right.Transform(_rotateMatrix);
            _upper.Transform(_rotateMatrix);
            _down.Transform(_rotateMatrix);
            _small2.Transform(_rotateMatrix);
            _small.Transform(_rotateMatrix);

            _yin = new Region(_gp);
            _yin.Exclude(_right);
            _yin.Union(_upper);
            _yin.Exclude(_down);
            _yin.Exclude(_small);
            _yin.Union(_small2);
            Region = _yin;
        }
    }
}
