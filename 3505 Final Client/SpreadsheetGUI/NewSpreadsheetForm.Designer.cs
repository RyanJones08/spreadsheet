namespace SS
{
    partial class NewSpreadsheetForm
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
            this.NewFilenameTextbox = new System.Windows.Forms.TextBox();
            this.NewPasswordTextbox = new System.Windows.Forms.TextBox();
            this.CreateButton = new System.Windows.Forms.Button();
            this.NewFilenameLabel = new System.Windows.Forms.Label();
            this.NewPasswordLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // NewFilenameTextbox
            // 
            this.NewFilenameTextbox.Location = new System.Drawing.Point(66, 53);
            this.NewFilenameTextbox.Name = "NewFilenameTextbox";
            this.NewFilenameTextbox.Size = new System.Drawing.Size(231, 20);
            this.NewFilenameTextbox.TabIndex = 0;
            this.NewFilenameTextbox.TextChanged += new System.EventHandler(this.NewFilenameTextbox_TextChanged);
            // 
            // NewPasswordTextbox
            // 
            this.NewPasswordTextbox.Location = new System.Drawing.Point(66, 126);
            this.NewPasswordTextbox.Name = "NewPasswordTextbox";
            this.NewPasswordTextbox.Size = new System.Drawing.Size(231, 20);
            this.NewPasswordTextbox.TabIndex = 1;
            this.NewPasswordTextbox.TextChanged += new System.EventHandler(this.NewPasswordTextbox_TextChanged);
            // 
            // CreateButton
            // 
            this.CreateButton.Enabled = false;
            this.CreateButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CreateButton.Location = new System.Drawing.Point(101, 208);
            this.CreateButton.Name = "CreateButton";
            this.CreateButton.Size = new System.Drawing.Size(150, 46);
            this.CreateButton.TabIndex = 2;
            this.CreateButton.Text = "Create";
            this.CreateButton.UseVisualStyleBackColor = true;
            this.CreateButton.Click += new System.EventHandler(this.CreateButton_Click);
            // 
            // NewFilenameLabel
            // 
            this.NewFilenameLabel.AutoSize = true;
            this.NewFilenameLabel.Location = new System.Drawing.Point(148, 37);
            this.NewFilenameLabel.Name = "NewFilenameLabel";
            this.NewFilenameLabel.Size = new System.Drawing.Size(54, 13);
            this.NewFilenameLabel.TabIndex = 3;
            this.NewFilenameLabel.Text = "File Name";
            // 
            // NewPasswordLabel
            // 
            this.NewPasswordLabel.AutoSize = true;
            this.NewPasswordLabel.Location = new System.Drawing.Point(148, 110);
            this.NewPasswordLabel.Name = "NewPasswordLabel";
            this.NewPasswordLabel.Size = new System.Drawing.Size(53, 13);
            this.NewPasswordLabel.TabIndex = 4;
            this.NewPasswordLabel.Text = "Password";
            // 
            // NewSpreadsheetForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(359, 303);
            this.Controls.Add(this.NewPasswordLabel);
            this.Controls.Add(this.NewFilenameLabel);
            this.Controls.Add(this.CreateButton);
            this.Controls.Add(this.NewPasswordTextbox);
            this.Controls.Add(this.NewFilenameTextbox);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "NewSpreadsheetForm";
            this.Text = "New Spreadsheet";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox NewFilenameTextbox;
        private System.Windows.Forms.TextBox NewPasswordTextbox;
        private System.Windows.Forms.Button CreateButton;
        private System.Windows.Forms.Label NewFilenameLabel;
        private System.Windows.Forms.Label NewPasswordLabel;
    }
}