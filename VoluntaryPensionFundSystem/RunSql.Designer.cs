namespace VoluntaryPensionFundSystem
{
    partial class RunSql
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
            this.rtSql = new System.Windows.Forms.RichTextBox();
            this.bRun = new System.Windows.Forms.Button();
            this.dgvResults = new System.Windows.Forms.DataGridView();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dgvResults)).BeginInit();
            this.SuspendLayout();
            // 
            // rtSql
            // 
            this.rtSql.AcceptsTab = true;
            this.rtSql.Font = new System.Drawing.Font("Courier New", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.rtSql.Location = new System.Drawing.Point(40, 138);
            this.rtSql.Name = "rtSql";
            this.rtSql.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.ForcedHorizontal;
            this.rtSql.Size = new System.Drawing.Size(914, 353);
            this.rtSql.TabIndex = 3;
            this.rtSql.Text = "";
            // 
            // bRun
            // 
            this.bRun.AutoSize = true;
            this.bRun.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.bRun.Location = new System.Drawing.Point(40, 94);
            this.bRun.Name = "bRun";
            this.bRun.Size = new System.Drawing.Size(75, 29);
            this.bRun.TabIndex = 4;
            this.bRun.Text = "Futtatás";
            this.bRun.UseVisualStyleBackColor = true;
            this.bRun.Click += new System.EventHandler(this.bRun_Click);
            // 
            // dgvResults
            // 
            this.dgvResults.AllowUserToAddRows = false;
            this.dgvResults.AllowUserToDeleteRows = false;
            this.dgvResults.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dgvResults.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvResults.Location = new System.Drawing.Point(40, 138);
            this.dgvResults.MultiSelect = false;
            this.dgvResults.Name = "dgvResults";
            this.dgvResults.ReadOnly = true;
            this.dgvResults.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.dgvResults.ShowEditingIcon = false;
            this.dgvResults.Size = new System.Drawing.Size(914, 353);
            this.dgvResults.TabIndex = 175;
            this.dgvResults.Visible = false;
            this.dgvResults.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.dgvResults_DataError);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label2.Location = new System.Drawing.Point(364, 72);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(280, 29);
            this.label2.TabIndex = 176;
            this.label2.Text = "SQL lekérdezés futtatása";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(121, 101);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(256, 17);
            this.label1.TabIndex = 180;
            this.label1.Text = "Lekérdezés folyamatban, kérem várjon!";
            // 
            // RunSql
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.ClientSize = new System.Drawing.Size(1008, 551);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.dgvResults);
            this.Controls.Add(this.bRun);
            this.Controls.Add(this.rtSql);
            this.Name = "RunSql";
            this.Text = "SQL lekérdezés futtatása";
            this.Controls.SetChildIndex(this.rtSql, 0);
            this.Controls.SetChildIndex(this.bRun, 0);
            this.Controls.SetChildIndex(this.dgvResults, 0);
            this.Controls.SetChildIndex(this.label2, 0);
            this.Controls.SetChildIndex(this.label1, 0);
            ((System.ComponentModel.ISupportInitialize)(this.dgvResults)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RichTextBox rtSql;
        private System.Windows.Forms.Button bRun;
        private System.Windows.Forms.DataGridView dgvResults;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
    }
}
