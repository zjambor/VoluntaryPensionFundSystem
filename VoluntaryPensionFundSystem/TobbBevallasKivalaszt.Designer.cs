namespace VoluntaryPensionFundSystem
{
    partial class TobbBevallasKivalaszt
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
            this.bKivalaszt = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.dgvBevKiv = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.dgvBevKiv)).BeginInit();
            this.SuspendLayout();
            // 
            // bKivalaszt
            // 
            this.bKivalaszt.AutoSize = true;
            this.bKivalaszt.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.bKivalaszt.Location = new System.Drawing.Point(409, 301);
            this.bKivalaszt.Name = "bKivalaszt";
            this.bKivalaszt.Size = new System.Drawing.Size(292, 29);
            this.bKivalaszt.TabIndex = 109;
            this.bKivalaszt.Text = "Kiválaszt";
            this.bKivalaszt.UseVisualStyleBackColor = true;
            this.bKivalaszt.Click += new System.EventHandler(this.bKivalaszt_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label2.Location = new System.Drawing.Point(436, 72);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(238, 29);
            this.label2.TabIndex = 108;
            this.label2.Text = "Bevallás kiválasztása";
            // 
            // dgvBevKiv
            // 
            this.dgvBevKiv.AllowUserToAddRows = false;
            this.dgvBevKiv.AllowUserToDeleteRows = false;
            this.dgvBevKiv.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dgvBevKiv.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvBevKiv.Location = new System.Drawing.Point(25, 120);
            this.dgvBevKiv.MultiSelect = false;
            this.dgvBevKiv.Name = "dgvBevKiv";
            this.dgvBevKiv.ReadOnly = true;
            this.dgvBevKiv.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.dgvBevKiv.ShowEditingIcon = false;
            this.dgvBevKiv.Size = new System.Drawing.Size(1061, 164);
            this.dgvBevKiv.TabIndex = 107;
            this.dgvBevKiv.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dgvBevKiv_CellFormatting);
            this.dgvBevKiv.DoubleClick += new System.EventHandler(this.dgvBevKiv_DoubleClick);
            this.dgvBevKiv.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dgvBevKiv_KeyDown);
            this.dgvBevKiv.MouseMove += new System.Windows.Forms.MouseEventHandler(this.dgvBevKiv_MouseMove);
            // 
            // TobbBevallasKivalaszt
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.ClientSize = new System.Drawing.Size(1110, 354);
            this.Controls.Add(this.bKivalaszt);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.dgvBevKiv);
            this.Name = "TobbBevallasKivalaszt";
            this.Text = "Bevallás kiválasztása";
            this.Controls.SetChildIndex(this.dgvBevKiv, 0);
            this.Controls.SetChildIndex(this.label2, 0);
            this.Controls.SetChildIndex(this.bKivalaszt, 0);
            ((System.ComponentModel.ISupportInitialize)(this.dgvBevKiv)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button bKivalaszt;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DataGridView dgvBevKiv;
    }
}
