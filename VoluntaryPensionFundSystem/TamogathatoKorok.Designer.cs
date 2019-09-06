namespace VoluntaryPensionFundSystem
{
    partial class TamogathatoKorok
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
            this.dgvTamogKorok = new System.Windows.Forms.DataGridView();
            this.tmkoridDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.leirasDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.spTamogathatokorokBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.vPFSDataSet = new VoluntaryPensionFundSystem.VPFSDataSet();
            this.label2 = new System.Windows.Forms.Label();
            this.Adatcimke = new System.Windows.Forms.Label();
            this.spTamogathato_korokTableAdapter = new VoluntaryPensionFundSystem.VPFSDataSetTableAdapters.spTamogathato_korokTableAdapter();
            ((System.ComponentModel.ISupportInitialize)(this.dgvTamogKorok)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.spTamogathatokorokBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.vPFSDataSet)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvTamogKorok
            // 
            this.dgvTamogKorok.AllowUserToDeleteRows = false;
            this.dgvTamogKorok.AutoGenerateColumns = false;
            this.dgvTamogKorok.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dgvTamogKorok.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvTamogKorok.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.tmkoridDataGridViewTextBoxColumn,
            this.leirasDataGridViewTextBoxColumn});
            this.dgvTamogKorok.DataSource = this.spTamogathatokorokBindingSource;
            this.dgvTamogKorok.Location = new System.Drawing.Point(29, 136);
            this.dgvTamogKorok.MultiSelect = false;
            this.dgvTamogKorok.Name = "dgvTamogKorok";
            this.dgvTamogKorok.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.dgvTamogKorok.ShowEditingIcon = false;
            this.dgvTamogKorok.Size = new System.Drawing.Size(969, 394);
            this.dgvTamogKorok.TabIndex = 100;
            this.dgvTamogKorok.CellEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvTamogKorok_CellEnter);
            this.dgvTamogKorok.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dgvTamogKorok_CellFormatting);
            this.dgvTamogKorok.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.dgvTamogKorok_DataError);
            this.dgvTamogKorok.EditingControlShowing += new System.Windows.Forms.DataGridViewEditingControlShowingEventHandler(this.dataGridView_EditingControlShowing);
            this.dgvTamogKorok.SelectionChanged += new System.EventHandler(this.dgvTamogKorok_SelectionChanged);
            this.dgvTamogKorok.MouseLeave += new System.EventHandler(this.dgvTamogKorok_MouseLeave);
            this.dgvTamogKorok.MouseMove += new System.Windows.Forms.MouseEventHandler(this.dgvTamogKorok_MouseMove);
            // 
            // tmkoridDataGridViewTextBoxColumn
            // 
            this.tmkoridDataGridViewTextBoxColumn.DataPropertyName = "tmkor_id";
            this.tmkoridDataGridViewTextBoxColumn.HeaderText = "Azonosító";
            this.tmkoridDataGridViewTextBoxColumn.Name = "tmkoridDataGridViewTextBoxColumn";
            this.tmkoridDataGridViewTextBoxColumn.ReadOnly = true;
            this.tmkoridDataGridViewTextBoxColumn.Width = 95;
            // 
            // leirasDataGridViewTextBoxColumn
            // 
            this.leirasDataGridViewTextBoxColumn.DataPropertyName = "leiras";
            this.leirasDataGridViewTextBoxColumn.HeaderText = "Leirás";
            this.leirasDataGridViewTextBoxColumn.MaxInputLength = 255;
            this.leirasDataGridViewTextBoxColumn.Name = "leirasDataGridViewTextBoxColumn";
            this.leirasDataGridViewTextBoxColumn.Width = 72;
            // 
            // spTamogathatokorokBindingSource
            // 
            this.spTamogathatokorokBindingSource.DataMember = "spTamogathato_korok";
            this.spTamogathatokorokBindingSource.DataSource = this.vPFSDataSet;
            // 
            // vPFSDataSet
            // 
            this.vPFSDataSet.DataSetName = "VPFSDataSet";
            this.vPFSDataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label2.Location = new System.Drawing.Point(380, 72);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(266, 29);
            this.label2.TabIndex = 103;
            this.label2.Text = "Támogathatósági körök";
            // 
            // Adatcimke
            // 
            this.Adatcimke.AutoSize = true;
            this.Adatcimke.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.Adatcimke.Location = new System.Drawing.Point(39, 98);
            this.Adatcimke.Name = "Adatcimke";
            this.Adatcimke.Size = new System.Drawing.Size(51, 20);
            this.Adatcimke.TabIndex = 102;
            this.Adatcimke.Text = "label1";
            // 
            // spTamogathato_korokTableAdapter
            // 
            this.spTamogathato_korokTableAdapter.ClearBeforeFill = true;
            // 
            // TamogathatoKorok
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1027, 556);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.Adatcimke);
            this.Controls.Add(this.dgvTamogKorok);
            this.Margin = new System.Windows.Forms.Padding(5);
            this.Name = "TamogathatoKorok";
            this.Text = "Támogathatósági körök";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.TamogathatoKorok_FormClosed);
            this.Load += new System.EventHandler(this.TamogathatoKorok_Load);
            this.Resize += new System.EventHandler(this.TamogathatoKorok_Resize);
            this.Controls.SetChildIndex(this.dgvTamogKorok, 0);
            this.Controls.SetChildIndex(this.Adatcimke, 0);
            this.Controls.SetChildIndex(this.label2, 0);
            ((System.ComponentModel.ISupportInitialize)(this.dgvTamogKorok)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.spTamogathatokorokBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.vPFSDataSet)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvTamogKorok;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label Adatcimke;
        private VPFSDataSet vPFSDataSet;
        private System.Windows.Forms.BindingSource spTamogathatokorokBindingSource;
        private VPFSDataSetTableAdapters.spTamogathato_korokTableAdapter spTamogathato_korokTableAdapter;
        private System.Windows.Forms.DataGridViewTextBoxColumn tmkoridDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn leirasDataGridViewTextBoxColumn;
    }
}