using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows;
using System.Configuration;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace VoluntaryPensionFundSystem
{
    public partial class BevRogzFoglKivalasztas : VoluntaryPensionFundSystem.BaseForm
    {
        private SqlConnection sconn;
        private SqlCommand scommand, lastScommand;
        private SqlDataAdapter da, da2;
        private DataTable dt;
        private DataTable dtCont;
        int i = 0;
        int ContactIndex = 0;
        bool InsertTrueUpdateFalse = false;
        string querystring;
        int foglId = 0;
        int ContactsCount;
        public const string MatchEmailPattern = @"^(([\w-]+\.)+[\w-]+|([a-zA-Z]{1}|[\w-]{2,}))@" + @"(([a-zA-Z]+[\w-]+\.)+[a-zA-Z]{2,4})$";
        public const string MatchTelPattern = @"^([0-9\+][0-9\-]+)|([0-9]+)\/" + @"([0-9\-]+)$";

        // Napló logok:
        TextWriterTraceListener myListener = new TextWriterTraceListener(@"LOG\Fogl_karbantartas.log", "myListener");
        List<string> tipusL = new List<string>();

        public BevRogzFoglKivalasztas(SqlConnection SqlConn)
        {
            InitializeComponent();

            Application.ThreadException += (sender, e) => MessageBox.Show(e.Exception.Message);

            this.sconn = SqlConn;
            if (sconn.State == ConnectionState.Closed) sconn.Open();

            tsDelete.Enabled = false;
            tsFind.Enabled = true;
            tsNew.Enabled = false;
            tsSave.Enabled = false;
            tsSearch.Enabled = false;
            tsUpdate.Enabled = false;
            dgw_Contacts.Enabled = false;
            txPnrid.Enabled = false;

            tipusL.Add("");
            tipusL.Add("GTG");
            tipusL.Add("SZMLY");
            cbFoglTipusa.DataSource = tipusL;

            //scommand = new SqlCommand("spFoglalkoztatokSelect1", sconn);
            //scommand.CommandType = CommandType.StoredProcedure;
            //lastScommand = scommand;

            //da = new SqlDataAdapter(scommand);
            //da2 = new SqlDataAdapter(scommand);
            //dt = new DataTable();
            //dtCont = new DataTable();

            //Frissit();
            //this.dgwFogl.DataSource = dt;

            //dgwFogl.Focus();
            yellowMode();
            txMegnevezes.Focus();
        }

        private void Frissit()
        {
            //dt.Dispose();
            try
            {
                da.Fill(dt);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                TraceBejegyzes(ex.Message);
            }

            dt.Columns["nev"].ColumnName = "Név";
            dt.Columns["adoszam"].ColumnName = "Adószám";
            dt.Columns["pnr_tipus"].ColumnName = "Típus";
            dt.Columns["egyeni_vall"].ColumnName = "Egy. váll.";
            dt.Columns["adoazonosito_jel"].ColumnName = "Adóazonosító";
            dt.Columns["orszagkod"].ColumnName = "Országkód";
            dt.Columns["ir_szam"].ColumnName = "Ir.szám";
            dt.Columns["helyseg"].ColumnName = "Helység";
            dt.Columns["cim"].ColumnName = "Cím";
            dt.Columns["email"].ColumnName = "E-mail";
            dt.Columns["telefon"].ColumnName = "Telefon";
            dt.Columns["ert_orszagkod"].ColumnName = "Ért.orsz.";
            dt.Columns["ert_irszam"].ColumnName = "Ért. ir.szám";
            dt.Columns["ert_helyseg"].ColumnName = "Ért. helység";
            dt.Columns["ert_cim"].ColumnName = "Ért. cím";
            dt.Columns["ksh_torzsszam"].ColumnName = "KSH törzsszám";
            dt.Columns["faxszam"].ColumnName = "Fax";
            dt.Columns["titulus"].ColumnName = "Titulus";
            dt.Columns["megjegyzes"].ColumnName = "Megjegyzés";
            dt.Columns["pnr_id"].ColumnName = "Pnr Id";
        }

        public void rejtId()
        {
            dgw_Contacts.Columns[0].Visible = false;
        }

        private void dgwFogl_SelectionChanged(object sender, EventArgs e)
        {
            try
            {
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
                txTitulus.Text = dgwFogl.Rows[i].Cells[17].Value.ToString();
                txPnrid.Text = dgwFogl.Rows[i].Cells[19].Value.ToString();

                ContactsLoadData();
            }
            catch
            {
                // kezdeti hiba
            }

            txPnrid.ReadOnly = true;
            txAdoszam.ReadOnly = true;
            txMegnevezes.ReadOnly = true;
            cbFoglTipusa.Enabled = false;
            txEgyeniVall.ReadOnly = true;
            txAdoazon.ReadOnly = true;
            txOrszKod.ReadOnly = true;
            txIrszam.ReadOnly = true;
            txCim.ReadOnly = true;
            txHelyseg.ReadOnly = true;
            txKSH.ReadOnly = true;
            txErtOrszKod.ReadOnly = true;
            txErtIrszam.ReadOnly = true;
            txErtHelyseg.ReadOnly = true;
            txErtCim.ReadOnly = true;
            txTitulus.ReadOnly = true;

            tsUpdate.Enabled = false;
            tsNew.Enabled = false;
            tsDelete.Enabled = false;
            tsFind.Enabled = false;
            tsSearch.Enabled = true;
            tsSave.Enabled = false;
            //txMegnevezes.Focus();
        }

        private void ContactsLoadData()
        {
            string query1 = "select count(*) from kapcsolattartok where pnr_id=" + txPnrid.Text.ToString() + ";";
            scommand = new SqlCommand(query1, sconn);
            ContactsCount = (Int32)scommand.ExecuteScalar();

            scommand = new SqlCommand("spKapcs", sconn);
            scommand.CommandType = CommandType.StoredProcedure;
            scommand.Parameters.Add(new SqlParameter("@pnr_id", SqlDbType.Int)).Value = int.Parse(txPnrid.Text.ToString());

            da2 = new SqlDataAdapter(scommand);
            dtCont = new DataTable();
            try
            {
                da2.Fill(dtCont);
                this.dgw_Contacts.DataSource = dtCont;
                if (sconn.State == ConnectionState.Closed) sconn.Open();
                //Frissit();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                TraceBejegyzes(ex.Message);
            }

            dtCont.Columns["nev"].ColumnName = "Név";
            dtCont.Columns["tel_szam"].ColumnName = "Telefon";
            dtCont.Columns["email"].ColumnName = "Email";
            dtCont.Columns["faxszam"].ColumnName = "Fax";
            dtCont.Columns["erv_kezdete"].ColumnName = "Érv. kezdete";
            dtCont.Columns["erv_vege"].ColumnName = "Érv. vége";
            dtCont.Columns["ervenyes"].ColumnName = "Érvényes";
            dtCont.Columns["megjegyzes"].ColumnName = "Megjegyzés";
            rejtId();
        }

        private void dgwFogl_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
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

        public override void tsUpdate_Click(object sender, EventArgs e)
        {
            
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

            try
            {
                if (int.Parse(cn.FoglId) != 0)      // ha a másik formon sikeres a mentés, átveszem az adatokat.
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
                    txTitulus.Text = cn.Titulus;
                    txPnrid.Text = cn.FoglId;

                    txMegnevezes.ReadOnly = true;
                    txAdoszam.ReadOnly = true;
                    txCim.ReadOnly = true;
                    txHelyseg.ReadOnly = true;
                    txIrszam.ReadOnly = true;
                    txKSH.ReadOnly = true;
                    txPnrid.ReadOnly = true;
                    txPnrid.Enabled = false;
                    cbFoglTipusa.Enabled = true;
                    txTitulus.ReadOnly = true;
                    txErtOrszKod.ReadOnly = true;
                    txErtIrszam.ReadOnly = true;
                    txErtHelyseg.ReadOnly = true;
                    txErtCim.ReadOnly = true;
                    txOrszKod.ReadOnly = true;
                    txEgyeniVall.ReadOnly = true;
                    txAdoazon.ReadOnly = true;

                    //betöltés
                    this.dgwFogl.DataSource = null;
                    querystring = "SELECT pnr.adoszam,coalesce(pnr.nev,pnr.megnevezes) as nev,pnr.pnr_tipus,pnr.egyeni_vall,pnr.adoazonosito_jel,pnr.orszagkod," +
                        "pnr.ir_szam,pnr.helyseg,pnr.cim,pnr.ert_orszagkod,pnr.ert_irszam,pnr.ert_helyseg,pnr.ert_cim,pnr.ksh_torzsszam,pnr.telefon,pnr.email,pnr.faxszam," +
                        "pnr.titulus,pnr.megjegyzes,pnr.pnr_id FROM partnerek pnr WHERE pnr_id=" + txPnrid.Text;

                    scommand = new SqlCommand(querystring, sconn);
                    lastScommand = scommand;
                    try
                    {
                        da.Dispose();
                    }
                    catch
                    {
                        // első keresés
                    }
                    da = new SqlDataAdapter(scommand);
                    dt = new DataTable();

                    Frissit();
                    this.dgwFogl.DataSource = dt;
                    bBankszla.Enabled = true;

                    tsDelete.Enabled = false;
                    tsFind.Enabled = false;
                    tsNew.Enabled = true;
                    tsSave.Enabled = false;
                    tsSearch.Enabled = true;
                    tsUpdate.Enabled = false;
                }
            }
            catch
            {
                // nem történt módosítás
            }
        }

        public override void save()
        {            
                   
        }

        public override void yellowMode()
        {
            txMegnevezes.ReadOnly = false;
            txAdoszam.ReadOnly = false;
            txCim.ReadOnly = false;
            txHelyseg.ReadOnly = false;
            txIrszam.ReadOnly = false;
            txKSH.ReadOnly = false;
            txPnrid.ReadOnly = false;
            txPnrid.Enabled = true;
            cbFoglTipusa.Enabled = true;
            txTitulus.ReadOnly = false;
            txErtOrszKod.ReadOnly = false;
            txErtIrszam.ReadOnly = false;
            txErtHelyseg.ReadOnly = false;
            txErtCim.ReadOnly = false;
            txOrszKod.ReadOnly = false;
            txEgyeniVall.ReadOnly = false;
            txAdoazon.ReadOnly = false;

            txAdoszam.Text = string.Empty;
            txMegnevezes.Text = string.Empty;
            txCim.Text = string.Empty;
            txHelyseg.Text = string.Empty;
            txIrszam.Text = string.Empty;
            txPnrid.Text = string.Empty;
            txTitulus.Text = string.Empty;
            cbFoglTipusa.Text = string.Empty;
            txErtOrszKod.Text = string.Empty;
            txErtIrszam.Text = string.Empty;
            txErtHelyseg.Text = string.Empty;
            txErtCim.Text = string.Empty;
            txOrszKod.Text = string.Empty;
            txEgyeniVall.Text = string.Empty;
            txKSH.Text = string.Empty;
            txAdoazon.Text = string.Empty;

            txMegnevezes.Focus();

            tsFind.Enabled = true;
            tsSearch.Enabled = false;

            tsUpdate.Enabled = false;
            tsNew.Enabled = false;
            tsDelete.Enabled = false;
            tsSave.Enabled = false;
        }

        public override void runQuery()
        {
            Keres();

            if (foglId != 0)
            {
                txMegnevezes.ReadOnly = true;
                txAdoszam.ReadOnly = true;
                txCim.ReadOnly = true;
                txHelyseg.ReadOnly = true;
                txIrszam.ReadOnly = true;
                txKSH.ReadOnly = true;
                txPnrid.ReadOnly = true;
                txPnrid.Enabled = false;
                cbFoglTipusa.Enabled = true;
                txTitulus.ReadOnly = true;
                txErtOrszKod.ReadOnly = true;
                txErtIrszam.ReadOnly = true;
                txErtHelyseg.ReadOnly = true;
                txErtCim.ReadOnly = true;
                txOrszKod.ReadOnly = true;
                txEgyeniVall.ReadOnly = true;
                txAdoazon.ReadOnly = true;

                tsFind.Enabled = false;
                tsSearch.Enabled = true;
                tsUpdate.Enabled = false;
                tsNew.Enabled = false;
                tsDelete.Enabled = false;
                tsSave.Enabled = false;
            }
        }

        private void Keres()
        {
            //dt = new DataTable();
            this.dgwFogl.DataSource = null;

            // A LEKÉRDEZÉS FELÉPÍTÉSE
            querystring = "SELECT pnr.adoszam,coalesce(pnr.nev,pnr.megnevezes) as nev,pnr.pnr_tipus,pnr.egyeni_vall,pnr.adoazonosito_jel,pnr.orszagkod," +
                "pnr.ir_szam,pnr.helyseg,pnr.cim,pnr.ert_orszagkod,pnr.ert_irszam,pnr.ert_helyseg,pnr.ert_cim,pnr.ksh_torzsszam,pnr.telefon,pnr.email,pnr.faxszam," +
                "pnr.titulus,pnr.megjegyzes,pnr.pnr_id FROM partnerek pnr WHERE (pnr_tipus='GTG' or (pnr_tipus='SZMLY' and egyeni_vall='I')) and ";
            querystring = txMegnevezes.Text != string.Empty ? querystring + "coalesce(megnevezes,nev) like '" + txMegnevezes.Text + "' and " : querystring;
            querystring = (cbFoglTipusa.Text != string.Empty ? querystring + "pnr_tipus like '" + cbFoglTipusa.Text + "' and " : querystring);
            querystring = (txPnrid.Text != string.Empty ? querystring + "pnr_id like '" + txPnrid.Text + "' and " : querystring);
            querystring = (txKSH.Text != string.Empty ? querystring + "ksh_torzsszam like '" + txKSH.Text + "' and " : querystring);
            querystring = (txAdoszam.Text != string.Empty ? querystring + "adoszam like '" + txAdoszam.Text + "' and " : querystring);
            querystring = (txAdoazon.Text != string.Empty ? querystring + "adoazonosito_jel like '" + txAdoazon.Text + "' and " : querystring);
            querystring = (txIrszam.Text != string.Empty ? querystring + "ir_szam like '" + txIrszam.Text + "' and " : querystring);
            querystring = (txHelyseg.Text != string.Empty ? querystring + "helyseg like '" + txHelyseg.Text + "' and " : querystring);
            querystring = (txCim.Text != string.Empty ? querystring + "cim like '" + txCim.Text + "' and " : querystring);
            querystring = (txErtIrszam.Text != string.Empty ? querystring + "ert_irszam like '" + txErtIrszam.Text + "' and " : querystring);
            querystring = (txErtHelyseg.Text != string.Empty ? querystring + "ert_helyseg like '" + txErtHelyseg.Text + "' and " : querystring);
            querystring = (txErtCim.Text != string.Empty ? querystring + "ert_cim like '" + txErtCim.Text + "' and " : querystring);
            querystring = (txOrszKod.Text != string.Empty ? querystring + "orszagkod like '" + txOrszKod.Text + "' and " : querystring);
            querystring = (txErtOrszKod.Text != string.Empty ? querystring + "ert_orszagkod like '" + txErtOrszKod.Text + "' and " : querystring);
            querystring = (txEgyeniVall.Text != string.Empty ? querystring + "egyeni_vall like '" + txEgyeniVall.Text + "' and " : querystring);
            querystring = (txTitulus.Text != string.Empty ? querystring + "titulus like '" + txTitulus.Text + "' and " : querystring);
            querystring += "(ervenytelen is null or ervenytelen!='I') order by nev;";

            scommand = new SqlCommand(querystring, sconn);
            lastScommand = scommand;
            try
            {
                da.Dispose();
            }
            catch
            {
                // első keresés
            }
            da = new SqlDataAdapter(scommand);
            dt = new DataTable();

            Frissit();
            this.dgwFogl.DataSource = dt;

            foglId = (txPnrid.Text == string.Empty ? 0 : int.Parse(txPnrid.Text));
        }

        private void exit()
        {
            if (tsSave.Enabled == true)
            {
                DialogResult dr = MessageBox.Show("Minden nem mentett adat el fog veszni! Biztosan kilép?", "Megerősítés", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
                if (dr == DialogResult.OK)
                {
                    Close();
                }
            }
            else Close();
        }

        public override void tsDelete_Click(object sender, EventArgs e)
        {
            
        }

        private void TraceBejegyzes(string msg)
        {
            myListener.WriteLine("-" + DateTime.Now + ": " + msg);
            myListener.Flush();
            myListener.Close();
        }

        private void panel1_Resize(object sender, EventArgs e)
        {
            dgwFogl.Dock = DockStyle.Fill;
        }

        private void panel2_Resize(object sender, EventArgs e)
        {
            dgw_Contacts.Dock = DockStyle.Fill;
        }

        private void FoglKivalasztas_Resize(object sender, EventArgs e)
        {
            panel1.Width = FoglKivalasztas.ActiveForm.Width - 77;
            //gr_Contacts.Width = FoglKivalasztas.ActiveForm.Width - 157;
            //panel2.Width = FoglKivalasztas.ActiveForm.Width - 144;
        }

        private void dgw_Contacts_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                try
                {
                    e.SuppressKeyPress = true;
                    int iColumn = dgw_Contacts.CurrentCell.ColumnIndex;
                    int iRow = dgw_Contacts.CurrentCell.RowIndex;
                    if (iColumn != dgw_Contacts.Columns.Count - 1)
                        dgw_Contacts.CurrentCell = dgw_Contacts[iColumn, iRow];
                    //else
                    //    dgw_Contacts.CurrentCell = dgw_Contacts[1, iRow+1];
                }
                catch
                { }
            }
        }

        private void dgwFogl_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                try
                {
                    e.SuppressKeyPress = true;
                    int iColumn = dgwFogl.CurrentCell.ColumnIndex;
                    int iRow = dgwFogl.CurrentCell.RowIndex;
                    if (iColumn != dgwFogl.Columns.Count - 1)
                        dgwFogl.CurrentCell = dgwFogl[iColumn, iRow];
                }
                catch
                { }
            }
        }

        private void dgw_Contacts_SelectionChanged(object sender, EventArgs e)
        {
            try
            {
                if (ContactsCount > 0)
                {
                    ContactIndex = dgw_Contacts.CurrentCell.RowIndex;
                    nKcstId.Text = dgw_Contacts.Rows[ContactIndex].Cells[0].Value.ToString();
                    txKcstNev.Text = dgw_Contacts.Rows[ContactIndex].Cells[1].Value.ToString();
                    txKcstTel.Text = dgw_Contacts.Rows[ContactIndex].Cells[2].Value.ToString();
                    txKcstEmail.Text = dgw_Contacts.Rows[ContactIndex].Cells[3].Value.ToString();
                    txKcstFax.Text = dgw_Contacts.Rows[ContactIndex].Cells[4].Value.ToString();

                    txErv.Text = dgw_Contacts.Rows[ContactIndex].Cells[7].Value.ToString();
                    txKcsMegj.Text = dgw_Contacts.Rows[ContactIndex].Cells[8].Value.ToString();

                    dErvkezd.Value = DateTime.Parse(dgw_Contacts.Rows[ContactIndex].Cells[5].Value.ToString());
                    if (txErv.Text == "I") chErv.Checked = true;
                    else chErv.Checked = false;                   
                }
                else
                {
                    nKcstId.Text = string.Empty;
                    txKcstNev.Text = string.Empty;
                    txKcstTel.Text = string.Empty;
                    txKcstEmail.Text = string.Empty;
                    txKcstFax.Text = string.Empty;

                    txErv.Text = string.Empty;
                    txKcsMegj.Text = string.Empty;
                    dErvkezd.Text = string.Empty;
                }
            }
            catch (Exception ex)
            {
                // kezdeti hiba
                TraceBejegyzes(ex.Message);
            }

            txKcstNev.ReadOnly = true;
            txKcstTel.ReadOnly = true;
            txKcstEmail.ReadOnly = true;
            txKcstFax.ReadOnly = true;

            txKcsMegj.ReadOnly = true;
            dErvkezd.Enabled = false;
            chErv.Enabled = false;

            bKapcsSave.Enabled = false;
            bKapcsTorol.Enabled = true;
            bModositKapcs.Enabled = true;
            bUjKapcs.Enabled = true;            
        }

        private void txMegjegyzes_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                try
                {
                    //e.SuppressKeyPress = true;
                    dgw_Contacts.Focus();
                    dgw_Contacts.CurrentCell = dgw_Contacts.Rows[0].Cells[1];
                }
                catch
                { }
            }
        }

        private void txPnrId_ValueChanged(object sender, EventArgs e)
        {
            if (int.Parse(txPnrid.Text) > 0)
                dgw_Contacts.Enabled = true;
            else
                dgw_Contacts.Enabled = false;
        }

        private void dgw_Contacts_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            MessageBox.Show("Hibás dátum!");
            TraceBejegyzes(e.Exception.Message);
        }

        private void bUjKapcs_Click(object sender, EventArgs e)
        {
            txKcstNev.ReadOnly = false;             // név mező írható          
            txKcstTel.ReadOnly = false;
            txKcstEmail.ReadOnly = false;
            txKcstFax.ReadOnly = false;
            txKcsMegj.ReadOnly = false;

            InsertTrueUpdateFalse = true;           // új sort kell menteni

            // mezők ürítése
            nKcstId.Text = string.Empty;
            txKcstNev.Text = string.Empty;
            txKcstTel.Text = string.Empty;
            txKcstEmail.Text = string.Empty;
            txKcstFax.Text = string.Empty;
            txErv.Text = string.Empty;
            txKcsMegj.Text = string.Empty;
            dErvkezd.Text = string.Empty;
            chErv.Enabled = true;

            bKapcsSave.Enabled = true;
            bKapcsTorol.Enabled = false;
            bModositKapcs.Enabled = false;
            bUjKapcs.Enabled = false;

            txKcstNev.Focus();
        }

        private void txKcstNev_Leave(object sender, EventArgs e)
        {
            txErv.Text = "I";
            chErv.Checked = true;
        }

        private void bModositKapcs_Click(object sender, EventArgs e)
        {
            txKcstNev.ReadOnly = false;
            txKcstTel.ReadOnly = false;
            txKcstEmail.ReadOnly = false;
            txKcstFax.ReadOnly = false;

            txKcsMegj.ReadOnly = false;
            dErvkezd.Enabled = true;
            chErv.Enabled = true;

            bKapcsSave.Enabled = true;
            bKapcsTorol.Enabled = false;
            bModositKapcs.Enabled = false;
            bUjKapcs.Enabled = false;

            InsertTrueUpdateFalse = false;          // nem új sor
        }

        private void bKapcsTorol_Click(object sender, EventArgs e)
        {
            string Query;
            DialogResult dr = MessageBox.Show("Biztos törli a " + (ContactIndex + 1) + ". rekordot? ",
                "Törlés?", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dr == DialogResult.Yes)
            {                
                // törlés az adatbázisból
                Query = "delete from kapcsolattartok where kcst_id=" + int.Parse(nKcstId.Text) + ";";
                scommand = new SqlCommand(Query, sconn);
                try { scommand.ExecuteNonQuery(); }
                catch (Exception ex) { MessageBox.Show("SQL Hiba " + ex.Message); TraceBejegyzes(ex.Message); }                

                // törlés a gridből
                ContactsLoadData();
                // dgw_Contacts.Rows.RemoveAt(dgw_Contacts.CurrentRow.Index);
                // dgw_Contacts.Rows.RemoveAt(i);

                bKapcsSave.Enabled = false;
                bKapcsTorol.Enabled = true;
                bModositKapcs.Enabled = true;
                bUjKapcs.Enabled = true;
            }
        }        

        private void txKcstEmail_Leave(object sender, EventArgs e)
        {
            //CellaBemasol();
        }

        private void dgw_Contacts_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
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

        private void bBankszla_Click(object sender, EventArgs e)
        {
            Bankszamlak ViewAccounts = new Bankszamlak(sconn, int.Parse(txPnrid.Text));
            ViewAccounts.Show();
        }

        private void chErv_CheckStateChanged(object sender, EventArgs e)
        {
            if (chErv.Checked) txErv.Text = "I";
            else txErv.Text = "N";
        }

        public static bool IsTelszam(string telszam)
        {
            try
            {
                if (telszam != null) return Regex.IsMatch(telszam, MatchTelPattern);
                else return false;
            }
            catch
            {
                return false;
            }
        }

        public bool IsEmail(string email)
        {
            try
            {
                if (email != null) return Regex.IsMatch(email, MatchEmailPattern);
                else return false;
            }
            catch
            {
                return false;
            }
        }

        private void bKapcsSave_Click(object sender, EventArgs e)
        {
            // ellenőrzés
            if (txKcstNev.Text == string.Empty) { MessageBox.Show("Kapcsolattartó neve nem lehet üres!"); txKcstNev.Focus(); return; }

            int newKcstid = 0;
            newKcstid = kcstSave();
            if (newKcstid == 0) { MessageBox.Show("A rekord mentése nem sikerült!"); txMegnevezes.Focus(); return; }
            if (newKcstid == 9) { return; }

            ContactsLoadData();

            bKapcsSave.Enabled = false;
            bKapcsTorol.Enabled = true;
            bModositKapcs.Enabled = true;
            bUjKapcs.Enabled = true;
        }

        private int kcstSave()
        {
            if (!(txKcstTel.Text == string.Empty) && !(IsTelszam(txKcstTel.Text))) { MessageBox.Show("A telefonszám nem megfelelő! Formátum: körzetszám/telefonszám"); txKcstTel.Focus(); return 9; }
            if (!(txKcstEmail.Text == string.Empty) && !(IsEmail(txKcstEmail.Text))) { MessageBox.Show("Az email cím nem megfelelő!"); txKcstEmail.Focus(); return 9; }
            
            if (InsertTrueUpdateFalse)
            {
                Int32 newid = 0;
                scommand = new SqlCommand("spKcstInsert2", sconn);
                scommand.CommandType = CommandType.StoredProcedure;

                scommand.Parameters.Add(new SqlParameter("@id", SqlDbType.Int)).Value = 1;
                scommand.Parameters.Add(new SqlParameter("@tipus", SqlDbType.VarChar, 15)).Value = "SZ";
                scommand.Parameters.Add(new SqlParameter("@pnr_id", SqlDbType.Int)).Value = int.Parse(txPnrid.Text);
                scommand.Parameters.Add(new SqlParameter("@nev", SqlDbType.VarChar, 80)).Value = txKcstNev.Text;
                scommand.Parameters.Add(new SqlParameter("@erv_kezdete", SqlDbType.Date)).Value = dErvkezd.Value;
                scommand.Parameters.Add(new SqlParameter("@modosit_neve", SqlDbType.VarChar, 80)).Value = User.name;
                scommand.Parameters.Add(new SqlParameter("@ervenyes", SqlDbType.VarChar, 1)).Value = txErv.Text;
                
                try
                {
                    if (sconn.State == ConnectionState.Closed) sconn.Open();
                    newid = (Int32)scommand.ExecuteScalar();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("SQL Hiba " + ex.Message);
                    TraceBejegyzes(ex.Message);
                }

                // UPDATE
                int kcstid = newid;
                if (txKcstTel.Text != string.Empty)
                {
                    scommand = new SqlCommand("UPDATE kapcsolattartok SET tel_szam='" + txKcstTel.Text + "' where kcst_id=" + kcstid, sconn);
                    try { scommand.ExecuteNonQuery(); }
                    catch (Exception ex) { MessageBox.Show("SQL hiba: " + ex.Message); TraceBejegyzes(ex.Message); }
                }

                if (txKcstEmail.Text != string.Empty)
                {
                    scommand = new SqlCommand("UPDATE kapcsolattartok SET email='" + txKcstEmail.Text + "' where kcst_id=" + kcstid, sconn);
                    try { scommand.ExecuteNonQuery(); }
                    catch (Exception ex) { MessageBox.Show("SQL hiba: " + ex.Message); TraceBejegyzes(ex.Message); }
                }

                if (txKcstFax.Text != string.Empty)
                {
                    scommand = new SqlCommand("UPDATE kapcsolattartok SET faxszam='" + txKcstFax.Text + "' where kcst_id=" + kcstid, sconn);
                    try { scommand.ExecuteNonQuery(); }
                    catch (Exception ex) { MessageBox.Show("SQL hiba: " + ex.Message); TraceBejegyzes(ex.Message); }
                }

                if (txKcsMegj.Text != string.Empty)
                {
                    scommand = new SqlCommand("UPDATE kapcsolattartok SET megjegyzes='" + txKcsMegj.Text + "' where kcst_id=" + kcstid, sconn);
                    try { scommand.ExecuteNonQuery(); }
                    catch (Exception ex) { MessageBox.Show("SQL hiba: " + ex.Message); TraceBejegyzes(ex.Message); }
                }

                txKcstNev.ReadOnly = true;
                txKcstTel.ReadOnly = true;
                txKcstEmail.ReadOnly = true;
                txKcstFax.ReadOnly = true;

                txKcsMegj.ReadOnly = true;
                dErvkezd.Enabled = false;
                chErv.Enabled = false;
                return (int)newid;
            }
            else
            {
                scommand = new SqlCommand("spKcstUpdate", sconn);
                scommand.CommandType = CommandType.StoredProcedure;
                
                scommand.Parameters.Add(new SqlParameter("@id", SqlDbType.Int)).Value = nKcstId.Text;
                scommand.Parameters.Add(new SqlParameter("@tipus", SqlDbType.VarChar, 15)).Value = "SZ";
                scommand.Parameters.Add(new SqlParameter("@nev", SqlDbType.VarChar, 80)).Value = txKcstNev.Text;
                scommand.Parameters.Add(new SqlParameter("@erv_kezdete", SqlDbType.Date)).Value = dErvkezd.Value;
                scommand.Parameters.Add(new SqlParameter("@ervenyes", SqlDbType.VarChar, 1)).Value = txErv.Text;
                scommand.Parameters.Add(new SqlParameter("@modosit_neve", SqlDbType.VarChar, 80)).Value = User.name;
                try
                {
                    if (sconn.State == ConnectionState.Closed) sconn.Open();
                    scommand.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("SQL Hiba " + ex.Message);
                    TraceBejegyzes(ex.Message);
                }

                // UPDATE
                int kcstid = int.Parse(nKcstId.Text);
                if (txKcstTel.Text != string.Empty)
                {
                    scommand = new SqlCommand("UPDATE kapcsolattartok SET tel_szam='" + txKcstTel.Text + "' where kcst_id=" + kcstid, sconn);
                    try { scommand.ExecuteNonQuery(); }
                    catch (Exception ex) { MessageBox.Show("SQL hiba: " + ex.Message); TraceBejegyzes(ex.Message); }
                }

                if (txKcstEmail.Text != string.Empty)
                {
                    scommand = new SqlCommand("UPDATE kapcsolattartok SET email='" + txKcstEmail.Text + "' where kcst_id=" + kcstid, sconn);
                    try { scommand.ExecuteNonQuery(); }
                    catch (Exception ex) { MessageBox.Show("SQL hiba: " + ex.Message); TraceBejegyzes(ex.Message); }
                }

                if (txKcstFax.Text != string.Empty)
                {
                    scommand = new SqlCommand("UPDATE kapcsolattartok SET faxszam='" + txKcstFax.Text + "' where kcst_id=" + kcstid, sconn);
                    try { scommand.ExecuteNonQuery(); }
                    catch (Exception ex) { MessageBox.Show("SQL hiba: " + ex.Message); TraceBejegyzes(ex.Message); }
                }

                if (txKcsMegj.Text != string.Empty)
                {
                    scommand = new SqlCommand("UPDATE kapcsolattartok SET megjegyzes='" + txKcsMegj.Text + "' where kcst_id=" + kcstid, sconn);
                    try { scommand.ExecuteNonQuery(); }
                    catch (Exception ex) { MessageBox.Show("SQL hiba: " + ex.Message); TraceBejegyzes(ex.Message); }
                }

                txKcstNev.ReadOnly = true;
                txKcstTel.ReadOnly = true;
                txKcstEmail.ReadOnly = true;
                txKcstFax.ReadOnly = true;

                txKcsMegj.ReadOnly = true;
                dErvkezd.Enabled = false;
                chErv.Enabled = false;
                return 1;
            }
        }

        private void bBevRogz_Click(object sender, EventArgs e)
        {
            BevRogzUj ujBevallas = new BevRogzUj(sconn, txPnrid.Text, txAdoszam.Text, txAdoazon.Text, txMegnevezes.Text, txHelyseg.Text, txCim.Text, txIrszam.Text);
            ujBevallas.ShowDialog();
            //try
            //{
            //    ujBevallas.ShowDialog();
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show("Hiba! " + ex.Message);
            //    TraceBejegyzes(ex.Message);
            //}
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

        private void dgw_Contacts_MouseMove(object sender, MouseEventArgs e)
        {
            DataGridView.HitTestInfo hit = dgw_Contacts.HitTest(e.X, e.Y);
            if (hit.Type == DataGridViewHitTestType.Cell)
            {
                dgw_Contacts.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                dgw_Contacts.DefaultCellStyle.SelectionBackColor = Color.LightYellow;
                dgw_Contacts.DefaultCellStyle.SelectionForeColor = Color.DarkBlue;
            }
        }

        private void txPnrid_TextChanged(object sender, EventArgs e)
        {
            if (txPnrid.Text != string.Empty)
                dgw_Contacts.Enabled = true;
            else
                dgw_Contacts.Enabled = false;
        }

        private void txIrszam_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Char.IsDigit(e.KeyChar) && !Char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void txKSH_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Char.IsDigit(e.KeyChar) && !Char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
        }
    }
}