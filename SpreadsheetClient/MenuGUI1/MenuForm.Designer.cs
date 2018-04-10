namespace MenuGUI1
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
            this.SpreadsheetListBox.FormattingEnabled = true;
            this.SpreadsheetListBox.ItemHeight = 25;
            this.SpreadsheetListBox.Location = new System.Drawing.Point(405, 49);
            this.SpreadsheetListBox.Name = "SpreadsheetListBox";
            this.SpreadsheetListBox.Size = new System.Drawing.Size(1100, 579);
            this.SpreadsheetListBox.TabIndex = 0;
            // 
            // ServerNameLabel
            // 
            this.ServerNameLabel.AutoSize = true;
            this.ServerNameLabel.Font = new System.Drawing.Font("Arial", 16.125F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ServerNameLabel.Location = new System.Drawing.Point(12, 12);
            this.ServerNameLabel.Name = "ServerNameLabel";
            this.ServerNameLabel.Size = new System.Drawing.Size(285, 51);
            this.ServerNameLabel.TabIndex = 1;
            this.ServerNameLabel.Text = "Server Name";
            // 
            // LogOutButton
            // 
            this.LogOutButton.Location = new System.Drawing.Point(21, 66);
            this.LogOutButton.Name = "LogOutButton";
            this.LogOutButton.Size = new System.Drawing.Size(144, 45);
            this.LogOutButton.TabIndex = 2;
            this.LogOutButton.Text = "Log Out";
            this.LogOutButton.UseVisualStyleBackColor = true;
            // 
            // LoadButton
            // 
            this.LoadButton.Location = new System.Drawing.Point(85, 181);
            this.LoadButton.Name = "LoadButton";
            this.LoadButton.Size = new System.Drawing.Size(133, 44);
            this.LoadButton.TabIndex = 3;
            this.LoadButton.Text = "Load";
            this.LoadButton.UseVisualStyleBackColor = true;
            // 
            // CreateNewButton
            // 
            this.CreateNewButton.Location = new System.Drawing.Point(85, 266);
            this.CreateNewButton.Name = "CreateNewButton";
            this.CreateNewButton.Size = new System.Drawing.Size(133, 54);
            this.CreateNewButton.TabIndex = 4;
            this.CreateNewButton.Text = "Create New";
            this.CreateNewButton.UseVisualStyleBackColor = true;
            // 
            // SpreadsheetInformationLabel
            // 
            this.SpreadsheetInformationLabel.AutoSize = true;
            this.SpreadsheetInformationLabel.Font = new System.Drawing.Font("Arial", 10.125F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SpreadsheetInformationLabel.Location = new System.Drawing.Point(400, 12);
            this.SpreadsheetInformationLabel.Name = "SpreadsheetInformationLabel";
            this.SpreadsheetInformationLabel.Size = new System.Drawing.Size(532, 32);
            this.SpreadsheetInformationLabel.TabIndex = 5;
            this.SpreadsheetInformationLabel.Text = "Select a spreadsheet or start from scratch.";
            // 
            // TeamInformationLinkLabel
            // 
            this.TeamInformationLinkLabel.AutoSize = true;
            this.TeamInformationLinkLabel.Font = new System.Drawing.Font("Arial", 10.125F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TeamInformationLinkLabel.Location = new System.Drawing.Point(540, 728);
            this.TeamInformationLinkLabel.Name = "TeamInformationLinkLabel";
            this.TeamInformationLinkLabel.Size = new System.Drawing.Size(192, 32);
            this.TeamInformationLinkLabel.TabIndex = 6;
            this.TeamInformationLinkLabel.TabStop = true;
            this.TeamInformationLinkLabel.Text = "Meet the Devs";
            // 
            // MenuForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1974, 779);
            this.Controls.Add(this.TeamInformationLinkLabel);
            this.Controls.Add(this.SpreadsheetInformationLabel);
            this.Controls.Add(this.CreateNewButton);
            this.Controls.Add(this.LoadButton);
            this.Controls.Add(this.LogOutButton);
            this.Controls.Add(this.ServerNameLabel);
            this.Controls.Add(this.SpreadsheetListBox);
            this.Name = "MenuForm";
            this.Text = "Form1";
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

