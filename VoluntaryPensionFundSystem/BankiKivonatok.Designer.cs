namespace VoluntaryPensionFundSystem
{
    partial class BankiKivonatok
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
            this.components = new System.ComponentModel.Container();
            this.label2 = new System.Windows.Forms.Label();
            this.dgvBankKiv = new System.Windows.Forms.DataGridView();
            this.spBankikivonatokBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.vPFSDataSet = new VoluntaryPensionFundSystem.VPFSDataSet();
            this.txId = new System.Windows.Forms.TextBox();
            this.txMegjegyzes = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.lbVege = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.cbIrany = new System.Windows.Forms.ComboBox();
            this.spBanki_kivonatokTableAdapter = new VoluntaryPensionFundSystem.VPFSDataSetTableAdapters.spBanki_kivonatokTableAdapter();
            this.txKivonatSzama = new System.Windows.Forms.NumericUpDown();
            this.datLetrehoz = new System.Windows.Forms.TextBox();
            this.datErteknap = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.dgvBankKiv)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.spBankikivonatokBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.vPFSDataSet)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txKivonatSzama)).BeginInit();
            this.SuspendLayout();
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label2.Location = new System.Drawing.Point(334, 67);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(180, 29);
            this.label2.TabIndex = 103;
            this.label2.Text = "Banki kivonatok";
            // 
            // dgvBankKiv
            // 
            this.dgvBankKiv.AllowUserToAddRows = false;
            this.dgvBankKiv.AllowUserToDeleteRows = false;
            this.dgvBankKiv.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvBankKiv.Location = new System.Drawing.Point(32, 115);
            this.dgvBankKiv.MultiSelect = false;
            this.dgvBankKiv.Name = "dgvBankKiv";
            this.dgvBankKiv.ReadOnly = true;
            this.dgvBankKiv.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.dgvBankKiv.ShowEditingIcon = false;
            this.dgvBankKiv.Size = new System.Drawing.Size(784, 164);
            this.dgvBankKiv.TabIndex = 0;
            this.dgvBankKiv.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dgvBankKiv_CellFormatting);
            this.dgvBankKiv.SelectionChanged += new System.EventHandler(this.dgvBankKiv_SelectionChanged);
            this.dgvBankKiv.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dgvBankKiv_KeyDown);
            this.dgvBankKiv.MouseMove += new System.Windows.Forms.MouseEventHandler(this.dgvBankKiv_MouseMove);
            // 
            // spBankikivonatokBindingSource
            // 
            this.spBankikivonatokBindingSource.DataMember = "spBanki_kivonatok";
            this.spBankikivonatokBindingSource.DataSource = this.vPFSDataSet;
            // 
            // vPFSDataSet
            // 
            this.vPFSDataSet.DataSetName = "VPFSDataSet";
            this.vPFSDataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // txId
            // 
            this.txId.Location = new System.Drawing.Point(786, 354);
            this.txId.MaxLength = 1;
            this.txId.Name = "txId";
            this.txId.ReadOnly = true;
            this.txId.Size = new System.Drawing.Size(46, 23);
            this.txId.TabIndex = 129;
            this.txId.Visible = false;
            // 
            // txMegjegyzes
            // 
            this.txMegjegyzes.Location = new System.Drawing.Point(34, 354);
            this.txMegjegyzes.MaxLength = 255;
            this.txMegjegyzes.Name = "txMegjegyzes";
            this.txMegjegyzes.ReadOnly = true;
            this.txMegjegyzes.Size = new System.Drawing.Size(746, 23);
            this.txMegjegyzes.TabIndex = 5;
            this.txMegjegyzes.TextChanged += new System.EventHandler(this.txMegjegyzes_TextChanged);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(33, 334);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(83, 17);
            this.label7.TabIndex = 127;
            this.label7.Text = "Megjegyzés";
            // 
            // lbVege
            // 
            this.lbVege.AutoSize = true;
            this.lbVege.Location = new System.Drawing.Point(323, 288);
            this.lbVege.Name = "lbVege";
            this.lbVege.Size = new System.Drawing.Size(130, 17);
            this.lbVege.TabIndex = 126;
            this.lbVege.Text = "Létrehozás dátuma";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(33, 288);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(65, 17);
            this.label5.TabIndex = 125;
            this.label5.Text = "Értéknap";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(255, 288);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(39, 17);
            this.label4.TabIndex = 123;
            this.label4.Text = "Irány";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(149, 288);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(100, 17);
            this.label1.TabIndex = 121;
            this.label1.Text = "Kivonat száma";
            // 
            // cbIrany
            // 
            this.cbIrany.Enabled = false;
            this.cbIrany.FormattingEnabled = true;
            this.cbIrany.Location = new System.Drawing.Point(255, 309);
            this.cbIrany.Name = "cbIrany";
            this.cbIrany.Size = new System.Drawing.Size(65, 24);
            this.cbIrany.TabIndex = 3;
            this.cbIrany.TextChanged += new System.EventHandler(this.cbIrany_TextChanged);
            this.cbIrany.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.cbIrany_KeyPress);
            // 
            // spBanki_kivonatokTableAdapter
            // 
            this.spBanki_kivonatokTableAdapter.ClearBeforeFill = true;
            // 
            // txKivonatSzama
            // 
            this.txKivonatSzama.Enabled = false;
            this.txKivonatSzama.Location = new System.Drawing.Point(152, 310);
            this.txKivonatSzama.Name = "txKivonatSzama";
            this.txKivonatSzama.Size = new System.Drawing.Size(97, 23);
            this.txKivonatSzama.TabIndex = 2;
            this.txKivonatSzama.ValueChanged += new System.EventHandler(this.txKivonatSzama_ValueChanged);
            this.txKivonatSzama.Enter += new System.EventHandler(this.txKivonatSzama_Enter);
            this.txKivonatSzama.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txKivonatSzama_KeyPress);
            // 
            // datLetrehoz
            // 
            this.datLetrehoz.Location = new System.Drawing.Point(326, 309);
            this.datLetrehoz.Name = "datLetrehoz";
            this.datLetrehoz.ReadOnly = true;
            this.datLetrehoz.Size = new System.Drawing.Size(127, 23);
            this.datLetrehoz.TabIndex = 4;
            this.datLetrehoz.TextChanged += new System.EventHandler(this.datLetrehoz_TextChanged);
            this.datLetrehoz.KeyDown += new System.Windows.Forms.KeyEventHandler(this.datLetrehoz_KeyDown);
            this.datLetrehoz.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.datLetrehoz_KeyPress);
            this.datLetrehoz.KeyUp += new System.Windows.Forms.KeyEventHandler(this.datLetrehoz_KeyUp);
            this.datLetrehoz.Leave += new System.EventHandler(this.datLetrehoz_Leave);
            // 
            // datErteknap
            // 
            this.datErteknap.Location = new System.Drawing.Point(32, 310);
            this.datErteknap.Name = "datErteknap";
            this.datErteknap.ReadOnly = true;
            this.datErteknap.Size = new System.Drawing.Size(114, 23);
            this.datErteknap.TabIndex = 1;
            this.datErteknap.TextChanged += new System.EventHandler(this.datLetrehoz_TextChanged);
            this.datErteknap.KeyDown += new System.Windows.Forms.KeyEventHandler(this.datErteknap_KeyDown);
            this.datErteknap.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.datErteknap_KeyPress);
            this.datErteknap.KeyUp += new System.Windows.Forms.KeyEventHandler(this.datErteknap_KeyUp);
            this.datErteknap.Leave += new System.EventHandler(this.datErteknap_Leave);
            // 
            // BankiKivonatok
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.ClientSize = new System.Drawing.Size(848, 397);
            this.Controls.Add(this.datLetrehoz);
            this.Controls.Add(this.datErteknap);
            this.Controls.Add(this.txKivonatSzama);
            this.Controls.Add(this.cbIrany);
            this.Controls.Add(this.txId);
            this.Controls.Add(this.txMegjegyzes);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.lbVege);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.dgvBankKiv);
            this.Name = "BankiKivonatok";
            this.Text = "Banki kivonatok";
            this.Resize += new System.EventHandler(this.BankiKivonatok_Resize);
            this.Controls.SetChildIndex(this.dgvBankKiv, 0);
            this.Controls.SetChildIndex(this.label2, 0);
            this.Controls.SetChildIndex(this.label1, 0);
            this.Controls.SetChildIndex(this.label4, 0);
            this.Controls.SetChildIndex(this.label5, 0);
            this.Controls.SetChildIndex(this.lbVege, 0);
            this.Controls.SetChildIndex(this.label7, 0);
            this.Controls.SetChildIndex(this.txMegjegyzes, 0);
            this.Controls.SetChildIndex(this.txId, 0);
            this.Controls.SetChildIndex(this.cbIrany, 0);
            this.Controls.SetChildIndex(this.txKivonatSzama, 0);
            this.Controls.SetChildIndex(this.datErteknap, 0);
            this.Controls.SetChildIndex(this.datLetrehoz, 0);
            ((System.ComponentModel.ISupportInitialize)(this.dgvBankKiv)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.spBankikivonatokBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.vPFSDataSet)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txKivonatSzama)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DataGridView dgvBankKiv;
        private System.Windows.Forms.TextBox txId;
        private System.Windows.Forms.TextBox txMegjegyzes;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label lbVege;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cbIrany;
        private VPFSDataSet vPFSDataSet;
        private System.Windows.Forms.BindingSource spBankikivonatokBindingSource;
        private VPFSDataSetTableAdapters.spBanki_kivonatokTableAdapter spBanki_kivonatokTableAdapter;
        private System.Windows.Forms.NumericUpDown txKivonatSzama;
        private System.Windows.Forms.TextBox datLetrehoz;
        private System.Windows.Forms.TextBox datErteknap;
    }
}
