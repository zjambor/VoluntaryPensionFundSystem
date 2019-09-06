namespace VoluntaryPensionFundSystem
{
    partial class BefizetesekRogz
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
            this.txKivonatSzama = new System.Windows.Forms.NumericUpDown();
            this.cbIrany = new System.Windows.Forms.ComboBox();
            this.txMegjegyzes = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.lbVege = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.dgvBankKiv = new System.Windows.Forms.DataGridView();
            this.datErteknap = new System.Windows.Forms.TextBox();
            this.datLetrehoz = new System.Windows.Forms.TextBox();
            this.txSorszam = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.txOsszeg = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.txSzamlaszam = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.txNev = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.txKozlemeny = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.txKonyveles = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.txStatus = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.txStorno = new System.Windows.Forms.TextBox();
            this.bSaveAndNext = new System.Windows.Forms.Button();
            this.txId = new System.Windows.Forms.TextBox();
            this.bUjkivonat = new System.Windows.Forms.Button();
            this.txBtlId = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.txKivonatSzama)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvBankKiv)).BeginInit();
            this.SuspendLayout();
            // 
            // txKivonatSzama
            // 
            this.txKivonatSzama.Enabled = false;
            this.txKivonatSzama.Location = new System.Drawing.Point(149, 137);
            this.txKivonatSzama.Name = "txKivonatSzama";
            this.txKivonatSzama.Size = new System.Drawing.Size(97, 23);
            this.txKivonatSzama.TabIndex = 1;
            this.txKivonatSzama.Enter += new System.EventHandler(this.txKivonatSzama_Enter);
            // 
            // cbIrany
            // 
            this.cbIrany.Enabled = false;
            this.cbIrany.FormattingEnabled = true;
            this.cbIrany.Location = new System.Drawing.Point(258, 136);
            this.cbIrany.Name = "cbIrany";
            this.cbIrany.Size = new System.Drawing.Size(65, 24);
            this.cbIrany.TabIndex = 2;
            // 
            // txMegjegyzes
            // 
            this.txMegjegyzes.Location = new System.Drawing.Point(23, 479);
            this.txMegjegyzes.MaxLength = 255;
            this.txMegjegyzes.Name = "txMegjegyzes";
            this.txMegjegyzes.Size = new System.Drawing.Size(667, 23);
            this.txMegjegyzes.TabIndex = 8;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(22, 459);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(83, 17);
            this.label7.TabIndex = 139;
            this.label7.Text = "Megjegyzés";
            // 
            // lbVege
            // 
            this.lbVege.AutoSize = true;
            this.lbVege.Location = new System.Drawing.Point(334, 117);
            this.lbVege.Name = "lbVege";
            this.lbVege.Size = new System.Drawing.Size(130, 17);
            this.lbVege.TabIndex = 138;
            this.lbVege.Text = "Létrehozás dátuma";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(24, 117);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(65, 17);
            this.label5.TabIndex = 137;
            this.label5.Text = "Értéknap";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(257, 117);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(39, 17);
            this.label4.TabIndex = 136;
            this.label4.Text = "Irány";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(146, 117);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(100, 17);
            this.label1.TabIndex = 135;
            this.label1.Text = "Kivonat száma";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label2.Location = new System.Drawing.Point(361, 72);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(189, 29);
            this.label2.TabIndex = 134;
            this.label2.Text = "Pénzügyi tételek";
            // 
            // dgvBankKiv
            // 
            this.dgvBankKiv.AllowUserToAddRows = false;
            this.dgvBankKiv.AllowUserToDeleteRows = false;
            this.dgvBankKiv.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvBankKiv.Location = new System.Drawing.Point(23, 193);
            this.dgvBankKiv.MultiSelect = false;
            this.dgvBankKiv.Name = "dgvBankKiv";
            this.dgvBankKiv.ReadOnly = true;
            this.dgvBankKiv.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.dgvBankKiv.ShowEditingIcon = false;
            this.dgvBankKiv.Size = new System.Drawing.Size(858, 164);
            this.dgvBankKiv.TabIndex = 128;
            this.dgvBankKiv.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dgvBankKiv_CellFormatting);
            this.dgvBankKiv.SelectionChanged += new System.EventHandler(this.dgvBankKiv_SelectionChanged);
            this.dgvBankKiv.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dgvBankKiv_KeyDown);
            this.dgvBankKiv.MouseMove += new System.Windows.Forms.MouseEventHandler(this.dgvBankKiv_MouseMove);
            // 
            // datErteknap
            // 
            this.datErteknap.Location = new System.Drawing.Point(23, 137);
            this.datErteknap.Name = "datErteknap";
            this.datErteknap.ReadOnly = true;
            this.datErteknap.Size = new System.Drawing.Size(114, 23);
            this.datErteknap.TabIndex = 0;
            this.datErteknap.KeyDown += new System.Windows.Forms.KeyEventHandler(this.datErteknap_KeyDown);
            this.datErteknap.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.datErteknap_KeyPress);
            this.datErteknap.KeyUp += new System.Windows.Forms.KeyEventHandler(this.datErteknap_KeyUp);
            this.datErteknap.Leave += new System.EventHandler(this.datErteknap_Leave);
            // 
            // datLetrehoz
            // 
            this.datLetrehoz.Location = new System.Drawing.Point(337, 136);
            this.datLetrehoz.Name = "datLetrehoz";
            this.datLetrehoz.ReadOnly = true;
            this.datLetrehoz.Size = new System.Drawing.Size(127, 23);
            this.datLetrehoz.TabIndex = 3;
            this.datLetrehoz.KeyDown += new System.Windows.Forms.KeyEventHandler(this.datLetrehoz_KeyDown);
            this.datLetrehoz.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.datLetrehoz_KeyPress);
            this.datLetrehoz.KeyUp += new System.Windows.Forms.KeyEventHandler(this.datLetrehoz_KeyUp);
            this.datLetrehoz.Leave += new System.EventHandler(this.datLetrehoz_Leave);
            // 
            // txSorszam
            // 
            this.txSorszam.Location = new System.Drawing.Point(23, 394);
            this.txSorszam.Name = "txSorszam";
            this.txSorszam.ReadOnly = true;
            this.txSorszam.Size = new System.Drawing.Size(60, 23);
            this.txSorszam.TabIndex = 142;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(20, 374);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(63, 17);
            this.label3.TabIndex = 143;
            this.label3.Text = "Sorszám";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(86, 374);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(56, 17);
            this.label6.TabIndex = 145;
            this.label6.Text = "Összeg";
            // 
            // txOsszeg
            // 
            this.txOsszeg.Location = new System.Drawing.Point(89, 394);
            this.txOsszeg.MaxLength = 9;
            this.txOsszeg.Name = "txOsszeg";
            this.txOsszeg.Size = new System.Drawing.Size(104, 23);
            this.txOsszeg.TabIndex = 4;
            this.txOsszeg.TextChanged += new System.EventHandler(this.txOsszeg_TextChanged);
            this.txOsszeg.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txOsszeg_KeyPress);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(196, 374);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(87, 17);
            this.label8.TabIndex = 147;
            this.label8.Text = "Számlaszám";
            // 
            // txSzamlaszam
            // 
            this.txSzamlaszam.Location = new System.Drawing.Point(199, 394);
            this.txSzamlaszam.MaxLength = 24;
            this.txSzamlaszam.Name = "txSzamlaszam";
            this.txSzamlaszam.Size = new System.Drawing.Size(250, 23);
            this.txSzamlaszam.TabIndex = 5;
            this.txSzamlaszam.TextChanged += new System.EventHandler(this.txSzamlaszam_TextChanged);
            this.txSzamlaszam.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txSzamlaszam_KeyPress);
            this.txSzamlaszam.Leave += new System.EventHandler(this.txSzamlaszam_Leave);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(455, 374);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(33, 17);
            this.label9.TabIndex = 149;
            this.label9.Text = "Név";
            // 
            // txNev
            // 
            this.txNev.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txNev.Location = new System.Drawing.Point(458, 394);
            this.txNev.MaxLength = 100;
            this.txNev.Name = "txNev";
            this.txNev.Size = new System.Drawing.Size(423, 23);
            this.txNev.TabIndex = 6;
            this.txNev.TextChanged += new System.EventHandler(this.txNev_TextChanged);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(20, 415);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(77, 17);
            this.label10.TabIndex = 151;
            this.label10.Text = "Közlemény";
            // 
            // txKozlemeny
            // 
            this.txKozlemeny.Location = new System.Drawing.Point(23, 435);
            this.txKozlemeny.MaxLength = 255;
            this.txKozlemeny.Name = "txKozlemeny";
            this.txKozlemeny.Size = new System.Drawing.Size(858, 23);
            this.txKozlemeny.TabIndex = 7;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(20, 507);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(124, 17);
            this.label11.TabIndex = 153;
            this.label11.Text = "Könyvelés dátuma";
            // 
            // txKonyveles
            // 
            this.txKonyveles.Location = new System.Drawing.Point(23, 527);
            this.txKonyveles.Name = "txKonyveles";
            this.txKonyveles.ReadOnly = true;
            this.txKonyveles.Size = new System.Drawing.Size(114, 23);
            this.txKonyveles.TabIndex = 10;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(140, 507);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(48, 17);
            this.label12.TabIndex = 155;
            this.label12.Text = "Státus";
            // 
            // txStatus
            // 
            this.txStatus.Location = new System.Drawing.Point(143, 527);
            this.txStatus.Name = "txStatus";
            this.txStatus.ReadOnly = true;
            this.txStatus.Size = new System.Drawing.Size(60, 23);
            this.txStatus.TabIndex = 11;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(206, 507);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(50, 17);
            this.label13.TabIndex = 157;
            this.label13.Text = "Storno";
            // 
            // txStorno
            // 
            this.txStorno.Location = new System.Drawing.Point(209, 527);
            this.txStorno.Name = "txStorno";
            this.txStorno.ReadOnly = true;
            this.txStorno.Size = new System.Drawing.Size(60, 23);
            this.txStorno.TabIndex = 12;
            // 
            // bSaveAndNext
            // 
            this.bSaveAndNext.AutoSize = true;
            this.bSaveAndNext.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.bSaveAndNext.Location = new System.Drawing.Point(706, 473);
            this.bSaveAndNext.Name = "bSaveAndNext";
            this.bSaveAndNext.Size = new System.Drawing.Size(153, 29);
            this.bSaveAndNext.TabIndex = 9;
            this.bSaveAndNext.Text = "Mentés és következő";
            this.bSaveAndNext.UseVisualStyleBackColor = true;
            this.bSaveAndNext.Click += new System.EventHandler(this.bSaveAndNext_Click);
            // 
            // txId
            // 
            this.txId.Location = new System.Drawing.Point(480, 136);
            this.txId.MaxLength = 1;
            this.txId.Name = "txId";
            this.txId.ReadOnly = true;
            this.txId.Size = new System.Drawing.Size(46, 23);
            this.txId.TabIndex = 158;
            this.txId.Visible = false;
            // 
            // bUjkivonat
            // 
            this.bUjkivonat.AutoSize = true;
            this.bUjkivonat.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.bUjkivonat.Location = new System.Drawing.Point(542, 133);
            this.bUjkivonat.Name = "bUjkivonat";
            this.bUjkivonat.Size = new System.Drawing.Size(82, 29);
            this.bUjkivonat.TabIndex = 159;
            this.bUjkivonat.Text = "Új kivonat";
            this.bUjkivonat.UseVisualStyleBackColor = true;
            this.bUjkivonat.Click += new System.EventHandler(this.bUjkivonat_Click);
            // 
            // txBtlId
            // 
            this.txBtlId.Location = new System.Drawing.Point(277, 527);
            this.txBtlId.MaxLength = 1;
            this.txBtlId.Name = "txBtlId";
            this.txBtlId.ReadOnly = true;
            this.txBtlId.Size = new System.Drawing.Size(46, 23);
            this.txBtlId.TabIndex = 160;
            this.txBtlId.Visible = false;
            // 
            // BefizetesekRogz
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.ClientSize = new System.Drawing.Size(911, 621);
            this.Controls.Add(this.txBtlId);
            this.Controls.Add(this.bUjkivonat);
            this.Controls.Add(this.txId);
            this.Controls.Add(this.bSaveAndNext);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.txStorno);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.txStatus);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.txKonyveles);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.txKozlemeny);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.txNev);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.txSzamlaszam);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.txOsszeg);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txSorszam);
            this.Controls.Add(this.datLetrehoz);
            this.Controls.Add(this.datErteknap);
            this.Controls.Add(this.txKivonatSzama);
            this.Controls.Add(this.cbIrany);
            this.Controls.Add(this.txMegjegyzes);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.lbVege);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.dgvBankKiv);
            this.Name = "BefizetesekRogz";
            this.Text = "Pénzügyi tételek";
            this.Load += new System.EventHandler(this.BefizetesekRogz_Load);
            this.Resize += new System.EventHandler(this.BefizetesekRogz_Resize);
            this.Controls.SetChildIndex(this.dgvBankKiv, 0);
            this.Controls.SetChildIndex(this.label2, 0);
            this.Controls.SetChildIndex(this.label1, 0);
            this.Controls.SetChildIndex(this.label4, 0);
            this.Controls.SetChildIndex(this.label5, 0);
            this.Controls.SetChildIndex(this.lbVege, 0);
            this.Controls.SetChildIndex(this.label7, 0);
            this.Controls.SetChildIndex(this.txMegjegyzes, 0);
            this.Controls.SetChildIndex(this.cbIrany, 0);
            this.Controls.SetChildIndex(this.txKivonatSzama, 0);
            this.Controls.SetChildIndex(this.datErteknap, 0);
            this.Controls.SetChildIndex(this.datLetrehoz, 0);
            this.Controls.SetChildIndex(this.txSorszam, 0);
            this.Controls.SetChildIndex(this.label3, 0);
            this.Controls.SetChildIndex(this.txOsszeg, 0);
            this.Controls.SetChildIndex(this.label6, 0);
            this.Controls.SetChildIndex(this.txSzamlaszam, 0);
            this.Controls.SetChildIndex(this.label8, 0);
            this.Controls.SetChildIndex(this.txNev, 0);
            this.Controls.SetChildIndex(this.label9, 0);
            this.Controls.SetChildIndex(this.txKozlemeny, 0);
            this.Controls.SetChildIndex(this.label10, 0);
            this.Controls.SetChildIndex(this.txKonyveles, 0);
            this.Controls.SetChildIndex(this.label11, 0);
            this.Controls.SetChildIndex(this.txStatus, 0);
            this.Controls.SetChildIndex(this.label12, 0);
            this.Controls.SetChildIndex(this.txStorno, 0);
            this.Controls.SetChildIndex(this.label13, 0);
            this.Controls.SetChildIndex(this.bSaveAndNext, 0);
            this.Controls.SetChildIndex(this.txId, 0);
            this.Controls.SetChildIndex(this.bUjkivonat, 0);
            this.Controls.SetChildIndex(this.txBtlId, 0);
            ((System.ComponentModel.ISupportInitialize)(this.txKivonatSzama)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvBankKiv)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.NumericUpDown txKivonatSzama;
        private System.Windows.Forms.ComboBox cbIrany;
        private System.Windows.Forms.TextBox txMegjegyzes;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label lbVege;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DataGridView dgvBankKiv;
        private System.Windows.Forms.TextBox datErteknap;
        private System.Windows.Forms.TextBox datLetrehoz;
        private System.Windows.Forms.TextBox txSorszam;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txOsszeg;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox txSzamlaszam;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox txNev;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox txKozlemeny;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox txKonyveles;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TextBox txStatus;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.TextBox txStorno;
        private System.Windows.Forms.Button bSaveAndNext;
        private System.Windows.Forms.TextBox txId;
        private System.Windows.Forms.Button bUjkivonat;
        private System.Windows.Forms.TextBox txBtlId;
    }
}
