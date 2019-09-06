namespace VoluntaryPensionFundSystem
{
    partial class Bankszamlak
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
            this.dgwBankszla = new System.Windows.Forms.DataGridView();
            this.bszlaidDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pnridDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.szamlaszamDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ervkezdeteDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ervvegeDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ervenyesDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.megjegyzesDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.spBankszlaBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.vPFSDataSetBankszla = new VoluntaryPensionFundSystem.VPFSDataSetBankszla();
            this.Adatcimke = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.spBankszlaTableAdapter = new VoluntaryPensionFundSystem.VPFSDataSetBankszlaTableAdapters.spBankszlaTableAdapter();
            ((System.ComponentModel.ISupportInitialize)(this.dgwBankszla)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.spBankszlaBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.vPFSDataSetBankszla)).BeginInit();
            this.SuspendLayout();
            // 
            // dgwBankszla
            // 
            this.dgwBankszla.AutoGenerateColumns = false;
            this.dgwBankszla.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgwBankszla.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.bszlaidDataGridViewTextBoxColumn,
            this.pnridDataGridViewTextBoxColumn,
            this.szamlaszamDataGridViewTextBoxColumn,
            this.ervkezdeteDataGridViewTextBoxColumn,
            this.ervvegeDataGridViewTextBoxColumn,
            this.ervenyesDataGridViewTextBoxColumn,
            this.megjegyzesDataGridViewTextBoxColumn});
            this.dgwBankszla.DataSource = this.spBankszlaBindingSource;
            this.dgwBankszla.Location = new System.Drawing.Point(27, 154);
            this.dgwBankszla.MultiSelect = false;
            this.dgwBankszla.Name = "dgwBankszla";
            this.dgwBankszla.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.dgwBankszla.ShowEditingIcon = false;
            this.dgwBankszla.Size = new System.Drawing.Size(941, 257);
            this.dgwBankszla.TabIndex = 3;
            this.dgwBankszla.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgwBankszla_CellEndEdit);
            this.dgwBankszla.CellEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgwBankszla_CellEnter);
            this.dgwBankszla.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dgwBankszla_CellFormatting);
            this.dgwBankszla.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.dgwBankszla_DataError);
            this.dgwBankszla.EditingControlShowing += new System.Windows.Forms.DataGridViewEditingControlShowingEventHandler(this.dataGridView_EditingControlShowing);
            this.dgwBankszla.SelectionChanged += new System.EventHandler(this.dgwBankszla_SelectionChanged);
            this.dgwBankszla.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dgwBankszla_KeyDown);
            this.dgwBankszla.MouseLeave += new System.EventHandler(this.dgwBankszla_MouseLeave);
            this.dgwBankszla.MouseMove += new System.Windows.Forms.MouseEventHandler(this.dgwBankszla_MouseMove);
            // 
            // bszlaidDataGridViewTextBoxColumn
            // 
            this.bszlaidDataGridViewTextBoxColumn.DataPropertyName = "bszla_id";
            this.bszlaidDataGridViewTextBoxColumn.HeaderText = "bszla_id";
            this.bszlaidDataGridViewTextBoxColumn.Name = "bszlaidDataGridViewTextBoxColumn";
            this.bszlaidDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // pnridDataGridViewTextBoxColumn
            // 
            this.pnridDataGridViewTextBoxColumn.DataPropertyName = "pnr_id";
            this.pnridDataGridViewTextBoxColumn.HeaderText = "pnr_id";
            this.pnridDataGridViewTextBoxColumn.Name = "pnridDataGridViewTextBoxColumn";
            this.pnridDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // szamlaszamDataGridViewTextBoxColumn
            // 
            this.szamlaszamDataGridViewTextBoxColumn.DataPropertyName = "szamlaszam";
            this.szamlaszamDataGridViewTextBoxColumn.HeaderText = "Számlaszám";
            this.szamlaszamDataGridViewTextBoxColumn.MaxInputLength = 24;
            this.szamlaszamDataGridViewTextBoxColumn.Name = "szamlaszamDataGridViewTextBoxColumn";
            this.szamlaszamDataGridViewTextBoxColumn.Width = 250;
            // 
            // ervkezdeteDataGridViewTextBoxColumn
            // 
            this.ervkezdeteDataGridViewTextBoxColumn.DataPropertyName = "erv_kezdete";
            this.ervkezdeteDataGridViewTextBoxColumn.HeaderText = "Érv. kezdete";
            this.ervkezdeteDataGridViewTextBoxColumn.Name = "ervkezdeteDataGridViewTextBoxColumn";
            this.ervkezdeteDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // ervvegeDataGridViewTextBoxColumn
            // 
            this.ervvegeDataGridViewTextBoxColumn.DataPropertyName = "erv_vege";
            this.ervvegeDataGridViewTextBoxColumn.HeaderText = "Érv. vége";
            this.ervvegeDataGridViewTextBoxColumn.Name = "ervvegeDataGridViewTextBoxColumn";
            this.ervvegeDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // ervenyesDataGridViewTextBoxColumn
            // 
            this.ervenyesDataGridViewTextBoxColumn.DataPropertyName = "ervenyes";
            this.ervenyesDataGridViewTextBoxColumn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ervenyesDataGridViewTextBoxColumn.HeaderText = "Érv.";
            this.ervenyesDataGridViewTextBoxColumn.Items.AddRange(new object[] {
            "I",
            "N"});
            this.ervenyesDataGridViewTextBoxColumn.Name = "ervenyesDataGridViewTextBoxColumn";
            this.ervenyesDataGridViewTextBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.ervenyesDataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.ervenyesDataGridViewTextBoxColumn.Width = 60;
            // 
            // megjegyzesDataGridViewTextBoxColumn
            // 
            this.megjegyzesDataGridViewTextBoxColumn.DataPropertyName = "megjegyzes";
            this.megjegyzesDataGridViewTextBoxColumn.HeaderText = "Megjegyzés";
            this.megjegyzesDataGridViewTextBoxColumn.MaxInputLength = 255;
            this.megjegyzesDataGridViewTextBoxColumn.Name = "megjegyzesDataGridViewTextBoxColumn";
            this.megjegyzesDataGridViewTextBoxColumn.Width = 350;
            // 
            // spBankszlaBindingSource
            // 
            this.spBankszlaBindingSource.DataMember = "spBankszla";
            this.spBankszlaBindingSource.DataSource = this.vPFSDataSetBankszla;
            // 
            // vPFSDataSetBankszla
            // 
            this.vPFSDataSetBankszla.DataSetName = "VPFSDataSetBankszla";
            this.vPFSDataSetBankszla.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // Adatcimke
            // 
            this.Adatcimke.AutoSize = true;
            this.Adatcimke.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.Adatcimke.Location = new System.Drawing.Point(24, 117);
            this.Adatcimke.Name = "Adatcimke";
            this.Adatcimke.Size = new System.Drawing.Size(51, 20);
            this.Adatcimke.TabIndex = 4;
            this.Adatcimke.Text = "label1";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label2.Location = new System.Drawing.Point(410, 75);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(154, 29);
            this.label2.TabIndex = 92;
            this.label2.Text = "Bankszámlák";
            // 
            // spBankszlaTableAdapter
            // 
            this.spBankszlaTableAdapter.ClearBeforeFill = true;
            // 
            // Bankszamlak
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(997, 449);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.Adatcimke);
            this.Controls.Add(this.dgwBankszla);
            this.Margin = new System.Windows.Forms.Padding(5);
            this.Name = "Bankszamlak";
            this.Text = "Bankszámlák";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Bankszamlak_FormClosed);
            this.Load += new System.EventHandler(this.Bankszamlak_Load);
            this.Resize += new System.EventHandler(this.Bankszamlak_Resize);
            this.Controls.SetChildIndex(this.dgwBankszla, 0);
            this.Controls.SetChildIndex(this.Adatcimke, 0);
            this.Controls.SetChildIndex(this.label2, 0);
            ((System.ComponentModel.ISupportInitialize)(this.dgwBankszla)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.spBankszlaBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.vPFSDataSetBankszla)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dgwBankszla;
        private System.Windows.Forms.Label Adatcimke;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.BindingSource spBankszlaBindingSource;
        private VPFSDataSetBankszla vPFSDataSetBankszla;
        private VPFSDataSetBankszlaTableAdapters.spBankszlaTableAdapter spBankszlaTableAdapter;
        private System.Windows.Forms.DataGridViewTextBoxColumn bszlaidDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn pnridDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn szamlaszamDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn ervkezdeteDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn ervvegeDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewComboBoxColumn ervenyesDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn megjegyzesDataGridViewTextBoxColumn;
    }
}