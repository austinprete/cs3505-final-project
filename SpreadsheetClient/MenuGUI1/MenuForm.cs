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
using Microsoft.VisualBasic;
using SpreadsheetGUI;

namespace MenuGUI
{
    public partial class MenuForm : Form
    {
        private SocketState SocketState;
        List<string> spreadsheet_names;
        private bool LoggedOut = false;
        CreateNewGUI.CreateNewForm CNF;
        private string CurrentSpreadsheetName;


        public MenuForm(List<string> names, SocketState ss)
        {
            spreadsheet_names = names;
            SocketState = ss;
            SocketState.callMe = MenuForm_ProcessMessage;
            InitializeComponent();
            SpreadsheetListBox.DataSource = spreadsheet_names;
        }


        private void MenuForm_Load(object sender, EventArgs e)
        {
            this.Size = new System.Drawing.Size(710, 490);
        }


        private void MenuForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (SocketState.theSocket.Connected)
            {
                SocketState.theSocket.Disconnect(true);
                SocketState.theSocket.Close();
            }
            if (!LoggedOut)
                Application.Exit();
        }


        private void LogOutButton_Click(object sender, EventArgs e)
        {
            LoggedOut = true;
            Networking.Send(SocketState, "disconnect ");
            Close();
        }


        private void LogOutButton_KeyPress(object sender, KeyPressEventArgs e)
        {
            LogOutButton_Click(sender, e);
        }


        private void LogOutButton_MouseEnter(object sender, EventArgs e)
        {
            LogOutButton.BackColor = Color.FromArgb(239, 239, 239);
        }


        private void LogOutButton_MouseLeave(object sender, EventArgs e)
        {
            LogOutButton.BackColor = Color.FromArgb(187, 187, 187);
        }


        private void SpreadsheetListBox_DoubleClick(object sender, EventArgs e)
        {
            LoadButton_Click(sender, e);
        }


        private void SpreadsheetListBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == Convert.ToChar(Keys.Enter))
            {
                LoadButton_Click(sender, e);
            }
        }


        private void LoadButton_Click(object sender, EventArgs e)
        {
            CurrentSpreadsheetName = (string)SpreadsheetListBox.SelectedValue;

            Networking.Send(SocketState, "load " + CurrentSpreadsheetName);
            SocketState.sb.Clear();
            Networking.GetData(SocketState);
        }


        private void LoadButton_KeyPress(object sender, KeyPressEventArgs e)
        {
            LoadButton_Click(sender, e);
        }


        private void LoadButton_MouseEnter(object sender, EventArgs e)
        {
            LoadButton.BackColor = Color.FromArgb(239, 239, 239);
        }


        private void LoadButton_MouseLeave(object sender, EventArgs e)
        {
            LoadButton.BackColor = Color.FromArgb(187, 187, 187);
        }


        private void CreateNewButton_Click(object sender, EventArgs e)
        {
            CNF = new CreateNewGUI.CreateNewForm();
            CNF.ShowDialog();
            CurrentSpreadsheetName = CNF.Get_SpreadsheetNameTextBox_Text();

            Networking.Send(SocketState, "load " + CurrentSpreadsheetName);
            SocketState.sb.Clear();
            Networking.GetData(SocketState);
        }


        private void CreateNewButton_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == Convert.ToChar(Keys.Enter))
            {
                CreateNewButton_Click(sender, e);
            }
        }


        private void CreateNewButton_MouseEnter(object sender, EventArgs e)
        {
            CreateNewButton.BackColor = Color.FromArgb(239, 239, 239);
        }


        private void CreateNewButton_MouseLeave(object sender, EventArgs e)
        {
            CreateNewButton.BackColor = Color.FromArgb(187, 187, 187);
        }








        



        
      
        
        
        

        
        


        

        

        

        
        /// <summary>
        /// receive message from server 
        /// </summary>
        /// <param name="ss"></param>
        private void MenuForm_ProcessMessage(SocketState ss)
        {
            
            string allData = ss.sb.ToString();
            string[] parts = allData.Split((Char)3);
            
            if (parts.Length == 1)
            {
                return;
            }

            foreach (string data in parts)
            {
                if (data == ((Char)3).ToString())
                {
                    ss.sb.Remove(0, data.Length);
                    continue;
                }
                
                //check if it's a "Connection_Accepted" message
                if (data.StartsWith("full_state"))
                {
                    List<string> cells = data.Substring(11).Split('\n').ToList<string>();
                    cells.RemoveAt(cells.Count - 1);
                    create_spreadsheet(cells);
                }
                else if (data.StartsWith("connect_accepted"))
                {
                    string data_substring = data.Substring(17);

                    List<string> names = data_substring.Split('\n').ToList<string>();
                    names.RemoveAt(names.Count - 1);

                    UpdateSpreadsheetList(names);

                }
                Console.WriteLine(data);
                if (ss.sb.Length >= data.Length)
                    ss.sb.Remove(0, data.Length);
            }
            Networking.GetData(ss);
        }
        

        private void UpdateSpreadsheetList(List<string> names)
        {
            //SpreadsheetListBox.DataSource = null;

            //spreadsheet_names.Clear();
            
            //foreach (string n in names)
            //{
            //    spreadsheet_names.Add(n);
            //}

            //SpreadsheetListBox.DataSource = spreadsheet_names;
        }

        private void create_spreadsheet(List<string> cells)
        {
            SpreadsheetForm ssf;
            ssf = new SpreadsheetForm(SocketState, spreadsheet_closed, CurrentSpreadsheetName);
            ssf.load_spreadsheet(cells);

            ssf.ShowDialog();
        }

        private void spreadsheet_closed(SocketState socket)
        {
            Networking.ConnectToServer(Reconnect, "lab1-11.eng.utah.edu");
            //socket_state = socket;
            //socket_state.callMe = MenuForm_ProcessMessage;
            //Application.Exit();
        }

        private void Reconnect(SocketState ss)
        {
            SocketState = ss;
            SocketState.callMe = MenuForm_ProcessMessage;

            Networking.Send(SocketState, "register ");
            Networking.GetData(SocketState);
        }


        public void SetServerNameLabel(string serverName)
        {
            ServerNameLabel.Text = serverName;
        }
    }
}
