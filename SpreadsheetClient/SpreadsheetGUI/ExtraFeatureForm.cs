using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SpreadsheetGUI
{
    /// <summary>
    /// Partial class of the main form. This contains an extension to our extra feature.
    /// After entering the Konami Code, this form will be activated.
    /// </summary>
    public partial class ExtraFeatureForm : Form
    {

        /// <summary>
        /// Contructor for ExtraFeatureForm.
        /// Initializes form when it is called.
        /// </summary>
        public ExtraFeatureForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// When this is called, the ExtraFeatureForm is revealed to the user.
        /// </summary>
        public void Roll()
        {
            Show();
        }
    }
}
