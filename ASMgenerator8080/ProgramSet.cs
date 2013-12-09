using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace ASMgenerator8080
{
    public partial class ProgramSet : Form
    {
        private static Regex reg = new Regex(@"0x[A-Fa-f0-9]{4}");

        public ProgramSet()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void ProgramSet_Load(object sender, EventArgs e)
        {
           
        }

        private void apply_Click(object sender, EventArgs e)
        {
            if (textBox1.Text.Length > 6 || !reg.IsMatch(textBox1.Text))
            {
                textBox1.Text = "";
            }
            else
            {
                Form1.strtAddr = Convert.ToInt16(textBox1.Text, 16);
                this.Close();
            }
        }
    }
}
