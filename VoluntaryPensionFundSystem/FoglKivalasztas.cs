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
    public partial class FoglKivalasztas : VoluntaryPensionFundSystem.BaseForm
    {
        private SqlConnection sconn;
        private SqlCommand scommand, lastScommand;
        private SqlDataAdapter da, da2;
        private DataTable dt;
        private DataTable dtCont;
        int i = 0;
        int foglId = 0;
        int ContactIndex = 0;
        bool irszamHiba = false;
        bool InsertTrueUpdateFalse = false;
        //bool keresomod = false;
        //int iiRow = 0;
        //DateTime datum;
        string querystring;
        public string foglid, megnev, nev, adoszam, adoazon, helyseg, cim, irszam;

        int ContactsCount;

        public const string MatchAdoszamPattern = @"^[0-9]{11}$";
        public const string MatchKshszamPattern = @"^[0-9]{17}$";
        public const string MatchEmailPattern = @"^(([\w-]+\.)+[\w-]+|([a-zA-Z]{1}|[\w-]{2,}))@" + @"(([a-zA-Z]+[\w-]+\.)+[a-zA-Z]{2,4})$";
        public const string MatchTelPattern = @"^([0-9\+][0-9\-]+)|([0-9]+)\/" + @"([0-9\-]+)$";
        // Napló logok:
        TextWriterTraceListener myListener = new TextWriterTraceListener(@"LOG\Fogl_karbantartas.log", "myListener");
        List<string> tipusL = new List<string>();

        public FoglKivalasztas(SqlConnection SqlConn)
        {
            InitializeComponent();

            this.sconn = SqlConn;
            if (sconn.State == ConnectionState.Closed) sconn.Open();

            tsDelete.Enabled = false;
            tsFind.Enabled = false;
            tsNew.Enabled = true;
            tsSave.Enabled = false;
            tsSearch.Enabled = true;
            tsUpdate.Enabled = true;
            dgw_Contacts.Enabled = false;
            //txPnrId.Enabled = false;

            tipusL.Add("");
            tipusL.Add("GTG");
            tipusL.Add("SZMLY");
            cbFoglTipusa.DataSource = tipusL;

            scommand = new SqlCommand("spFoglalkoztatokSelect1", sconn);
            scommand.CommandType = CommandType.StoredProcedure;
            lastScommand = scommand;

            //da = new SqlDataAdapter(scommand);
            //da2 = new SqlDataAdapter(scommand);
            //dt = new DataTable();
            //dtCont = new DataTable();

            //Frissit();
            //this.dgwFogl.DataSource = dt;
            kereso();
            //dgwFogl.Focus();
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
                txTelszam.Text = dgwFogl.Rows[i].Cells[14].Value.ToString();
                txEmail.Text = dgwFogl.Rows[i].Cells[15].Value.ToString();
                txFax.Text = dgwFogl.Rows[i].Cells[16].Value.ToString();
                txTitulus.Text = dgwFogl.Rows[i].Cells[17].Value.ToString();
                txMegjegyzes.Text = dgwFogl.Rows[i].Cells[18].Value.ToString();
                txPnrId.Text = dgwFogl.Rows[i].Cells[19].Value.ToString();

                ContactsLoadData();
                //this.spKapcsTableAdapter.Fill(this.vPFSDataSet.spKapcs, new System.Nullable<int>(((int)(System.Convert.ChangeType(txPnrId.Value, typeof(int))))));
                //dgw_Contacts.Rows[0].Cells[1].ValueType = typeof(string);
                //this.dgw_Contacts.Columns["erv_kezdete"].DefaultCellStyle.Format = "d";
            }
            catch
            {
                // kezdeti hiba
                // System.Windows.Forms.MessageBox.Show(ex.Message);
            }

            txPnrId.ReadOnly = true;
            txPnrId.Enabled = false;

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
            txTelszam.ReadOnly = true;
            txErtOrszKod.ReadOnly = true;
            txErtIrszam.ReadOnly = true;
            txErtHelyseg.ReadOnly = true;
            txErtCim.ReadOnly = true;
            txEmail.ReadOnly = true;
            txFax.ReadOnly = true;
            txTitulus.ReadOnly = true;
            txMegjegyzes.ReadOnly = true;

            tsUpdate.Enabled = true;
            tsNew.Enabled = true;
            tsDelete.Enabled = true;
            tsFind.Enabled = false;
            tsSearch.Enabled = true;
            tsSave.Enabled = false;
        }

        private void ContactsLoadData()
        {
            string query1 = "select count(*) from kapcsolattartok where pnr_id=" + txPnrId.Value.ToString() + ";";
            scommand = new SqlCommand(query1, sconn);
            ContactsCount = (Int32)scommand.ExecuteScalar();

            scommand = new SqlCommand("spKapcs", sconn);
            scommand.CommandType = CommandType.StoredProcedure;
            scommand.Parameters.Add(new SqlParameter("@pnr_id", SqlDbType.Int)).Value = txPnrId.Value;

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
            updateRecord();
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
                    txTelszam.Text = cn.Telszam;
                    txEmail.Text = cn.Email;
                    txFax.Text = cn.Fax;
                    txTitulus.Text = cn.Titulus;
                    txMegjegyzes.Text = cn.Megjegyzes;
                    txPnrId.Text = cn.FoglId;

                    txMegnevezes.ReadOnly = true;
                    txAdoszam.ReadOnly = true;
                    txCim.ReadOnly = true;
                    txHelyseg.ReadOnly = true;
                    txIrszam.ReadOnly = true;
                    txKSH.ReadOnly = true;
                    txPnrId.ReadOnly = true;
                    txPnrId.Enabled = false;
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

                    //betöltés
                    this.dgwFogl.DataSource = null;
                    querystring = "SELECT pnr.adoszam,coalesce(pnr.nev,pnr.megnevezes) as nev,pnr.pnr_tipus,pnr.egyeni_vall,pnr.adoazonosito_jel,pnr.orszagkod," +
                        "pnr.ir_szam,pnr.helyseg,pnr.cim,pnr.ert_orszagkod,pnr.ert_irszam,pnr.ert_helyseg,pnr.ert_cim,pnr.ksh_torzsszam,pnr.telefon,pnr.email,pnr.faxszam," +
                        "pnr.titulus,pnr.megjegyzes,pnr.pnr_id FROM partnerek pnr WHERE pnr_id=" + txPnrId.Text;

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
                    bMszerz.Enabled = true;
                    bTamszerz.Enabled = true;

                    tsDelete.Enabled = true;
                    tsFind.Enabled = false;
                    tsNew.Enabled = true;
                    tsSave.Enabled = false;
                    tsSearch.Enabled = true;
                    tsUpdate.Enabled = true;
                }
            }
            catch
            {
                // nem történt módosítás
            }
        }

        public override void save()
        {            
            // ELLENŐRZÉSEK
            if (cbFoglTipusa.Text == string.Empty) { MessageBox.Show("Foglalkoztató típusa nincs kiválasztva!"); cbFoglTipusa.Focus(); return; }
            if (txKSH.Text == string.Empty) { MessageBox.Show("KSH megadása kötelező!"); txKSH.Focus(); return; }
            if (txAdoszam.Text == string.Empty) { MessageBox.Show("Adószám megadása kötelező!"); txAdoszam.Focus(); return; }

            if (txMegnevezes.Text == string.Empty || txHelyseg.Text == string.Empty || txIrszam.Text == string.Empty || txCim.Text == string.Empty)
            {
                MessageBox.Show("Egyik mező sem lehet üres:\n-Név\n-Irányítószám\n-Helység\n-Cím");
                txMegnevezes.Focus();
                return;
            }
            if (irszamHiba) { MessageBox.Show("Hibás irányítószám!"); txIrszam.Focus(); return; }         // az ir. szám ellenőrzések külön metódusban
            if (!(txEmail.Text == string.Empty) && !(IsEmail(txEmail.Text))) { MessageBox.Show("Az email cím nem megfelelő!"); txEmail.Focus(); return; }
            if (!(txTelszam.Text == string.Empty) && !(IsTelszam(txTelszam.Text))) { MessageBox.Show("A telefonszám nem megfelelő! Formátum: körzetszám/telefonszám"); txTelszam.Focus(); return; }
            if (!IsAdoszam(txAdoszam.Text)) { MessageBox.Show("Az adószám hibás!"); txAdoazon.Focus(); return; }

            // rekord módosítása

            // UPDATE
            int newPnrid = (int)txPnrId.Value;

            int idOK = UpdateCurrentPartner(newPnrid);
            if (idOK == 0) { MessageBox.Show("A rekord mentése nem sikerült!"); txMegnevezes.Focus(); return; }

            if (cbFoglTipusa.Text == tipusL[1])
            {
                scommand = new SqlCommand("update partnerek set megnevezes='" + txMegnevezes.Text + "' where pnr_id=" + newPnrid, sconn);
                try { scommand.ExecuteNonQuery(); }
                catch (Exception ex) { MessageBox.Show("SQL Hiba " + ex.Message); }
                scommand = new SqlCommand("update partnerek set nev=null where pnr_id=" + newPnrid, sconn);
                try { scommand.ExecuteNonQuery(); }
                catch (Exception ex) { MessageBox.Show("SQL Hiba " + ex.Message); }
            }
            else
            {
                scommand = new SqlCommand("update partnerek set nev='" + txMegnevezes.Text + "' where pnr_id=" + newPnrid, sconn);
                try { scommand.ExecuteNonQuery(); }
                catch (Exception ex) { MessageBox.Show("SQL Hiba " + ex.Message); }
                scommand = new SqlCommand("update partnerek set megnevezes=null where pnr_id=" + newPnrid, sconn);
                try { scommand.ExecuteNonQuery(); }
                catch (Exception ex) { MessageBox.Show("SQL Hiba " + ex.Message); }
            }


            if (txAdoazon.Text != string.Empty) scommand = new SqlCommand("update partnerek set adoazonosito_jel=" + long.Parse(txAdoazon.Text) + " where pnr_id=" + newPnrid, sconn);
            else scommand = new SqlCommand("update partnerek set adoazonosito_jel=null where pnr_id=" + newPnrid, sconn);
            try { scommand.ExecuteNonQuery(); }
            catch (Exception ex) { MessageBox.Show("SQL Hiba " + ex.Message); }

            if (txFax.Text != string.Empty)
            {
                scommand = new SqlCommand("update partnerek set faxszam='" + txFax.Text + "' where pnr_id=" + newPnrid, sconn);
            }
            else scommand = new SqlCommand("update partnerek set faxszam=null where pnr_id=" + newPnrid, sconn);
            try { scommand.ExecuteNonQuery(); }
            catch (Exception ex) { MessageBox.Show("SQL Hiba " + ex.Message); }

            if (txEgyeniVall.Text != string.Empty)
            {
                scommand = new SqlCommand("update partnerek set egyeni_vall='" + txEgyeniVall.Text + "' where pnr_id=" + newPnrid, sconn);
            }
            else scommand = new SqlCommand("update partnerek set egyeni_vall=null where pnr_id=" + newPnrid, sconn);
            try { scommand.ExecuteNonQuery(); }
            catch (Exception ex) { MessageBox.Show("SQL Hiba " + ex.Message); }

            if (txErtIrszam.Text != string.Empty)
            {
                scommand = new SqlCommand("update partnerek set ert_irszam='" + int.Parse(txErtIrszam.Text) + "' where pnr_id=" + newPnrid, sconn);
            }
            else scommand = new SqlCommand("update partnerek set ert_irszam=null where pnr_id=" + newPnrid, sconn);
            try { scommand.ExecuteNonQuery(); }
            catch (Exception ex) { MessageBox.Show("SQL Hiba " + ex.Message); }

            if (txErtHelyseg.Text != string.Empty)
            {
                scommand = new SqlCommand("update partnerek set ert_helyseg='" + txErtHelyseg.Text + "' where pnr_id=" + newPnrid, sconn);
            }
            else scommand = new SqlCommand("update partnerek set ert_helyseg=null where pnr_id=" + newPnrid, sconn);
            try { scommand.ExecuteNonQuery(); }
            catch (Exception ex) { MessageBox.Show("SQL Hiba " + ex.Message); }

            if (txErtCim.Text != string.Empty)
            {
                scommand = new SqlCommand("update partnerek set ert_cim='" + txErtCim.Text + "' where pnr_id=" + newPnrid, sconn);
            }
            else scommand = new SqlCommand("update partnerek set ert_cim=null where pnr_id=" + newPnrid, sconn);
            try { scommand.ExecuteNonQuery(); }
            catch (Exception ex) { MessageBox.Show("SQL Hiba " + ex.Message); }

            if (txTelszam.Text != string.Empty)
            {
                scommand = new SqlCommand("update partnerek set telefon='" + txTelszam.Text + "' where pnr_id=" + newPnrid, sconn);
            }
            else scommand = new SqlCommand("update partnerek set telefon=null where pnr_id=" + newPnrid, sconn);
            try { scommand.ExecuteNonQuery(); }
            catch (Exception ex) { MessageBox.Show("SQL Hiba " + ex.Message); }

            if (txEmail.Text != string.Empty)
            {
                scommand = new SqlCommand("update partnerek set email='" + txEmail.Text + "' where pnr_id=" + newPnrid, sconn);
            }
            else scommand = new SqlCommand("update partnerek set email=null where pnr_id=" + newPnrid, sconn);
            try { scommand.ExecuteNonQuery(); }
            catch (Exception ex) { MessageBox.Show("SQL Hiba " + ex.Message); }

            if (txErtOrszKod.Text != string.Empty)
            {
                scommand = new SqlCommand("update partnerek set ert_orszagkod='" + txErtOrszKod.Text + "' where pnr_id=" + newPnrid, sconn);
            }
            else scommand = new SqlCommand("update partnerek set ert_orszagkod=null where pnr_id=" + newPnrid, sconn);
            try { scommand.ExecuteNonQuery(); }
            catch (Exception ex) { MessageBox.Show("SQL Hiba " + ex.Message); }

            if (txTitulus.Text != string.Empty)
            {
                scommand = new SqlCommand("update partnerek set titulus='" + txTitulus.Text + "' where pnr_id=" + newPnrid, sconn);
            }
            else scommand = new SqlCommand("update partnerek set titulus=null where pnr_id=" + newPnrid, sconn);
            try { scommand.ExecuteNonQuery(); }
            catch (Exception ex) { MessageBox.Show("SQL Hiba " + ex.Message); }

            if (txMegjegyzes.Text != string.Empty)
            {
                scommand = new SqlCommand("update partnerek set megjegyzes='" + txMegjegyzes.Text + "' where pnr_id=" + newPnrid, sconn);
            }
            else scommand = new SqlCommand("update partnerek set megjegyzes=null where pnr_id=" + newPnrid, sconn);
            try { scommand.ExecuteNonQuery(); }
            catch (Exception ex) { MessageBox.Show("SQL Hiba " + ex.Message); }

            // ha a mentés kész:
            // a fogl. grid újratöltése
            scommand = lastScommand;
            da = new SqlDataAdapter(scommand);
            dt = new DataTable();            
            Frissit();
            this.dgwFogl.DataSource = dt;

            txMegnevezes.ReadOnly = true;
            txAdoszam.ReadOnly = true;
            txCim.ReadOnly = true;
            txHelyseg.ReadOnly = true;
            txIrszam.ReadOnly = true;
            txKSH.ReadOnly = true;
            txPnrId.ReadOnly = true;
            txPnrId.Enabled = false;
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

            tsSave.Enabled = false;         
        }

        private int UpdateCurrentPartner(int id)
        {
            scommand = new SqlCommand("spFoglalkoztatokUpdate", sconn);
            scommand.CommandType = CommandType.StoredProcedure;

            scommand.Parameters.Add(new SqlParameter("@pnr_id", SqlDbType.Int)).Value = id;
            scommand.Parameters.Add(new SqlParameter("@pnr_tipus", SqlDbType.VarChar, 5)).Value = cbFoglTipusa.Text;
            scommand.Parameters.Add(new SqlParameter("@adoszam", SqlDbType.VarChar, 15)).Value = txAdoszam.Text;
            scommand.Parameters.Add(new SqlParameter("@ksh_torzsszam", SqlDbType.VarChar, 20)).Value = txKSH.Text;
            scommand.Parameters.Add(new SqlParameter("@ir_szam", SqlDbType.Decimal, 4)).Value = int.Parse(txIrszam.Text);
            scommand.Parameters.Add(new SqlParameter("@helyseg", SqlDbType.VarChar, 20)).Value = txHelyseg.Text;
            scommand.Parameters.Add(new SqlParameter("@cim", SqlDbType.VarChar, 80)).Value = txCim.Text;
            scommand.Parameters.Add(new SqlParameter("@orszagkod", SqlDbType.VarChar, 5)).Value = txOrszKod.Text;
            scommand.Parameters.Add(new SqlParameter("@modosit_neve", SqlDbType.VarChar, 80)).Value = User.name;

            try
            {
                if (sconn.State == ConnectionState.Closed) sconn.Open();
                scommand.ExecuteScalar();
            }
            catch (Exception ex)
            {
                MessageBox.Show("SQL Hiba " + ex.Message);
                TraceBejegyzes(ex.Message);
                return 0;
            }
            return 1;
        }

        private void updateRecord()
        {
            txMegnevezes.ReadOnly = false;
            txAdoszam.ReadOnly = false;
            txCim.ReadOnly = false;
            txHelyseg.ReadOnly = false;
            txIrszam.ReadOnly = false;
            txKSH.ReadOnly = false;
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
        }

        public override void yellowMode()
        {
            dgwFogl.DataSource = null;
            dgwFogl.Refresh();
            dgw_Contacts.DataSource = null;
            dgw_Contacts.Refresh();

            kereso();
        }

        public void kereso()
        {
            txMegnevezes.ReadOnly = false;
            txAdoszam.ReadOnly = false;
            txCim.ReadOnly = false;
            txHelyseg.ReadOnly = false;
            txIrszam.ReadOnly = false;
            txKSH.ReadOnly = false;
            txPnrId.ReadOnly = false;
            txPnrId.Enabled = true;
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
            txPnrId.Value = 0;
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

            bBankszla.Enabled = false;
            bMszerz.Enabled = false;
            bTamszerz.Enabled = false;

            txMegnevezes.Focus();

            tsFind.Enabled = true;
            tsSearch.Enabled = false;

            tsUpdate.Enabled = false;
            tsNew.Enabled = true;
            tsDelete.Enabled = false;
            tsSave.Enabled = false;
        }

        public override void runQuery()
        {
            Keres();
            if ((int)txPnrId.Value != 0)
            {
                bBankszla.Enabled = true;
                bMszerz.Enabled = true;
                bTamszerz.Enabled = true;
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
            querystring = (txPnrId.Value != 0 ? querystring + "pnr_id like " + txPnrId.Value + " and " : querystring);
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

            foglId = (int)txPnrId.Value;
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
            DialogResult dr = MessageBox.Show("Biztos törli a kijelölt rekordot? ",
                "Törlés?", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dr == DialogResult.Yes)
            {
                // idegen kulcsok ellenőrzése
                int count = 0;
                scommand = new SqlCommand("SELECT dbo.FuncFoglFK(@id)", sconn);             // FUNCTION futtatása
                scommand.Parameters.Add(new SqlParameter("@id", SqlDbType.Int)).Value = txPnrId.Value;
                
                try
                {
                    if (sconn.State == ConnectionState.Closed) sconn.Open();
                    count = (int)scommand.ExecuteScalar();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("SQL Hiba " + ex.Message);
                    TraceBejegyzes(ex.Message);
                }

                // A visszatérési érték ellenőrzése
                switch (count)
                {
                    case 1: MessageBox.Show("A partner nem törölhető, amíg munkaviszony kapcsolódik hozzá!", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        break;
                    case 2: MessageBox.Show("A partner nem törölhető, amíg kapcsolattartó tartozik hozzá!", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        break;
                    case 3: MessageBox.Show("A partner nem törölhető, amíg bevallás kapcsolódik hozzá!", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        break;
                    case 4: MessageBox.Show("A partner nem törölhető, amíg munkáltatói szerződés kapcsolódik hozzá!", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        break;
                    case 5: MessageBox.Show("A partner nem törölhető, amíg támogatói szerződés kapcsolódik hozzá!", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        break;
                    case 6: MessageBox.Show("A partner nem törölhető, amíg tagsági szerződés kapcsolódik hozzá!", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        break;
                    case 7: MessageBox.Show("A partner nem törölhető, amíg bankszámla kapcsolódik hozzá!", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        break;
                    case 0: scommand = new SqlCommand("spFoglalkoztatokDelete", sconn);
                        scommand.CommandType = CommandType.StoredProcedure;
                        scommand.Parameters.Add(new SqlParameter("@id", SqlDbType.Int)).Value = txPnrId.Value;
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
                        MessageBox.Show("A törlés megtörtént!", "OK", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        break;
                    default: MessageBox.Show("A törlés nem sikerült!", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        break;
                }

                // a fogl. grid újratöltése
                scommand = lastScommand;
                da = new SqlDataAdapter(scommand);
                dt = new DataTable();
                Frissit();
                this.dgwFogl.DataSource = dt;
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

        public bool IsAdoszam(string adoazon)
        {
            try
            {
                if (adoazon != null) return Regex.IsMatch(adoazon, MatchAdoszamPattern);
                else return false;
            }
            catch
            {
                return false;
            }
        }

        private bool irszamEllenoriz(string irsz)
        {
            try
            {
                int szam = int.Parse(irsz);
            }
            catch
            {
                MessageBox.Show("Hibás irányítószám!");
                return true;
            }
            return false;
        }

        private void txIrszam_Leave(object sender, EventArgs e)
        {
            if (txIrszam.Text != string.Empty)
            {
                // település beolvasás
                scommand = new SqlCommand("SELECT telepules FROM iranyitoszamok where irszam=" + txIrszam.Text, sconn);
                if (sconn.State == ConnectionState.Closed) sconn.Open();
                SqlDataReader sqlReader = scommand.ExecuteReader();

                while (sqlReader.Read())
                {
                    txHelyseg.Text = (sqlReader["telepules"].ToString());
                }

                sqlReader.Close();
            }
            irszamHiba = false;
            irszamLeave(txIrszam.Text);
            if (irszamHiba) txIrszam.Focus();
        }

        private void irszamLeave(string irszamMezo)
        {
            if (irszamMezo.Length != 4)
            {
                MessageBox.Show("Hibás irányítószám!");
                irszamHiba = true;
            }
            else
            {
                irszamHiba = irszamEllenoriz(irszamMezo);
            }
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

        private void txTitulus_TextChanged(object sender, EventArgs e)
        {
            if (!tsFind.Enabled)
            {
                tsSave.Enabled = true;
            }
        }

        private void cbFoglTipusa_SelectedIndexChanged(object sender, EventArgs e)
        {
            //tsSave.Enabled = true;
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
            if (txPnrId.Value > 0)
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
            Bankszamlak ViewAccounts = new Bankszamlak(sconn, (int)txPnrId.Value);
            ViewAccounts.Show();
        }

        private void chErv_CheckStateChanged(object sender, EventArgs e)
        {
            if (chErv.Checked) txErv.Text = "I";
            else txErv.Text = "N";
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
                scommand.Parameters.Add(new SqlParameter("@pnr_id", SqlDbType.Int)).Value = txPnrId.Value;
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

        private void bMszerz_Click(object sender, EventArgs e)
        {
            foglid=txPnrId.Text;
            megnev=txMegnevezes.Text;
            nev=txMegnevezes.Text;
            adoszam=txAdoszam.Text;
            adoazon=txAdoazon.Text;
            helyseg=txHelyseg.Text;
            cim = txCim.Text;
            irszam = txIrszam.Text;
            MunkSzerzodesek msz = new MunkSzerzodesek(sconn, this);
            msz.Show();
        }

        private void bTamszerz_Click(object sender, EventArgs e)
        {
            foglid = txPnrId.Text;
            megnev = txMegnevezes.Text;
            nev = txMegnevezes.Text;
            adoszam = txAdoszam.Text;
            adoazon = txAdoazon.Text;
            helyseg = txHelyseg.Text;
            cim = txCim.Text;
            irszam = txIrszam.Text;
            TamogSzerz tsz = new TamogSzerz(sconn, this);
            tsz.Show();
        }

        private void txErtIrszam_Leave(object sender, EventArgs e)
        {
            if (txErtIrszam.Text != string.Empty)
            {
                // település beolvasás
                scommand = new SqlCommand("SELECT telepules FROM iranyitoszamok where irszam=" + txErtIrszam.Text, sconn);
                if (sconn.State == ConnectionState.Closed) sconn.Open();
                SqlDataReader sqlReader = scommand.ExecuteReader();

                while (sqlReader.Read())
                {
                    txErtHelyseg.Text = (sqlReader["telepules"].ToString());
                }

                sqlReader.Close();
            }
            irszamHiba = false;
            if (txErtIrszam.Text != string.Empty)
                irszamLeave(txErtIrszam.Text);
            if (irszamHiba) txErtIrszam.Focus();
        }

        private void txIrszam_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Char.IsDigit(e.KeyChar) && !Char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void txErtIrszam_KeyPress(object sender, KeyPressEventArgs e)
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

        private void txKcstTel_Leave(object sender, EventArgs e)
        {

        }

        private void bMunkaviszonyok_Click(object sender, EventArgs e)
        {
            FoglalkoztatoMunkaviszonyok fm = new FoglalkoztatoMunkaviszonyok(sconn, (int)txPnrId.Value);
            fm.Show();
        }
    }
}




/* a sikertelen próbálkozások 
 * 
 * // Dátum mezők lekezelése

        /*private void dErvkezd_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Back: back = true;
                    break;
                case Keys.Enter: enter = true;
                    break;
                default: break;
            }
        }

        private void dErvkezd_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar <= '9' && e.KeyChar >= '0')
            {
                if (dErvkezd.TextLength == 8) e.Handled = true;
            }
            else
            {
                if (enter)
                {
                    DateTransformA();
                }
                else if (!back)
                    e.Handled = true;
            }
        }        

        private void dErvvege_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar <= '9' && e.KeyChar >= '0')
            {
                if (dErvvege.TextLength == 8) e.Handled = true;
            }
            else
            {
                if (enter)
                {
                    DateTransformB();
                }
                else if (!back)
                    e.Handled = true;
            }
        }

        private void DateTransformA()
        {
            if (dErvkezd.TextLength == 8)
            {
                dErvkezd.Text = dErvkezd.Text.Substring(0, 4) + "." + dErvkezd.Text.Substring(4, 2) + "." + dErvkezd.Text.Substring(6, 2);
                try
                {
                    dgw_Contacts.Rows[ContactIndex].Cells[5].Value = DateTime.Parse(dErvkezd.Text).ToShortDateString();
                }
                catch
                {
                    MessageBox.Show("Hibás dátum!");
                    dErvkezd.Focus();
                }
            }
            else if (dErvkezd.TextLength == 0)
            {
                dgw_Contacts.Rows[ContactIndex].Cells[5].Value = string.Empty;
            }
            else
            {
                if (dErvkezd.TextLength != 10)
                {
                    MessageBox.Show("Hibás dátum!");
                    dErvkezd.Focus();
                }
            }
        }

        private void DateTransformB()
        {
            if (dErvvege.TextLength == 8)
            {
                dErvvege.Text = dErvvege.Text.Substring(0, 4) + "." + dErvvege.Text.Substring(4, 2) + "." + dErvvege.Text.Substring(6, 2);
                try
                {
                    dgw_Contacts.Rows[ContactIndex].Cells[6].Value = DateTime.Parse(dErvvege.Text).ToShortDateString();
                }
                catch
                {
                    MessageBox.Show("Hibás dátum!");
                    dErvvege.Focus();
                }
            }
            else if (dErvvege.TextLength == 0)
            {
                dgw_Contacts.Rows[ContactIndex].Cells[6].Value = string.Empty;
            }
            else
            {
                if (dErvkezd.TextLength != 10)
                {
                    MessageBox.Show("Hibás dátum!");
                    dErvkezd.Focus();
                }
            }
        }

        private void dErvkezd_KeyUp(object sender, KeyEventArgs e)
        {
            back = false;
            enter = false;
        }
 * 
 * //private void dErvkezd_Leave(object sender, EventArgs e)
        //{
        //    DateTransformA();
        //    CellaBemasol();
        //}

        //private void dErvvege_Leave(object sender, EventArgs e)
        //{
        //    DateTransformB();
        //    CellaBemasol();
        //}
 * 
 * private void dgw_Contacts_UserAddedRow(object sender, DataGridViewRowEventArgs e)
        //{
        //    // új sor keletkezésekor azt megjelöljük I-vel insertre
        //    int iRow = dgw_Contacts.CurrentCell.RowIndex;
        //    dgw_Contacts.Rows[iRow].Cells[9].Value = "I";
        //}

        //private void dgw_Contacts_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        //{
        //}

        //private void dgw_Contacts_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        //{
        //    endEdit = true;
        //    // cella módosítás esetén megjelöljük U betűvel update-ra (ha még nem I)
        //    int iRow = dgw_Contacts.CurrentCell.RowIndex;
        //    try
        //    {
        //        if (dgw_Contacts.Rows[iRow].Cells[9].Value.ToString() != "I")
        //        {
        //            dgw_Contacts.Rows[iRow].Cells[9].Value = "U";
        //        }
        //    }
        //    catch
        //    {
        //        // null esetén
        //    }
        //    tsSave.Enabled = true;
        //} 
 * 
//private void dgw_Contacts_KeyPress(object sender, KeyPressEventArgs e)
//{
//    this.iiRow = dgw_Contacts.CurrentCell.RowIndex;
//    if ((dgw_Contacts.CurrentCell == dgw_Contacts.Rows[iiRow].Cells[5]) || (dgw_Contacts.CurrentCell == dgw_Contacts.Rows[iiRow].Cells[6]))
//    {
//        if (e.KeyChar <= '9' && e.KeyChar >= '0')
//        {
//            //if (dgw_Contacts.CurrentCell.Value.ToString().Length == 8)
//            //{

//            //    //txdat.Text = txdat.Text.Substring(0, 4) + "." + txdat.Text.Substring(4, 2) + "." + txdat.Text.Substring(6, 2);
//            //    //dateTimePicker1.Value = DateTime.Parse(txdat.Text);
//            //}
//        }
//        else
//        {
//            if (enter)
//            {
//                // hiba -> data error event
//                if (dgw_Contacts.CurrentCell.Value.ToString().Length == 8)
//                {
//                    string a;
//                    a = dgw_Contacts.Rows[iiRow].Cells[5].Value.ToString();
//                    a = a.Substring(0, 4) + "." + a.Substring(4, 2) + "." + a.Substring(6, 2);
//                    dgw_Contacts.Rows[iiRow].Cells[5].Value = DateTime.Parse(a);
//                }
//                //MessageBox.Show("Most kéne lekezelni.1");
//            }
//            else if (!back)
//            {
//                e.Handled = true;
//                MessageBox.Show("Most kéne lekezelni.2");
//            }
//            //MessageBox.Show("Most kéne lekezelni.3");
//        }
//    }
//}

//private void dgw_Contacts_KeyUp(object sender, KeyEventArgs e)
//{
//    back = false;
//    enter = false;
//}

// Ez nem jött be...
        
private void dgw_Contacts_SelectionChanged(object sender, EventArgs e)
{
    try
    {
        this.iiRow = dgw_Contacts.CurrentCell.RowIndex;
        if (dgw_Contacts.CurrentCell == dgw_Contacts.Rows[iiRow].Cells[5])
        {
            if (dgw_Contacts.Rows[iiRow].Cells[1].Value.ToString() == "")
            {
                nevkotelezolabel.Visible = true;
                monthCalendar1.Visible = false;
                monthCalendar2.Visible = false;
            }
            else
            {
                monthCalendar1.Visible = true;
                monthCalendar2.Visible = false;
                nevkotelezolabel.Visible = false;
            }

        }
        else if (dgw_Contacts.CurrentCell == dgw_Contacts.Rows[iiRow].Cells[6])
        {
            if (dgw_Contacts.Rows[iiRow].Cells[1].Value.ToString() == "")
            {
                nevkotelezolabel.Visible = true;
                monthCalendar1.Visible = false;
                monthCalendar2.Visible = false;
            }
            else
            {
                monthCalendar2.Visible = true;
                monthCalendar1.Visible = false;
                nevkotelezolabel.Visible = false;
            }
        }
        else
        {
            monthCalendar1.Visible = false;
            monthCalendar2.Visible = false;
            nevkotelezolabel.Visible = false;
        }
        if (endEdit)
        {
            int iColumn = dgw_Contacts.CurrentCell.ColumnIndex;
            int iRow = dgw_Contacts.CurrentCell.RowIndex;
            if (iColumn != dgw_Contacts.Columns.Count - 1)
                dgw_Contacts.CurrentCell = dgw_Contacts[iColumn + 1, iRow - 1];
            endEdit = false;
        }
    }
    catch
    {
        //
    }
}

private void monthCalendar1_DateSelected(object sender, DateRangeEventArgs e)
{
    // a dátum kiválasztása után fókusz vissza a dátum cellára
    monthCalendar1.Visible = false;
    this.datum = monthCalendar1.SelectionStart;
    dgw_Contacts.Focus();
    dgw_Contacts.CurrentCell = dgw_Contacts.Rows[iiRow].Cells[5];
    dgw_Contacts.Rows[iiRow].Cells[5].Value = this.datum;
}

private void monthCalendar2_DateSelected(object sender, DateRangeEventArgs e)
{
    // a dátum kiválasztása után fókusz vissza a dátum cellára
    monthCalendar2.Visible = false;
    this.datum = monthCalendar2.SelectionStart;
    dgw_Contacts.Focus();
    dgw_Contacts.CurrentCell = dgw_Contacts.Rows[iiRow].Cells[6];
    dgw_Contacts.Rows[iiRow].Cells[6].Value = this.datum;
}
 * 
 * // az adott foglalkoztatóhoz tartozó kapcsolattartó sorok betöltése
            //try
            //{
            //    this.spKapcsTableAdapter.Fill(this.vPFSDataSet.spKapcs, new System.Nullable<int>(((int)(System.Convert.ChangeType(txPnrId.Value, typeof(int))))));
            //}
            //catch (System.Exception ex)
            //{
            //    System.Windows.Forms.MessageBox.Show(ex.Message);
            //}
 * 
 * //string erv;
                //bool hiba = false;

                //erv = dgw_Contacts.Rows[j].Cells[5].Value.ToString();
                //if (erv.Length > 0)
                //{
                //    UpdateQuery = "Update kapcsolattartok set erv_kezdete='" + DateTime.Parse(dgw_Contacts.Rows[j].Cells[5].Value.ToString()) + "' where kcst_id=" + int.Parse(dgw_Contacts.Rows[j].Cells[0].Value.ToString());
                //    scommand = new SqlCommand(UpdateQuery, sconn);
                //    try { scommand.ExecuteNonQuery(); }
                //    catch (Exception ex) { MessageBox.Show("SQL Hiba " + ex.Message); }
                //}

                //try
                //{
                //    erv = DateTime.Parse(dgw_Contacts.Rows[j].Cells[5].Value.ToString()).ToShortDateString();
                //MessageBox.Show("Ennek nem kéne lefutni.");

                
                /*try
                {
                    UpdateQuery = "Update kapcsolattartok set erv_kezdete='" + DateTime.Parse(dgw_Contacts.Rows[j].Cells[5].Value.ToString()) + "' where kcst_id=" + id;
                    scommand = new SqlCommand(UpdateQuery, sconn);
                    scommand.ExecuteNonQuery();
                }
                catch
                {
                    //MessageBox.Show("törlés indul");
                    UpdateQuery = "Update kapcsolattartok set erv_kezdete=NULL where kcst_id=" + id;
                    scommand = new SqlCommand(UpdateQuery, sconn);
                    try { scommand.ExecuteNonQuery(); }
                    catch (Exception ex) { MessageBox.Show("SQL Hiba " + ex.Message); TraceBejegyzes(ex.Message); }
                    //MessageBox.Show("törlés kész");
                }


                
                try
                {
                    UpdateQuery = "Update kapcsolattartok set erv_vege='" + DateTime.Parse(dgw_Contacts.Rows[j].Cells[6].Value.ToString()) + "' where kcst_id=" + id;
                    scommand = new SqlCommand(UpdateQuery, sconn);
                    scommand.ExecuteNonQuery();
                }
                catch
                {
                    //MessageBox.Show("törlés indul");
                    UpdateQuery = "Update kapcsolattartok set erv_vege=NULL where kcst_id=" + id;
                    scommand = new SqlCommand(UpdateQuery, sconn);
                    try { scommand.ExecuteNonQuery(); }
                    catch (Exception ex) { MessageBox.Show("SQL Hiba " + ex.Message); TraceBejegyzes(ex.Message); }
                    //MessageBox.Show("törlés kész");
                }

                //catch (Exception ex)
                //{
                //    //TraceBejegyzes(ex.ToString());
                //    //hiba = true;
                //    //MessageBox.Show("Ez hiba.");
                //    UpdateQuery = "Update kapcsolattartok set erv_kezdete=NULL where kcst_id=" + id;
                //    scommand = new SqlCommand(UpdateQuery, sconn);
                //    try { scommand.ExecuteNonQuery(); MessageBox.Show("D törölve."); }
                //    catch (Exception ex2) { MessageBox.Show("SQL Hiba " + ex2.Message); }
                //    //TraceBejegyzes("dátummező törölve, id: " + id);
                //    break;
                //}

                //try
                //{
                //    erv = DateTime.Parse(dgw_Contacts.Rows[j].Cells[5].Value.ToString()).ToShortDateString();
                //    if (!hiba)
                //    {
                //        MessageBox.Show("Ennek nem kéne lefutni.");
                //        UpdateQuery = "Update kapcsolattartok set erv_kezdete='" + dgw_Contacts.Rows[j].Cells[5].Value + "' where kcst_id=" + id;
                //        scommand = new SqlCommand(UpdateQuery, sconn);
                //        try { scommand.ExecuteNonQuery(); }
                //        catch (Exception ex) { MessageBox.Show("SQL Hiba " + ex.Message); }
                //    }
                //}
                //catch (Exception ex)
                //{
                //    TraceBejegyzes(ex.ToString());
                //    hiba = true;
                //    MessageBox.Show("Ez hiba.");
                //    UpdateQuery = "Update kapcsolattartok set erv_kezdete=NULL where kcst_id=" + id;
                //    scommand = new SqlCommand(UpdateQuery, sconn);
                //    try { scommand.ExecuteNonQuery(); MessageBox.Show("D törölve."); }
                //    catch (Exception ex2) { MessageBox.Show("SQL Hiba " + ex2.Message); }
                //    //TraceBejegyzes("dátummező törölve, id: " + id);
                //    break;
                //}

                //erv = dgw_Contacts.Rows[j].Cells[6].Value.ToString();
                //if (erv.Length > 0)
                //{
                //    UpdateQuery = "Update kapcsolattartok set erv_vege='" + DateTime.Parse(dgw_Contacts.Rows[j].Cells[6].Value.ToString()) + "' where kcst_id=" + int.Parse(dgw_Contacts.Rows[j].Cells[0].Value.ToString());
                //    scommand = new SqlCommand(UpdateQuery, sconn);
                //    try { scommand.ExecuteNonQuery(); }
                //    catch (Exception ex) { MessageBox.Show("SQL Hiba " + ex.Message); }
                //}

                //hiba = false;
                //try
                //{
                //    erv = DateTime.Parse(dgw_Contacts.Rows[j].Cells[6].Value.ToString()).ToShortDateString();
                //    if (!hiba)
                //    {
                //        MessageBox.Show("Ennek nem kéne lefutni.");
                //        UpdateQuery = "Update kapcsolattartok set erv_vege='" + dgw_Contacts.Rows[j].Cells[6].Value + "' where kcst_id=" + id;
                //        scommand = new SqlCommand(UpdateQuery, sconn);
                //        try { scommand.ExecuteNonQuery(); }
                //        catch (Exception ex) { MessageBox.Show("SQL Hiba " + ex.Message); }
                //    }
                //}
                //catch (Exception ex)
                //{
                //    TraceBejegyzes(ex.ToString());
                //    hiba = true;
                //    MessageBox.Show("Ez hiba.");
                //    UpdateQuery = "Update kapcsolattartok set erv_vege=NULL where kcst_id=" + id;
                //    scommand = new SqlCommand(UpdateQuery, sconn);
                //    try { scommand.ExecuteNonQuery(); MessageBox.Show("Dátum Törölve."); }
                //    catch (Exception ex2) { MessageBox.Show("SQL Hiba " + ex2.Message); }
                //    //TraceBejegyzes("dátummező törölve, id: " + id);
                //    break;
                //}
*
 * 
 * */
