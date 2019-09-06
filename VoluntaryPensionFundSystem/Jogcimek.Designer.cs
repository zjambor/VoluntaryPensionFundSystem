namespace VoluntaryPensionFundSystem
{
    partial class Jogcimek
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
            this.dgvJogcimek = new System.Windows.Forms.DataGridView();
            this.jcmidDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tipusDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.megnevezesDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ervenyesDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.spJogcimekBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.vPFSDataSet = new VoluntaryPensionFundSystem.VPFSDataSet();
            this.spJogcimekTableAdapter = new VoluntaryPensionFundSystem.VPFSDataSetTableAdapters.spJogcimekTableAdapter();
            ((System.ComponentModel.ISupportInitialize)(this.dgvJogcimek)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.spJogcimekBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.vPFSDataSet)).BeginInit();
            this.SuspendLayout();
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label2.Location = new System.Drawing.Point(345, 67);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(117, 29);
            this.label2.TabIndex = 109;
            this.label2.Text = "Jogcímek";
            // 
            // Adatcimke
            // 
            this.Adatcimke.AutoSize = true;
            this.Adatcimke.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.Adatcimke.Location = new System.Drawing.Point(35, 90);
            this.Adatcimke.Name = "Adatcimke";
            this.Adatcimke.Size = new System.Drawing.Size(51, 20);
            this.Adatcimke.TabIndex = 108;
            this.Adatcimke.Text = "label1";
            // 
            // dgvJogcimek
            // 
            this.dgvJogcimek.AllowUserToDeleteRows = false;
            this.dgvJogcimek.AutoGenerateColumns = false;
            this.dgvJogcimek.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dgvJogcimek.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvJogcimek.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.jcmidDataGridViewTextBoxColumn,
            this.tipusDataGridViewTextBoxColumn,
            this.megnevezesDataGridViewTextBoxColumn,
            this.ervenyesDataGridViewTextBoxColumn});
            this.dgvJogcimek.DataSource = this.spJogcimekBindingSource;
            this.dgvJogcimek.Location = new System.Drawing.Point(39, 124);
            this.dgvJogcimek.MultiSelect = false;
            this.dgvJogcimek.Name = "dgvJogcimek";
            this.dgvJogcimek.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.dgvJogcimek.ShowEditingIcon = false;
            this.dgvJogcimek.Size = new System.Drawing.Size(729, 284);
            this.dgvJogcimek.TabIndex = 107;
            this.dgvJogcimek.CellEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvJogcimek_CellEnter);
            this.dgvJogcimek.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dgvJogcimek_CellFormatting);
            this.dgvJogcimek.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.dgvJogcimek_DataError);
            this.dgvJogcimek.SelectionChanged += new System.EventHandler(this.dgvJogcimek_SelectionChanged);
            this.dgvJogcimek.MouseLeave += new System.EventHandler(this.dgvJogcimek_MouseLeave);
            this.dgvJogcimek.MouseMove += new System.Windows.Forms.MouseEventHandler(this.dgvJogcimek_MouseMove);
            // 
            // jcmidDataGridViewTextBoxColumn
            // 
            this.jcmidDataGridViewTextBoxColumn.DataPropertyName = "jcm_id";
            this.jcmidDataGridViewTextBoxColumn.HeaderText = "Jcm id";
            this.jcmidDataGridViewTextBoxColumn.Name = "jcmidDataGridViewTextBoxColumn";
            this.jcmidDataGridViewTextBoxColumn.ReadOnly = true;
            this.jcmidDataGridViewTextBoxColumn.Width = 73;
            // 
            // tipusDataGridViewTextBoxColumn
            // 
            this.tipusDataGridViewTextBoxColumn.DataPropertyName = "tipus";
            this.tipusDataGridViewTextBoxColumn.HeaderText = "Típus";
            this.tipusDataGridViewTextBoxColumn.MaxInputLength = 1;
            this.tipusDataGridViewTextBoxColumn.Name = "tipusDataGridViewTextBoxColumn";
            this.tipusDataGridViewTextBoxColumn.Width = 68;
            // 
            // megnevezesDataGridViewTextBoxColumn
            // 
            this.megnevezesDataGridViewTextBoxColumn.DataPropertyName = "megnevezes";
            this.megnevezesDataGridViewTextBoxColumn.HeaderText = "Megnevezés";
            this.megnevezesDataGridViewTextBoxColumn.MaxInputLength = 100;
            this.megnevezesDataGridViewTextBoxColumn.Name = "megnevezesDataGridViewTextBoxColumn";
            this.megnevezesDataGridViewTextBoxColumn.Width = 113;
            // 
            // ervenyesDataGridViewTextBoxColumn
            // 
            this.ervenyesDataGridViewTextBoxColumn.DataPropertyName = "ervenyes";
            this.ervenyesDataGridViewTextBoxColumn.DisplayStyle = System.Windows.Forms.DataGridViewComboBoxDisplayStyle.ComboBox;
            this.ervenyesDataGridViewTextBoxColumn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ervenyesDataGridViewTextBoxColumn.HeaderText = "Érvényes";
            this.ervenyesDataGridViewTextBoxColumn.Items.AddRange(new object[] {
            "I",
            "N"});
            this.ervenyesDataGridViewTextBoxColumn.Name = "ervenyesDataGridViewTextBoxColumn";
            this.ervenyesDataGridViewTextBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.ervenyesDataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.ervenyesDataGridViewTextBoxColumn.Width = 92;
            // 
            // spJogcimekBindingSource
            // 
            this.spJogcimekBindingSource.DataMember = "spJogcimek";
            this.spJogcimekBindingSource.DataSource = this.vPFSDataSet;
            // 
            // vPFSDataSet
            // 
            this.vPFSDataSet.DataSetName = "VPFSDataSet";
            this.vPFSDataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // spJogcimekTableAdapter
            // 
            this.spJogcimekTableAdapter.ClearBeforeFill = true;
            // 
            // Jogcimek
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.ClientSize = new System.Drawing.Size(806, 452);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.Adatcimke);
            this.Controls.Add(this.dgvJogcimek);
            this.Name = "Jogcimek";
            this.Text = "Jogcímek";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Jogcimek_FormClosed);
            this.Load += new System.EventHandler(this.Jogcimek_Load);
            this.Resize += new System.EventHandler(this.Jogcimek_Resize);
            this.Controls.SetChildIndex(this.dgvJogcimek, 0);
            this.Controls.SetChildIndex(this.Adatcimke, 0);
            this.Controls.SetChildIndex(this.label2, 0);
            ((System.ComponentModel.ISupportInitialize)(this.dgvJogcimek)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.spJogcimekBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.vPFSDataSet)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label Adatcimke;
        private System.Windows.Forms.DataGridView dgvJogcimek;
        private VPFSDataSet vPFSDataSet;
        private System.Windows.Forms.BindingSource spJogcimekBindingSource;
        private VPFSDataSetTableAdapters.spJogcimekTableAdapter spJogcimekTableAdapter;
        private System.Windows.Forms.DataGridViewTextBoxColumn jcmidDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn tipusDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn megnevezesDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewComboBoxColumn ervenyesDataGridViewTextBoxColumn;
    }
}
