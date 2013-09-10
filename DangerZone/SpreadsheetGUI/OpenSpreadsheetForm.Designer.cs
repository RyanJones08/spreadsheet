namespace SS
{
    partial class OpenSpreadsheetForm
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
            this.OpenPasswordLabel = new System.Windows.Forms.Label();
            this.OpenFilenameLabel = new System.Windows.Forms.Label();
            this.JoinButton = new System.Windows.Forms.Button();
            this.OpenPasswordTextbox = new System.Windows.Forms.TextBox();
            this.OpenFilenameTextbox = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // OpenPasswordLabel
            // 
            this.OpenPasswordLabel.AutoSize = true;
            this.OpenPasswordLabel.Location = new System.Drawing.Point(147, 119);
            this.OpenPasswordLabel.Name = "OpenPasswordLabel";
            this.OpenPasswordLabel.Size = new System.Drawing.Size(53, 13);
            this.OpenPasswordLabel.TabIndex = 9;
            this.OpenPasswordLabel.Text = "Password";
            // 
            // OpenFilenameLabel
            // 
            this.OpenFilenameLabel.AutoSize = true;
            this.OpenFilenameLabel.Location = new System.Drawing.Point(147, 46);
            this.OpenFilenameLabel.Name = "OpenFilenameLabel";
            this.OpenFilenameLabel.Size = new System.Drawing.Size(54, 13);
            this.OpenFilenameLabel.TabIndex = 8;
            this.OpenFilenameLabel.Text = "File Name";
            // 
            // JoinButton
            // 
            this.JoinButton.Enabled = false;
            this.JoinButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.JoinButton.Location = new System.Drawing.Point(100, 217);
            this.JoinButton.Name = "JoinButton";
            this.JoinButton.Size = new System.Drawing.Size(150, 46);
            this.JoinButton.TabIndex = 7;
            this.JoinButton.Text = "Join";
            this.JoinButton.UseVisualStyleBackColor = true;
            this.JoinButton.Click += new System.EventHandler(this.JoinButton_Click);
            // 
            // OpenPasswordTextbox
            // 
            this.OpenPasswordTextbox.Location = new System.Drawing.Point(65, 135);
            this.OpenPasswordTextbox.Name = "OpenPasswordTextbox";
            this.OpenPasswordTextbox.Size = new System.Drawing.Size(231, 20);
            this.OpenPasswordTextbox.TabIndex = 6;
            this.OpenPasswordTextbox.TextChanged += new System.EventHandler(this.OpenPasswordTextbox_TextChanged);
            // 
            // OpenFilenameTextbox
            // 
            this.OpenFilenameTextbox.Location = new System.Drawing.Point(65, 62);
            this.OpenFilenameTextbox.Name = "OpenFilenameTextbox";
            this.OpenFilenameTextbox.Size = new System.Drawing.Size(231, 20);
            this.OpenFilenameTextbox.TabIndex = 5;
            this.OpenFilenameTextbox.TextChanged += new System.EventHandler(this.OpenFilenameTextbox_TextChanged);
            // 
            // OpenSpreadsheetForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(359, 303);
            this.Controls.Add(this.OpenPasswordLabel);
            this.Controls.Add(this.OpenFilenameLabel);
            this.Controls.Add(this.JoinButton);
            this.Controls.Add(this.OpenPasswordTextbox);
            this.Controls.Add(this.OpenFilenameTextbox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "OpenSpreadsheetForm";
            this.Text = "Open Spreadsheet";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label OpenPasswordLabel;
        private System.Windows.Forms.Label OpenFilenameLabel;
        private System.Windows.Forms.Button JoinButton;
        private System.Windows.Forms.TextBox OpenPasswordTextbox;
        private System.Windows.Forms.TextBox OpenFilenameTextbox;
    }
}