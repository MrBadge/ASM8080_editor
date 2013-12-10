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
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var error = false;
            if (startAddr.Text.Length > 6 || !reg.IsMatch(startAddr.Text))
            {
                startAddr.Focus();
                startAddr.SelectionStart = 0;
                startAddr.SelectionLength = startAddr.Text.Length;
                error = true;
            }
            else
            {
                Form1.strtAddr = Convert.ToUInt16(startAddr.Text, 16);
            }
            if (!error)
            if (readFrom.Text.Length > 6 || !reg.IsMatch(readFrom.Text))
            {
                readFrom.Focus();
                readFrom.SelectionStart = 0;
                readFrom.SelectionLength = readFrom.Text.Length;
                error = true;
            }
            else
            {
                Form1.readFrom = Convert.ToUInt16(readFrom.Text, 16);
            }
            if (!error)
            if (readTo.Text.Length > 6 || !reg.IsMatch(readTo.Text))
            {
                readTo.Focus();
                readTo.SelectionStart = 0;
                readTo.SelectionLength = readTo.Text.Length;
                error = true;
            }
            else
            {
                Form1.readTo = Convert.ToUInt16(readTo.Text, 16);
            }
            
            Form1.PS.databits = (int) databits.SelectedItem;
            Form1.PS.baud = (int) baud.SelectedItem;
            Form1.PS.par = (Parity) parity.SelectedItem;
            Form1.PS.sb = (StopBits) stopbits.SelectedItem;
            if (!error)
                Close();
        }
    }
}
