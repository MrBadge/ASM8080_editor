namespace ASMgenerator8080
{
    partial class PortSettings
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.button1 = new System.Windows.Forms.Button();
            this.comSet = new System.Windows.Forms.Panel();
            this.databits = new System.Windows.Forms.ComboBox();
            this.stopbits = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.parity = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.baud = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.stAddr = new System.Windows.Forms.Panel();
            this.readTo = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.readFrom = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.startAddr = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.comSet.SuspendLayout();
            this.stAddr.SuspendLayout();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(122, 252);
            this.button1.Margin = new System.Windows.Forms.Padding(6);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(188, 40);
            this.button1.TabIndex = 7;
            this.button1.Text = "Apply";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // comSet
            // 
            this.comSet.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.comSet.Controls.Add(this.databits);
            this.comSet.Controls.Add(this.stopbits);
            this.comSet.Controls.Add(this.label4);
            this.comSet.Controls.Add(this.label3);
            this.comSet.Controls.Add(this.parity);
            this.comSet.Controls.Add(this.label2);
            this.comSet.Controls.Add(this.baud);
            this.comSet.Controls.Add(this.label1);
            this.comSet.Location = new System.Drawing.Point(458, 12);
            this.comSet.Name = "comSet";
            this.comSet.Size = new System.Drawing.Size(422, 350);
            this.comSet.TabIndex = 10;
            // 
            // databits
            // 
            this.databits.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.databits.FormattingEnabled = true;
            this.databits.Location = new System.Drawing.Point(202, 197);
            this.databits.Margin = new System.Windows.Forms.Padding(6);
            this.databits.Name = "databits";
            this.databits.Size = new System.Drawing.Size(202, 33);
            this.databits.TabIndex = 5;
            // 
            // stopbits
            // 
            this.stopbits.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.stopbits.FormattingEnabled = true;
            this.stopbits.Location = new System.Drawing.Point(202, 278);
            this.stopbits.Margin = new System.Windows.Forms.Padding(6);
            this.stopbits.Name = "stopbits";
            this.stopbits.Size = new System.Drawing.Size(202, 33);
            this.stopbits.TabIndex = 6;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label4.Location = new System.Drawing.Point(24, 280);
            this.label4.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(151, 37);
            this.label4.TabIndex = 15;
            this.label4.Text = "Stop bits:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label3.Location = new System.Drawing.Point(24, 197);
            this.label3.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(153, 37);
            this.label3.TabIndex = 14;
            this.label3.Text = "Data bits:";
            // 
            // parity
            // 
            this.parity.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.parity.FormattingEnabled = true;
            this.parity.Location = new System.Drawing.Point(202, 116);
            this.parity.Margin = new System.Windows.Forms.Padding(6);
            this.parity.Name = "parity";
            this.parity.Size = new System.Drawing.Size(202, 33);
            this.parity.TabIndex = 4;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label2.Location = new System.Drawing.Point(24, 116);
            this.label2.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(107, 37);
            this.label2.TabIndex = 12;
            this.label2.Text = "Parity:";
            // 
            // baud
            // 
            this.baud.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.baud.FormattingEnabled = true;
            this.baud.Location = new System.Drawing.Point(202, 39);
            this.baud.Margin = new System.Windows.Forms.Padding(6);
            this.baud.MaxDropDownItems = 3;
            this.baud.Name = "baud";
            this.baud.Size = new System.Drawing.Size(202, 33);
            this.baud.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.Location = new System.Drawing.Point(24, 36);
            this.label1.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(165, 37);
            this.label1.TabIndex = 10;
            this.label1.Text = "Baud rate:";
            // 
            // stAddr
            // 
            this.stAddr.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.stAddr.Controls.Add(this.readTo);
            this.stAddr.Controls.Add(this.label7);
            this.stAddr.Controls.Add(this.readFrom);
            this.stAddr.Controls.Add(this.label6);
            this.stAddr.Controls.Add(this.startAddr);
            this.stAddr.Controls.Add(this.label5);
            this.stAddr.Location = new System.Drawing.Point(12, 12);
            this.stAddr.Name = "stAddr";
            this.stAddr.Size = new System.Drawing.Size(417, 150);
            this.stAddr.TabIndex = 11;
            // 
            // readTo
            // 
            this.readTo.Location = new System.Drawing.Point(313, 85);
            this.readTo.Name = "readTo";
            this.readTo.Size = new System.Drawing.Size(78, 31);
            this.readTo.TabIndex = 2;
            this.readTo.Text = "0x2100";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label7.Location = new System.Drawing.Point(267, 79);
            this.label7.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(44, 37);
            this.label7.TabIndex = 8;
            this.label7.Text = "to";
            // 
            // readFrom
            // 
            this.readFrom.Location = new System.Drawing.Point(184, 85);
            this.readFrom.Name = "readFrom";
            this.readFrom.Size = new System.Drawing.Size(78, 31);
            this.readFrom.TabIndex = 1;
            this.readFrom.Text = "0x2100";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label6.Location = new System.Drawing.Point(15, 79);
            this.label6.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(166, 37);
            this.label6.TabIndex = 6;
            this.label6.Text = "Read from";
            // 
            // startAddr
            // 
            this.startAddr.Location = new System.Drawing.Point(225, 29);
            this.startAddr.Name = "startAddr";
            this.startAddr.Size = new System.Drawing.Size(166, 31);
            this.startAddr.TabIndex = 0;
            this.startAddr.Text = "0x2100";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label5.Location = new System.Drawing.Point(15, 23);
            this.label5.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(217, 37);
            this.label5.TabIndex = 4;
            this.label5.Text = "Start address:";
            this.label5.Click += new System.EventHandler(this.label5_Click);
            // 
            // PortSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(892, 387);
            this.Controls.Add(this.stAddr);
            this.Controls.Add(this.comSet);
            this.Controls.Add(this.button1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Margin = new System.Windows.Forms.Padding(6);
            this.Name = "PortSettings";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Program Settings";
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.PortSettings_KeyPress);
            this.comSet.ResumeLayout(false);
            this.comSet.PerformLayout();
            this.stAddr.ResumeLayout(false);
            this.stAddr.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Panel comSet;
        private System.Windows.Forms.ComboBox databits;
        private System.Windows.Forms.ComboBox stopbits;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox parity;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox baud;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel stAddr;
        private System.Windows.Forms.TextBox readTo;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox readFrom;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox startAddr;
        private System.Windows.Forms.Label label5;
    }
}