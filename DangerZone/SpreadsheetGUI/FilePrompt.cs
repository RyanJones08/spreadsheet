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
    public partial class FilePrompt : Form
    {
        Form parent_form;
        FormClosingEventArgs even;
        public FilePrompt(string caption, Form form, FormClosingEventArgs eve)
        {
            even = eve;
            parent_form = form;
            InitializeComponent();
            this.Text = caption;
        }
        public FilePrompt(string caption, Form form)
        {
            parent_form = form;
            InitializeComponent();
            this.Text = caption;
        }
        private void FilePrompt_Load(object sender, EventArgs e)
        {

        }
        private void Yes_Click(object sender, EventArgs e)
        {
            parent_form.Close();
        }

        private void No_Click(object sender, EventArgs e)
        {
            if (even != null)
            {
                even.Cancel = true;
            }
            Close();
        }
    }
}
