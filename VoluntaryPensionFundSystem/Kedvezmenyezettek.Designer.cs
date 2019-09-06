namespace VoluntaryPensionFundSystem
{
    partial class Kedvezmenyezettek
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
            this.txIrszam = new System.Windows.Forms.TextBox();
            this.label29 = new System.Windows.Forms.Label();
            this.txPnrid = new System.Windows.Forms.TextBox();
            this.label15 = new System.Windows.Forms.Label();
            this.dtSzulIdo = new System.Windows.Forms.DateTimePicker();
            this.label14 = new System.Windows.Forms.Label();
            this.txCim = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.txTelefon = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.txHelyseg = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.txAdoazon = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.txSzulNev = new System.Windows.Forms.TextBox();
            this.txAnyjaNeve = new System.Windows.Forms.TextBox();
            this.txSzulHely = new System.Windows.Forms.TextBox();
            this.txNev = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txBankszla = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.txAdoszam = new System.Windows.Forms.TextBox();
            this.cim = new System.Windows.Forms.Label();
            this.numReszesedes = new System.Windows.Forms.NumericUpDown();
            this.label8 = new System.Windows.Forms.Label();
            this.txReszOssz = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.txKedvOssz = new System.Windows.Forms.NumericUpDown();
            this.panel1 = new System.Windows.Forms.Panel();
            this.dgvKedvezmenyezettek = new System.Windows.Forms.DataGridView();
            this.chTerm = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.txKdvPnrid = new System.Windows.Forms.TextBox();
            this.label16 = new System.Windows.Forms.Label();
            this.txKdv_id = new System.Windows.Forms.TextBox();
            this.label17 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.numReszesedes)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txKedvOssz)).BeginInit();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvKedvezmenyezettek)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // txIrszam
            // 
            this.txIrszam.Location = new System.Drawing.Point(25, 393);
            this.txIrszam.MaxLength = 4;
            this.txIrszam.Name = "txIrszam";
            this.txIrszam.ReadOnly = true;
            this.txIrszam.Size = new System.Drawing.Size(82, 23);
            this.txIrszam.TabIndex = 6;
            this.txIrszam.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txAdoazon_KeyPress);
            this.txIrszam.Leave += new System.EventHandler(this.txIrszam_Leave);
            // 
            // label29
            // 
            this.label29.Location = new System.Drawing.Point(367, 331);
            this.label29.Name = "label29";
            this.label29.Size = new System.Drawing.Size(104, 17);
            this.label29.TabIndex = 235;
            this.label29.Text = "Részesedés";
            // 
            // txPnrid
            // 
            this.txPnrid.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txPnrid.Location = new System.Drawing.Point(490, 350);
            this.txPnrid.Name = "txPnrid";
            this.txPnrid.ReadOnly = true;
            this.txPnrid.Size = new System.Drawing.Size(66, 23);
            this.txPnrid.TabIndex = 232;
            this.txPnrid.Visible = false;
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(487, 331);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(69, 17);
            this.label15.TabIndex = 231;
            this.label15.Text = "tag Pnr id";
            this.label15.Visible = false;
            // 
            // dtSzulIdo
            // 
            this.dtSzulIdo.Enabled = false;
            this.dtSzulIdo.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtSzulIdo.Location = new System.Drawing.Point(238, 350);
            this.dtSzulIdo.Name = "dtSzulIdo";
            this.dtSzulIdo.Size = new System.Drawing.Size(110, 23);
            this.dtSzulIdo.TabIndex = 4;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(347, 373);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(31, 17);
            this.label14.TabIndex = 229;
            this.label14.Text = "Cím";
            // 
            // txCim
            // 
            this.txCim.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txCim.Location = new System.Drawing.Point(350, 393);
            this.txCim.Name = "txCim";
            this.txCim.ReadOnly = true;
            this.txCim.Size = new System.Drawing.Size(437, 23);
            this.txCim.TabIndex = 8;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(235, 418);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(56, 17);
            this.label13.TabIndex = 228;
            this.label13.Text = "Telefon";
            // 
            // txTelefon
            // 
            this.txTelefon.Location = new System.Drawing.Point(238, 438);
            this.txTelefon.Name = "txTelefon";
            this.txTelefon.ReadOnly = true;
            this.txTelefon.Size = new System.Drawing.Size(233, 23);
            this.txTelefon.TabIndex = 10;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(131, 373);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(59, 17);
            this.label12.TabIndex = 227;
            this.label12.Text = "Helység";
            // 
            // txHelyseg
            // 
            this.txHelyseg.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txHelyseg.Location = new System.Drawing.Point(134, 393);
            this.txHelyseg.Name = "txHelyseg";
            this.txHelyseg.ReadOnly = true;
            this.txHelyseg.Size = new System.Drawing.Size(198, 23);
            this.txHelyseg.TabIndex = 7;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(22, 373);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(57, 17);
            this.label11.TabIndex = 226;
            this.label11.Text = "Ir. szám";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(22, 417);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(112, 17);
            this.label9.TabIndex = 225;
            this.label9.Text = "Adóazonosító jel";
            // 
            // txAdoazon
            // 
            this.txAdoazon.Location = new System.Drawing.Point(25, 438);
            this.txAdoazon.MaxLength = 10;
            this.txAdoazon.Name = "txAdoazon";
            this.txAdoazon.ReadOnly = true;
            this.txAdoazon.Size = new System.Drawing.Size(207, 23);
            this.txAdoazon.TabIndex = 9;
            this.txAdoazon.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txAdoazon_KeyPress);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(235, 330);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(62, 17);
            this.label6.TabIndex = 223;
            this.label6.Text = "Szül. idő";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(22, 329);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(69, 17);
            this.label4.TabIndex = 221;
            this.label4.Text = "Szül. hely";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(367, 281);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(78, 17);
            this.label3.TabIndex = 210;
            this.label3.Text = "Anyja neve";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(22, 281);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(190, 17);
            this.label2.TabIndex = 220;
            this.label2.Text = "Születési név (leánykori név)";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(22, 235);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(155, 17);
            this.label1.TabIndex = 219;
            this.label1.Text = "Kedvezményezett neve";
            // 
            // txSzulNev
            // 
            this.txSzulNev.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txSzulNev.Location = new System.Drawing.Point(25, 302);
            this.txSzulNev.Name = "txSzulNev";
            this.txSzulNev.ReadOnly = true;
            this.txSzulNev.Size = new System.Drawing.Size(339, 23);
            this.txSzulNev.TabIndex = 1;
            // 
            // txAnyjaNeve
            // 
            this.txAnyjaNeve.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txAnyjaNeve.Location = new System.Drawing.Point(370, 302);
            this.txAnyjaNeve.Name = "txAnyjaNeve";
            this.txAnyjaNeve.ReadOnly = true;
            this.txAnyjaNeve.Size = new System.Drawing.Size(410, 23);
            this.txAnyjaNeve.TabIndex = 2;
            // 
            // txSzulHely
            // 
            this.txSzulHely.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txSzulHely.Location = new System.Drawing.Point(25, 350);
            this.txSzulHely.Name = "txSzulHely";
            this.txSzulHely.ReadOnly = true;
            this.txSzulHely.Size = new System.Drawing.Size(207, 23);
            this.txSzulHely.TabIndex = 3;
            // 
            // txNev
            // 
            this.txNev.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txNev.Location = new System.Drawing.Point(25, 255);
            this.txNev.Name = "txNev";
            this.txNev.ReadOnly = true;
            this.txNev.Size = new System.Drawing.Size(755, 23);
            this.txNev.TabIndex = 0;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(223, 24);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(156, 17);
            this.label5.TabIndex = 246;
            this.label5.Text = "Pénzforgalmi jelzőszám";
            // 
            // txBankszla
            // 
            this.txBankszla.Location = new System.Drawing.Point(226, 44);
            this.txBankszla.MaxLength = 24;
            this.txBankszla.Name = "txBankszla";
            this.txBankszla.ReadOnly = true;
            this.txBankszla.Size = new System.Drawing.Size(402, 23);
            this.txBankszla.TabIndex = 1;
            this.txBankszla.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txAdoazon_KeyPress);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(10, 23);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(66, 17);
            this.label7.TabIndex = 245;
            this.label7.Text = "Adószám";
            // 
            // txAdoszam
            // 
            this.txAdoszam.Location = new System.Drawing.Point(13, 44);
            this.txAdoszam.MaxLength = 10;
            this.txAdoszam.Name = "txAdoszam";
            this.txAdoszam.ReadOnly = true;
            this.txAdoszam.Size = new System.Drawing.Size(207, 23);
            this.txAdoszam.TabIndex = 0;
            this.txAdoszam.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txAdoazon_KeyPress);
            // 
            // cim
            // 
            this.cim.AutoSize = true;
            this.cim.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.cim.Location = new System.Drawing.Point(292, 65);
            this.cim.Name = "cim";
            this.cim.Size = new System.Drawing.Size(228, 29);
            this.cim.TabIndex = 247;
            this.cim.Text = "Kedvezményezettek";
            // 
            // numReszesedes
            // 
            this.numReszesedes.DecimalPlaces = 2;
            this.numReszesedes.Enabled = false;
            this.numReszesedes.Location = new System.Drawing.Point(370, 350);
            this.numReszesedes.Name = "numReszesedes";
            this.numReszesedes.Size = new System.Drawing.Size(101, 23);
            this.numReszesedes.TabIndex = 5;
            this.numReszesedes.Enter += new System.EventHandler(this.numReszesedes_Enter);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(638, 546);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(150, 17);
            this.label8.TabIndex = 252;
            this.label8.Text = "Részesedés összesen";
            // 
            // txReszOssz
            // 
            this.txReszOssz.Location = new System.Drawing.Point(641, 566);
            this.txReszOssz.Name = "txReszOssz";
            this.txReszOssz.ReadOnly = true;
            this.txReszOssz.Size = new System.Drawing.Size(74, 23);
            this.txReszOssz.TabIndex = 250;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(452, 546);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(180, 17);
            this.label10.TabIndex = 251;
            this.label10.Text = "Kedvezményezettek száma";
            // 
            // txKedvOssz
            // 
            this.txKedvOssz.Enabled = false;
            this.txKedvOssz.Location = new System.Drawing.Point(455, 567);
            this.txKedvOssz.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.txKedvOssz.Name = "txKedvOssz";
            this.txKedvOssz.Size = new System.Drawing.Size(101, 23);
            this.txKedvOssz.TabIndex = 253;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.dgvKedvezmenyezettek);
            this.panel1.Location = new System.Drawing.Point(25, 97);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(762, 135);
            this.panel1.TabIndex = 254;
            this.panel1.Resize += new System.EventHandler(this.panel1_Resize);
            // 
            // dgvKedvezmenyezettek
            // 
            this.dgvKedvezmenyezettek.AllowUserToAddRows = false;
            this.dgvKedvezmenyezettek.AllowUserToDeleteRows = false;
            this.dgvKedvezmenyezettek.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dgvKedvezmenyezettek.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvKedvezmenyezettek.Location = new System.Drawing.Point(3, 3);
            this.dgvKedvezmenyezettek.Name = "dgvKedvezmenyezettek";
            this.dgvKedvezmenyezettek.ReadOnly = true;
            this.dgvKedvezmenyezettek.Size = new System.Drawing.Size(756, 129);
            this.dgvKedvezmenyezettek.TabIndex = 28;
            this.dgvKedvezmenyezettek.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dgvKedvezmenyezettek_CellFormatting);
            this.dgvKedvezmenyezettek.SelectionChanged += new System.EventHandler(this.dgvKedvezmenyezettek_SelectionChanged);
            this.dgvKedvezmenyezettek.MouseMove += new System.Windows.Forms.MouseEventHandler(this.dgvKedvezmenyezettek_MouseMove);
            // 
            // chTerm
            // 
            this.chTerm.AutoSize = true;
            this.chTerm.Checked = true;
            this.chTerm.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chTerm.Enabled = false;
            this.chTerm.Location = new System.Drawing.Point(525, 439);
            this.chTerm.Name = "chTerm";
            this.chTerm.Size = new System.Drawing.Size(164, 21);
            this.chTerm.TabIndex = 11;
            this.chTerm.Text = "Természetes személy";
            this.chTerm.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.txBankszla);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.txAdoszam);
            this.groupBox1.Location = new System.Drawing.Point(12, 466);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(667, 78);
            this.groupBox1.TabIndex = 12;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Ha a kedvezményezett nem természetes személy:";
            // 
            // txKdvPnrid
            // 
            this.txKdvPnrid.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txKdvPnrid.Location = new System.Drawing.Point(562, 349);
            this.txKdvPnrid.Name = "txKdvPnrid";
            this.txKdvPnrid.ReadOnly = true;
            this.txKdvPnrid.Size = new System.Drawing.Size(70, 23);
            this.txKdvPnrid.TabIndex = 259;
            this.txKdvPnrid.Visible = false;
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(559, 330);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(71, 17);
            this.label16.TabIndex = 258;
            this.label16.Text = "kdv Pnr id";
            this.label16.Visible = false;
            // 
            // txKdv_id
            // 
            this.txKdv_id.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txKdv_id.Location = new System.Drawing.Point(638, 349);
            this.txKdv_id.Name = "txKdv_id";
            this.txKdv_id.ReadOnly = true;
            this.txKdv_id.Size = new System.Drawing.Size(77, 23);
            this.txKdv_id.TabIndex = 261;
            this.txKdv_id.Visible = false;
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(635, 330);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(45, 17);
            this.label17.TabIndex = 260;
            this.label17.Text = "kdv id";
            this.label17.Visible = false;
            // 
            // Kedvezmenyezettek
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.ClientSize = new System.Drawing.Size(813, 598);
            this.Controls.Add(this.txKdv_id);
            this.Controls.Add(this.label17);
            this.Controls.Add(this.txKdvPnrid);
            this.Controls.Add(this.label16);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.chTerm);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.txKedvOssz);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.txReszOssz);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.numReszesedes);
            this.Controls.Add(this.cim);
            this.Controls.Add(this.txIrszam);
            this.Controls.Add(this.label29);
            this.Controls.Add(this.txPnrid);
            this.Controls.Add(this.label15);
            this.Controls.Add(this.dtSzulIdo);
            this.Controls.Add(this.label14);
            this.Controls.Add(this.txCim);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.txTelefon);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.txHelyseg);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.txAdoazon);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txSzulNev);
            this.Controls.Add(this.txAnyjaNeve);
            this.Controls.Add(this.txSzulHely);
            this.Controls.Add(this.txNev);
            this.Name = "Kedvezmenyezettek";
            this.Resize += new System.EventHandler(this.Kedvezmenyezettek_Resize);
            this.Controls.SetChildIndex(this.txNev, 0);
            this.Controls.SetChildIndex(this.txSzulHely, 0);
            this.Controls.SetChildIndex(this.txAnyjaNeve, 0);
            this.Controls.SetChildIndex(this.txSzulNev, 0);
            this.Controls.SetChildIndex(this.label1, 0);
            this.Controls.SetChildIndex(this.label2, 0);
            this.Controls.SetChildIndex(this.label3, 0);
            this.Controls.SetChildIndex(this.label4, 0);
            this.Controls.SetChildIndex(this.label6, 0);
            this.Controls.SetChildIndex(this.txAdoazon, 0);
            this.Controls.SetChildIndex(this.label9, 0);
            this.Controls.SetChildIndex(this.label11, 0);
            this.Controls.SetChildIndex(this.txHelyseg, 0);
            this.Controls.SetChildIndex(this.label12, 0);
            this.Controls.SetChildIndex(this.txTelefon, 0);
            this.Controls.SetChildIndex(this.label13, 0);
            this.Controls.SetChildIndex(this.txCim, 0);
            this.Controls.SetChildIndex(this.label14, 0);
            this.Controls.SetChildIndex(this.dtSzulIdo, 0);
            this.Controls.SetChildIndex(this.label15, 0);
            this.Controls.SetChildIndex(this.txPnrid, 0);
            this.Controls.SetChildIndex(this.label29, 0);
            this.Controls.SetChildIndex(this.txIrszam, 0);
            this.Controls.SetChildIndex(this.cim, 0);
            this.Controls.SetChildIndex(this.numReszesedes, 0);
            this.Controls.SetChildIndex(this.label10, 0);
            this.Controls.SetChildIndex(this.txReszOssz, 0);
            this.Controls.SetChildIndex(this.label8, 0);
            this.Controls.SetChildIndex(this.txKedvOssz, 0);
            this.Controls.SetChildIndex(this.panel1, 0);
            this.Controls.SetChildIndex(this.chTerm, 0);
            this.Controls.SetChildIndex(this.groupBox1, 0);
            this.Controls.SetChildIndex(this.label16, 0);
            this.Controls.SetChildIndex(this.txKdvPnrid, 0);
            this.Controls.SetChildIndex(this.label17, 0);
            this.Controls.SetChildIndex(this.txKdv_id, 0);
            ((System.ComponentModel.ISupportInitialize)(this.numReszesedes)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txKedvOssz)).EndInit();
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvKedvezmenyezettek)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txIrszam;
        private System.Windows.Forms.Label label29;
        private System.Windows.Forms.TextBox txPnrid;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.DateTimePicker dtSzulIdo;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.TextBox txCim;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.TextBox txTelefon;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TextBox txHelyseg;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox txAdoazon;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txSzulNev;
        private System.Windows.Forms.TextBox txAnyjaNeve;
        private System.Windows.Forms.TextBox txSzulHely;
        private System.Windows.Forms.TextBox txNev;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txBankszla;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txAdoszam;
        private System.Windows.Forms.Label cim;
        private System.Windows.Forms.NumericUpDown numReszesedes;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox txReszOssz;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.NumericUpDown txKedvOssz;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.DataGridView dgvKedvezmenyezettek;
        private System.Windows.Forms.CheckBox chTerm;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox txKdvPnrid;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.TextBox txKdv_id;
        private System.Windows.Forms.Label label17;
    }
}
