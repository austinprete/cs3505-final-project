namespace MenuGUI
{
    partial class MenuForm
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
            this.SpreadsheetListBox = new System.Windows.Forms.ListBox();
            this.ServerNameLabel = new System.Windows.Forms.Label();
            this.LogOutButton = new System.Windows.Forms.Button();
            this.LoadButton = new System.Windows.Forms.Button();
            this.CreateNewButton = new System.Windows.Forms.Button();
            this.SpreadsheetInformationLabel = new System.Windows.Forms.Label();
            this.TeamInformationLinkLabel = new System.Windows.Forms.LinkLabel();
            this.SuspendLayout();
            // 
            // SpreadsheetListBox
            // 
            this.SpreadsheetListBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.SpreadsheetListBox.ForeColor = System.Drawing.SystemColors.InactiveCaption;
            this.SpreadsheetListBox.FormattingEnabled = true;
            this.SpreadsheetListBox.ItemHeight = 20;
            this.SpreadsheetListBox.Location = new System.Drawing.Point(394, 39);
            this.SpreadsheetListBox.Margin = new System.Windows.Forms.Padding(2);
            this.SpreadsheetListBox.Name = "SpreadsheetListBox";
            this.SpreadsheetListBox.Size = new System.Drawing.Size(621, 504);
            this.SpreadsheetListBox.TabIndex = 0;
            // 
            // ServerNameLabel
            // 
            this.ServerNameLabel.AutoSize = true;
            this.ServerNameLabel.Font = new System.Drawing.Font("Arial", 16.125F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ServerNameLabel.Location = new System.Drawing.Point(9, 10);
            this.ServerNameLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.ServerNameLabel.Name = "ServerNameLabel";
            this.ServerNameLabel.Size = new System.Drawing.Size(339, 38);
            this.ServerNameLabel.TabIndex = 1;
            this.ServerNameLabel.Text = "lab1-11.eng.utah.edu";
            // 
            // LogOutButton
            // 
            this.LogOutButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(187)))), ((int)(((byte)(187)))), ((int)(((byte)(187)))));
            this.LogOutButton.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LogOutButton.Location = new System.Drawing.Point(16, 53);
            this.LogOutButton.Margin = new System.Windows.Forms.Padding(2);
            this.LogOutButton.Name = "LogOutButton";
            this.LogOutButton.Size = new System.Drawing.Size(124, 52);
            this.LogOutButton.TabIndex = 2;
            this.LogOutButton.Text = "Log Out";
            this.LogOutButton.UseVisualStyleBackColor = false;
            this.LogOutButton.Click += new System.EventHandler(this.LogOutButton_Click);
            this.LogOutButton.MouseEnter += new System.EventHandler(this.LogOutButton_MouseEnter);
            this.LogOutButton.MouseLeave += new System.EventHandler(this.LogOutButton_MouseLeave);
            // 
            // LoadButton
            // 
            this.LoadButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(187)))), ((int)(((byte)(187)))), ((int)(((byte)(187)))));
            this.LoadButton.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LoadButton.Location = new System.Drawing.Point(394, 554);
            this.LoadButton.Margin = new System.Windows.Forms.Padding(2);
            this.LoadButton.Name = "LoadButton";
            this.LoadButton.Size = new System.Drawing.Size(101, 52);
            this.LoadButton.TabIndex = 3;
            this.LoadButton.Text = "Load";
            this.LoadButton.UseVisualStyleBackColor = false;
            this.LoadButton.Click += new System.EventHandler(this.LoadButton_Click);
            this.LoadButton.MouseEnter += new System.EventHandler(this.LoadButton_MouseEnter);
            this.LoadButton.MouseLeave += new System.EventHandler(this.LoadButton_MouseLeave);
            // 
            // CreateNewButton
            // 
            this.CreateNewButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(187)))), ((int)(((byte)(187)))), ((int)(((byte)(187)))));
            this.CreateNewButton.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CreateNewButton.Location = new System.Drawing.Point(506, 554);
            this.CreateNewButton.Margin = new System.Windows.Forms.Padding(2);
            this.CreateNewButton.Name = "CreateNewButton";
            this.CreateNewButton.Size = new System.Drawing.Size(156, 52);
            this.CreateNewButton.TabIndex = 4;
            this.CreateNewButton.Text = "Create New";
            this.CreateNewButton.UseVisualStyleBackColor = false;
            this.CreateNewButton.Click += new System.EventHandler(this.CreateNewButton_Click);
            this.CreateNewButton.MouseEnter += new System.EventHandler(this.CreateNewButton_MouseEnter);
            this.CreateNewButton.MouseLeave += new System.EventHandler(this.CreateNewButton_MouseLeave);
            // 
            // SpreadsheetInformationLabel
            // 
            this.SpreadsheetInformationLabel.AutoSize = true;
            this.SpreadsheetInformationLabel.Font = new System.Drawing.Font("Arial", 10.125F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SpreadsheetInformationLabel.Location = new System.Drawing.Point(390, 10);
            this.SpreadsheetInformationLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.SpreadsheetInformationLabel.Name = "SpreadsheetInformationLabel";
            this.SpreadsheetInformationLabel.Size = new System.Drawing.Size(410, 24);
            this.SpreadsheetInformationLabel.TabIndex = 5;
            this.SpreadsheetInformationLabel.Text = "Select a spreadsheet or start from scratch.";
            // 
            // TeamInformationLinkLabel
            // 
            this.TeamInformationLinkLabel.ActiveLinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(119)))), ((int)(((byte)(119)))), ((int)(((byte)(119)))));
            this.TeamInformationLinkLabel.AutoSize = true;
            this.TeamInformationLinkLabel.Font = new System.Drawing.Font("Arial", 10.125F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TeamInformationLinkLabel.LinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(119)))), ((int)(((byte)(119)))), ((int)(((byte)(119)))));
            this.TeamInformationLinkLabel.Location = new System.Drawing.Point(446, 651);
            this.TeamInformationLinkLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.TeamInformationLinkLabel.Name = "TeamInformationLinkLabel";
            this.TeamInformationLinkLabel.Size = new System.Drawing.Size(147, 24);
            this.TeamInformationLinkLabel.TabIndex = 6;
            this.TeamInformationLinkLabel.TabStop = true;
            this.TeamInformationLinkLabel.Text = "Meet the Devs";
            this.TeamInformationLinkLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.TeamInformationLinkLabel.VisitedLinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(119)))), ((int)(((byte)(119)))), ((int)(((byte)(119)))));
            // 
            // MenuForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(238)))), ((int)(((byte)(238)))), ((int)(((byte)(238)))));
            this.ClientSize = new System.Drawing.Size(1443, 840);
            this.Controls.Add(this.TeamInformationLinkLabel);
            this.Controls.Add(this.SpreadsheetInformationLabel);
            this.Controls.Add(this.CreateNewButton);
            this.Controls.Add(this.LoadButton);
            this.Controls.Add(this.LogOutButton);
            this.Controls.Add(this.ServerNameLabel);
            this.Controls.Add(this.SpreadsheetListBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "MenuForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Spreadsheet Application";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MenuForm_FormClosing);
            this.Load += new System.EventHandler(this.MenuForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox SpreadsheetListBox;
        private System.Windows.Forms.Label ServerNameLabel;
        private System.Windows.Forms.Button LogOutButton;
        private System.Windows.Forms.Button LoadButton;
        private System.Windows.Forms.Button CreateNewButton;
        private System.Windows.Forms.Label SpreadsheetInformationLabel;
        private System.Windows.Forms.LinkLabel TeamInformationLinkLabel;
    }
}

