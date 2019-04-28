using System.Windows.Forms;
using NUnit.Framework;
using TicTacToe.MouseEventsHandling;

namespace TicTacToeTests.MouseEventsHandling
{
	public class MouseMessageHandlerTests
	{
		private MouseMessageHandler _unitUnderTest;

		[SetUp]
		public void Initialize()
		{
			_unitUnderTest = new MouseMessageHandler();
		}

		[Test]
		[TestCase(MouseEvent.LeftButtonDown)]
		[TestCase(MouseEvent.LeftButtonUp)]
		[TestCase(MouseEvent.RightButtonDown)]
		[TestCase(MouseEvent.RightButtonUp)]
		public void PreFilterMessage_KnownMouseEvent_True(int m)
		{
			var message = new Message { Msg = m };

			var result = _unitUnderTest.PreFilterMessage(ref message);

			Assert.IsTrue(result);
		}

		[Test]
		public void PreFilterMessage_UnknownMouseEvent_False()
		{
			var message = new Message { Msg = 0 };

			var result = _unitUnderTest.PreFilterMessage(ref message);

			Assert.IsFalse(result);
		}
	}
}
