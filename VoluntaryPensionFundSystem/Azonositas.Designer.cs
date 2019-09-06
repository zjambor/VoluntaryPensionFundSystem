namespace VoluntaryPensionFundSystem
{
    partial class Azonositas
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
            this.bCancel = new System.Windows.Forms.Button();
            this.bOK = new System.Windows.Forms.Button();
            this.txJelszo = new System.Windows.Forms.TextBox();
            this.txFhnev = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // bCancel
            // 
            this.bCancel.AutoSize = true;
            this.bCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.bCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.bCancel.Location = new System.Drawing.Point(235, 233);
            this.bCancel.Name = "bCancel";
            this.bCancel.Size = new System.Drawing.Size(75, 29);
            this.bCancel.TabIndex = 71;
            this.bCancel.Text = "Mégse";
            this.bCancel.UseVisualStyleBackColor = true;
            this.bCancel.Click += new System.EventHandler(this.bCancel_Click);
            // 
            // bOK
            // 
            this.bOK.AutoSize = true;
            this.bOK.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.bOK.Location = new System.Drawing.Point(118, 233);
            this.bOK.Name = "bOK";
            this.bOK.Size = new System.Drawing.Size(75, 29);
            this.bOK.TabIndex = 2;
            this.bOK.Text = "OK";
            this.bOK.UseVisualStyleBackColor = true;
            this.bOK.Click += new System.EventHandler(this.bOK_Click);
            // 
            // txJelszo
            // 
            this.txJelszo.Location = new System.Drawing.Point(146, 173);
            this.txJelszo.MaxLength = 24;
            this.txJelszo.Name = "txJelszo";
            this.txJelszo.PasswordChar = '*';
            this.txJelszo.Size = new System.Drawing.Size(176, 23);
            this.txJelszo.TabIndex = 1;
            // 
            // txFhnev
            // 
            this.txFhnev.Location = new System.Drawing.Point(146, 124);
            this.txFhnev.Name = "txFhnev";
            this.txFhnev.Size = new System.Drawing.Size(211, 23);
            this.txFhnev.TabIndex = 0;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(84, 176);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(48, 17);
            this.label4.TabIndex = 69;
            this.label4.Text = "Jelszó";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(25, 127);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(107, 17);
            this.label2.TabIndex = 68;
            this.label2.Text = "Felhasználónév";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label13.Location = new System.Drawing.Point(163, 62);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(103, 29);
            this.label13.TabIndex = 72;
            this.label13.Text = "Belépés";
            // 
            // Azonositas
            // 
            this.AcceptButton = this.bOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.CancelButton = this.bCancel;
            this.ClientSize = new System.Drawing.Size(429, 317);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.bCancel);
            this.Controls.Add(this.bOK);
            this.Controls.Add(this.txJelszo);
            this.Controls.Add(this.txFhnev);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label2);
            this.Name = "Azonositas";
            this.Text = "Azonosítás";
            this.Controls.SetChildIndex(this.label2, 0);
            this.Controls.SetChildIndex(this.label4, 0);
            this.Controls.SetChildIndex(this.txFhnev, 0);
            this.Controls.SetChildIndex(this.txJelszo, 0);
            this.Controls.SetChildIndex(this.bOK, 0);
            this.Controls.SetChildIndex(this.bCancel, 0);
            this.Controls.SetChildIndex(this.label13, 0);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button bCancel;
        private System.Windows.Forms.Button bOK;
        private System.Windows.Forms.TextBox txJelszo;
        private System.Windows.Forms.TextBox txFhnev;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label13;
    }
}
