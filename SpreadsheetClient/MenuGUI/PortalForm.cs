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
        private SocketState theServer;
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
                LoginButton.BackColor = Color.FromArgb(105, 109, 192);
                ServerNameTextBox.Text = "Server Name";
                ServerNameTextBox.ForeColor = Color.FromArgb(117, 117, 117);
            }
            else if (ServerNameTextBox.Text != "Server Name")
            {
                LoginButton.BackColor = Color.FromArgb(105, 105, 105);
                Networking.ConnectToServer(firstContact, ServerNameTextBox.Text);
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
        /// <summary>
        /// login works when you hit enter
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ServerNameTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                LoginButton.BackColor = Color.FromArgb(50, 50, 50);
                Networking.ConnectToServer(firstContact, ServerNameTextBox.Text);
            }
        }
        /// <summary>
        /// begin connection with server
        /// </summary>
        /// <param name="socketstate"></param>
        private void firstContact(SocketState socketstate)
        {
            theServer = socketstate;
            theServer.callMe = ProcessMessage;
            Networking.Send(theServer, "register");
            Networking.GetData(theServer);
        }


        /// <summary>
        /// Processes incoming data and adds it to the message buffer in the socket state
        /// </summary>
        /// <param name="ss"></param>
        private void ProcessMessage(SocketState ss)
        {
            string data = ss.sb.ToString();
            //check if it's a "Connection_Accepted" message
            if (data.Substring(0, 7) == "connect")
            {
                //remove "connect_accepted"
                data = data.Substring(17);
                string[] spreadsheet_list = data.Split('\n');
                ListBox list_box = new ListBox();
                list_box.DataSource = data;
            }
            Console.WriteLine(data);
            ss.sb.Remove(0, data.Length);
            Networking.GetData(ss);
        }

        private void PortalForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            for (int i = 0; i < Networking.connectedSockets.Count; i++)
            {
                Networking.connectedSockets[i].Close();
            }
        }
    }
}
