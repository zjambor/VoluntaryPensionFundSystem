namespace VoluntaryPensionFundSystem
{
    partial class FokonyviTetelekBong
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
            this.label2 = new System.Windows.Forms.Label();
            this.dgvFtl = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.dgvFtl)).BeginInit();
            this.SuspendLayout();
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label2.Location = new System.Drawing.Point(419, 77);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(325, 29);
            this.label2.TabIndex = 110;
            this.label2.Text = "Főkönyvi tételek böngészése";
            // 
            // dgvFtl
            // 
            this.dgvFtl.AllowUserToAddRows = false;
            this.dgvFtl.AllowUserToDeleteRows = false;
            this.dgvFtl.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dgvFtl.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvFtl.Location = new System.Drawing.Point(12, 136);
            this.dgvFtl.MultiSelect = false;
            this.dgvFtl.Name = "dgvFtl";
            this.dgvFtl.ReadOnly = true;
            this.dgvFtl.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.dgvFtl.ShowEditingIcon = false;
            this.dgvFtl.Size = new System.Drawing.Size(1138, 374);
            this.dgvFtl.TabIndex = 109;
            this.dgvFtl.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dgvFtl_CellFormatting);
            this.dgvFtl.MouseMove += new System.Windows.Forms.MouseEventHandler(this.dgvFtl_MouseMove);
            // 
            // FokonyviTetelekBong
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.ClientSize = new System.Drawing.Size(1162, 522);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.dgvFtl);
            this.Name = "FokonyviTetelekBong";
            this.Text = "Főkönyvi tételek böngészése";
            this.Resize += new System.EventHandler(this.FokonyviTetelekBong_Resize);
            this.Controls.SetChildIndex(this.dgvFtl, 0);
            this.Controls.SetChildIndex(this.label2, 0);
            ((System.ComponentModel.ISupportInitialize)(this.dgvFtl)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DataGridView dgvFtl;
    }
}
