//using System.Diagnostics.Eventing.Reader;
//using System.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Windows.Forms;
using ASMgenerator8080.Properties;
using FarsiLibrary.Win;
using FastColoredTextBoxNS;

namespace ASMgenerator8080
{
    public partial class Form1 : Form
    {
        public static ComPortSettings PS = new ComPortSettings();
        private readonly MarkerStyle MS = new MarkerStyle(new SolidBrush(Color.FromArgb(100, Color.Gray)));
        private readonly OpenFileDialog OFD;
        private readonly SaveFileDialog SFD;
        private BinaryGenerator BinGen;
        public string DescFile = null;
        public List<string> labels = new List<string>();

        public Form1()
        {
            try
            {
                DescFile = Path.GetTempFileName();
                byte[] tmpBytes = Resources.asmDesc;
                var streamWithASMDesc = new FileStream(DescFile, FileMode.OpenOrCreate);
                streamWithASMDesc.Write(tmpBytes, 0, tmpBytes.Length);
                streamWithASMDesc.Close();
            }
            catch (Exception)
            {
                DescFile = null;
            }

            InitializeComponent();

            PS = new ComPortSettings();
            PS.SetPortSet();

            SFD = new SaveFileDialog
            {
                InitialDirectory = Directory.GetCurrentDirectory(),
                Filter = "asm files (*.asm)|*.asm",
                FilterIndex = 2,
                RestoreDirectory = true,
            };
            OFD = new OpenFileDialog
            {
                InitialDirectory = Directory.GetCurrentDirectory(),
                FilterIndex = 2,
                RestoreDirectory = true,
            };
            BinGen = new BinaryGenerator();
            tsFiles.Dock = DockStyle.Fill;
            panel1.Dock = DockStyle.Fill;
            stStrip.Items.Add("Start work with opening or creating new file");
            stStrip.Items.Add("");
            //stStrip.Items.Add("          Current COM-Port: ");
            //stStrip.Items[2].TextAlign = ContentAlignment.MiddleRight;
        }

        private FastColoredTextBox CurrentTB
        {
            get
            {
                if (tsFiles.SelectedItem == null)
                    return null;
                return (tsFiles.SelectedItem.Controls[0] as FastColoredTextBox);
            }

            set
            {
                tsFiles.SelectedItem = (value.Parent as FATabStripItem);
                value.Focus();
            }
        }

        private bool Save(FATabStripItem tab)
        {
            var fastColoredTextBox = tab.Controls[0] as FastColoredTextBox;

            if (tab.Tag == null)
            {
                if (SFD.ShowDialog() != DialogResult.OK)
                    return false;
                tab.Title = Path.GetFileName(SFD.FileName);
                tab.Tag = SFD.FileName;
            }
            try
            {
                File.WriteAllText(tab.Tag as string, fastColoredTextBox.Text);
                fastColoredTextBox.IsChanged = false;
            }
            catch (Exception ex)
            {
                return MessageBox.Show(ex.Message, "Error", MessageBoxButtons.RetryCancel, MessageBoxIcon.Hand) ==
                       DialogResult.Retry && Save(tab);
            }
            fastColoredTextBox.Invalidate();
            tsFiles.TabStripItemClosing += tsFiles_TabStripItemClosing;
            return true;
        }

        private string OpenFile(string capt, string template)
        {
            OFD.Filter = template;
            OFD.Title = capt;
            return OFD.ShowDialog() == DialogResult.OK ? OFD.FileName : null;
        }

        //private string LocateFile(string filename, string msg1, string msg2, string template, bool mandatory, string capt)
        //{
        //    if (File.Exists(filename))
        //        return filename;
        //    else
        //    {
        //MessageBox.Show(msg1, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //filename = OpenFile(capt, template);
        //if (filename != null)
        //    return filename;
        //else
        //{
        //    if (!mandatory)
        //    {
        //        var res = MessageBox.Show(msg2, "Information", MessageBoxButtons.OKCancel,
        //            MessageBoxIcon.Question);
        //        if (res == DialogResult.Cancel)
        //            Close();
        //        return null;
        //    }
        //    else
        //    {
        //        Close();
        //        return null;
        //    }
        //}
        //}
        //}

        private void CreateTab(string fileName)
        {
            try
            {
                var tb = new FastColoredTextBox
                {
                    Font = new Font("Consolas", 9.75f),
                    Dock = DockStyle.Fill,
                    BorderStyle = BorderStyle.Fixed3D,
                    VirtualSpace = true,
                    LeftPadding = 9,
                    Language = Language.Custom
                };

                var tab = new FATabStripItem(fileName != null ? Path.GetFileName(fileName) : "[new]", tb)
                {
                    Tag = fileName
                };
                if (fileName != null)
                    tb.Text = File.ReadAllText(fileName);
                //else
                //    tb.Text = "\n";
                //else
                //    tb.Text = "\n"; //bug? 
                tb.ClearUndo();
                tb.Tag = new TbInfo();
                tb.IsChanged = false;
                tsFiles.AddTab(tab);
                tsFiles.SelectedItem = tab;
                tb.Focus();
                tb.DelayedTextChangedInterval = 500;
                tb.DelayedEventsInterval = 500;
                //tb.SizeChanged += TbOnSizeChanged;
                tb.HintClick += tb_HintClick;
                //tb.LineInserted += tb_LineInserted;
                //tb.TextChangedDelayed += new EventHandler<TextChangedEventArgs>(tb_TextChangedDelayed);
                tb.SelectionChangedDelayed += tb_SelectionChangedDelayed;
                //tb.KeyDown += new KeyEventHandler(tb_KeyDown);
                //tb.MouseMove += new MouseEventHandler(tb_MouseMove);
                //tb.ChangedLineColor = changedLineColor;
                //if (btHighlightCurrentLine.Checked)
                //    tb.CurrentLineColor = currentLineColor;
                //tb.ShowFoldingLines = btShowFoldingLines.Checked;
                tb.DescriptionFile = DescFile;

                tb.HighlightingRangeType = HighlightingRangeType.VisibleRange;
                var popupMenu = new AutocompleteMenu(tb) {MinFragmentLength = 1};
                popupMenu.Items.Width = 100;
                popupMenu.Items.SetAutocompleteItems(Constants.Commands);
                //create autocomplete popup menu
                //AutocompleteMenu popupMenu = new AutocompleteMenu(tb);
                //popupMenu.Items.ImageList = ilAutocomplete;
                //popupMenu.Opening += new EventHandler<CancelEventArgs>(popupMenu_Opening);
                //BuildAutocompleteMenu(popupMenu);
                (tb.Tag as TbInfo).popupMenu = popupMenu;
            }
            catch (Exception ex)
            {
                if (MessageBox.Show(ex.Message, "Error", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error) ==
                    DialogResult.Retry)
                    CreateTab(fileName);
            }
        }

        private void tb_SelectionChangedDelayed(object sender, EventArgs e)
        {
            var tb = sender as FastColoredTextBox;
            //highlight same words
            tb.VisibleRange.ClearStyle(MS);
            if (tb.Selection.IsEmpty)
                return;
            Range fragment = tb.Selection.GetFragment(@"\w");
            string text = fragment.Text;
            if (text.Length == 0)
                return;
            Range[] ranges = tb.VisibleRange.GetRanges("\\b" + text + "\\b").ToArray();

            if (ranges.Length > 1)
                foreach (Range r in ranges)
                    r.SetStyle(MS);
        }

        private void tb_HintClick(object sender, HintClickEventArgs e)
        {
            CurrentTB.Hints.Clear();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Text = "ASM editor and loader for KR580";
            //mainField.AutoIndentNeeded += fctb_AutoIndentNeeded;
            //mainField.AutoIndent = true;
            //var list = new List<string>();
            //DescFile = LocateFile("asmDesc.xml", "Please, locate the XML file for ASM",
            //    "Continue without code highlighting?", "xml files (*.xml)|*.xml", false, "ASM description file");
            //DescFile = pathToASMDesc;//"ASMHighLightingDescFile";
            //CurrentTB.SyntaxHighlighter.HighlightSyntax(Language.Custom, new Range(CurrentTB, ));
            //var cmds = LocateFile("cmds.txt", "Please, locate file with asm commands",
            //    "Continue without autocomplete?", "txt files (*.txt)|*.txt", false, "ASM commands file");
            KeyPreview = true;

            Top -= 100;
            Height = 500; //Screen.PrimaryScreen.WorkingArea.Height / 10;
            tsFiles.Left = menu.Left;
            tsFiles.Top = menu.Top + menu.Height + 2;
            tsFiles.Width = Width - 15;
            tsFiles.Height = Height;
        }

        private void loadASMDescFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string filename = OpenFile("ASM description file", "xml files (*.xml)|*.xml");
            if (filename == null) return;
            DescFile = filename;
            CurrentTB.DescriptionFile = DescFile;
            if (CurrentTB == null) return;
            CurrentTB.DescriptionFile = filename;
            UpdateHighlighting(CurrentTB, CurrentTB.Range);
        }

        private void UpdateHighlighting(FastColoredTextBox e, Range rng)
        {
            e.SyntaxHighlighter.HighlightSyntax(e.DescriptionFile, rng);
            /*var lines = new List<string>(CurrentTB.Lines);
            string s = "";
            for (int i = 0; i < lines.Count; i++)
            {
                lines[i] = lines[i] + " ";
                s += lines[i] + "\n";
            }
            CurrentTB.Text = s;   */
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //stStrip.Items[0].Text = "";
            string filename = OpenFile("Open file", "All files (*.*)|*.*|Asm files (*.asm)|*.asm");
            if (filename == null) return;
            stStrip.Items[0].Text = "Opening " + filename + " ...";
            CreateTab(filename);
            //CurrentTB.OpenBindingFile(filename, Encoding.Unicode);
            UpdateHighlighting(CurrentTB, CurrentTB.Range);
            stStrip.Items[0].Text = "";
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (tsFiles.SelectedItem == null)
                return;
            Save(tsFiles.SelectedItem);
            //string filename = (CurrentTB.Parent as FATabStripItem).Title == "[new]"
            //    ? SaveFile("Save file", "asm files (*.asm)|*.asm")
            //    : (CurrentTB.Parent as FATabStripItem).Title;
            //if (filename == null) return;
            //if (CurrentTB != null)
            //    CurrentTB.SaveToFile(filename, Encoding.Unicode);
            //(CurrentTB.Parent as FATabStripItem).Title = Path.GetFileName(filename);
        }

        private void findToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (CurrentTB != null && tsFiles.Items.Count > 0)
                CurrentTB.ShowFindDialog(CurrentTB.SelectedText);
        }

        private void replaceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (CurrentTB != null && tsFiles.Items.Count > 0)
                CurrentTB.ShowReplaceDialog(CurrentTB.SelectedText);
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            stStrip.Items[0].Text = "";
            CreateTab(null);
        }

        private void undoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (CurrentTB != null && CurrentTB.UndoEnabled && tsFiles.Items.Count > 0)
                CurrentTB.Undo();
        }

        private void redoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (CurrentTB != null && CurrentTB.RedoEnabled && tsFiles.Items.Count > 0)
                CurrentTB.Redo();
        }

        private void cutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (CurrentTB == null || (CurrentTB.LinesCount == 1 && CurrentTB.Text == "")) return;
            CurrentTB.Cut();
            //if (CurrentTB.LinesCount == 0)
            //    CurrentTB.Text = "";
        }

        private void pasteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (CurrentTB != null && tsFiles.Items.Count > 0)
                CurrentTB.Paste();
        }

        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (CurrentTB != null && tsFiles.Items.Count > 0)
                CurrentTB.Copy();
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //if (tsFiles.SelectedItem == null) return;
            //string oldFile = tsFiles.SelectedItem.Tag as string;
            //tsFiles.SelectedItem.Tag = null;
            //string newFile = SaveFile("Save file", "asm files (*.asm)|*.asm");
            //if (newFile == null) return;
            //tsFiles.SelectedItem.Tag = newFile;
            //tsFiles.SelectedItem.Title = Path.GetFileName(newFile);
            if (tsFiles.SelectedItem == null)
                return;
            var path = tsFiles.SelectedItem.Tag as string;
            tsFiles.SelectedItem.Tag = null;
            if (!Save(tsFiles.SelectedItem) && path != null)
            {
                tsFiles.SelectedItem.Tag = path;
                tsFiles.SelectedItem.Title = Path.GetFileName(path);
            }
        }

        private void commentSelectedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CurrentTB.CommentSelected(";");
            //CurrentTB.InsertLinePrefix(";");
            //UpdateHighlighting();
        }

        private void compileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (CurrentTB == null)
            {
                //MessageBox.Show("Nothing to compile yet", "Error", MessageBoxButtons.OK,
                    //MessageBoxIcon.Exclamation);
                return;
            }
            stStrip.Items[0].Text = "Compiling ...";
            Cursor.Current = Cursors.WaitCursor;
            stStrip.Refresh();
            GetBinary(CurrentTB.Text, 0x2100 + 0x23 + Constants.BigProgramLoader.Length);
            stStrip.Items[0].Text = "Done";
            Cursor.Current = Cursors.Default;
        }

        private void serialPortToolStripMenuItem_DropDownOpening(object sender, EventArgs e)
        {
            string[] ports = SerialPort.GetPortNames();
            string tmpComPort = PS.ComPortName;
            var portsArr = new ArrayList();
            portsArr.AddRange(ports);
            if (portsArr.Count == 0)
            {
                serialPortToolStripMenuItem1.DropDownItems.Clear();
                var tmp = new ToolStripMenuItem {CheckState = CheckState.Unchecked, Text = "No ports found"};
                serialPortToolStripMenuItem1.DropDownItems.Add(tmp);
            }
            else
            {
                serialPortToolStripMenuItem1.DropDownItems.Clear();
                foreach (string port in ports)
                {
                    var tmp = new ToolStripMenuItem {CheckState = CheckState.Unchecked, Checked = false, Text = port};
                    if (tmp.Text == tmpComPort)
                        tmp.Checked = true;
                    tmp.Click += comToolStripMenuItem_Click;
                    serialPortToolStripMenuItem1.DropDownItems.Add(tmp);
                }
            }
        }

        private byte[] GetNewSettings(ComPortSettings PS)
        {
            string USARTSet = "";
            byte timerSet = 0;
            switch (PS.sb)
            {
                case StopBits.None:
                    USARTSet += "00";
                    break;
                case StopBits.One:
                    USARTSet += "01";
                    break;
                case StopBits.Two:
                    USARTSet += "11";
                    break;
                case StopBits.OnePointFive:
                    USARTSet += "10";
                    break;
            }
            switch (PS.par)
            {
                case Parity.None:
                    USARTSet += "00";
                    break;
                case Parity.Odd:
                    USARTSet += "01";
                    break;
                case Parity.Even:
                    USARTSet += "11";
                    break;
            }
            switch (PS.databits)
            {
                case 5:
                    USARTSet += "00";
                    break;
                case 6:
                    USARTSet += "01";
                    break;
                case 7:
                    USARTSet += "10";
                    break;
                case 8:
                    USARTSet += "11";
                    break;
            }
            if (PS.baud == 19200)
                USARTSet += "01";
            else
                USARTSet += "10";

            switch (PS.baud)
            {
                case 2400:
                    timerSet = 0x34;
                    break;
                case 4800:
                    timerSet = 0x1A;
                    break;
                case 9600:
                    timerSet = 0x0D;
                    break;
                case 19200:
                    timerSet = 0x68;
                    break;
            }
            //0x34 - 2400
            //0x1A - 4800
            //0x0D - 9600
            //0x68 - 19200 + 7E -> 7D
            var temp = new byte[2];
            temp[0] = timerSet; 
            temp[1] = Convert.ToByte(USARTSet, 2);
            return temp;
        }

        private void SendBigLoader(byte[] BigLoaderHex, int startAddr = 0x2100)
        {
            try
            {
                var port = new SerialPort(PS.ComPortName, 4800, Parity.Even, 7, StopBits.Two);
                port.Open();
                port.Write(BigLoaderHex, 0, BigLoaderHex.Length);
                port.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
        }

        private void sendToKR580ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (CurrentTB == null)
            {
                //MessageBox.Show("Nothing to send yet", "Error", MessageBoxButtons.OK,
                    //MessageBoxIcon.Exclamation);
                return;    
            }
            if (string.IsNullOrEmpty(PS.ComPortName))
            {
                MessageBox.Show("Choose an appropriate COM-Port first", "Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Exclamation);
                return;
            }
            if (GetBinary(CurrentTB.Text, 0x2100 + 0x23 + Constants.BigProgramLoader.Length) == null)
                return;
            // start address = 0x2100 + small loader size + big loader size
            byte[] tmp = GetNewSettings(PS);
            Constants.BigProgramLoader[5] = tmp[0];
            Constants.BigProgramLoader[9] = tmp[1];
            SendBigLoader(Constants.BigProgramLoader);
            var port = new SerialPort(PS.ComPortName, PS.baud, PS.par, PS.databits, PS.sb);
            try
            {
                stStrip.Items[0].Text = "Sending to KR580...";
                stStrip.Refresh();
                port.Open();

                var _data = new ArrayList(BinGen.getBinaryDump());
                byte[] startAddr = BitConverter.GetBytes(BinGen.getStartAddress());
                stStrip.Items[1].Text = "| Start address of your program in memory = " + startAddr[0] + startAddr[1];
                var data = new byte[_data.Count*2 + 3];
                data[0] = 1;
                data[1] = startAddr[0];
                data[2] = startAddr[1];
                int j = 0;
                for (int i = 3; i < data.Count(); i++)
                    if (i%2 == 1)
                        data[i] = 2;
                    else
                    {
                        data[i] = (_data[j] != null ? (byte) _data[j] : (byte) 0);
                        j += 1;
                    }

                Cursor.Current = Cursors.WaitCursor;
                port.Write(data, 0, data.Length);
                Cursor.Current = Cursors.Default;
                port.Close();
                stStrip.Items[0].Text = "Sending seccesfully comleted";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
        }

        private void comToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //if ((sender as ToolStripMenuItem).Name != "No ports found")
            (sender as ToolStripMenuItem).Checked = !(sender as ToolStripMenuItem).Checked;
            PS.ComPortName = (sender as ToolStripMenuItem).Text;
            //stStrip.Items[2].Text = "Current COM-Port: "+ PS.ComPortName;
        }

        private void tsFiles_TabStripItemClosing(TabStripItemClosingEventArgs e)
        {
            if (!(e.Item.Controls[0] as FastColoredTextBox).IsChanged)
                return;
            switch (
                MessageBox.Show("Do you want save " + e.Item.Title + " ?", "Save", MessageBoxButtons.YesNoCancel,
                    MessageBoxIcon.Asterisk))
            {
                case DialogResult.Cancel:
                    e.Cancel = true;
                    break;
                case DialogResult.Yes:
                    if (!Save(e.Item))
                    {
                        e.Cancel = true;
                        break;
                    }
                    break;
            }
        }

        private void editToolStripMenuItem_DropDownOpening(object sender, EventArgs e)
        {
            foreach (object dropDownItem in editToolStripMenuItem1.DropDownItems)
            {
                if (!(dropDownItem is ToolStripSeparator))
                    (dropDownItem as ToolStripMenuItem).Enabled = (CurrentTB != null && tsFiles.Items.Count > 0);
            }
        }

        private void projectToolStripMenuItem_DropDownOpening(object sender, EventArgs e)
        {
            foreach (object dropDownItem in projectToolStripMenuItem1.DropDownItems)
            {
                if (!(dropDownItem is ToolStripSeparator))
                    (dropDownItem as ToolStripMenuItem).Enabled = (CurrentTB != null && tsFiles.Items.Count > 0);
            }
        }

        private BinaryGenerator GetBinary(string s, int startAddr)
        {
            if (string.IsNullOrEmpty(CurrentTB.Text)) return null;

            s = s.Replace("?", "0x00");
            //var nocomments = new Regex(@";(?:\S| )*", RegexOptions.Multiline);
            //s = Regex.Replace(s, @";(?:\S| )*", "", RegexOptions.Multiline);
            //s = Regex.Replace(s, @"\s+$[\r\n]*", "\r\n", RegexOptions.Multiline);
            if (s[s.Length - 2].Equals('\r') && s[s.Length - 1].Equals('\n'))
                s = s.Remove(s.Length - 2, 2);
            if (BinGen == null)
                BinGen = new BinaryGenerator();
            CurrentTB.Hints.Clear();
            try
            {
                BinGen.generateBinary(s, startAddr);
            }
            catch (BinaryGeneratorException e)
            {
                BinGen = null;
                var r = new Range(CurrentTB, 0, e.line, CurrentTB.Lines[e.line].Length, e.line);
                var hint = new Hint(r, e.Message, true, true);
                hint.BackColor = Color.Red;
                CurrentTB.Hints.Add(hint);
                CurrentTB.Navigate(e.line);
            }

            return BinGen;
        }

        private void fileToolStripMenuItem_DropDownOpening(object sender, EventArgs e)
        {
            saveToolStripMenuItem1.Enabled = (CurrentTB != null && tsFiles.Items.Count > 0);
            saveAsToolStripMenuItem1.Enabled = (CurrentTB != null && tsFiles.Items.Count > 0);
        }

        private void viewHexToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (CurrentTB == null) 
                return;
            if (GetBinary(CurrentTB.Text, 0x2100 + 0x23 + Constants.BigProgramLoader.Length) == null) return;
            var hexView = new HexDump();
            hexView.viewBinaryDump(BinGen);
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            List<FATabStripItem> list = tsFiles.Items.Cast<FATabStripItem>().ToList();
            foreach (FATabStripItem tab in list)
            {
                var args = new TabStripItemClosingEventArgs(tab);
                tsFiles_TabStripItemClosing(args);
                if (args.Cancel)
                {
                    e.Cancel = true;
                    return;
                }
                tsFiles.RemoveTab(tab);
            }
        }

        private void serialPortSettingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var portsettings = new PortSettings(PS);
            portsettings.ShowDialog(this);
        }

        public class TbInfo
        {
            public AutocompleteMenu popupMenu;
        }
    }
}