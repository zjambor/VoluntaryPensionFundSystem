namespace VoluntaryPensionFundSystem
{
    partial class MunkFolyoszamlaReportIndito
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
            this.label4 = new System.Windows.Forms.Label();
            this.bIndit = new System.Windows.Forms.Button();
            this.dtVege = new System.Windows.Forms.DateTimePicker();
            this.dtKezdete = new System.Windows.Forms.DateTimePicker();
            this.txPnrid = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.bFoglKeres = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label4.Location = new System.Drawing.Point(6, 79);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(588, 29);
            this.label4.TabIndex = 115;
            this.label4.Text = "Munkáltatói folyószámla-egyenleg értesítő nyomtatása";
            // 
            // bIndit
            // 
            this.bIndit.AutoSize = true;
            this.bIndit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.bIndit.Location = new System.Drawing.Point(255, 254);
            this.bIndit.Name = "bIndit";
            this.bIndit.Size = new System.Drawing.Size(111, 29);
            this.bIndit.TabIndex = 114;
            this.bIndit.Text = "Riport indítása";
            this.bIndit.UseVisualStyleBackColor = true;
            this.bIndit.Click += new System.EventHandler(this.bIndit_Click);
            // 
            // dtVege
            // 
            this.dtVege.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtVege.Location = new System.Drawing.Point(398, 194);
            this.dtVege.Name = "dtVege";
            this.dtVege.Size = new System.Drawing.Size(100, 23);
            this.dtVege.TabIndex = 113;
            // 
            // dtKezdete
            // 
            this.dtKezdete.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtKezdete.Location = new System.Drawing.Point(398, 171);
            this.dtKezdete.Name = "dtKezdete";
            this.dtKezdete.Size = new System.Drawing.Size(100, 23);
            this.dtKezdete.TabIndex = 112;
            // 
            // txPnrid
            // 
            this.txPnrid.Location = new System.Drawing.Point(398, 148);
            this.txPnrid.Name = "txPnrid";
            this.txPnrid.Size = new System.Drawing.Size(100, 23);
            this.txPnrid.TabIndex = 111;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(273, 196);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(84, 17);
            this.label3.TabIndex = 110;
            this.label3.Text = "Vége dátum";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(273, 175);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(91, 17);
            this.label2.TabIndex = 109;
            this.label2.Text = "Kezdő dátum";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(273, 154);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(70, 17);
            this.label1.TabIndex = 108;
            this.label1.Text = "Partner id";
            // 
            // bFoglKeres
            // 
            this.bFoglKeres.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.bFoglKeres.Location = new System.Drawing.Point(103, 152);
            this.bFoglKeres.Name = "bFoglKeres";
            this.bFoglKeres.Size = new System.Drawing.Size(148, 65);
            this.bFoglKeres.TabIndex = 116;
            this.bFoglKeres.Text = "Munkáltató keresése és kiválasztása";
            this.bFoglKeres.UseVisualStyleBackColor = true;
            this.bFoglKeres.Click += new System.EventHandler(this.bFoglKeres_Click);
            // 
            // MunkFolyoszamlaReportIndito
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.ClientSize = new System.Drawing.Size(600, 341);
            this.Controls.Add(this.bFoglKeres);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.bIndit);
            this.Controls.Add(this.dtVege);
            this.Controls.Add(this.dtKezdete);
            this.Controls.Add(this.txPnrid);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "MunkFolyoszamlaReportIndito";
            this.Text = "Munkáltatói folyószámla-egyenleg értesítő nyomtatása";
            this.Controls.SetChildIndex(this.label1, 0);
            this.Controls.SetChildIndex(this.label2, 0);
            this.Controls.SetChildIndex(this.label3, 0);
            this.Controls.SetChildIndex(this.txPnrid, 0);
            this.Controls.SetChildIndex(this.dtKezdete, 0);
            this.Controls.SetChildIndex(this.dtVege, 0);
            this.Controls.SetChildIndex(this.bIndit, 0);
            this.Controls.SetChildIndex(this.label4, 0);
            this.Controls.SetChildIndex(this.bFoglKeres, 0);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button bIndit;
        private System.Windows.Forms.DateTimePicker dtVege;
        private System.Windows.Forms.DateTimePicker dtKezdete;
        private System.Windows.Forms.TextBox txPnrid;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button bFoglKeres;
    }
}
