﻿namespace SpreadsheetGUI
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
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.closeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.panel1 = new System.Windows.Forms.Panel();
            this.ErrorMsgBox = new System.Windows.Forms.TextBox();
            this.EnterButton = new System.Windows.Forms.Button();
            this.CellContentsTextBox = new System.Windows.Forms.TextBox();
            this.spreadsheetPanel1 = new SS.SpreadsheetPanel();
            this.CellValueTextBox = new System.Windows.Forms.TextBox();
            this.CellNameTextbox = new System.Windows.Forms.TextBox();
            this.menuStrip1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newToolStripMenuItem,
            this.saveToolStripMenuItem,
            this.openToolStripMenuItem,
            this.closeToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(64, 36);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // newToolStripMenuItem
            // 
            this.newToolStripMenuItem.Name = "newToolStripMenuItem";
            this.newToolStripMenuItem.Size = new System.Drawing.Size(268, 38);
            this.newToolStripMenuItem.Text = "New";
            this.newToolStripMenuItem.Click += new System.EventHandler(this.newToolStripMenuItem_Click);
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(268, 38);
            this.saveToolStripMenuItem.Text = "Save";
            this.saveToolStripMenuItem.Click += new System.EventHandler(this.saveToolStripMenuItem_Click);
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.Size = new System.Drawing.Size(268, 38);
            this.openToolStripMenuItem.Text = "Open";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
            // 
            // closeToolStripMenuItem
            // 
            this.closeToolStripMenuItem.Name = "closeToolStripMenuItem";
            this.closeToolStripMenuItem.Size = new System.Drawing.Size(268, 38);
            this.closeToolStripMenuItem.Text = "Close";
            this.closeToolStripMenuItem.Click += new System.EventHandler(this.closeToolStripMenuItem_Click);
            // 
            // menuStrip1
            // 
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(36, 36);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Padding = new System.Windows.Forms.Padding(4, 2, 0, 2);
            this.menuStrip1.Size = new System.Drawing.Size(1366, 40);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(77, 36);
            this.helpToolStripMenuItem.Text = "Help";
            this.helpToolStripMenuItem.Click += new System.EventHandler(this.helpToolStripMenuItem_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.ErrorMsgBox);
            this.panel1.Controls.Add(this.EnterButton);
            this.panel1.Controls.Add(this.CellContentsTextBox);
            this.panel1.Controls.Add(this.spreadsheetPanel1);
            this.panel1.Controls.Add(this.CellValueTextBox);
            this.panel1.Controls.Add(this.CellNameTextbox);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 40);
            this.panel1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1366, 791);
            this.panel1.TabIndex = 2;
            // 
            // ErrorMsgBox
            // 
            this.ErrorMsgBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ErrorMsgBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.ErrorMsgBox.Enabled = false;
            this.ErrorMsgBox.Location = new System.Drawing.Point(10, 62);
            this.ErrorMsgBox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.ErrorMsgBox.Name = "ErrorMsgBox";
            this.ErrorMsgBox.ReadOnly = true;
            this.ErrorMsgBox.Size = new System.Drawing.Size(1344, 31);
            this.ErrorMsgBox.TabIndex = 5;
            this.ErrorMsgBox.Visible = false;
            // 
            // EnterButton
            // 
            this.EnterButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.EnterButton.Location = new System.Drawing.Point(1178, 12);
            this.EnterButton.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.EnterButton.Name = "EnterButton";
            this.EnterButton.Size = new System.Drawing.Size(176, 42);
            this.EnterButton.TabIndex = 4;
            this.EnterButton.Text = "Enter";
            this.EnterButton.UseVisualStyleBackColor = true;
            this.EnterButton.Click += new System.EventHandler(this.EnterButton_Click);
            // 
            // CellContentsTextBox
            // 
            this.CellContentsTextBox.AcceptsReturn = true;
            this.CellContentsTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.CellContentsTextBox.Location = new System.Drawing.Point(740, 12);
            this.CellContentsTextBox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.CellContentsTextBox.Name = "CellContentsTextBox";
            this.CellContentsTextBox.Size = new System.Drawing.Size(432, 31);
            this.CellContentsTextBox.TabIndex = 3;
            // 
            // spreadsheetPanel1
            // 
            this.spreadsheetPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.spreadsheetPanel1.Location = new System.Drawing.Point(10, 112);
            this.spreadsheetPanel1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.spreadsheetPanel1.Name = "spreadsheetPanel1";
            this.spreadsheetPanel1.Size = new System.Drawing.Size(1354, 679);
            this.spreadsheetPanel1.TabIndex = 2;
            // 
            // CellValueTextBox
            // 
            this.CellValueTextBox.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.CellValueTextBox.Enabled = false;
            this.CellValueTextBox.Location = new System.Drawing.Point(104, 12);
            this.CellValueTextBox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.CellValueTextBox.Name = "CellValueTextBox";
            this.CellValueTextBox.ReadOnly = true;
            this.CellValueTextBox.Size = new System.Drawing.Size(622, 31);
            this.CellValueTextBox.TabIndex = 1;
            // 
            // CellNameTextbox
            // 
            this.CellNameTextbox.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.CellNameTextbox.Enabled = false;
            this.CellNameTextbox.Location = new System.Drawing.Point(10, 12);
            this.CellNameTextbox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.CellNameTextbox.Name = "CellNameTextbox";
            this.CellNameTextbox.ReadOnly = true;
            this.CellNameTextbox.Size = new System.Drawing.Size(72, 31);
            this.CellNameTextbox.TabIndex = 0;
            // 
            // SpreadsheetForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1366, 831);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.menuStrip1);
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "SpreadsheetForm";
            this.Text = "Spreadsheet Editor";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem newToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem closeToolStripMenuItem;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TextBox CellValueTextBox;
        private System.Windows.Forms.TextBox CellNameTextbox;
        private SS.SpreadsheetPanel spreadsheetPanel1;
        private System.Windows.Forms.TextBox CellContentsTextBox;
        private System.Windows.Forms.Button EnterButton;
        private System.Windows.Forms.TextBox ErrorMsgBox;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
    }
}

