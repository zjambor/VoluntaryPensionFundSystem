namespace VoluntaryPensionFundSystem
{
    partial class Idoszakok
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
            this.Adatcimke = new System.Windows.Forms.Label();
            this.dgwIdoszakok = new System.Windows.Forms.DataGridView();
            this.idkidDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.evhoDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.kezdeteDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.vegeDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ervenyesDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.megjegyzesDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.rogzitneveDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.rogzitdatumDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.modositneveDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.modositdatumDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.spIdoszakokBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.vPFSDataSet = new VoluntaryPensionFundSystem.VPFSDataSet();
            this.spIdoszakokTableAdapter = new VoluntaryPensionFundSystem.VPFSDataSetTableAdapters.spIdoszakokTableAdapter();
            ((System.ComponentModel.ISupportInitialize)(this.dgwIdoszakok)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.spIdoszakokBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.vPFSDataSet)).BeginInit();
            this.SuspendLayout();
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label2.Location = new System.Drawing.Point(444, 70);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(121, 29);
            this.label2.TabIndex = 95;
            this.label2.Text = "Időszakok";
            // 
            // Adatcimke
            // 
            this.Adatcimke.AutoSize = true;
            this.Adatcimke.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.Adatcimke.Location = new System.Drawing.Point(32, 94);
            this.Adatcimke.Name = "Adatcimke";
            this.Adatcimke.Size = new System.Drawing.Size(51, 20);
            this.Adatcimke.TabIndex = 94;
            this.Adatcimke.Text = "label1";
            // 
            // dgwIdoszakok
            // 
            this.dgwIdoszakok.AutoGenerateColumns = false;
            this.dgwIdoszakok.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgwIdoszakok.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.idkidDataGridViewTextBoxColumn,
            this.evhoDataGridViewTextBoxColumn,
            this.kezdeteDataGridViewTextBoxColumn,
            this.vegeDataGridViewTextBoxColumn,
            this.ervenyesDataGridViewTextBoxColumn,
            this.megjegyzesDataGridViewTextBoxColumn,
            this.rogzitneveDataGridViewTextBoxColumn,
            this.rogzitdatumDataGridViewTextBoxColumn,
            this.modositneveDataGridViewTextBoxColumn,
            this.modositdatumDataGridViewTextBoxColumn});
            this.dgwIdoszakok.DataSource = this.spIdoszakokBindingSource;
            this.dgwIdoszakok.Location = new System.Drawing.Point(35, 131);
            this.dgwIdoszakok.MultiSelect = false;
            this.dgwIdoszakok.Name = "dgwIdoszakok";
            this.dgwIdoszakok.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.dgwIdoszakok.ShowEditingIcon = false;
            this.dgwIdoszakok.Size = new System.Drawing.Size(941, 364);
            this.dgwIdoszakok.TabIndex = 93;
            this.dgwIdoszakok.CellEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgwIdoszakok_CellEnter);
            this.dgwIdoszakok.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dgwIdoszakok_CellFormatting);
            this.dgwIdoszakok.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.dgwIdoszakok_DataError);
            this.dgwIdoszakok.EditingControlShowing += new System.Windows.Forms.DataGridViewEditingControlShowingEventHandler(this.dataGridView_EditingControlShowing);
            this.dgwIdoszakok.SelectionChanged += new System.EventHandler(this.dgwIdoszakok_SelectionChanged);
            this.dgwIdoszakok.MouseLeave += new System.EventHandler(this.dgwIdoszakok_MouseLeave);
            this.dgwIdoszakok.MouseMove += new System.Windows.Forms.MouseEventHandler(this.dgwIdoszakok_MouseMove);
            // 
            // idkidDataGridViewTextBoxColumn
            // 
            this.idkidDataGridViewTextBoxColumn.DataPropertyName = "idk_id";
            this.idkidDataGridViewTextBoxColumn.HeaderText = "idk_id";
            this.idkidDataGridViewTextBoxColumn.Name = "idkidDataGridViewTextBoxColumn";
            this.idkidDataGridViewTextBoxColumn.ReadOnly = true;
            this.idkidDataGridViewTextBoxColumn.Visible = false;
            // 
            // evhoDataGridViewTextBoxColumn
            // 
            this.evhoDataGridViewTextBoxColumn.DataPropertyName = "ev_ho";
            this.evhoDataGridViewTextBoxColumn.HeaderText = "Von. időszak";
            this.evhoDataGridViewTextBoxColumn.MaxInputLength = 6;
            this.evhoDataGridViewTextBoxColumn.Name = "evhoDataGridViewTextBoxColumn";
            // 
            // kezdeteDataGridViewTextBoxColumn
            // 
            this.kezdeteDataGridViewTextBoxColumn.DataPropertyName = "kezdete";
            this.kezdeteDataGridViewTextBoxColumn.HeaderText = "Kezdete";
            this.kezdeteDataGridViewTextBoxColumn.Name = "kezdeteDataGridViewTextBoxColumn";
            this.kezdeteDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // vegeDataGridViewTextBoxColumn
            // 
            this.vegeDataGridViewTextBoxColumn.DataPropertyName = "vege";
            this.vegeDataGridViewTextBoxColumn.HeaderText = "Vége";
            this.vegeDataGridViewTextBoxColumn.Name = "vegeDataGridViewTextBoxColumn";
            this.vegeDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // ervenyesDataGridViewTextBoxColumn
            // 
            this.ervenyesDataGridViewTextBoxColumn.DataPropertyName = "ervenyes";
            this.ervenyesDataGridViewTextBoxColumn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ervenyesDataGridViewTextBoxColumn.HeaderText = "Érvényes";
            this.ervenyesDataGridViewTextBoxColumn.Items.AddRange(new object[] {
            "I",
            "N"});
            this.ervenyesDataGridViewTextBoxColumn.Name = "ervenyesDataGridViewTextBoxColumn";
            this.ervenyesDataGridViewTextBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.ervenyesDataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.ervenyesDataGridViewTextBoxColumn.Width = 80;
            // 
            // megjegyzesDataGridViewTextBoxColumn
            // 
            this.megjegyzesDataGridViewTextBoxColumn.DataPropertyName = "megjegyzes";
            this.megjegyzesDataGridViewTextBoxColumn.HeaderText = "Megjegyzés";
            this.megjegyzesDataGridViewTextBoxColumn.Name = "megjegyzesDataGridViewTextBoxColumn";
            this.megjegyzesDataGridViewTextBoxColumn.Width = 350;
            // 
            // rogzitneveDataGridViewTextBoxColumn
            // 
            this.rogzitneveDataGridViewTextBoxColumn.DataPropertyName = "rogzit_neve";
            this.rogzitneveDataGridViewTextBoxColumn.HeaderText = "rogzit_neve";
            this.rogzitneveDataGridViewTextBoxColumn.Name = "rogzitneveDataGridViewTextBoxColumn";
            this.rogzitneveDataGridViewTextBoxColumn.Visible = false;
            // 
            // rogzitdatumDataGridViewTextBoxColumn
            // 
            this.rogzitdatumDataGridViewTextBoxColumn.DataPropertyName = "rogzit_datum";
            this.rogzitdatumDataGridViewTextBoxColumn.HeaderText = "rogzit_datum";
            this.rogzitdatumDataGridViewTextBoxColumn.Name = "rogzitdatumDataGridViewTextBoxColumn";
            this.rogzitdatumDataGridViewTextBoxColumn.Visible = false;
            // 
            // modositneveDataGridViewTextBoxColumn
            // 
            this.modositneveDataGridViewTextBoxColumn.DataPropertyName = "modosit_neve";
            this.modositneveDataGridViewTextBoxColumn.HeaderText = "modosit_neve";
            this.modositneveDataGridViewTextBoxColumn.Name = "modositneveDataGridViewTextBoxColumn";
            this.modositneveDataGridViewTextBoxColumn.Visible = false;
            // 
            // modositdatumDataGridViewTextBoxColumn
            // 
            this.modositdatumDataGridViewTextBoxColumn.DataPropertyName = "modosit_datum";
            this.modositdatumDataGridViewTextBoxColumn.HeaderText = "modosit_datum";
            this.modositdatumDataGridViewTextBoxColumn.Name = "modositdatumDataGridViewTextBoxColumn";
            this.modositdatumDataGridViewTextBoxColumn.Visible = false;
            // 
            // spIdoszakokBindingSource
            // 
            this.spIdoszakokBindingSource.DataMember = "spIdoszakok";
            this.spIdoszakokBindingSource.DataSource = this.vPFSDataSet;
            // 
            // vPFSDataSet
            // 
            this.vPFSDataSet.DataSetName = "VPFSDataSet";
            this.vPFSDataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // spIdoszakokTableAdapter
            // 
            this.spIdoszakokTableAdapter.ClearBeforeFill = true;
            // 
            // Idoszakok
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.ClientSize = new System.Drawing.Size(1008, 522);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.Adatcimke);
            this.Controls.Add(this.dgwIdoszakok);
            this.Name = "Idoszakok";
            this.Text = "Időszakok";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Bankszamlak_FormClosed);
            this.Load += new System.EventHandler(this.Idoszakok_Load);
            this.Resize += new System.EventHandler(this.Idoszakok_Resize);
            this.Controls.SetChildIndex(this.dgwIdoszakok, 0);
            this.Controls.SetChildIndex(this.Adatcimke, 0);
            this.Controls.SetChildIndex(this.label2, 0);
            ((System.ComponentModel.ISupportInitialize)(this.dgwIdoszakok)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.spIdoszakokBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.vPFSDataSet)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label Adatcimke;
        private System.Windows.Forms.DataGridView dgwIdoszakok;
        private VPFSDataSet vPFSDataSet;
        private System.Windows.Forms.BindingSource spIdoszakokBindingSource;
        private VPFSDataSetTableAdapters.spIdoszakokTableAdapter spIdoszakokTableAdapter;
        private System.Windows.Forms.DataGridViewTextBoxColumn idkidDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn evhoDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn kezdeteDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn vegeDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewComboBoxColumn ervenyesDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn megjegyzesDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn rogzitneveDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn rogzitdatumDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn modositneveDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn modositdatumDataGridViewTextBoxColumn;
    }
}
