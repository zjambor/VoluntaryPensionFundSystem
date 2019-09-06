namespace VoluntaryPensionFundSystem
{
    partial class UjPortfolio
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
            this.txMegjegyzes = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.datKezdete = new System.Windows.Forms.DateTimePicker();
            this.txNev = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txLeiras = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txTipus = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // txMegjegyzes
            // 
            this.txMegjegyzes.Location = new System.Drawing.Point(47, 234);
            this.txMegjegyzes.MaxLength = 255;
            this.txMegjegyzes.Name = "txMegjegyzes";
            this.txMegjegyzes.Size = new System.Drawing.Size(746, 23);
            this.txMegjegyzes.TabIndex = 4;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(46, 214);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(83, 17);
            this.label7.TabIndex = 126;
            this.label7.Text = "Megjegyzés";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(686, 123);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(87, 17);
            this.label5.TabIndex = 124;
            this.label5.Text = "Érv. kezdete";
            // 
            // datKezdete
            // 
            this.datKezdete.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.datKezdete.Location = new System.Drawing.Point(685, 143);
            this.datKezdete.Name = "datKezdete";
            this.datKezdete.Size = new System.Drawing.Size(108, 23);
            this.datKezdete.TabIndex = 2;
            // 
            // txNev
            // 
            this.txNev.Location = new System.Drawing.Point(99, 143);
            this.txNev.MaxLength = 80;
            this.txNev.Name = "txNev";
            this.txNev.Size = new System.Drawing.Size(560, 23);
            this.txNev.TabIndex = 1;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(98, 123);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(33, 17);
            this.label4.TabIndex = 120;
            this.label4.Text = "Név";
            // 
            // txLeiras
            // 
            this.txLeiras.Location = new System.Drawing.Point(47, 188);
            this.txLeiras.MaxLength = 255;
            this.txLeiras.Name = "txLeiras";
            this.txLeiras.Size = new System.Drawing.Size(612, 23);
            this.txLeiras.TabIndex = 3;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(46, 168);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(47, 17);
            this.label3.TabIndex = 118;
            this.label3.Text = "Leírás";
            // 
            // txTipus
            // 
            this.txTipus.Location = new System.Drawing.Point(47, 143);
            this.txTipus.MaxLength = 1;
            this.txTipus.Name = "txTipus";
            this.txTipus.Size = new System.Drawing.Size(46, 23);
            this.txTipus.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(46, 123);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(43, 17);
            this.label1.TabIndex = 116;
            this.label1.Text = "Típus";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label2.Location = new System.Drawing.Point(367, 75);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(132, 29);
            this.label2.TabIndex = 130;
            this.label2.Text = "Új Portfólió";
            // 
            // UjPortfolio
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(866, 317);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txMegjegyzes);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.datKezdete);
            this.Controls.Add(this.txNev);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txLeiras);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txTipus);
            this.Controls.Add(this.label1);
            this.Name = "UjPortfolio";
            this.Text = "Új Portfolió";
            this.Controls.SetChildIndex(this.label1, 0);
            this.Controls.SetChildIndex(this.txTipus, 0);
            this.Controls.SetChildIndex(this.label3, 0);
            this.Controls.SetChildIndex(this.txLeiras, 0);
            this.Controls.SetChildIndex(this.label4, 0);
            this.Controls.SetChildIndex(this.txNev, 0);
            this.Controls.SetChildIndex(this.datKezdete, 0);
            this.Controls.SetChildIndex(this.label5, 0);
            this.Controls.SetChildIndex(this.label7, 0);
            this.Controls.SetChildIndex(this.txMegjegyzes, 0);
            this.Controls.SetChildIndex(this.label2, 0);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txMegjegyzes;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.DateTimePicker datKezdete;
        private System.Windows.Forms.TextBox txNev;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txLeiras;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txTipus;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
    }
}