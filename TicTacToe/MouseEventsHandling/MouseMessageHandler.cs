using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace TicTacToe.MouseEventsHandling
{
	public class MouseMessageHandler : IMessageFilter
	{
		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		private static extern IntPtr SendMessage(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam);

		public bool PreFilterMessage(ref Message message)
		{
			switch (message.Msg)
			{
				case MouseEvent.LeftButtonDown:
					SendMessage(message.HWnd, MouseEvent.RightButtonDown, message.WParam, message.LParam);
					break;
				case MouseEvent.LeftButtonUp:
					SendMessage(message.HWnd, MouseEvent.RightButtonUp, message.WParam, message.LParam);
					break;
				case MouseEvent.RightButtonDown:
					SendMessage(message.HWnd, MouseEvent.LeftButtonDown, message.WParam, message.LParam);
					break;
				case MouseEvent.RightButtonUp:
					SendMessage(message.HWnd, MouseEvent.LeftButtonUp, message.WParam, message.LParam);
					break;
				default:
					return false;
			}

			return true;
		}
	}
}
