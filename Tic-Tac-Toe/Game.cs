using System;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using TicTacToe.MouseEventsHandling;

namespace TicTacToe
{
	public partial class Game : Form
	{
		private int _playerNumber;
		private int _boardSize = 3;
		private readonly string[] _windowText = { Properties.Resources.Yin, Properties.Resources.Yang };
		private GraphicsPath _right, _upper, _down, _small, _small2;
		private int[,] _gameBoard;

		public Game()
		{
			InitializeComponent();
			StartGame();

			Application.AddMessageFilter(new MouseMessageHandler());
		}

		private void StartGame()
		{
			_playerNumber = 0;
			_gameBoard = CreateEmptyGameBoard();

			Text = _windowText[0];
			GamePanel.ColumnStyles.Clear();
			GamePanel.RowStyles.Clear();
			GamePanel.Controls.Clear();
			GamePanel.RowCount = _boardSize + 1;
			GamePanel.ColumnCount = _boardSize;

			for (int i = 0; i < GamePanel.RowCount - 1; ++i)
			{
				GamePanel.RowStyles.Add(new RowStyle(SizeType.Absolute, (GamePanel.Height - BoardSizeBar.Height) / _boardSize));
				GamePanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, (float)100.0 / _boardSize));

				for (int j = 0; j < GamePanel.ColumnCount; ++j)
				{
					var btnCard = new Button
					{
						Anchor = AnchorStyles.Bottom | AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right,
						BackColor = SystemColors.Control,
						Margin = new Padding(0)
					};
					btnCard.Click += BoardCellClicked;

					btnCard.ContextMenuStrip = new ContextMenuStrip();
					var _ = new ToolStripMenuItem(Properties.Resources.Reset) { Owner = btnCard.ContextMenuStrip };
					btnCard.ContextMenuStrip.Items[0].Click += ResetCellValue;

					var gp = CreateDefaultEllipse();
					btnCard.Region = new Region(gp);
					btnCard.UseVisualStyleBackColor = true;
					GamePanel.Controls.Add(btnCard);
				}
			}
		}

		private void IsGameOver()
		{
			var winner = true;

			for (int i = 0; i < _boardSize; i++)
			{
				winner = true;
				if (_gameBoard[i, 0] == -1)
					winner = false;
				for (int j = 0; j < _boardSize && winner; j++)
					if (_gameBoard[i, 0] != _gameBoard[i, j])
						winner = false;
				if (winner)
				{
					GameOver(Properties.Resources.Win);
					return;
				}
			}
			for (int i = 0; i < _boardSize; i++)
			{
				winner = true;
				if (_gameBoard[0, i] == -1)
					winner = false;
				for (int j = 0; j < _boardSize && winner; j++)
					if (_gameBoard[0, i] != _gameBoard[j, i])
						winner = false;
				if (winner)
				{
					GameOver(Properties.Resources.Win);
					return;
				}
			}
			for (int i = 1; i < _boardSize; i++)
			{
				winner = true;
				if (_gameBoard[i, i] == -1)
				{
					winner = false;
					break;
				}
				if (_gameBoard[i, i] != _gameBoard[0, 0])
				{
					winner = false;
					break;
				}
			}
			if (winner)
			{
				GameOver(Properties.Resources.Win);
				return;
			}
			for (int i = _boardSize - 1; i >= 0; i--)
			{
				winner = true;
				if (_gameBoard[_boardSize - i - 1, i] == -1)
				{
					winner = false;
					break;
				}
				if (_gameBoard[_boardSize - i - 1, i] != _gameBoard[0, _boardSize - 1])
				{
					winner = false;
					break;
				}
			}
			if (winner)
			{
				GameOver(Properties.Resources.Win);
				return;
			}

			for (int i = 0; i < _boardSize; i++)
			{
				for (int j = 0; j < _boardSize; j++)
				{
					if (_gameBoard[i, j] == -1)
					{
						return;
					}
				}
			}

			GameOver(Properties.Resources.Tie);
		}

		private int[,] CreateEmptyGameBoard()
		{
			var gameBoard = new int[_boardSize, _boardSize];
			for (var i = 0; i < _boardSize; i++)
			{
				for (var j = 0; j < _boardSize; j++)
				{
					gameBoard[i, j] = -1;
				}
			}

			return gameBoard;
		}

		private void GameOver(string gameResult)
		{
			var result = MessageBox.Show(gameResult, Properties.Resources.GameOver, MessageBoxButtons.YesNo);
			if (result == DialogResult.Yes)
				StartGame();
			else
				Close();
		}

		private void ResetCellValue(object sender, EventArgs e)
		{
			ToolStripMenuItem c = sender as ToolStripMenuItem;
			ContextMenuStrip ts = (ContextMenuStrip)c.Owner;
			Control btn = ts.SourceControl;
			var row = btn.TabIndex / _boardSize;
			var column = btn.TabIndex % _boardSize;
			if (_gameBoard[column, row] != -1)
			{
				_gameBoard[column, row] = -1;
				btn.Region = new Region(CreateDefaultEllipse());
			}
		}

		private void BoardCellClicked(object sender, EventArgs e)
		{
			Control c = sender as Control;
			var row = c.TabIndex / _boardSize;
			var column = c.TabIndex % _boardSize;
			if (_gameBoard[column, row] == -1)
			{
				var gp = CreateDefaultEllipse();
				ResetYinYangSegments();

				int x = c.Width / 35;
				int y = c.Height / 35;

				InitializeYinYangSegments(x, y, c.Size, _playerNumber);

				c.Region = CreateYinYangPath(gp);

				_gameBoard[column, row] = _playerNumber;
				_playerNumber = (_playerNumber + 1) % 2;
				Text = _windowText[_playerNumber];

				IsGameOver();
			}
		}

		private void BoardSizeChanged(object sender, EventArgs e)
		{
			_boardSize = BoardSizeBar.Value;
			StartGame();
		}

		private void GamePanelSizeChanged(object sender, EventArgs e)
		{
			GamePanel.RowStyles.Clear();
			for (int i = 0; i < GamePanel.RowCount - 1; ++i)
				GamePanel.RowStyles.Add(new RowStyle(SizeType.Absolute, (GamePanel.Height - BoardSizeBar.Height) / _boardSize));

			var gp = CreateDefaultEllipse();

			foreach (Button c in GamePanel.Controls)
			{
				var row = c.TabIndex / _boardSize;
				var column = c.TabIndex % _boardSize;

				if (_gameBoard[column, row] != -1)
				{
					ResetYinYangSegments();

					int x = c.Width / 35;
					int y = c.Height / 35;

					InitializeYinYangSegments(x, y, c.Size, _gameBoard[column, row]);

					c.Region = CreateYinYangPath(gp);

				}
				else
					c.Region = new Region(gp);
			}
		}

		private void InitializeYinYangSegments(int x, int y, Size size, int playerNumber)
		{
			var angle = 180;
			if (playerNumber == 1)
			{
				angle *= -1;
			}

			_right.AddArc(x, y, (GamePanel.Width - BoardSizeBar.Height / 2) / _boardSize - 2 * x, (GamePanel.Height - 3 * BoardSizeBar.Height / 2) / _boardSize - 2 * y, 90, angle);
			_upper.AddEllipse(size.Width * 1 / 5 + x, y, (size.Width - 3 * x) / 2, (size.Height - 3 * y) / 2);
			_down.AddEllipse(size.Width * 1 / 5 + x, size.Height / 2 - y, (size.Width - 3 * x) / 2, (size.Height - 3 * y) / 2);
			_small.AddEllipse(size.Width * 4 / 10 + x, size.Height * 2 / 9 + y, size.Width / 10, size.Height / 10);
			_small2.AddEllipse(size.Width * 4 / 10 + x, size.Height * 6 / 9 + y, size.Width / 10, size.Height / 10);
		}

		private Region CreateYinYangPath(GraphicsPath gp)
		{
			var yin = new Region(gp);
			yin.Exclude(_right);
			yin.Union(_upper);
			yin.Exclude(_down);
			yin.Exclude(_small);
			yin.Union(_small2);
			return yin;
		}

		private GraphicsPath CreateDefaultEllipse()
		{
			var gp = new GraphicsPath();
			gp.AddEllipse(0, 0, (GamePanel.Width - BoardSizeBar.Height / 2) / _boardSize + 3, (GamePanel.Height - 3 * BoardSizeBar.Height / 2) / _boardSize + 3);
			return gp;
		}

		private void ResetYinYangSegments()
		{
			_right = new GraphicsPath();
			_upper = new GraphicsPath();
			_small = new GraphicsPath();
			_down = new GraphicsPath();
			_small2 = new GraphicsPath();
		}
	}
}
