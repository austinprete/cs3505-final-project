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
            this.MeetTheDevsLinkLabel = new System.Windows.Forms.LinkLabel();
            this.SuspendLayout();
            // 
            // SpreadsheetListBox
            // 
            this.SpreadsheetListBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.SpreadsheetListBox.Font = new System.Drawing.Font("Arial", 10.875F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SpreadsheetListBox.ForeColor = System.Drawing.SystemColors.ControlLight;
            this.SpreadsheetListBox.FormattingEnabled = true;
            this.SpreadsheetListBox.ItemHeight = 33;
            this.SpreadsheetListBox.Location = new System.Drawing.Point(525, 49);
            this.SpreadsheetListBox.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.SpreadsheetListBox.Name = "SpreadsheetListBox";
            this.SpreadsheetListBox.ScrollAlwaysVisible = true;
            this.SpreadsheetListBox.Size = new System.Drawing.Size(827, 598);
            this.SpreadsheetListBox.Sorted = true;
            this.SpreadsheetListBox.TabIndex = 0;
            this.SpreadsheetListBox.DoubleClick += new System.EventHandler(this.SpreadsheetListBox_DoubleClick);
            this.SpreadsheetListBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.SpreadsheetListBox_KeyPress);
            // 
            // ServerNameLabel
            // 
            this.ServerNameLabel.AutoSize = true;
            this.ServerNameLabel.Font = new System.Drawing.Font("Arial", 16.125F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ServerNameLabel.Location = new System.Drawing.Point(12, 12);
            this.ServerNameLabel.Name = "ServerNameLabel";
            this.ServerNameLabel.Size = new System.Drawing.Size(446, 51);
            this.ServerNameLabel.TabIndex = 1;
            this.ServerNameLabel.Text = "lab1-11.eng.utah.edu";
            // 
            // LogOutButton
            // 
            this.LogOutButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(187)))), ((int)(((byte)(187)))), ((int)(((byte)(187)))));
            this.LogOutButton.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LogOutButton.Location = new System.Drawing.Point(21, 66);
            this.LogOutButton.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.LogOutButton.Name = "LogOutButton";
            this.LogOutButton.Size = new System.Drawing.Size(165, 65);
            this.LogOutButton.TabIndex = 2;
            this.LogOutButton.Text = "Log Out";
            this.LogOutButton.UseVisualStyleBackColor = false;
            this.LogOutButton.Click += new System.EventHandler(this.LogOutButton_Click);
            this.LogOutButton.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.LogOutButton_KeyPress);
            this.LogOutButton.MouseEnter += new System.EventHandler(this.LogOutButton_MouseEnter);
            this.LogOutButton.MouseLeave += new System.EventHandler(this.LogOutButton_MouseLeave);
            // 
            // LoadButton
            // 
            this.LoadButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(187)))), ((int)(((byte)(187)))), ((int)(((byte)(187)))));
            this.LoadButton.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LoadButton.Location = new System.Drawing.Point(525, 660);
            this.LoadButton.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.LoadButton.Name = "LoadButton";
            this.LoadButton.Size = new System.Drawing.Size(135, 65);
            this.LoadButton.TabIndex = 3;
            this.LoadButton.Text = "Load";
            this.LoadButton.UseVisualStyleBackColor = false;
            this.LoadButton.Click += new System.EventHandler(this.LoadButton_Click);
            this.LoadButton.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.LoadButton_KeyPress);
            this.LoadButton.MouseEnter += new System.EventHandler(this.LoadButton_MouseEnter);
            this.LoadButton.MouseLeave += new System.EventHandler(this.LoadButton_MouseLeave);
            // 
            // CreateNewButton
            // 
            this.CreateNewButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(187)))), ((int)(((byte)(187)))), ((int)(((byte)(187)))));
            this.CreateNewButton.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CreateNewButton.Location = new System.Drawing.Point(676, 660);
            this.CreateNewButton.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.CreateNewButton.Name = "CreateNewButton";
            this.CreateNewButton.Size = new System.Drawing.Size(208, 65);
            this.CreateNewButton.TabIndex = 4;
            this.CreateNewButton.Text = "Create New";
            this.CreateNewButton.UseVisualStyleBackColor = false;
            this.CreateNewButton.Click += new System.EventHandler(this.CreateNewButton_Click);
            this.CreateNewButton.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.CreateNewButton_KeyPress);
            this.CreateNewButton.MouseEnter += new System.EventHandler(this.CreateNewButton_MouseEnter);
            this.CreateNewButton.MouseLeave += new System.EventHandler(this.CreateNewButton_MouseLeave);
            // 
            // SpreadsheetInformationLabel
            // 
            this.SpreadsheetInformationLabel.AutoSize = true;
            this.SpreadsheetInformationLabel.Font = new System.Drawing.Font("Arial", 10.125F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SpreadsheetInformationLabel.Location = new System.Drawing.Point(520, 12);
            this.SpreadsheetInformationLabel.Name = "SpreadsheetInformationLabel";
            this.SpreadsheetInformationLabel.Size = new System.Drawing.Size(532, 32);
            this.SpreadsheetInformationLabel.TabIndex = 5;
            this.SpreadsheetInformationLabel.Text = "Select a spreadsheet or start from scratch.";
            // 
            // MeetTheDevsLinkLabel
            // 
            this.MeetTheDevsLinkLabel.ActiveLinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(119)))), ((int)(((byte)(119)))), ((int)(((byte)(119)))));
            this.MeetTheDevsLinkLabel.AutoSize = true;
            this.MeetTheDevsLinkLabel.Font = new System.Drawing.Font("Arial", 10.125F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MeetTheDevsLinkLabel.LinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(119)))), ((int)(((byte)(119)))), ((int)(((byte)(119)))));
            this.MeetTheDevsLinkLabel.Location = new System.Drawing.Point(599, 773);
            this.MeetTheDevsLinkLabel.Name = "MeetTheDevsLinkLabel";
            this.MeetTheDevsLinkLabel.Size = new System.Drawing.Size(192, 32);
            this.MeetTheDevsLinkLabel.TabIndex = 6;
            this.MeetTheDevsLinkLabel.TabStop = true;
            this.MeetTheDevsLinkLabel.Text = "Meet the Devs";
            this.MeetTheDevsLinkLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.MeetTheDevsLinkLabel.VisitedLinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(119)))), ((int)(((byte)(119)))), ((int)(((byte)(119)))));
            // 
            // MenuForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(238)))), ((int)(((byte)(238)))), ((int)(((byte)(238)))));
            this.ClientSize = new System.Drawing.Size(1924, 810);
            this.Controls.Add(this.MeetTheDevsLinkLabel);
            this.Controls.Add(this.SpreadsheetInformationLabel);
            this.Controls.Add(this.CreateNewButton);
            this.Controls.Add(this.LoadButton);
            this.Controls.Add(this.LogOutButton);
            this.Controls.Add(this.ServerNameLabel);
            this.Controls.Add(this.SpreadsheetListBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "MenuForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Spreadsheet Menu";
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
        private System.Windows.Forms.LinkLabel MeetTheDevsLinkLabel;
    }
}

