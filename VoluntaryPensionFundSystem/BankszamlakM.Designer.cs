namespace VoluntaryPensionFundSystem
{
    partial class BankszamlakM
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
            this.txFoglId = new System.Windows.Forms.TextBox();
            this.label16 = new System.Windows.Forms.Label();
            this.txEgyeniVall = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.txAdoszam = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txMegnevezes = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.txSzamlaszam = new System.Windows.Forms.TextBox();
            this.dgwBankszla = new System.Windows.Forms.DataGridView();
            this.bSzures = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgwBankszla)).BeginInit();
            this.SuspendLayout();
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label2.Location = new System.Drawing.Point(418, 65);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(154, 29);
            this.label2.TabIndex = 95;
            this.label2.Text = "Bankszámlák";
            // 
            // txFoglId
            // 
            this.txFoglId.Location = new System.Drawing.Point(32, 169);
            this.txFoglId.Name = "txFoglId";
            this.txFoglId.ReadOnly = true;
            this.txFoglId.Size = new System.Drawing.Size(100, 23);
            this.txFoglId.TabIndex = 1;
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(514, 149);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(66, 17);
            this.label16.TabIndex = 250;
            this.label16.Text = "Egyéni v.";
            // 
            // txEgyeniVall
            // 
            this.txEgyeniVall.Location = new System.Drawing.Point(517, 169);
            this.txEgyeniVall.Name = "txEgyeniVall";
            this.txEgyeniVall.ReadOnly = true;
            this.txEgyeniVall.Size = new System.Drawing.Size(59, 23);
            this.txEgyeniVall.TabIndex = 3;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(579, 149);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(66, 17);
            this.label8.TabIndex = 246;
            this.label8.Text = "Adószám";
            // 
            // txAdoszam
            // 
            this.txAdoszam.Location = new System.Drawing.Point(582, 169);
            this.txAdoszam.MaxLength = 11;
            this.txAdoszam.Name = "txAdoszam";
            this.txAdoszam.ReadOnly = true;
            this.txAdoszam.Size = new System.Drawing.Size(212, 23);
            this.txAdoszam.TabIndex = 4;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(135, 149);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(171, 17);
            this.label3.TabIndex = 241;
            this.label3.Text = "Megnevezés vagy EV név";
            // 
            // txMegnevezes
            // 
            this.txMegnevezes.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txMegnevezes.Location = new System.Drawing.Point(138, 169);
            this.txMegnevezes.Name = "txMegnevezes";
            this.txMegnevezes.ReadOnly = true;
            this.txMegnevezes.Size = new System.Drawing.Size(373, 23);
            this.txMegnevezes.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(30, 149);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(107, 17);
            this.label1.TabIndex = 240;
            this.label1.Text = "Foglalkoztató id";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(29, 106);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(87, 17);
            this.label9.TabIndex = 251;
            this.label9.Text = "Számlaszám";
            // 
            // txSzamlaszam
            // 
            this.txSzamlaszam.Location = new System.Drawing.Point(32, 125);
            this.txSzamlaszam.Name = "txSzamlaszam";
            this.txSzamlaszam.Size = new System.Drawing.Size(309, 23);
            this.txSzamlaszam.TabIndex = 0;
            this.txSzamlaszam.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txSzamlaszam_KeyPress);
            this.txSzamlaszam.Leave += new System.EventHandler(this.txSzamlaszam_Leave);
            // 
            // dgwBankszla
            // 
            this.dgwBankszla.AllowUserToAddRows = false;
            this.dgwBankszla.AllowUserToDeleteRows = false;
            this.dgwBankszla.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dgwBankszla.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.dgwBankszla.ColumnHeadersHeight = 30;
            this.dgwBankszla.Location = new System.Drawing.Point(12, 210);
            this.dgwBankszla.MultiSelect = false;
            this.dgwBankszla.Name = "dgwBankszla";
            this.dgwBankszla.ReadOnly = true;
            this.dgwBankszla.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.dgwBankszla.ShowEditingIcon = false;
            this.dgwBankszla.Size = new System.Drawing.Size(978, 267);
            this.dgwBankszla.TabIndex = 5;
            this.dgwBankszla.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dgwBankszla_CellFormatting);
            this.dgwBankszla.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.dgwBankszla_DataError);
            this.dgwBankszla.SelectionChanged += new System.EventHandler(this.dgwBankszla_SelectionChanged);
            this.dgwBankszla.DoubleClick += new System.EventHandler(this.dgwBankszla_DoubleClick);
            this.dgwBankszla.MouseMove += new System.Windows.Forms.MouseEventHandler(this.dgwBankszla_MouseMove);
            // 
            // bSzures
            // 
            this.bSzures.AutoSize = true;
            this.bSzures.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.bSzures.Location = new System.Drawing.Point(361, 122);
            this.bSzures.Name = "bSzures";
            this.bSzures.Size = new System.Drawing.Size(75, 29);
            this.bSzures.TabIndex = 252;
            this.bSzures.Text = "Szűrés";
            this.bSzures.UseVisualStyleBackColor = true;
            this.bSzures.Click += new System.EventHandler(this.bSzures_Click);
            // 
            // BankszamlakM
            // 
            this.AcceptButton = this.bSzures;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.ClientSize = new System.Drawing.Size(1002, 501);
            this.Controls.Add(this.bSzures);
            this.Controls.Add(this.dgwBankszla);
            this.Controls.Add(this.txSzamlaszam);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.txFoglId);
            this.Controls.Add(this.label16);
            this.Controls.Add(this.txEgyeniVall);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.txAdoszam);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txMegnevezes);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label2);
            this.Name = "BankszamlakM";
            this.Resize += new System.EventHandler(this.Bankszamlak_Resize);
            this.Controls.SetChildIndex(this.label2, 0);
            this.Controls.SetChildIndex(this.label1, 0);
            this.Controls.SetChildIndex(this.txMegnevezes, 0);
            this.Controls.SetChildIndex(this.label3, 0);
            this.Controls.SetChildIndex(this.txAdoszam, 0);
            this.Controls.SetChildIndex(this.label8, 0);
            this.Controls.SetChildIndex(this.txEgyeniVall, 0);
            this.Controls.SetChildIndex(this.label16, 0);
            this.Controls.SetChildIndex(this.txFoglId, 0);
            this.Controls.SetChildIndex(this.label9, 0);
            this.Controls.SetChildIndex(this.txSzamlaszam, 0);
            this.Controls.SetChildIndex(this.dgwBankszla, 0);
            this.Controls.SetChildIndex(this.bSzures, 0);
            ((System.ComponentModel.ISupportInitialize)(this.dgwBankszla)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txFoglId;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.TextBox txEgyeniVall;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox txAdoszam;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txMegnevezes;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox txSzamlaszam;
        private System.Windows.Forms.DataGridView dgwBankszla;
        private System.Windows.Forms.Button bSzures;
    }
}
