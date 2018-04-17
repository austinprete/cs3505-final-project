namespace CreateNewGUI
{
    partial class CreateNewForm
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
            this.NewSpreadsheetNameLabel = new System.Windows.Forms.Label();
            this.CreateButton = new System.Windows.Forms.Button();
            this.SpreadsheetNameTextBox = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // NewSpreadsheetNameLabel
            // 
            this.NewSpreadsheetNameLabel.AutoSize = true;
            this.NewSpreadsheetNameLabel.Font = new System.Drawing.Font("Arial", 10.125F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.NewSpreadsheetNameLabel.Location = new System.Drawing.Point(185, 34);
            this.NewSpreadsheetNameLabel.Name = "NewSpreadsheetNameLabel";
            this.NewSpreadsheetNameLabel.Size = new System.Drawing.Size(395, 32);
            this.NewSpreadsheetNameLabel.TabIndex = 0;
            this.NewSpreadsheetNameLabel.Text = "Give your spreadsheet a name.";
            // 
            // CreateButton
            // 
            this.CreateButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(187)))), ((int)(((byte)(187)))), ((int)(((byte)(187)))));
            this.CreateButton.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CreateButton.Location = new System.Drawing.Point(316, 160);
            this.CreateButton.Name = "CreateButton";
            this.CreateButton.Size = new System.Drawing.Size(135, 70);
            this.CreateButton.TabIndex = 1;
            this.CreateButton.Text = "Create";
            this.CreateButton.UseVisualStyleBackColor = false;
            this.CreateButton.Click += new System.EventHandler(this.CreateButton_Click);
            this.CreateButton.MouseEnter += new System.EventHandler(this.CreateButton_MouseEnter);
            this.CreateButton.MouseLeave += new System.EventHandler(this.CreateButton_MouseLeave);
            this.CreateButton.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.CreateButton_PreviewKeyDown);
            // 
            // SpreadsheetNameTextBox
            // 
            this.SpreadsheetNameTextBox.BackColor = System.Drawing.Color.White;
            this.SpreadsheetNameTextBox.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SpreadsheetNameTextBox.Location = new System.Drawing.Point(119, 93);
            this.SpreadsheetNameTextBox.Name = "SpreadsheetNameTextBox";
            this.SpreadsheetNameTextBox.Size = new System.Drawing.Size(537, 44);
            this.SpreadsheetNameTextBox.TabIndex = 2;
            // 
            // CreateNewForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(238)))), ((int)(((byte)(238)))), ((int)(((byte)(238)))));
            this.ClientSize = new System.Drawing.Size(511, 177);
            this.Controls.Add(this.SpreadsheetNameTextBox);
            this.Controls.Add(this.CreateButton);
            this.Controls.Add(this.NewSpreadsheetNameLabel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "CreateNewForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Load += new System.EventHandler(this.CreateNewForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label NewSpreadsheetNameLabel;
        private System.Windows.Forms.Button CreateButton;
        private System.Windows.Forms.TextBox SpreadsheetNameTextBox;
    }
}

