namespace VoluntaryPensionFundSystem
{
    partial class Felhasznalok
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.dgwMunk = new System.Windows.Forms.DataGridView();
            this.label13 = new System.Windows.Forms.Label();
            this.txErvVege = new System.Windows.Forms.TextBox();
            this.txErvKezdete = new System.Windows.Forms.TextBox();
            this.txTeljesnev = new System.Windows.Forms.TextBox();
            this.txFhnev = new System.Windows.Forms.TextBox();
            this.txFhoid = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.bJelszo = new System.Windows.Forms.Button();
            this.label9 = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgwMunk)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.dgwMunk);
            this.panel1.Location = new System.Drawing.Point(29, 110);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(701, 195);
            this.panel1.TabIndex = 0;
            this.panel1.Resize += new System.EventHandler(this.panel1_Resize);
            // 
            // dgwMunk
            // 
            this.dgwMunk.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dgwMunk.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgwMunk.Location = new System.Drawing.Point(3, 3);
            this.dgwMunk.Name = "dgwMunk";
            this.dgwMunk.ReadOnly = true;
            this.dgwMunk.Size = new System.Drawing.Size(695, 189);
            this.dgwMunk.TabIndex = 0;
            this.dgwMunk.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dgwMunk_CellFormatting);
            this.dgwMunk.SelectionChanged += new System.EventHandler(this.dgwMunk_SelectionChanged);
            this.dgwMunk.MouseMove += new System.Windows.Forms.MouseEventHandler(this.dgwMunk_MouseMove);
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label13.Location = new System.Drawing.Point(302, 70);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(155, 29);
            this.label13.TabIndex = 55;
            this.label13.Text = "Felhasználók";
            // 
            // txErvVege
            // 
            this.txErvVege.Location = new System.Drawing.Point(619, 385);
            this.txErvVege.Name = "txErvVege";
            this.txErvVege.ReadOnly = true;
            this.txErvVege.Size = new System.Drawing.Size(100, 23);
            this.txErvVege.TabIndex = 33;
            this.txErvVege.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txAlkVege_KeyDown);
            this.txErvVege.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txAlkVege_KeyPress);
            this.txErvVege.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txAlkVege_KeyUp);
            this.txErvVege.Leave += new System.EventHandler(this.txAlkVege_Leave);
            // 
            // txErvKezdete
            // 
            this.txErvKezdete.Location = new System.Drawing.Point(619, 355);
            this.txErvKezdete.Name = "txErvKezdete";
            this.txErvKezdete.ReadOnly = true;
            this.txErvKezdete.Size = new System.Drawing.Size(100, 23);
            this.txErvKezdete.TabIndex = 32;
            this.txErvKezdete.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txAlkKezdete_KeyDown);
            this.txErvKezdete.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txAlkKezdete_KeyPress);
            this.txErvKezdete.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txAlkKezdete_KeyUp);
            this.txErvKezdete.Leave += new System.EventHandler(this.txAlkKezdete_Leave);
            // 
            // txTeljesnev
            // 
            this.txTeljesnev.Location = new System.Drawing.Point(154, 388);
            this.txTeljesnev.Name = "txTeljesnev";
            this.txTeljesnev.ReadOnly = true;
            this.txTeljesnev.Size = new System.Drawing.Size(263, 23);
            this.txTeljesnev.TabIndex = 51;
            this.txTeljesnev.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txTeljesnev_KeyUp);
            this.txTeljesnev.Leave += new System.EventHandler(this.txTeljesnev_Leave);
            // 
            // txFhnev
            // 
            this.txFhnev.Location = new System.Drawing.Point(154, 358);
            this.txFhnev.Name = "txFhnev";
            this.txFhnev.ReadOnly = true;
            this.txFhnev.Size = new System.Drawing.Size(263, 23);
            this.txFhnev.TabIndex = 50;
            this.txFhnev.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txFhnev_KeyUp);
            // 
            // txFhoid
            // 
            this.txFhoid.Location = new System.Drawing.Point(154, 328);
            this.txFhoid.MaxLength = 8;
            this.txFhoid.Name = "txFhoid";
            this.txFhoid.ReadOnly = true;
            this.txFhoid.Size = new System.Drawing.Size(100, 23);
            this.txFhoid.TabIndex = 31;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(460, 388);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(125, 17);
            this.label8.TabIndex = 45;
            this.label8.Text = "Érvényesség vége";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(460, 358);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(144, 17);
            this.label7.TabIndex = 44;
            this.label7.Text = "Érvényesség kezdete";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(29, 391);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(73, 17);
            this.label3.TabIndex = 39;
            this.label3.Text = "Teljes név";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(29, 361);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(107, 17);
            this.label2.TabIndex = 36;
            this.label2.Text = "Felhasználónév";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(29, 331);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(47, 17);
            this.label1.TabIndex = 35;
            this.label1.Text = "Fho id";
            // 
            // bJelszo
            // 
            this.bJelszo.AutoSize = true;
            this.bJelszo.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.bJelszo.Location = new System.Drawing.Point(154, 427);
            this.bJelszo.Name = "bJelszo";
            this.bJelszo.Size = new System.Drawing.Size(130, 29);
            this.bJelszo.TabIndex = 57;
            this.bJelszo.Text = "Jelszó megadása";
            this.bJelszo.UseVisualStyleBackColor = true;
            this.bJelszo.Click += new System.EventHandler(this.bJelszo_Click);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.ForeColor = System.Drawing.Color.Red;
            this.label9.Location = new System.Drawing.Point(460, 427);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(0, 17);
            this.label9.TabIndex = 58;
            this.label9.Visible = false;
            // 
            // Felhasznalok
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.ClientSize = new System.Drawing.Size(758, 503);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.bJelszo);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.txErvVege);
            this.Controls.Add(this.txErvKezdete);
            this.Controls.Add(this.txTeljesnev);
            this.Controls.Add(this.txFhnev);
            this.Controls.Add(this.txFhoid);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "Felhasznalok";
            this.Text = "Felhasználók";
            this.Resize += new System.EventHandler(this.Felhasznalok_Resize);
            this.Controls.SetChildIndex(this.label1, 0);
            this.Controls.SetChildIndex(this.label2, 0);
            this.Controls.SetChildIndex(this.label3, 0);
            this.Controls.SetChildIndex(this.label7, 0);
            this.Controls.SetChildIndex(this.label8, 0);
            this.Controls.SetChildIndex(this.txFhoid, 0);
            this.Controls.SetChildIndex(this.txFhnev, 0);
            this.Controls.SetChildIndex(this.txTeljesnev, 0);
            this.Controls.SetChildIndex(this.txErvKezdete, 0);
            this.Controls.SetChildIndex(this.txErvVege, 0);
            this.Controls.SetChildIndex(this.label13, 0);
            this.Controls.SetChildIndex(this.panel1, 0);
            this.Controls.SetChildIndex(this.bJelszo, 0);
            this.Controls.SetChildIndex(this.label9, 0);
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgwMunk)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.DataGridView dgwMunk;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.TextBox txErvVege;
        private System.Windows.Forms.TextBox txErvKezdete;
        private System.Windows.Forms.TextBox txTeljesnev;
        private System.Windows.Forms.TextBox txFhnev;
        private System.Windows.Forms.TextBox txFhoid;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button bJelszo;
        private System.Windows.Forms.Label label9;
    }
}
