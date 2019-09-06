namespace VoluntaryPensionFundSystem
{
    partial class TagiEgyeniSzamlaReportIndito
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.txPnrid = new System.Windows.Forms.TextBox();
            this.dtKezdete = new System.Windows.Forms.DateTimePicker();
            this.dtVege = new System.Windows.Forms.DateTimePicker();
            this.bIndit = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.bKeres = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(267, 147);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(70, 17);
            this.label1.TabIndex = 3;
            this.label1.Text = "Partner id";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(267, 171);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(91, 17);
            this.label2.TabIndex = 4;
            this.label2.Text = "Kezdő dátum";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(267, 192);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(84, 17);
            this.label3.TabIndex = 5;
            this.label3.Text = "Vége dátum";
            // 
            // txPnrid
            // 
            this.txPnrid.Location = new System.Drawing.Point(365, 144);
            this.txPnrid.Name = "txPnrid";
            this.txPnrid.Size = new System.Drawing.Size(100, 23);
            this.txPnrid.TabIndex = 6;
            // 
            // dtKezdete
            // 
            this.dtKezdete.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtKezdete.Location = new System.Drawing.Point(365, 167);
            this.dtKezdete.Name = "dtKezdete";
            this.dtKezdete.Size = new System.Drawing.Size(100, 23);
            this.dtKezdete.TabIndex = 7;
            // 
            // dtVege
            // 
            this.dtVege.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtVege.Location = new System.Drawing.Point(365, 190);
            this.dtVege.Name = "dtVege";
            this.dtVege.Size = new System.Drawing.Size(100, 23);
            this.dtVege.TabIndex = 8;
            // 
            // bIndit
            // 
            this.bIndit.AutoSize = true;
            this.bIndit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.bIndit.Location = new System.Drawing.Point(249, 230);
            this.bIndit.Name = "bIndit";
            this.bIndit.Size = new System.Drawing.Size(75, 29);
            this.bIndit.TabIndex = 9;
            this.bIndit.Text = "Indít";
            this.bIndit.UseVisualStyleBackColor = true;
            this.bIndit.Click += new System.EventHandler(this.bIndit_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label4.Location = new System.Drawing.Point(16, 73);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(540, 29);
            this.label4.TabIndex = 107;
            this.label4.Text = "Egyéni folyószámla-egyenleg értesítő nyomtatása";
            // 
            // bKeres
            // 
            this.bKeres.AutoSize = true;
            this.bKeres.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.bKeres.Location = new System.Drawing.Point(40, 151);
            this.bKeres.Name = "bKeres";
            this.bKeres.Size = new System.Drawing.Size(207, 58);
            this.bKeres.TabIndex = 108;
            this.bKeres.Text = "Tag keresése és kiválasztása";
            this.bKeres.UseVisualStyleBackColor = true;
            this.bKeres.Click += new System.EventHandler(this.bKeres_Click);
            // 
            // TagiEgyeniSzamlaReportIndito
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.ClientSize = new System.Drawing.Size(572, 296);
            this.Controls.Add(this.bKeres);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.bIndit);
            this.Controls.Add(this.dtVege);
            this.Controls.Add(this.dtKezdete);
            this.Controls.Add(this.txPnrid);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "TagiEgyeniSzamlaReportIndito";
            this.Text = "Egyéni folyószámla-egyenleg értesítő nyomtatása";
            this.Controls.SetChildIndex(this.label1, 0);
            this.Controls.SetChildIndex(this.label2, 0);
            this.Controls.SetChildIndex(this.label3, 0);
            this.Controls.SetChildIndex(this.txPnrid, 0);
            this.Controls.SetChildIndex(this.dtKezdete, 0);
            this.Controls.SetChildIndex(this.dtVege, 0);
            this.Controls.SetChildIndex(this.bIndit, 0);
            this.Controls.SetChildIndex(this.label4, 0);
            this.Controls.SetChildIndex(this.bKeres, 0);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txPnrid;
        private System.Windows.Forms.DateTimePicker dtKezdete;
        private System.Windows.Forms.DateTimePicker dtVege;
        private System.Windows.Forms.Button bIndit;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button bKeres;
    }
}
