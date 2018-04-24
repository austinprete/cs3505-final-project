using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Network;



namespace MenuGUI
{
    public partial class PortalForm : Form
    {
        private List<string> SpreadsheetList;
        private SocketState ServerSocket;
        private bool CreateMenu;
        private bool MenuClosed;
        

        public PortalForm()
        {
            InitializeComponent();
            SpreadsheetList = new List<string>();
        }


        private void PortalForm_Click(object sender, EventArgs e)
        {
            if (ServerNameTextBox.Text.Length == 0)
            {
                ServerNameTextBox.Text = "Server Name";
                ServerNameTextBox.ForeColor = Color.FromArgb(117, 117, 117);
            }
        }


        private void PortalForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            for (int i = 0; i < Networking.connectedSockets.Count; i++)
            {
                Networking.connectedSockets[i].Close();
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


        private void ServerNameTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == Convert.ToChar(Keys.Enter))
                LoginButton_Click(sender, e);
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


        private void LoginButton_Click(object sender, EventArgs e)
        {
            if (ServerNameTextBox.Text != "Server Name" && ServerNameTextBox.Text.Length != 0)
            {
                CreateMenu = true;
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


        private void LoginButton_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == Convert.ToChar(Keys.Enter))
                LoginButton_Click(sender, e);
        }
        

        private void LoginButton_MouseEnter(object sender, EventArgs e)
        {
            LoginButton.BackColor = Color.FromArgb(239, 239, 239);
        }


        private void LoginButton_MouseLeave(object sender, EventArgs e)
        {
            LoginButton.BackColor = Color.FromArgb(187, 187, 187);
        }





        private void firstContact(SocketState ss)
        {
            ServerSocket = ss;
            ServerSocket.callMe = ProcessMessage;
            Networking.Send(ServerSocket, "register");
            Networking.GetData(ServerSocket);
        }


        private void ProcessMessage(SocketState ss)
        {
            string allData = ss.sb.ToString();
            string[] parts = allData.Split((Char)3);

            if (parts.Length == 1)
                return;

            foreach (string currentData in parts)
            {
                if (currentData == ((Char)3).ToString())
                {
                    ss.sb.Remove(0, currentData.Length);
                    continue;
                }

                string data = currentData;
                //check if it's a "Connection_Accepted" message

                if (data.StartsWith("connect"))
                {
                    //remove "connect_accepted"
                    data = data.Substring(17);
                    SpreadsheetList = data.Split('\n').ToList<string>();
                    SpreadsheetList.RemoveAt(SpreadsheetList.Count - 1);
                    if (CreateMenu)
                    {
                        CreateMenu = false;
                        create_menu();
                    }


                }//check if it's a "Connection_Accepted" message

                Console.WriteLine(data);
                if (ss.sb.Length >= data.Length)
                    ss.sb.Remove(0, data.Length);

                if (data.StartsWith("disconnect"))
                    Show();
            }
            if (!MenuClosed)
                Networking.Send(ServerSocket, "disconnect ");
        }
        

        private void create_menu()
        {
            MenuForm mf = new MenuForm(SpreadsheetList, ServerSocket);
            mf.SetServerNameLabel(ServerNameTextBox.Text);
            mf.ShowDialog();
            MenuClosed = true;
        }
    }
}