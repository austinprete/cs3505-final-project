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

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void connectButton_Click(object sender, EventArgs e)
        {
            //Networking.ConnectToServer([NetworkAction], serverNameTextBox);
        }
    }
}
