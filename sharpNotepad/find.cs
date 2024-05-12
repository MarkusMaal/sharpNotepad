using System;
using System.Windows.Forms;

namespace sharpNotepad
{
    public partial class find : Form
    {
        public find()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            Program.mainform.findstring = textBox1.Text;
            this.Close();
        }

        private void find_Load(object sender, EventArgs e)
        {
            this.BackColor = Program.mainform.newToolStripMenuItem.BackColor;
            this.ForeColor = Program.mainform.newToolStripMenuItem.ForeColor;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void find_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked == true)
            {
                Program.mainform.countcase = true;
            }
            else if (checkBox1.Checked == false)
            {
                Program.mainform.countcase = false;
            }
        }
    }

//    public static string getBetween(string strSource, string strStart, string strEnd)
  //  {
    //    const int kNotFound = -1;
    //
      //  var startIdx = strSource.IndexOf(strStart);
        //if (startIdx != kNotFound)
        //{
          //  startIdx += strStart.Length;
           // var endIdx = strSource.IndexOf(strEnd, startIdx);
            //if (endIdx > startIdx)
           // {
            //    return strSource.Substring(startIdx, endIdx - startIdx);
           // }
        //}
        //return String.Empty;
   // }
}
