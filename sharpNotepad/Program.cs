using System;
using System.Windows.Forms;
using System.IO;

namespace sharpNotepad
{
    static class Program
    {
        internal static string text;
        internal static string fnm;
        public static ReadMode readmode = new ReadMode();
        public static Form1 mainform = new Form1();
        public static find findform = new find();
        public static replace changeform = new replace();
        public static presuff ps = new presuff();
        public static AboutBox1 ab = new AboutBox1();
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
           if (args != null && args.Length > 0)
            {
                string filename = args[0];
                if (File.Exists(filename))
                {
                    text = File.ReadAllText(filename);
                    fnm = filename;
                    Application.EnableVisualStyles();
                    Application.Run(mainform);
                }
                else
                {
                    
                    Application.EnableVisualStyles();
                    // Application.SetCompatibleTextRenderingDefault(false);
                    MessageBox.Show("Järgnevat faili ei eksisteeri: " + filename, "Tõrge", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                }
            }
            else
            {
                Application.EnableVisualStyles();
                Application.Run(mainform);
            }
        }
            }
}
