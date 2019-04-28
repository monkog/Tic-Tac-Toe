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
			_size = 3;
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

			_unitUnderTest[2, 1] = value;

			Assert.AreEqual(value, _unitUnderTest[2, 1]);
		}

		[Test]
		public void HasWinningPosition_DefaultBoard_False()
		{
			var result = _unitUnderTest.HasWinningPosition();

			Assert.IsFalse(result);
		}

		[Test]
		public void HasWinningPosition_FirstRowWinning_True()
		{
			_unitUnderTest[0, 0] = 1;
			_unitUnderTest[1, 0] = 1;
			_unitUnderTest[2, 0] = 1;

			var result = _unitUnderTest.HasWinningPosition();

			Assert.IsTrue(result);
		}

		[Test]
		public void HasWinningPosition_SecondRowWinning_True()
		{
			_unitUnderTest[0, 1] = 1;
			_unitUnderTest[1, 1] = 1;
			_unitUnderTest[2, 1] = 1;

			var result = _unitUnderTest.HasWinningPosition();

			Assert.IsTrue(result);
		}

		[Test]
		public void HasWinningPosition_SecondColumnWinning_True()
		{
			_unitUnderTest[1, 0] = 1;
			_unitUnderTest[1, 1] = 1;
			_unitUnderTest[1, 2] = 1;

			var result = _unitUnderTest.HasWinningPosition();

			Assert.IsTrue(result);
		}

		[Test]
		public void HasWinningPosition_LastColumnWinning_True()
		{
			_unitUnderTest[_size - 1, 0] = 1;
			_unitUnderTest[_size - 1, 1] = 1;
			_unitUnderTest[_size - 1, 2] = 1;

			var result = _unitUnderTest.HasWinningPosition();

			Assert.IsTrue(result);
		}

		[Test]
		public void HasWinningPosition_DiagonalWinning_True()
		{
			_unitUnderTest[0, 0] = 1;
			_unitUnderTest[1, 1] = 1;
			_unitUnderTest[2, 2] = 1;

			var result = _unitUnderTest.HasWinningPosition();

			Assert.IsTrue(result);
		}

		[Test]
		public void HasWinningPosition_OtherDiagonalWinning_True()
		{
			_unitUnderTest[2, 0] = 1;
			_unitUnderTest[1, 1] = 1;
			_unitUnderTest[0, 2] = 1;

			var result = _unitUnderTest.HasWinningPosition();

			Assert.IsTrue(result);
		}

		[Test]
		public void HasWinningPosition_RandomValues_False()
		{
			_unitUnderTest[0, 0] = 1;
			_unitUnderTest[0, 1] = 0;
			_unitUnderTest[1, 1] = 1;
			_unitUnderTest[2, 1] = 1;
			_unitUnderTest[2, 1] = 0;
			_unitUnderTest[2, 2] = 0;

			var result = _unitUnderTest.HasWinningPosition();

			Assert.False(result);
		}

		[Test]
		public void CanContinueGame_DefaultBoard_True()
		{
			var result = _unitUnderTest.CanContinueGame;

			Assert.IsTrue(result);
		}

		[Test]
		public void CanContinueGame_OneValueNotSet_True()
		{
			_unitUnderTest[0, 0] = 1;
			_unitUnderTest[0, 1] = 0;
			_unitUnderTest[0, 2] = 0;
			_unitUnderTest[1, 1] = 1;
			_unitUnderTest[1, 2] = 1;
			_unitUnderTest[2, 0] = 1;
			_unitUnderTest[2, 1] = 1;
			_unitUnderTest[2, 2] = 0;

			var result = _unitUnderTest.CanContinueGame;

			Assert.IsTrue(result);
		}

		[Test]
		public void CanContinueGame_AllValuesSet_False()
		{
			_unitUnderTest[0, 0] = 1;
			_unitUnderTest[0, 1] = 0;
			_unitUnderTest[0, 2] = 0;
			_unitUnderTest[1, 0] = 1;
			_unitUnderTest[1, 1] = 1;
			_unitUnderTest[1, 2] = 1;
			_unitUnderTest[2, 0] = 1;
			_unitUnderTest[2, 1] = 1;
			_unitUnderTest[2, 2] = 0;

			var result = _unitUnderTest.CanContinueGame;

			Assert.IsFalse(result);
		}
	}
}
