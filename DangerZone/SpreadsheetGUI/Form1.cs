//Created by Alex Gritton, U0667623 for Cs3500 Fall 2012
//
//Modified by Anthony Jordan Wanlass and Ryan Jones for CS 3505 Spring 2013

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.Net.Sockets;
using CustomNetworking;
using System.Threading;

namespace SS
{
    public partial class Form1 : Form
    {

        private Spreadsheet my_spread_sheet;//this is the abstract spreadsheet that the gui reads from.
        private string save_location;
        private string cell_name;
        private string spreadsheet_name;
        private TcpClient client;// This will be the client used to connect to a server
        private StringSocket stringsocket;// This will be used to communicate with the server
        private NewSpreadsheetForm NewForm;// This is the form that will open if the user tries to create a new spreadsheet
        private OpenSpreadsheetForm OpenForm;// This is the form that will open if the user tries to open an existing spreadsheet
        private Queue<string> unprocessed_commands;// This will hold commands that are unprocessed/incomplete
        private Queue<string> old_contents;// This will hold the old contents of a cell from the spreadsheet
        private Queue<string> changes_queue;// This is a queue of things that have yet to be put into the spreadsheet
        private ISet<string> cells_to_recalulate;// This will be used in entering in things into the spreadsheet
        private int wait_time = 0;// This is how many times second_process will run;
        private int version = 0;// This is the version of the current spreadsheet;
        private int change_version = 0;
        private int row = 0;// This will be used in entering in things into the spreadsheet
        private int col = 0;// This will be used in entering in things into the spreadsheet
        private readonly object change_lock;// This is the lock for when a current change is being done
        private readonly object queue_lock;
        private readonly object command_queue_lock;
        private Boolean server_ready;
        private Boolean waiting;
        private Boolean undo_pending;
        private Boolean send_ok;
        private Boolean ignore_timer;
        private Boolean ignore_timer1;

        


        /// <summary>
        /// Creates a new spread sheet the user can interact with.
        /// </summary>
        public Form1()
        {
            InitializeComponent();
            //sets the title of the spread sheet to 1 more than the one that opened it.
            this.Text = "DangerZone";//"Spreadsheet " + (SpreadsheetApplicationContext.getAppContext().get_form_count() + 1);
            my_spread_sheet = new Spreadsheet(IsValid, Normalize, "ps6");
            //sets the initial user edit location to the contents box.
            contents_box.Select();
            spreadsheetPanel1.SelectionChanged += update_display;
            spreadsheetPanel1.SetSelection(0, 0);
            unprocessed_commands = new Queue<string>();
            old_contents = new Queue<string>();
            changes_queue = new Queue<string>();
            cells_to_recalulate = new HashSet<string>();
            change_lock = new object();// This is the lock for when a current change is being done
            queue_lock = new object();
            command_queue_lock = new object();
            cell_name = string.Empty;
            spreadsheet_name = string.Empty;
            server_ready = true;
            waiting = false;
            undo_pending = false;
            send_ok = false;
            ignore_timer = false;
            ignore_timer1 = false;
            NewForm = null;
            OpenForm = null;
        }

        /// <summary>
        /// updates the display of the GUI.
        /// </summary>
        /// <param name="ss"></param>
        private void update_display(SpreadsheetPanel ss)
        {
            contents_box.Select();
            //stores where the user has selected on the spreadsheetpanel.
            int col, row;
            ss.GetSelection(out col, out row);
            //converts the col row location on the spreadsheetpanel to a usable name such as A1 for (0,0).
            string cell_name = (char)(65 + col) + (row + 1).ToString();
            //if the contents of the highlighted cell is a double or a string this and the contents are longer than 
            //0(meaning they aren't "" then this sets the contents box back to what the user entered before, otherwise
            //sets the contents box to empty.
            if (my_spread_sheet.GetCellContents(cell_name) is double || my_spread_sheet.GetCellContents(cell_name) is string)
            {
                if(my_spread_sheet.GetCellContents(cell_name).ToString().Length >=1)
                {
                    contents_box.Text = my_spread_sheet.GetCellContents(cell_name).ToString();  
                }
                else
                {
                    contents_box.Text = "";
                }
            }
            //if the cell names contents are not a double or a string then it must be a formula so an = is prepended.
            else
            {
                contents_box.Text = "=" + my_spread_sheet.GetCellContents(cell_name).ToString();
            }
            //if the cell name has a value then it is displayed in the value box.
            if (my_spread_sheet.GetCellValue(cell_name)!= null)
            {
                value_box.Text = my_spread_sheet.GetCellValue(cell_name).ToString();
            }
            //if the cell name does not have a value then the value box is cleared.
            else
            {
                value_box.Clear();
            }

            //this shows the current cell name selected along with value appended in the value label next to the value box.
            value_label.Text = cell_name + " Value";
            contents_box.SelectAll();
        }

        /// <summary>
        /// If the enter key is pressed while the contents box is selected this event is handled.
        /// This event updates the spread sheet to what is currently in the contents box.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void update_key_press(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Return)
            {
                e.Handled = true;

                spreadsheetPanel1.GetSelection(out col, out row);

                string value = contents_box.Text;
                cell_name = (char)(65 + col) + (row + 1).ToString();
                string content = cell_name + " " + value;
                
                TimerCallback tcb = check_timeout;
                System.Threading.Timer timeout = new System.Threading.Timer(tcb, new AutoResetEvent(false), 5000, 0);
                ignore_timer = false;
                lock (queue_lock)
                {
                    changes_queue.Enqueue(content);
                }
                ignore_timer = true;               
                if (server_ready)
                {
                    server_ready = false;
                    int temp_version = version;
                   SendMessage(temp_version);
                }
            }
        }

        /// <summary>
        /// This will see if we are blocking on the gui thread and need to rejoin
        /// </summary>
        /// <param name="state"></param>
        private void check_timeout(object state)
        {
            if (ignore_timer)
            {
                return;
            }
            else
            {
                MessageBox.Show("timed out");
            }

        }

        /// <summary>
        /// This will see if we are blocking the gui thread in send message and need to rejoin
        /// </summary>
        /// <param name="state"></param>
        private void check_timeout1(object state)
        {
            if (ignore_timer1)
            {
                return;
            }
            else
            {

                MessageBox.Show("timed out1");
            }
        }

        /// <summary>
        /// Checks if the user presses any of the arrow keys or Ctl+S.
        /// If the user presses an arrow the focus is moved the appriate cell.
        /// If the user presses Ctl+S this file is saved to the current file location.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void update_key_press(object sender, KeyEventArgs e)
        {
            int col, row;
            spreadsheetPanel1.GetSelection(out col, out row);
            if (e.KeyCode == Keys.Up)
            {
                spreadsheetPanel1.SetSelection(col, row - 1);
                update_display(spreadsheetPanel1);
            }
            else if (e.KeyCode == Keys.Down)
            {
                spreadsheetPanel1.SetSelection(col, row + 1);
                update_display(spreadsheetPanel1);
            }
            else if (e.KeyCode == Keys.Left)
            {
                spreadsheetPanel1.SetSelection(col - 1, row);
                update_display(spreadsheetPanel1);
            }
            else if (e.KeyCode == Keys.Right)
            {
                spreadsheetPanel1.SetSelection(col + 1, row);
                update_display(spreadsheetPanel1);
            }
            else if (e.Modifiers == Keys.Control && e.KeyCode == Keys.S)
            {
                e.Handled = true;
                saveToolStripMenuItem1_Click(sender, e);
            }
        }

        /// <summary>
        /// If the user clicks the new button in the file menu strip, a new spreadsheet GUI is created.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (OpenForm != null)
            {
                OpenForm.Invoke(new Action(() => OpenForm.Close()));
                OpenForm = null;
            }
            SpreadsheetApplicationContext.getAppContext().RunForm(NewForm = new NewSpreadsheetForm());
            NewForm.new_setup += new_setup_received;
        }

        /// <summary>
        /// If the user clicks the save button in the file menu, the current file is saved.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void saveToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            stringsocket.BeginSend("SAVE\n" + "Name:" + spreadsheet_name + "\n", (ex, p) => { }, null);
        }

        /// <summary>
        /// This opens a valid spread sheet in a new window.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (NewForm != null)
            {
                NewForm.Invoke(new Action(() => NewForm.Close()));
                NewForm = null;
            }
            SpreadsheetApplicationContext.getAppContext().RunForm(OpenForm = new OpenSpreadsheetForm());
            OpenForm.open_setup += open_setup_received;
        }

        /// <summary>
        /// This function loops through all the newly added names in a spread sheet and updates the spreadsheetpanel accordingly.
        /// </summary>
        private void open_function()
        {
            IEnumerable<string> non_empty = my_spread_sheet.GetNamesOfAllNonemptyCells();
            foreach (string name in non_empty)
            {
                spreadsheetPanel1.SetValue(name[0] - 65, int.Parse(name.Substring(1)) - 1, my_spread_sheet.GetCellValue(name).ToString());
            }
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
                try
                {
                    client.Client.Shutdown(SocketShutdown.Both);
                    client.Client.Close();
                    stringsocket = null;
                }
                catch (Exception)
                {
                }
            }
        }

        /// <summary>
        /// Exits the program if no changes have been made, otherwise asks the user what they would like to do.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        /// <summary>
        /// Normalize delegate to be passed into a newly created spreadsheet.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        private string Normalize(string name)
        {
            return name.ToUpper();
        }

        /// <summary>
        /// IsValid function to be passed into a newly created spreadsheet.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        private bool IsValid(string name)
        {
            bool valid = false;
            //Name must be either a Single letter followed by a non zero digit or a Single letter folloed by a non zero digit and a digit.
            Regex var = new Regex(@"(^[A-Z][1-9]$)|(^[A-Z][1-9][0-9]$)");
            if (var.IsMatch(Normalize(name)))
            {
                valid = true;
            }
            return valid;
        }

        /// <summary>
        /// Displays a message when the about is clicked in the help menu.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Created by Alex Gritton, Anthony Jordan Wanlass, Ryan Jones, Triston Thorpe Spring 2013 for CS 3505.");
        }

        /// <summary>
        /// Explains how to save a file when the save is clicked in the help menu.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void helpMenuSave_Click(object sender, EventArgs e)
        {
            MessageBox.Show("This will save the current spreadsheet onto the server.");
        }
    
        /// <summary>
        /// Allows the user to see the current file location when the Show File Location is clicked in the view menu.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void viewMenuFileLocation_Click(object sender, EventArgs e)
        {
            viewMenuHideLocation.Enabled = true;
            viewMenuFileLocation.Enabled = false;
        }

        /// <summary>
        /// Prevents the user from seeing the current file location when the Hide File Locatoin is clicked in the view menu.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void viewMenuHideLocation_Click(object sender, EventArgs e)
        {
            viewMenuHideLocation.Enabled = false;
            viewMenuFileLocation.Enabled = true;
        }



        /// <summary>
        /// Explains how the contents box works to the user.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void viewMenuCellContents_Click(object sender, EventArgs e)
        {
            MessageBox.Show("The Contents Box allows the user to input three types of data.\n 1. A formula starting with \"=\". Example =A1+A2.\n 2. Text. Example \"Hello World\". \n 3. A ber. Example \"3.5\".");
        }

        private void viewMenuTextBoxValue_Click(object sender, EventArgs e)
        {
            MessageBox.Show("The Value Box shows the user the value of the current cell indicated in the Label to the left of the Value Box.");
        }

        private void viewMenuCellBoxValue_Click(object sender, EventArgs e)
        {
            MessageBox.Show("The values of the Cells are shown in the cell corresponding to its name.");
        }

        private void helpMenuSpreadSheetFormula_Click(object sender, EventArgs e)
        {
            MessageBox.Show("If the user inputs a formula that references an empty or invalid cell, a SpreadSheet.FormulaError is placed in the cell value. Example(A1: Hello, A2: =A1+2)");
        }

        private void helpMenuInvalidFormula_Click(object sender, EventArgs e)
        {
            MessageBox.Show("If the user inputs an invalid formula into the contents box, an Invalid Formula Popup is displayed. Bad Formula Examples: (=A1++), (=AA11+BB22), (=A3A), (=A1+3())");
        }

        private void helpMenuCircularException_Click(object sender, EventArgs e)
        {
            MessageBox.Show("A Circular Exception window is displayed if a cells formula references itself. Example: (A1: =A1+2)");
        }





/*
 * Below here are the new functions that will be added to support client/server communication.
 * Functions above this point have possibly been modified to support client/server communication.
 * 
 * 
 * */



        /*
         * This function will connect to the server
         * 
         * */
        private void connectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (stringsocket == null)
                {
                     client = new TcpClient(ip_address_box.Text, Convert.ToInt32(port_text_box.Text));
                     stringsocket = new StringSocket(client.Client, UTF8Encoding.Default);
                     ip_address_box.Enabled = false;
                     port_text_box.Enabled = false;
                     connect_button.Enabled = false;
                     connected_label1.Text = "Connected: Online";
                     connected_label1.ForeColor = System.Drawing.Color.Green;
                     newToolStripMenuItem.Enabled = true;
                     openToolStripMenuItem.Enabled = true;
                     stringsocket.BeginReceive(first_process, null);            
                }
            }
            catch (Exception)
            {
                ip_address_box.Enabled = true;
                port_text_box.Enabled = true;
                MessageBox.Show("Connection to server failed, please attempt to reconnect.");
            }
        }

        /*
         * This will make sure that the user cannot attempt to connect to a server unless there is text in the 
         * IPAddress text box.
         * 
         * */
        private void ip_address_box_TextChanged(object sender, EventArgs e)
        {
            if (ip_address_box.Text == string.Empty && port_text_box.Text == string.Empty)
            {
                connect_button.Enabled = false;
            }
            else
            {
                connect_button.Enabled = true;
            }
        }

        /*
         * This will send the create command with the filename and the password to the server
         * 
         * */
        private void new_setup_received(object s)
        {
            if (s.ToString().Length != 0)
            {
                stringsocket.BeginSend("CREATE\n" + s.ToString() + "\n", (e, p) => { }, null);
            }        
            NewForm = null;
        }

        /*
         * This will send the join command with the filename and the password to the server
         * 
         * */
        private void open_setup_received(object s)
        {
            if (s.ToString().Length != 0)
            {
                stringsocket.BeginSend("JOIN\n" + s.ToString() + "\n", (e, p) => { }, null);
            }

            OpenForm = null;
        }

        /*
         * Sends the undo command to the server, undoes the last modification made to the spreadsheet by any user
         * 
         * */
        private void undoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            stringsocket.BeginSend("UNDO\n" + "Name:" + spreadsheet_name + "\n" + "Version:" + version.ToString() + "\n", (ex, p) => { }, null);
        }

        /*
         * This is the first callback for beginreceive, it will search for command keywords
         * set wait time, and begins listening for the rest of the message using the secondprocess callback
         * 
         * */
        private void first_process(string s, Exception e, object p)
        {
            lock (command_queue_lock)
            {
                unprocessed_commands.Enqueue(s);
            }
            //MessageBox.Show("first call back " + s);
            switch (s)
            {
                case "CREATE OK":
                    wait_time = 2;
                    stringsocket.BeginReceive(second_process, null);
                    break;
                case "CREATE FAIL":
                    wait_time = 2;
                    stringsocket.BeginReceive(second_process, null);
                    break;
                case "JOIN OK":
                    wait_time = 4;
                    stringsocket.BeginReceive(second_process, null);
                    break;
                case "JOIN FAIL":
                    wait_time = 2;
                    stringsocket.BeginReceive(second_process, null);
                    break;
                case "CHANGE OK":
                    wait_time = 2;
                    stringsocket.BeginReceive(second_process, null);
                    break;
                case "CHANGE WAIT":
                    wait_time = 2;
                    stringsocket.BeginReceive(second_process, null);
                    break;
                case "CHANGE FAIL":
                    wait_time = 2;
                    stringsocket.BeginReceive(second_process, null);
                    break;
                case "UNDO FAIL":
                    wait_time = 2;
                    stringsocket.BeginReceive(second_process, null);
                    break;
                case "UNDO END":
                    wait_time = 2;
                    stringsocket.BeginReceive(second_process, null);
                    break;
                case "UNDO OK":
                    wait_time = 5;
                    stringsocket.BeginReceive(second_process, null);
                    break;
                case "UNDO WAIT":
                    wait_time = 2;
                    stringsocket.BeginReceive(second_process, null);
                    break;
                case "UPDATE":
                    wait_time = 5;
                    stringsocket.BeginReceive(second_process, null);
                    break;
                case "SAVE OK":
                    wait_time = 1;
                    stringsocket.BeginReceive(second_process, null);
                    break;
                case "SAVE FAIL":
                    wait_time = 2;
                    stringsocket.BeginReceive(second_process, null);
                    break;
                case "ERROR":
                    break;
                case null:
                    disconnect();
                    break;
            }

        }

        /*
         * This is the second callback, it will collect the appropriate number of commands and then call
         * final process
         * 
         * */
        private void second_process(string command, Exception e, object p)
        {
            if (command == null)
            {
                //MessageBox.Show("got a null");
                disconnect();
            }
            if (wait_time > 1)
            {
                lock (command_queue_lock)
                {
                    unprocessed_commands.Enqueue(command);
                }
                wait_time--;
                stringsocket.BeginReceive(second_process, null);
            }
            else
            {
                //MessageBox.Show("In Second process");
                lock (command_queue_lock)
                {
                    unprocessed_commands.Enqueue(command);
                }
                wait_time--;
                stringsocket.BeginReceive(first_process, null);
                final_process();
            }
        }

        /*
         * This will take in a full command and update the spreadsheet accordingly
         * 
         * */
        private void final_process()
        {
            string s = string.Empty;
            lock (command_queue_lock)
            {
                s = unprocessed_commands.Dequeue();
                //MessageBox.Show("second call back " + s);
                switch (s)
                {
                    // This means we have successfully created a new spreadsheet
                    case "CREATE OK":
                        spreadsheet_name = unprocessed_commands.Dequeue().Substring(5);// this gets the spreadsheet name
                        string pass = unprocessed_commands.Dequeue().Substring(9);//This removes password from the queue
                        stringsocket.BeginSend("JOIN\n" + "Name:" + spreadsheet_name + "\n" + "Password:" + pass + "\n", (ex, p) => { }, null);
                        break;
                    // This means the creation of the new spreadsheet was unsuccessful
                    case "CREATE FAIL":
                        unprocessed_commands.Dequeue();// This removes the name from the queue
                        MessageConsoletextBox.Invoke(new Action(() => MessageConsoletextBox.Text = unprocessed_commands.Dequeue()));
                        //MessageBox.Show(unprocessed_commands.Dequeue());//displays the error message
                        break;
                    case "JOIN OK":
                        spreadsheet_name = unprocessed_commands.Dequeue().Substring(5);// removes the name from the queue
                        int.TryParse(unprocessed_commands.Dequeue().Substring(8), out version);// sets the version number
                        unprocessed_commands.Dequeue();// removes the length from the queue
                        temp_save(unprocessed_commands.Dequeue());// This creates a temporary xml file.
                        my_spread_sheet = new Spreadsheet("TempFile.xml", IsValid, Normalize, "ps6");// This creates a new spreadsheet (logic)
                        open_function();// This updates the spreadsheet gui

                        contents_box.Invoke(new Action(() => contents_box.Enabled = true));// enables us to begin making change
                        contents_box.Invoke(new Action(() => contents_box.Select()));// give focus to the contents box
                        Invoke(new Action(() => newToolStripMenuItem.Enabled = false));// disables the option to create a new spreadsheet
                        Invoke(new Action(() => openToolStripMenuItem.Enabled = false));// disables the option to open a new spreadsheet
                        Invoke(new Action(() => saveToolStripMenuItem1.Enabled = true));// enables the option to open a new spreadsheet
                        Invoke(new Action(() => undoToolStripMenuItem.Enabled = true));// enable the the option to undo
                        Invoke(new Action(() => leaveSessionToolStripMenuItem.Enabled = true));// enable the leave option
                        Invoke(new Action(() => this.Text = spreadsheet_name));// updates the name of the spreadsheet
                        Invoke(new Action(() => MessageConsoletextBox.Text = "Join Successful: " + spreadsheet_name));// updates the name of the spreadsheet
                        break;
                    case "JOIN FAIL":
                        unprocessed_commands.Dequeue();// This removes the name from the queue
                        MessageConsoletextBox.Invoke(new Action(() => MessageConsoletextBox.Text = unprocessed_commands.Dequeue()));
                        //MessageBox.Show(unprocessed_commands.Dequeue());//displays the error message
                        break;
                    case "CHANGE OK":
                        unprocessed_commands.Dequeue();
                        int.TryParse(unprocessed_commands.Dequeue().Substring(8), out version);// gets our version
                        spreadsheet_insert();
                        ////////////////////////////////
                        gui_update_execute();// spawn on new thread
                        ////////////////////////////////
                        lock (queue_lock)
                        {
                            changes_queue.Dequeue();// removes the command from the queue since it was successful
                        }
                        server_ready = true;// tells us that we can submit the next change to the server
                        break;
                    case "CHANGE WAIT":
                        waiting = true;// we need to wait for the right version
                        unprocessed_commands.Dequeue();// removes name from the queue
                        int.TryParse(unprocessed_commands.Dequeue().Substring(8), out change_version);// gets the updated version

                        if (version == change_version)
                        {
                            SendMessage(change_version);
                        }
                        break;
                    case "CHANGE FAIL":
                        unprocessed_commands.Dequeue();// remove name from the queue

                        /////////////////////
                        old_value();// puts back in the old value into our spreadsheet logic (new thread)
                        ////////////////////
                        MessageConsoletextBox.Invoke(new Action(() => unprocessed_commands.Dequeue()));// show error message
                        break;
                    case "UNDO FAIL":
                        unprocessed_commands.Dequeue();// remove name from the queue
                        MessageConsoletextBox.Invoke(new Action(() => unprocessed_commands.Dequeue()));// show error message
                        break;
                    case "UNDO END":
                        for (int i = 0; i < 2; i++)
                            unprocessed_commands.Dequeue();// removes name and version from the queue
                        MessageConsoletextBox.Invoke(new Action(() => MessageConsoletextBox.Text = "No Undos available"));// alert user that no undos are available
                        break;
                    case "UNDO OK":
                        unprocessed_commands.Dequeue();// remove the name from the queue
                        int.TryParse(unprocessed_commands.Dequeue().Substring(8), out version);// get the version
                        string sent_cell = unprocessed_commands.Dequeue().Substring(5);// get the cell name
                        unprocessed_commands.Dequeue();// remove the content length from the queue

                        int temp_version = version;
                        /////////////////////////////
                        undo_accepted(temp_version, sent_cell, unprocessed_commands.Dequeue());// make the undo happen (new thread)
                        //////////////////////////////

                        break;
                    case "UNDO WAIT":
                        unprocessed_commands.Dequeue();// removes name from the queue
                        int.TryParse(unprocessed_commands.Dequeue().Substring(8), out version);// sets the version
                        undo_pending = true;
                        Invoke(new Action(() => undoToolStripMenuItem.Enabled = false));// disable the undo option
                        break;
                    case "UPDATE":
                        unprocessed_commands.Dequeue();// remove name from the queue
                        int.TryParse(unprocessed_commands.Dequeue().Substring(8), out version);// get the version
                        string sent_cell_name = unprocessed_commands.Dequeue().Substring(5);// get the cell name
                        unprocessed_commands.Dequeue();// remove the content length from the queue

                        /////////////////////////////
                        update_received(version, sent_cell_name, unprocessed_commands.Dequeue());// make the update happen (new thread)
                        //////////////////////////////
                        break;
                    case "SAVE OK":
                        unprocessed_commands.Dequeue();// removes the name from the queue
                        MessageConsoletextBox.Invoke(new Action(() => MessageConsoletextBox.Text = "Save Successful"));
                        break;
                    case "SAVE FAIL":

                        unprocessed_commands.Dequeue();// This removes the name from the queue
                        MessageConsoletextBox.Invoke(new Action(() => unprocessed_commands.Dequeue()));
                        //MessageBox.Show(unprocessed_commands.Dequeue());//displays the error message
                        break;
                    case "ERROR":
                        break;
                    case null:
                        //MessageBox.Show("got a null 1");
                        disconnect();
                        break;
                }
            }
        }

        /*
         * This function takes in a string and saves it as an xml file
         * 
         * */
        private void temp_save(string s)
        {
            System.IO.File.WriteAllText("TempFile.xml", s);
        }

        /*
         * This updates the gui after the change has been accepted by the server
         * 
         * */
        private void gui_update_execute()
        {
            try
            {
                //this sets the value of the cell on the spreadsheetpanel so the user can see.
                spreadsheetPanel1.SetValue(col, row, my_spread_sheet.GetCellValue(cell_name).ToString());
            }
            catch
            { }
            foreach (string name in cells_to_recalulate)
            {
                //this loops through each cell dependent on the cell name and displays its value to the user on the spreadsheetpanel
                spreadsheetPanel1.SetValue(name[0] - 65, int.Parse(name.Substring(1)) - 1, my_spread_sheet.GetCellValue(name).ToString());
            }
        }

        /*
         * This will actually call the beginsend
         * 
         * */
        private void SendMessage(int temp_version)
        {
            waiting = false;

            string temp = changes_queue.Peek();// look at whats the first thing in the changes queue without removing it
            cell_name = temp.Substring(0, temp.IndexOf(" "));// gets the cell name
            string value = temp.Substring(temp.IndexOf(" ") + 1);// gets the value
            cell_name.Trim();
            value.Trim();
            TimerCallback tcb = check_timeout1;
            System.Threading.Timer timeout = new System.Threading.Timer(tcb, new AutoResetEvent(false), 10000, 0);
            ignore_timer1 = false;
            lock (change_lock)
            {

                /*
                    * Put this into the logic spreadsheet and checks for circular dependencies
                    * 
                    * */
                try
                {
                        if (value != "")
                        {
                            cells_to_recalulate = my_spread_sheet.SetContentsOfCell(cell_name, value);
                        }
                        else
                        {
                            cells_to_recalulate = my_spread_sheet.SetContentsOfCell(cell_name, " ");
                        }
                        send_ok = true;
                        spreadsheet_remove();// take the value out of the spreadsheet until it is accepted
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    cells_to_recalulate = new HashSet<string> { };
                    send_ok = false;                 
                    lock (queue_lock)
                    {
                        changes_queue.Dequeue();
                    }
                    
                    server_ready = true;// begin the next send
                }
                ignore_timer1 = true;
            }

            

            // if we didn't get a circular exception send to server
            if (send_ok)
            {
                stringsocket.BeginSend("CHANGE\n" + "Name:" + spreadsheet_name + "\n" + "Version:" + temp_version.ToString() + "\n" + "Cell:" + cell_name + "\n" + "Length:" + value.Length.ToString() + "\n" + value + "\n", MessageSent, null);
            }
        }

        /*
         * Callback for the beginsend
         * 
         * */
        private void MessageSent(Exception e, object payload)
        {
            if (changes_queue.Count != 0)
            {
                if (server_ready)
                {
                    //lock (change_lock)
                   // {
                        int temp_version = version;
                        SendMessage(temp_version);
                    //}
                }
            }
            else
            {
                server_ready = true;
            }
        }

        /*
         * This is the function that is called when we receive an update from the server
         * It checks if we are waiting to receive the newest version and then updates the spreadsheet
         * 
         * */
        private void update_received(int sent_version, string cell, string content)
        {
            int update_version = sent_version;
            ISet<string> update_cells_to_recalculate = new HashSet<string>();
            lock (change_lock)
            {
                

                // if we receive a undo wait command and receive the newest version
                if (undo_pending && version == update_version)
                {
                    stringsocket.BeginSend("UNDO\n" + "Name:" + spreadsheet_name + "\n" + "Version:" + version.ToString() + "\n", (ex, p) => { }, null);
                    undo_pending = false;
                    Invoke(new Action(() => undoToolStripMenuItem.Enabled = true));
                }

                /*
                 * Update the spreadsheet logic
                 * 
                 * */
                try
                {
                        if (content != "")
                        {
                            update_cells_to_recalculate = my_spread_sheet.SetContentsOfCell(cell, content);
                        }
                        else
                        {
                            update_cells_to_recalculate = my_spread_sheet.SetContentsOfCell(cell, " ");
                        }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    cells_to_recalulate = new HashSet<string> { };
                }
            }
                // convert the cell name into a row and column
                int temp_row, temp_col;
                temp_col = cell[0] - 65;
                int.TryParse(cell.Substring(1, cell.Length - 1), out temp_row);
                temp_row--;

                /*
                 * Update the spreadsheet Gui
                 * 
                 * */
                try
                {
                    //this sets the value of the cell on the spreadsheetpanel so the user can see.
                    spreadsheetPanel1.Invoke(new Action(() => spreadsheetPanel1.SetValue(temp_col, temp_row, my_spread_sheet.GetCellValue(cell).ToString())));
                }
                catch
                {
                    MessageBox.Show("Unable to Update Gui");
                }
                foreach (string name in update_cells_to_recalculate)
                {
                    //this loops through each cell dependent on the cell name and displays its value to the user on the spreadsheetpanel
                    spreadsheetPanel1.Invoke(new Action(() => spreadsheetPanel1.SetValue(name[0] - 65, int.Parse(name.Substring(1)) - 1, my_spread_sheet.GetCellValue(name).ToString())));
                }


            // if we received a change wait command and receive the newest version
            if (waiting && change_version == update_version)
            {
                waiting = false;

                //////////////////////////////////////
                SendMessage(update_version);// new thread
                //////////////////////////////////////  
            }
        }

        /*
         * Inserts back into the spreadsheet the old value before you attempted to make a change
         * 
         * */
        private void old_value(Boolean change_wait = false)
        {
            // remove the bad change from the changes queue
            lock (change_lock)
            {
                if (!change_wait)
                {
                    lock (queue_lock)
                    {
                        changes_queue.Dequeue();
                    }
                }               
            }

            string previous_value = string.Empty;
            spreadsheetPanel1.GetValue(col, row, out previous_value);

            /*
             * Updates the spreadsheet logic.
             * 
             * */
            try
            {
                if (previous_value != "")
                {
                    cells_to_recalulate = my_spread_sheet.SetContentsOfCell(cell_name, previous_value);
                }
                else
                {
                    cells_to_recalulate = my_spread_sheet.SetContentsOfCell(cell_name, " ");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                cells_to_recalulate = new HashSet<string> { };
            }
        }

        /*
         * This function will be called when a successful undo occurs
         * It wil re-insert the old modification into the spreadsheet.
         * 
         * */
        private void undo_accepted(int sent_version, string cell, string content)
        {
            lock (change_lock)
            {
                /*
                 * Update the spreadsheet logic
                 * 
                 * */
                ISet<string> update_cells_to_recalculate = new HashSet<string>();
                try
                {

                    if (content != "")
                    {
                        update_cells_to_recalculate = my_spread_sheet.SetContentsOfCell(cell, content);
                    }
                    else
                    {
                        update_cells_to_recalculate = my_spread_sheet.SetContentsOfCell(cell, " ");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    cells_to_recalulate = new HashSet<string> { };
                }

                // convert the cell name into a row and column
                int temp_row, temp_col;
                temp_col = cell[0] - 65;
                int.TryParse(cell.Substring(1, cell.Length - 1), out temp_row);
                temp_row--;

                /*
                 * Update the spreadsheet Gui
                 * 
                 * */
                try
                {
                    //this sets the value of the cell on the spreadsheetpanel so the user can see.
                    spreadsheetPanel1.Invoke(new Action(() => spreadsheetPanel1.SetValue(temp_col, temp_row, my_spread_sheet.GetCellValue(cell).ToString())));
                }
                catch
                { }
                foreach (string name in update_cells_to_recalculate)
                {
                    //this loops through each cell dependent on the cell name and displays its value to the user on the spreadsheetpanel
                    spreadsheetPanel1.Invoke(new Action(() => spreadsheetPanel1.SetValue(name[0] - 65, int.Parse(name.Substring(1)) - 1, my_spread_sheet.GetCellValue(name).ToString())));
                }
            }

        }

        /*
         * This disconnects the client connection
         * 
         * */
        private void disconnect()
        {
            try
            {
                connected_label1.Invoke(new Action(() => connected_label1.Text = "Connected: Offline"));
                connected_label1.Invoke(new Action(() => connected_label1.ForeColor = System.Drawing.Color.Red));
                connect_button.Invoke(new Action(() => connect_button.Enabled = true));
                contents_box.Invoke(new Action(() => contents_box.Enabled = false));
                contents_box.Invoke(new Action(() => contents_box.Clear()));
                value_box.Invoke(new Action(() => value_box.Clear()));
                Invoke(new Action(() => newToolStripMenuItem.Enabled = false));
                Invoke(new Action(() => openToolStripMenuItem.Enabled = false));
                Invoke(new Action(() => saveToolStripMenuItem1.Enabled = false));
                Invoke(new Action(() => undoToolStripMenuItem.Enabled = false));
                Invoke(new Action(() => leaveSessionToolStripMenuItem.Enabled = false));
                ip_address_box.Invoke(new Action(()=>ip_address_box.Enabled = true));
                port_text_box.Invoke(new Action(() => port_text_box.Enabled = true));

                // clear the spreadsheet logic and gui
                my_spread_sheet = new Spreadsheet(IsValid, Normalize, "ps6");
                spreadsheetPanel1.Invoke(new Action(() => spreadsheetPanel1.Clear()));

                this.Invoke(new Action(() => this.Text = "DangerZone"));


                MessageConsoletextBox.Invoke(new Action(() => MessageConsoletextBox.Clear()));// clears the message console text box.
                lock (queue_lock)
                {
                    changes_queue.Clear();// empty out out changes queue
                }
                lock (command_queue_lock)
                {
                    unprocessed_commands.Clear();// empty out the unprocessed commands queue
                }
            }
            catch
            {
            }
            if(NewForm != null)
            {
                NewForm.Invoke(new Action(() => NewForm.Close()));
                NewForm = null;
            }

            if (OpenForm != null)
            {
                OpenForm.Invoke(new Action(() => OpenForm.Close()));
                OpenForm = null;
            }

            try
            {
                client.Client.Shutdown(SocketShutdown.Both);
                client.Client.Close();
                stringsocket = null;
            }
            catch (Exception)
            {
            }
        }

        /*
         * This will leave a spreadsheet editing session
         * 
         * */
        private void leaveSessionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // tell the server you are leaving the session
            if (stringsocket != null)
            {
                stringsocket.BeginSend("LEAVE\n" + "Name:" + spreadsheet_name + "\n", (ex, p) => { }, null);
            }

            contents_box.Enabled = false;// disables us to begin making changes
            contents_box.Clear();// clear all the text from the contents box
            value_box.Clear();// clear all the text form the value box
            newToolStripMenuItem.Enabled = true;// enables the option to create a new spreadsheet
            openToolStripMenuItem.Enabled = true;// enables the option to open a new spreadsheet
            saveToolStripMenuItem1.Enabled = false;// disables the option to open a new spreadsheet
            undoToolStripMenuItem.Enabled = false;// disables the option to undo
            leaveSessionToolStripMenuItem.Enabled = false;// disables the option to leave a spreadsheet editing session


            // clear the spreadsheet logic and gui
            my_spread_sheet = new Spreadsheet(IsValid, Normalize, "ps6");
            spreadsheetPanel1.Clear();

            this.Text = "DangerZone";

            MessageConsoletextBox.Text = "";// clears the message console text box.
            lock (queue_lock)
            {
                changes_queue.Clear();// empty out out changes queue
            }
            lock (command_queue_lock)
            {
                unprocessed_commands.Clear();// empty out the unprocessed commands queue
            }
        }

        /*
         * Inserts whatever is in the queue into the spreadsheet
         * 
         * */
        private void spreadsheet_insert()
        {
            string temp = changes_queue.Peek();// look at whats the first thing in the changes queue without removing it
            cell_name = temp.Substring(0, temp.IndexOf(" "));// gets the cell name
            string value = temp.Substring(temp.IndexOf(" ") + 1);// gets the value
            cell_name.Trim();
            value.Trim();

            /*
             * Put this into the logic spreadsheet and checks for circular dependencies
             * 
             * */
            try
            {
                if (value != "")
                {
                    cells_to_recalulate = my_spread_sheet.SetContentsOfCell(cell_name, value);
                }
                else
                {
                    cells_to_recalulate = my_spread_sheet.SetContentsOfCell(cell_name, " ");
                }
                send_ok = true;
                //stringsocket.BeginSend("CHANGE\n" + "Name:" + spreadsheet_name + "\n" + "Version:" + version.ToString() + "\n" + "Cell:" + cell_name + "\n" + "Length:" + value.Length.ToString() + "\n" + value + "\n", MessageSent, null);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                cells_to_recalulate = new HashSet<string> { };
                send_ok = false;
                old_value();
                //changes_queue.Dequeue();// remove the bad change from the changes queue
                server_ready = true;// begin the next send
            }
        }

        /*
         * Removes whatever is in the spreadsheet and replaces it with a blank or the previous value (if there was one)
         * 
         * */
        private void spreadsheet_remove()
        {
            string previous_value = string.Empty;
            spreadsheetPanel1.GetValue(col, row, out previous_value);

            /*
             * Updates the spreadsheet logic.
             * 
             * */
            try
            {
                if (previous_value != "")
                {
                    cells_to_recalulate = my_spread_sheet.SetContentsOfCell(cell_name, previous_value);
                }
                else
                {
                    cells_to_recalulate = my_spread_sheet.SetContentsOfCell(cell_name, " ");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                cells_to_recalulate = new HashSet<string> { };
            }
        }

    }
}
