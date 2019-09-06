namespace VoluntaryPensionFundSystem
{
    partial class UjJelszo
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
            this.txJelszo = new System.Windows.Forms.TextBox();
            this.txTeljesnev = new System.Windows.Forms.TextBox();
            this.txFhnev = new System.Windows.Forms.TextBox();
            this.txFhoid = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.txJelszo2 = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.bOK = new System.Windows.Forms.Button();
            this.bCancel = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // txJelszo
            // 
            this.txJelszo.Location = new System.Drawing.Point(171, 227);
            this.txJelszo.MaxLength = 24;
            this.txJelszo.Name = "txJelszo";
            this.txJelszo.PasswordChar = '*';
            this.txJelszo.Size = new System.Drawing.Size(176, 23);
            this.txJelszo.TabIndex = 0;
            this.txJelszo.Leave += new System.EventHandler(this.txJelszo_Leave);
            // 
            // txTeljesnev
            // 
            this.txTeljesnev.Location = new System.Drawing.Point(171, 197);
            this.txTeljesnev.Name = "txTeljesnev";
            this.txTeljesnev.ReadOnly = true;
            this.txTeljesnev.Size = new System.Drawing.Size(263, 23);
            this.txTeljesnev.TabIndex = 59;
            // 
            // txFhnev
            // 
            this.txFhnev.Location = new System.Drawing.Point(171, 167);
            this.txFhnev.Name = "txFhnev";
            this.txFhnev.ReadOnly = true;
            this.txFhnev.Size = new System.Drawing.Size(263, 23);
            this.txFhnev.TabIndex = 58;
            // 
            // txFhoid
            // 
            this.txFhoid.Location = new System.Drawing.Point(171, 137);
            this.txFhoid.MaxLength = 8;
            this.txFhoid.Name = "txFhoid";
            this.txFhoid.ReadOnly = true;
            this.txFhoid.Size = new System.Drawing.Size(100, 23);
            this.txFhoid.TabIndex = 53;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(26, 230);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(48, 17);
            this.label4.TabIndex = 57;
            this.label4.Text = "Jelszó";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(26, 200);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(73, 17);
            this.label3.TabIndex = 56;
            this.label3.Text = "Teljes név";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(26, 170);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(107, 17);
            this.label2.TabIndex = 55;
            this.label2.Text = "Felhasználónév";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(26, 140);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(47, 17);
            this.label1.TabIndex = 54;
            this.label1.Text = "Fho id";
            // 
            // txJelszo2
            // 
            this.txJelszo2.Location = new System.Drawing.Point(171, 256);
            this.txJelszo2.MaxLength = 24;
            this.txJelszo2.Name = "txJelszo2";
            this.txJelszo2.PasswordChar = '*';
            this.txJelszo2.Size = new System.Drawing.Size(176, 23);
            this.txJelszo2.TabIndex = 1;
            this.txJelszo2.Leave += new System.EventHandler(this.txJelszo2_Leave);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(26, 259);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(137, 17);
            this.label5.TabIndex = 61;
            this.label5.Text = "Jelszó megerősítése";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label13.Location = new System.Drawing.Point(174, 79);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(201, 29);
            this.label13.TabIndex = 63;
            this.label13.Text = "Jelszó megadása";
            // 
            // bOK
            // 
            this.bOK.AutoSize = true;
            this.bOK.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.bOK.Location = new System.Drawing.Point(178, 310);
            this.bOK.Name = "bOK";
            this.bOK.Size = new System.Drawing.Size(75, 29);
            this.bOK.TabIndex = 2;
            this.bOK.Text = "OK";
            this.bOK.UseVisualStyleBackColor = true;
            this.bOK.Click += new System.EventHandler(this.bOK_Click);
            // 
            // bCancel
            // 
            this.bCancel.AutoSize = true;
            this.bCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.bCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.bCancel.Location = new System.Drawing.Point(295, 310);
            this.bCancel.Name = "bCancel";
            this.bCancel.Size = new System.Drawing.Size(75, 29);
            this.bCancel.TabIndex = 65;
            this.bCancel.Text = "Mégse";
            this.bCancel.UseVisualStyleBackColor = true;
            this.bCancel.Click += new System.EventHandler(this.bCancel_Click);
            // 
            // label6
            // 
            this.label6.ForeColor = System.Drawing.Color.Red;
            this.label6.Location = new System.Drawing.Point(353, 230);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(183, 20);
            this.label6.TabIndex = 66;
            this.label6.Text = "label6";
            this.label6.Visible = false;
            // 
            // label7
            // 
            this.label7.ForeColor = System.Drawing.Color.Red;
            this.label7.Location = new System.Drawing.Point(353, 262);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(183, 44);
            this.label7.TabIndex = 67;
            this.label7.Text = "label7";
            this.label7.Visible = false;
            // 
            // UjJelszo
            // 
            this.AcceptButton = this.bOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.CancelButton = this.bCancel;
            this.ClientSize = new System.Drawing.Size(548, 376);
            this.ControlBox = false;
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
            this.Controls.Add(this.txFhoid);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "UjJelszo";
            this.Text = "Jelszó megadása";
            this.Load += new System.EventHandler(this.UjJelszo_Load);
            this.Controls.SetChildIndex(this.label1, 0);
            this.Controls.SetChildIndex(this.label2, 0);
            this.Controls.SetChildIndex(this.label3, 0);
            this.Controls.SetChildIndex(this.label4, 0);
            this.Controls.SetChildIndex(this.txFhoid, 0);
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
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txJelszo;
        private System.Windows.Forms.TextBox txTeljesnev;
        private System.Windows.Forms.TextBox txFhnev;
        private System.Windows.Forms.TextBox txFhoid;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txJelszo2;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Button bOK;
        private System.Windows.Forms.Button bCancel;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
    }
}
