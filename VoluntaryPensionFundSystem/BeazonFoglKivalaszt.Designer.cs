namespace VoluntaryPensionFundSystem
{
    partial class BeazonFoglKivalaszt
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
            this.dgvBankKiv = new System.Windows.Forms.DataGridView();
            this.bKivalaszt = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvBankKiv)).BeginInit();
            this.SuspendLayout();
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label2.Location = new System.Drawing.Point(293, 71);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(263, 29);
            this.label2.TabIndex = 105;
            this.label2.Text = "Munkáltató kiválasztása";
            // 
            // dgvBankKiv
            // 
            this.dgvBankKiv.AllowUserToAddRows = false;
            this.dgvBankKiv.AllowUserToDeleteRows = false;
            this.dgvBankKiv.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dgvBankKiv.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvBankKiv.Location = new System.Drawing.Point(12, 119);
            this.dgvBankKiv.MultiSelect = false;
            this.dgvBankKiv.Name = "dgvBankKiv";
            this.dgvBankKiv.ReadOnly = true;
            this.dgvBankKiv.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.dgvBankKiv.ShowEditingIcon = false;
            this.dgvBankKiv.Size = new System.Drawing.Size(824, 190);
            this.dgvBankKiv.TabIndex = 104;
            this.dgvBankKiv.SelectionChanged += new System.EventHandler(this.dgvBankKiv_SelectionChanged);
            this.dgvBankKiv.DoubleClick += new System.EventHandler(this.dgvBankKiv_DoubleClick);
            this.dgvBankKiv.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dgvBankKiv_KeyDown);
            this.dgvBankKiv.MouseMove += new System.Windows.Forms.MouseEventHandler(this.dgvBankKiv_MouseMove);
            // 
            // bKivalaszt
            // 
            this.bKivalaszt.AutoSize = true;
            this.bKivalaszt.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.bKivalaszt.Location = new System.Drawing.Point(348, 315);
            this.bKivalaszt.Name = "bKivalaszt";
            this.bKivalaszt.Size = new System.Drawing.Size(153, 29);
            this.bKivalaszt.TabIndex = 106;
            this.bKivalaszt.Text = "Kiválaszt";
            this.bKivalaszt.UseVisualStyleBackColor = true;
            this.bKivalaszt.Click += new System.EventHandler(this.bKivalaszt_Click);
            // 
            // BeazonFoglKivalaszt
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(848, 352);
            this.Controls.Add(this.bKivalaszt);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.dgvBankKiv);
            this.Name = "BeazonFoglKivalaszt";
            this.Text = "Munkáltató kiválasztása";
            this.Resize += new System.EventHandler(this.BeazonFoglKivalaszt_Resize);
            this.Controls.SetChildIndex(this.dgvBankKiv, 0);
            this.Controls.SetChildIndex(this.label2, 0);
            this.Controls.SetChildIndex(this.bKivalaszt, 0);
            ((System.ComponentModel.ISupportInitialize)(this.dgvBankKiv)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DataGridView dgvBankKiv;
        private System.Windows.Forms.Button bKivalaszt;
    }
}