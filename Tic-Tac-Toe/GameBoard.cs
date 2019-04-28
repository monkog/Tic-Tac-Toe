namespace TicTacToe
{
	public class GameBoard
	{
		public int Size { get; }

		public int Width { get; private set; }

		public int Height { get; private set; }

		public int CellWidth { get { return Width / Size - 4; } }

		public int CellHeight { get { return Height / Size - 4; } }

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
