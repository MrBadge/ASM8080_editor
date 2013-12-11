using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
//using System.Linq;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
//using System.Threading.Tasks;
using System.Windows.Forms;

namespace ASMgenerator8080
{
    public partial class HexDump :  Form
    {
        private const int fontSize = 9;
        private const int len = 16;
        private string fontName = "Courier New";
        private Size padSize;
        private BinaryGenerator binGen;
        private readonly SaveFileDialog SFD;
        private byte[] bytes;
   
        public HexDump()
        {
            InitializeComponent();
            KeyPreview = true;
            dumpView.ReadOnly = true;
            SFD = new SaveFileDialog();
            SFD.Title = "Save";
            SFD.Filter = "Bin files (*.bin) | *.bin";
            padSize = new Size(this.Size.Width - dumpView.Size.Width, this.Size.Height - dumpView.Size.Height);
        }

        public void viewBinaryDump(byte[] buf, int stAddr)
        {
            bytes = buf;
            string[] dump = getDumpStrings(buf);
            updateDataGrid(dump, getASCIIDump(dump), stAddr);

        }

        public void viewBinaryDump(BinaryGenerator binGen)
        {
            //if (string.IsNullOrEmpty(source)) return;
            //this.binGen = binGen;
            var tmp = binGen.getBinaryDump();
            if (tmp[tmp.Count-1] == null)
                tmp.RemoveAt(tmp.Count - 1);
            bytes = (byte[])tmp.ToArray(typeof(byte));
            string[] dump = binGen.getBinaryDumpToString(len);
            string[] ASCIIDump = binGen.getACIIDumpToString(len);
            updateDataGrid(dump, ASCIIDump, binGen.getStartAddress());
            
        }

        public void updateDataGrid(string[] dump, string[] ASCIIDump, int stAddr)
        {
            dumpView.ColumnCount = 3;
            dumpView.Columns[0].Name = "Address";
            dumpView.Columns[1].Name = "Memory dump";
            dumpView.Columns[2].Name = "ASCII";

            for (int i = 0; i < dumpView.ColumnCount; ++i)
            {
                dumpView.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
                dumpView.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            }

            dumpView.Columns[0].ReadOnly = true;
            dumpView.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            dumpView.Font = new Font(fontName, fontSize);

            int startAddr = stAddr;
            int strLen = 3 * len;
            for (int i = 0; i < dump.Length; ++i)
                dumpView.Rows.Add(new string[]
                {
                    (startAddr + i*len).ToString("x") + ":", completeString(dump[i], len),
                    completeString(ASCIIDump[i], len, true)
                });

            string zeroString = "";
            string zeroStringASCII = "";
            for (int i = 0; i < len; ++i)
            {
                zeroString += "00 ";
                zeroStringASCII += ".";
            }

            int maxAddr = (0x23ff - startAddr) / 0x10;
            for (int i = dump.Length; i <= maxAddr; ++i)
                dumpView.Rows.Add(new string[] { (startAddr + i * len).ToString("x") + ":", zeroString, zeroStringASCII });

            this.Show();    
        }

        private string stringToASCII(string s)
        {
            if (s == null || s.Length == 0) return "";
            Regex rgx = new Regex(" ");
            string[] nums = rgx.Split(s);
            
            string res = "";
            byte num = 0;
            for (int i = 0; i < nums.Length; ++i)
            {
                if (nums[i].Length != 0)
                {
                    num = Convert.ToByte("0x" + nums[i], 16);
                    res += num < 32 || num > 127 ? '.' : (char)num;
                }
            }

            return res;
        }

        private string[] getASCIIDump(string[] dump)
        {
            string[] tmp = new string[dump.Length];

            int index = 0;
            foreach (var s in dump)
            {
                tmp[index++] = stringToASCII(s);
            }

            return tmp;
        }

        private string completeString(string s, int len, bool asciiMode = false)
        {
            if (s == null || s.Length >= (asciiMode ? len : 3*len)) return s;

            for (int i = asciiMode ? s.Length : s.Length / 3; i < len; ++i)
                if (!asciiMode) s += "00 ";
                else s += '.';

            return s;
        }

        /*private string ASCIIToString(string s)
        {
            
        }*/

        private string[] getDumpStrings(byte[] buf)
        {
            string[] dump = new string[buf.Length%len == 0 ? buf.Length/len : buf.Length/len + 1];
            int index = 0;
        
            for (int i = 0; i < buf.Length; ++i)
            {
                if (i != 0 && i%len == 0) ++index;
                var tmp = buf[i].ToString("X") + ' ';
                dump[index] += tmp.Length < 3 ? '0' + tmp: tmp;
            }

            if (dump[index].Length < len)
                completeString(dump[index], len);

            return dump;
        }

        private void HexDump_Resize(object sender, EventArgs e)
        {
            Control control = (Control)sender;
            dumpView.Size = new Size(control.Size.Width - padSize.Width, control.Size.Height - padSize.Height);
            Size = new System.Drawing.Size(592, Height);
        }

        private void dumpView_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == (Keys.Control | Keys.S))
            {
                Save();
            }
        }

        private bool Save()
        {
            if (SFD.ShowDialog() != DialogResult.OK)
                return false;

            //ArrayList mem = binGen.getBinaryDump();

            try
            {
                File.WriteAllBytes(SFD.FileName, bytes);
            }
            catch (Exception ex)
            {
                return MessageBox.Show(ex.Message, "Error", MessageBoxButtons.RetryCancel, MessageBoxIcon.Hand) ==
                       DialogResult.Retry && Save();
            }
            return true;
        }

    }
}
