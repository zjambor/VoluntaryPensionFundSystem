namespace VoluntaryPensionFundSystem
{
    partial class FoglalkoztatoMunkaviszonyok
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
            this.dgvMunk = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.dgvMunk)).BeginInit();
            this.SuspendLayout();
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label2.Location = new System.Drawing.Point(121, 73);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(507, 29);
            this.label2.TabIndex = 112;
            this.label2.Text = "Foglalkoztatóhoz kapcsolódó munkaviszonyok";
            // 
            // dgvMunk
            // 
            this.dgvMunk.AllowUserToAddRows = false;
            this.dgvMunk.AllowUserToDeleteRows = false;
            this.dgvMunk.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dgvMunk.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvMunk.Location = new System.Drawing.Point(12, 124);
            this.dgvMunk.MultiSelect = false;
            this.dgvMunk.Name = "dgvMunk";
            this.dgvMunk.ReadOnly = true;
            this.dgvMunk.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.dgvMunk.ShowEditingIcon = false;
            this.dgvMunk.Size = new System.Drawing.Size(724, 386);
            this.dgvMunk.TabIndex = 111;
            this.dgvMunk.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dgvMunk_CellFormatting);
            this.dgvMunk.MouseMove += new System.Windows.Forms.MouseEventHandler(this.dgvMunk_MouseMove);
            // 
            // FoglalkoztatoMunkaviszonyok
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.ClientSize = new System.Drawing.Size(748, 522);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.dgvMunk);
            this.Name = "FoglalkoztatoMunkaviszonyok";
            this.Text = "Foglalkoztatóhoz kapcsolódó munkaviszonyok";
            this.Resize += new System.EventHandler(this.FoglalkoztatoMunkaviszonyok_Resize);
            this.Controls.SetChildIndex(this.dgvMunk, 0);
            this.Controls.SetChildIndex(this.label2, 0);
            ((System.ComponentModel.ISupportInitialize)(this.dgvMunk)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DataGridView dgvMunk;
    }
}
