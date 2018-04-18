﻿using System;
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
        SpreadsheetForm spreadsheet;
        private SocketState socket_state;
        List<string> spreadsheet_names;
        private bool LoggedOut = false;
        CreateNewGUI.CreateNewForm CNF;

        public bool GetLoggedOut()
        {
            return LoggedOut;
        }



        public MenuForm(List<string> names, SocketState ss)
        {
            spreadsheet_names = names;
            this.socket_state = ss;
            socket_state.callMe = MenuForm_ProcessMessage;
            InitializeComponent();
            foreach (string n in names)
            {
                SpreadsheetListBox.Items.Add(n);
            }
            SpreadsheetListBox.DataSource = spreadsheet_names;
        }

        /// <summary>
        /// load the menu form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MenuForm_Load(object sender, EventArgs e)
        {
            this.Size = new System.Drawing.Size(710, 490);
        }

        /// <summary>
        /// actions when mouse enters log out button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LogOutButton_MouseEnter(object sender, EventArgs e)
        {
            LogOutButton.BackColor = Color.FromArgb(239, 239, 239);
        }

        /// <summary>
        /// action when mouse leave logout button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LogOutButton_MouseLeave(object sender, EventArgs e)
        {
            LogOutButton.BackColor = Color.FromArgb(187, 187, 187);
        }

        /// <summary>
        /// action when mouse enters load button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LoadButton_MouseEnter(object sender, EventArgs e)
        {
            LoadButton.BackColor = Color.FromArgb(239, 239, 239);
        }

        /// <summary>
        /// action when mouse leaves loadbutton
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
            Networking.Send(socket_state, "load " + name);
            socket_state.sb.Clear();
            Networking.GetData(socket_state);
        }

        private void LogOutButton_Click(object sender, EventArgs e)
        {
            LoggedOut = true;
            Networking.Send(socket_state, "disconnect ");
           Close();
        }

        /// <summary>
        /// should disconnect the socket with the server
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void MenuForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (socket_state.theSocket.Connected)
            {
                socket_state.theSocket.Disconnect(true);
                socket_state.theSocket.Close();
            }
            if (!LoggedOut)
                Application.Exit();
        }
        /// <summary>
        /// receive message from server 
        /// </summary>
        /// <param name="ss"></param>
        private void MenuForm_ProcessMessage(SocketState ss)
        {
            string data = ss.sb.ToString();
            //check if it's a "Connection_Accepted" message
            if (data.StartsWith("full_state"))
            {
                List<string> cells = data.Substring(11).Split('\n').ToList<string>();
                cells.RemoveAt(cells.Count - 1);
                create_spreadsheet(cells);
            }
            Console.WriteLine(data);
            ss.sb.Remove(0, data.Length);
            Networking.GetData(ss);
        }

        /// <summary>
        /// creates a spreadsheet
        /// </summary>
        /// <param name="cells"></param>
        private void create_spreadsheet(List<string> cells)
        {
            spreadsheet = new SpreadsheetForm(socket_state);
            spreadsheet.load_spreadsheet(cells);

            spreadsheet.ShowDialog();
        }

        private void CreateNewButton_Click(object sender, EventArgs e) {
            CNF = new CreateNewGUI.CreateNewForm();
            CNF.ShowDialog();
            string name = CNF.Get_SpreadsheetNameTextBox_Text();


            Networking.Send(socket_state, "load " + name);
            socket_state.sb.Clear();
            Networking.GetData(socket_state);
        }

        //private void CloseCreateNew(object sender, EventArgs e) {
        //    createNewForm.Close();
        //}

        private void LoadButton_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            //Same as click, fill when finished
        }

        private void CreateNewButton_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            //Same as click, fill when finished
        }

        private void LogOutButton_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            //Same as click, fill when finished
        }
    }
}
