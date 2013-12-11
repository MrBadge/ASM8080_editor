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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.serialPortSettingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.stStrip = new System.Windows.Forms.StatusStrip();
            this.tsStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.tsFiles = new FarsiLibrary.Win.FATabStrip();
            this.menu = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.newToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator9 = new System.Windows.Forms.ToolStripSeparator();
            this.openToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.saveAsToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.editToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.copyToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.pasteToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.cutToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator10 = new System.Windows.Forms.ToolStripSeparator();
            this.findToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.replaceToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator11 = new System.Windows.Forms.ToolStripSeparator();
            this.undoToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.redoToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator12 = new System.Windows.Forms.ToolStripSeparator();
            this.commUncommToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.highlightCurrentLineToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.projectToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.compileToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.sendToKR580ToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.viewHexToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.settingsToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.loadCustomASMDescFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.serialPortToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.comToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.serialPortSettingsToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.showSmallLoaderToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.decompileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.decompileToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.fromFileToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.fromMemoryToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.getMemoryDumpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tsFiles)).BeginInit();
            this.menu.SuspendLayout();
            this.SuspendLayout();
            // 
            // serialPortSettingsToolStripMenuItem
            // 
            this.serialPortSettingsToolStripMenuItem.Name = "serialPortSettingsToolStripMenuItem";
            this.serialPortSettingsToolStripMenuItem.Size = new System.Drawing.Size(218, 22);
            this.serialPortSettingsToolStripMenuItem.Text = "Serial port settings";
            this.serialPortSettingsToolStripMenuItem.Click += new System.EventHandler(this.serialPortSettingsToolStripMenuItem_Click);
            // 
            // stStrip
            // 
            this.stStrip.Location = new System.Drawing.Point(0, 609);
            this.stStrip.Name = "stStrip";
            this.stStrip.Padding = new System.Windows.Forms.Padding(2, 0, 28, 0);
            this.stStrip.Size = new System.Drawing.Size(856, 22);
            this.stStrip.TabIndex = 4;
            this.stStrip.Text = "statusStrip1";
            // 
            // tsStatus
            // 
            this.tsStatus.Name = "tsStatus";
            this.tsStatus.Size = new System.Drawing.Size(413, 17);
            this.tsStatus.Spring = true;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.tsFiles);
            this.panel1.Location = new System.Drawing.Point(24, 52);
            this.panel1.Margin = new System.Windows.Forms.Padding(6);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(808, 531);
            this.panel1.TabIndex = 5;
            // 
            // tsFiles
            // 
            this.tsFiles.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.tsFiles.Location = new System.Drawing.Point(94, 48);
            this.tsFiles.Name = "tsFiles";
            this.tsFiles.Size = new System.Drawing.Size(460, 335);
            this.tsFiles.TabIndex = 4;
            this.tsFiles.Text = "faTabStrip1";
            // 
            // menu
            // 
            this.menu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem1,
            this.editToolStripMenuItem1,
            this.projectToolStripMenuItem1,
            this.settingsToolStripMenuItem1,
            this.decompileToolStripMenuItem});
            this.menu.Location = new System.Drawing.Point(0, 0);
            this.menu.Name = "menu";
            this.menu.Padding = new System.Windows.Forms.Padding(12, 4, 0, 4);
            this.menu.Size = new System.Drawing.Size(856, 44);
            this.menu.TabIndex = 1;
            this.menu.Text = "menu";
            // 
            // fileToolStripMenuItem1
            // 
            this.fileToolStripMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newToolStripMenuItem1,
            this.toolStripSeparator9,
            this.openToolStripMenuItem1,
            this.saveToolStripMenuItem1,
            this.saveAsToolStripMenuItem1});
            this.fileToolStripMenuItem1.Name = "fileToolStripMenuItem1";
            this.fileToolStripMenuItem1.Size = new System.Drawing.Size(64, 36);
            this.fileToolStripMenuItem1.Text = "File";
            this.fileToolStripMenuItem1.DropDownOpening += new System.EventHandler(this.fileToolStripMenuItem_DropDownOpening);
            // 
            // newToolStripMenuItem1
            // 
            this.newToolStripMenuItem1.Name = "newToolStripMenuItem1";
            this.newToolStripMenuItem1.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.N)));
            this.newToolStripMenuItem1.Size = new System.Drawing.Size(315, 36);
            this.newToolStripMenuItem1.Text = "New";
            this.newToolStripMenuItem1.Click += new System.EventHandler(this.newToolStripMenuItem_Click);
            // 
            // toolStripSeparator9
            // 
            this.toolStripSeparator9.Name = "toolStripSeparator9";
            this.toolStripSeparator9.Size = new System.Drawing.Size(312, 6);
            // 
            // openToolStripMenuItem1
            // 
            this.openToolStripMenuItem1.Name = "openToolStripMenuItem1";
            this.openToolStripMenuItem1.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.openToolStripMenuItem1.Size = new System.Drawing.Size(315, 36);
            this.openToolStripMenuItem1.Text = "Open";
            this.openToolStripMenuItem1.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
            // 
            // saveToolStripMenuItem1
            // 
            this.saveToolStripMenuItem1.Name = "saveToolStripMenuItem1";
            this.saveToolStripMenuItem1.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.saveToolStripMenuItem1.Size = new System.Drawing.Size(315, 36);
            this.saveToolStripMenuItem1.Text = "Save";
            this.saveToolStripMenuItem1.Click += new System.EventHandler(this.saveToolStripMenuItem_Click);
            // 
            // saveAsToolStripMenuItem1
            // 
            this.saveAsToolStripMenuItem1.Name = "saveAsToolStripMenuItem1";
            this.saveAsToolStripMenuItem1.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.S)));
            this.saveAsToolStripMenuItem1.Size = new System.Drawing.Size(315, 36);
            this.saveAsToolStripMenuItem1.Text = "Save as";
            this.saveAsToolStripMenuItem1.Click += new System.EventHandler(this.saveAsToolStripMenuItem_Click);
            // 
            // editToolStripMenuItem1
            // 
            this.editToolStripMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.copyToolStripMenuItem1,
            this.pasteToolStripMenuItem1,
            this.cutToolStripMenuItem1,
            this.toolStripSeparator10,
            this.findToolStripMenuItem1,
            this.replaceToolStripMenuItem1,
            this.toolStripSeparator11,
            this.undoToolStripMenuItem1,
            this.redoToolStripMenuItem1,
            this.toolStripSeparator12,
            this.commUncommToolStripMenuItem,
            this.highlightCurrentLineToolStripMenuItem});
            this.editToolStripMenuItem1.Name = "editToolStripMenuItem1";
            this.editToolStripMenuItem1.Size = new System.Drawing.Size(67, 36);
            this.editToolStripMenuItem1.Text = "Edit";
            this.editToolStripMenuItem1.DropDownOpening += new System.EventHandler(this.editToolStripMenuItem_DropDownOpening);
            // 
            // copyToolStripMenuItem1
            // 
            this.copyToolStripMenuItem1.Name = "copyToolStripMenuItem1";
            this.copyToolStripMenuItem1.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.C)));
            this.copyToolStripMenuItem1.Size = new System.Drawing.Size(349, 36);
            this.copyToolStripMenuItem1.Text = "Copy";
            this.copyToolStripMenuItem1.Click += new System.EventHandler(this.copyToolStripMenuItem_Click);
            // 
            // pasteToolStripMenuItem1
            // 
            this.pasteToolStripMenuItem1.Name = "pasteToolStripMenuItem1";
            this.pasteToolStripMenuItem1.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.V)));
            this.pasteToolStripMenuItem1.Size = new System.Drawing.Size(349, 36);
            this.pasteToolStripMenuItem1.Text = "Paste";
            this.pasteToolStripMenuItem1.Click += new System.EventHandler(this.pasteToolStripMenuItem_Click);
            // 
            // cutToolStripMenuItem1
            // 
            this.cutToolStripMenuItem1.Name = "cutToolStripMenuItem1";
            this.cutToolStripMenuItem1.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.X)));
            this.cutToolStripMenuItem1.Size = new System.Drawing.Size(349, 36);
            this.cutToolStripMenuItem1.Text = "Cut";
            this.cutToolStripMenuItem1.Click += new System.EventHandler(this.cutToolStripMenuItem_Click);
            // 
            // toolStripSeparator10
            // 
            this.toolStripSeparator10.Name = "toolStripSeparator10";
            this.toolStripSeparator10.Size = new System.Drawing.Size(346, 6);
            // 
            // findToolStripMenuItem1
            // 
            this.findToolStripMenuItem1.Name = "findToolStripMenuItem1";
            this.findToolStripMenuItem1.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.F)));
            this.findToolStripMenuItem1.Size = new System.Drawing.Size(349, 36);
            this.findToolStripMenuItem1.Text = "Find";
            this.findToolStripMenuItem1.Click += new System.EventHandler(this.findToolStripMenuItem_Click);
            // 
            // replaceToolStripMenuItem1
            // 
            this.replaceToolStripMenuItem1.Name = "replaceToolStripMenuItem1";
            this.replaceToolStripMenuItem1.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.R)));
            this.replaceToolStripMenuItem1.Size = new System.Drawing.Size(349, 36);
            this.replaceToolStripMenuItem1.Text = "Replace";
            this.replaceToolStripMenuItem1.Click += new System.EventHandler(this.replaceToolStripMenuItem_Click);
            // 
            // toolStripSeparator11
            // 
            this.toolStripSeparator11.Name = "toolStripSeparator11";
            this.toolStripSeparator11.Size = new System.Drawing.Size(346, 6);
            // 
            // undoToolStripMenuItem1
            // 
            this.undoToolStripMenuItem1.Name = "undoToolStripMenuItem1";
            this.undoToolStripMenuItem1.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Z)));
            this.undoToolStripMenuItem1.Size = new System.Drawing.Size(349, 36);
            this.undoToolStripMenuItem1.Text = "Undo";
            this.undoToolStripMenuItem1.Click += new System.EventHandler(this.undoToolStripMenuItem_Click);
            // 
            // redoToolStripMenuItem1
            // 
            this.redoToolStripMenuItem1.Name = "redoToolStripMenuItem1";
            this.redoToolStripMenuItem1.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Y)));
            this.redoToolStripMenuItem1.Size = new System.Drawing.Size(349, 36);
            this.redoToolStripMenuItem1.Text = "Redo";
            this.redoToolStripMenuItem1.Click += new System.EventHandler(this.redoToolStripMenuItem_Click);
            // 
            // toolStripSeparator12
            // 
            this.toolStripSeparator12.Name = "toolStripSeparator12";
            this.toolStripSeparator12.Size = new System.Drawing.Size(346, 6);
            // 
            // commUncommToolStripMenuItem
            // 
            this.commUncommToolStripMenuItem.Name = "commUncommToolStripMenuItem";
            this.commUncommToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.K)));
            this.commUncommToolStripMenuItem.Size = new System.Drawing.Size(349, 36);
            this.commUncommToolStripMenuItem.Text = "Comm/Uncomm";
            this.commUncommToolStripMenuItem.Click += new System.EventHandler(this.commentSelectedToolStripMenuItem_Click);
            // 
            // highlightCurrentLineToolStripMenuItem
            // 
            this.highlightCurrentLineToolStripMenuItem.Name = "highlightCurrentLineToolStripMenuItem";
            this.highlightCurrentLineToolStripMenuItem.Size = new System.Drawing.Size(349, 36);
            this.highlightCurrentLineToolStripMenuItem.Text = "Highlight current line";
            this.highlightCurrentLineToolStripMenuItem.Click += new System.EventHandler(this.highlightCurrentLineToolStripMenuItem_Click);
            // 
            // projectToolStripMenuItem1
            // 
            this.projectToolStripMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.compileToolStripMenuItem1,
            this.sendToKR580ToolStripMenuItem1,
            this.viewHexToolStripMenuItem1});
            this.projectToolStripMenuItem1.Name = "projectToolStripMenuItem1";
            this.projectToolStripMenuItem1.Size = new System.Drawing.Size(100, 36);
            this.projectToolStripMenuItem1.Text = "Project";
            this.projectToolStripMenuItem1.DropDownOpening += new System.EventHandler(this.projectToolStripMenuItem_DropDownOpening);
            // 
            // compileToolStripMenuItem1
            // 
            this.compileToolStripMenuItem1.Name = "compileToolStripMenuItem1";
            this.compileToolStripMenuItem1.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.B)));
            this.compileToolStripMenuItem1.Size = new System.Drawing.Size(326, 36);
            this.compileToolStripMenuItem1.Text = "Compile";
            this.compileToolStripMenuItem1.Click += new System.EventHandler(this.compileToolStripMenuItem_Click);
            // 
            // sendToKR580ToolStripMenuItem1
            // 
            this.sendToKR580ToolStripMenuItem1.Name = "sendToKR580ToolStripMenuItem1";
            this.sendToKR580ToolStripMenuItem1.ShortcutKeys = System.Windows.Forms.Keys.F5;
            this.sendToKR580ToolStripMenuItem1.Size = new System.Drawing.Size(326, 36);
            this.sendToKR580ToolStripMenuItem1.Text = "Send to KR580";
            this.sendToKR580ToolStripMenuItem1.Click += new System.EventHandler(this.sendToKR580ToolStripMenuItem_Click);
            // 
            // viewHexToolStripMenuItem1
            // 
            this.viewHexToolStripMenuItem1.Name = "viewHexToolStripMenuItem1";
            this.viewHexToolStripMenuItem1.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.H)));
            this.viewHexToolStripMenuItem1.Size = new System.Drawing.Size(326, 36);
            this.viewHexToolStripMenuItem1.Text = "View hex";
            this.viewHexToolStripMenuItem1.Click += new System.EventHandler(this.viewHexToolStripMenuItem_Click);
            // 
            // settingsToolStripMenuItem1
            // 
            this.settingsToolStripMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.loadCustomASMDescFileToolStripMenuItem,
            this.serialPortToolStripMenuItem1,
            this.serialPortSettingsToolStripMenuItem1,
            this.showSmallLoaderToolStripMenuItem});
            this.settingsToolStripMenuItem1.Name = "settingsToolStripMenuItem1";
            this.settingsToolStripMenuItem1.Size = new System.Drawing.Size(113, 36);
            this.settingsToolStripMenuItem1.Text = "Settings";
            // 
            // loadCustomASMDescFileToolStripMenuItem
            // 
            this.loadCustomASMDescFileToolStripMenuItem.Name = "loadCustomASMDescFileToolStripMenuItem";
            this.loadCustomASMDescFileToolStripMenuItem.Size = new System.Drawing.Size(378, 36);
            this.loadCustomASMDescFileToolStripMenuItem.Text = "Load custom ASM desc file";
            this.loadCustomASMDescFileToolStripMenuItem.Click += new System.EventHandler(this.loadASMDescFileToolStripMenuItem_Click);
            // 
            // serialPortToolStripMenuItem1
            // 
            this.serialPortToolStripMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.comToolStripMenuItem1});
            this.serialPortToolStripMenuItem1.Name = "serialPortToolStripMenuItem1";
            this.serialPortToolStripMenuItem1.Size = new System.Drawing.Size(378, 36);
            this.serialPortToolStripMenuItem1.Text = "Serial port";
            this.serialPortToolStripMenuItem1.DropDownOpening += new System.EventHandler(this.serialPortToolStripMenuItem_DropDownOpening);
            // 
            // comToolStripMenuItem1
            // 
            this.comToolStripMenuItem1.Name = "comToolStripMenuItem1";
            this.comToolStripMenuItem1.Size = new System.Drawing.Size(136, 36);
            this.comToolStripMenuItem1.Text = "com";
            // 
            // serialPortSettingsToolStripMenuItem1
            // 
            this.serialPortSettingsToolStripMenuItem1.Name = "serialPortSettingsToolStripMenuItem1";
            this.serialPortSettingsToolStripMenuItem1.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.P)));
            this.serialPortSettingsToolStripMenuItem1.Size = new System.Drawing.Size(378, 36);
            this.serialPortSettingsToolStripMenuItem1.Text = "Settings";
            this.serialPortSettingsToolStripMenuItem1.Click += new System.EventHandler(this.serialPortSettingsToolStripMenuItem_Click);
            // 
            // showSmallLoaderToolStripMenuItem
            // 
            this.showSmallLoaderToolStripMenuItem.Name = "showSmallLoaderToolStripMenuItem";
            this.showSmallLoaderToolStripMenuItem.Size = new System.Drawing.Size(378, 36);
            this.showSmallLoaderToolStripMenuItem.Text = "Show Small Loader";
            this.showSmallLoaderToolStripMenuItem.Click += new System.EventHandler(this.showSmallLoaderToolStripMenuItem_Click);
            // 
            // decompileToolStripMenuItem
            // 
            this.decompileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.decompileToolStripMenuItem1,
            this.getMemoryDumpToolStripMenuItem});
            this.decompileToolStripMenuItem.Name = "decompileToolStripMenuItem";
            this.decompileToolStripMenuItem.Size = new System.Drawing.Size(84, 36);
            this.decompileToolStripMenuItem.Text = "Tools";
            // 
            // decompileToolStripMenuItem1
            // 
            this.decompileToolStripMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fromFileToolStripMenuItem1,
            this.fromMemoryToolStripMenuItem1});
            this.decompileToolStripMenuItem1.Name = "decompileToolStripMenuItem1";
            this.decompileToolStripMenuItem1.Size = new System.Drawing.Size(293, 36);
            this.decompileToolStripMenuItem1.Text = "Decompile";
            // 
            // fromFileToolStripMenuItem1
            // 
            this.fromFileToolStripMenuItem1.Name = "fromFileToolStripMenuItem1";
            this.fromFileToolStripMenuItem1.Size = new System.Drawing.Size(244, 36);
            this.fromFileToolStripMenuItem1.Text = "From file";
            this.fromFileToolStripMenuItem1.Click += new System.EventHandler(this.fromFileToolStripMenuItem1_Click);
            // 
            // fromMemoryToolStripMenuItem1
            // 
            this.fromMemoryToolStripMenuItem1.Name = "fromMemoryToolStripMenuItem1";
            this.fromMemoryToolStripMenuItem1.Size = new System.Drawing.Size(244, 36);
            this.fromMemoryToolStripMenuItem1.Text = "From memory";
            this.fromMemoryToolStripMenuItem1.Click += new System.EventHandler(this.fromMemoryToolStripMenuItem1_Click);
            // 
            // getMemoryDumpToolStripMenuItem
            // 
            this.getMemoryDumpToolStripMenuItem.Name = "getMemoryDumpToolStripMenuItem";
            this.getMemoryDumpToolStripMenuItem.Size = new System.Drawing.Size(293, 36);
            this.getMemoryDumpToolStripMenuItem.Text = "Get memory dump";
            this.getMemoryDumpToolStripMenuItem.Click += new System.EventHandler(this.getMemoryDumpToolStripMenuItem_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(856, 631);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.stStrip);
            this.Controls.Add(this.menu);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menu;
            this.Margin = new System.Windows.Forms.Padding(6);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Form1";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.tsFiles)).EndInit();
            this.menu.ResumeLayout(false);
            this.menu.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.StatusStrip stStrip;
        private System.Windows.Forms.Panel panel1;
        private FarsiLibrary.Win.FATabStrip tsFiles;
        private System.Windows.Forms.ToolStripStatusLabel tsStatus;
        private System.Windows.Forms.ToolStripMenuItem serialPortSettingsToolStripMenuItem;
        private System.Windows.Forms.MenuStrip menu;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem newToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem projectToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem settingsToolStripMenuItem1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator9;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem saveAsToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem copyToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem pasteToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem cutToolStripMenuItem1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator10;
        private System.Windows.Forms.ToolStripMenuItem findToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem replaceToolStripMenuItem1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator11;
        private System.Windows.Forms.ToolStripMenuItem undoToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem redoToolStripMenuItem1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator12;
        private System.Windows.Forms.ToolStripMenuItem commUncommToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem compileToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem sendToKR580ToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem viewHexToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem loadCustomASMDescFileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem serialPortToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem comToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem serialPortSettingsToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem highlightCurrentLineToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem decompileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem showSmallLoaderToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem decompileToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem fromFileToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem fromMemoryToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem getMemoryDumpToolStripMenuItem;
    }
}

