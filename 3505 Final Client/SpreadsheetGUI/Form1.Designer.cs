namespace SS
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.spreadsheetPanel1 = new SS.SpreadsheetPanel();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.leaveSessionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.undoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.viewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.viewMenuFileLocation = new System.Windows.Forms.ToolStripMenuItem();
            this.viewMenuHideLocation = new System.Windows.Forms.ToolStripMenuItem();
            this.toolsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.savingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpMenuSave = new System.Windows.Forms.ToolStripMenuItem();
            this.helpMenuSaveAs = new System.Windows.Forms.ToolStripMenuItem();
            this.cellToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.viewMenuCellContents = new System.Windows.Forms.ToolStripMenuItem();
            this.valueToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.viewMenuTextBoxValue = new System.Windows.Forms.ToolStripMenuItem();
            this.viewMenuCellBoxValue = new System.Windows.Forms.ToolStripMenuItem();
            this.exceptionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpMenuInvalidFormula = new System.Windows.Forms.ToolStripMenuItem();
            this.helpMenuCircularException = new System.Windows.Forms.ToolStripMenuItem();
            this.helpMenuSpreadSheetFormula = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.contents_box = new System.Windows.Forms.TextBox();
            this.position_label = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.value_label = new System.Windows.Forms.Label();
            this.value_box = new System.Windows.Forms.TextBox();
            this.ipAddressLabel = new System.Windows.Forms.Label();
            this.ip_address_box = new System.Windows.Forms.TextBox();
            this.connect_button = new System.Windows.Forms.Button();
            this.connected_label1 = new System.Windows.Forms.Label();
            this.MessageConsoletextBox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.port_label = new System.Windows.Forms.Label();
            this.port_text_box = new System.Windows.Forms.TextBox();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // spreadsheetPanel1
            // 
            this.spreadsheetPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.spreadsheetPanel1.Location = new System.Drawing.Point(0, 80);
            this.spreadsheetPanel1.Name = "spreadsheetPanel1";
            this.spreadsheetPanel1.Size = new System.Drawing.Size(747, 305);
            this.spreadsheetPanel1.TabIndex = 0;
            // 
            // menuStrip1
            // 
            this.menuStrip1.Dock = System.Windows.Forms.DockStyle.None;
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.editToolStripMenuItem,
            this.viewToolStripMenuItem,
            this.toolsToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(172, 24);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newToolStripMenuItem,
            this.openToolStripMenuItem,
            this.saveToolStripMenuItem1,
            this.toolStripSeparator1,
            this.leaveSessionToolStripMenuItem,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // newToolStripMenuItem
            // 
            this.newToolStripMenuItem.Enabled = false;
            this.newToolStripMenuItem.Name = "newToolStripMenuItem";
            this.newToolStripMenuItem.Size = new System.Drawing.Size(146, 22);
            this.newToolStripMenuItem.Text = "New";
            this.newToolStripMenuItem.Click += new System.EventHandler(this.newToolStripMenuItem_Click);
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Enabled = false;
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.Size = new System.Drawing.Size(146, 22);
            this.openToolStripMenuItem.Text = "Open";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
            // 
            // saveToolStripMenuItem1
            // 
            this.saveToolStripMenuItem1.Enabled = false;
            this.saveToolStripMenuItem1.Name = "saveToolStripMenuItem1";
            this.saveToolStripMenuItem1.Size = new System.Drawing.Size(146, 22);
            this.saveToolStripMenuItem1.Text = "Save";
            this.saveToolStripMenuItem1.Click += new System.EventHandler(this.saveToolStripMenuItem1_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(143, 6);
            // 
            // leaveSessionToolStripMenuItem
            // 
            this.leaveSessionToolStripMenuItem.Enabled = false;
            this.leaveSessionToolStripMenuItem.Name = "leaveSessionToolStripMenuItem";
            this.leaveSessionToolStripMenuItem.Size = new System.Drawing.Size(146, 22);
            this.leaveSessionToolStripMenuItem.Text = "Leave Session";
            this.leaveSessionToolStripMenuItem.Click += new System.EventHandler(this.leaveSessionToolStripMenuItem_Click);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(146, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // editToolStripMenuItem
            // 
            this.editToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.undoToolStripMenuItem});
            this.editToolStripMenuItem.Name = "editToolStripMenuItem";
            this.editToolStripMenuItem.Size = new System.Drawing.Size(39, 20);
            this.editToolStripMenuItem.Text = "Edit";
            // 
            // undoToolStripMenuItem
            // 
            this.undoToolStripMenuItem.Enabled = false;
            this.undoToolStripMenuItem.Name = "undoToolStripMenuItem";
            this.undoToolStripMenuItem.Size = new System.Drawing.Size(103, 22);
            this.undoToolStripMenuItem.Text = "Undo";
            this.undoToolStripMenuItem.Click += new System.EventHandler(this.undoToolStripMenuItem_Click);
            // 
            // viewToolStripMenuItem
            // 
            this.viewToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.viewMenuFileLocation,
            this.viewMenuHideLocation});
            this.viewToolStripMenuItem.Name = "viewToolStripMenuItem";
            this.viewToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.viewToolStripMenuItem.Text = "View";
            // 
            // viewMenuFileLocation
            // 
            this.viewMenuFileLocation.Enabled = false;
            this.viewMenuFileLocation.Name = "viewMenuFileLocation";
            this.viewMenuFileLocation.Size = new System.Drawing.Size(173, 22);
            this.viewMenuFileLocation.Text = "Show File Location";
            this.viewMenuFileLocation.Click += new System.EventHandler(this.viewMenuFileLocation_Click);
            // 
            // viewMenuHideLocation
            // 
            this.viewMenuHideLocation.Name = "viewMenuHideLocation";
            this.viewMenuHideLocation.Size = new System.Drawing.Size(173, 22);
            this.viewMenuHideLocation.Text = "Hide File Location";
            this.viewMenuHideLocation.Click += new System.EventHandler(this.viewMenuHideLocation_Click);
            // 
            // toolsToolStripMenuItem
            // 
            this.toolsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.savingToolStripMenuItem,
            this.cellToolStripMenuItem,
            this.exceptionsToolStripMenuItem,
            this.aboutToolStripMenuItem});
            this.toolsToolStripMenuItem.Name = "toolsToolStripMenuItem";
            this.toolsToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.toolsToolStripMenuItem.Text = "Help";
            // 
            // savingToolStripMenuItem
            // 
            this.savingToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.helpMenuSave,
            this.helpMenuSaveAs});
            this.savingToolStripMenuItem.Name = "savingToolStripMenuItem";
            this.savingToolStripMenuItem.Size = new System.Drawing.Size(130, 22);
            this.savingToolStripMenuItem.Text = "Saving";
            // 
            // helpMenuSave
            // 
            this.helpMenuSave.Name = "helpMenuSave";
            this.helpMenuSave.Size = new System.Drawing.Size(98, 22);
            this.helpMenuSave.Text = "Save";
            this.helpMenuSave.Click += new System.EventHandler(this.helpMenuSave_Click);
            // 
            // helpMenuSaveAs
            // 
            this.helpMenuSaveAs.Name = "helpMenuSaveAs";
            this.helpMenuSaveAs.Size = new System.Drawing.Size(98, 22);
            // 
            // cellToolStripMenuItem
            // 
            this.cellToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.viewMenuCellContents,
            this.valueToolStripMenuItem});
            this.cellToolStripMenuItem.Name = "cellToolStripMenuItem";
            this.cellToolStripMenuItem.Size = new System.Drawing.Size(130, 22);
            this.cellToolStripMenuItem.Text = "Cell";
            // 
            // viewMenuCellContents
            // 
            this.viewMenuCellContents.Name = "viewMenuCellContents";
            this.viewMenuCellContents.Size = new System.Drawing.Size(122, 22);
            this.viewMenuCellContents.Text = "Contents";
            this.viewMenuCellContents.Click += new System.EventHandler(this.viewMenuCellContents_Click);
            // 
            // valueToolStripMenuItem
            // 
            this.valueToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.viewMenuTextBoxValue,
            this.viewMenuCellBoxValue});
            this.valueToolStripMenuItem.Name = "valueToolStripMenuItem";
            this.valueToolStripMenuItem.Size = new System.Drawing.Size(122, 22);
            this.valueToolStripMenuItem.Text = "Value";
            // 
            // viewMenuTextBoxValue
            // 
            this.viewMenuTextBoxValue.Name = "viewMenuTextBoxValue";
            this.viewMenuTextBoxValue.Size = new System.Drawing.Size(118, 22);
            this.viewMenuTextBoxValue.Text = "Text Box";
            this.viewMenuTextBoxValue.Click += new System.EventHandler(this.viewMenuTextBoxValue_Click);
            // 
            // viewMenuCellBoxValue
            // 
            this.viewMenuCellBoxValue.Name = "viewMenuCellBoxValue";
            this.viewMenuCellBoxValue.Size = new System.Drawing.Size(118, 22);
            this.viewMenuCellBoxValue.Text = "Cell Box";
            this.viewMenuCellBoxValue.Click += new System.EventHandler(this.viewMenuCellBoxValue_Click);
            // 
            // exceptionsToolStripMenuItem
            // 
            this.exceptionsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.helpMenuInvalidFormula,
            this.helpMenuCircularException,
            this.helpMenuSpreadSheetFormula});
            this.exceptionsToolStripMenuItem.Name = "exceptionsToolStripMenuItem";
            this.exceptionsToolStripMenuItem.Size = new System.Drawing.Size(130, 22);
            this.exceptionsToolStripMenuItem.Text = "Exceptions";
            // 
            // helpMenuInvalidFormula
            // 
            this.helpMenuInvalidFormula.Name = "helpMenuInvalidFormula";
            this.helpMenuInvalidFormula.Size = new System.Drawing.Size(186, 22);
            this.helpMenuInvalidFormula.Text = "Invalid Formula";
            this.helpMenuInvalidFormula.Click += new System.EventHandler(this.helpMenuInvalidFormula_Click);
            // 
            // helpMenuCircularException
            // 
            this.helpMenuCircularException.Name = "helpMenuCircularException";
            this.helpMenuCircularException.Size = new System.Drawing.Size(186, 22);
            this.helpMenuCircularException.Text = "Circular Exception";
            this.helpMenuCircularException.Click += new System.EventHandler(this.helpMenuCircularException_Click);
            // 
            // helpMenuSpreadSheetFormula
            // 
            this.helpMenuSpreadSheetFormula.Name = "helpMenuSpreadSheetFormula";
            this.helpMenuSpreadSheetFormula.Size = new System.Drawing.Size(186, 22);
            this.helpMenuSpreadSheetFormula.Text = "SpreadSheet.Formula";
            this.helpMenuSpreadSheetFormula.Click += new System.EventHandler(this.helpMenuSpreadSheetFormula_Click);
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(130, 22);
            this.aboutToolStripMenuItem.Text = "About";
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
            // 
            // contents_box
            // 
            this.contents_box.Enabled = false;
            this.contents_box.Location = new System.Drawing.Point(74, 57);
            this.contents_box.Name = "contents_box";
            this.contents_box.Size = new System.Drawing.Size(170, 20);
            this.contents_box.TabIndex = 4;
            this.contents_box.KeyDown += new System.Windows.Forms.KeyEventHandler(this.update_key_press);
            this.contents_box.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.update_key_press);
            // 
            // position_label
            // 
            this.position_label.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.position_label.AutoSize = true;
            this.position_label.Location = new System.Drawing.Point(12, 364);
            this.position_label.Name = "position_label";
            this.position_label.Size = new System.Drawing.Size(0, 13);
            this.position_label.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.label1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label1.Location = new System.Drawing.Point(8, 60);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(60, 15);
            this.label1.TabIndex = 5;
            this.label1.Text = " Contents  ";
            // 
            // value_label
            // 
            this.value_label.AutoSize = true;
            this.value_label.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.value_label.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.value_label.Location = new System.Drawing.Point(253, 62);
            this.value_label.Name = "value_label";
            this.value_label.Size = new System.Drawing.Size(52, 15);
            this.value_label.TabIndex = 6;
            this.value_label.Text = "A1 Value";
            // 
            // value_box
            // 
            this.value_box.Enabled = false;
            this.value_box.Location = new System.Drawing.Point(311, 59);
            this.value_box.Name = "value_box";
            this.value_box.Size = new System.Drawing.Size(82, 20);
            this.value_box.TabIndex = 7;
            // 
            // ipAddressLabel
            // 
            this.ipAddressLabel.AutoSize = true;
            this.ipAddressLabel.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.ipAddressLabel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ipAddressLabel.Location = new System.Drawing.Point(8, 36);
            this.ipAddressLabel.Name = "ipAddressLabel";
            this.ipAddressLabel.Size = new System.Drawing.Size(60, 15);
            this.ipAddressLabel.TabIndex = 8;
            this.ipAddressLabel.Text = "IP Address";
            // 
            // ip_address_box
            // 
            this.ip_address_box.Location = new System.Drawing.Point(74, 33);
            this.ip_address_box.Name = "ip_address_box";
            this.ip_address_box.Size = new System.Drawing.Size(170, 20);
            this.ip_address_box.TabIndex = 9;
            this.ip_address_box.Text = "155.98.111.69";
            this.ip_address_box.TextChanged += new System.EventHandler(this.ip_address_box_TextChanged);
            // 
            // connect_button
            // 
            this.connect_button.Location = new System.Drawing.Point(399, 30);
            this.connect_button.Name = "connect_button";
            this.connect_button.Size = new System.Drawing.Size(93, 23);
            this.connect_button.TabIndex = 10;
            this.connect_button.Text = "Connect";
            this.connect_button.UseVisualStyleBackColor = true;
            this.connect_button.Click += new System.EventHandler(this.connectToolStripMenuItem_Click);
            // 
            // connected_label1
            // 
            this.connected_label1.AutoSize = true;
            this.connected_label1.BackColor = System.Drawing.SystemColors.Control;
            this.connected_label1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.connected_label1.ForeColor = System.Drawing.Color.Red;
            this.connected_label1.Location = new System.Drawing.Point(498, 36);
            this.connected_label1.Name = "connected_label1";
            this.connected_label1.Size = new System.Drawing.Size(97, 15);
            this.connected_label1.TabIndex = 11;
            this.connected_label1.Text = "Connected: Offline";
            // 
            // MessageConsoletextBox
            // 
            this.MessageConsoletextBox.Enabled = false;
            this.MessageConsoletextBox.Location = new System.Drawing.Point(498, 59);
            this.MessageConsoletextBox.Name = "MessageConsoletextBox";
            this.MessageConsoletextBox.Size = new System.Drawing.Size(254, 20);
            this.MessageConsoletextBox.TabIndex = 12;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.label2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label2.Location = new System.Drawing.Point(399, 62);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(93, 15);
            this.label2.TabIndex = 13;
            this.label2.Text = "Message Console";
            // 
            // port_label
            // 
            this.port_label.AutoSize = true;
            this.port_label.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.port_label.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.port_label.Location = new System.Drawing.Point(253, 35);
            this.port_label.Name = "port_label";
            this.port_label.Size = new System.Drawing.Size(52, 15);
            this.port_label.TabIndex = 14;
            this.port_label.Text = "    Port    ";
            // 
            // port_text_box
            // 
            this.port_text_box.Location = new System.Drawing.Point(311, 32);
            this.port_text_box.MaxLength = 4;
            this.port_text_box.Name = "port_text_box";
            this.port_text_box.Size = new System.Drawing.Size(82, 20);
            this.port_text_box.TabIndex = 15;
            this.port_text_box.Text = "1984";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(759, 386);
            this.Controls.Add(this.port_text_box);
            this.Controls.Add(this.port_label);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.MessageConsoletextBox);
            this.Controls.Add(this.connected_label1);
            this.Controls.Add(this.connect_button);
            this.Controls.Add(this.ip_address_box);
            this.Controls.Add(this.ipAddressLabel);
            this.Controls.Add(this.value_box);
            this.Controls.Add(this.value_label);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.position_label);
            this.Controls.Add(this.contents_box);
            this.Controls.Add(this.spreadsheetPanel1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Form1";
            this.Text = "DangerZone";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private SpreadsheetPanel spreadsheetPanel1;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem newToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.TextBox contents_box;
        private System.Windows.Forms.Label position_label;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label value_label;
        private System.Windows.Forms.TextBox value_box;
        private System.Windows.Forms.ToolStripMenuItem toolsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem savingToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpMenuSave;
        private System.Windows.Forms.ToolStripMenuItem helpMenuSaveAs;
        private System.Windows.Forms.ToolStripMenuItem viewToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem viewMenuFileLocation;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem viewMenuHideLocation;
        private System.Windows.Forms.ToolStripMenuItem cellToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem viewMenuCellContents;
        private System.Windows.Forms.ToolStripMenuItem valueToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem viewMenuTextBoxValue;
        private System.Windows.Forms.ToolStripMenuItem viewMenuCellBoxValue;
        private System.Windows.Forms.ToolStripMenuItem exceptionsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpMenuInvalidFormula;
        private System.Windows.Forms.ToolStripMenuItem helpMenuCircularException;
        private System.Windows.Forms.ToolStripMenuItem helpMenuSpreadSheetFormula;
        private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem undoToolStripMenuItem;
        private System.Windows.Forms.Label ipAddressLabel;
        private System.Windows.Forms.TextBox ip_address_box;
        private System.Windows.Forms.Button connect_button;
        private System.Windows.Forms.Label connected_label1;
        private System.Windows.Forms.ToolStripMenuItem leaveSessionToolStripMenuItem;
        private System.Windows.Forms.TextBox MessageConsoletextBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label port_label;
        private System.Windows.Forms.TextBox port_text_box;
    }
}

