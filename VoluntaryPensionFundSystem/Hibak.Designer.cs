namespace VoluntaryPensionFundSystem
{
    partial class Hibak
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
            this.dgwHibak = new System.Windows.Forms.DataGridView();
            this.hibaidDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.leirasDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.bevervenyesDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.ervenyesDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.megjegyzesDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.rogzitneveDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.rogzitdatumDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.modositneveDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.modositdatumDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.spHibakBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.vPFSDataSet = new VoluntaryPensionFundSystem.VPFSDataSet();
            this.spHibakTableAdapter = new VoluntaryPensionFundSystem.VPFSDataSetTableAdapters.spHibakTableAdapter();
            ((System.ComponentModel.ISupportInitialize)(this.dgwHibak)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.spHibakBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.vPFSDataSet)).BeginInit();
            this.SuspendLayout();
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label2.Location = new System.Drawing.Point(474, 66);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(75, 29);
            this.label2.TabIndex = 98;
            this.label2.Text = "Hibák";
            // 
            // Adatcimke
            // 
            this.Adatcimke.AutoSize = true;
            this.Adatcimke.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.Adatcimke.Location = new System.Drawing.Point(32, 86);
            this.Adatcimke.Name = "Adatcimke";
            this.Adatcimke.Size = new System.Drawing.Size(51, 20);
            this.Adatcimke.TabIndex = 97;
            this.Adatcimke.Text = "label1";
            // 
            // dgwHibak
            // 
            this.dgwHibak.AutoGenerateColumns = false;
            this.dgwHibak.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgwHibak.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.hibaidDataGridViewTextBoxColumn,
            this.leirasDataGridViewTextBoxColumn,
            this.bevervenyesDataGridViewTextBoxColumn,
            this.ervenyesDataGridViewTextBoxColumn,
            this.megjegyzesDataGridViewTextBoxColumn,
            this.rogzitneveDataGridViewTextBoxColumn,
            this.rogzitdatumDataGridViewTextBoxColumn,
            this.modositneveDataGridViewTextBoxColumn,
            this.modositdatumDataGridViewTextBoxColumn});
            this.dgwHibak.DataSource = this.spHibakBindingSource;
            this.dgwHibak.Location = new System.Drawing.Point(41, 123);
            this.dgwHibak.MultiSelect = false;
            this.dgwHibak.Name = "dgwHibak";
            this.dgwHibak.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.dgwHibak.ShowEditingIcon = false;
            this.dgwHibak.Size = new System.Drawing.Size(941, 509);
            this.dgwHibak.TabIndex = 0;
            this.dgwHibak.CellEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgwHibak_CellEnter);
            this.dgwHibak.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dgwHibak_CellFormatting);
            this.dgwHibak.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.dgwHibak_DataError);
            this.dgwHibak.EditingControlShowing += new System.Windows.Forms.DataGridViewEditingControlShowingEventHandler(this.dataGridView_EditingControlShowing);
            this.dgwHibak.SelectionChanged += new System.EventHandler(this.dgwHibak_SelectionChanged);
            this.dgwHibak.MouseLeave += new System.EventHandler(this.dgwHibak_MouseLeave);
            this.dgwHibak.MouseMove += new System.Windows.Forms.MouseEventHandler(this.dgwHibak_MouseMove);
            // 
            // hibaidDataGridViewTextBoxColumn
            // 
            this.hibaidDataGridViewTextBoxColumn.DataPropertyName = "hiba_id";
            this.hibaidDataGridViewTextBoxColumn.HeaderText = "Hibakód";
            this.hibaidDataGridViewTextBoxColumn.Name = "hibaidDataGridViewTextBoxColumn";
            this.hibaidDataGridViewTextBoxColumn.ReadOnly = true;
            this.hibaidDataGridViewTextBoxColumn.Width = 79;
            // 
            // leirasDataGridViewTextBoxColumn
            // 
            this.leirasDataGridViewTextBoxColumn.DataPropertyName = "leiras";
            this.leirasDataGridViewTextBoxColumn.HeaderText = "Leírás";
            this.leirasDataGridViewTextBoxColumn.MaxInputLength = 255;
            this.leirasDataGridViewTextBoxColumn.Name = "leirasDataGridViewTextBoxColumn";
            this.leirasDataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.leirasDataGridViewTextBoxColumn.Width = 350;
            // 
            // bevervenyesDataGridViewTextBoxColumn
            // 
            this.bevervenyesDataGridViewTextBoxColumn.AutoComplete = false;
            this.bevervenyesDataGridViewTextBoxColumn.DataPropertyName = "bev_ervenyes";
            this.bevervenyesDataGridViewTextBoxColumn.DisplayStyleForCurrentCellOnly = true;
            this.bevervenyesDataGridViewTextBoxColumn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.bevervenyesDataGridViewTextBoxColumn.HeaderText = "Bev.Érv.";
            this.bevervenyesDataGridViewTextBoxColumn.Items.AddRange(new object[] {
            "I",
            "N"});
            this.bevervenyesDataGridViewTextBoxColumn.Name = "bevervenyesDataGridViewTextBoxColumn";
            this.bevervenyesDataGridViewTextBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.bevervenyesDataGridViewTextBoxColumn.Width = 67;
            // 
            // ervenyesDataGridViewTextBoxColumn
            // 
            this.ervenyesDataGridViewTextBoxColumn.AutoComplete = false;
            this.ervenyesDataGridViewTextBoxColumn.DataPropertyName = "ervenyes";
            this.ervenyesDataGridViewTextBoxColumn.DisplayStyleForCurrentCellOnly = true;
            this.ervenyesDataGridViewTextBoxColumn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ervenyesDataGridViewTextBoxColumn.HeaderText = "Érvényes";
            this.ervenyesDataGridViewTextBoxColumn.Items.AddRange(new object[] {
            "I",
            "N"});
            this.ervenyesDataGridViewTextBoxColumn.Name = "ervenyesDataGridViewTextBoxColumn";
            this.ervenyesDataGridViewTextBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.ervenyesDataGridViewTextBoxColumn.Width = 73;
            // 
            // megjegyzesDataGridViewTextBoxColumn
            // 
            this.megjegyzesDataGridViewTextBoxColumn.DataPropertyName = "megjegyzes";
            this.megjegyzesDataGridViewTextBoxColumn.HeaderText = "Megjegyzés";
            this.megjegyzesDataGridViewTextBoxColumn.MaxInputLength = 255;
            this.megjegyzesDataGridViewTextBoxColumn.Name = "megjegyzesDataGridViewTextBoxColumn";
            this.megjegyzesDataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.megjegyzesDataGridViewTextBoxColumn.Width = 300;
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
            // spHibakBindingSource
            // 
            this.spHibakBindingSource.DataMember = "spHibak";
            this.spHibakBindingSource.DataSource = this.vPFSDataSet;
            // 
            // vPFSDataSet
            // 
            this.vPFSDataSet.DataSetName = "VPFSDataSet";
            this.vPFSDataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // spHibakTableAdapter
            // 
            this.spHibakTableAdapter.ClearBeforeFill = true;
            // 
            // Hibak
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.ClientSize = new System.Drawing.Size(1022, 661);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.Adatcimke);
            this.Controls.Add(this.dgwHibak);
            this.Name = "Hibak";
            this.Text = "Hibák";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Hibak_FormClosed);
            this.Load += new System.EventHandler(this.Hibak_Load);
            this.Resize += new System.EventHandler(this.Hibak_Resize);
            this.Controls.SetChildIndex(this.dgwHibak, 0);
            this.Controls.SetChildIndex(this.Adatcimke, 0);
            this.Controls.SetChildIndex(this.label2, 0);
            ((System.ComponentModel.ISupportInitialize)(this.dgwHibak)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.spHibakBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.vPFSDataSet)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label Adatcimke;
        private System.Windows.Forms.DataGridView dgwHibak;
        private VPFSDataSet vPFSDataSet;
        private System.Windows.Forms.BindingSource spHibakBindingSource;
        private VPFSDataSetTableAdapters.spHibakTableAdapter spHibakTableAdapter;
        private System.Windows.Forms.DataGridViewTextBoxColumn hibaidDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn leirasDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewComboBoxColumn bevervenyesDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewComboBoxColumn ervenyesDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn megjegyzesDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn rogzitneveDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn rogzitdatumDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn modositneveDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn modositdatumDataGridViewTextBoxColumn;
    }
}
