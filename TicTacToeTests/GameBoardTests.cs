using NUnit.Framework;
using TicTacToe;

namespace TicTacToeTests
{
	[TestFixture]
	public class GameBoardTests
	{
		private int _size;

		private GameBoard _unitUnderTest;

		[SetUp]
		public void Initialize()
		{
			_size = 5;
			_unitUnderTest = new GameBoard(_size, 0, 0);
		}

		[Test]
		public void Constructor_Params_PropertiesInitialized()
		{
			const int size = 5;
			const int width = 1000;
			const int height = 30;

			var unitUnderTest = new GameBoard(size, width, height);

			Assert.AreEqual(size, unitUnderTest.Size);
			Assert.AreEqual(width, unitUnderTest.Width);
			Assert.AreEqual(height, unitUnderTest.Height);
		}

		[Test]
		public void Indexer_DefaultObject_NegativeOneValues()
		{
			for (var i = 0; i < _size; i++)
			{
				for (var j = 0; j < _size; j++)
				{
					Assert.AreEqual(-1, _unitUnderTest[i, j]);
				}
			}
		}

		[Test]
		public void SetValue_Integer_ValueSet()
		{
			const int value = 10;

			_unitUnderTest[2, 3] = value;

			Assert.AreEqual(value, _unitUnderTest[2, 3]);
		}
	}
}
