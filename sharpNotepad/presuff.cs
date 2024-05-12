using System;
using System.Windows.Forms;

namespace sharpNotepad
{
    public partial class presuff : Form
    {
        public presuff()
        {
            InitializeComponent();
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton2.Checked == true)
            {
                label4.Text = "Sufiks";
            }
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton1.Checked == true)
            { 
                label4.Text = "Prefiks";
            }
        }

        private void presuff_Shown(object sender, EventArgs e)
        {
            comboBox1.Items.Clear();
            comboBox2.Items.Clear();
            comboBox3.Items.Clear();
            string linecount;
            linecount = Program.mainform.toolStripStatusLabel1.Text.Replace("Rida: ", "");
            string[] lines = linecount.Split('/');
            int totalines = Convert.ToInt32(lines[1]);
            for (int i = 1; i <= totalines; i++)
            {
                comboBox1.Items.Add(i.ToString());
                comboBox2.Items.Add(i.ToString());
            }
            for (int i = 1; i <= totalines / 2; i++)
            {
                comboBox3.Items.Add(i.ToString());
            }
            comboBox1.SelectedIndex = 0;
            comboBox2.SelectedIndex = 0;
            if (comboBox3.Items.Count > 0)
            { 
                comboBox3.SelectedIndex = 0;
            } else
            {
                MessageBox.Show("Dokument on liiga lühike", "Toiming ebaõnnestus", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string[] textlines = Program.mainform.textBox1.Text.Split('\n');
            ListBox sequence = new ListBox();
            int start = Convert.ToInt32(comboBox1.SelectedItem.ToString()) - 1;
            int end = Convert.ToInt32(comboBox2.SelectedItem.ToString());
            int step = Convert.ToInt32(comboBox3.SelectedItem.ToString());
            for (int i = start; i < end; i = i + step)
            {
                sequence.Items.Add(i);
            }
            int sequenceindex = 0;
            string newtext = "";
            if (label4.Text == "Sufiks") { Program.mainform.textBox1.Text = ""; }
            Program.mainform.textBox1.Visible = false;
            for (int i = 0; i < textlines.Length; i++)
            {
                if (sequenceindex < sequence.Items.Count) { 
                    if (i.ToString() == sequence.Items[sequenceindex].ToString())
                    {
                        sequenceindex++;
                        if (label4.Text == "Prefiks") { newtext += textBox1.Text + textlines[i].ToString() + '\n'; }
                        if (label4.Text == "Sufiks") { textlines[i] += textBox1.Text.ToString();}
                        if (label4.Text == "Sufiks") { Program.mainform.textBox1.Text += textlines[i]; }
                        if (label4.Text == "Sufiks") { Program.mainform.textBox1.AppendText(Environment.NewLine); }
                    }
                    else
                    {
                        if (label4.Text == "Prefiks") { newtext += textlines[i].ToString() + '\n'; }
                        if (label4.Text == "Sufiks") { Program.mainform.textBox1.Text += textlines[i].ToString(); }
                        if (label4.Text == "Sufiks") { Program.mainform.textBox1.AppendText(Environment.NewLine); }
                    }
                } else
                {
                    if (label4.Text == "Prefiks") { newtext += textlines[i].ToString() + '\n'; }
                    if (label4.Text == "Sufiks") { Program.mainform.textBox1.Text += textlines[i].ToString(); }
                    if (label4.Text == "Sufiks") { Program.mainform.textBox1.AppendText(Environment.NewLine); }
                }
            }
            if (label4.Text == "Prefiks") { Program.mainform.textBox1.Text = newtext; }
            Program.mainform.textBox1.Visible = true;
            this.Close();
        }

        private void presuff_Load(object sender, EventArgs e)
        {
            this.BackColor = Program.mainform.newToolStripMenuItem.BackColor;
            this.ForeColor = Program.mainform.newToolStripMenuItem.ForeColor;
        }
    }
}
