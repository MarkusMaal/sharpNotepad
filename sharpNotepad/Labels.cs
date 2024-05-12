using System;
using System.Windows.Forms;

namespace sharpNotepad
{
    public partial class Labels : Form
    {
        public Labels()
        {
            InitializeComponent();
        }

        private void Labels_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Hide();
            Program.mainform.sildidToolStripMenuItem.Checked = false;
            e.Cancel = true;
        }

        private void listView1_ItemActivate(object sender, EventArgs e)
        {
            Program.mainform.textBox1.SelectionStart = Program.mainform.textBox1.GetFirstCharIndexFromLine(Convert.ToInt32(listView1.SelectedItems[0].SubItems[1].Text.ToString()) - 1);
            Program.mainform.textBox1.ScrollToCaret();
        }
    }
}
