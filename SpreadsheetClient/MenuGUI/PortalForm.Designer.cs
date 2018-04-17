namespace MenuGUI
{
    partial class PortalForm
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
            this.LoginButton = new System.Windows.Forms.Button();
            this.ServerNameTextBox = new System.Windows.Forms.TextBox();
            this.serverLabel = new System.Windows.Forms.Label();
            this.fontDialog1 = new System.Windows.Forms.FontDialog();
            this.TeamNameLabel = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // LoginButton
            // 
            this.LoginButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(187)))), ((int)(((byte)(187)))), ((int)(((byte)(187)))));
            this.LoginButton.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.LoginButton.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LoginButton.ForeColor = System.Drawing.SystemColors.ControlText;
            this.LoginButton.Location = new System.Drawing.Point(149, 158);
            this.LoginButton.Margin = new System.Windows.Forms.Padding(0);
            this.LoginButton.Name = "LoginButton";
            this.LoginButton.Size = new System.Drawing.Size(75, 35);
            this.LoginButton.TabIndex = 0;
            this.LoginButton.Text = "Login";
            this.LoginButton.UseVisualStyleBackColor = false;
            this.LoginButton.Click += new System.EventHandler(this.LoginButton_Click);
            this.LoginButton.MouseEnter += new System.EventHandler(this.LoginButton_MouseEnter);
            this.LoginButton.MouseLeave += new System.EventHandler(this.LoginButton_MouseLeave);
            this.LoginButton.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.LoginButton_PreviewKeyDown);
            // 
            // ServerNameTextBox
            // 
            this.ServerNameTextBox.BackColor = System.Drawing.Color.White;
            this.ServerNameTextBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ServerNameTextBox.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ServerNameTextBox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(117)))), ((int)(((byte)(117)))), ((int)(((byte)(117)))));
            this.ServerNameTextBox.Location = new System.Drawing.Point(45, 120);
            this.ServerNameTextBox.Margin = new System.Windows.Forms.Padding(4);
            this.ServerNameTextBox.Name = "ServerNameTextBox";
            this.ServerNameTextBox.Size = new System.Drawing.Size(281, 44);
            this.ServerNameTextBox.TabIndex = 1;
            this.ServerNameTextBox.Text = "Server Name";
            this.ServerNameTextBox.MouseClick += new System.Windows.Forms.MouseEventHandler(this.ServerNameTextBox_MouseClick);
            this.ServerNameTextBox.Enter += new System.EventHandler(this.ServerNameTextBox_Enter);
            this.ServerNameTextBox.Leave += new System.EventHandler(this.ServerNameTextBox_Leave);
            this.ServerNameTextBox.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.ServerNameTextBox_PreviewKeyDown);
            // 
            // serverLabel
            // 
            this.serverLabel.AutoSize = true;
            this.serverLabel.Location = new System.Drawing.Point(38, 38);
            this.serverLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.serverLabel.Name = "serverLabel";
            this.serverLabel.Size = new System.Drawing.Size(0, 25);
            this.serverLabel.TabIndex = 2;
            // 
            // TeamNameLabel
            // 
            this.TeamNameLabel.AutoSize = true;
            this.TeamNameLabel.Font = new System.Drawing.Font("Arial", 16.125F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TeamNameLabel.Location = new System.Drawing.Point(99, 38);
            this.TeamNameLabel.Name = "TeamNameLabel";
            this.TeamNameLabel.Size = new System.Drawing.Size(354, 51);
            this.TeamNameLabel.TabIndex = 3;
            this.TeamNameLabel.Text = "PB and Jensens";
            this.TeamNameLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Arial", 10.125F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label1.Location = new System.Drawing.Point(37, 74);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(576, 32);
            this.label1.TabIndex = 4;
            this.label1.Text = "Simply type in the desired server\'s IP address.";
            // 
            // PortalForm
            // 
            this.AccessibleRole = System.Windows.Forms.AccessibleRole.None;
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(238)))), ((int)(((byte)(238)))), ((int)(((byte)(238)))));
            this.ClientSize = new System.Drawing.Size(374, 229);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.TeamNameLabel);
            this.Controls.Add(this.serverLabel);
            this.Controls.Add(this.ServerNameTextBox);
            this.Controls.Add(this.LoginButton);
            this.Font = new System.Drawing.Font("Arial", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.KeyPreview = true;
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "PortalForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Spreadsheet Application";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.PortalForm_FormClosing);
            this.Click += new System.EventHandler(this.PortalForm_Click);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button LoginButton;
        private System.Windows.Forms.TextBox ServerNameTextBox;
        private System.Windows.Forms.Label serverLabel;
        private System.Windows.Forms.FontDialog fontDialog1;
        private System.Windows.Forms.Label TeamNameLabel;
        private System.Windows.Forms.Label label1;
    }
}

