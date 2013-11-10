using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ASMgenerator8080
{
    public partial class PortSettings : Form
    {
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

            databits.Items.Add(5);
            databits.Items.Add(6);
            databits.Items.Add(7);
            databits.Items.Add(8);
            databits.SelectedItem = tmp.databits;
            databits.DropDownStyle = ComboBoxStyle.DropDownList;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form1.PS.databits = (int) databits.SelectedItem;
            Form1.PS.baud = (int) baud.SelectedItem;
            Form1.PS.par = (Parity) parity.SelectedItem;
            Form1.PS.sb = (StopBits) stopbits.SelectedItem;
            Close();
        }
    }
}
