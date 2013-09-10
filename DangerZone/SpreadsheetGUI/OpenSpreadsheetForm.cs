using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SS
{
    public partial class OpenSpreadsheetForm : Form
    {
        public event Action<string> open_setup;
        public OpenSpreadsheetForm()
        {
            InitializeComponent();
        }

        /*
         * Checks to make sure that the user cannot click create until both text boxes have text in them
         * 
         * */
        private void OpenFilenameTextbox_TextChanged(object sender, EventArgs e)
        {
            if (OpenFilenameTextbox.Text.Length != 0 && OpenPasswordTextbox.Text.Length != 0)
            {
                JoinButton.Enabled = true;
            }
            else
                JoinButton.Enabled = false;
        }

        /*
         * Checks to make sure that the user cannot click create until both text boxes have text in them
         * 
         * */
        private void OpenPasswordTextbox_TextChanged(object sender, EventArgs e)
        {
            if (OpenFilenameTextbox.Text.Length != 0 && OpenPasswordTextbox.Text.Length != 0)
            {
                JoinButton.Enabled = true;
            }
            else
                JoinButton.Enabled = false;
        }

        /*
         * This will fire the setup event and close the new form
         * 
         * */
        private void JoinButton_Click(object sender, EventArgs e)
        {
            string temp = string.Empty;

            // This is what will be sent back to the spreadsheet form (or Form1)
            temp = "Name:" + OpenFilenameTextbox.Text + "\n" + "Password:" + OpenPasswordTextbox.Text;

            if (open_setup != null)
            {
                open_setup(temp);
            }

            Close();
        }

        /// <summary>
        /// Prompts the user if the file has changed before they close it allowing them to discard or cancel.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);

            if (e.CloseReason == CloseReason.UserClosing)
            {
                string temp = string.Empty;

                if (open_setup != null)
                {
                    open_setup(temp);
                }
            }
        }
    }
}
