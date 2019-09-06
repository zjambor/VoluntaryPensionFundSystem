namespace VoluntaryPensionFundSystem
{
    partial class Portfoliok
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
            this.dgvPortfoliok = new System.Windows.Forms.DataGridView();
            this.bfkidDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tipusDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.nevDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.leirasDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ervkezdeteDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ervvegeDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.megjegyzesDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.rogzitneveDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.rogzitdatumDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.modositneveDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.modositdatumDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.spPortfoliokBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.vPFSDataSet = new VoluntaryPensionFundSystem.VPFSDataSet();
            this.spPortfoliokTableAdapter = new VoluntaryPensionFundSystem.VPFSDataSetTableAdapters.spPortfoliokTableAdapter();
            this.label1 = new System.Windows.Forms.Label();
            this.txTipus = new System.Windows.Forms.TextBox();
            this.txLeiras = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txNev = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.datKezdete = new System.Windows.Forms.DateTimePicker();
            this.datVege = new System.Windows.Forms.DateTimePicker();
            this.label5 = new System.Windows.Forms.Label();
            this.lbVege = new System.Windows.Forms.Label();
            this.txMegjegyzes = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.cbErv = new System.Windows.Forms.CheckBox();
            this.txId = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPortfoliok)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.spPortfoliokBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.vPFSDataSet)).BeginInit();
            this.SuspendLayout();
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label2.Location = new System.Drawing.Point(447, 69);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(115, 29);
            this.label2.TabIndex = 101;
            this.label2.Text = "Portfóliók";
            // 
            // dgvPortfoliok
            // 
            this.dgvPortfoliok.AllowUserToAddRows = false;
            this.dgvPortfoliok.AllowUserToDeleteRows = false;
            this.dgvPortfoliok.AutoGenerateColumns = false;
            this.dgvPortfoliok.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dgvPortfoliok.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvPortfoliok.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.bfkidDataGridViewTextBoxColumn,
            this.tipusDataGridViewTextBoxColumn,
            this.nevDataGridViewTextBoxColumn,
            this.leirasDataGridViewTextBoxColumn,
            this.ervkezdeteDataGridViewTextBoxColumn,
            this.ervvegeDataGridViewTextBoxColumn,
            this.megjegyzesDataGridViewTextBoxColumn,
            this.rogzitneveDataGridViewTextBoxColumn,
            this.rogzitdatumDataGridViewTextBoxColumn,
            this.modositneveDataGridViewTextBoxColumn,
            this.modositdatumDataGridViewTextBoxColumn});
            this.dgvPortfoliok.DataSource = this.spPortfoliokBindingSource;
            this.dgvPortfoliok.Location = new System.Drawing.Point(34, 119);
            this.dgvPortfoliok.MultiSelect = false;
            this.dgvPortfoliok.Name = "dgvPortfoliok";
            this.dgvPortfoliok.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.dgvPortfoliok.ShowEditingIcon = false;
            this.dgvPortfoliok.Size = new System.Drawing.Size(941, 164);
            this.dgvPortfoliok.TabIndex = 99;
            this.dgvPortfoliok.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dgvPortfoliok_CellFormatting);
            this.dgvPortfoliok.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.dgvPortfoliok_DataError);
            this.dgvPortfoliok.SelectionChanged += new System.EventHandler(this.dgvPortfoliok_SelectionChanged);
            this.dgvPortfoliok.MouseMove += new System.Windows.Forms.MouseEventHandler(this.dgvPortfoliok_MouseMove);
            // 
            // bfkidDataGridViewTextBoxColumn
            // 
            this.bfkidDataGridViewTextBoxColumn.DataPropertyName = "bfk_id";
            this.bfkidDataGridViewTextBoxColumn.HeaderText = "bfk_id";
            this.bfkidDataGridViewTextBoxColumn.Name = "bfkidDataGridViewTextBoxColumn";
            this.bfkidDataGridViewTextBoxColumn.ReadOnly = true;
            this.bfkidDataGridViewTextBoxColumn.Width = 71;
            // 
            // tipusDataGridViewTextBoxColumn
            // 
            this.tipusDataGridViewTextBoxColumn.DataPropertyName = "tipus";
            this.tipusDataGridViewTextBoxColumn.HeaderText = "Típus";
            this.tipusDataGridViewTextBoxColumn.Name = "tipusDataGridViewTextBoxColumn";
            this.tipusDataGridViewTextBoxColumn.ReadOnly = true;
            this.tipusDataGridViewTextBoxColumn.Width = 68;
            // 
            // nevDataGridViewTextBoxColumn
            // 
            this.nevDataGridViewTextBoxColumn.DataPropertyName = "nev";
            this.nevDataGridViewTextBoxColumn.HeaderText = "Név";
            this.nevDataGridViewTextBoxColumn.Name = "nevDataGridViewTextBoxColumn";
            this.nevDataGridViewTextBoxColumn.ReadOnly = true;
            this.nevDataGridViewTextBoxColumn.Width = 58;
            // 
            // leirasDataGridViewTextBoxColumn
            // 
            this.leirasDataGridViewTextBoxColumn.DataPropertyName = "leiras";
            this.leirasDataGridViewTextBoxColumn.HeaderText = "Leírás";
            this.leirasDataGridViewTextBoxColumn.Name = "leirasDataGridViewTextBoxColumn";
            this.leirasDataGridViewTextBoxColumn.ReadOnly = true;
            this.leirasDataGridViewTextBoxColumn.Width = 72;
            // 
            // ervkezdeteDataGridViewTextBoxColumn
            // 
            this.ervkezdeteDataGridViewTextBoxColumn.DataPropertyName = "erv_kezdete";
            this.ervkezdeteDataGridViewTextBoxColumn.HeaderText = "Érv.kezdete";
            this.ervkezdeteDataGridViewTextBoxColumn.Name = "ervkezdeteDataGridViewTextBoxColumn";
            this.ervkezdeteDataGridViewTextBoxColumn.ReadOnly = true;
            this.ervkezdeteDataGridViewTextBoxColumn.Width = 108;
            // 
            // ervvegeDataGridViewTextBoxColumn
            // 
            this.ervvegeDataGridViewTextBoxColumn.DataPropertyName = "erv_vege";
            this.ervvegeDataGridViewTextBoxColumn.HeaderText = "Érv.vége";
            this.ervvegeDataGridViewTextBoxColumn.Name = "ervvegeDataGridViewTextBoxColumn";
            this.ervvegeDataGridViewTextBoxColumn.ReadOnly = true;
            this.ervvegeDataGridViewTextBoxColumn.Width = 89;
            // 
            // megjegyzesDataGridViewTextBoxColumn
            // 
            this.megjegyzesDataGridViewTextBoxColumn.DataPropertyName = "megjegyzes";
            this.megjegyzesDataGridViewTextBoxColumn.HeaderText = "Megjegyzés";
            this.megjegyzesDataGridViewTextBoxColumn.Name = "megjegyzesDataGridViewTextBoxColumn";
            this.megjegyzesDataGridViewTextBoxColumn.ReadOnly = true;
            this.megjegyzesDataGridViewTextBoxColumn.Width = 108;
            // 
            // rogzitneveDataGridViewTextBoxColumn
            // 
            this.rogzitneveDataGridViewTextBoxColumn.DataPropertyName = "rogzit_neve";
            this.rogzitneveDataGridViewTextBoxColumn.HeaderText = "rogzit_neve";
            this.rogzitneveDataGridViewTextBoxColumn.Name = "rogzitneveDataGridViewTextBoxColumn";
            this.rogzitneveDataGridViewTextBoxColumn.Visible = false;
            this.rogzitneveDataGridViewTextBoxColumn.Width = 107;
            // 
            // rogzitdatumDataGridViewTextBoxColumn
            // 
            this.rogzitdatumDataGridViewTextBoxColumn.DataPropertyName = "rogzit_datum";
            this.rogzitdatumDataGridViewTextBoxColumn.HeaderText = "rogzit_datum";
            this.rogzitdatumDataGridViewTextBoxColumn.Name = "rogzitdatumDataGridViewTextBoxColumn";
            this.rogzitdatumDataGridViewTextBoxColumn.Visible = false;
            this.rogzitdatumDataGridViewTextBoxColumn.Width = 115;
            // 
            // modositneveDataGridViewTextBoxColumn
            // 
            this.modositneveDataGridViewTextBoxColumn.DataPropertyName = "modosit_neve";
            this.modositneveDataGridViewTextBoxColumn.HeaderText = "modosit_neve";
            this.modositneveDataGridViewTextBoxColumn.Name = "modositneveDataGridViewTextBoxColumn";
            this.modositneveDataGridViewTextBoxColumn.Visible = false;
            this.modositneveDataGridViewTextBoxColumn.Width = 121;
            // 
            // modositdatumDataGridViewTextBoxColumn
            // 
            this.modositdatumDataGridViewTextBoxColumn.DataPropertyName = "modosit_datum";
            this.modositdatumDataGridViewTextBoxColumn.HeaderText = "modosit_datum";
            this.modositdatumDataGridViewTextBoxColumn.Name = "modositdatumDataGridViewTextBoxColumn";
            this.modositdatumDataGridViewTextBoxColumn.Visible = false;
            this.modositdatumDataGridViewTextBoxColumn.Width = 129;
            // 
            // spPortfoliokBindingSource
            // 
            this.spPortfoliokBindingSource.DataMember = "spPortfoliok";
            this.spPortfoliokBindingSource.DataSource = this.vPFSDataSet;
            // 
            // vPFSDataSet
            // 
            this.vPFSDataSet.DataSetName = "VPFSDataSet";
            this.vPFSDataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // spPortfoliokTableAdapter
            // 
            this.spPortfoliokTableAdapter.ClearBeforeFill = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(33, 286);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(43, 17);
            this.label1.TabIndex = 102;
            this.label1.Text = "Típus";
            // 
            // txTipus
            // 
            this.txTipus.Location = new System.Drawing.Point(34, 306);
            this.txTipus.MaxLength = 1;
            this.txTipus.Name = "txTipus";
            this.txTipus.Size = new System.Drawing.Size(46, 23);
            this.txTipus.TabIndex = 0;
            this.txTipus.TextChanged += new System.EventHandler(this.txTipus_TextChanged);
            this.txTipus.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txTipus_KeyPress);
            // 
            // txLeiras
            // 
            this.txLeiras.Location = new System.Drawing.Point(34, 351);
            this.txLeiras.MaxLength = 255;
            this.txLeiras.Name = "txLeiras";
            this.txLeiras.Size = new System.Drawing.Size(612, 23);
            this.txLeiras.TabIndex = 3;
            this.txLeiras.TextChanged += new System.EventHandler(this.txLeiras_TextChanged);
            this.txLeiras.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txLeiras_KeyPress);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(33, 331);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(47, 17);
            this.label3.TabIndex = 104;
            this.label3.Text = "Leírás";
            // 
            // txNev
            // 
            this.txNev.Location = new System.Drawing.Point(86, 306);
            this.txNev.MaxLength = 80;
            this.txNev.Name = "txNev";
            this.txNev.Size = new System.Drawing.Size(560, 23);
            this.txNev.TabIndex = 1;
            this.txNev.TextChanged += new System.EventHandler(this.txNev_TextChanged);
            this.txNev.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txNev_KeyPress);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(85, 286);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(33, 17);
            this.label4.TabIndex = 106;
            this.label4.Text = "Név";
            // 
            // datKezdete
            // 
            this.datKezdete.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.datKezdete.Location = new System.Drawing.Point(672, 306);
            this.datKezdete.Name = "datKezdete";
            this.datKezdete.Size = new System.Drawing.Size(108, 23);
            this.datKezdete.TabIndex = 2;
            this.datKezdete.ValueChanged += new System.EventHandler(this.datKezdete_ValueChanged);
            // 
            // datVege
            // 
            this.datVege.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.datVege.Location = new System.Drawing.Point(672, 351);
            this.datVege.Name = "datVege";
            this.datVege.Size = new System.Drawing.Size(108, 23);
            this.datVege.TabIndex = 109;
            this.datVege.ValueChanged += new System.EventHandler(this.datVege_ValueChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(673, 286);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(87, 17);
            this.label5.TabIndex = 110;
            this.label5.Text = "Érv. kezdete";
            // 
            // lbVege
            // 
            this.lbVege.AutoSize = true;
            this.lbVege.Location = new System.Drawing.Point(669, 331);
            this.lbVege.Name = "lbVege";
            this.lbVege.Size = new System.Drawing.Size(68, 17);
            this.lbVege.TabIndex = 111;
            this.lbVege.Text = "Érv. vége";
            // 
            // txMegjegyzes
            // 
            this.txMegjegyzes.Location = new System.Drawing.Point(34, 397);
            this.txMegjegyzes.MaxLength = 255;
            this.txMegjegyzes.Name = "txMegjegyzes";
            this.txMegjegyzes.Size = new System.Drawing.Size(746, 23);
            this.txMegjegyzes.TabIndex = 4;
            this.txMegjegyzes.TextChanged += new System.EventHandler(this.txMegjegyzes_TextChanged);
            this.txMegjegyzes.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txMegjegyzes_KeyPress);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(33, 377);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(83, 17);
            this.label7.TabIndex = 112;
            this.label7.Text = "Megjegyzés";
            // 
            // cbErv
            // 
            this.cbErv.AutoSize = true;
            this.cbErv.Checked = true;
            this.cbErv.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbErv.Location = new System.Drawing.Point(804, 308);
            this.cbErv.Name = "cbErv";
            this.cbErv.Size = new System.Drawing.Size(86, 21);
            this.cbErv.TabIndex = 114;
            this.cbErv.Text = "Érvényes";
            this.cbErv.UseVisualStyleBackColor = true;
            this.cbErv.CheckedChanged += new System.EventHandler(this.cbErv_CheckedChanged);
            this.cbErv.Click += new System.EventHandler(this.cbErv_Click);
            // 
            // txId
            // 
            this.txId.Location = new System.Drawing.Point(804, 397);
            this.txId.MaxLength = 1;
            this.txId.Name = "txId";
            this.txId.Size = new System.Drawing.Size(46, 23);
            this.txId.TabIndex = 115;
            this.txId.Visible = false;
            // 
            // Portfoliok
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1008, 466);
            this.Controls.Add(this.txId);
            this.Controls.Add(this.cbErv);
            this.Controls.Add(this.txMegjegyzes);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.lbVege);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.datVege);
            this.Controls.Add(this.datKezdete);
            this.Controls.Add(this.txNev);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txLeiras);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txTipus);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.dgvPortfoliok);
            this.Name = "Portfoliok";
            this.Text = "Portfóliók";
            this.Load += new System.EventHandler(this.Portfoliok_Load);
            this.Resize += new System.EventHandler(this.Portfoliok_Resize);
            this.Controls.SetChildIndex(this.dgvPortfoliok, 0);
            this.Controls.SetChildIndex(this.label2, 0);
            this.Controls.SetChildIndex(this.label1, 0);
            this.Controls.SetChildIndex(this.txTipus, 0);
            this.Controls.SetChildIndex(this.label3, 0);
            this.Controls.SetChildIndex(this.txLeiras, 0);
            this.Controls.SetChildIndex(this.label4, 0);
            this.Controls.SetChildIndex(this.txNev, 0);
            this.Controls.SetChildIndex(this.datKezdete, 0);
            this.Controls.SetChildIndex(this.datVege, 0);
            this.Controls.SetChildIndex(this.label5, 0);
            this.Controls.SetChildIndex(this.lbVege, 0);
            this.Controls.SetChildIndex(this.label7, 0);
            this.Controls.SetChildIndex(this.txMegjegyzes, 0);
            this.Controls.SetChildIndex(this.cbErv, 0);
            this.Controls.SetChildIndex(this.txId, 0);
            ((System.ComponentModel.ISupportInitialize)(this.dgvPortfoliok)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.spPortfoliokBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.vPFSDataSet)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DataGridView dgvPortfoliok;
        private VPFSDataSet vPFSDataSet;
        private System.Windows.Forms.BindingSource spPortfoliokBindingSource;
        private VPFSDataSetTableAdapters.spPortfoliokTableAdapter spPortfoliokTableAdapter;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txTipus;
        private System.Windows.Forms.TextBox txLeiras;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txNev;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.DateTimePicker datKezdete;
        private System.Windows.Forms.DateTimePicker datVege;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label lbVege;
        private System.Windows.Forms.TextBox txMegjegyzes;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.CheckBox cbErv;
        private System.Windows.Forms.DataGridViewTextBoxColumn bfkidDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn tipusDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn nevDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn leirasDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn ervkezdeteDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn ervvegeDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn megjegyzesDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn rogzitneveDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn rogzitdatumDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn modositneveDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn modositdatumDataGridViewTextBoxColumn;
        private System.Windows.Forms.TextBox txId;
    }
}