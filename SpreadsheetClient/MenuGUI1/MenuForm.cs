using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MenuGUI
{
    public partial class MenuForm : Form
    {
        public MenuForm()
        {
            InitializeComponent();
        }

        private void MenuForm_Load(object sender, EventArgs e)
        {
            this.Size = new System.Drawing.Size(710, 490);
            //ServerNameLabel.Text
        }

        private void LogOutButton_MouseEnter(object sender, EventArgs e)
        {
            LogOutButton.BackColor = Color.FromArgb(239, 239, 239);
        }

        private void LogOutButton_MouseLeave(object sender, EventArgs e)
        {
            LogOutButton.BackColor = Color.FromArgb(187, 187, 187);
        }

        private void LoadButton_MouseEnter(object sender, EventArgs e)
        {
            LoadButton.BackColor = Color.FromArgb(239, 239, 239);
        }

        private void LoadButton_MouseLeave(object sender, EventArgs e)
        {
            LoadButton.BackColor = Color.FromArgb(187, 187, 187);
        }

        private void CreateNewButton_MouseEnter(object sender, EventArgs e)
        {
            CreateNewButton.BackColor = Color.FromArgb(239, 239, 239);
        }

        private void CreateNewButton_MouseLeave(object sender, EventArgs e)
        {
            CreateNewButton.BackColor = Color.FromArgb(187, 187, 187);
        }
    }
}
