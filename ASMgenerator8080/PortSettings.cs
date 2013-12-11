using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace ASMgenerator8080
{
    public partial class PortSettings : Form
    {
        private static Regex reg = new Regex(@"0x[A-Fa-f0-9]{4}");

        public PortSettings(ComPortSettings tmp)
        {
            InitializeComponent();
            parity.Items.Add(Parity.Even);
            //parity.Items.Add(Parity.Mark);
            parity.Items.Add(Parity.None);
            parity.Items.Add(Parity.Odd);
            //parity.Items.Add(Parity.Space);
            parity.SelectedItem = tmp.par;
            parity.DropDownStyle = ComboBoxStyle.DropDownList;

            baud.Items.Add(2400);
            baud.Items.Add(4800);
            baud.Items.Add(9600);
            baud.Items.Add(19200);
            baud.SelectedItem = tmp.baud;
            baud.DropDownStyle = ComboBoxStyle.DropDownList;

            stopbits.Items.Add(StopBits.None);
            stopbits.Items.Add(StopBits.One);
            stopbits.Items.Add(StopBits.OnePointFive);
            stopbits.Items.Add(StopBits.Two);
            stopbits.SelectedItem = tmp.sb;
            stopbits.DropDownStyle = ComboBoxStyle.DropDownList;

            //databits.Items.Add(5);
            //databits.Items.Add(6);
            //databits.Items.Add(7);
            databits.Items.Add(8);
            databits.SelectedItem = tmp.databits;
            databits.DropDownStyle = ComboBoxStyle.DropDownList;

            startAddr.Text = "0x" + Convert.ToString(Form1.strtAddr, 16).ToUpper();
            readFrom.Text = "0x" + Convert.ToString(Form1.readFrom, 16).ToUpper();
            readTo.Text = "0x" + Convert.ToString(Form1.readTo, 16).ToUpper();

            startAddr.Focus();
            KeyPreview = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var error = false;
            var stA = startAddr.Text.Trim();
            var rF = readFrom.Text.Trim();
            var rT = readTo.Text.Trim();
            int rFi = 0;
            int rTi = 0;
            if (stA.Length > 6 || !reg.IsMatch(stA))
            {
                startAddr.Focus();
                startAddr.SelectionStart = 0;
                startAddr.SelectionLength = startAddr.Text.Length;
                error = true;
            }
            else
            {
                Form1.strtAddr = Convert.ToUInt16(stA, 16);
            }
            if (!error)
            if (rF.Length > 6 || !reg.IsMatch(rF))
            {
                readFrom.Focus();
                readFrom.SelectionStart = 0;
                readFrom.SelectionLength = readFrom.Text.Length;
                error = true;
            }
            else
            {
                rFi = Convert.ToUInt16(rF, 16);
            }
            if (!error)
            if (rT.Length > 6 || !reg.IsMatch(rT))
            {
                readTo.Focus();
                readTo.SelectionStart = 0;
                readTo.SelectionLength = readTo.Text.Length;
                error = true;
            }
            else
            {
                rTi = Convert.ToUInt16(rT, 16);
            }
            if (rFi > rTi)
            {
                error = true;
                readFrom.Focus();
                readFrom.SelectionStart = 0;
                readFrom.SelectionLength = readFrom.Text.Length;
                MessageBox.Show("Ending adress is less than starting Adress", "Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            else
            {
                Form1.readFrom = rFi;
                Form1.readTo = rTi;
            }
            if (!error)
            {
                Form1.PS.databits = (int) databits.SelectedItem;
                Form1.PS.baud = (int) baud.SelectedItem;
                Form1.PS.par = (Parity) parity.SelectedItem;
                Form1.PS.sb = (StopBits) stopbits.SelectedItem;
                Close();
            }
        }

        private void PortSettings_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13)
            {
                button1_Click(this, e);
            }
        }
    }
}
