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



namespace MenuGUI
{
    public partial class PortalForm : Form
    {
        private List<string> spreadsheet_list;
        private SocketState server_socket;
        private Boolean menu = true;
        SpreadsheetForm spreadsheet_form;

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
                    //ServerNameTextBox.Text = "Server Name";
                    //ServerNameTextBox.ForeColor = Color.FromArgb(117, 117, 117);
                }
                catch
                {
                    MessageBox.Show("Unable to connect to server.");
                    LoginButton.Enabled = true;
                    ServerNameTextBox.Enabled = true;
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
            //this.Hide();
            MenuForm mf = new MenuForm(spreadsheet_list, server_socket);
            mf.ShowDialog();
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
            // create_menu();
        }


        /// <summary>
        /// Processes incoming data and adds it to the message buffer in the socket state
        /// </summary>
        /// <param name="ss"></param>
        private void ProcessMessage(SocketState ss)
        {
            if(spreadsheet_form != null && !spreadsheet_form.IsDisposed) {
                spreadsheet_form.ProcessMessage(ss);
                return;
            }
            //ss.sb.Clear();
            string data = ss.sb.ToString();
            //check if it's a "Connection_Accepted" message
            if (data.StartsWith("connect"))
            {
                //remove "connect_accepted"
                data = data.Substring(17);
                spreadsheet_list = data.Split('\n').ToList<string>();
                spreadsheet_list.RemoveAt(spreadsheet_list.Count - 1);
                if (menu)
                {
                    create_menu();
                    menu = false;
                }
                   

            }//check if it's a "Connection_Accepted" message
            else if (data.StartsWith("full_state"))
            {
                List<string> cells = data.Substring(11).Split('\n').ToList<string>();
                cells.RemoveAt(cells.Count - 1);
                create_spreadsheet(cells);
            }
            Console.WriteLine(data);
            ss.sb.Remove(0, data.Length);
            Networking.GetData(ss);
        }

        private void create_spreadsheet(List<string> cells)
        {
            spreadsheet_form = new SpreadsheetForm(server_socket);
            spreadsheet_form.load_spreadsheet(cells);

            spreadsheet_form.ShowDialog();
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
                    LoginButton.Enabled = false;
                    ServerNameTextBox.Enabled = false;
                    menu = true;
                    try
                    {
                        Networking.ConnectToServer(firstContact, ServerNameTextBox.Text);
                    }
                    catch
                    {

                    }
                    LoginButton.Enabled = true;
                    ServerNameTextBox.Enabled = true;
                    //ServerNameTextBox.Text = "Server Name";
                    //ServerNameTextBox.ForeColor = Color.FromArgb(117, 117, 117);
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
                    LoginButton.Enabled = false;
                    ServerNameTextBox.Enabled = false;
                    menu = true;
                    try
                    {
                        Networking.ConnectToServer(firstContact, ServerNameTextBox.Text);
                    }
                    catch
                    {

                    }
                    LoginButton.Enabled = true;
                    ServerNameTextBox.Enabled = true;
                    //ServerNameTextBox.Text = "Server Name";
                    //ServerNameTextBox.ForeColor = Color.FromArgb(117, 117, 117);
                }
            }
        }
    }
}
