namespace VoluntaryPensionFundSystem
{
    partial class BaseForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(BaseForm));
            this.Fomenu = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.bezárToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.nyomtatásiKépToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.nyomtatásToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mindentBezárToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.kilépésToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.névjegyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tsIkonsor = new System.Windows.Forms.ToolStrip();
            this.tsNew = new System.Windows.Forms.ToolStripButton();
            this.tsSave = new System.Windows.Forms.ToolStripButton();
            this.tsUpdate = new System.Windows.Forms.ToolStripButton();
            this.tsSearch = new System.Windows.Forms.ToolStripButton();
            this.tsFind = new System.Windows.Forms.ToolStripButton();
            this.tsDelete = new System.Windows.Forms.ToolStripButton();
            this.tsExit = new System.Windows.Forms.ToolStripButton();
            this.pageSetupDialog1 = new System.Windows.Forms.PageSetupDialog();
            this.printDialog1 = new System.Windows.Forms.PrintDialog();
            this.Fomenu.SuspendLayout();
            this.tsIkonsor.SuspendLayout();
            this.SuspendLayout();
            // 
            // Fomenu
            // 
            this.Fomenu.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.Fomenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.Fomenu.Location = new System.Drawing.Point(0, 0);
            this.Fomenu.Name = "Fomenu";
            this.Fomenu.Size = new System.Drawing.Size(1008, 29);
            this.Fomenu.TabIndex = 1;
            this.Fomenu.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.bezárToolStripMenuItem,
            this.nyomtatásiKépToolStripMenuItem,
            this.nyomtatásToolStripMenuItem,
            this.mindentBezárToolStripMenuItem,
            this.kilépésToolStripMenuItem1});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(46, 25);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // bezárToolStripMenuItem
            // 
            this.bezárToolStripMenuItem.Name = "bezárToolStripMenuItem";
            this.bezárToolStripMenuItem.Size = new System.Drawing.Size(189, 26);
            this.bezárToolStripMenuItem.Text = "Bezár";
            this.bezárToolStripMenuItem.Click += new System.EventHandler(this.bezárToolStripMenuItem_Click);
            // 
            // nyomtatásiKépToolStripMenuItem
            // 
            this.nyomtatásiKépToolStripMenuItem.Name = "nyomtatásiKépToolStripMenuItem";
            this.nyomtatásiKépToolStripMenuItem.Size = new System.Drawing.Size(189, 26);
            this.nyomtatásiKépToolStripMenuItem.Text = "Nyomtatási kép";
            this.nyomtatásiKépToolStripMenuItem.Click += new System.EventHandler(this.nyomtatásiKépToolStripMenuItem_Click);
            // 
            // nyomtatásToolStripMenuItem
            // 
            this.nyomtatásToolStripMenuItem.Name = "nyomtatásToolStripMenuItem";
            this.nyomtatásToolStripMenuItem.Size = new System.Drawing.Size(189, 26);
            this.nyomtatásToolStripMenuItem.Text = "Nyomtatás";
            this.nyomtatásToolStripMenuItem.Click += new System.EventHandler(this.nyomtatásToolStripMenuItem_Click);
            // 
            // mindentBezárToolStripMenuItem
            // 
            this.mindentBezárToolStripMenuItem.Name = "mindentBezárToolStripMenuItem";
            this.mindentBezárToolStripMenuItem.Size = new System.Drawing.Size(189, 26);
            this.mindentBezárToolStripMenuItem.Text = "Mindent bezár";
            this.mindentBezárToolStripMenuItem.Click += new System.EventHandler(this.mindentBezárToolStripMenuItem_Click);
            // 
            // kilépésToolStripMenuItem1
            // 
            this.kilépésToolStripMenuItem1.Name = "kilépésToolStripMenuItem1";
            this.kilépésToolStripMenuItem1.Size = new System.Drawing.Size(189, 26);
            this.kilépésToolStripMenuItem1.Text = "Kilépés";
            this.kilépésToolStripMenuItem1.Click += new System.EventHandler(this.kilépésToolStripMenuItem1_Click);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.névjegyToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(54, 25);
            this.helpToolStripMenuItem.Text = "Help";
            // 
            // névjegyToolStripMenuItem
            // 
            this.névjegyToolStripMenuItem.Name = "névjegyToolStripMenuItem";
            this.névjegyToolStripMenuItem.Size = new System.Drawing.Size(152, 26);
            this.névjegyToolStripMenuItem.Text = "Névjegy";
            this.névjegyToolStripMenuItem.Click += new System.EventHandler(this.névjegyToolStripMenuItem_Click);
            // 
            // tsIkonsor
            // 
            this.tsIkonsor.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.tsIkonsor.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsNew,
            this.tsSave,
            this.tsUpdate,
            this.tsSearch,
            this.tsFind,
            this.tsDelete,
            this.tsExit});
            this.tsIkonsor.Location = new System.Drawing.Point(0, 29);
            this.tsIkonsor.Name = "tsIkonsor";
            this.tsIkonsor.Size = new System.Drawing.Size(1008, 28);
            this.tsIkonsor.TabIndex = 2;
            this.tsIkonsor.Text = "Ikonsor";
            // 
            // tsNew
            // 
            this.tsNew.Image = global::VoluntaryPensionFundSystem.Properties.Resources.edit_add;
            this.tsNew.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsNew.Name = "tsNew";
            this.tsNew.Size = new System.Drawing.Size(45, 25);
            this.tsNew.Text = "Új";
            this.tsNew.ToolTipText = "Új - F4";
            this.tsNew.Click += new System.EventHandler(this.tsNew_Click);
            // 
            // tsSave
            // 
            this.tsSave.Image = global::VoluntaryPensionFundSystem.Properties.Resources.media_floppy_3_5_2;
            this.tsSave.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsSave.Name = "tsSave";
            this.tsSave.Size = new System.Drawing.Size(81, 25);
            this.tsSave.Text = "Mentés";
            this.tsSave.ToolTipText = "Mentés - F10";
            this.tsSave.Click += new System.EventHandler(this.tsSave_Click);
            // 
            // tsUpdate
            // 
            this.tsUpdate.Image = global::VoluntaryPensionFundSystem.Properties.Resources.edit;
            this.tsUpdate.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsUpdate.Name = "tsUpdate";
            this.tsUpdate.Size = new System.Drawing.Size(87, 25);
            this.tsUpdate.Text = "Módosít";
            this.tsUpdate.ToolTipText = "Módosít - F9";
            this.tsUpdate.Click += new System.EventHandler(this.tsUpdate_Click);
            // 
            // tsSearch
            // 
            this.tsSearch.Image = global::VoluntaryPensionFundSystem.Properties.Resources.viewmag;
            this.tsSearch.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsSearch.Name = "tsSearch";
            this.tsSearch.Size = new System.Drawing.Size(109, 25);
            this.tsSearch.Text = "Keresőmód";
            this.tsSearch.ToolTipText = "Keresés - F7";
            this.tsSearch.Click += new System.EventHandler(this.tsSearch_Click);
            // 
            // tsFind
            // 
            this.tsFind.Image = global::VoluntaryPensionFundSystem.Properties.Resources.forward;
            this.tsFind.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsFind.Name = "tsFind";
            this.tsFind.Size = new System.Drawing.Size(83, 25);
            this.tsFind.Text = "Keresés";
            this.tsFind.ToolTipText = "Keresés indítása - F8";
            this.tsFind.Click += new System.EventHandler(this.tsFind_Click);
            // 
            // tsDelete
            // 
            this.tsDelete.Image = global::VoluntaryPensionFundSystem.Properties.Resources.button_cancel;
            this.tsDelete.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsDelete.Name = "tsDelete";
            this.tsDelete.Size = new System.Drawing.Size(72, 25);
            this.tsDelete.Text = "Törlés";
            this.tsDelete.ToolTipText = "Törlés - F5";
            this.tsDelete.Click += new System.EventHandler(this.tsDelete_Click);
            // 
            // tsExit
            // 
            this.tsExit.Image = global::VoluntaryPensionFundSystem.Properties.Resources.exit;
            this.tsExit.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsExit.Name = "tsExit";
            this.tsExit.Size = new System.Drawing.Size(79, 25);
            this.tsExit.Text = "Kilépés";
            this.tsExit.Click += new System.EventHandler(this.tsExit_Click);
            // 
            // printDialog1
            // 
            this.printDialog1.UseEXDialog = true;
            // 
            // BaseForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.GhostWhite;
            this.ClientSize = new System.Drawing.Size(1008, 522);
            this.Controls.Add(this.tsIkonsor);
            this.Controls.Add(this.Fomenu);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "BaseForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Load += new System.EventHandler(this.BaseForm_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.BaseForm_KeyDown);
            this.Fomenu.ResumeLayout(false);
            this.Fomenu.PerformLayout();
            this.tsIkonsor.ResumeLayout(false);
            this.tsIkonsor.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem bezárToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem nyomtatásToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem mindentBezárToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem kilépésToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem névjegyToolStripMenuItem;
        public System.Windows.Forms.ToolStrip tsIkonsor;
        public System.Windows.Forms.ToolStripButton tsNew;
        public System.Windows.Forms.ToolStripButton tsSave;
        public System.Windows.Forms.ToolStripButton tsUpdate;
        public System.Windows.Forms.ToolStripButton tsSearch;
        public System.Windows.Forms.ToolStripButton tsFind;
        public System.Windows.Forms.ToolStripButton tsDelete;
        public System.Windows.Forms.ToolStripButton tsExit;
        public System.Windows.Forms.MenuStrip Fomenu;
        private System.Windows.Forms.ToolStripMenuItem nyomtatásiKépToolStripMenuItem;
        private System.Windows.Forms.PageSetupDialog pageSetupDialog1;
        private System.Windows.Forms.PrintDialog printDialog1;
    }
}

