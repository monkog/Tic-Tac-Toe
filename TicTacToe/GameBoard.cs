namespace TicTacToe
{
	public class GameBoard
	{
		/// <summary>
		/// Gets the number of rows and columns on the board.
		/// </summary>
		public int Size { get; }

		/// <summary>
		/// Gets the width od the board in pixels.
		/// </summary>
		public int Width { get; private set; }

		/// <summary>
		/// Gets the height of the board in pixels.
		/// </summary>
		public int Height { get; private set; }

		/// <summary>
		/// Gets a value indicating whether the game can be continued.
		/// Returns true if there is at least one field that has not been taken by the user.
		/// </summary>
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

		/// <summary>
		/// Resizes the game board.
		/// </summary>
		/// <param name="width">New width of the board.</param>
		/// <param name="height">New height of the board.</param>
		public void Resize(int width, int height)
		{
			Width = width;
			Height = height;
		}

		/// <summary>
		/// Checks whether there is a winning position in a game.
		/// </summary>
		/// <returns>True if there is a winning position, false otherwise.</returns>
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
