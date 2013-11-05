using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics.Eventing.Reader;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using FarsiLibrary.Win;
using FastColoredTextBoxNS;
using System.IO.Ports;

namespace ASMgenerator8080
{   
    public partial class Form1 : Form
    {
        public List<string> labels = new List<string>();
        //public List<TabPage> pages = new List<TabPage>(); 
        //public List<FastColoredTextBox> fields = new List<FastColoredTextBox>(); 
        //public AutocompleteMenu popMenu;
        public string DescFile = "";
        public string ComPort = "";
        private readonly SaveFileDialog SFD;
        private readonly OpenFileDialog OFD;
        private BinaryGenerator BinGen;
        private readonly MarkerStyle MS = new MarkerStyle(new SolidBrush(Color.FromArgb(100, Color.Gray)));

        public Form1()
        {
            InitializeComponent();
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

            //tsFiles.Height = Height - 100;
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
            return true;
        }

        private string OpenFile(string capt, string template)
        {
            OFD.Filter = template;
            OFD.Title = capt;
            return OFD.ShowDialog() == DialogResult.OK ? OFD.FileName : null;
        }

        private string LocateFile(string filename, string msg1, string msg2, string template, bool mandatory, string capt)
        {
            if (File.Exists(filename))
                return filename;
            else
            {
                MessageBox.Show(msg1, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                filename = OpenFile(capt, template);
                if (filename != null)
                    return filename;
                else
                {
                    if (!mandatory)
                    {
                        var res = MessageBox.Show(msg2, "Information", MessageBoxButtons.OKCancel,
                            MessageBoxIcon.Question);
                        if (res == DialogResult.Cancel)
                            Close();
                        return null;
                    }
                    else
                    {
                        Close();
                        return null;
                    }
                }
            }
        }

        /*private void LoadCMDs(string filename)
        {
            if (filename == null) return;
            using (var r = new StreamReader(filename))
            {
                string line;
                commands.Clear();
                while ((line = r.ReadLine()) != null)
                {
                    commands.Add(line);
                }
            }
        } */

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
                //tsFiles.Height = 400;
                //foreach (var item in tsFiles.Items)
                //{
                //    (item as FATabStripItem).Height = 400;

                //}
                var tb = new FastColoredTextBox
                {
                    Font = new Font("Consolas", 9.75f),
                    Dock = DockStyle.Fill,
                    BorderStyle = BorderStyle.Fixed3D,
                    VirtualSpace = true,
                    LeftPadding = 9,
                    Language = Language.Custom
                };
                //tb.ContextMenuStrip = cmMain;
                
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

        private void Form1_SizeChanged(object sender, EventArgs e)
        {
            
        }

        private void TbOnSizeChanged(object sender, EventArgs eventArgs)
        {
            //throw new NotImplementedException();
            tsFiles.Width = (sender as FastColoredTextBox).Width;
            //MessageBox.Show(Convert.ToString((sender as FastColoredTextBox).LinesCount));
            tsFiles.Height = (sender as FastColoredTextBox).LinesCount*15;
            //Width = CurrentTB.Width;
            //if (tsFiles.Height < Screen.PrimaryScreen.WorkingArea.Height)
            CurrentTB.Height = 400;
            Height = CurrentTB.Height + 100;
        }

        private void tb_LineInserted(object sender, LineInsertedEventArgs e)
        {
            //if ((sender as FastColoredTextBox).LinesCount > 21)
             //   tsFiles.Height = (sender as FastColoredTextBox).LinesCount*10;
        }

        public class TbInfo
        {
            public AutocompleteMenu popupMenu;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Text = "ASM editor for KR580";
            //mainField.AutoIndentNeeded += fctb_AutoIndentNeeded;
            //mainField.AutoIndent = true;
            //var list = new List<string>();
            DescFile = LocateFile("asmDesc.xml", "Please, locate the XML file for ASM",
                "Continue without code highlighting?", "xml files (*.xml)|*.xml", false, "ASM description file");
            //var cmds = LocateFile("cmds.txt", "Please, locate file with asm commands",
            //    "Continue without autocomplete?", "txt files (*.txt)|*.txt", false, "ASM commands file");
            //LoadCMDs(cmds);
            KeyPreview = true;

            Top -= 100;
            Height = 500;//Screen.PrimaryScreen.WorkingArea.Height / 10;
            tsFiles.Left = menu.Left;
            tsFiles.Top = menu.Top + menu.Height + 2;
            tsFiles.Width = Width - 15;
            tsFiles.Height = Height;
        }


        private void mainField_TextChanged(object sender, TextChangedEventArgs e)
        {
            //var s = mainField.Text.Substring(e.ChangedRange.Bounds.iStartChar,
            //    e.ChangedRange.Bounds.iEndChar - e.ChangedRange.Bounds.iStartChar);
            //MessageBox.Show(s);
        }

        private void loadASMDescFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var filename = OpenFile("ASM description file", "xml files (*.xml)|*.xml");
            if (filename != null)
            {
                CurrentTB.DescriptionFile = filename;
            }
        }

        /*private void loadASMCommandsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var filename = OpenFile("ASM commands file", "txt files (*.txt)|*.txt");
            if (filename == null) return;
            LoadCMDs(filename);
            if (popMenu != null)
            {
                popMenu.Items.SetAutocompleteItems(commands);
            }
        }*/

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
            var filename = OpenFile("Open file", "asm files (*.asm)|*.asm");
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

        private void mainField_TextChanging(object sender, TextChangingEventArgs e)
        {
                
        }

        private void mainField_VisibleRangeChanged(object sender, EventArgs e)
        {
            
        }

        private void findToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CurrentTB.ShowFindDialog(CurrentTB.SelectedText);
        }

        private void replaceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CurrentTB.ShowReplaceDialog(CurrentTB.SelectedText);
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CreateTab(null);
        }

        private void undoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (CurrentTB != null && CurrentTB.UndoEnabled)
                CurrentTB.Undo();
        }

        private void redoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (CurrentTB != null && CurrentTB.RedoEnabled)
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
            if (CurrentTB != null)    
                CurrentTB.Paste();
        }

        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (CurrentTB != null)
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
            var tmpComPort = ComPort;
            var portsArr = new ArrayList();
            portsArr.AddRange(ports);
            if (portsArr.Count == 0)
            {
                serialPortToolStripMenuItem.DropDownItems.Clear();
                var tmp = new ToolStripMenuItem {CheckState = CheckState.Unchecked, Text = "No ports found"};
                serialPortToolStripMenuItem.DropDownItems.Add(tmp);
            }
            else
            {
                foreach (var port in ports)
                {
                    var tmp = new ToolStripMenuItem {CheckState = CheckState.Unchecked, Checked = false, Text = port};
                    if (tmp.Text == tmpComPort)
                        tmp.Checked = true;
                    tmp.Click += comToolStripMenuItem_Click;
                    serialPortToolStripMenuItem.DropDownItems.Add(tmp);
                }
            }
        }

        private void sendToKR580ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (ComPort == "")
            {
                MessageBox.Show("Choose an appropriate COM-Port first", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                return;
            }
            if (GetBinary(CurrentTB.Text) == null) return;
            var port = new SerialPort(ComPort, 4800, Parity.None, 8, StopBits.One);
            try
            {
                stStrip.Text = "Sending to KR580...";
                Refresh();
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
                        
                
                //byte[] data1 = { 0, 1, 2, 1, 0 };
                Cursor.Current = Cursors.WaitCursor;
                port.Write(data, 0, data.Length);
                Cursor.Current = Cursors.Default;
                port.Close();
                stStrip.Text = "Done";
                Refresh();
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
            ComPort = (sender as ToolStripMenuItem).Text;
        }

        private void tsFiles_TabStripItemClosing(TabStripItemClosingEventArgs e)
        {
            if (!((e.Item.Controls[0] as FastColoredTextBox).IsChanged || (e.Item.Controls[0] as FastColoredTextBox).Text != ""))
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
            foreach (var dropDownItem in editToolStripMenuItem.DropDownItems)
            {
                if (!(dropDownItem is ToolStripSeparator))
                    (dropDownItem as ToolStripMenuItem).Enabled = (CurrentTB != null);
            }  
        }

        private void projectToolStripMenuItem_DropDownOpening(object sender, EventArgs e)
        {
            foreach (var dropDownItem in projectToolStripMenuItem.DropDownItems)
            {

                if (!(dropDownItem is ToolStripSeparator))
                    (dropDownItem as ToolStripMenuItem).Enabled = (CurrentTB != null);
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
            saveToolStripMenuItem.Enabled = (CurrentTB != null);
            saveAsToolStripMenuItem.Enabled = (CurrentTB != null);
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

        private void Form1_Resize(object sender, EventArgs e)
        {

        }

        
    }
}
