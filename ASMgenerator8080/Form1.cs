﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics.Eventing.Reader;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using FarsiLibrary.Win;
using FastColoredTextBoxNS;

namespace ASMgenerator8080
{   
    public partial class Form1 : Form
    {
        public List<string> commands = new List<string>();
        public List<string> labels = new List<string>();
        public List<TabPage> pages = new List<TabPage>(); 
        public List<FastColoredTextBox> fields = new List<FastColoredTextBox>(); 
        public AutocompleteMenu popMenu;
        public string DescFile = "";

        public Form1()
        {
            InitializeComponent();
        }

        private string SaveFile(string capt, string template)
        {
            var SVD = new SaveFileDialog
            {
                InitialDirectory = Directory.GetCurrentDirectory(),
                Filter = template,
                FilterIndex = 2,
                RestoreDirectory = true,
                Title = capt
            };
            return SVD.ShowDialog() == DialogResult.OK ? SVD.FileName : null;
        }

        private string OpenFile(string capt, string template)
        {
            var OFD = new OpenFileDialog
                {
                    InitialDirectory = Directory.GetCurrentDirectory(),
                    Filter = template,
                    FilterIndex = 2,
                    RestoreDirectory = true,
                    Title = capt
                };
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

        private void LoadCMDs(string filename)
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
        }

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
                var tb = new FastColoredTextBox();
                tb.Font = new Font("Consolas", 9.75f);
                //tb.ContextMenuStrip = cmMain;
                tb.Dock = DockStyle.Fill;
                tb.BorderStyle = BorderStyle.Fixed3D;
                tb.VirtualSpace = true;
                tb.LeftPadding = 10;
                tb.Language = Language.Custom;
                tb.AddStyle(new MarkerStyle(new SolidBrush(Color.FromArgb(50, Color.Gray))));//same words style
                var tab = new FATabStripItem(fileName != null ? Path.GetFileName(fileName) : "[new]", tb);
                tab.Tag = fileName;
                if (fileName != null)
                    tb.Text = File.ReadAllText(fileName);
                tb.ClearUndo();
                tb.Tag = new TbInfo();
                tb.IsChanged = false;
                tsFiles.AddTab(tab);
                tsFiles.SelectedItem = tab;
                tb.Focus();
                tb.DelayedTextChangedInterval = 1000;
                tb.DelayedEventsInterval = 1000;
                //tb.TextChangedDelayed += new EventHandler<TextChangedEventArgs>(tb_TextChangedDelayed);
                //tb.SelectionChangedDelayed += new EventHandler(tb_SelectionChangedDelayed);
                //tb.KeyDown += new KeyEventHandler(tb_KeyDown);
                //tb.MouseMove += new MouseEventHandler(tb_MouseMove);
                //tb.ChangedLineColor = changedLineColor;
                //if (btHighlightCurrentLine.Checked)
                //    tb.CurrentLineColor = currentLineColor;
                //tb.ShowFoldingLines = btShowFoldingLines.Checked;
                tb.DescriptionFile = DescFile;
                tb.HighlightingRangeType = HighlightingRangeType.VisibleRange;
                popMenu = new AutocompleteMenu(tb) { MinFragmentLength = 1 };
                popMenu.Items.Width = 100;
                popMenu.Items.SetAutocompleteItems(commands);
                //create autocomplete popup menu
                //AutocompleteMenu popupMenu = new AutocompleteMenu(tb);
                //popupMenu.Items.ImageList = ilAutocomplete;
                //popupMenu.Opening += new EventHandler<CancelEventArgs>(popupMenu_Opening);
                //BuildAutocompleteMenu(popupMenu);
                (tb.Tag as TbInfo).popupMenu = popMenu;
            }
            catch (Exception ex)
            {
                if (MessageBox.Show(ex.Message, "Error", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error) == System.Windows.Forms.DialogResult.Retry)
                    CreateTab(fileName);
            }
        }

        public class TbInfo
        {
            public AutocompleteMenu popupMenu;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Text = "ASM editor for K580";
            //mainField.AutoIndentNeeded += fctb_AutoIndentNeeded;
            //mainField.AutoIndent = true;
            //var list = new List<string>();
            DescFile = LocateFile("asmDesc.xml", "Please, locate the XML file for ASM",
                "Continue without code highlighting?", "xml files (*.xml)|*.xml", false, "ASM description file");
            var cmds = LocateFile("cmds.txt", "Please, locate file with asm commands",
                "Continue without autocomplete?", "txt files (*.txt)|*.txt", false, "ASM commands file");
            LoadCMDs(cmds);
            KeyPreview = true;

            Top -= 100;
            Height = 500;//Screen.PrimaryScreen.WorkingArea.Height;
            tsFiles.Left = menu.Left;
            tsFiles.Top = menu.Top + menu.Height + 2;
            tsFiles.Width = Width - 15;
            tsFiles.Height = Height - 15;
        }

        private void Form1_SizeChanged(object sender, EventArgs e)
        {
            tsFiles.Width = Width - 15;
            tsFiles.Height = Height - 15;
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

        private void loadASMCommandsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var filename = OpenFile("ASM commands file", "txt files (*.txt)|*.txt");
            if (filename == null) return;
            LoadCMDs(filename);
            if (popMenu != null)
            {
                popMenu.Items.SetAutocompleteItems(commands);
            }
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
            var filename = OpenFile("Open file", "asm files (*.asm)|*.asm");
            if (filename == null) return;
            CreateTab(filename);
            //CurrentTB.OpenBindingFile(filename, Encoding.Unicode);
            UpdateHighlighting();
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if ((CurrentTB.Parent as FATabStripItem) == null) { return; }
            string filename = (CurrentTB.Parent as FATabStripItem).Title == "[new]"
                ? SaveFile("Save file", "asm files (*.asm)|*.asm")
                : (CurrentTB.Parent as FATabStripItem).Title;
            if (filename == null) return;
            if (CurrentTB != null)
                CurrentTB.SaveToFile(filename, Encoding.Unicode);
            (CurrentTB.Parent as FATabStripItem).Title = Path.GetFileName(filename);
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

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {

        }

        private void undoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (CurrentTB.UndoEnabled)
                CurrentTB.Undo();
        }

        private void redoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (CurrentTB.RedoEnabled)
                CurrentTB.Redo();
        }

        private void cutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CurrentTB.Cut();
        }

        private void pasteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CurrentTB.Paste();
        }

        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CurrentTB.Copy();
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (tsFiles.SelectedItem != null)
            {
                string oldFile = tsFiles.SelectedItem.Tag as string;
                tsFiles.SelectedItem.Tag = null;
                string newFile = SaveFile("Save file", "asm files (*.asm)|*.asm");
                if (newFile != null)
                {
                    tsFiles.SelectedItem.Tag = newFile;
                    tsFiles.SelectedItem.Title = Path.GetFileName(newFile);   
                }
            }
        }

        private void commentSelectedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (CurrentTB == null)
                return;
            CurrentTB.CommentSelected(";");
            //CurrentTB.InsertLinePrefix(";");
            //UpdateHighlighting();
        }

        private void compileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (CurrentTB == null) return;
            string source = CurrentTB.Text;
            
            var hexView = new HexDump();
            hexView.viewBinaryDump(source);
        }

        
    }
}
