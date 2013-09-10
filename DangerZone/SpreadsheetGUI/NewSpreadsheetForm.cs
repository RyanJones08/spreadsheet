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
    public partial class NewSpreadsheetForm : Form
    {
        public event Action<string> new_setup;
        public NewSpreadsheetForm()
        {
            InitializeComponent();
        }

        /*
         * This will fire the setup event and close the new form
         * 
         * */
        private void CreateButton_Click(object sender, EventArgs e)
        {
            string temp = string.Empty;

            // This is what will be sent back to the spreadsheet form (or Form1)
            temp = "Name:" + NewFilenameTextbox.Text + "\n" + "Password:" + NewPasswordTextbox.Text;

            if (new_setup != null)
            {
                new_setup(temp);
            }

            Close();
        }


        /*
         * Checks to make sure that the user cannot click create until both text boxes have text in them
         * 
         * */
        private void NewFilenameTextbox_TextChanged(object sender, EventArgs e)
        {
            if (NewFilenameTextbox.Text.Length != 0 && NewPasswordTextbox.Text.Length != 0)
            {
                CreateButton.Enabled = true;
            }
            else
                CreateButton.Enabled = false;
        }


       /*
        * Checks to make sure that the user cannot click create until both text boxes have text in them
        * 
        * */
        private void NewPasswordTextbox_TextChanged(object sender, EventArgs e)
        {
            if (NewFilenameTextbox.Text.Length != 0 && NewPasswordTextbox.Text.Length != 0)
            {
                CreateButton.Enabled = true;
            }
            else
                CreateButton.Enabled = false;
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

                if (new_setup != null)
                {
                    new_setup(temp);
                }
            }
        }

    }
}
