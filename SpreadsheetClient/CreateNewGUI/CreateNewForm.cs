using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        private void CreateButton_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void CreateButton_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            // Same as click, fill when complete
        }

        private void CreateButton_MouseEnter(object sender, EventArgs e)
        {
            CreateButton.BackColor = Color.FromArgb(239, 239, 239);
        }

        private void CreateButton_MouseLeave(object sender, EventArgs e)
        {
            CreateButton.BackColor = Color.FromArgb(187, 187, 187);
        }

        public string Get_SpreadsheetNameTextBox_Text()
        {
            return SpreadsheetNameTextBox.Text;
        }
    }
}
