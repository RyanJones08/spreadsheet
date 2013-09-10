using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace SS
{
    class SpreadsheetApplicationContext : ApplicationContext
    {
        private int form_count = 0;

        private static SpreadsheetApplicationContext appContext;

        /// <summary>
        /// Private constructor for singleton pattern
        /// </summary>
        private SpreadsheetApplicationContext()
        {
        }

        /// <summary>
        /// Returns the one DemoApplicationContext.
        /// </summary>
        public static SpreadsheetApplicationContext getAppContext()
        {
            if (appContext == null)
            {
                appContext = new SpreadsheetApplicationContext();
            }
            return appContext;
        }
        public int get_form_count()
        {
            return form_count;
        }
        /// <summary>
        /// Runs the form
        /// </summary>
        public void RunForm(Form form)
        {
            // One more form is running
            form_count++;

            // When this form closes, we want to find out
            form.FormClosed += (o, e) => { if (--form_count <= 0) ExitThread(); };

            // Run the form
            form.Show();
        }
    }
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            
            SpreadsheetApplicationContext appContext = SpreadsheetApplicationContext.getAppContext();
            appContext.RunForm(new Form1());
            Application.Run(appContext);
        }
    }
}
