using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SpreadsheetGUI
{
    public partial class LogInWindow : Form
    {
        private SS.Form1 model;
        private string SSName, SSPass, IPAddress;

        public LogInWindow()
        {
            InitializeComponent();
            model = new SS.Form1();
            //model.IncomingLineEvent += MessageReceived;
        }



        /*
         * This will connect to the server and create a new spreadsheet.  If it successfully connects, error_message will be an empty string
         * If something goes wrong then the error message will be displayed in a message box.
         * 
         * */
        private void CreateButton_Click(object sender, EventArgs e)
        {
            //string error_message = model.Connect(IPAddress, 1984, SSName, SSPass, true);
            
           // if (error_message.Length > 0)
            //{
            //    MessageBox.Show("Sever is currently unavailable, please try to reconnect.");
          //  }
        }

        /*
         * This will connect to the server and join an existing spreadsheet.  
         * If it successfully connects, error_message will be an empty string
         * If something goes wrong then the error message will be displayed in a message box.
         * 
         * */
        private void join_button_click(object sender, EventArgs e)
        {
           // string error_message = model.Connect(IPAddress, 1984, SSName, SSPass, false);

            //if (error_message.Length > 0)
            //{
             //   MessageBox.Show("Sever is currently unavailable, please try to reconnect.");
            //}
        }

        private void IPText_TextChanged(object sender, EventArgs e)
        {
            IPAddress = IPText.Text;
           //ConnectButton.Enabled = false;

            if (IPText.Text.Length != 0 && SSNameText.Text.Length != 0 && SSPassText.Text.Length != 0)
                CreateButton.Enabled = true;
            else
                CreateButton.Enabled = false;
        }

        private void SSNameText_TextChanged(object sender, EventArgs e)
        {
            SSName = SSNameText.Text;
            //ConnectButton.Enabled = false;
            if (IPText.Text.Length != 0 && SSNameText.Text.Length != 0 && SSPassText.Text.Length != 0)
                CreateButton.Enabled = true;
            else
                CreateButton.Enabled = false;
        }

        private void SSPassText_TextChanged(object sender, EventArgs e)
        {
            SSPass = SSPassText.Text; 
           // ConnectButton.Enabled = false;
            if (IPText.Text.Length != 0 && SSNameText.Text.Length != 0 && SSPassText.Text.Length != 0)
                CreateButton.Enabled = true;
            else
                CreateButton.Enabled = false;
        }








    }
}
