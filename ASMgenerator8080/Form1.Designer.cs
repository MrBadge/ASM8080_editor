namespace ASMgenerator8080
{
    partial class Form1
    {
        /// <summary>
        /// Требуется переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Обязательный метод для поддержки конструктора - не изменяйте
        /// содержимое данного метода при помощи редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.mainField = new FastColoredTextBoxNS.FastColoredTextBox();
            ((System.ComponentModel.ISupportInitialize)(this.mainField)).BeginInit();
            this.SuspendLayout();
            // 
            // mainField
            // 
            this.mainField.AutoScrollMinSize = new System.Drawing.Size(27, 14);
            this.mainField.BackBrush = null;
            this.mainField.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.mainField.CommentPrefix = ";";
            this.mainField.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.mainField.DescriptionFile = "C:\\Dropbox\\MEPhI\\ASMgenerator8080\\asmDesc.xml";
            this.mainField.DisabledColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            this.mainField.Font = new System.Drawing.Font("Courier New", 9.75F);
            this.mainField.IsReplaceMode = false;
            this.mainField.Location = new System.Drawing.Point(3, 12);
            this.mainField.Name = "mainField";
            this.mainField.Paddings = new System.Windows.Forms.Padding(0);
            this.mainField.SelectionColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.mainField.Size = new System.Drawing.Size(381, 272);
            this.mainField.TabIndex = 0;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(396, 301);
            this.Controls.Add(this.mainField);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.SizeChanged += new System.EventHandler(this.Form1_SizeChanged);
            ((System.ComponentModel.ISupportInitialize)(this.mainField)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private FastColoredTextBoxNS.FastColoredTextBox mainField;
    }
}

