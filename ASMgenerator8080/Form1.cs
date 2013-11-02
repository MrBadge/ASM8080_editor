using System;
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

namespace ASMgenerator8080
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Text = "ASM editor for K580";
            mainField.Left = 0;
            mainField.Top = 0;
            mainField.Width = Width;
            mainField.Height = Height;
            if (File.Exists("asmDesc.xml"))
                mainField.DescriptionFile = "asmDesc.xml";
            else
            { 
                MessageBox.Show("Locate the XML file for ASM", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                var OFD = new OpenFileDialog();
                OFD.InitialDirectory = Directory.GetCurrentDirectory();
                OFD.Filter = "xml files (*.xml)|*.xml";
                OFD.FilterIndex = 2;
                OFD.RestoreDirectory = true;
                if (OFD.ShowDialog() == DialogResult.OK)
                    mainField.DescriptionFile = OFD.FileName;
                else
                {
                    var res = MessageBox.Show("Continue without code highlighting?", "Information", MessageBoxButtons.OKCancel,
                        MessageBoxIcon.Question);
                    if (res == DialogResult.Cancel)
                        Close();
                }
            }
        }

        private void Form1_SizeChanged(object sender, EventArgs e)
        {
            mainField.Width = Width;
            mainField.Height = Height;
        }
    }
}
