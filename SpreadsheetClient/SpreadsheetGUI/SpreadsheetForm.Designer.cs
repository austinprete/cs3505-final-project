namespace SpreadsheetGUI
{
    partial class SpreadsheetForm
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.ErrorMsgBox = new System.Windows.Forms.TextBox();
            this.CellContentsTextBox = new System.Windows.Forms.TextBox();
            this.CellValueTextBox = new System.Windows.Forms.TextBox();
            this.CellNameTextbox = new System.Windows.Forms.TextBox();
            this.spreadsheetPanel1 = new SS.SpreadsheetPanel();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.undoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.revertToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.panel1.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.ErrorMsgBox);
            this.panel1.Controls.Add(this.CellContentsTextBox);
            this.panel1.Controls.Add(this.spreadsheetPanel1);
            this.panel1.Controls.Add(this.CellValueTextBox);
            this.panel1.Controls.Add(this.CellNameTextbox);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 40);
            this.panel1.Margin = new System.Windows.Forms.Padding(4);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1611, 865);
            this.panel1.TabIndex = 2;
            // 
            // ErrorMsgBox
            // 
            this.ErrorMsgBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ErrorMsgBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.ErrorMsgBox.Enabled = false;
            this.ErrorMsgBox.Location = new System.Drawing.Point(11, 62);
            this.ErrorMsgBox.Margin = new System.Windows.Forms.Padding(4);
            this.ErrorMsgBox.Name = "ErrorMsgBox";
            this.ErrorMsgBox.ReadOnly = true;
            this.ErrorMsgBox.Size = new System.Drawing.Size(1589, 31);
            this.ErrorMsgBox.TabIndex = 5;
            this.ErrorMsgBox.Visible = false;
            // 
            // CellContentsTextBox
            // 
            this.CellContentsTextBox.AcceptsReturn = true;
            this.CellContentsTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.CellContentsTextBox.Enabled = false;
            this.CellContentsTextBox.Location = new System.Drawing.Point(740, 12);
            this.CellContentsTextBox.Margin = new System.Windows.Forms.Padding(4);
            this.CellContentsTextBox.Name = "CellContentsTextBox";
            this.CellContentsTextBox.Size = new System.Drawing.Size(677, 31);
            this.CellContentsTextBox.TabIndex = 3;
            this.CellContentsTextBox.Visible = false;
            // 
            // CellValueTextBox
            // 
            this.CellValueTextBox.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.CellValueTextBox.Enabled = false;
            this.CellValueTextBox.Location = new System.Drawing.Point(104, 12);
            this.CellValueTextBox.Margin = new System.Windows.Forms.Padding(4);
            this.CellValueTextBox.Name = "CellValueTextBox";
            this.CellValueTextBox.ReadOnly = true;
            this.CellValueTextBox.Size = new System.Drawing.Size(623, 31);
            this.CellValueTextBox.TabIndex = 1;
            // 
            // CellNameTextbox
            // 
            this.CellNameTextbox.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.CellNameTextbox.Enabled = false;
            this.CellNameTextbox.Location = new System.Drawing.Point(11, 12);
            this.CellNameTextbox.Margin = new System.Windows.Forms.Padding(4);
            this.CellNameTextbox.Name = "CellNameTextbox";
            this.CellNameTextbox.ReadOnly = true;
            this.CellNameTextbox.Size = new System.Drawing.Size(72, 31);
            this.CellNameTextbox.TabIndex = 0;
            // 
            // spreadsheetPanel1
            // 
            this.spreadsheetPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.spreadsheetPanel1.Location = new System.Drawing.Point(11, 112);
            this.spreadsheetPanel1.Margin = new System.Windows.Forms.Padding(5);
            this.spreadsheetPanel1.Name = "spreadsheetPanel1";
            this.spreadsheetPanel1.Size = new System.Drawing.Size(1600, 753);
            this.spreadsheetPanel1.TabIndex = 2;
            //this.spreadsheetPanel1.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.spreadsheetPanel1_KeyPress);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(77, 36);
            this.helpToolStripMenuItem.Text = "Help";
            this.helpToolStripMenuItem.Click += new System.EventHandler(this.helpToolStripMenuItem_Click);
            // 
            // undoToolStripMenuItem
            // 
            this.undoToolStripMenuItem.Name = "undoToolStripMenuItem";
            this.undoToolStripMenuItem.Size = new System.Drawing.Size(85, 36);
            this.undoToolStripMenuItem.Text = "Undo";
            this.undoToolStripMenuItem.Click += new System.EventHandler(this.undoToolStripMenuItem_Click);
            // 
            // revertToolStripMenuItem
            // 
            this.revertToolStripMenuItem.Name = "revertToolStripMenuItem";
            this.revertToolStripMenuItem.Size = new System.Drawing.Size(94, 36);
            this.revertToolStripMenuItem.Text = "Revert";
            this.revertToolStripMenuItem.Click += new System.EventHandler(this.revertToolStripMenuItem_Click);
            // 
            // menuStrip1
            // 
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(36, 36);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.helpToolStripMenuItem,
            this.undoToolStripMenuItem,
            this.revertToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Padding = new System.Windows.Forms.Padding(4, 2, 0, 2);
            this.menuStrip1.Size = new System.Drawing.Size(1611, 40);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // SpreadsheetForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1611, 905);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.menuStrip1);
            this.KeyPreview = true;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "SpreadsheetForm";
            this.Text = "Spreadsheet Editor";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.SpreadsheetForm_FormClosed);
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.SpreadsheetForm_KeyPress);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TextBox CellValueTextBox;
        private System.Windows.Forms.TextBox CellNameTextbox;
        private SS.SpreadsheetPanel spreadsheetPanel1;
        private System.Windows.Forms.TextBox CellContentsTextBox;
        private System.Windows.Forms.TextBox ErrorMsgBox;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem undoToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem revertToolStripMenuItem;
        private System.Windows.Forms.MenuStrip menuStrip1;
    }
}

