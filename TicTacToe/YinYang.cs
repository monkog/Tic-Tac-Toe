using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace TicTacToe
{
	[ExcludeFromCodeCoverage]
	public class YinYang
	{
		private const int Margin = 4;
		private static GraphicsPath _includedTail, _excludedCircle, _circleIncluded, _smallIncluded, _smallExcluded;

		/// <summary>
		/// Draws the ☯️ (Yin Yang) sign.
		/// </summary>
		/// <param name="width">Width of the available space.</param>
		/// <param name="height">Height of the available space.</param>
		/// <param name="division">The number of ☯️ to draw.</param>
		/// <param name="isReverse">Value determining whether the ☯️ should be drawn in reverse.</param>
		/// <returns>Returns the constructed region containing ☯️.</returns>
		public static Region Draw(int width, int height, int division, bool isReverse)
		{
			var cellWidth = width / division;
			var cellHeight = height / division;

			ResetYinYangSegments();
			InitializeYinYangSegments(cellWidth, cellHeight, isReverse);
			return CreateYinYangPath(cellWidth, cellHeight);
		}

		/// <summary>
		/// Draws an empty ellipse.
		/// </summary>
		/// <param name="width">Width of the available space.</param>
		/// <param name="height">Height of the available space.</param>
		/// <param name="division">The number of ellipses to draw.</param>
		/// <returns>Returns the constructed region containing ellipse.</returns>
		public static Region DrawEmpty(int width, int height, int division)
		{
			var path = new GraphicsPath();
			path.AddEllipse(Margin, Margin, width / division - (2 * Margin), height / division - (2 * Margin));
			return new Region(path);
		}

		private static void InitializeYinYangSegments(int cellWidth, int cellHeight, bool isReverse)
		{
			var angle = 180;
			if (isReverse)
			{
				angle *= -1;
			}

			var bigCircleRadiusX = (cellWidth - (4 * Margin)) / 2;
			var bigCircleRadiusY = (cellHeight - (4 * Margin)) / 2;
			var smallCircleRadiusX = bigCircleRadiusX / 8;
			var smallCircleRadiusY = bigCircleRadiusY / 8;

			_includedTail.AddArc(Margin * 2, Margin * 2, bigCircleRadiusX * 2, bigCircleRadiusY * 2, 90, angle);
			_excludedCircle.AddEllipse((cellWidth - bigCircleRadiusX) / 2, Margin * 2, bigCircleRadiusX, bigCircleRadiusY);
			_circleIncluded.AddEllipse((cellWidth - bigCircleRadiusX) / 2, cellHeight / 2, bigCircleRadiusX, bigCircleRadiusY);
			_smallIncluded.AddEllipse((cellWidth - bigCircleRadiusX / 2) / 2 + smallCircleRadiusX, bigCircleRadiusY / 2, smallCircleRadiusX * 2, smallCircleRadiusY * 2);
			_smallExcluded.AddEllipse((cellWidth - bigCircleRadiusX / 2) / 2 + smallCircleRadiusX, 3 * bigCircleRadiusY / 2, smallCircleRadiusX * 2, smallCircleRadiusY * 2);
		}

		private static Region CreateYinYangPath(int cellWidth, int cellHeight)
		{
			var path = new GraphicsPath();
			path.AddEllipse(Margin, Margin, cellWidth - (2 * Margin), cellHeight - (2 * Margin));

			var yin = new Region(path);
			yin.Exclude(_includedTail);
			yin.Union(_excludedCircle);
			yin.Exclude(_circleIncluded);
			yin.Exclude(_smallIncluded);
			yin.Union(_smallExcluded);
			return yin;
		}

		private static void ResetYinYangSegments()
		{
			_includedTail = new GraphicsPath();
			_excludedCircle = new GraphicsPath();
			_smallIncluded = new GraphicsPath();
			_circleIncluded = new GraphicsPath();
			_smallExcluded = new GraphicsPath();
		}
	}
}
