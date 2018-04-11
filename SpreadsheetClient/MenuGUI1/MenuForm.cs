using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Network;
namespace MenuGUI
{
    public partial class MenuForm : Form
    {
        private SocketState ss;
        string[] spreadsheet_names;
        private bool LoggedOut = false;


        public MenuForm(string[] names, SocketState ss)
        {
            spreadsheet_names = names;
            this.ss = ss;
            InitializeComponent();
            /*foreach (string n in names)
            {
                SpreadsheetListBox.Items.Add(n);
            }*/
            SpreadsheetListBox.DataSource = spreadsheet_names;
        }

        private void MenuForm_Load(object sender, EventArgs e)
        {
            this.Size = new System.Drawing.Size(710, 490);
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

        private void LoadButton_Click(object sender, EventArgs e)
        {
            string name = (string)SpreadsheetListBox.SelectedValue;
            Networking.Send(ss, "load " + name);
        }

        private void LogOutButton_Click(object sender, EventArgs e)
        {
            LoggedOut = true;
            this.Close();
        }

        private void MenuForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!LoggedOut)
                Application.Exit();
        }
    }
}
