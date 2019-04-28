using System;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;

namespace TicTacToe
{
    public partial class Game : Form
    {
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        static extern IntPtr SendMessage(IntPtr hWnd, UInt32 msg, IntPtr wParam, IntPtr lParam);

        private int _playerNumber;
        private int _boardSize = 3;
        private readonly string[] _windowText = { "Yin", "Yang" };
        private GraphicsPath _right, _upper, _down, _small, _small2;
        private Region _yin;

        public class MouseChange : IMessageFilter
        {
            const int WM_LBUTTONDOWN = 0x0201;
            const int WM_RBUTTONDOWN = 0x0204;
            const int WM_RBUTTONUP = 0x0205;
            const int WM_LBUTTONUP = 0x0202;

            public bool PreFilterMessage(ref Message m)
            {
                var changed = true;

                if (m.Msg == WM_LBUTTONDOWN)
                    SendMessage(m.HWnd, WM_RBUTTONDOWN, m.WParam, m.LParam);
                else if (m.Msg == WM_LBUTTONUP)
                    SendMessage(m.HWnd, WM_RBUTTONUP, m.WParam, m.LParam);
                else if (m.Msg == WM_RBUTTONDOWN)
                    SendMessage(m.HWnd, WM_LBUTTONDOWN, m.WParam, m.LParam);
                else if (m.Msg == WM_RBUTTONUP)
                    SendMessage(m.HWnd, WM_LBUTTONUP, m.WParam, m.LParam);
                else
                    changed = false;
                return changed;
            }
        }

        public Game()
        {
            InitializeComponent();
            MinimumSize = new System.Drawing.Size(500, 500);
            StartGame();
            Application.AddMessageFilter(new MouseChange());
        }

        private void StartGame()
        {
            _playerNumber = 0;
            Text = _windowText[0];
            tableLayoutPanel.ColumnStyles.Clear();
            tableLayoutPanel.RowStyles.Clear();
            tableLayoutPanel.Controls.Clear();
            tableLayoutPanel.RowCount = _boardSize + 1;
            tableLayoutPanel.ColumnCount = _boardSize;

            for (int i = 0; i < tableLayoutPanel.RowCount - 1; ++i)
            {
                tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, (tableLayoutPanel.Height - trackBar.Height) / _boardSize));
                tableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, (float)100.0 / _boardSize));

                for (int j = 0; j < tableLayoutPanel.ColumnCount; ++j)
                {
                    var btnCard = new Button
                    {
                        Tag = -1,
                        Anchor = AnchorStyles.Bottom | AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right,
                        BackColor = SystemColors.Control,
                        Margin = new Padding(0)
                    };
                    btnCard.Click += pctrCard_Click;

                    btnCard.ContextMenuStrip = new ContextMenuStrip();
                    ToolStripMenuItem tsmi = new ToolStripMenuItem("Reset");
                    tsmi.Owner = btnCard.ContextMenuStrip;
                    btnCard.ContextMenuStrip.Items[0].Click += resetClick;

                    GraphicsPath gp = new GraphicsPath();
                    gp.AddEllipse(0, 0, (tableLayoutPanel.Width - trackBar.Height / 2) / _boardSize, (tableLayoutPanel.Height - 3 * trackBar.Height / 2) / _boardSize);
                    btnCard.Region = new Region(gp);
                    btnCard.UseVisualStyleBackColor = true;
                    tableLayoutPanel.Controls.Add(btnCard);
                }
            }
        }

        private void GameOver()
        {
            var winner = true;
            for (int i = 0; i < _boardSize; i++)
            {
                winner = true;
                Control con = tableLayoutPanel.GetControlFromPosition(i, 0);
                if ((int)con.Tag == -1)
                    winner = false;
                for (int j = 0; j < _boardSize && winner; j++)
                    if ((int)con.Tag != (int)tableLayoutPanel.GetControlFromPosition(i, j).Tag)
                        winner = false;
                if (winner)
                {
                    won();
                    return;
                }
            }
            for (int i = 0; i < _boardSize; i++)
            {
                winner = true;
                Control con = tableLayoutPanel.GetControlFromPosition(0, i);
                if ((int)con.Tag == -1)
                    winner = false;
                for (int j = 0; j < _boardSize && winner; j++)
                    if ((int)con.Tag != (int)tableLayoutPanel.GetControlFromPosition(j, i).Tag)
                        winner = false;
                if (winner)
                {
                    won();
                    return;
                }
            }
            for (int i = 1; i < _boardSize; i++)
            {
                winner = true;
                Control con = tableLayoutPanel.GetControlFromPosition(i, i);
                if ((int)con.Tag == -1)
                {
                    winner = false;
                    break;
                }
                if ((int)con.Tag != (int)tableLayoutPanel.GetControlFromPosition(0, 0).Tag)
                {
                    winner = false;
                    break;
                }
            }
            if (winner)
            {
                won();
                return;
            }
            for (int i = _boardSize - 1; i >= 0; i--)
            {
                winner = true;
                Control con = tableLayoutPanel.GetControlFromPosition(_boardSize - i - 1, i);
                if ((int)con.Tag == -1)
                {
                    winner = false;
                    break;
                }
                if ((int)con.Tag != (int)tableLayoutPanel.GetControlFromPosition(0, _boardSize - 1).Tag)
                {
                    winner = false;
                    break;
                }
            }
            if (winner)
            {
                won();
                return;
            }

            var tie = true;
            foreach (Button btnCard in tableLayoutPanel.Controls)
                if ((int)btnCard.Tag == -1)
                {
	                return;
                }
            if (tie)
            {
                DialogResult dr = MessageBox.Show("Tie. Play again?", null, MessageBoxButtons.YesNo);
                if (dr == DialogResult.Yes)
                    StartGame();
                else
                    Close();
            }
        }

        private void won()
        {
            DialogResult dr = MessageBox.Show("You won!!! Play again?", null, MessageBoxButtons.YesNo);
            if (dr == DialogResult.Yes)
                StartGame();
            else
                Close();
        }

        private void resetClick(Object sender, EventArgs e)
        {
            ToolStripMenuItem c = sender as ToolStripMenuItem;
            ContextMenuStrip ts = (ContextMenuStrip)c.Owner;
            Control btn = ts.SourceControl;
            if ((int)btn.Tag != -1)
            {
                btn.Tag = -1;
                GraphicsPath gp = new GraphicsPath();
                gp.AddEllipse(0, 0, (tableLayoutPanel.Width - trackBar.Height / 2) / _boardSize, (tableLayoutPanel.Height - 3 * trackBar.Height / 2) / _boardSize);
                btn.Region = new Region(gp);
            }
        }

        private void pctrCard_Click(Object sender, EventArgs e)
        {
            Control c = sender as Control;
            if ((int)c.Tag == -1)
            {
                GraphicsPath gp = new GraphicsPath();
                gp.AddEllipse(0, 0, (tableLayoutPanel.Width - trackBar.Height / 2) / _boardSize + 3, (tableLayoutPanel.Height - 3 * trackBar.Height / 2) / _boardSize + 3);

                _right = new GraphicsPath();
                _upper = new GraphicsPath();
                _small = new GraphicsPath();
                _down = new GraphicsPath();
                _small2 = new GraphicsPath();

                int x = c.Width / 35;
                int y = c.Height / 35;

                if (_playerNumber == 1)
                {
                    _right.AddArc(x, y, (tableLayoutPanel.Width - trackBar.Height / 2) / _boardSize - 2 * x, (tableLayoutPanel.Height - 3 * trackBar.Height / 2) / _boardSize - 2 * y, 90, -180);
                    _upper.AddEllipse(c.Size.Width * 1 / 5 + x, y, (c.Size.Width - 3 * x) / 2, (c.Size.Height - 3 * y) / 2);
                    _down.AddEllipse(c.Size.Width * 1 / 5 + x, c.Size.Height / 2 - y, (c.Size.Width - 3 * x) / 2, (c.Size.Height - 3 * y) / 2);
                    _small.AddEllipse(c.Size.Width * 4 / 10 + x, c.Size.Height * 2 / 9 + y, c.Size.Width / 10, c.Size.Height / 10);
                    _small2.AddEllipse(c.Size.Width * 4 / 10 + x, c.Size.Height * 6 / 9 + y, c.Size.Width / 10, c.Size.Height / 10);
                }
                else
                {
                    _right.AddArc(x, y, (tableLayoutPanel.Width - trackBar.Height / 2) / _boardSize - 2 * x, (tableLayoutPanel.Height - 3 * trackBar.Height / 2) / _boardSize - 2 * y, 90, 180);
                    _upper.AddEllipse(c.Size.Width * 1 / 5 + x, y, (c.Size.Width - 3 * x) / 2, (c.Size.Height - 3 * y) / 2);
                    _down.AddEllipse(c.Size.Width * 1 / 5 + x, c.Size.Height / 2 - y, (c.Size.Width - 3 * x) / 2, (c.Size.Height - 3 * y) / 2);
                    _small.AddEllipse(c.Size.Width * 4 / 10 + x, c.Size.Height * 2 / 9 + y, c.Size.Width / 10, c.Size.Height / 10);
                    _small2.AddEllipse(c.Size.Width * 4 / 10 + x, c.Size.Height * 6 / 9 + y, c.Size.Width / 10, c.Size.Height / 10);
                }

                _yin = new Region(gp);
                _yin.Exclude(_right);
                _yin.Union(_upper);
                _yin.Exclude(_down);
                _yin.Exclude(_small);
                _yin.Union(_small2);

                c.Region = _yin;

                c.Tag = _playerNumber;
                _playerNumber = (_playerNumber + 1) % 2;
                Text = _windowText[_playerNumber];

                GameOver();
            }
        }

        private void trackBar_ValueChanged(object sender, EventArgs e)
        {
            _boardSize = trackBar.Value;
            StartGame();
        }

        private void tableLayoutPanel_SizeChanged(object sender, EventArgs e)
        {
            tableLayoutPanel.RowStyles.Clear();
            for (int i = 0; i < tableLayoutPanel.RowCount - 1; ++i)
                tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, (tableLayoutPanel.Height - trackBar.Height) / _boardSize));

            GraphicsPath gp = new GraphicsPath();
            gp.AddEllipse(0, 0, (tableLayoutPanel.Width - trackBar.Height / 2) / _boardSize + 3, (tableLayoutPanel.Height - 3 * trackBar.Height / 2) / _boardSize + 3);

            foreach (Button btnCard in tableLayoutPanel.Controls)
            {
                if ((int)btnCard.Tag != -1)
                {
                    _right = new GraphicsPath();
                    _upper = new GraphicsPath();
                    _small = new GraphicsPath();
                    _down = new GraphicsPath();
                    _small2 = new GraphicsPath();

                    int x = btnCard.Width / 35;
                    int y = btnCard.Height / 35;

                    if ((int)btnCard.Tag == 1)
                    {
                        _right.AddArc(x, y, (tableLayoutPanel.Width - trackBar.Height / 2) / _boardSize - 2 * x, (tableLayoutPanel.Height - 3 * trackBar.Height / 2) / _boardSize - 2 * y, 90, -180);
                        _upper.AddEllipse(btnCard.Size.Width * 1 / 5 + x, y, (btnCard.Size.Width - 3 * x) / 2, (btnCard.Size.Height - 3 * y) / 2);
                        _down.AddEllipse(btnCard.Size.Width * 1 / 5 + x, btnCard.Size.Height / 2 - y, (btnCard.Size.Width - 3 * x) / 2, (btnCard.Size.Height - 3 * y) / 2);
                        _small.AddEllipse(btnCard.Size.Width * 4 / 10 + x, btnCard.Size.Height * 2 / 9 + y, btnCard.Size.Width / 10, btnCard.Size.Height / 10);
                        _small2.AddEllipse(btnCard.Size.Width * 4 / 10 + x, btnCard.Size.Height * 6 / 9 + y, btnCard.Size.Width / 10, btnCard.Size.Height / 10);
                    }
                    else
                    {
                        _right.AddArc(x, y, (tableLayoutPanel.Width - trackBar.Height / 2) / _boardSize - 2 * x, (tableLayoutPanel.Height - 3 * trackBar.Height / 2) / _boardSize - 2 * y, 90, 180);
                        _upper.AddEllipse(btnCard.Size.Width * 1 / 5 + x, y, (btnCard.Size.Width - 2 * x) / 2, (btnCard.Size.Height - 2 * y) / 2);
                        _down.AddEllipse(btnCard.Size.Width * 1 / 5 + x, btnCard.Size.Height / 2 - y, (btnCard.Size.Width - 2 * x) / 2, (btnCard.Size.Height - 2 * y) / 2);
                        _small.AddEllipse(btnCard.Size.Width * 4 / 10 + x, btnCard.Size.Height * 2 / 9 + y, btnCard.Size.Width / 10, btnCard.Size.Height / 10);
                        _small2.AddEllipse(btnCard.Size.Width * 4 / 10 + x, btnCard.Size.Height * 6 / 9 + y, btnCard.Size.Width / 10, btnCard.Size.Height / 10);
                    }

                    _yin = new Region(gp);
                    _yin.Exclude(_right);
                    _yin.Union(_upper);
                    _yin.Exclude(_down);
                    _yin.Exclude(_small);
                    _yin.Union(_small2);

                    btnCard.Region = _yin;

                }
                else
                    btnCard.Region = new Region(gp);
            }
        }
    }
}
