using System;
using System.Collections;
using System.Collections.Generic;
//using System.Diagnostics.Eventing.Reader;
using System.Drawing;
using System.IO;
using System.Linq;
//using System.Threading.Tasks;
using System.Windows.Forms;
using FarsiLibrary.Win;
using FastColoredTextBoxNS;
using System.IO.Ports;

namespace ASMgenerator8080
{
    public partial class Form1 : Form
    {
        public List<string> labels = new List<string>();
        public string DescFile = null;
        private readonly SaveFileDialog SFD;
        private readonly OpenFileDialog OFD;
        public static ComPortSettings PS = new ComPortSettings();
        private BinaryGenerator BinGen;
        private readonly MarkerStyle MS = new MarkerStyle(new SolidBrush(Color.FromArgb(100, Color.Gray)));

        public Form1()
        {
            try
            {
                DescFile = Path.GetTempFileName();
                byte[] tmpBytes = ASMgenerator8080.Properties.Resources.asmDesc;
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

            this.SFD = new SaveFileDialog
            {
                InitialDirectory = Directory.GetCurrentDirectory(),
                Filter = "asm files (*.asm)|*.asm",
                FilterIndex = 2,
                RestoreDirectory = true,
            };
            this.OFD = new OpenFileDialog
            {
                InitialDirectory = Directory.GetCurrentDirectory(),
                FilterIndex = 2,
                RestoreDirectory = true,
            };
            BinGen = new BinaryGenerator();
            tsFiles.Dock = DockStyle.Fill;
            panel1.Dock = DockStyle.Fill;

        }

        private bool Save(FATabStripItem tab)
        {
            FastColoredTextBox fastColoredTextBox = tab.Controls[0] as FastColoredTextBox;
            
            if (tab.Tag == null)
            {
                if (this.SFD.ShowDialog() != DialogResult.OK)
                    return false;
                tab.Title = Path.GetFileName(SFD.FileName);
                tab.Tag = (object)SFD.FileName;
            }
            try
            {
                File.WriteAllText(tab.Tag as string, fastColoredTextBox.Text);
                fastColoredTextBox.IsChanged = false;
            }
            catch (Exception ex)
            {
                return MessageBox.Show(ex.Message, "Error", MessageBoxButtons.RetryCancel, MessageBoxIcon.Hand) ==
                       DialogResult.Retry && this.Save(tab);
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

        FastColoredTextBox CurrentTB
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
                tb.DelayedTextChangedInterval = 1000;
                tb.DelayedEventsInterval = 1000;
                //tb.SizeChanged += TbOnSizeChanged;
                tb.HintClick += tb_HintClick;
                //tb.LineInserted += tb_LineInserted;
                //tb.TextChangedDelayed += new EventHandler<TextChangedEventArgs>(tb_TextChangedDelayed);
                tb.SelectionChangedDelayed += new EventHandler(tb_SelectionChangedDelayed);
                //tb.KeyDown += new KeyEventHandler(tb_KeyDown);
                //tb.MouseMove += new MouseEventHandler(tb_MouseMove);
                //tb.ChangedLineColor = changedLineColor;
                //if (btHighlightCurrentLine.Checked)
                //    tb.CurrentLineColor = currentLineColor;
                //tb.ShowFoldingLines = btShowFoldingLines.Checked;
                tb.DescriptionFile = DescFile;
                
                tb.HighlightingRangeType = HighlightingRangeType.VisibleRange;
                var popupMenu = new AutocompleteMenu(tb) { MinFragmentLength = 1 };
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
                if (MessageBox.Show(ex.Message, "Error", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error) == System.Windows.Forms.DialogResult.Retry)
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
            var fragment = tb.Selection.GetFragment(@"\w");
            string text = fragment.Text;
            if (text.Length == 0)
                return;
            Range[] ranges = tb.VisibleRange.GetRanges("\\b" + text + "\\b").ToArray();

            if (ranges.Length > 1)
                foreach (var r in ranges)
                    r.SetStyle(MS);
        }

        void tb_HintClick(object sender, HintClickEventArgs e)
        {
            CurrentTB.Hints.Clear();
        }

        public class TbInfo
        {
            public AutocompleteMenu popupMenu;
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
            Height = 500;//Screen.PrimaryScreen.WorkingArea.Height / 10;
            tsFiles.Left = menu.Left;
            tsFiles.Top = menu.Top + menu.Height + 2;
            tsFiles.Width = Width - 15;
            tsFiles.Height = Height;

        }

        private void loadASMDescFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var filename = OpenFile("ASM description file", "xml files (*.xml)|*.xml");
            if (filename == null) return;
            DescFile = filename;
            CurrentTB.DescriptionFile = DescFile;
            if (CurrentTB == null) return;
            CurrentTB.DescriptionFile = filename;
            CurrentTB.SyntaxHighlighter.HighlightSyntax(CurrentTB.DescriptionFile, CurrentTB.Range);
        }

        private void UpdateHighlighting()
        {
            var lines = new List<string>(CurrentTB.Lines);
            string s = "";
            for (int i = 0; i < lines.Count; i++)
            {
                lines[i] = lines[i] + " ";
                s += lines[i] + "\n";
            }
            CurrentTB.Text = s;   
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var filename = OpenFile("Open file", "All files (*.*)|*.*|Asm files (*.asm)|*.asm");
            if (filename == null) return;
            stStrip.Text = "Opening " + filename + "...";
            CreateTab(filename);
            //CurrentTB.OpenBindingFile(filename, Encoding.Unicode);
            UpdateHighlighting();
            stStrip.Text = "";
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
            if (this.tsFiles.SelectedItem == null)
                return;
            string path = this.tsFiles.SelectedItem.Tag as string;
            this.tsFiles.SelectedItem.Tag = (object)null;
            if (!this.Save(this.tsFiles.SelectedItem) && path != null)
            {
                this.tsFiles.SelectedItem.Tag = (object)path;
                this.tsFiles.SelectedItem.Title = Path.GetFileName(path);
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
            GetBinary(CurrentTB.Text);
        }

        private void serialPortToolStripMenuItem_DropDownOpening(object sender, EventArgs e)
        {
            string[] ports = SerialPort.GetPortNames();
            var tmpComPort = PS.ComPortName;
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
                foreach (var port in ports)
                {
                    var tmp = new ToolStripMenuItem {CheckState = CheckState.Unchecked, Checked = false, Text = port};
                    if (tmp.Text == tmpComPort)
                        tmp.Checked = true;
                    tmp.Click += comToolStripMenuItem_Click;
                    serialPortToolStripMenuItem1.DropDownItems.Add(tmp);
                }
            }
        }

        private void SendSmallLoader(byte[] SmallLoaderHex, int startAddr = 0x2100)
        {
            var port = new SerialPort(PS.ComPortName, 4800, Parity.Even, 7, StopBits.Two);
            port.Open();
            port.Write(SmallLoaderHex, 0, SmallLoaderHex.Length);
            port.Close();
        }

        private void sendToKR580ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (PS.ComPortName == "")
            {
                MessageBox.Show("Choose an appropriate COM-Port first", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                return;
            }
            if (GetBinary(CurrentTB.Text) == null) return;
            SendSmallLoader();
            var port = new SerialPort(PS.ComPortName, PS.baud, PS.par, PS.databits, PS.sb);
            try
            {
                stStrip.Text = "Sending to KR580...";
                stStrip.Refresh();
                port.Open();

                var _data = new ArrayList(BinGen.getBinaryDump());
                byte[] startAddr = BitConverter.GetBytes(BinGen.getStartAddress());
                byte[] data = new byte[_data.Count * 2 + 3];
                data[0] = 1;
                data[1] = startAddr[0];
                data[2] = startAddr[1];
                int j = 0;
                for (int i = 3; i < data.Count(); i++)
                    if (i%2 == 1)
                        data[i] = 2;
                    else
                    {
                        data[i] = (_data[j] != null ? (byte)_data[j] : (byte)0);
                        j += 1;
                    }
                        
                Cursor.Current = Cursors.WaitCursor;
                port.Write(data, 0, data.Length);
                Cursor.Current = Cursors.Default;
                port.Close();
                stStrip.Text = "Done";
                stStrip.Refresh();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                throw;
            }
        }

        private void comToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //if ((sender as ToolStripMenuItem).Name != "No ports found")
            (sender as ToolStripMenuItem).Checked = !(sender as ToolStripMenuItem).Checked;
            PS.ComPortName = (sender as ToolStripMenuItem).Text;
        }

        private void tsFiles_TabStripItemClosing(TabStripItemClosingEventArgs e)
        {
            if (!(e.Item.Controls[0] as FastColoredTextBox).IsChanged)
                return;
            switch (MessageBox.Show("Do you want save " + e.Item.Title + " ?", "Save", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Asterisk))
            {
                case DialogResult.Cancel:
                    e.Cancel = true;
                    break;
                case DialogResult.Yes:
                    if (!this.Save(e.Item))
                    {
                        e.Cancel = true;
                        break;
                    }
                    else
                        break;
            }
        }

        private void editToolStripMenuItem_DropDownOpening(object sender, EventArgs e)
        {
            foreach (var dropDownItem in editToolStripMenuItem1.DropDownItems)
            {
                if (!(dropDownItem is ToolStripSeparator))
                    (dropDownItem as ToolStripMenuItem).Enabled = (CurrentTB != null && tsFiles.Items.Count > 0);
            }  
        }

        private void projectToolStripMenuItem_DropDownOpening(object sender, EventArgs e)
        {
            foreach (var dropDownItem in projectToolStripMenuItem1.DropDownItems)
            {

                if (!(dropDownItem is ToolStripSeparator))
                    (dropDownItem as ToolStripMenuItem).Enabled = (CurrentTB != null && tsFiles.Items.Count > 0);
            } 
        }

        private BinaryGenerator GetBinary(string s)
        {
            
            if (string.IsNullOrEmpty(CurrentTB.Text)) return null;
            
            s = s.Replace("?", "0x00");
            //var nocomments = new Regex(@";(?:\S| )*", RegexOptions.Multiline);
            //s = Regex.Replace(s, @";(?:\S| )*", "", RegexOptions.Multiline);
            //s = Regex.Replace(s, @"\s+$[\r\n]*", "\r\n", RegexOptions.Multiline);
            if (s[s.Length-2].Equals('\r') && s[s.Length-1].Equals('\n'))
                s = s.Remove(s.Length - 2, 2);
            if (BinGen == null)
                BinGen = new BinaryGenerator();
            CurrentTB.Hints.Clear();
            try
            {
                BinGen.generateBinary(s);
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
            if (GetBinary(CurrentTB.Text) == null) return;
            var hexView = new HexDump();
            hexView.viewBinaryDump(BinGen);
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            var list = tsFiles.Items.Cast<FATabStripItem>().ToList();
            foreach (var tab in list)
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
        
    }
}
