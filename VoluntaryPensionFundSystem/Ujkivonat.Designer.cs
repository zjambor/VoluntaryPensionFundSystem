namespace VoluntaryPensionFundSystem
{
    partial class Ujkivonat
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
            this.txKivonatSzama = new System.Windows.Forms.NumericUpDown();
            this.cbIrany = new System.Windows.Forms.ComboBox();
            this.txId = new System.Windows.Forms.TextBox();
            this.txMegjegyzes = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.lbVege = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.bSave = new System.Windows.Forms.Button();
            this.bCancel = new System.Windows.Forms.Button();
            this.datErteknap = new System.Windows.Forms.DateTimePicker();
            this.datLetrehoz = new System.Windows.Forms.DateTimePicker();
            ((System.ComponentModel.ISupportInitialize)(this.txKivonatSzama)).BeginInit();
            this.SuspendLayout();
            // 
            // txKivonatSzama
            // 
            this.txKivonatSzama.Location = new System.Drawing.Point(146, 154);
            this.txKivonatSzama.Name = "txKivonatSzama";
            this.txKivonatSzama.Size = new System.Drawing.Size(97, 23);
            this.txKivonatSzama.TabIndex = 1;
            this.txKivonatSzama.ValueChanged += new System.EventHandler(this.txKivonatSzama_ValueChanged);
            // 
            // cbIrany
            // 
            this.cbIrany.Location = new System.Drawing.Point(249, 153);
            this.cbIrany.MaxLength = 1;
            this.cbIrany.Name = "cbIrany";
            this.cbIrany.Size = new System.Drawing.Size(65, 24);
            this.cbIrany.TabIndex = 2;
            this.cbIrany.TextChanged += new System.EventHandler(this.cbIrany_TextChanged);
            this.cbIrany.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.cbIrany_KeyPress);
            // 
            // txId
            // 
            this.txId.Location = new System.Drawing.Point(475, 153);
            this.txId.MaxLength = 1;
            this.txId.Name = "txId";
            this.txId.ReadOnly = true;
            this.txId.Size = new System.Drawing.Size(46, 23);
            this.txId.TabIndex = 140;
            this.txId.Visible = false;
            // 
            // txMegjegyzes
            // 
            this.txMegjegyzes.Location = new System.Drawing.Point(28, 198);
            this.txMegjegyzes.MaxLength = 255;
            this.txMegjegyzes.Name = "txMegjegyzes";
            this.txMegjegyzes.Size = new System.Drawing.Size(493, 23);
            this.txMegjegyzes.TabIndex = 4;
            this.txMegjegyzes.TextChanged += new System.EventHandler(this.datErteknap_TextChanged);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(27, 178);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(83, 17);
            this.label7.TabIndex = 139;
            this.label7.Text = "Megjegyzés";
            // 
            // lbVege
            // 
            this.lbVege.AutoSize = true;
            this.lbVege.Location = new System.Drawing.Point(317, 132);
            this.lbVege.Name = "lbVege";
            this.lbVege.Size = new System.Drawing.Size(130, 17);
            this.lbVege.TabIndex = 138;
            this.lbVege.Text = "Létrehozás dátuma";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(27, 132);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(65, 17);
            this.label5.TabIndex = 137;
            this.label5.Text = "Értéknap";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(249, 132);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(39, 17);
            this.label4.TabIndex = 136;
            this.label4.Text = "Irány";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(143, 132);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(100, 17);
            this.label1.TabIndex = 135;
            this.label1.Text = "Kivonat száma";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label2.Location = new System.Drawing.Point(191, 75);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(181, 29);
            this.label2.TabIndex = 141;
            this.label2.Text = "Új banki kivonat";
            // 
            // bSave
            // 
            this.bSave.AutoSize = true;
            this.bSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.bSave.Location = new System.Drawing.Point(167, 238);
            this.bSave.Name = "bSave";
            this.bSave.Size = new System.Drawing.Size(140, 29);
            this.bSave.TabIndex = 5;
            this.bSave.Text = "Mentés és bezárás";
            this.bSave.UseVisualStyleBackColor = true;
            this.bSave.Click += new System.EventHandler(this.bSave_Click);
            // 
            // bCancel
            // 
            this.bCancel.AutoSize = true;
            this.bCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.bCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.bCancel.Location = new System.Drawing.Point(313, 238);
            this.bCancel.Name = "bCancel";
            this.bCancel.Size = new System.Drawing.Size(82, 29);
            this.bCancel.TabIndex = 161;
            this.bCancel.Text = "Mégse";
            this.bCancel.UseVisualStyleBackColor = true;
            this.bCancel.Click += new System.EventHandler(this.bCancel_Click);
            // 
            // datErteknap
            // 
            this.datErteknap.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.datErteknap.Location = new System.Drawing.Point(26, 154);
            this.datErteknap.Name = "datErteknap";
            this.datErteknap.Size = new System.Drawing.Size(114, 23);
            this.datErteknap.TabIndex = 0;
            // 
            // datLetrehoz
            // 
            this.datLetrehoz.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.datLetrehoz.Location = new System.Drawing.Point(320, 153);
            this.datLetrehoz.Name = "datLetrehoz";
            this.datLetrehoz.Size = new System.Drawing.Size(114, 23);
            this.datLetrehoz.TabIndex = 3;
            // 
            // Ujkivonat
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.CancelButton = this.bCancel;
            this.ClientSize = new System.Drawing.Size(563, 303);
            this.Controls.Add(this.datLetrehoz);
            this.Controls.Add(this.datErteknap);
            this.Controls.Add(this.bCancel);
            this.Controls.Add(this.bSave);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txKivonatSzama);
            this.Controls.Add(this.cbIrany);
            this.Controls.Add(this.txId);
            this.Controls.Add(this.txMegjegyzes);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.lbVege);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label1);
            this.Name = "Ujkivonat";
            this.Text = "Új banki kivonat";
            this.Load += new System.EventHandler(this.Ujkivonat_Load);
            this.Controls.SetChildIndex(this.label1, 0);
            this.Controls.SetChildIndex(this.label4, 0);
            this.Controls.SetChildIndex(this.label5, 0);
            this.Controls.SetChildIndex(this.lbVege, 0);
            this.Controls.SetChildIndex(this.label7, 0);
            this.Controls.SetChildIndex(this.txMegjegyzes, 0);
            this.Controls.SetChildIndex(this.txId, 0);
            this.Controls.SetChildIndex(this.cbIrany, 0);
            this.Controls.SetChildIndex(this.txKivonatSzama, 0);
            this.Controls.SetChildIndex(this.label2, 0);
            this.Controls.SetChildIndex(this.bSave, 0);
            this.Controls.SetChildIndex(this.bCancel, 0);
            this.Controls.SetChildIndex(this.datErteknap, 0);
            this.Controls.SetChildIndex(this.datLetrehoz, 0);
            ((System.ComponentModel.ISupportInitialize)(this.txKivonatSzama)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.NumericUpDown txKivonatSzama;
        private System.Windows.Forms.ComboBox cbIrany;
        private System.Windows.Forms.TextBox txId;
        private System.Windows.Forms.TextBox txMegjegyzes;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label lbVege;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button bSave;
        private System.Windows.Forms.Button bCancel;
        private System.Windows.Forms.DateTimePicker datErteknap;
        private System.Windows.Forms.DateTimePicker datLetrehoz;
    }
}
