namespace VoluntaryPensionFundSystem
{
    partial class GazdasagiEsemenyek
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
            this.dgvGenykod = new System.Windows.Forms.DataGridView();
            this.genyidDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.genykodDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.leirasDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.megjegyzesDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.spGazdasagiesemenyekBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.vPFSDataSet = new VoluntaryPensionFundSystem.VPFSDataSet();
            this.spGazdasagi_esemenyekTableAdapter = new VoluntaryPensionFundSystem.VPFSDataSetTableAdapters.spGazdasagi_esemenyekTableAdapter();
            ((System.ComponentModel.ISupportInitialize)(this.dgvGenykod)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.spGazdasagiesemenyekBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.vPFSDataSet)).BeginInit();
            this.SuspendLayout();
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label2.Location = new System.Drawing.Point(376, 66);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(257, 29);
            this.label2.TabIndex = 104;
            this.label2.Text = "Gazdasági események";
            // 
            // Adatcimke
            // 
            this.Adatcimke.AutoSize = true;
            this.Adatcimke.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.Adatcimke.Location = new System.Drawing.Point(33, 82);
            this.Adatcimke.Name = "Adatcimke";
            this.Adatcimke.Size = new System.Drawing.Size(51, 20);
            this.Adatcimke.TabIndex = 103;
            this.Adatcimke.Text = "label1";
            // 
            // dgvGenykod
            // 
            this.dgvGenykod.AllowUserToDeleteRows = false;
            this.dgvGenykod.AutoGenerateColumns = false;
            this.dgvGenykod.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dgvGenykod.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvGenykod.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.genyidDataGridViewTextBoxColumn,
            this.genykodDataGridViewTextBoxColumn,
            this.leirasDataGridViewTextBoxColumn,
            this.megjegyzesDataGridViewTextBoxColumn});
            this.dgvGenykod.DataSource = this.spGazdasagiesemenyekBindingSource;
            this.dgvGenykod.Location = new System.Drawing.Point(34, 119);
            this.dgvGenykod.MultiSelect = false;
            this.dgvGenykod.Name = "dgvGenykod";
            this.dgvGenykod.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.dgvGenykod.ShowEditingIcon = false;
            this.dgvGenykod.Size = new System.Drawing.Size(941, 268);
            this.dgvGenykod.TabIndex = 102;
            this.dgvGenykod.CellEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvGenykod_CellEnter);
            this.dgvGenykod.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dgvGenykod_CellFormatting);
            this.dgvGenykod.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.dgvGenykod_DataError);
            this.dgvGenykod.EditingControlShowing += new System.Windows.Forms.DataGridViewEditingControlShowingEventHandler(this.dataGridView_EditingControlShowing);
            this.dgvGenykod.SelectionChanged += new System.EventHandler(this.dgvGenykod_SelectionChanged);
            this.dgvGenykod.MouseLeave += new System.EventHandler(this.dgvGenykod_MouseLeave);
            this.dgvGenykod.MouseMove += new System.Windows.Forms.MouseEventHandler(this.dgvGenykod_MouseMove);
            // 
            // genyidDataGridViewTextBoxColumn
            // 
            this.genyidDataGridViewTextBoxColumn.DataPropertyName = "geny_id";
            this.genyidDataGridViewTextBoxColumn.HeaderText = "geny_id";
            this.genyidDataGridViewTextBoxColumn.Name = "genyidDataGridViewTextBoxColumn";
            this.genyidDataGridViewTextBoxColumn.ReadOnly = true;
            this.genyidDataGridViewTextBoxColumn.Width = 83;
            // 
            // genykodDataGridViewTextBoxColumn
            // 
            this.genykodDataGridViewTextBoxColumn.DataPropertyName = "geny_kod";
            this.genykodDataGridViewTextBoxColumn.HeaderText = "GENYKOD";
            this.genykodDataGridViewTextBoxColumn.MaxInputLength = 25;
            this.genykodDataGridViewTextBoxColumn.Name = "genykodDataGridViewTextBoxColumn";
            this.genykodDataGridViewTextBoxColumn.Width = 102;
            // 
            // leirasDataGridViewTextBoxColumn
            // 
            this.leirasDataGridViewTextBoxColumn.DataPropertyName = "leiras";
            this.leirasDataGridViewTextBoxColumn.HeaderText = "Leirás";
            this.leirasDataGridViewTextBoxColumn.MaxInputLength = 255;
            this.leirasDataGridViewTextBoxColumn.Name = "leirasDataGridViewTextBoxColumn";
            this.leirasDataGridViewTextBoxColumn.Width = 72;
            // 
            // megjegyzesDataGridViewTextBoxColumn
            // 
            this.megjegyzesDataGridViewTextBoxColumn.DataPropertyName = "megjegyzes";
            this.megjegyzesDataGridViewTextBoxColumn.HeaderText = "Megjegyzés";
            this.megjegyzesDataGridViewTextBoxColumn.MaxInputLength = 255;
            this.megjegyzesDataGridViewTextBoxColumn.Name = "megjegyzesDataGridViewTextBoxColumn";
            this.megjegyzesDataGridViewTextBoxColumn.Width = 108;
            // 
            // spGazdasagiesemenyekBindingSource
            // 
            this.spGazdasagiesemenyekBindingSource.DataMember = "spGazdasagi_esemenyek";
            this.spGazdasagiesemenyekBindingSource.DataSource = this.vPFSDataSet;
            // 
            // vPFSDataSet
            // 
            this.vPFSDataSet.DataSetName = "VPFSDataSet";
            this.vPFSDataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // spGazdasagi_esemenyekTableAdapter
            // 
            this.spGazdasagi_esemenyekTableAdapter.ClearBeforeFill = true;
            // 
            // GazdasagiEsemenyek
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1008, 423);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.Adatcimke);
            this.Controls.Add(this.dgvGenykod);
            this.Name = "GazdasagiEsemenyek";
            this.Text = "Gazdasági események";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.GazdasagiEsemenyek_FormClosed);
            this.Load += new System.EventHandler(this.GazdasagiEsemenyek_Load);
            this.Resize += new System.EventHandler(this.GazdasagiEsemenyek_Resize);
            this.Controls.SetChildIndex(this.dgvGenykod, 0);
            this.Controls.SetChildIndex(this.Adatcimke, 0);
            this.Controls.SetChildIndex(this.label2, 0);
            ((System.ComponentModel.ISupportInitialize)(this.dgvGenykod)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.spGazdasagiesemenyekBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.vPFSDataSet)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label Adatcimke;
        private System.Windows.Forms.DataGridView dgvGenykod;
        private VPFSDataSet vPFSDataSet;
        private System.Windows.Forms.BindingSource spGazdasagiesemenyekBindingSource;
        private VPFSDataSetTableAdapters.spGazdasagi_esemenyekTableAdapter spGazdasagi_esemenyekTableAdapter;
        private System.Windows.Forms.DataGridViewTextBoxColumn genyidDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn genykodDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn leirasDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn megjegyzesDataGridViewTextBoxColumn;
    }
}