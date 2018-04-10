using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MenuGUI1
{
    public partial class MenuForm : Form
    {
        public MenuForm()
        {
            //this.Size = new Size(900, 200);
            InitializeComponent();
        }

        private void MenuForm_Load(object sender, EventArgs e)
        {
            this.Size = new System.Drawing.Size(800, 533);

        }
    }
}
