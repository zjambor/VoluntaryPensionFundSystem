namespace VoluntaryPensionFundSystem
{
    partial class Korcsoportok
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
            this.dgvKorcsoport = new System.Windows.Forms.DataGridView();
            this.spKorcsoportokBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.vPFSDataSet = new VoluntaryPensionFundSystem.VPFSDataSet();
            this.spKorcsoportokTableAdapter = new VoluntaryPensionFundSystem.VPFSDataSetTableAdapters.spKorcsoportokTableAdapter();
            this.cim = new System.Windows.Forms.Label();
            this.txAdoazon = new System.Windows.Forms.TextBox();
            this.txAdoszam = new System.Windows.Forms.TextBox();
            this.label24 = new System.Windows.Forms.Label();
            this.label23 = new System.Windows.Forms.Label();
            this.label19 = new System.Windows.Forms.Label();
            this.txPnrid = new System.Windows.Forms.TextBox();
            this.txMegnev = new System.Windows.Forms.TextBox();
            this.label18 = new System.Windows.Forms.Label();
            this.bKivalaszt = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvKorcsoport)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.spKorcsoportokBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.vPFSDataSet)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvKorcsoport
            // 
            this.dgvKorcsoport.AllowUserToAddRows = false;
            this.dgvKorcsoport.AllowUserToDeleteRows = false;
            this.dgvKorcsoport.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvKorcsoport.Location = new System.Drawing.Point(12, 202);
            this.dgvKorcsoport.MultiSelect = false;
            this.dgvKorcsoport.Name = "dgvKorcsoport";
            this.dgvKorcsoport.ReadOnly = true;
            this.dgvKorcsoport.Size = new System.Drawing.Size(984, 324);
            this.dgvKorcsoport.TabIndex = 3;
            this.dgvKorcsoport.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dgvKorcsoport_CellFormatting);
            this.dgvKorcsoport.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.dgvKorcsoport_DataError);
            this.dgvKorcsoport.MouseLeave += new System.EventHandler(this.dgvKorcsoport_MouseLeave);
            this.dgvKorcsoport.MouseMove += new System.Windows.Forms.MouseEventHandler(this.dgvKorcsoport_MouseMove);
            // 
            // spKorcsoportokBindingSource
            // 
            this.spKorcsoportokBindingSource.DataMember = "spKorcsoportok";
            this.spKorcsoportokBindingSource.DataSource = this.vPFSDataSet;
            // 
            // vPFSDataSet
            // 
            this.vPFSDataSet.DataSetName = "VPFSDataSet";
            this.vPFSDataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // spKorcsoportokTableAdapter
            // 
            this.spKorcsoportokTableAdapter.ClearBeforeFill = true;
            // 
            // cim
            // 
            this.cim.AutoSize = true;
            this.cim.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.cim.Location = new System.Drawing.Point(426, 73);
            this.cim.Name = "cim";
            this.cim.Size = new System.Drawing.Size(157, 29);
            this.cim.TabIndex = 248;
            this.cim.Text = "Korcsoportok";
            // 
            // txAdoazon
            // 
            this.txAdoazon.Location = new System.Drawing.Point(354, 167);
            this.txAdoazon.Name = "txAdoazon";
            this.txAdoazon.ReadOnly = true;
            this.txAdoazon.Size = new System.Drawing.Size(178, 23);
            this.txAdoazon.TabIndex = 254;
            // 
            // txAdoszam
            // 
            this.txAdoszam.Location = new System.Drawing.Point(121, 167);
            this.txAdoszam.Name = "txAdoszam";
            this.txAdoszam.ReadOnly = true;
            this.txAdoszam.Size = new System.Drawing.Size(227, 23);
            this.txAdoszam.TabIndex = 252;
            // 
            // label24
            // 
            this.label24.AutoSize = true;
            this.label24.Location = new System.Drawing.Point(353, 147);
            this.label24.Name = "label24";
            this.label24.Size = new System.Drawing.Size(112, 17);
            this.label24.TabIndex = 253;
            this.label24.Text = "Adóazonosító jel";
            // 
            // label23
            // 
            this.label23.AutoSize = true;
            this.label23.Location = new System.Drawing.Point(120, 149);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(66, 17);
            this.label23.TabIndex = 251;
            this.label23.Text = "Adószám";
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Location = new System.Drawing.Point(10, 147);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(54, 17);
            this.label19.TabIndex = 249;
            this.label19.Text = "Fogl. id";
            // 
            // txPnrid
            // 
            this.txPnrid.Location = new System.Drawing.Point(12, 167);
            this.txPnrid.MaxLength = 8;
            this.txPnrid.Name = "txPnrid";
            this.txPnrid.ReadOnly = true;
            this.txPnrid.Size = new System.Drawing.Size(100, 23);
            this.txPnrid.TabIndex = 250;
            // 
            // txMegnev
            // 
            this.txMegnev.Location = new System.Drawing.Point(538, 167);
            this.txMegnev.Name = "txMegnev";
            this.txMegnev.ReadOnly = true;
            this.txMegnev.Size = new System.Drawing.Size(458, 23);
            this.txMegnev.TabIndex = 256;
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Location = new System.Drawing.Point(535, 149);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(81, 17);
            this.label18.TabIndex = 255;
            this.label18.Text = "Munk. neve";
            // 
            // bKivalaszt
            // 
            this.bKivalaszt.AutoSize = true;
            this.bKivalaszt.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.bKivalaszt.Location = new System.Drawing.Point(12, 112);
            this.bKivalaszt.Name = "bKivalaszt";
            this.bKivalaszt.Size = new System.Drawing.Size(109, 29);
            this.bKivalaszt.TabIndex = 257;
            this.bKivalaszt.Text = "Fogl. kiválaszt";
            this.bKivalaszt.UseVisualStyleBackColor = true;
            this.bKivalaszt.Click += new System.EventHandler(this.bKivalaszt_Click);
            // 
            // Korcsoportok
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.ClientSize = new System.Drawing.Size(1008, 538);
            this.Controls.Add(this.bKivalaszt);
            this.Controls.Add(this.txMegnev);
            this.Controls.Add(this.label18);
            this.Controls.Add(this.txAdoazon);
            this.Controls.Add(this.txAdoszam);
            this.Controls.Add(this.label24);
            this.Controls.Add(this.label23);
            this.Controls.Add(this.label19);
            this.Controls.Add(this.txPnrid);
            this.Controls.Add(this.cim);
            this.Controls.Add(this.dgvKorcsoport);
            this.Name = "Korcsoportok";
            this.Text = "Korcsoportok";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Korcsoportok_FormClosed);
            this.Load += new System.EventHandler(this.Korcsoportok_Load);
            this.Resize += new System.EventHandler(this.Korcsoportok_Resize);
            this.Controls.SetChildIndex(this.dgvKorcsoport, 0);
            this.Controls.SetChildIndex(this.cim, 0);
            this.Controls.SetChildIndex(this.txPnrid, 0);
            this.Controls.SetChildIndex(this.label19, 0);
            this.Controls.SetChildIndex(this.label23, 0);
            this.Controls.SetChildIndex(this.label24, 0);
            this.Controls.SetChildIndex(this.txAdoszam, 0);
            this.Controls.SetChildIndex(this.txAdoazon, 0);
            this.Controls.SetChildIndex(this.label18, 0);
            this.Controls.SetChildIndex(this.txMegnev, 0);
            this.Controls.SetChildIndex(this.bKivalaszt, 0);
            ((System.ComponentModel.ISupportInitialize)(this.dgvKorcsoport)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.spKorcsoportokBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.vPFSDataSet)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvKorcsoport;
        private VPFSDataSet vPFSDataSet;
        private System.Windows.Forms.BindingSource spKorcsoportokBindingSource;
        private VPFSDataSetTableAdapters.spKorcsoportokTableAdapter spKorcsoportokTableAdapter;
        private System.Windows.Forms.Label cim;
        private System.Windows.Forms.TextBox txAdoazon;
        private System.Windows.Forms.TextBox txAdoszam;
        private System.Windows.Forms.Label label24;
        private System.Windows.Forms.Label label23;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.TextBox txPnrid;
        private System.Windows.Forms.TextBox txMegnev;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.Button bKivalaszt;
    }
}
