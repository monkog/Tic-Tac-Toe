namespace TicTacToe
{
	public class GameBoard
	{
		public int Size { get; }

		public int Width { get; private set; }

		public int Height { get; private set; }

		public bool CanContinueGame
		{
			get
			{
				for (int i = 0; i < Size; i++)
				{
					for (int j = 0; j < Size; j++)
					{
						if (_gameBoard[i, j] == -1) return true;
					}
				}

				return false;
			}
		}

		private readonly int[,] _gameBoard;

		public GameBoard(int size, int width, int height)
		{
			Size = size;
			Width = width;
			Height = height;

			_gameBoard = CreateEmptyGameBoard();
		}

		public int this[int i, int j]
		{
			get { return _gameBoard[i, j]; }
			set { _gameBoard[i, j] = value; }
		}

		public void Resize(int width, int height)
		{
			Width = width;
			Height = height;
		}

		public bool HasWinningPosition()
		{
			// Check all rows and columns.
			for (var i = 0; i < Size; i++)
			{
				if (AreAllPositionsSame(i, 0, 0, 1)) return true;
				if (AreAllPositionsSame(0, i, 1, 0)) return true;
			}

			// Check diagonals.
			if (AreAllPositionsSame(0, 0, 1, 1)) return true;
			if (AreAllPositionsSame(0, Size - 1, 1, -1)) return true;

			return false;
		}

		private bool AreAllPositionsSame(int initialI, int initialJ, int deltaI, int deltaJ)
		{
			var firstValue = _gameBoard[initialI, initialJ];
			if (firstValue == -1) return false;

			for (int i = initialI, j = initialJ; i >= 0 && i < Size && j >= 0 && j < Size; i += deltaI, j += deltaJ)
			{
				if (_gameBoard[i, j] != firstValue) return false;
			}

			return true;
		}

		private int[,] CreateEmptyGameBoard()
		{
			var gameBoard = new int[Size, Size];
			for (var i = 0; i < Size; i++)
			{
				for (var j = 0; j < Size; j++)
				{
					gameBoard[i, j] = -1;
				}
			}

			return gameBoard;
		}
	}
}
