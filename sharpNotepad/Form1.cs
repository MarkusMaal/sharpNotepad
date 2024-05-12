using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;
using System.Drawing.Printing;
using System.Text.RegularExpressions;
using System.Runtime.InteropServices;

namespace sharpNotepad
{
    public partial class Form1 : Form
    {
        public bool countcase = true;
        public string findstring = "";
        int findindex = 0;
        int lineindex = 0;
        int lastlocation = 0;
        int line = 1;
        PrintDocument prd = new PrintDocument();
        Encoding en = Encoding.UTF8;
        Labels l = new Labels();
        RichTextBox rt = new RichTextBox();
        int caret = 2;
        WaitForm wf;
        Point mousePos = new Point();
        bool insertmode = false;
        static int idx = 0;
        static int startidx = 0;
        static int endidx = 0;

        bool usewinsplit = false;
    
        public Form1()
        {
            InitializeComponent();
            prd.DefaultPageSettings = pageSetupDialog2.PageSettings;
            prd.PrintPage += new PrintPageEventHandler(document_PrintPage);
        }
        void document_PrintPage(object sender, PrintPageEventArgs e)
        {
            e.Graphics.FillRectangle(new SolidBrush(textBox1.BackColor), 0, 0, e.PageBounds.Width, e.PageBounds.Height);
            e.Graphics.DrawString(textBox1.Text, textBox1.Font, new SolidBrush(textBox1.ForeColor), new Point(e.MarginBounds.X, e.MarginBounds.Y));
        }
        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            textBox1.Text = "";
            textBox1.BackColor = Color.White;
            textBox1.ForeColor = Color.Black;
            textBox1.Font = textBox2.Font;
            textBox2.Text = "";
            saveToolStripMenuItem.Enabled = false;
            this.Text = "Kiirmärkmik - newdocument.txt";
            openDialog.FileName = "";
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openDialog.ShowDialog();
            if (openDialog.FileName != "")
            {
                if (File.Exists(openDialog.FileName))
                {
                    if ((openDialog.FileName.Contains(".bat")) || (openDialog.FileName.Contains(".cmd")))
                    {
                        en = Encoding.GetEncoding("windows-1252");
                    } else
                    { 
                        using (StreamReader sr = new StreamReader(openDialog.FileName, true))
                        {
                            en = sr.CurrentEncoding;
                        }
                    }
                    string fileconent = File.ReadAllText(openDialog.FileName, en);
                    if (fileconent.Replace("\r\n", "") != fileconent)
                    {
                        usewinsplit = true;
                    } else
                    {
                        usewinsplit = false;
                    }
                    textBox2.Text = fileconent;
                    textBox1.Text = fileconent;
                    saveToolStripMenuItem.Enabled = true;
                    this.Text = "Kiirmärkmik - " + openDialog.SafeFileName;

                }
            }
            openDialog.FilterIndex = 1;
            rt.Font = fontWindow.Font;
            textBox1.Font = fontWindow.Font;
            textBox1.Visible = false;
            timer2.Enabled = true;
            wf = new WaitForm();
            wf.StartPosition = FormStartPosition.Manual;
            wf.Location = new Point(this.Location.X + Convert.ToInt32(this.Size.Width / 2) - Convert.ToInt32(wf.Size.Width / 2), this.Location.Y + Convert.ToInt32(this.Size.Height / 2) - Convert.ToInt32(wf.Size.Height / 2));
            wf.BackColor = textBox1.BackColor;
            wf.ForeColor = textBox1.ForeColor;
            wf.label1.ForeColor = textBox1.ForeColor;
            wf.Show();
        }

        void updateOne(int eval)
        {
            idx = 0;
            startidx = eval;
            foreach (string line in textBox1.Lines)
            {
                if (idx == startidx)
                {
                    int start = textBox1.GetFirstCharIndexFromLine(idx);
                    int end = textBox1.GetFirstCharIndexFromLine(idx + 1) - 1;
                    int selen = end - start;
                    textBox1.Focus();
                    textBox1.SelectionStart = start;
                    try { textBox1.SelectionLength = selen; } catch { }
                    if (textBox1.SelectedText.StartsWith("::"))
                    {
                        textBox1.SelectionColor = Color.Green;
                        textBox1.DeselectAll();
                        textBox1.SelectionStart = start;
                        try { textBox1.SelectionLength = selen; } catch { }
                        textBox1.SelectionFont = new Font(textBox1.Font.FontFamily, textBox1.Font.Size, FontStyle.Italic);
                    }
                    else if (textBox1.SelectedText.StartsWith(":"))
                    {
                        textBox1.SelectionBackColor = Color.White;
                        textBox1.DeselectAll();
                        textBox1.SelectionStart = start;
                        try { textBox1.SelectionLength = selen; } catch { }
                        textBox1.SelectionColor = Color.Blue;
                        textBox1.DeselectAll();
                        textBox1.SelectionStart = start;
                        try { textBox1.SelectionLength = selen; } catch { }
                        textBox1.SelectionFont = new Font(textBox1.Font.FontFamily, textBox1.Font.Size, FontStyle.Bold);
                    }
                    idx++;
                }
                else
                {
                    idx++;
                }
            }
        }
        void updateFormat(int eval = -64)
        {
            return;
            if (eval == -64)
            {
                eval = textBox1.GetLineFromCharIndex(textBox1.SelectionStart);
            }
            idx = 0;
            startidx = eval - 15;
            if (startidx < 0) { startidx = 0; }
            endidx = eval + 15;
            foreach (string line in textBox1.Lines)
            {
                if (idx >= startidx) { 
                    if (idx < endidx)
                    {
                        int start = textBox1.GetFirstCharIndexFromLine(idx);
                        int end = textBox1.GetFirstCharIndexFromLine(idx + 1) - 1;
                        int selen = end - start;
                        textBox1.Focus();
                        textBox1.SelectionStart = start;
                        try { textBox1.SelectionLength = selen; } catch { }
                        if (textBox1.SelectedText.StartsWith("::"))
                        {
                            textBox1.SelectionColor = Color.Green;
                            textBox1.DeselectAll();
                            textBox1.SelectionStart = start;
                            try { textBox1.SelectionLength = selen; } catch { }
                            textBox1.SelectionFont = new Font(textBox1.Font.FontFamily, textBox1.Font.Size, FontStyle.Italic);
                        }
                        else if (textBox1.SelectedText.StartsWith(":"))
                        {
                            textBox1.SelectionBackColor = Color.White;
                            textBox1.DeselectAll();
                            textBox1.SelectionStart = start;
                            try { textBox1.SelectionLength = selen; } catch { }
                            textBox1.SelectionColor = Color.Blue;
                            textBox1.DeselectAll();
                            textBox1.SelectionStart = start;
                            try { textBox1.SelectionLength = selen; } catch { }
                            textBox1.SelectionFont = new Font(textBox1.Font.FontFamily, textBox1.Font.Size, FontStyle.Bold);
                        }
                        idx++;
                    }
                    else
                    {
                        break;
                    }
                } else
                {
                    idx++;
                }
            }
        }

        void scrollBar()
        {
            // get the first visible char index
            int firstVisibleChar = textBox1.GetCharIndexFromPosition(new Point(0, 0));
            //get the line index from the char index
            int lineIndex = textBox1.GetLineFromCharIndex(firstVisibleChar);
            textBox3.Text = "";
            if (textBox1.GetLineFromCharIndex(textBox1.SelectionStart) + 128 < textBox1.Lines.Length)
            {
                for (int i = lineIndex; i < lineIndex + 128; i++)
                {
                    textBox3.Text += (i + 1).ToString() + Environment.NewLine;
                }
            }
            else
            {
                for (int i = lineIndex ; i < textBox1.Lines.Length; i++)
                {
                    textBox3.Text += (i + 1).ToString() + Environment.NewLine;
                }
            }
        }

        private void fontToolStripMenuItem_Click(object sender, EventArgs e)
        {
            fontWindow.ShowDialog();
            int bck = textBox1.SelectionStart;
            int bck2 = textBox1.SelectionLength;
            textBox1.SelectAll();
            textBox1.SelectionFont = fontWindow.Font;
            textBox1.DeselectAll();
            updateFormat();
            textBox1.Font = fontWindow.Font;
            textBox3.Font = textBox1.Font;
            textBox1.SelectionStart = bck;
            textBox1.SelectionLength = bck2;
            updateFormat();

        }

        private void statusBarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (statusBar.Visible == true)
            {
                statusBar.Visible = false;
                statusBarToolStripMenuItem.CheckState = CheckState.Unchecked;
                timer1.Enabled = false;
                textBox1.WordWrap = false;
                wordwrapToolStripMenuItem.Checked = true;
            }
            else
            {
                statusBar.Visible = true;
                statusBarToolStripMenuItem.CheckState = CheckState.Checked;
                timer1.Enabled = true;
                textBox1.WordWrap = true;
                wordwrapToolStripMenuItem.Checked = false;
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void cutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            textBox1.Cut();
        }

        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            textBox1.Copy();
        }

        private void pasteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            textBox1.Paste();
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void selectAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            textBox1.SelectAll();
        }

        private void insertDatetimeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            textBox1.Text = textBox1.Text + DateTime.Today.ToString();
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (saveDialog.ShowDialog() == DialogResult.OK)
                {
                    if (usewinsplit)
                    { 
                        string[] lines = textBox1.Lines;
                        List<string> list_of_string = new List<string>();
                        foreach (string line in lines)
                        {
                            list_of_string.Add(line.Replace("\n", "\r\n"));
                        }
                        File.WriteAllLines(saveDialog.FileName, list_of_string, en);
                    }
                    else
                    {
                        string[] lines = textBox1.Lines;
                        List<string> list_of_string = new List<string>();
                        foreach (string line in lines)
                        {
                            list_of_string.Add(line.Replace("\r\n", "\n"));
                        }
                        File.WriteAllLines(saveDialog.FileName, list_of_string, en);
                    }
                    openDialog.FileName = saveDialog.FileName;
                }
            }
            catch
            {
                MessageBox.Show("Tõrge", "Midagi läks valesti", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void aboutNotepadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Program.ab.ShowDialog();
        }

        private void backgroundToolStripMenuItem_Click(object sender, EventArgs e)
        {
            colorDialog.ShowDialog();
            textBox1.BackColor = colorDialog.Color;
            eT();
        }

        private void foregroundToolStripMenuItem_Click(object sender, EventArgs e)
        {
            colorDialog.ShowDialog();
            textBox1.ForeColor = colorDialog.Color;
            eT();
        }

        private void undoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            textBox1.Undo();
        }

        private void findToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        void eT ()
        {
            textBox1.Visible = false;
            string bck = textBox1.Text;
            textBox1.Rtf = "";
            textBox1.Text = bck;
            lastlocation = textBox1.SelectionStart;
            int idx = 0;
            foreach (String line in textBox1.Lines)
            {
                if (idx < 35) {
                    int start = textBox1.GetFirstCharIndexFromLine(idx);
                    int end = textBox1.GetFirstCharIndexFromLine(idx + 1) - 1;
                    int selen = end - start;
                    textBox1.Focus();
                    textBox1.SelectionStart = start;
                    try { textBox1.SelectionLength = selen; } catch { }
                    if (textBox1.SelectedText.StartsWith("::"))
                    {
                        textBox1.SelectionColor = Color.Green;
                        textBox1.DeselectAll();
                        textBox1.SelectionStart = start;
                        try { textBox1.SelectionLength = selen; } catch { }
                        textBox1.SelectionFont = new Font(fontWindow.Font.FontFamily, fontWindow.Font.Size, FontStyle.Italic);
                    }
                    else if (textBox1.SelectedText.StartsWith(":"))
                    {
                        textBox1.SelectionBackColor = Color.White;
                        textBox1.DeselectAll();
                        textBox1.SelectionStart = start;
                        try { textBox1.SelectionLength = selen; } catch { }
                        textBox1.SelectionColor = Color.Blue;
                        textBox1.DeselectAll();
                        textBox1.SelectionStart = start;
                        try { textBox1.SelectionLength = selen; } catch { }
                        textBox1.SelectionFont = new Font(fontWindow.Font.FontFamily, fontWindow.Font.Size, FontStyle.Bold);
                    }
                }
                idx++;
            }
            textBox1.DeselectAll();
            textBox1.SelectionStart = lastlocation;
            textBox1.Visible = true;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (IsKeyLocked(Keys.Scroll))
            {
                toolStripStatusLabel4.Text = "Kerimisrežiim";
            }
            else
            {
                if (insertmode)
                {
                    toolStripStatusLabel4.Text = "Muutmisrežiim";
                }
                else
                {
                    toolStripStatusLabel4.Text = "Sisestusrežiim";
                }
            }
            if (IsKeyLocked(Keys.Scroll))
            {
                if (!vScrollBar1.Enabled)
                {
                    SendKeys.SendWait("{SCROLLLOCK}");
                    toolStripStatusLabel4.Text = "Ei saa kerida";
                }
            }
            if (usewinsplit)
            {
                toolStripStatusLabel6.Text = "Reavahetus: Windows (CRLF)";
            }
            else
            {
                toolStripStatusLabel6.Text = "Reavahetus: Unix (LF)";
            }
            toolStripStatusLabel5.Text = "Kodeering: " + en.EncodingName;
            textBox1.SelectionFont = fontWindow.Font;
            int xintindex = textBox1.SelectionStart + textBox1.SelectionLength;
            string xindex = xintindex.ToString();
            toolStripStatusLabel2.Text = "Tähemärk: " + xindex;

            string comps = textBox1.SelectedText;
            xintindex = textBox1.SelectionStart;
            string[] textlines = textBox1.Text.Split('\n');
            int currentchr = 0;
            string rida;
            rida = "Rida: 0";

            var SerialsCounter = 0;
            string[] lines = System.Text.RegularExpressions.Regex.Split(textBox1.Text.Trim(), "\r\n");
            SerialsCounter = lines.Length;

            for (int i = 0; i < textlines.Length; i++)
            {
                currentchr += textlines[i].Length;
                if (currentchr >= xintindex)
                {
                    rida = "Rida: " + (i + 1).ToString();
                    break;
                }
            }
            if (xintindex == -1)
            {
                rida = "Rida: " + SerialsCounter.ToString();
            }
            toolStripStatusLabel1.Text = "Rida: " + (textBox1.GetLineFromCharIndex(textBox1.SelectionStart) + 1).ToString() + "/" + textBox1.Lines.Length.ToString();

        }

        private void wordwrapToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void blueToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Applytheme(Color.LightSkyBlue, Color.White, false);
            menuBar.RenderMode = ToolStripRenderMode.System;
            menuBar.BackColor = Color.LightSkyBlue;
            menuBar.ForeColor = Color.White;
            this.BackColor = Color.LightSkyBlue;
            systemToolStripMenuItem.Checked = false;
            blueToolStripMenuItem.Checked = true;
            darkToolStripMenuItem.Checked = false;
            heleToolStripMenuItem.Checked = false;
            eT();
        }

        private void systemToolStripMenuItem_Click(object sender, EventArgs e)
        {
            systemToolStripMenuItem.Checked = true;
            blueToolStripMenuItem.Checked = false;
            darkToolStripMenuItem.Checked = false;
            heleToolStripMenuItem.Checked = false;
            menuBar.RenderMode = ToolStripRenderMode.System;
            LoadTheme();
            eT();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            scrollBar();
            int lineHeight = TextRenderer.MeasureText("X", this.textBox1.Font).Height;
            double linesPerPage = 1.0 * this.textBox1.ClientSize.Height / lineHeight;
            if (textBox1.Lines.Length - Convert.ToInt32(linesPerPage) > 1)
            {
                vScrollBar1.Enabled = true;
            } else
            {
                vScrollBar1.Enabled = false;
            }
            vScrollBar1.Minimum = Convert.ToInt32(linesPerPage);
            vScrollBar1.Maximum = textBox1.Lines.Length - Convert.ToInt32(linesPerPage);
        }
        private void menuBar_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        void LoadTheme()
        {
            string[] bgfg = File.ReadAllText(Environment.GetEnvironmentVariable("HOMEDRIVE").ToString() + "\\mas\\scheme.cfg").Split(';');
            string[] bgs = bgfg[0].ToString().Split(':');
            string[] fgs = bgfg[1].ToString().Split(':');
            Applytheme(Color.FromArgb(Convert.ToInt32(bgs[0].ToString()), Convert.ToInt32(bgs[1].ToString()), Convert.ToInt32(bgs[2].ToString())), Color.FromArgb(Convert.ToInt32(fgs[0].ToString()), Convert.ToInt32(fgs[1].ToString()), Convert.ToInt32(fgs[2].ToString())));
        }


        void Applytheme(Color bg, Color fg, bool text = true)
        {
            foreach (ToolStripItem ti in fileToolStripMenuItem.DropDownItems)
            {
                ti.BackColor = bg;
                ti.ForeColor = fg;
            }
            foreach (ToolStripItem ti in editToolStripMenuItem.DropDownItems)
            {
                ti.BackColor = bg;
                ti.ForeColor = fg;
            }
            foreach (ToolStripItem ti in batchToolStripMenuItem.DropDownItems)
            {
                ti.BackColor = bg;
                ti.ForeColor = fg;
            }
            foreach (ToolStripItem ti in formatToolStripMenuItem.DropDownItems)
            {
                if (ti is ToolStripMenuItem)
                {
                    ti.BackColor = bg;
                    ti.ForeColor = fg;
                }
            }
            foreach (ToolStripItem ti in viewToolStripMenuItem.DropDownItems)
            {
                ti.BackColor = bg;
                ti.ForeColor = fg;
            }
            foreach (ToolStripItem ti in helpToolStripMenuItem.DropDownItems)
            {
                ti.BackColor = bg;
                ti.ForeColor = fg;
            }
            foreach (ToolStripItem ti in colorToolStripMenuItem.DropDownItems)
            {
                ti.BackColor = bg;
                ti.ForeColor = fg;
            }
            foreach (ToolStripItem ti in themeToolStripMenuItem.DropDownItems)
            {
                ti.BackColor = bg;
                ti.ForeColor = fg;
            }
            statusBar.BackColor = bg;
            statusBar.ForeColor = fg;
            /*
            if (text == true) {
                textBox1.BackColor = bg;
                textBox1.ForeColor = fg;
            } else
            {
                textBox1.BackColor = Color.White;
                textBox1.ForeColor = Color.Black;
            }*/
            menuBar.BackColor = bg;
            menuBar.ForeColor = bg;
            this.BackColor = bg;
            this.ForeColor = fg;
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            /*
             * Check for existing file
             * If Program filename is not null, then display the name
             * on the windowtitle.
             */
            if (!Directory.Exists(Environment.GetEnvironmentVariable("HOMEDRIVE") + "\\mas"))
            {
                MessageBox.Show("Tundub, et olete käivitanud selle programmi arvutis, milles pole paigaldatud Markuse arvuti asjad. See võib mõjutada programmi funktsionaalsust.", "Pole juurutatud", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                teaveMarkuseAsjadeKohtaToolStripMenuItem.Enabled = false;
            }
            textBox1.Text = Program.text;
            if (Program.fnm != null)
            {
                this.Text = "Kiirmärkmik - " + Program.fnm;
            }
            LoadTheme();
            textBox1.SelectAll();
            textBox1.SelectionFont = fontWindow.Font;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Hide();
            if (textBox1.Text == "")
            {
                e.Cancel = false;
            }
            else if (textBox1.Text == textBox2.Text)
            {
                e.Cancel = false;
            }
            else
            {
                DialogResult dr = MessageBox.Show("Kas soovite dokumendi salvestada?", "Salvestamata muudatused tuvastatud", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);
                if (dr == DialogResult.Yes)
                {
                    if (saveToolStripMenuItem.Enabled == true)
                    {
                        saveToolStripMenuItem.PerformClick();
                    }
                    else if (saveToolStripMenuItem.Enabled == false)
                    {
                        saveAsToolStripMenuItem.PerformClick();
                    }
                    e.Cancel = false;
                }
                else if (dr == DialogResult.No)
                {
                    e.Cancel = false;
                } else
                {
                    e.Cancel = true;
                    this.Show();
                }
            }
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
        }

        private void textBox1_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.Text))
                e.Effect = DragDropEffects.Copy;
            else
                e.Effect = DragDropEffects.None;
        }

        private void textBox1_DragEnter(object sender, DragEventArgs e)
        {
        }

        private void darkToolStripMenuItem_Click(object sender, EventArgs e)
        {
            menuBar.RenderMode = ToolStripRenderMode.System;
            menuBar.BackColor = Color.Black;
            menuBar.ForeColor = Color.LightGray;
            Applytheme(Color.Black, Color.LightGray);
            this.BackColor = Color.Black;
            textBox1.BackColor = Color.Black;
            textBox1.ForeColor = Color.LightGray;
            systemToolStripMenuItem.Checked = false;
            blueToolStripMenuItem.Checked = false;
            darkToolStripMenuItem.Checked = true;
            heleToolStripMenuItem.Checked = false;
            eT();
        }

        private void openBatchFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openDialog.FilterIndex = 2;
            openToolStripMenuItem.PerformClick();
        }

        private void readModedoubleClickToExitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Program.readmode.textBox1.Text = textBox1.Text;
            Program.readmode.textBox1.Font = textBox1.Font;
            Program.readmode.textBox1.BackColor = textBox1.BackColor;
            Program.readmode.textBox1.ForeColor = textBox1.ForeColor;
            Program.readmode.ShowDialog();
        }

        private void findNextToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_DoubleClick(object sender, EventArgs e)
        {
            readModedoubleClickToExitToolStripMenuItem.PerformClick();
        }

        private void runWithoutDebuggingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string tfile = openDialog.FileName + ".temp.bat";
            if (File.Exists(tfile) == true) { File.Delete(tfile); }
            File.WriteAllText(tfile, textBox1.Text, Encoding.GetEncoding("windows-1252"));
            Process p = new Process();
            p.StartInfo.FileName = tfile;
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.WorkingDirectory = openDialog.FileName.Substring(0, openDialog.FileName.Length - openDialog.SafeFileName.Length - 1);
            p.Start();
        }

        private void debugToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string tfile = openDialog.FileName + ".temp.bat";
            if (File.Exists(tfile) == true) { File.Delete(tfile); }
            File.WriteAllText(tfile, textBox1.Text, Encoding.GetEncoding("windows-1252"));
            Process p = new Process();
            p.StartInfo.FileName = "cmd.exe";
            p.StartInfo.Arguments = "/k " + "\"" + tfile + "\"";
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.WorkingDirectory = openDialog.FileName.Substring(0, openDialog.FileName.Length - openDialog.SafeFileName.Length - 1);
            p.Start();
        }

        private void openInMicrosoftNotepadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (File.Exists(Environment.GetEnvironmentVariable("TEMP").ToString() + "\\" + openDialog.SafeFileName) == true) { File.Delete(Environment.GetEnvironmentVariable("TEMP").ToString() + "\\" + openDialog.SafeFileName); }

            if (usewinsplit)
            {
                string[] lines = textBox1.Lines;
                List<string> list_of_string = new List<string>();
                foreach (string line in lines)
                {
                    list_of_string.Add(line.Replace("\n", "\r\n"));
                }
                File.WriteAllLines(Environment.GetEnvironmentVariable("TEMP").ToString() + "\\" + openDialog.SafeFileName, list_of_string, en);
            }
            else
            {
                File.WriteAllText(Environment.GetEnvironmentVariable("TEMP").ToString() + "\\" + openDialog.SafeFileName, textBox1.Text, en);
            }
            Process.Start("notepad.exe", Environment.GetEnvironmentVariable("TEMP").ToString() + "\\" + openDialog.SafeFileName);
        }

        public static Encoding GetEncoding(string filename)
        {
            // Read the BOM
            var bom = new byte[4];
            using (var file = new FileStream(filename, FileMode.Open, FileAccess.Read))
            {
                file.Read(bom, 0, 4);
            }

            // Analyze the BOM
            if (bom[0] == 0x2b && bom[1] == 0x2f && bom[2] == 0x76) return Encoding.UTF7;
            if (bom[0] == 0xef && bom[1] == 0xbb && bom[2] == 0xbf) return Encoding.UTF8;
            if (bom[0] == 0xff && bom[1] == 0xfe && bom[2] == 0 && bom[3] == 0) return Encoding.UTF32; //UTF-32LE
            if (bom[0] == 0xff && bom[1] == 0xfe) return Encoding.Unicode; //UTF-16LE
            if (bom[0] == 0xfe && bom[1] == 0xff) return Encoding.BigEndianUnicode; //UTF-16BE
            if (bom[0] == 0 && bom[1] == 0 && bom[2] == 0xfe && bom[3] == 0xff) return new UTF32Encoding(true, true);  //UTF-32BE

            // We actually have no idea what the encoding is if we reach this point, so
            // you may wish to return null instead of defaulting to ASCII
            return Encoding.ASCII;
        }


        private void wordwrapToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            if (wordwrapToolStripMenuItem.Checked == true)
            {
                textBox1.WordWrap = false;
                wordwrapToolStripMenuItem.Checked = false;
                if (statusBarToolStripMenuItem.Checked == false)
                {
                    statusBar.Visible = true;
                }
                else
                {
                    statusBar.Visible = false;
                }
                statusBarToolStripMenuItem.Enabled = true;
            }
            else
            {
                textBox1.WordWrap = true;
                wordwrapToolStripMenuItem.Checked = true;
                statusBar.Visible = false;
                statusBarToolStripMenuItem.Enabled = false;
            }
        }

        private void hideControlsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (hideControlsToolStripMenuItem.Checked == false)
            {
                hideControlsToolStripMenuItem.Checked = true;
                menuBar.Visible = false;
            }
            else
            {
                hideControlsToolStripMenuItem.Checked = false;
                menuBar.Visible = true;
            }
        }

        private void sequenceLoopTestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("See on teist tüüpi krahhimine, kus akent lõputult minimeeritakse ja taastatakse. Salvestamata muudatused lähevat kaotsi. Jätkan sellegipoolest?", "TÄHELEPANU", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (dr == DialogResult.Yes)
            {
                while (true)
                {
                    this.WindowState = FormWindowState.Maximized;
                    this.WindowState = FormWindowState.Normal;
                }
            }
            else if (dr == DialogResult.No)
            {
                MessageBox.Show("Krahhimine 2 protseduurist loobuti", "Krahhimine", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void crashToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("See käsk krahhib programmi täielikult läbides lõputut tsüklit. Salvestamata muudatused lähevad kaotsi. Jätkan sellegipoolest?", "TÄHELEPANU", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (dr == DialogResult.Yes)
            {
                Text = "(Krahhitud) " + Text;
                while (true)
                {
                }
            }
            else if (dr == DialogResult.No)
            {
                MessageBox.Show("Krahhimise protseduurist loobuti", "Krahhimine", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void kustutaAjutisedSilumisfailidToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string tfile = openDialog.FileName + ".temp.bat";
            if (File.Exists(tfile))
            {
                File.Delete(tfile);
            }
        }

        private void toolStripStatusLabel3_Click(object sender, EventArgs e)
        {

        }

        int SeeIndex(string s, string element)
        {
            int iout = 0;
            for (int i = 0; i < s.Length - 1; i++)
            {
                int seewee = element.Length;
                if (s.Substring(i, seewee) == element)
                {
                    iout = i;
                    break;
                }
            }
            return iout;
        }

        private void LeiaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string lasting = findstring;
            Program.findform.ShowDialog();
            if (countcase == false) { findstring = findstring.ToUpper(); }
            if (findstring != lasting)
            {
                leiaJärgmineToolStripMenuItem.Enabled = true;
                leiaJärgmineToolStripMenuItem.PerformClick();
            } else
            {
                if (leiaJärgmineToolStripMenuItem.Enabled == true) { leiaJärgmineToolStripMenuItem.PerformClick(); }
            }
        }

        private void leiaJärgmineToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                int tfindindex = 0;
                string[] rowlst = toolStripStatusLabel1.Text.Replace("Rida: ", "").Split('/');
                lineindex = Convert.ToInt32(rowlst[0].ToString()) - 1;
                findindex = Convert.ToInt32(toolStripStatusLabel2.Text.Replace("Tähemärk: ", ""));
                string[] linelist = textBox1.Text.Split('\n');
                //loo list, kus on esimesed n elementi eemaldatud
                string[] nextlist = new string[linelist.Length - lineindex];
                for (int i = 0; i < lineindex; i++)
                {
                    tfindindex += linelist[i].Length + 1;
                }
                for (int i = lineindex; i < linelist.Length; i++)
                {
                    nextlist[i - lineindex] = linelist[i];
                }
                foreach (string line in nextlist)
                {
                    string s = line + "\n";
                    if (countcase == false) { s = s.ToUpper(); }
                    if (s != "\n")
                    {
                        if (s.Contains(findstring))
                        {
                            tfindindex += SeeIndex(s, findstring);
                            if (s.Substring(SeeIndex(s, findstring), findstring.Length) != findstring)
                            {
                                lineindex += 1;
                                continue;
                            }
                            if (findindex < tfindindex)
                            {
                                lineindex += 1;
                                findindex = tfindindex;
                                break;
                            }
                            else
                            {
                                if (s == nextlist[nextlist.Length - 1])
                                {
                                    MessageBox.Show("Jõudsite faili lõppu");
                                    break;
                                }
                                continue;
                            }
                        }
                        lineindex += 1;
                        tfindindex += s.Length;
                    }
                }
                textBox1.Select(findindex, findstring.Length);
                SearchTimer.Enabled = true;
            }
            catch
            {
                MessageBox.Show("Jõudsite faili lõppu");
                return;
            }
        }

        private void SearchTimer_Tick(object sender, EventArgs e)
        {
            SearchTimer.Enabled = false;
            if (countcase == true) {
                if (textBox1.SelectedText.ToString() != findstring.ToString())
                {
                    leiaJärgmineToolStripMenuItem.PerformClick();
                }
            } else
            {
                if (textBox1.SelectedText.ToString().ToUpper() != findstring.ToString())
                {
                    leiaJärgmineToolStripMenuItem.PerformClick();
                }
            }
        }

        private void asendaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Program.changeform.ShowDialog();
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            textBox1.WordWrap = true;
            prd.DocumentName = openDialog.SafeFileName;
            printPreviewDialog1.Document = prd;
            printPreviewDialog1.ShowDialog();
        }

        private void prindiToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (textBox1.BackColor != Color.White)
            {
                if (MessageBox.Show("Te olete valinud taustaks värvi, mis ei ole valge. Sellega võib kaasneda ootamatult kõrge tindi/tahma kasutus. Kas soovite siiski jätkata?", "Hoiatus", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) != DialogResult.Yes) {
                    return;
                }
            }
            textBox1.WordWrap = true;
            prd.DocumentName = openDialog.SafeFileName;
            printDialog1.Document = prd;
            if (printDialog1.ShowDialog() == DialogResult.OK)
            {
                prd.Print();
            }
        }

        private void leheHäälestamineToolStripMenuItem_Click(object sender, EventArgs e)
        {
            textBox1.WordWrap = true;
            pageSetupDialog2.Document = prd;
            if (pageSetupDialog2.ShowDialog() == DialogResult.OK)
            {
                prd.DefaultPageSettings = pageSetupDialog2.PageSettings;
                prd.PrinterSettings = pageSetupDialog2.PrinterSettings;
            }
        }

        private void kustutaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            textBox1.SelectedText = "";
        }

        private void prefiksiJaSuffiksiLisamineToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Program.ps.ShowDialog();
        }

        private void teaveMarkuseAsjadeKohtaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            File.WriteAllText("C:\\mas\\showabout.txt", "#Notepad");
        }

        private void heleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            menuBar.RenderMode = ToolStripRenderMode.System;
            menuBar.BackColor = Color.White;
            menuBar.ForeColor = Color.DimGray;
            Applytheme(Color.White, Color.DimGray, false);
            this.BackColor = Color.White;
            textBox1.BackColor = Color.White;
            textBox1.ForeColor = Color.DimGray;
            systemToolStripMenuItem.Checked = false;
            blueToolStripMenuItem.Checked = false;
            darkToolStripMenuItem.Checked = false;
            heleToolStripMenuItem.Checked = true;
            eT();
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void sildidToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!l.Visible) {
                l.listView1.Items.Clear();
                int ln = 1;
                foreach (string element in textBox1.Text.Split('\n'))
                {
                    if (!element.StartsWith("::")) {
                        if (element.StartsWith(":"))
                        {
                            ListViewItem li = new ListViewItem();
                            li.Text = element.Replace(":", "");
                            li.SubItems.Add(ln.ToString());
                            l.listView1.Items.Add(li);
                        }
                    }
                    ln++;
                }
                sildidToolStripMenuItem.Checked = true;
                l.Show();
            } else
            {
                l.Hide();
                sildidToolStripMenuItem.Checked = false;
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Down)
            {
                if (IsKeyLocked(Keys.Scroll))
                {
                    if (vScrollBar1.Enabled)
                    {
                        vScrollBar1.Focus();
                    }
                }
                else
                {
                    if (!insertmode)
                    { 
                        if (textBox1.SelectionStart != lastlocation)
                        {
                            line++;
                            lastlocation = textBox1.SelectionStart;
                        }
                    }
                }
            }
            if ((textBox1.Lines.Length > 0) && (textBox1.Lines[textBox1.GetLineFromCharIndex(textBox1.SelectionStart)].Replace(" ", "").StartsWith(":")))
            { 
                updateOne(textBox1.GetLineFromCharIndex(textBox1.SelectionStart));
            }
            else if (e.KeyCode == Keys.Up)
            {
                if (IsKeyLocked(Keys.Scroll))
                {
                    if (vScrollBar1.Enabled)
                    {
                        vScrollBar1.Focus();
                    }
                    else
                    {
                        if (!insertmode)
                        {
                            if (textBox1.SelectionStart != lastlocation)
                            {
                                line--;
                                lastlocation = textBox1.SelectionStart;
                            }
                        }
                    }
                }
                try
                {
                    vScrollBar1.Value = textBox1.GetLineFromCharIndex(textBox1.SelectionStart);
                    scrollBar();
                }
                catch
                {

                }
            }
            else if (e.KeyCode == Keys.Insert)
            {
                if (insertmode == true)
                {
                    insertmode = false;
                } else
                {
                    insertmode = true;
                }
            }
        }

        private void textBox1_MouseDown(object sender, MouseEventArgs e)
        {
        }

        private void textBox1_VScroll(object sender, EventArgs e)
        {
        }

        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            mousePos.X = e.X;
            mousePos.Y = e.Y;
        }

        private void textBox1_MouseMove(object sender, MouseEventArgs e)
        {
            mousePos.X = e.X;
            mousePos.Y = e.Y;
        }

        private void laadiMarkeridUuestiToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int backup = textBox1.SelectionStart;
            int backup1 = textBox1.SelectionLength;
            textBox1.Focus();
            updateFormat(textBox1.GetLineFromCharIndex(textBox1.SelectionStart));
            textBox1.DeselectAll();
            textBox1.SelectionStart = backup;
            textBox1.SelectionLength = backup1;
            textBox1.ScrollToCaret();
        }

        private void kuvaUnicodeSümbolidToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
        }

        private void kuvaUnicodeSümbolidToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            if (kuvaUnicodeSümbolidToolStripMenuItem.Checked == true)
            {
                string s = textBox1.Text;
                s = s.Replace("Ū", "█");
                s = s.Replace("É", "╔");
                s = s.Replace("Ķ", "═");
                s = s.Replace("»", "╗");
                s = s.Replace("Č", "╚");
                s = s.Replace("¼", "╝");
                s = s.Replace("Ź", "╩");
                s = s.Replace("Ė", "╦");
                s = s.Replace("Ī", "╬");
                s = s.Replace("ö", "÷");
                textBox1.Text = s;
            } else
            {
                string s = textBox1.Text;
                s = s.Replace("█", "Ū");
                s = s.Replace("╔", "É");
                s = s.Replace("═", "Ķ");
                s = s.Replace("╗", "»");
                s = s.Replace("╚", "Č");
                s = s.Replace("╝", "¼");
                s = s.Replace("╩", "Ź");
                s = s.Replace("╦", "Ė");
                s = s.Replace("╬", "Ī");
                s = s.Replace("÷", "ö");
                textBox1.Text = s;
            }
        }

        private void vScrollBar1_Scroll(object sender, ScrollEventArgs e)
        {
            try
            {
                textBox1.SelectionStart = textBox1.GetFirstCharIndexFromLine(vScrollBar1.Value);
                scrollBar();
            }
            catch { }
            textBox1.ScrollToCaret();
            
            scrollBar();
        }

        private void textBox1_KeyUp(object sender, KeyEventArgs e)
        {
            try { 
                vScrollBar1.Value = textBox1.GetLineFromCharIndex(textBox1.SelectionStart);
                scrollBar();
            } catch
            {

            }
        }

        private void vScrollBar1_MouseLeave(object sender, EventArgs e)
        {

        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (insertmode)
                { 
                    textBox1.SelectionStart -= 1;
                    textBox1.SelectionLength = 1;
                    textBox1.SelectionFont = fontWindow.Font;
                    textBox1.SelectionStart += 1;
                }
            }
            catch { }
        }

        private void vScrollBar1_KeyDown(object sender, KeyEventArgs e)
        {
            if (!IsKeyLocked(Keys.Scroll))
            {
                textBox1.Focus();
            }
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            timer2.Enabled = false;
            updateFormat();
            scrollBar();
            textBox1.SelectionStart = 0;
            textBox1.Visible = true;
            wf.Close();
        }

        private void fontWindow_Apply(object sender, EventArgs e)
        {

        }

        private void textBox1_FontChanged(object sender, EventArgs e)
        {
            textBox1.Font = fontWindow.Font;
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            textBox3.Font = textBox1.Font;
        }

        private void unixLFToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void windowsCRLFToolStripMenuItem_CheckStateChanged(object sender, EventArgs e)
        {
            if (windowsCRLFToolStripMenuItem.Checked == true)
            {
                unixLFToolStripMenuItem.Checked = false;
                usewinsplit = true;
            }
        }

        private void unixLFToolStripMenuItem_CheckStateChanged(object sender, EventArgs e)
        {
            if (unixLFToolStripMenuItem.Checked == true)
            {
                windowsCRLFToolStripMenuItem.Checked = false;
                usewinsplit = false;
            }
        }

        private void aNSIToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (aNSIToolStripMenuItem.Checked == true)
            {
                unicodeToolStripMenuItem.Checked = false;
                uTF8ToolStripMenuItem.Checked = false;
                standardASCIIToolStripMenuItem.Checked = false;
                uTF7ToolStripMenuItem.Checked = false;
                uTF32ToolStripMenuItem.Checked = false;
                en = Encoding.GetEncoding("windows-1252");
            }
        }

        private void unicodeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (unicodeToolStripMenuItem.Checked == true)
            {
                uTF8ToolStripMenuItem.Checked = false;
                standardASCIIToolStripMenuItem.Checked = false;
                aNSIToolStripMenuItem.Checked = false;
                uTF7ToolStripMenuItem.Checked = false;
                uTF32ToolStripMenuItem.Checked = false;
                en = Encoding.Unicode;
            }
        }

        private void uTF8ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (uTF8ToolStripMenuItem.Checked == true)
            {
                unicodeToolStripMenuItem.Checked = false;
                standardASCIIToolStripMenuItem.Checked = false;
                aNSIToolStripMenuItem.Checked = false;
                uTF7ToolStripMenuItem.Checked = false;
                uTF32ToolStripMenuItem.Checked = false;
                en = Encoding.UTF8;
            }
        }

        private void standardASCIIToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (standardASCIIToolStripMenuItem.Checked == true)
            {
                unicodeToolStripMenuItem.Checked = false;
                uTF8ToolStripMenuItem.Checked = false;
                aNSIToolStripMenuItem.Checked = false;
                uTF7ToolStripMenuItem.Checked = false;
                uTF32ToolStripMenuItem.Checked = false;
                en = Encoding.ASCII;
            }
        }

        private void windowsCRLFToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void uTF7ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (uTF7ToolStripMenuItem.Checked == true)
            {
                unicodeToolStripMenuItem.Checked = false;
                uTF8ToolStripMenuItem.Checked = false;
                aNSIToolStripMenuItem.Checked = false;
                standardASCIIToolStripMenuItem.Checked = false;
                uTF32ToolStripMenuItem.Checked = false;
                en = Encoding.UTF7;
            }
        }

        private void uTF32ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (uTF32ToolStripMenuItem.Checked == true)
            {
                unicodeToolStripMenuItem.Checked = false;
                uTF8ToolStripMenuItem.Checked = false;
                aNSIToolStripMenuItem.Checked = false;
                standardASCIIToolStripMenuItem.Checked = false;
                uTF7ToolStripMenuItem.Checked = false;
                en = Encoding.UTF32;
            }
        }

        private void reapoolitusToolStripMenuItem_DropDownOpening(object sender, EventArgs e)
        {
            foreach (ToolStripMenuItem ti in reapoolitusToolStripMenuItem.DropDownItems)
            {
                ti.BackColor = reapoolitusToolStripMenuItem.BackColor;
                ti.ForeColor = reapoolitusToolStripMenuItem.ForeColor;
            }
        }

        private void kodeeringToolStripMenuItem_DropDownOpening(object sender, EventArgs e)
        {
            foreach (ToolStripMenuItem ti in kodeeringToolStripMenuItem.DropDownItems)
            {
                ti.BackColor = kodeeringToolStripMenuItem.BackColor;
                ti.ForeColor = kodeeringToolStripMenuItem.ForeColor;
            }
        }

        private void korrigeeriTabulatsioonToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int depth = 0;
            int next = -1;
            string[] newtxt = textBox1.Lines;
            string start = "";
            for (int i = 0; i < newtxt.Length; i++)
            {
                if (i == next)
                {
                    if ((newtxt[i].Replace("\t", "").Replace(" ", "").StartsWith(")") || newtxt[i].Replace("\t", "").Replace(" ", "").EndsWith(")")))
                    {
                        try { 
                        newtxt[i] = start.Substring(1, start.Length - 1) + newtxt[i];
                        } catch
                        {

                        }
                    } else
                    {
                        newtxt[i] = start + newtxt[i];
                    }

                    next++;
                }
                if ((newtxt[i].Replace("\t", "").Replace(" ", "").StartsWith(")") || newtxt[i].Replace("\t", "").Replace(" ", "").EndsWith(")")))
                {
                    depth--;
                    if (start.Length > 0)
                    {
                        start = start.Substring(1, start.Length - 1);
                    }
                    if (depth == 0)
                    {
                        next = -1;
                    }
                }
                if ((newtxt[i].Contains("if") || newtxt[i].Contains("for")) && (newtxt[i].Replace("\t", "").Replace(" ", "").StartsWith("(") || newtxt[i].Replace("\t", "").Replace(" ", "").EndsWith("(")))
                {
                    next = i + 1;
                    depth++;
                    start += "\t";
                }
            }
            textBox1.Lines = newtxt;
            updateFormat();
        }

        private void vScrollBar1_KeyUp(object sender, KeyEventArgs e)
        {
            updateFormat();
        }
    }
}

