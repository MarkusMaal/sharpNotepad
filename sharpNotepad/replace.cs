using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace sharpNotepad
{
    public partial class replace : Form
    {
        public replace()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (checkBox1.Checked == true) { 
                Program.mainform.textBox1.Text = Program.mainform.textBox1.Text.Replace(textBox1.Text, textBox2.Text);
            }
            else
            {
                Program.mainform.textBox1.Text = Program.mainform.textBox1.Text.Replace(textBox1.Text.ToLower(), textBox2.Text.ToLower());
            }
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void replace_Load(object sender, EventArgs e)
        {
            this.BackColor = Program.mainform.newToolStripMenuItem.BackColor;
            this.ForeColor = Program.mainform.newToolStripMenuItem.ForeColor;
        }
    }
}
