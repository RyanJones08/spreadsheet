namespace SpreadsheetGUI
{
    partial class LogInWindow
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
            this.CreateButton = new System.Windows.Forms.Button();
            this.IPText = new System.Windows.Forms.TextBox();
            this.IPLabel = new System.Windows.Forms.Label();
            this.SSNameLabel = new System.Windows.Forms.Label();
            this.SSNameText = new System.Windows.Forms.TextBox();
            this.SSPassLabel = new System.Windows.Forms.Label();
            this.SSPassText = new System.Windows.Forms.TextBox();
            this.JoinButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // CreateButton
            // 
            this.CreateButton.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.CreateButton.Enabled = false;
            this.CreateButton.Font = new System.Drawing.Font("Times New Roman", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CreateButton.ForeColor = System.Drawing.SystemColors.WindowText;
            this.CreateButton.Location = new System.Drawing.Point(22, 250);
            this.CreateButton.Name = "CreateButton";
            this.CreateButton.Size = new System.Drawing.Size(140, 54);
            this.CreateButton.TabIndex = 3;
            this.CreateButton.Text = "Create";
            this.CreateButton.UseVisualStyleBackColor = true;
            this.CreateButton.Click += new System.EventHandler(this.CreateButton_Click);
            // 
            // IPText
            // 
            this.IPText.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.IPText.Location = new System.Drawing.Point(69, 35);
            this.IPText.Name = "IPText";
            this.IPText.Size = new System.Drawing.Size(239, 20);
            this.IPText.TabIndex = 4;
            this.IPText.Text = "155.98.111.69";
            this.IPText.TextChanged += new System.EventHandler(this.IPText_TextChanged);
            // 
            // IPLabel
            // 
            this.IPLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.IPLabel.AutoSize = true;
            this.IPLabel.Location = new System.Drawing.Point(160, 19);
            this.IPLabel.Name = "IPLabel";
            this.IPLabel.Size = new System.Drawing.Size(58, 13);
            this.IPLabel.TabIndex = 2;
            this.IPLabel.Text = "IP Address";
            // 
            // SSNameLabel
            // 
            this.SSNameLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.SSNameLabel.AutoSize = true;
            this.SSNameLabel.Location = new System.Drawing.Point(160, 83);
            this.SSNameLabel.Name = "SSNameLabel";
            this.SSNameLabel.Size = new System.Drawing.Size(54, 13);
            this.SSNameLabel.TabIndex = 3;
            this.SSNameLabel.Text = "File Name";
            // 
            // SSNameText
            // 
            this.SSNameText.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.SSNameText.Location = new System.Drawing.Point(69, 99);
            this.SSNameText.Name = "SSNameText";
            this.SSNameText.Size = new System.Drawing.Size(235, 20);
            this.SSNameText.TabIndex = 1;
            this.SSNameText.TextChanged += new System.EventHandler(this.SSNameText_TextChanged);
            // 
            // SSPassLabel
            // 
            this.SSPassLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.SSPassLabel.AutoSize = true;
            this.SSPassLabel.Location = new System.Drawing.Point(160, 150);
            this.SSPassLabel.Name = "SSPassLabel";
            this.SSPassLabel.Size = new System.Drawing.Size(53, 13);
            this.SSPassLabel.TabIndex = 5;
            this.SSPassLabel.Text = "Password";
            // 
            // SSPassText
            // 
            this.SSPassText.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.SSPassText.ForeColor = System.Drawing.SystemColors.WindowText;
            this.SSPassText.Location = new System.Drawing.Point(67, 166);
            this.SSPassText.Name = "SSPassText";
            this.SSPassText.Size = new System.Drawing.Size(237, 20);
            this.SSPassText.TabIndex = 2;
            this.SSPassText.TextChanged += new System.EventHandler(this.SSPassText_TextChanged);
            // 
            // JoinButton
            // 
            this.JoinButton.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.JoinButton.Enabled = false;
            this.JoinButton.Font = new System.Drawing.Font("Times New Roman", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.JoinButton.ForeColor = System.Drawing.SystemColors.WindowText;
            this.JoinButton.Location = new System.Drawing.Point(195, 250);
            this.JoinButton.Name = "JoinButton";
            this.JoinButton.Size = new System.Drawing.Size(140, 54);
            this.JoinButton.TabIndex = 6;
            this.JoinButton.Text = "Join";
            this.JoinButton.UseVisualStyleBackColor = true;
            this.JoinButton.Click += new System.EventHandler(this.join_button_click);
            // 
            // LogInWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(369, 331);
            this.Controls.Add(this.JoinButton);
            this.Controls.Add(this.SSPassText);
            this.Controls.Add(this.SSPassLabel);
            this.Controls.Add(this.SSNameText);
            this.Controls.Add(this.SSNameLabel);
            this.Controls.Add(this.IPLabel);
            this.Controls.Add(this.IPText);
            this.Controls.Add(this.CreateButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(375, 216);
            this.Name = "LogInWindow";
            this.Text = "Log In Window";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button CreateButton;
        private System.Windows.Forms.TextBox IPText;
        private System.Windows.Forms.Label IPLabel;
        private System.Windows.Forms.Label SSNameLabel;
        private System.Windows.Forms.TextBox SSNameText;
        private System.Windows.Forms.Label SSPassLabel;
        private System.Windows.Forms.TextBox SSPassText;
        private System.Windows.Forms.Button JoinButton;
    }
}