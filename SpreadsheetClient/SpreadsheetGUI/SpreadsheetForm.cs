using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using SpreadsheetUtilities;
using SS;
using Network;
using System.Diagnostics;
using System.Net.Sockets;

namespace SpreadsheetGUI
{
    /// <summary>
    /// The primary form displayed for the spreadsheet program.
    /// </summary>
    public partial class SpreadsheetForm : Form
    {
        // Provides the backend logic for the spreadsheet GUI
        private Spreadsheet Spread;

        // The following variables are utilized for the extra feature.
        private List<Keys> lastKeyPresses = new List<Keys>();
        private static List<Keys> konamiCode = new List<Keys> { Keys.Up, Keys.Up, Keys.Down, Keys.Down, Keys.Left, Keys.Right, Keys.Left, Keys.Right, Keys.B, Keys.A };

        private SocketState ServerSocket;
        private Stopwatch StopWat = new Stopwatch();
        private System.Timers.Timer timer;
        private int PingDelay;
        private bool isEditing = false;
        public delegate void SpreadsheetCloseDelegate(SocketState ss);
        private SpreadsheetCloseDelegate closeDel;



        //these variables are to keep track of what commands were pressed
        //private bool revert, undo, edit;
        /// <summary>
        /// The constructor for the spreadsheet form, performs the intial
        /// set up such as setting event handlers.
        /// </summary>
        public SpreadsheetForm(SocketState socket, SpreadsheetCloseDelegate ss, string name)
        {
            InitializeComponent();
            Text = name;
            closeDel = ss;

            ServerSocket = socket;
            ServerSocket.callMe = Spreadsheet_ProcessMessage;
            Networking.GetData(ServerSocket);

            PingDelay = 0;
            Size = new Size(1200, 600);

            // Assign event handlers
            spreadsheetPanel1.SelectionChanged += CellSelected;
            spreadsheetPanel1.EnterDel = SpreadsheetForm_EnterPress;
            spreadsheetPanel1.UpDel = SpreadsheetForm_UpPress;
            spreadsheetPanel1.LeftDel = SpreadsheetForm_LeftPress;
            spreadsheetPanel1.RightDel = SpreadsheetForm_RightPress;
            spreadsheetPanel1.StartEditingCellDel = StartEditingCell;
            spreadsheetPanel1.LeaveCellDel = leavecell;

            // Instantiate the backing Spreadsheet instance
            Spread = new Spreadsheet(s => true, s => s.ToUpper(), "CHANGE ME");

            // Sets the UI to display the information for cell "A1" initially
            DisplayCellInfo(0, 0);

            timer = new System.Timers.Timer();
            timer.Interval = 10000;
            timer.Elapsed += new System.Timers.ElapsedEventHandler(send_ping);
            timer.Enabled = true;
        }


        private void SpreadsheetForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            TerminateConnection();
            timer.Enabled = false;
            closeDel(ServerSocket);
        }


        private void SpreadsheetForm_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == Convert.ToChar(Keys.Enter))
                SpreadsheetForm_EnterPress();
            else if (e.KeyChar == Convert.ToChar(Keys.Escape))
            {
                Close();
                //TerminateConnection();
                timer.Enabled = false;
                closeDel(ServerSocket);
            }
            else
                Networking.GetData(ServerSocket);
        }

        
        private void SpreadsheetForm_EnterPress()
        {
            spreadsheetPanel1.GetSelection(out int col, out int row);

            string variableName = ConvertColRowToName(col, row);

            // spreadsheet.SetContentsOfCell(variableName, t);
            //Networking.Send(serverSocket, "unfocus ");
            send_edit_to_server(ServerSocket, "unfocus ");
            isEditing = false;


            spreadsheetPanel1.GetValue(col, row, out string contents);
            System.Diagnostics.Debug.WriteLine("CLIENT: edit " + variableName + ":" + contents);
            cell_edit_to_server(ServerSocket, variableName, contents);

            spreadsheetPanel1.SetSelection(col, row + 1);
            Networking.GetData(ServerSocket);
        }


        private void SpreadsheetForm_UpPress()
        {
            spreadsheetPanel1.GetSelection(out int col, out int row);
            if (row != 0)
            {
                string variableName = ConvertColRowToName(col, row);

                // spreadsheet.SetContentsOfCell(variableName, t);
                // Networking.Send(serverSocket, "unfocus ");
                send_edit_to_server(ServerSocket, "unfocus ");

                isEditing = false;

                spreadsheetPanel1.GetValue(col, row, out string contents);
                System.Diagnostics.Debug.WriteLine("edit " + variableName + ":" + contents);
                cell_edit_to_server(ServerSocket, variableName, contents);

                spreadsheetPanel1.SetSelection(col, row - 1);
                Networking.GetData(ServerSocket);
            }
        }


        private void SpreadsheetForm_LeftPress()
        {
            spreadsheetPanel1.GetSelection(out int col, out int row);
            string variableName = ConvertColRowToName(col, row);

            // spreadsheet.SetContentsOfCell(variableName, t);
            // Networking.Send(serverSocket, "unfocus ");
            send_edit_to_server(ServerSocket, "unfocus ");

            isEditing = false;

            spreadsheetPanel1.GetValue(col, row, out string contents);
            System.Diagnostics.Debug.WriteLine("CLIENT: edit " + variableName + ":" + contents);
            cell_edit_to_server(ServerSocket, variableName, contents);

            spreadsheetPanel1.SetSelection(col + 1, row);
            Networking.GetData(ServerSocket);
        }


        private void SpreadsheetForm_RightPress()
        {
            spreadsheetPanel1.GetSelection(out int col, out int row);
            if (col != 0)
            {
                string variableName = ConvertColRowToName(col, row);

                // spreadsheet.SetContentsOfCell(variableName, t);
                // Networking.Send(serverSocket, "unfocus ");
                send_edit_to_server(ServerSocket, "unfocus ");

                isEditing = false;

                spreadsheetPanel1.GetValue(col, row, out string contents);
                System.Diagnostics.Debug.WriteLine("edit " + variableName + ":" + contents);
                cell_edit_to_server(ServerSocket, variableName, contents);

                spreadsheetPanel1.SetSelection(col - 1, row);
                Networking.GetData(ServerSocket);
            }
        }


        private void HelpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            HelpForm form = new HelpForm();

            form.ShowDialog();
        }


        private void UndoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Networking.Send(ServerSocket, "undo ");
        }


        private void RevertToolStripMenuItem_Click(object sender, EventArgs e)
        {
            spreadsheetPanel1.GetSelection(out int col, out int row);
            string cell_name = ConvertColRowToName(col, row);
            Networking.Send(ServerSocket, "revert " + cell_name);
        }






















        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            spreadsheetPanel1.GetSelection(out int col, out int row);

            if (keyData == Keys.Up)
            {
                //move up row
                row--;
            }
            else if (keyData == Keys.Down)
            {
                //move down row
                row++;
            }
            else if (keyData == Keys.Left)
            {

                //move left column
                col--;
            }
            else if (keyData == Keys.Right || keyData == Keys.Tab)
            {
                //move down column
                col++;
            }
            else
            {
                return base.ProcessCmdKey(ref msg, keyData);
            }

            spreadsheetPanel1.SetSelection(col, row);

            return base.ProcessCmdKey(ref msg, keyData);
        }


        /// <summary>
        /// send ping every 60 sec
        /// </summary>
        private void send_ping(object source, System.Timers.ElapsedEventArgs e)
        {
            PingDelay += 10;
            if (PingDelay == 60)//Connection is dead, terminate it
            {
                TerminateConnection();
            }
            Networking.Send(ServerSocket, "ping ");

            System.Diagnostics.Debug.WriteLine("CLIENT: ping");
        }
        /// <summary>
        /// A command was received from the server, we need to process it
        /// </summary>
        /// <param name="ss"></param>
        public void Spreadsheet_ProcessMessage(SocketState ss)
        {
            //lock (spreadsheet)
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

                    //It's an incomplete message, wait for later
                    if (data.StartsWith("ping_response"))
                    {
                        PingDelay = 0;
                    }
                    else if (data.StartsWith("ping") && data.Length < 12)
                    {
                        // Networking.Send(serverSocket, "ping_response ");
                        send_ping_response();
                    }
                    else if (data.StartsWith("change "))
                    {
                        string cellName = data.Substring("change ".Length, data.IndexOf(":") - "change ".Length);
                        try
                        {
                            string cellContents = data.Substring(data.IndexOf(":") + 1);
                            ISet<string> dependents = Spread.SetContentsOfCell(cellName, cellContents);

                            ConvertNameToColRow(cellName, out int dependentCol, out int dependentRow);

                            // Update the displayed cell info for the newly modified cell
                            MethodInvoker invoker = new MethodInvoker(() => DisplayCellInfo(dependentCol, dependentRow));
                            //DisplayCellInfo(dependentCol, dependentRow);
                            // Updates the displayed values of each of the dependent cells (this includes the modified cell)
                            UpdateDependentCells(dependents);
                        }
                        catch (CircularException)
                        {
                            string cellContents = "";
                            MessageBox.Show("There is a circular dependency. Unacceptable");
                            cell_edit_to_server(ServerSocket, cellName, cellContents);
                            ISet<string> dependents = Spread.SetContentsOfCell(cellName, cellContents);
                            ConvertNameToColRow(cellName, out int dependentCol, out int dependentRow);

                            // Update the displayed cell info for the newly modified cell
                            MethodInvoker invoker = new MethodInvoker(() => DisplayCellInfo(dependentCol, dependentRow));
                            //DisplayCellInfo(dependentCol, dependentRow);
                            // Updates the displayed values of each of the dependent cells (this includes the modified cell)
                            UpdateDependentCells(dependents);
                        }
                    }else if (data.StartsWith("focus ")) {
                        
                        string cellName = data.Substring("focus ".Length, data.IndexOf(":") - "focus ".Length);
                        string id = data.Substring(data.IndexOf(":") + 1);

                        spreadsheetPanel1.GetSelection(out int col, out int row);
                        if (cellName != ConvertColRowToName(col, row)) {
                            spreadsheetPanel1.FocusCell(cellName, id);
                        }

                    } else if (data.StartsWith("unfocus")) {
                        string id = data.Substring("unfocus ".Length);
                        spreadsheetPanel1.UnfocusCell(id);
                    }
                    if (data.Length <= ss.sb.Length)
                        ss.sb.Remove(0, data.Length);
                }

                Networking.GetData(ss);
            }
        }

        private void send_ping_response()
        {
            Networking.Send(ServerSocket, "ping_response ");
            System.Diagnostics.Debug.WriteLine("CLIENT: ping_response");
        }
        private void TerminateConnection()
        {
            Networking.Send(ServerSocket, "disconnect ");
            ServerSocket.theSocket.DisconnectAsync(new SocketAsyncEventArgs());
        }

        private void StartEditingCell()
        {
            spreadsheetPanel1.GetSelection(out int col, out int row);
            if (!isEditing)
            {
                Networking.Send(ServerSocket, "focus " + ConvertColRowToName(col, row));
                isEditing = true;
            }
        }


        private void leavecell(int c, int r, string s)
        {
            spreadsheetPanel1.GetSelection(out int col, out int row);

            string variableName = ConvertColRowToName(col, row);

            // spreadsheet.SetContentsOfCell(variableName, t);
            //Networking.Send(serverSocket, "unfocus ");
            send_edit_to_server(ServerSocket, "unfocus ");
            isEditing = false;


            spreadsheetPanel1.GetValue(col, row, out string contents);
            System.Diagnostics.Debug.WriteLine("CLIENT: edit " + variableName + ":" + contents);
            cell_edit_to_server(ServerSocket, variableName, contents);

            //spreadsheetPanel1.SetSelection(col, row + 1);
            //Networking.GetData(serverSocket);
            Networking.GetData(ServerSocket);
        }


        

        private void cell_edit_to_server(SocketState ss, string cellname, string s)
        {
            string cellContents = s;
            try
            {
                ISet<string> dependents = Spread.SetContentsOfCell(cellname, cellContents);

                ConvertNameToColRow(cellname, out int dependentCol, out int dependentRow);
            }
            catch (CircularException)
            {
                cellContents = "";
                MessageBox.Show("There is a circular dependency. Unacceptable");
                //ISet<string> dependents = spreadsheet.SetContentsOfCell(cellname, cellContents);
                ConvertNameToColRow(cellname, out int col, out int row);
                spreadsheetPanel1.SetValue(col, row, cellContents);

            }
            Networking.Send(ss, "edit " + cellname + ":" + cellContents);
        }
        /// <summary>
        /// When a cell is selected
        /// </summary>
        /// <param name="sender"></param>
        private void CellSelected(SpreadsheetPanel sender)
        {

            // spreadsheet.SetContentsOfCell(variableName, t);
            // Networking.Send(serverSocket, "unfocus ");
            send_edit_to_server(ServerSocket, "unfocus ");

            isEditing = false;



            // If an error message for another cell is being displayed, this hides it.
            ErrorMsgBox.Visible = false;

            // Gets the coordinates for the currently selected cell from the spreadsheet panel.
            sender.GetSelection(out int col, out int row);

            object originalObj = Spread.GetCellContents(ConvertColRowToName(col, row));
            string originalString = Spread.GetCellContents(ConvertColRowToName(col, row)).ToString();
            if (originalObj as Formula != null)
            {
                originalString = "=" + originalString;
            }
            sender.SetValue(col, row, originalString);


            // Update the UI to show the currently selected cell's information
            DisplayCellInfo(col, row);

            Networking.GetData(ServerSocket);

        }

        /// <summary>
        /// Updates the user interface to display the info (name, contents, value) for 
        /// the specified cell.
        /// </summary>
        /// <param name="col">the column number of the cell (0 indexed integer)</param>
        /// <param name="row">the row number of the cell (0 indexed integer)</param>
        private void DisplayCellInfo(int col, int row)
        {
            string variableName = ConvertColRowToName(col, row);
            CellNameTextbox.Text = variableName;

            Object cellValue = Spread.GetCellValue(variableName);

            // If the value is a formula error we display the reason for it
            if (cellValue is FormulaError)
            {
                CellValueTextBox.Text = ((FormulaError)cellValue).Reason;
            }
            else // otherwise, we display the default string representation of the value
            {
                CellValueTextBox.Text = cellValue.ToString();
            }

            Object cellContents = Spread.GetCellContents(variableName);
            string contentString = cellContents.ToString();

            // If the contents are a formula, we need to prepend an equals sign
            if (cellContents is Formula)
            {
                contentString = "=" + contentString;
            }

            CellContentsTextBox.Text = contentString;
        }

        /// <summary>
        /// Take a cell's column and row number as input and converts it to a variable name.
        /// 
        /// Note: column 0 corresponds to the letter "A", and row 0 corresonds to the number 1
        /// </summary>
        /// <param name="col">the cell's column number</param>
        /// <param name="row">the cell's row number</param>
        /// <returns>the variable name corresponding to the cell coordinates</returns>
        private string ConvertColRowToName(int col, int row)
        {
            char letter = (char)('A' + col);
            string variableName = string.Format("{0}{1}", letter, row + 1);

            return variableName;
        }
        
        

        /// <summary>
        /// Provided a set of cell names, updates their corresponding cells on the
        /// spreadsheet panel. Used to update the dependents of a modified cell.
        /// </summary>
        /// <param name="dependents"></param>
        private void UpdateDependentCells(ISet<string> dependents)
        {
            foreach (string dependent in dependents)
            {
                // Convert the cell name to the corresponding integer coordinates
                ConvertNameToColRow(dependent, out int col, out int row);

                Object cellValue = Spread.GetCellValue(dependent);

                // If the value of the cell is a formula error, we display an error string
                // in that cell on the spreadsheet panel.
                if (cellValue is FormulaError)
                {
                    spreadsheetPanel1.SetValue(col, row, "#ERROR?");
                }
                else // Otherwise we display the cell value's string representation
                {
                    spreadsheetPanel1.SetValue(col, row, cellValue.ToString());
                }
            }
        }

        /// <summary>
        /// Provided a cell name, sets the col and row out parameters to the corresponding
        /// integer coordinates of the name.
        /// </summary>
        /// <param name="name">the cell variable name</param>
        /// <param name="col">an out parameter that will be initialized to the cell's column number</param>
        /// <param name="row">an out parameter that will be initialized to the cell's row number</param>
        private void ConvertNameToColRow(string name, out int col, out int row)
        {
            col = name.First() - 'A';
            row = Int32.Parse(name.Substring(1).ToString()) - 1;
        }

        /// <summary>
        /// Provided a file name, loads the contents as a spreadsheet and updates UI
        /// </summary>
        /// <param name="filename">the file name of the spreadsheet to open</param>
        private void UpdateSpreadsheetUIFromFile(string filename)
        {
            try
            {
                // Check if the current spreadsheet has been changed
                if (Spread.Changed)
                {
                    DialogResult result = MessageBox.Show(
                        "Current spreadsheet has unsaved changes. Do you want to exit without saving?",
                        "WARNING", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                    // If it has, display a message box asking the user to confirm
                    // that they want to exit without saving
                    if (result.Equals(DialogResult.No))
                    {
                        // If the user choses "No", the new spreadsheet won't be loaded.
                        return;
                    }
                }

                // Attempt to load the specified spreadsheet file as a Spreadsheet instance
                Spread = new Spreadsheet(filename, s => true, s => s.ToUpper(), "ps6");

                // For every cell displayed in our UI, see if that cell is set in the loaded
                // spreadsheet and update the UI as necessary.

                HashSet<string> cellsToUpdate = new HashSet<string>();
                for (int col = 0; col <= 25; col++)
                {
                    for (int row = 0; row <= 98; row++)
                    {
                        string variableName = ConvertColRowToName(col, row);
                        cellsToUpdate.Add(variableName);
                    }
                }

                UpdateDependentCells(cellsToUpdate);

                // Set the selected cell to "A1"
                spreadsheetPanel1.SetSelection(0, 0);
                Networking.Send(ServerSocket, "unfocus ");
                DisplayCellInfo(0, 0);
            }
            catch (SpreadsheetReadWriteException)
            {
                // If the spreadsheet file couldn't be loaded, display an error message box to the user.
                MessageBox.Show(
                    String.Format("Couldn't open spreadsheet at location {0}", filename),
                    "ERROR",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        
        public void load_spreadsheet(List<string> cells)
        {
            foreach (string cell_n_contents in cells)
            {
                string[] split = cell_n_contents.Split(':');
                Spread.SetContentsOfCell(split[0], split[1]);

                ISet<string> dependents = Spread.SetContentsOfCell(split[0], split[1]);

                ConvertNameToColRow(split[0], out int dependentCol, out int dependentRow);

                // Update the displayed cell info for the newly modified cell 
                MethodInvoker invoker = new MethodInvoker(() => DisplayCellInfo(dependentCol, dependentRow));
                //DisplayCellInfo(dependentCol, dependentRow);
                // Updates the displayed values of each of the dependent cells (this includes the modified cell)
                UpdateDependentCells(dependents);
            }
        }

        private void send_edit_to_server(SocketState ss, string s)
        {
            Networking.Send(ss, s);
        }
    }
}
