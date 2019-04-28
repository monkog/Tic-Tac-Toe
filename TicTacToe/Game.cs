using System;
using System.Drawing;
using System.Windows.Forms;
using TicTacToe.MouseEventsHandling;

namespace TicTacToe
{
	public partial class Game : Form
	{
		private int _playerNumber;
		private readonly string[] _playerNames = { Properties.Resources.Yin, Properties.Resources.Yang };
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

			Text = _playerNames[0];
			PrepareGameBoardControls();
		}

		private void PrepareGameBoardControls()
		{
			GamePanel.ColumnStyles.Clear();
			GamePanel.RowStyles.Clear();
			GamePanel.Controls.Clear();
			GamePanel.RowCount = _gameBoard.Size;
			GamePanel.ColumnCount = _gameBoard.Size;

			for (var i = 0; i < _gameBoard.Size; ++i)
			{
				GamePanel.RowStyles.Add(new RowStyle(SizeType.Percent, (float)100.0 / _gameBoard.Size));
				GamePanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, (float)100.0 / _gameBoard.Size));

				for (var j = 0; j < _gameBoard.Size; ++j)
				{
					var control = new Button
					{
						Anchor = AnchorStyles.Bottom | AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right,
						BackColor = SystemColors.Control,
						Margin = new Padding(0)
					};
					control.Click += BoardCellClicked;

					control.ContextMenuStrip = new ContextMenuStrip();
					var _ = new ToolStripMenuItem(Properties.Resources.Reset) { Owner = control.ContextMenuStrip };
					control.ContextMenuStrip.Items[0].Click += ResetCellValue;

					control.Region = YinYang.DrawEmpty(_gameBoard.Width, _gameBoard.Height, _gameBoard.Size);
					GamePanel.Controls.Add(control);
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
			var c = sender as ToolStripMenuItem;
			var ts = c.Owner as ContextMenuStrip;
			var control = ts.SourceControl;

			var row = control.TabIndex / _gameBoard.Size;
			var column = control.TabIndex % _gameBoard.Size;
			if (_gameBoard[column, row] == -1) return;

			_gameBoard[column, row] = -1;
			control.Region = YinYang.DrawEmpty(_gameBoard.Width, _gameBoard.Height, _gameBoard.Size);
		}

		private void BoardCellClicked(object sender, EventArgs e)
		{
			var control = sender as Control;
			var row = control.TabIndex / _gameBoard.Size;
			var column = control.TabIndex % _gameBoard.Size;
			if (_gameBoard[column, row] != -1) return;

			control.Region = YinYang.Draw(_gameBoard.Width, _gameBoard.Height, _gameBoard.Size, _playerNumber == 1);

			_gameBoard[column, row] = _playerNumber;
			_playerNumber = (_playerNumber + 1) % 2;
			Text = _playerNames[_playerNumber];

			IsGameOver();
		}

		private void BoardSizeChanged(object sender, EventArgs e)
		{
			StartGame();
		}

		private void GamePanelSizeChanged(object sender, EventArgs e)
		{
			_gameBoard.Resize(GamePanel.Width, GamePanel.Height);

			foreach (Button button in GamePanel.Controls)
			{
				var row = button.TabIndex / _gameBoard.Size;
				var column = button.TabIndex % _gameBoard.Size;

				button.Region = _gameBoard[column, row] != -1
					? YinYang.Draw(_gameBoard.Width, _gameBoard.Height, _gameBoard.Size, _gameBoard[column, row] == 1)
					: YinYang.DrawEmpty(_gameBoard.Width, _gameBoard.Height, _gameBoard.Size);
			}
		}
	}
}
