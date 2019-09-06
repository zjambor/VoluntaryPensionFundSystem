namespace VoluntaryPensionFundSystem
{
    partial class Newuser
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
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.bCancel = new System.Windows.Forms.Button();
            this.bOK = new System.Windows.Forms.Button();
            this.label13 = new System.Windows.Forms.Label();
            this.txJelszo2 = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txJelszo = new System.Windows.Forms.TextBox();
            this.txTeljesnev = new System.Windows.Forms.TextBox();
            this.txFhnev = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label7
            // 
            this.label7.ForeColor = System.Drawing.Color.Red;
            this.label7.Location = new System.Drawing.Point(365, 235);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(183, 44);
            this.label7.TabIndex = 82;
            this.label7.Text = "label7";
            this.label7.Visible = false;
            // 
            // label6
            // 
            this.label6.ForeColor = System.Drawing.Color.Red;
            this.label6.Location = new System.Drawing.Point(365, 203);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(183, 20);
            this.label6.TabIndex = 81;
            this.label6.Text = "label6";
            this.label6.Visible = false;
            // 
            // bCancel
            // 
            this.bCancel.AutoSize = true;
            this.bCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.bCancel.Location = new System.Drawing.Point(356, 283);
            this.bCancel.Name = "bCancel";
            this.bCancel.Size = new System.Drawing.Size(75, 27);
            this.bCancel.TabIndex = 80;
            this.bCancel.Text = "Mégse";
            this.bCancel.UseVisualStyleBackColor = true;
            this.bCancel.Click += new System.EventHandler(this.bCancel_Click);
            // 
            // bOK
            // 
            this.bOK.AutoSize = true;
            this.bOK.Location = new System.Drawing.Point(239, 283);
            this.bOK.Name = "bOK";
            this.bOK.Size = new System.Drawing.Size(75, 27);
            this.bOK.TabIndex = 4;
            this.bOK.Text = "OK";
            this.bOK.UseVisualStyleBackColor = true;
            this.bOK.Click += new System.EventHandler(this.bOK_Click);
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label13.Location = new System.Drawing.Point(254, 82);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(163, 29);
            this.label13.TabIndex = 79;
            this.label13.Text = "Új felhasználó";
            // 
            // txJelszo2
            // 
            this.txJelszo2.Location = new System.Drawing.Point(183, 229);
            this.txJelszo2.MaxLength = 24;
            this.txJelszo2.Name = "txJelszo2";
            this.txJelszo2.PasswordChar = '*';
            this.txJelszo2.Size = new System.Drawing.Size(176, 23);
            this.txJelszo2.TabIndex = 3;
            this.txJelszo2.Leave += new System.EventHandler(this.txJelszo2_Leave);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(38, 232);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(137, 17);
            this.label5.TabIndex = 78;
            this.label5.Text = "Jelszó megerősítése";
            // 
            // txJelszo
            // 
            this.txJelszo.Location = new System.Drawing.Point(183, 200);
            this.txJelszo.MaxLength = 24;
            this.txJelszo.Name = "txJelszo";
            this.txJelszo.PasswordChar = '*';
            this.txJelszo.Size = new System.Drawing.Size(176, 23);
            this.txJelszo.TabIndex = 2;
            this.txJelszo.Leave += new System.EventHandler(this.txJelszo_Leave);
            // 
            // txTeljesnev
            // 
            this.txTeljesnev.Location = new System.Drawing.Point(183, 170);
            this.txTeljesnev.Name = "txTeljesnev";
            this.txTeljesnev.Size = new System.Drawing.Size(263, 23);
            this.txTeljesnev.TabIndex = 1;
            this.txTeljesnev.Leave += new System.EventHandler(this.txTeljesnev_Leave);
            // 
            // txFhnev
            // 
            this.txFhnev.Location = new System.Drawing.Point(183, 140);
            this.txFhnev.Name = "txFhnev";
            this.txFhnev.Size = new System.Drawing.Size(263, 23);
            this.txFhnev.TabIndex = 0;
            this.txFhnev.Leave += new System.EventHandler(this.txFhnev_Leave);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(38, 203);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(48, 17);
            this.label4.TabIndex = 75;
            this.label4.Text = "Jelszó";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(38, 173);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(73, 17);
            this.label3.TabIndex = 74;
            this.label3.Text = "Teljes név";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(38, 143);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(107, 17);
            this.label2.TabIndex = 73;
            this.label2.Text = "Felhasználónév";
            // 
            // label8
            // 
            this.label8.ForeColor = System.Drawing.Color.Red;
            this.label8.Location = new System.Drawing.Point(452, 143);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(183, 20);
            this.label8.TabIndex = 83;
            this.label8.Text = "label8";
            this.label8.Visible = false;
            // 
            // label9
            // 
            this.label9.ForeColor = System.Drawing.Color.Red;
            this.label9.Location = new System.Drawing.Point(452, 170);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(183, 20);
            this.label9.TabIndex = 84;
            this.label9.Text = "label9";
            this.label9.Visible = false;
            // 
            // Newuser
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.ClientSize = new System.Drawing.Size(670, 359);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.bCancel);
            this.Controls.Add(this.bOK);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.txJelszo2);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.txJelszo);
            this.Controls.Add(this.txTeljesnev);
            this.Controls.Add(this.txFhnev);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Newuser";
            this.Text = "Új felhasználó";
            this.Load += new System.EventHandler(this.Newuser_Load);
            this.Controls.SetChildIndex(this.label2, 0);
            this.Controls.SetChildIndex(this.label3, 0);
            this.Controls.SetChildIndex(this.label4, 0);
            this.Controls.SetChildIndex(this.txFhnev, 0);
            this.Controls.SetChildIndex(this.txTeljesnev, 0);
            this.Controls.SetChildIndex(this.txJelszo, 0);
            this.Controls.SetChildIndex(this.label5, 0);
            this.Controls.SetChildIndex(this.txJelszo2, 0);
            this.Controls.SetChildIndex(this.label13, 0);
            this.Controls.SetChildIndex(this.bOK, 0);
            this.Controls.SetChildIndex(this.bCancel, 0);
            this.Controls.SetChildIndex(this.label6, 0);
            this.Controls.SetChildIndex(this.label7, 0);
            this.Controls.SetChildIndex(this.label8, 0);
            this.Controls.SetChildIndex(this.label9, 0);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button bCancel;
        private System.Windows.Forms.Button bOK;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.TextBox txJelszo2;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txJelszo;
        private System.Windows.Forms.TextBox txTeljesnev;
        private System.Windows.Forms.TextBox txFhnev;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
    }
}
