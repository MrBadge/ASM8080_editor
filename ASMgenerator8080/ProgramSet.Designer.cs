namespace ASMgenerator8080
{
    partial class ProgramSet
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
            this.staddr = new System.Windows.Forms.Label();
            this.apply = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.Location = new System.Drawing.Point(12, 81);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(0, 31);
            this.label1.TabIndex = 0;
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // staddr
            // 
            this.staddr.AutoSize = true;
            this.staddr.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.staddr.Location = new System.Drawing.Point(11, 19);
            this.staddr.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.staddr.Name = "staddr";
            this.staddr.Size = new System.Drawing.Size(201, 37);
            this.staddr.TabIndex = 1;
            this.staddr.Text = "Start addres:";
            // 
            // apply
            // 
            this.apply.Location = new System.Drawing.Point(179, 81);
            this.apply.Name = "apply";
            this.apply.Size = new System.Drawing.Size(202, 52);
            this.apply.TabIndex = 2;
            this.apply.Text = "Apply";
            this.apply.UseVisualStyleBackColor = true;
            this.apply.Click += new System.EventHandler(this.apply_Click);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(261, 19);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(286, 31);
            this.textBox1.TabIndex = 3;
            // 
            // ProgramSet
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(572, 149);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.apply);
            this.Controls.Add(this.staddr);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "ProgramSet";
            this.Text = "ProgramSet";
            this.Load += new System.EventHandler(this.ProgramSet_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label staddr;
        private System.Windows.Forms.Button apply;
        private System.Windows.Forms.TextBox textBox1;
    }
}