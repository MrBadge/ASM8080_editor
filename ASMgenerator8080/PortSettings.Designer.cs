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
            this.label1 = new System.Windows.Forms.Label();
            this.baud = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.parity = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.dataNumb = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.stopbits = new System.Windows.Forms.ComboBox();
            this.button1 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataNumb)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.Location = new System.Drawing.Point(12, 18);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(83, 20);
            this.label1.TabIndex = 0;
            this.label1.Text = "Baud rate:";
            // 
            // baud
            // 
            this.baud.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.baud.FormattingEnabled = true;
            this.baud.Location = new System.Drawing.Point(101, 20);
            this.baud.Name = "baud";
            this.baud.Size = new System.Drawing.Size(103, 21);
            this.baud.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label2.Location = new System.Drawing.Point(12, 60);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(52, 20);
            this.label2.TabIndex = 2;
            this.label2.Text = "Parity:";
            // 
            // parity
            // 
            this.parity.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.parity.FormattingEnabled = true;
            this.parity.Location = new System.Drawing.Point(101, 60);
            this.parity.Name = "parity";
            this.parity.Size = new System.Drawing.Size(103, 21);
            this.parity.TabIndex = 3;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label3.Location = new System.Drawing.Point(12, 102);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(77, 20);
            this.label3.TabIndex = 4;
            this.label3.Text = "Data bits:";
            // 
            // dataNumb
            // 
            this.dataNumb.Location = new System.Drawing.Point(155, 102);
            this.dataNumb.Maximum = new decimal(new int[] {
            256,
            0,
            0,
            0});
            this.dataNumb.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.dataNumb.Name = "dataNumb";
            this.dataNumb.Size = new System.Drawing.Size(48, 20);
            this.dataNumb.TabIndex = 5;
            this.dataNumb.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label4.Location = new System.Drawing.Point(12, 145);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(76, 20);
            this.label4.TabIndex = 6;
            this.label4.Text = "Stop bits:";
            // 
            // stopbits
            // 
            this.stopbits.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.stopbits.FormattingEnabled = true;
            this.stopbits.Location = new System.Drawing.Point(101, 144);
            this.stopbits.Name = "stopbits";
            this.stopbits.Size = new System.Drawing.Size(103, 21);
            this.stopbits.TabIndex = 7;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(61, 182);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(94, 21);
            this.button1.TabIndex = 8;
            this.button1.Text = "Apply";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // PortSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(220, 215);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.stopbits);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.dataNumb);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.parity);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.baud);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "PortSettings";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "COM-Port Settings";
            ((System.ComponentModel.ISupportInitialize)(this.dataNumb)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox baud;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox parity;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown dataNumb;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox stopbits;
        private System.Windows.Forms.Button button1;
    }
}