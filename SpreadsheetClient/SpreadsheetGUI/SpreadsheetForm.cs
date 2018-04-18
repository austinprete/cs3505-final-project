﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using SpreadsheetUtilities;
using SS;
using Network;
using System.Diagnostics;
using System.Threading;


namespace SpreadsheetGUI
{
    /// <summary>
    /// The primary form displayed for the spreadsheet program.
    /// </summary>
    public partial class SpreadsheetForm : Form
    {
        // Provides the backend logic for the spreadsheet GUI
        private Spreadsheet spreadsheet;

        // The following variables are utilized for the extra feature.
        private List<Keys> lastKeyPresses = new List<Keys>();
        private static List<Keys> konamiCode = new List<Keys> { Keys.Up, Keys.Up, Keys.Down, Keys.Down, Keys.Left, Keys.Right, Keys.Left, Keys.Right, Keys.B, Keys.A };

        private SocketState serverSocket;

        private bool timeout;
        private double current_time;

        private Stopwatch stopwatch = new Stopwatch();

        int pingDelay;

        private string name;

        //these variables are to keep track of what commands were pressed
        //private bool revert, undo, edit;
        /// <summary>
        /// The constructor for the spreadsheet form, performs the intial
        /// set up such as setting event handlers.
        /// </summary>
        public SpreadsheetForm(SocketState socket)
        {
            InitializeComponent();

            serverSocket = socket;
            serverSocket.callMe = ProcessMessage;

            //name = spreadsheet_name;

            pingDelay = 0;

            Size = new Size(1200, 600);

            // Assign event handlers
            spreadsheetPanel1.SelectionChanged += CellSelected;
            spreadsheetPanel1.enterDel = EnterPressedOnPanel;
            spreadsheetPanel1.startEditingCell = StartEditingCell;
            FormClosing += SpreadsheetFormClosing;
            CellContentsTextBox.KeyDown += KeyDownHandler;

            // Instantiate the backing Spreadsheet instance
            spreadsheet = new Spreadsheet(s => true, s => s.ToUpper(), "CHANGE ME");

            // Sets control focus to the cell contents text box at startup
            //ActiveControl = CellContentsTextBox;

            // Sets the UI to display the information for cell "A1" initially
            DisplayCellInfo(0, 0);
            timeout = false;
            current_time = 0;

            //var thread = new Thread(send_ping);
            //thread.Start();

            System.Timers.Timer timer = new System.Timers.Timer();
            timer.Interval = 10000;
            timer.Elapsed += new System.Timers.ElapsedEventHandler(send_ping);
            timer.Enabled = true;

        }
        /// <summary>
        /// send ping every 60 sec
        /// </summary>
        private void send_ping(object source, System.Timers.ElapsedEventArgs e)
        {
            pingDelay += 10;
            if (pingDelay == 60)//Connection is dead, terminate it
            {
                TerminateConnection();
            }
            Networking.Send(serverSocket, "ping ");
            
        }
        /// <summary>
        /// A command was received from the server, we need to process it
        /// </summary>
        /// <param name="ss"></param>
        public void ProcessMessage(SocketState ss) 
        {
            string data = ss.sb.ToString();
            string[] parts = data.Split((Char)3);

            //It's an incomplete message, wait for later
            if (parts.Length == 1) {
                return;
            }
            
            foreach (string p in parts) {
                if (p == "" || p == ((Char)3).ToString()) {
                    ss.sb.Remove(0, 1);
                    continue;
                }

                if (p.StartsWith("ping_response")) {
                    timeout = false;
                    pingDelay = 0;
                    ss.sb.Remove(0,p.Length);
                } else if (p.StartsWith("ping")) {
                    Networking.Send(serverSocket, "ping_response ");
                    ss.sb.Remove(0, p.Length);
                }
            }
        }

        private void TerminateConnection()
        {
            Networking.Send(serverSocket, "disconnect ");
            serverSocket.theSocket.Close();
        }

        private void StartEditingCell()
        {
            spreadsheetPanel1.GetSelection(out int col, out int row);
            Networking.Send(serverSocket, "focus " + ConvertColRowToName(col, row));
        }

        /// <summary>
        /// Is called from spreadsheet panel and indicates that the enter button was pressed
        /// </summary>
        private void EnterPressedOnPanel()
        {
            spreadsheetPanel1.GetSelection(out int col, out int row);
            Networking.Send(serverSocket, "unfocus ");
            Networking.Send(serverSocket, "edit " + ConvertColRowToName(col, row) + ":" + spreadsheet.GetCellContents(ConvertColRowToName(col, row)).ToString());
            Networking.GetData(serverSocket);
            //EnterButton_Click(this, EventArgs.Empty);
        }

        /// <summary>
        /// When a cell is selected
        /// </summary>
        /// <param name="sender"></param>
        private void CellSelected(SpreadsheetPanel sender)
        {
            // If an error message for another cell is being displayed, this hides it.
            ErrorMsgBox.Visible = false;

            // Gets the coordinates for the currently selected cell from the spreadsheet panel.
            sender.GetSelection(out int col, out int row);

            object originalObj = spreadsheet.GetCellContents(ConvertColRowToName(col, row));
            string originalString = spreadsheet.GetCellContents(ConvertColRowToName(col, row)).ToString();
            if (originalObj as Formula != null)
            {
                originalString = "=" + originalString;
            }
            sender.SetValue(col, row, originalString);


            // Update the UI to show the currently selected cell's information
            DisplayCellInfo(col, row);
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

            Object cellValue = spreadsheet.GetCellValue(variableName);

            // If the value is a formula error we display the reason for it
            if (cellValue is FormulaError)
            {
                CellValueTextBox.Text = ((FormulaError)cellValue).Reason;
            }
            else // otherwise, we display the default string representation of the value
            {
                CellValueTextBox.Text = cellValue.ToString();
            }

            Object cellContents = spreadsheet.GetCellContents(variableName);
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
        /// The event handler for when the enter button has been clicked. Updates the 
        /// currently selected cell's contents with the input of the contents text box.
        /// 
        /// Note: this is also called for a key press of the enter key from the KeyDownHandler()
        /// method. Normally this could be done by setting the AcceptButton to the EnterButton,
        /// but our additional feature introduced some complexity with that approach.
        /// </summary>
        /// <param name="sender">the sender of the event</param>
        /// <param name="e">the arguments of the event</param>
        private void EnterButton_Click(object sender, EventArgs e)
        {
            // If the ErrorMsgBox was being displayed, it should now be hidden.
            ErrorMsgBox.Visible = false;

            spreadsheetPanel1.GetSelection(out int col, out int row);
            string variableName = ConvertColRowToName(col, row);
            spreadsheetPanel1.GetValue(col, row, out string contents);
            Networking.Send(serverSocket, "edit " + variableName + ":" + contents);
            

            /*
            try
            {
                // Attempts to set the contents of the cell to the user's input in the cell contents text box
                spreadsheetPanel1.GetValue(col, row, out string value);
                ISet<string> dependents = spreadsheet.SetContentsOfCell(variableName, value);

                ConvertNameToColRow(variableName, out int dependentCol, out int dependentRow);

                // Update the displayed cell info for the newly modified cell 
                DisplayCellInfo(dependentCol, dependentRow);
                // Updates the displayed values of each of the dependent cells (this includes the modified cell)
                UpdateDependentCells(dependents);
            }
            catch (Exception exception)
            {
                if (exception is FormulaFormatException || exception is InvalidNameException ||
                    exception is CircularException)
                {
                    // Since no value can be computed, the cell shouldn't ever be set on the GUI
                    spreadsheet.SetContentsOfCell(variableName, "");

                    // Show the error message box and change the text to indicate the current cell
                    // contains an invalid formula.
                    ErrorMsgBox.Visible = true;
                    ErrorMsgBox.Text = String.Format("{0} contains an invalid formula", variableName);
                }
                else // Any other exception should be thrown
                {
                    throw;
                }
            }*/
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

                Object cellValue = spreadsheet.GetCellValue(dependent);

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
        /// Click event handler for the "New" menu button, opens a new spreadsheet window.
        /// <param name="sender">the sender of the event</param>
        /// <param name="e">the event arguments</param>
        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Open a new spreadsheet window
            //new SpreadsheetForm().Show();
        }

        /// <summary>
        /// Click event handler for the "Save" button of the menu, opens a save file dialog
        /// for the user to choose a location to save the spreadsheet to.
        /// </summary>
        /// <param name="sender">the sender of the event</param>
        /// <param name="e">the event arguments</param>
        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Create a new save file dialog that defaults to the .sprd extension
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.DefaultExt = "sprd";
            dialog.Filter = "Spreadsheet Files (*.sprd)|*.sprd|All files (*.*)|*.*";

            // If the dialog result is OK, attempt to save the spreadsheet to the chosen location
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    // Attempt to save the spreadsheet to disk.
                    spreadsheet.Save(dialog.FileName);
                }
                catch (SpreadsheetReadWriteException)
                {
                    // Display a message box alerting the user that the file couldn't be saved.
                    MessageBox.Show("Couldn't save file", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        /// <summary>
        /// Click event handler for the "Open" button of the menu. Opens a dialog window
        /// for the user to chose a spreadsheet file to open in the editor. 
        /// </summary>
        /// <param name="sender">the sender of the event</param>
        /// <param name="e">the event arguments</param>
        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Create a new open file dialog with filter options for the .sprd extension and all files
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "Spreadsheet Files (*.sprd)|*.sprd|All files (*.*)|*.*";

            // If the result of the dialog is OK, attempt to load spreadsheet file and update UI
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                UpdateSpreadsheetUIFromFile(dialog.FileName);
            }
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
                if (spreadsheet.Changed)
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
                spreadsheet = new Spreadsheet(filename, s => true, s => s.ToUpper(), "ps6");

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

        /// <summary>
        /// This method allows the user to close the spreadhseet in any other way
        /// other than the close menu item.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        /// <summary>
        /// This method keeps track of the closing event on the spreadsheet.
        /// If changes have been made to the spreadsheet, a dialog box will open asking
        /// the user if they still want to exit without saving. If the user clicks no,
        /// the closing event is cancelled.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SpreadsheetFormClosing(object sender, FormClosingEventArgs e)
        {
            /*if (spreadsheet.Changed)
            {
                DialogResult result = MessageBox.Show(
                    "Current spreadsheet has unsaved changes. Do you want to exit without saving?",
                    "WARNING", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (result.Equals(DialogResult.No))
                {
                    e.Cancel = true; //cancels the form closing event
                    return;
                }
            }*/
        }

        /// <summary>
        /// This method will show the HelpForm when the help menu item is clicked.
        /// Creates a new HelpForm object and shows the dialog box.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void helpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            HelpForm form = new HelpForm();

            form.ShowDialog();
        }

        /// <summary>
        /// This method keeps track of the extra feature. During spreadsheet use
        /// This method keeps track of the last 10 key presses the user entered
        /// into the contents text box on the spreadsheet. When the user presses
        /// enter, this method checks if the user's last 10 key presses match the 
        /// konami code (list that contains a specific sequence of key presses). 
        /// If the last key presses match the code, our extra feature form is activated.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void KeyDownHandler(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) //checks if enter is pressed
            {
                if (lastKeyPresses.Count == 10 && lastKeyPresses.SequenceEqual(konamiCode)) //checks if there were 10 key presses and if they match the konami code
                {
                    lastKeyPresses.Clear();
                    ExtraFeatureForm rick = new ExtraFeatureForm();
                    rick.Roll(); //shows the ExtraFeatureForm 
                    return;
                }
                else
                {
                    EnterButton_Click(sender, null); //If the code is not entered, this line makes sure the enter key follows its enter key logic
                }
            }

            lastKeyPresses.Add(e.KeyCode); //adds the key press into the list

            if (lastKeyPresses.Count > 10)
            {
                lastKeyPresses.RemoveAt(0); //always removes from the list when the number of key presses exceeds 10.
            }

        }

        private void undoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //send undo to server
        }

        private void revertToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //sent revert to server 
        }
         
        private void SpreadsheetForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            TerminateConnection();
        }

        public void load_spreadsheet(List<string> cells)
        {
            foreach(string cell_n_contents in cells)
            {
                string[] split = cell_n_contents.Split(':');
                spreadsheet.SetContentsOfCell(split[0], split[1]);

                ISet<string> dependents = spreadsheet.SetContentsOfCell(split[0], split[1]);

                ConvertNameToColRow(split[0], out int dependentCol, out int dependentRow);

                // Update the displayed cell info for the newly modified cell 
                DisplayCellInfo(dependentCol, dependentRow);
                // Updates the displayed values of each of the dependent cells (this includes the modified cell)
                UpdateDependentCells(dependents);
            }
        }

        private void SpreadsheetForm_KeyPress(object sender, KeyPressEventArgs e) {
            /*if (e.KeyChar == Convert.ToChar(Keys.Enter)) {
                EnterButton_Click(this, EventArgs.Empty);
            }*/
        }

        private void spreadsheetPanel1_KeyPress(object sender, KeyPressEventArgs e) {
            if (e.KeyChar == Convert.ToChar(Keys.Enter)) {
                EnterButton_Click(this, EventArgs.Empty);
            }
        }
    }
}
