using System;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using TicTacToe.MouseEventsHandling;

namespace TicTacToe
{
	public partial class Game : Form
	{
		private const int YinYangMargin = 4;
		private int _playerNumber;
		private readonly string[] _windowText = { Properties.Resources.Yin, Properties.Resources.Yang };
		private GraphicsPath _includedTail, _excludedCircle, _circleIncluded, _smallIncluded, _smallExcluded;
		private GameBoard _gameBoard;

		public Game()
		{
			InitializeComponent();
			StartGame();

			Application.AddMessageFilter(new MouseMessageHandler());
		}

		private void StartGame()
		{
			_playerNumber = 0;
			_gameBoard = new GameBoard(BoardSizeBar.Value, GamePanel.Width, GamePanel.Height);

			Text = _windowText[0];
			GamePanel.ColumnStyles.Clear();
			GamePanel.RowStyles.Clear();
			GamePanel.Controls.Clear();
			GamePanel.RowCount = _gameBoard.Size + 1;
			GamePanel.ColumnCount = _gameBoard.Size;

			for (int i = 0; i < GamePanel.RowCount - 1; ++i)
			{
				GamePanel.RowStyles.Add(new RowStyle(SizeType.Percent, (float)100.0 / _gameBoard.Size));
				GamePanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, (float)100.0 / _gameBoard.Size));

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
			if (_gameBoard.HasWinningPosition())
				GameOver(Properties.Resources.Win);
			if (!_gameBoard.CanContinueGame)
				GameOver(Properties.Resources.Tie);
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
			var row = btn.TabIndex / _gameBoard.Size;
			var column = btn.TabIndex % _gameBoard.Size;
			if (_gameBoard[column, row] != -1)
			{
				_gameBoard[column, row] = -1;
				btn.Region = new Region(CreateDefaultEllipse());
			}
		}

		private void BoardCellClicked(object sender, EventArgs e)
		{
			Control c = sender as Control;
			var row = c.TabIndex / _gameBoard.Size;
			var column = c.TabIndex % _gameBoard.Size;
			if (_gameBoard[column, row] == -1)
			{
				var gp = CreateDefaultEllipse();
				ResetYinYangSegments();
				InitializeYinYangSegments(_playerNumber);
				c.Region = CreateYinYangPath(gp);

				_gameBoard[column, row] = _playerNumber;
				_playerNumber = (_playerNumber + 1) % 2;
				Text = _windowText[_playerNumber];

				IsGameOver();
			}
		}

		private void BoardSizeChanged(object sender, EventArgs e)
		{
			StartGame();
		}

		private void GamePanelSizeChanged(object sender, EventArgs e)
		{
			_gameBoard.Resize(GamePanel.Width, GamePanel.Height);
			var gp = CreateDefaultEllipse();

			foreach (Button c in GamePanel.Controls)
			{
				var row = c.TabIndex / _gameBoard.Size;
				var column = c.TabIndex % _gameBoard.Size;

				if (_gameBoard[column, row] != -1)
				{
					ResetYinYangSegments();
					InitializeYinYangSegments(_gameBoard[column, row]);
					c.Region = CreateYinYangPath(gp);
				}
				else
					c.Region = new Region(gp);
			}
		}

		private void InitializeYinYangSegments(int playerNumber)
		{
			var angle = 180;
			if (playerNumber == 1)
			{
				angle *= -1;
			}

			var bigCircleRadiusX = (_gameBoard.CellWidth - (3 * YinYangMargin)) / 2;
			var bigCircleRadiusY = (_gameBoard.CellHeight - (3 * YinYangMargin)) / 2;
			var smallCircleRadiusX = bigCircleRadiusX / 8;
			var smallCircleRadiusY = bigCircleRadiusY / 8;

			_includedTail.AddArc(YinYangMargin * 2, YinYangMargin * 2, bigCircleRadiusX * 2, bigCircleRadiusY * 2, 90, angle);
			_excludedCircle.AddEllipse((_gameBoard.CellWidth - bigCircleRadiusX) / 2, YinYangMargin * 2, bigCircleRadiusX, bigCircleRadiusY);
			_circleIncluded.AddEllipse((_gameBoard.CellWidth - bigCircleRadiusX) / 2, (_gameBoard.CellHeight + YinYangMargin) / 2, bigCircleRadiusX, bigCircleRadiusY);
			_smallIncluded.AddEllipse((_gameBoard.CellWidth - bigCircleRadiusX / 2) / 2 + smallCircleRadiusX, bigCircleRadiusY / 2, smallCircleRadiusX * 2, smallCircleRadiusY * 2);
			_smallExcluded.AddEllipse((_gameBoard.CellWidth - bigCircleRadiusX / 2) / 2 + smallCircleRadiusX, 3 * bigCircleRadiusY / 2, smallCircleRadiusX * 2, smallCircleRadiusY * 2);
		}

		private Region CreateYinYangPath(GraphicsPath gp)
		{
			var yin = new Region(gp);
			yin.Exclude(_includedTail);
			yin.Union(_excludedCircle);
			yin.Exclude(_circleIncluded);
			yin.Exclude(_smallIncluded);
			yin.Union(_smallExcluded);
			return yin;
		}

		private GraphicsPath CreateDefaultEllipse()
		{
			var gp = new GraphicsPath();
			gp.AddEllipse(YinYangMargin, YinYangMargin, _gameBoard.Width / _gameBoard.Size - (2 * YinYangMargin), _gameBoard.Height / _gameBoard.Size - (2 * YinYangMargin));
			return gp;
		}

		private void ResetYinYangSegments()
		{
			_includedTail = new GraphicsPath();
			_excludedCircle = new GraphicsPath();
			_smallIncluded = new GraphicsPath();
			_circleIncluded = new GraphicsPath();
			_smallExcluded = new GraphicsPath();
		}
	}
}
