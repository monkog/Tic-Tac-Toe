namespace TicTacToe
{
    partial class Game
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
			this.GamePanel = new System.Windows.Forms.TableLayoutPanel();
			this.BoardSizeBar = new System.Windows.Forms.TrackBar();
			((System.ComponentModel.ISupportInitialize)(this.BoardSizeBar)).BeginInit();
			this.SuspendLayout();
			// 
			// GamePanel
			// 
			this.GamePanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.GamePanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.GamePanel.BackColor = System.Drawing.Color.Black;
			this.GamePanel.ColumnCount = 3;
			this.GamePanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
			this.GamePanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33334F));
			this.GamePanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33334F));
			this.GamePanel.GrowStyle = System.Windows.Forms.TableLayoutPanelGrowStyle.FixedSize;
			this.GamePanel.Location = new System.Drawing.Point(0, 0);
			this.GamePanel.Name = "GamePanel";
			this.GamePanel.RowCount = 3;
			this.GamePanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
			this.GamePanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
			this.GamePanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
			this.GamePanel.Size = new System.Drawing.Size(484, 419);
			this.GamePanel.TabIndex = 0;
			this.GamePanel.SizeChanged += new System.EventHandler(this.GamePanelSizeChanged);
			// 
			// BoardSizeBar
			// 
			this.BoardSizeBar.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.BoardSizeBar.Location = new System.Drawing.Point(0, 417);
			this.BoardSizeBar.Maximum = 5;
			this.BoardSizeBar.Minimum = 2;
			this.BoardSizeBar.Name = "BoardSizeBar";
			this.BoardSizeBar.Size = new System.Drawing.Size(484, 45);
			this.BoardSizeBar.TabIndex = 1;
			this.BoardSizeBar.Value = 3;
			this.BoardSizeBar.ValueChanged += new System.EventHandler(this.BoardSizeChanged);
			// 
			// Game
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.Black;
			this.ClientSize = new System.Drawing.Size(484, 462);
			this.Controls.Add(this.BoardSizeBar);
			this.Controls.Add(this.GamePanel);
			this.MinimumSize = new System.Drawing.Size(500, 500);
			this.Name = "Game";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			((System.ComponentModel.ISupportInitialize)(this.BoardSizeBar)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel GamePanel;
        private System.Windows.Forms.TrackBar BoardSizeBar;
    }
}