namespace VoluntaryPensionFundSystem
{
    partial class AnalitikaHibak
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
            this.dgvAnalitikaHibak = new System.Windows.Forms.DataGridView();
            this.label2 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dgvAnalitikaHibak)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvAnalitikaHibak
            // 
            this.dgvAnalitikaHibak.AllowUserToAddRows = false;
            this.dgvAnalitikaHibak.AllowUserToDeleteRows = false;
            this.dgvAnalitikaHibak.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dgvAnalitikaHibak.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvAnalitikaHibak.Location = new System.Drawing.Point(25, 133);
            this.dgvAnalitikaHibak.Margin = new System.Windows.Forms.Padding(4);
            this.dgvAnalitikaHibak.MultiSelect = false;
            this.dgvAnalitikaHibak.Name = "dgvAnalitikaHibak";
            this.dgvAnalitikaHibak.ReadOnly = true;
            this.dgvAnalitikaHibak.Size = new System.Drawing.Size(1070, 468);
            this.dgvAnalitikaHibak.TabIndex = 0;
            this.dgvAnalitikaHibak.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dgvAnalitikaHibak_CellFormatting);
            this.dgvAnalitikaHibak.MouseMove += new System.Windows.Forms.MouseEventHandler(this.dgvAnalitikaHibak_MouseMove);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label2.Location = new System.Drawing.Point(480, 77);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(168, 29);
            this.label2.TabIndex = 99;
            this.label2.Text = "Bevallás hibák";
            // 
            // AnalitikaHibak
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1128, 650);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.dgvAnalitikaHibak);
            this.Margin = new System.Windows.Forms.Padding(5);
            this.Name = "AnalitikaHibak";
            this.Text = "Analitika hibák";
            this.Controls.SetChildIndex(this.dgvAnalitikaHibak, 0);
            this.Controls.SetChildIndex(this.label2, 0);
            ((System.ComponentModel.ISupportInitialize)(this.dgvAnalitikaHibak)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvAnalitikaHibak;
        private System.Windows.Forms.Label label2;
    }
}