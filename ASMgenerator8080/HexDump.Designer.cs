namespace ASMgenerator8080
{
    partial class HexDump
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
            this.dumpView = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.dumpView)).BeginInit();
            this.SuspendLayout();
            // 
            // dumpView
            // 
            this.dumpView.BackgroundColor = System.Drawing.Color.White;
            this.dumpView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dumpView.Location = new System.Drawing.Point(12, 12);
            this.dumpView.Name = "dumpView";
            this.dumpView.RowHeadersVisible = false;
            this.dumpView.Size = new System.Drawing.Size(551, 237);
            this.dumpView.TabIndex = 0;
            // 
            // HexDump
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(575, 261);
            this.Controls.Add(this.dumpView);
            this.Name = "HexDump";
            this.Text = "HexDump";
            this.Resize += new System.EventHandler(this.HexDump_Resize);
            ((System.ComponentModel.ISupportInitialize)(this.dumpView)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dumpView;







    }
}