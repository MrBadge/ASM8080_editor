using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ASMgenerator8080
{
    public partial class HexDump :  Form
    {
        private const int fontSize = 9;
        private string fontName = "Courier New";
        private Size padSize;
   
        public HexDump()
        {
            InitializeComponent();
            padSize = new Size(this.Size.Width - dumpView.Size.Width, this.Size.Height - dumpView.Size.Height);
        }

        public void viewBinaryDump(BinaryGenerator binGen)
        {
            //if (string.IsNullOrEmpty(source)) return;

            int len = 16;
            string[] dump = binGen.getBinaryDumpToString(len);
            string[] ASCIDump = binGen.getACIIDumpToString(len);

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

            int startAddr = binGen.getStartAddress();
            int strLen = 3 * len;
            for (int i = 0; i < dump.Length; ++i)
                dumpView.Rows.Add(new string[] { (startAddr + i * len).ToString("x") + ":", completeString(dump[i], len), completeString(ASCIDump[i], len, true) });

            string zeroString = "";
            string zeroStringASCII = "";
            for (int i = 0; i < len; ++i)
            {
                zeroString += "00 ";
                zeroStringASCII += ".";
            }

            int maxAddr = (0x23ff - startAddr) / 0x10;
            for (int i = dump.Length; i <= maxAddr; ++i)
                dumpView.Rows.Add(new string[] {(startAddr + i*len).ToString("x") + ":",  zeroString, zeroStringASCII});

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
                    num = Convert.ToByte("0x" + nums[i]);
                    res += num < 32 || num > 127 ? '.' : (char)num;
                }
            }

            return res;
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

        private void HexDump_Resize(object sender, EventArgs e)
        {
            Control control = (Control)sender;
            dumpView.Size = new Size(control.Size.Width - padSize.Width, control.Size.Height - padSize.Height);
            Size = new System.Drawing.Size(592, Height);
        }

    }
}
