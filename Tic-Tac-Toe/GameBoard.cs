namespace TicTacToe
{
	public class GameBoard
	{
		public int Size { get; }

		public int Width { get; }

		public int Height { get; }

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
