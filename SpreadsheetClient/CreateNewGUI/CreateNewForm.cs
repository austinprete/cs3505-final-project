using System;
using System.Drawing;
using System.Windows.Forms;

namespace CreateNewGUI
{
    public partial class CreateNewForm : Form
    {
        public CreateNewForm()
        {
            InitializeComponent();
        }


        private void CreateNewForm_Load(object sender, EventArgs e)
        {
            Width = 400;
            Height = 175;
        }


        public string GetSpreadsheetNameTextBox_Text()
        {
            return SpreadsheetNameTextBox.Text;
        }


        private void CreateButton_Click(object sender, EventArgs e)
        {
            Close();
        }


        private void CreateButton_KeyPress(object sender, KeyPressEventArgs e)
        {
            CreateButton_Click(sender, e);
        }


        private void CreateButton_MouseEnter(object sender, EventArgs e)
        {
            CreateButton.BackColor = Color.FromArgb(239, 239, 239);
        }


        private void CreateButton_MouseLeave(object sender, EventArgs e)
        {
            CreateButton.BackColor = Color.FromArgb(187, 187, 187);
        }
    }
}
