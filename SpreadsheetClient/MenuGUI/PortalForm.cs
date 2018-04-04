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
    public partial class PortalForm : Form
    {
        public PortalForm()
        {
            InitializeComponent();
        }

        private void PortalForm_Click(object sender, EventArgs e)
        {
            if (ServerNameTextBox.Text.Length == 0)
            {
                ServerNameTextBox.Text = "Server Name";
                ServerNameTextBox.ForeColor = Color.FromArgb(117, 117, 117);
            }
        }

        private void ServerNameTextBox_Click(object sender, EventArgs e)
        {
            if (ServerNameTextBox.Text == "Server Name")
            {
                ServerNameTextBox.Text = "";
                ServerNameTextBox.ForeColor = SystemColors.ControlText;
            }
        }

        private void ServerNameTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (ServerNameTextBox.Text == "Server Name")
            {
                ServerNameTextBox.Text = "";
                ServerNameTextBox.ForeColor = SystemColors.ControlText;
            }
        }

        private void LoginButton_Click(object sender, EventArgs e)
        {
            if (ServerNameTextBox.Text.Length == 0)
            {
                ServerNameTextBox.Text = "Server Name";
                ServerNameTextBox.ForeColor = Color.FromArgb(117, 117, 117);
            }
            else
            {
                //Networking.ConnectToServer();
            }
        }

        private void LoginButton_MouseEnter(object sender, EventArgs e)
        {
            LoginButton.BackColor = Color.FromArgb(239, 239, 239);
        }

        private void LoginButton_MouseLeave(object sender, EventArgs e)
        {
            LoginButton.BackColor = Color.FromArgb(187, 187, 187);
        }

    }
}
