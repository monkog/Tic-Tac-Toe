using System;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

namespace TicTacToe
{
	public partial class YinYangAnimation : Form
	{
		private int _tick;
		private readonly Matrix _rotateMatrix;
		private GraphicsPath _excludedTail, _includedCircle, _excludedCircle, _smallExcluded, _smallIncluded, _gp;

		public YinYangAnimation()
		{
			InitializeComponent();

			timer.Start();
			InitializeYinYangSegments();
			CreateYinYangPath();

			_rotateMatrix = new Matrix();
			_rotateMatrix.RotateAt(1, new PointF(128, 128));

			Show();
		}

		private void InitializeYinYangSegments()
		{
			_gp = new GraphicsPath();
			_gp.AddEllipse(0, 0, 256, 256);

			_excludedTail = new GraphicsPath();
			_includedCircle = new GraphicsPath();
			_smallExcluded = new GraphicsPath();
			_excludedCircle = new GraphicsPath();
			_smallIncluded = new GraphicsPath();

			_excludedTail.AddArc(3, 3, 250, 250, 90, -180);
			_includedCircle.AddEllipse(60, 3, 125, 125);
			_excludedCircle.AddEllipse(60, 128, 125, 125);
			_smallExcluded.AddEllipse(115, 53, 25, 25);
			_smallIncluded.AddEllipse(115, 175, 25, 25);
		}

		private void CreateYinYangPath()
		{
			var yin = new Region(_gp);
			yin.Exclude(_excludedTail);
			yin.Union(_includedCircle);
			yin.Exclude(_excludedCircle);
			yin.Exclude(_smallExcluded);
			yin.Union(_smallIncluded);
			Region = yin;
		}

		private void TransformYinYangParts()
		{
			_excludedTail.Transform(_rotateMatrix);
			_includedCircle.Transform(_rotateMatrix);
			_excludedCircle.Transform(_rotateMatrix);
			_smallIncluded.Transform(_rotateMatrix);
			_smallExcluded.Transform(_rotateMatrix);
		}

		private void TimerTick(object sender, EventArgs e)
		{
			if (_tick >= 200)
				Close();

			_tick++;

			TransformYinYangParts();
			CreateYinYangPath();
		}
	}
}
