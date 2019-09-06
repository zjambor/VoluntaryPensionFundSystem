namespace VoluntaryPensionFundSystem
{
    partial class Nyugdijpenztarak
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
            this.dgvNyugdijpenzt = new System.Windows.Forms.DataGridView();
            this.nypidDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.nevDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.irszamDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.helysegDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cimDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.megjegyzesDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.spNyugdijpenztarakBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.vPFSDataSet = new VoluntaryPensionFundSystem.VPFSDataSet();
            this.spNyugdijpenztarakTableAdapter = new VoluntaryPensionFundSystem.VPFSDataSetTableAdapters.spNyugdijpenztarakTableAdapter();
            ((System.ComponentModel.ISupportInitialize)(this.dgvNyugdijpenzt)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.spNyugdijpenztarakBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.vPFSDataSet)).BeginInit();
            this.SuspendLayout();
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label2.Location = new System.Drawing.Point(405, 64);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(199, 29);
            this.label2.TabIndex = 106;
            this.label2.Text = "Nyugdíjpénztárak";
            // 
            // Adatcimke
            // 
            this.Adatcimke.AutoSize = true;
            this.Adatcimke.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.Adatcimke.Location = new System.Drawing.Point(32, 87);
            this.Adatcimke.Name = "Adatcimke";
            this.Adatcimke.Size = new System.Drawing.Size(51, 20);
            this.Adatcimke.TabIndex = 105;
            this.Adatcimke.Text = "label1";
            // 
            // dgvNyugdijpenzt
            // 
            this.dgvNyugdijpenzt.AllowUserToDeleteRows = false;
            this.dgvNyugdijpenzt.AutoGenerateColumns = false;
            this.dgvNyugdijpenzt.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dgvNyugdijpenzt.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvNyugdijpenzt.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.nypidDataGridViewTextBoxColumn,
            this.nevDataGridViewTextBoxColumn,
            this.irszamDataGridViewTextBoxColumn,
            this.helysegDataGridViewTextBoxColumn,
            this.cimDataGridViewTextBoxColumn,
            this.megjegyzesDataGridViewTextBoxColumn});
            this.dgvNyugdijpenzt.DataSource = this.spNyugdijpenztarakBindingSource;
            this.dgvNyugdijpenzt.Location = new System.Drawing.Point(34, 121);
            this.dgvNyugdijpenzt.MultiSelect = false;
            this.dgvNyugdijpenzt.Name = "dgvNyugdijpenzt";
            this.dgvNyugdijpenzt.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.dgvNyugdijpenzt.ShowEditingIcon = false;
            this.dgvNyugdijpenzt.Size = new System.Drawing.Size(941, 383);
            this.dgvNyugdijpenzt.TabIndex = 104;
            this.dgvNyugdijpenzt.CellEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvNyugdijpenzt_CellEnter);
            this.dgvNyugdijpenzt.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dgvNyugdijpenzt_CellFormatting);
            this.dgvNyugdijpenzt.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.dgvNyugdijpenzt_DataError);
            this.dgvNyugdijpenzt.EditingControlShowing += new System.Windows.Forms.DataGridViewEditingControlShowingEventHandler(this.dataGridView_EditingControlShowing);
            this.dgvNyugdijpenzt.SelectionChanged += new System.EventHandler(this.dgvNyugdijpenzt_SelectionChanged);
            this.dgvNyugdijpenzt.MouseLeave += new System.EventHandler(this.dgvNyugdijpenzt_MouseLeave);
            this.dgvNyugdijpenzt.MouseMove += new System.Windows.Forms.MouseEventHandler(this.dgvNyugdijpenzt_MouseMove);
            // 
            // nypidDataGridViewTextBoxColumn
            // 
            this.nypidDataGridViewTextBoxColumn.DataPropertyName = "nyp_id";
            this.nypidDataGridViewTextBoxColumn.HeaderText = "Azonosító";
            this.nypidDataGridViewTextBoxColumn.Name = "nypidDataGridViewTextBoxColumn";
            this.nypidDataGridViewTextBoxColumn.ReadOnly = true;
            this.nypidDataGridViewTextBoxColumn.Width = 95;
            // 
            // nevDataGridViewTextBoxColumn
            // 
            this.nevDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.nevDataGridViewTextBoxColumn.DataPropertyName = "nev";
            this.nevDataGridViewTextBoxColumn.HeaderText = "Név";
            this.nevDataGridViewTextBoxColumn.MaxInputLength = 100;
            this.nevDataGridViewTextBoxColumn.Name = "nevDataGridViewTextBoxColumn";
            this.nevDataGridViewTextBoxColumn.Width = 250;
            // 
            // irszamDataGridViewTextBoxColumn
            // 
            this.irszamDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.irszamDataGridViewTextBoxColumn.DataPropertyName = "ir_szam";
            this.irszamDataGridViewTextBoxColumn.HeaderText = "Ir.szám";
            this.irszamDataGridViewTextBoxColumn.MaxInputLength = 4;
            this.irszamDataGridViewTextBoxColumn.Name = "irszamDataGridViewTextBoxColumn";
            this.irszamDataGridViewTextBoxColumn.Width = 80;
            // 
            // helysegDataGridViewTextBoxColumn
            // 
            this.helysegDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.helysegDataGridViewTextBoxColumn.DataPropertyName = "helyseg";
            this.helysegDataGridViewTextBoxColumn.HeaderText = "Helység";
            this.helysegDataGridViewTextBoxColumn.MaxInputLength = 20;
            this.helysegDataGridViewTextBoxColumn.Name = "helysegDataGridViewTextBoxColumn";
            this.helysegDataGridViewTextBoxColumn.Width = 150;
            // 
            // cimDataGridViewTextBoxColumn
            // 
            this.cimDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.cimDataGridViewTextBoxColumn.DataPropertyName = "cim";
            this.cimDataGridViewTextBoxColumn.HeaderText = "Cím";
            this.cimDataGridViewTextBoxColumn.MaxInputLength = 80;
            this.cimDataGridViewTextBoxColumn.Name = "cimDataGridViewTextBoxColumn";
            this.cimDataGridViewTextBoxColumn.Width = 200;
            // 
            // megjegyzesDataGridViewTextBoxColumn
            // 
            this.megjegyzesDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.megjegyzesDataGridViewTextBoxColumn.DataPropertyName = "megjegyzes";
            this.megjegyzesDataGridViewTextBoxColumn.HeaderText = "Megjegyzés";
            this.megjegyzesDataGridViewTextBoxColumn.MaxInputLength = 255;
            this.megjegyzesDataGridViewTextBoxColumn.Name = "megjegyzesDataGridViewTextBoxColumn";
            this.megjegyzesDataGridViewTextBoxColumn.Width = 250;
            // 
            // spNyugdijpenztarakBindingSource
            // 
            this.spNyugdijpenztarakBindingSource.DataMember = "spNyugdijpenztarak";
            this.spNyugdijpenztarakBindingSource.DataSource = this.vPFSDataSet;
            // 
            // vPFSDataSet
            // 
            this.vPFSDataSet.DataSetName = "VPFSDataSet";
            this.vPFSDataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // spNyugdijpenztarakTableAdapter
            // 
            this.spNyugdijpenztarakTableAdapter.ClearBeforeFill = true;
            // 
            // Nyugdijpenztarak
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1008, 522);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.Adatcimke);
            this.Controls.Add(this.dgvNyugdijpenzt);
            this.Name = "Nyugdijpenztarak";
            this.Text = "Nyugdíjpénztárak";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Nyugdijpenztarak_FormClosed);
            this.Load += new System.EventHandler(this.Nyugdijpenztarak_Load);
            this.Resize += new System.EventHandler(this.Nyugdijpenztarak_Resize);
            this.Controls.SetChildIndex(this.dgvNyugdijpenzt, 0);
            this.Controls.SetChildIndex(this.Adatcimke, 0);
            this.Controls.SetChildIndex(this.label2, 0);
            ((System.ComponentModel.ISupportInitialize)(this.dgvNyugdijpenzt)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.spNyugdijpenztarakBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.vPFSDataSet)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label Adatcimke;
        private System.Windows.Forms.DataGridView dgvNyugdijpenzt;
        private VPFSDataSet vPFSDataSet;
        private System.Windows.Forms.BindingSource spNyugdijpenztarakBindingSource;
        private VPFSDataSetTableAdapters.spNyugdijpenztarakTableAdapter spNyugdijpenztarakTableAdapter;
        private System.Windows.Forms.DataGridViewTextBoxColumn nypidDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn nevDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn irszamDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn helysegDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn cimDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn megjegyzesDataGridViewTextBoxColumn;
    }
}