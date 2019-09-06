using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Windows;
using System.Configuration;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace VoluntaryPensionFundSystem
{
    public partial class Munkaviszony : VoluntaryPensionFundSystem.BaseForm
    {
        private SqlConnection sconn;
        private SqlCommand scommand;
        private SqlDataAdapter da;
        private DataTable dt;
        int i = 0;
        //bool keresomod = false;

        Okiratok okir;
        BevRogzLemezes BevR;
        //Object adat;
        Object Szulo;

        public const string MatchAdoszamPattern = @"^[0-9]{11}$";
        public const string MatchKshszamPattern = @"^[0-9]{17}$";
        TextWriterTraceListener myListener = new TextWriterTraceListener(@"LOG\Munkaviszony.log", "myListener");
        List<string> tipusL = new List<string>();

        public Munkaviszony(Object Szulo, SqlConnection SqlConn)
        {
            InitializeComponent();
            //Object okr, 
            //this.okir = (Okiratok)okr;
            this.Szulo = Szulo;
            
            this.sconn = SqlConn;

            tsDelete.Enabled = false;
            tsFind.Enabled = true;
            tsNew.Enabled = true;
            tsSave.Enabled = false;
            tsSearch.Enabled = false;
            tsUpdate.Enabled = false;

            yellowMode();

            tipusL.Add(""); 
            tipusL.Add("GTG");
            tipusL.Add("SZMLY");
            cbFoglTipusa.DataSource = tipusL;
        }

        private void Frissit()
        {
            /*
                * adoszam * nev * pnr_tipus * egyeni_vall * adoazonosito_jel * orszagkod * ir_szam * helyseg * cim * ert_orszagkod * ert_irszam * ert_helyseg
                * ert_cim * ksh_torzsszam * telefon * email * faxszam * titulus,megjegyzes,rogzit_neve,rogzit_datum,modosit_neve,modosit_datum,pnr_id
             */
            dgwFogl.Columns[0].HeaderText = "Adószám";
            dgwFogl.Columns[1].HeaderText = "Név";
            dgwFogl.Columns[2].HeaderText = "Típus";
            dgwFogl.Columns[3].HeaderText = "Egy. váll.";
            dgwFogl.Columns[4].HeaderText = "Adóazonosító";
            dgwFogl.Columns[5].HeaderText = "Országkód";
            dgwFogl.Columns[6].HeaderText = "Ir.szám";
            dgwFogl.Columns[7].HeaderText = "Helység";
            dgwFogl.Columns[8].HeaderText = "Cím";
            dgwFogl.Columns[9].HeaderText = "Ért.orsz.kód";
            dgwFogl.Columns[10].HeaderText = "Ért. ir. szám";
            dgwFogl.Columns[11].HeaderText = "Ért. helység";
            dgwFogl.Columns[12].HeaderText = "Ért. cím";
            dgwFogl.Columns[13].HeaderText = "KSH törzsszám";
            dgwFogl.Columns[14].HeaderText = "Telefon";
            dgwFogl.Columns[15].HeaderText = "Email";
            dgwFogl.Columns[16].HeaderText = "Fax";
            dgwFogl.Columns[17].HeaderText = "Titulus";
            dgwFogl.Columns[18].HeaderText = "Megjegyzés";
            dgwFogl.Columns[19].HeaderText = "Pnr Id";
        }

        public void rejtId()
        {
            //dgwFogl.Columns[0].Visible = false;
        }

        private void dgwFogl_SelectionChanged(object sender, EventArgs e)
        {
            try
            {
                /*
                * adoszam * nev * pnr_tipus * egyeni_vall * adoazonosito_jel * orszagkod * ir_szam * helyseg * cim * ert_orszagkod * ert_irszam * ert_helyseg
                * ert_cim * ksh_torzsszam * telefon * email * faxszam * titulus,megjegyzes,rogzit_neve,rogzit_datum,modosit_neve,modosit_datum,pnr_id
                */
                i = dgwFogl.SelectedCells[0].RowIndex;
                txAdoszam.Text = dgwFogl.Rows[i].Cells[0].Value.ToString();
                txMegnevezes.Text = dgwFogl.Rows[i].Cells[1].Value.ToString();
                cbFoglTipusa.Text = dgwFogl.Rows[i].Cells[2].Value.ToString();
                txEgyeniVall.Text = dgwFogl.Rows[i].Cells[3].Value.ToString();
                txAdoazon.Text = dgwFogl.Rows[i].Cells[4].Value.ToString();
                txOrszKod.Text = dgwFogl.Rows[i].Cells[5].Value.ToString();
                txIrszam.Text = dgwFogl.Rows[i].Cells[6].Value.ToString();
                txHelyseg.Text = dgwFogl.Rows[i].Cells[7].Value.ToString();
                txCim.Text = dgwFogl.Rows[i].Cells[8].Value.ToString();
                txErtOrszKod.Text = dgwFogl.Rows[i].Cells[9].Value.ToString();
                txErtIrszam.Text = dgwFogl.Rows[i].Cells[10].Value.ToString();
                txErtHelyseg.Text = dgwFogl.Rows[i].Cells[11].Value.ToString();
                txErtCim.Text = dgwFogl.Rows[i].Cells[12].Value.ToString();
                txKSH.Text = dgwFogl.Rows[i].Cells[13].Value.ToString();
                txTelszam.Text = dgwFogl.Rows[i].Cells[14].Value.ToString();
                txEmail.Text = dgwFogl.Rows[i].Cells[15].Value.ToString();
                txFax.Text = dgwFogl.Rows[i].Cells[16].Value.ToString();
                txTitulus.Text = dgwFogl.Rows[i].Cells[17].Value.ToString();
                txMegjegyzes.Text = dgwFogl.Rows[i].Cells[18].Value.ToString();
                txFoglId.Text = dgwFogl.Rows[i].Cells[19].Value.ToString();
            }
            catch
            {
                // Kezdeti probléma...
            }

            //txFoglId.ReadOnly = true;
            //txFoglId.BackColor = System.Drawing.SystemColors.Control;

            //txAdoszam.ReadOnly = true;
            //txCim.ReadOnly = true;
            //txFoglId.ReadOnly = true;
            //txHelyseg.ReadOnly = true;
            //txIrszam.ReadOnly = true;
            //txKSH.ReadOnly = true;
            //txMegnevezes.ReadOnly = true;
            //txTelszam.ReadOnly = true;

            //keresomod = false;
            //bSave.Enabled = false;
        }

        public void dgwFogl_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex / 2 * 2 != e.RowIndex)
            {
                e.CellStyle.BackColor = Color.AliceBlue;
            }
            else
            {
                e.CellStyle.BackColor = Color.White;
            }
        }

        public override void tsNew_Click(object sender, EventArgs e)
        {
            createNew();     
        }

        public override void tsSave_Click(object sender, EventArgs e)
        {
            save();
        }

        public override void tsSearch_Click(object sender, EventArgs e)
        {
            yellowMode();
        }

        public override void tsFind_Click(object sender, EventArgs e)
        {
            runQuery();
        }

        public override void tsExit_Click(object sender, EventArgs e)
        {
            exit();
        }

        public override void createNew()
        {
            FoglUj cn = new FoglUj(sconn);
            cn.ShowDialog();

            if (int.Parse(cn.FoglId) != 0)
            {
                txAdoszam.Text = cn.adoszam;
                txMegnevezes.Text = cn.nev;
                cbFoglTipusa.Text = cn.FoglTipusa;
                txEgyeniVall.Text = cn.EgyeniVall;
                txAdoazon.Text = cn.Adoazon;
                txOrszKod.Text = cn.OrszKod;
                txIrszam.Text = cn.Irszam;
                txHelyseg.Text = cn.Helyseg;
                txCim.Text = cn.Cim;
                txErtOrszKod.Text = cn.ErtOrszKod;
                txErtIrszam.Text = cn.ErtIrszam;
                txErtHelyseg.Text = cn.ErtHelyseg;
                txErtCim.Text = cn.ErtCim;
                txKSH.Text = cn.KSH;
                txTelszam.Text = cn.Telszam;
                txEmail.Text = cn.Email;
                txFax.Text = cn.Fax;
                txTitulus.Text = cn.Titulus;
                txMegjegyzes.Text = cn.Megjegyzes;
                txFoglId.Text = cn.FoglId;

                txMegnevezes.ReadOnly = true;
                txAdoszam.ReadOnly = true;
                txCim.ReadOnly = true;
                txHelyseg.ReadOnly = true;
                txIrszam.ReadOnly = true;
                txKSH.ReadOnly = true;
                txFoglId.ReadOnly = true;
                txTelszam.ReadOnly = true;
                cbFoglTipusa.Enabled = true;
                txTitulus.ReadOnly = true;
                txErtOrszKod.ReadOnly = true;
                txErtIrszam.ReadOnly = true;
                txErtHelyseg.ReadOnly = true;
                txErtCim.ReadOnly = true;
                txOrszKod.ReadOnly = true;
                txEmail.ReadOnly = true;
                txEgyeniVall.ReadOnly = true;
                txMegjegyzes.ReadOnly = true;
                txFax.ReadOnly = true;
                txAdoazon.ReadOnly = true;

                //txMegnevezes.BackColor = System.Drawing.SystemColors.Control;
                //txAdoszam.BackColor = System.Drawing.SystemColors.Control;
                //txCim.BackColor = System.Drawing.SystemColors.Control;
                //txHelyseg.BackColor = System.Drawing.SystemColors.Control;
                //txIrszam.BackColor = System.Drawing.SystemColors.Control;
                //txKSH.BackColor = System.Drawing.SystemColors.Control;
                //txFoglId.BackColor = System.Drawing.SystemColors.Control;
                //txTelszam.BackColor = System.Drawing.SystemColors.Control;
                //txTitulus.BackColor = System.Drawing.SystemColors.Control;
                //cbFoglTipusa.BackColor = System.Drawing.SystemColors.Control;
                //txErtOrszKod.BackColor = System.Drawing.SystemColors.Control;
                //txErtIrszam.BackColor = System.Drawing.SystemColors.Control;
                //txErtHelyseg.BackColor = System.Drawing.SystemColors.Control;
                //txErtCim.BackColor = System.Drawing.SystemColors.Control;
                //txOrszKod.BackColor = System.Drawing.SystemColors.Control;
                //txEmail.BackColor = System.Drawing.SystemColors.Control;
                //txEgyeniVall.BackColor = System.Drawing.SystemColors.Control;
                //txMegjegyzes.BackColor = System.Drawing.SystemColors.Control;
                //txFax.BackColor = System.Drawing.SystemColors.Control;
                //txAdoazon.BackColor = System.Drawing.SystemColors.Control;
            }
        }

        public override void yellowMode()
        {
            txMegnevezes.ReadOnly = false;
            txAdoszam.ReadOnly = false;
            txCim.ReadOnly = false;
            txHelyseg.ReadOnly = false;
            txIrszam.ReadOnly = false;
            txKSH.ReadOnly = false;
            txFoglId.ReadOnly = false;
            txTelszam.ReadOnly = false;
            cbFoglTipusa.Enabled = true;
            txTitulus.ReadOnly = false;
            txErtOrszKod.ReadOnly = false;
            txErtIrszam.ReadOnly = false;
            txErtHelyseg.ReadOnly = false;
            txErtCim.ReadOnly = false;
            txOrszKod.ReadOnly = false;
            txEmail.ReadOnly = false;
            txEgyeniVall.ReadOnly = false;
            txMegjegyzes.ReadOnly = false;
            txFax.ReadOnly = false;
            txAdoazon.ReadOnly = false;

            txAdoszam.Text = string.Empty;
            txMegnevezes.Text = string.Empty;
            txCim.Text = string.Empty;
            txHelyseg.Text = string.Empty;
            txIrszam.Text = string.Empty;
            txFoglId.Text = string.Empty;
            txTelszam.Text = string.Empty;
            txTitulus.Text = string.Empty;
            cbFoglTipusa.Text = string.Empty;
            txErtOrszKod.Text = string.Empty;
            txErtIrszam.Text = string.Empty;
            txErtHelyseg.Text = string.Empty;
            txErtCim.Text = string.Empty;
            txOrszKod.Text = string.Empty;
            txEmail.Text = string.Empty;
            txEgyeniVall.Text = string.Empty;
            txMegjegyzes.Text = string.Empty;
            txFax.Text = string.Empty;
            txKSH.Text = string.Empty;
            txAdoazon.Text = string.Empty;

            txMegnevezes.Focus();

            tsFind.Enabled = true;
            tsSearch.Enabled = false;
        }

        public override void runQuery()
        {
            txMegnevezes.ReadOnly = true;
            txAdoszam.ReadOnly = true;
            txCim.ReadOnly = true;
            txHelyseg.ReadOnly = true;
            txIrszam.ReadOnly = true;
            txKSH.ReadOnly = true;
            txFoglId.ReadOnly = true;
            txTelszam.ReadOnly = true;
            cbFoglTipusa.Enabled = true;
            txTitulus.ReadOnly = true;
            txErtOrszKod.ReadOnly = true;
            txErtIrszam.ReadOnly = true;
            txErtHelyseg.ReadOnly = true;
            txErtCim.ReadOnly = true;
            txOrszKod.ReadOnly = true;
            txEmail.ReadOnly = true;
            txEgyeniVall.ReadOnly = true;
            txMegjegyzes.ReadOnly = true;
            txFax.ReadOnly = true;
            txAdoazon.ReadOnly = true;

            Keres();

            tsFind.Enabled = false;
            tsSearch.Enabled = true;
        }

        private void Keres()
        {
            //dt = new DataTable();
            this.dgwFogl.DataSource = null;            

            // A LEKÉRDEZÉS FELÉPÍTÉSE
            string querystring = "SELECT pnr.adoszam,coalesce(pnr.nev,pnr.megnevezes) as nev,pnr.pnr_tipus,pnr.egyeni_vall,pnr.adoazonosito_jel,pnr.orszagkod," +
                "pnr.ir_szam,pnr.helyseg,pnr.cim,pnr.ert_orszagkod,pnr.ert_irszam,pnr.ert_helyseg,pnr.ert_cim,pnr.ksh_torzsszam,pnr.telefon,pnr.email,pnr.faxszam," +
                "pnr.titulus,pnr.megjegyzes,pnr.pnr_id FROM partnerek pnr WHERE (pnr_tipus='GTG' or (pnr_tipus='SZMLY' and egyeni_vall='I')) and ";
            querystring = txMegnevezes.Text != string.Empty ? querystring + "coalesce(megnevezes,nev) like '" + txMegnevezes.Text + "' and " : querystring;
            querystring = (cbFoglTipusa.Text != string.Empty ? querystring + "pnr_tipus like '" + cbFoglTipusa.Text + "' and " : querystring);
            querystring = (txFoglId.Text != string.Empty ? querystring + "pnr_id like '" + txFoglId.Text + "' and " : querystring);
            querystring = (txKSH.Text != string.Empty ? querystring + "ksh_torzsszam like '" + txKSH.Text + "' and " : querystring);
            querystring = (txAdoszam.Text != string.Empty ? querystring + "adoszam like '" + txAdoszam.Text + "' and " : querystring);
            querystring = (txAdoazon.Text != string.Empty ? querystring + "adoazonosito_jel like '" + txAdoazon.Text + "' and " : querystring);            
            querystring = (txIrszam.Text != string.Empty ? querystring + "ir_szam like '" + txIrszam.Text + "' and " : querystring);
            querystring = (txHelyseg.Text != string.Empty ? querystring + "helyseg like '" + txHelyseg.Text + "' and " : querystring);
            querystring = (txCim.Text != string.Empty ? querystring + "cim like '" + txCim.Text + "' and " : querystring);
            querystring = (txErtIrszam.Text != string.Empty ? querystring + "ert_irszam like '" + txErtIrszam.Text + "' and " : querystring);
            querystring = (txErtHelyseg.Text != string.Empty ? querystring + "ert_helyseg like '" + txErtHelyseg.Text + "' and " : querystring);            
            querystring = (txErtCim.Text != string.Empty ? querystring + "ert_cim like '" + txErtCim.Text + "' and " : querystring);
            querystring = (txTelszam.Text != string.Empty ? querystring + "telefon like '" + txTelszam.Text + "' and " : querystring);
            querystring = (txEmail.Text != string.Empty ? querystring + "email like '" + txEmail.Text + "' and " : querystring);
            querystring = (txOrszKod.Text != string.Empty ? querystring + "orszagkod like '" + txOrszKod.Text + "' and " : querystring);
            querystring = (txErtOrszKod.Text != string.Empty ? querystring + "ert_orszagkod like '" + txErtOrszKod.Text + "' and " : querystring);
            querystring = (txFax.Text != string.Empty ? querystring + "faxszam like '" + txFax.Text + "' and " : querystring);
            querystring = (txEgyeniVall.Text != string.Empty ? querystring + "egyeni_vall like '" + txEgyeniVall.Text + "' and " : querystring);
            querystring = (txTitulus.Text != string.Empty ? querystring + "titulus like '" + txTitulus.Text + "' and " : querystring);
            querystring = (txMegjegyzes.Text != string.Empty ? querystring + "megjegyzes like '" + txMegjegyzes.Text + "' and " : querystring);            
            querystring += "(ervenytelen is null or ervenytelen!='I') order by nev;";

            scommand = new SqlCommand(querystring, sconn);
            da = new SqlDataAdapter(scommand);
            dt = new DataTable();
            try
            {
                da.Fill(dt);
                this.dgwFogl.DataSource = dt;
                if (sconn.State == ConnectionState.Closed) sconn.Open();
                Frissit();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                TraceBejegyzes(ex.Message);
            }

            dgwFogl.Focus();
        }

        private void bSave_Click(object sender, EventArgs e)
        {
            save();
        }

        public override void save()
        {
            // Adatátadás
            if (Szulo.GetType() == typeof(Okiratok))
            {
                this.okir = (Okiratok)Szulo;
                //Okiratok adat = (Okiratok)this.Parent;
                okir.PnrId = int.Parse(txFoglId.Text);
                okir.megnevezes = txMegnevezes.Text;
                okir.irszam = txIrszam.Text;
                okir.helyseg = txHelyseg.Text;
                okir.foglcim = txCim.Text;
                okir.telszam = txTelszam.Text;
                okir.adoszam = txAdoszam.Text;
            }
            else if (Szulo.GetType() == typeof(BevRogzLemezes))
            {
                //BevRogzLemezes adat = (BevRogzLemezes)this.Parent;
                this.BevR = (BevRogzLemezes)Szulo;
                BevR.PnrId = int.Parse(txFoglId.Text);
                BevR.megnevezes = txMegnevezes.Text;
                BevR.irszam = txIrszam.Text;
                BevR.helyseg = txHelyseg.Text;
                BevR.foglcim = txCim.Text;
                BevR.telszam = txTelszam.Text;
                BevR.adoszam = txAdoszam.Text;
            }
            Close();
        }

        private void bKilep_Click(object sender, EventArgs e)
        {
            exit();
        }

        private void exit()
        {
            if (tsSave.Enabled == true)
            {
                DialogResult dr = MessageBox.Show("Minden nem mentett adat el fog veszni! Biztosan kilép?", "Megerősítés", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
                if (dr == DialogResult.OK)
                    Close();
            }
            else Close();
        }

        private void TraceBejegyzes(string msg)
        {
            myListener.WriteLine("Foglalkoztatók kiválasztása - " + DateTime.Now + ": " + msg);
            myListener.Flush();
            myListener.Close();
        }

        private void panel1_Resize(object sender, EventArgs e)
        {
            dgwFogl.Dock = DockStyle.Fill;
        }

        private void Munkaviszony_Resize(object sender, EventArgs e)
        {
            panel1.Width = Munkaviszony.ActiveForm.Width - 77; 
        }

        private void dgwFogl_DoubleClick(object sender, EventArgs e)
        {
            save();
        }

        private void dgwFogl_MouseMove(object sender, MouseEventArgs e)
        {
            DataGridView.HitTestInfo hit = dgwFogl.HitTest(e.X, e.Y);
            if (hit.Type == DataGridViewHitTestType.Cell)
            {
                dgwFogl.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                dgwFogl.DefaultCellStyle.SelectionBackColor = Color.LightYellow;
                dgwFogl.DefaultCellStyle.SelectionForeColor = Color.DarkBlue;
            }
        }
    }
}
