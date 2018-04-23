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
using System.Diagnostics;
using SpreadsheetGUI;
using System.Threading;



namespace MenuGUI
{
    public partial class PortalForm : Form
    {
        private List<string> spreadsheet_list;
        private SocketState server_socket;
        private Boolean menu = true;
        private MenuForm mf;
        private Boolean menuClosed = false;
        

        public PortalForm()
        {
            InitializeComponent();
            spreadsheet_list = new List<string>();
        }

        private void PortalForm_Click(object sender, EventArgs e)
        {
            if (ServerNameTextBox.Text.Length == 0)
            {
                ServerNameTextBox.Text = "Server Name";
                ServerNameTextBox.ForeColor = Color.FromArgb(117, 117, 117);
            }
        }

        /// <summary>
        /// connect with server after login
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LoginButton_Click(object sender, EventArgs e)
        {
            if (ServerNameTextBox.Text != "Server Name")
            {
                menu = true;
                LoginButton.Enabled = false;
                ServerNameTextBox.Enabled = false;
                try
                {
                    Networking.ConnectToServer(firstContact, ServerNameTextBox.Text);
                    //WindowState = FormWindowState.Minimized;
                }
                catch
                {
                    MessageBox.Show("Unable to connect to server.");
                    LoginButton.Enabled = true;
                    ServerNameTextBox.Enabled = true;
                    //WindowState = FormWindowState.Normal;
                }
                LoginButton.Enabled = true;
                ServerNameTextBox.Enabled = true;
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

        private void create_menu()
        {
            mf = new MenuForm(spreadsheet_list, server_socket);
            mf.Text = "Spreadsheet Application -- " + ServerNameTextBox.Text;
            mf.SetServerNameLabel(ServerNameTextBox.Text);
            mf.ShowDialog();
            menuClosed = true;
        }

        

        /// <summary>
        /// begin connection with server
        /// </summary>
        /// <param name="socketstate"></param>
        private void firstContact(SocketState socketstate)
        {
            server_socket = socketstate;
            server_socket.callMe = ProcessMessage;
            Networking.Send(server_socket, "register");
            Networking.GetData(server_socket);
        }
        
        /// <summary>
        /// Processes incoming data and adds it to the message buffer in the socket state
        /// </summary>
        /// <param name="ss"></param>
        private void ProcessMessage(SocketState ss)
        {
            string allData = ss.sb.ToString();
            string[] parts = allData.Split((Char)3);

            if (parts.Length == 1) {
                return;
            }

            foreach (string currentData in parts) {
                
                if (currentData == ((Char)3).ToString()) {
                    ss.sb.Remove(0, currentData.Length);
                    continue;
                }
                string data = currentData;
                //check if it's a "Connection_Accepted" message
                if (data.StartsWith("connect")) {
                    //remove "connect_accepted"
                    data = data.Substring(17);
                    spreadsheet_list = data.Split('\n').ToList<string>();
                    spreadsheet_list.RemoveAt(spreadsheet_list.Count - 1);
                    if (menu) {
                        menu = false;
                        create_menu();
                    }


                }//check if it's a "Connection_Accepted" message

                Console.WriteLine(data);
                ss.sb.Remove(0, data.Length);

                if (data.StartsWith("disconnect"))
                    Show();
            }
            if (!menuClosed)
                Networking.GetData(ss);
        }



        private void PortalForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            for (int i = 0; i < Networking.connectedSockets.Count; i++)
            {
                Networking.connectedSockets[i].Close();
            }
        }

        private void ServerNameTextBox_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (ServerNameTextBox.Text != "Server Name" && ServerNameTextBox.Text.Length != 0)
                {
                    LoginButton_Click(sender, e);
                }
            }
        }

        private void ServerNameTextBox_MouseClick(object sender, MouseEventArgs e)
        {
            if (ServerNameTextBox.Text == "Server Name")
            {
                ServerNameTextBox.Text = "";
                ServerNameTextBox.ForeColor = SystemColors.ControlText;
            }
        }

        private void ServerNameTextBox_Enter(object sender, EventArgs e)
        {
            if (ServerNameTextBox.Text == "Server Name")
            {
                ServerNameTextBox.Text = "";
                ServerNameTextBox.ForeColor = SystemColors.ControlText;
            }
        }

        private void ServerNameTextBox_Leave(object sender, EventArgs e)
        {
            if (ServerNameTextBox.Text.Length == 0)
            {
                ServerNameTextBox.Text = "Server Name";
                ServerNameTextBox.ForeColor = Color.FromArgb(117, 117, 117);
            }
        }

        private void LoginButton_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (ServerNameTextBox.Text != "Server Name" && ServerNameTextBox.Text.Length != 0)
                {
                    LoginButton_Click(sender, e);
                }
            }
        }
    }
}
