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
    public partial class PartnerTorzsadatok : VoluntaryPensionFundSystem.BaseForm
    {
        private SqlConnection sconn;
        private SqlCommand scommand, lastScommand;
        private SqlDataAdapter da;
        private DataTable dt;
        private List<string> lista = new List<string>();
        private List<string> csalAllapotLista = new List<string>();
        private List<string> tagdijfiz = new List<string>();
        private int i = 0;
        private bool keresomod = false;
        //private bool irszamHiba = false;
        public int selectedId;
        private string querystring;
        private string csalAllapot = string.Empty;
        private int index = 0;

        TextWriterTraceListener myListener = new TextWriterTraceListener(@"LOG\PartnerTorzsadatok.log", "myListener");
        public const string MatchEmailPattern = @"^(([\w-]+\.)+[\w-]+|([a-zA-Z]{1}|[\w-]{2,}))@" + @"(([a-zA-Z]+[\w-]+\.)+[a-zA-Z]{2,4})$";
        public const string MatchAdoszamPattern = @"^[0-9]{10}$";

        public PartnerTorzsadatok(SqlConnection SqlConn)
        {
            InitializeComponent();

            this.sconn = SqlConn;
            if (sconn.State == ConnectionState.Closed) sconn.Open();

            // adattábla
            dt = new DataTable();           

            lista.Add("");
            lista.Add("FÉRFI");
            lista.Add("NŐ");
            cbNeme.DataSource = lista;
            csalAllapotLista.Add("");
            csalAllapotLista.Add("NŐS");
            csalAllapotLista.Add("FÉRJEZETT");
            csalAllapotLista.Add("ELVÁLT");
            csalAllapotLista.Add("ÖZVEGY");
            csalAllapotLista.Add("NŐTLEN");
            csalAllapotLista.Add("HAJADON");
            cbCsalAll.DataSource = csalAllapotLista;

            tagdijfiz.Add("SAJÁT");
            tagdijfiz.Add("MUNK. SAJÁT");
            tagdijfiz.Add("MUNK. HOZZÁJÁRULÁS");
            tagdijfiz.Add("HARMADIK SZEMÉLY");
            cbTagdijfizTipusa.DataSource = tagdijfiz;

            tsDelete.Enabled = true;
            tsFind.Enabled = false;
            tsNew.Enabled = true;
            tsSave.Enabled = false;
            tsSearch.Enabled = true;
            tsUpdate.Enabled = false;

            dtSzulIdo.Visible = false;
            txSzuldat.Visible = true;

            // Belépési jogcímek
            scommand = new SqlCommand("SELECT * FROM jogcimek WHERE tipus='B' AND ervenyes='I'", sconn);
            if (sconn.State == ConnectionState.Closed) sconn.Open();
            SqlDataReader sqlReader = scommand.ExecuteReader();

            while (sqlReader.Read())
            {
                cbBelepTipusa.Items.Add(sqlReader["megnevezes"].ToString());
            }

            sqlReader.Close();

            // Életciklus jogcímek
            scommand = new SqlCommand("SELECT * FROM jogcimek WHERE tipus='E' AND ervenyes='I'", sconn);
            if (sconn.State == ConnectionState.Closed) sconn.Open();
            sqlReader = scommand.ExecuteReader();

            while (sqlReader.Read())
            {
                cbEletciklStatusz.Items.Add(sqlReader["megnevezes"].ToString());
            }

            sqlReader.Close();

            // Kilépési jogcímek
            scommand = new SqlCommand("SELECT * FROM jogcimek WHERE tipus='M' AND ervenyes='I'", sconn);
            if (sconn.State == ConnectionState.Closed) sconn.Open();
            sqlReader = scommand.ExecuteReader();

            while (sqlReader.Read())
            {
                cbMegszunesTipusa.Items.Add(sqlReader["megnevezes"].ToString());
            }

            sqlReader.Close();

            // portfóliók
            scommand = new SqlCommand("SELECT * FROM befektetesi_kombinaciok", sconn);
            if (sconn.State == ConnectionState.Closed) sconn.Open();
            sqlReader = scommand.ExecuteReader();

            while (sqlReader.Read())
            {
                cbPortfolio.Items.Add(sqlReader["tipus"].ToString());
            }

            sqlReader.Close();

            dgwOkiratok.Focus();
        }

        private void Frissit()
        {
            dt.Dispose();
            //dt = new DataTable();

            try
            {
                da.Fill(dt);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                TraceBejegyzes(ex.Message);
            }

            dt.Columns["tszs_id"].ColumnName = "PAJ";
            dt.Columns["adoazonosito_jel"].ColumnName = "Adóazonosító jel";
            dt.Columns["titulus"].ColumnName = "Titulus";
            dt.Columns["nev"].ColumnName = "Név";
            dt.Columns["orszagkod"].ColumnName = "Orsz.kód";
            dt.Columns["ir_szam"].ColumnName = "Ir.szám";
            dt.Columns["helyseg"].ColumnName = "Helység";
            dt.Columns["cim"].ColumnName = "Cím";
            dt.Columns["belep_tipusa"].ColumnName = "Belép. típ.";
            dt.Columns["belep_datum"].ColumnName = "Belép. dátum";
            dt.Columns["tagdijfiz_tipus"].ColumnName = "Tagdíjfiz. típ.";
            dt.Columns["vallalt_tagdij"].ColumnName = "Vállalt tagdíj";
            dt.Columns["felvet_datum"].ColumnName = "Felvét dátuma";
            dt.Columns["zaradek_datum"].ColumnName = "Záradékolás";
            dt.Columns["eletcikl_stat"].ColumnName = "Életcikl. stát.";
            dt.Columns["leanykori_nev"].ColumnName = "Születési Név";
            dt.Columns["anyja_neve"].ColumnName = "Anyja Neve";
            dt.Columns["szul_helye"].ColumnName = "Születési Hely";
            dt.Columns["szul_dat"].ColumnName = "Szül. Idő";
            dt.Columns["neme"].ColumnName = "Neme";
            dt.Columns["csal_allapot"].ColumnName = "Csal. állapot";
            dt.Columns["allampolg"].ColumnName = "Állampolgárság";
            dt.Columns["email"].ColumnName = "E-mail";
            dt.Columns["telefon"].ColumnName = "Telefon";
            dt.Columns["alair_datum"].ColumnName = "Aláírás dátuma";
            dt.Columns["erkez_datum"].ColumnName = "Érkezés dátuma";
            dt.Columns["ert_orszagkod"].ColumnName = "Ért. orsz.";
            dt.Columns["ert_irszam"].ColumnName = "Ért. ir.szám";
            dt.Columns["ert_helyseg"].ColumnName = "Ért. helység";
            dt.Columns["ert_cim"].ColumnName = "Ért. cím";
            dt.Columns["megsz_tipusa"].ColumnName = "Megsz. típusa";
            dt.Columns["kilep_datum"].ColumnName = "Kilépés dátuma";
            dt.Columns["eredeti_tagsag"].ColumnName = "Eredeti tagság";
            dt.Columns["szamla_lezar"].ColumnName = "Számla lez. dát.";
            dt.Columns["csekket_ker"].ColumnName = "Csekket kér";
            dt.Columns["megjegyzes"].ColumnName = "Megjegyzés";
            dt.Columns["pnr_id"].ColumnName = "Pnr id";
            dt.Columns["penztartag"].ColumnName = "Pénztártag";
            dt.Columns["kedvezmenyezett"].ColumnName = "Kedvezményezett";
            dt.Columns["ervenytelen"].ColumnName = "Érvénytelen";
            dt.Columns["portfolio"].ColumnName = "Portfólió";
        }

        public void rejtId()
        {
            dgwOkiratok.Columns[0].Visible = false;
        }

        private void dgwOkiratok_SelectionChanged(object sender, EventArgs e)
        {
            try
            {
                i = dgwOkiratok.SelectedCells[0].RowIndex;
                index = dgwOkiratok.SelectedCells[0].RowIndex;
                txPnrid.Text = dgwOkiratok.Rows[i].Cells[0].Value.ToString();
                txTszsId.Text = dgwOkiratok.Rows[i].Cells[1].Value.ToString();
                txNev.Text = dgwOkiratok.Rows[i].Cells[2].Value.ToString();
                txAdoazon.Text = dgwOkiratok.Rows[i].Cells[3].Value.ToString();

                txOrszKod.Text = dgwOkiratok.Rows[i].Cells[4].Value.ToString();
                txIrszam.Text = dgwOkiratok.Rows[i].Cells[5].Value.ToString();
                txHelyseg.Text = dgwOkiratok.Rows[i].Cells[6].Value.ToString();
                txCim.Text = dgwOkiratok.Rows[i].Cells[7].Value.ToString();

                txSzulNev.Text = dgwOkiratok.Rows[i].Cells[8].Value.ToString();
                txAnyjaNeve.Text = dgwOkiratok.Rows[i].Cells[9].Value.ToString();
                txSzulHely.Text = dgwOkiratok.Rows[i].Cells[11].Value.ToString();
                cbNeme.Text = dgwOkiratok.Rows[i].Cells[12].Value.ToString();
                cbCsalAll.Text = dgwOkiratok.Rows[i].Cells[13].Value.ToString();

                txErtOrszKod.Text = dgwOkiratok.Rows[i].Cells[14].Value.ToString();
                txErtIrszam.Text = dgwOkiratok.Rows[i].Cells[15].Value.ToString();
                txErtHelyseg.Text = dgwOkiratok.Rows[i].Cells[16].Value.ToString();
                txErtCim.Text = dgwOkiratok.Rows[i].Cells[17].Value.ToString();

                txTelefon.Text = dgwOkiratok.Rows[i].Cells[18].Value.ToString();
                txAllampolg.Text = dgwOkiratok.Rows[i].Cells[19].Value.ToString();
                txEmail.Text = dgwOkiratok.Rows[i].Cells[20].Value.ToString();
                txTitulus.Text = dgwOkiratok.Rows[i].Cells[21].Value.ToString();
                txPenztartag.Text = dgwOkiratok.Rows[i].Cells[22].Value.ToString();
                txKedvezm.Text = dgwOkiratok.Rows[i].Cells[23].Value.ToString();
                txMegjegyzes.Text = dgwOkiratok.Rows[i].Cells[24].Value.ToString();
                cbPortfolio.Text = dgwOkiratok.Rows[i].Cells[40].Value.ToString();

                cbBelepTipusa.Text = dgwOkiratok.Rows[i].Cells[25].Value.ToString();

                cbTagdijfizTipusa.Text = dgwOkiratok.Rows[i].Cells[27].Value.ToString();
                numVallaltTagd.Text = dgwOkiratok.Rows[i].Cells[28].Value.ToString();
                cbEletciklStatusz.Text = dgwOkiratok.Rows[i].Cells[30].Value.ToString();
                cbMegszunesTipusa.Text = dgwOkiratok.Rows[i].Cells[31].Value.ToString();
                txErvenytelen.Text = dgwOkiratok.Rows[i].Cells[39].Value.ToString();
            }
            catch
            {
                // Kezdeti probléma...
            }
            try { dtSzulIdo.Text = DateTime.Parse(dgwOkiratok.Rows[i].Cells[10].Value.ToString()).ToShortDateString(); }
            catch { dtSzulIdo.Text = string.Empty; }
            try { dtBelepDatuma.Text = DateTime.Parse(dgwOkiratok.Rows[i].Cells[26].Value.ToString()).ToShortDateString(); }
            catch { dtBelepDatuma.Text = string.Empty; }
            try { dtKilepesDatuma.Text = DateTime.Parse(dgwOkiratok.Rows[i].Cells[32].Value.ToString()).ToShortDateString(); }
            catch { dtKilepesDatuma.Text = string.Empty; }
            try { dtAlairasDatuma.Text = DateTime.Parse(dgwOkiratok.Rows[i].Cells[33].Value.ToString()).ToShortDateString(); }
            catch { dtAlairasDatuma.Text = string.Empty; }
            try { dtErkezesDatuma.Text = DateTime.Parse(dgwOkiratok.Rows[i].Cells[34].Value.ToString()).ToShortDateString(); }
            catch { dtErkezesDatuma.Text = string.Empty; }
            try { dtFelvetelDatuma.Text = DateTime.Parse(dgwOkiratok.Rows[i].Cells[35].Value.ToString()).ToShortDateString(); }
            catch { dtFelvetelDatuma.Text = string.Empty; }
            try { dtZaradekolasDatuma.Text = DateTime.Parse(dgwOkiratok.Rows[i].Cells[36].Value.ToString()).ToShortDateString(); }
            catch { dtZaradekolasDatuma.Text = string.Empty; }
            try { dtSzamlaLezDatuma.Text = DateTime.Parse(dgwOkiratok.Rows[i].Cells[38].Value.ToString()).ToShortDateString(); }
            catch { dtSzamlaLezDatuma.Text = string.Empty; }

            txNev.ReadOnly = true;
            //txNev.BackColor = System.Drawing.SystemColors.Control;
            txAdoazon.ReadOnly = true;
            txAllampolg.ReadOnly = true;
            txAnyjaNeve.ReadOnly = true;
            txCim.ReadOnly = true;
            txEmail.ReadOnly = true;
            txHelyseg.ReadOnly = true;
            txIrszam.ReadOnly = true;
            txSzulHely.ReadOnly = true;
            txSzulNev.ReadOnly = true;
            txTelefon.ReadOnly = true;
            txErtCim.ReadOnly = true;
            txErtHelyseg.ReadOnly = true;
            txErtIrszam.ReadOnly = true;

            cbCsalAll.Enabled = false;
            cbNeme.Enabled = false;
            dtSzulIdo.Enabled = false;

            txPnrid.ReadOnly = true;
            txTszsId.ReadOnly = true;
            txOrszKod.ReadOnly = true;
            txErtOrszKod.ReadOnly = true;
            txTitulus.ReadOnly = true;
            numVallaltTagd.ReadOnly = true;
            txPenztartag.ReadOnly = true;
            txErvenytelen.ReadOnly = true;
            txKedvezm.ReadOnly = true;
            txTelefon.ReadOnly = true;

            dtBelepDatuma.ReadOnly = true;
            dtAlairasDatuma.ReadOnly = true;
            dtErkezesDatuma.ReadOnly = true;
            dtFelvetelDatuma.ReadOnly = true;
            dtKilepesDatuma.ReadOnly = true;
            dtSzamlaLezDatuma.ReadOnly = true;
            dtZaradekolasDatuma.ReadOnly = true;
            numVallaltTagd.ReadOnly = true;

            cbBelepTipusa.Enabled = false;
            cbEletciklStatusz.Enabled = false;
            cbMegszunesTipusa.Enabled = false;
            cbPortfolio.Enabled = false;
            cbTagdijfizTipusa.Enabled = false;
            dtSzulIdo.Visible = true;
            txSzuldat.Visible = false;
            txSzuldat.ReadOnly = true;

            tsNew.Enabled = true;
            tsDelete.Enabled = true;

            keresomod = false;
            tsFind.Enabled = false;
            tsSave.Enabled = false;
            tsUpdate.Enabled = true;
        }

        private void dgwOkiratok_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
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
            Okiratok ujokirat = new Okiratok(sconn);
            ujokirat.Show();
        }

        public override void save()
        {
            selectedId = int.Parse(txPnrid.Text);
            int x = index;
            // partner adatok tárolása
            scommand = new SqlCommand("spPnrUpdate", sconn);
            scommand.CommandType = CommandType.StoredProcedure;

            // NT=NŐTLEN, H=HAJADON, E=ELVÁLT, N=NŐS, F=FÉRJEZETT, Ö=ÖZVEGY
            scommand.Parameters.Add(new SqlParameter("@pnr_id", SqlDbType.Int)).Value = selectedId;
            scommand.Parameters.Add(new SqlParameter("@adoazonosito_jel", SqlDbType.Decimal, 10)).Value = double.Parse(txAdoazon.Text);
            scommand.Parameters.Add(new SqlParameter("@pnr_tipus", SqlDbType.VarChar, 5)).Value = "SZMLY";
            scommand.Parameters.Add(new SqlParameter("@nev", SqlDbType.VarChar, 50)).Value = txNev.Text;
            scommand.Parameters.Add(new SqlParameter("@anyja_neve", SqlDbType.VarChar, 50)).Value = txAnyjaNeve.Text;
            scommand.Parameters.Add(new SqlParameter("@szul_helye", SqlDbType.VarChar, 20)).Value = txSzulHely.Text;
            scommand.Parameters.Add(new SqlParameter("@szul_dat", SqlDbType.Date)).Value = dtSzulIdo.Value;
            scommand.Parameters.Add(new SqlParameter("@neme", SqlDbType.Decimal, 1)).Value = (cbNeme.Text == "NŐ" ? 2 : 1);

            switch (cbCsalAll.Text)
            {
                case "NŐTLEN": csalAllapot = "NT"; break;
                case "HAJADON": csalAllapot = "H"; break;
                case "ELVÁLT": csalAllapot = "E"; break;
                case "NŐS": csalAllapot = "N"; break;
                case "FÉRJEZETT": csalAllapot = "F"; break;
                case "ÖZVEGY": csalAllapot = "Ö"; break;
                default: break;
            }
            scommand.Parameters.Add(new SqlParameter("@csal_allapot", SqlDbType.VarChar, 15)).Value = csalAllapot;
            scommand.Parameters.Add(new SqlParameter("@ir_szam", SqlDbType.Decimal, 4)).Value = txIrszam.Text;
            scommand.Parameters.Add(new SqlParameter("@helyseg", SqlDbType.VarChar, 20)).Value = txHelyseg.Text;
            scommand.Parameters.Add(new SqlParameter("@cim", SqlDbType.VarChar, 80)).Value = txCim.Text;
            scommand.Parameters.Add(new SqlParameter("@allampolg", SqlDbType.VarChar, 15)).Value = txAllampolg.Text;
            scommand.Parameters.Add(new SqlParameter("@orszagkod", SqlDbType.VarChar, 5)).Value = txOrszKod.Text;
            scommand.Parameters.Add(new SqlParameter("@penztartag", SqlDbType.VarChar, 1)).Value = txPenztartag.Text;
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
            if (txSzulNev.Text != string.Empty)
            {
                scommand = new SqlCommand("update partnerek set leanykori_nev='" + txSzulNev.Text + "' where pnr_id=" + selectedId, sconn);
                try { scommand.ExecuteNonQuery(); }
                catch (Exception ex) { MessageBox.Show("SQL Hiba " + ex.Message); }
            }
            if (txErtIrszam.Text != string.Empty)
            {
                scommand = new SqlCommand("update partnerek set ert_irszam='" + int.Parse(txErtIrszam.Text) + "' where pnr_id=" + selectedId, sconn);
                try { scommand.ExecuteNonQuery(); }
                catch (Exception ex) { MessageBox.Show("SQL Hiba " + ex.Message); }
            }
            if (txErtHelyseg.Text != string.Empty)
            {
                scommand = new SqlCommand("update partnerek set ert_helyseg='" + txErtHelyseg.Text + "' where pnr_id=" + selectedId, sconn);
                try { scommand.ExecuteNonQuery(); }
                catch (Exception ex) { MessageBox.Show("SQL Hiba " + ex.Message); }
            }
            if (txErtCim.Text != string.Empty)
            {
                scommand = new SqlCommand("update partnerek set ert_cim='" + txErtCim.Text + "' where pnr_id=" + selectedId, sconn);
                try { scommand.ExecuteNonQuery(); }
                catch (Exception ex) { MessageBox.Show("SQL Hiba " + ex.Message); }
            }
            if (txTelefon.Text != string.Empty)
            {
                scommand = new SqlCommand("update partnerek set telefon='" + txTelefon.Text + "' where pnr_id=" + selectedId, sconn);
                try { scommand.ExecuteNonQuery(); }
                catch (Exception ex) { MessageBox.Show("SQL Hiba " + ex.Message); }
            }
            if (txEmail.Text != string.Empty)
            {
                scommand = new SqlCommand("update partnerek set email='" + txEmail.Text + "' where pnr_id=" + selectedId, sconn);
                try { scommand.ExecuteNonQuery(); }
                catch (Exception ex) { MessageBox.Show("SQL Hiba " + ex.Message); }
            }
            if (txErtOrszKod.Text != string.Empty)
            {
                scommand = new SqlCommand("update partnerek set ert_orszagkod='" + txErtOrszKod.Text + "' where pnr_id=" + selectedId, sconn);
                try { scommand.ExecuteNonQuery(); }
                catch (Exception ex) { MessageBox.Show("SQL Hiba " + ex.Message); }
            }
            if (txTitulus.Text != string.Empty)
            {
                scommand = new SqlCommand("update partnerek set titulus='" + txTitulus.Text + "' where pnr_id=" + selectedId, sconn);
                try { scommand.ExecuteNonQuery(); }
                catch (Exception ex) { MessageBox.Show("SQL Hiba " + ex.Message); }
            }
            if (txMegjegyzes.Text != string.Empty)
            {
                scommand = new SqlCommand("update partnerek set megjegyzes='" + txMegjegyzes.Text + "' where pnr_id=" + selectedId, sconn);
                try { scommand.ExecuteNonQuery(); }
                catch (Exception ex) { MessageBox.Show("SQL Hiba " + ex.Message); }
            }

            // tszs adatok tárolása
            scommand = new SqlCommand("spTszsUpdate", sconn);
            scommand.CommandType = CommandType.StoredProcedure;

            scommand.Parameters.Add(new SqlParameter("@id", SqlDbType.Int)).Value = int.Parse(txTszsId.Text);
            scommand.Parameters.Add(new SqlParameter("@pnr_id", SqlDbType.Int)).Value = int.Parse(txPnrid.Text);
            scommand.Parameters.Add(new SqlParameter("@belep_tipusa", SqlDbType.VarChar, 15)).Value = cbBelepTipusa.Text;
            scommand.Parameters.Add(new SqlParameter("@belep_datum", SqlDbType.Date)).Value = dtBelepDatuma.Text; // ----

            scommand.Parameters.Add(new SqlParameter("@tagdijfiz_tipus", SqlDbType.VarChar, 30)).Value = cbTagdijfizTipusa.Text;
            scommand.Parameters.Add(new SqlParameter("@vallalt_tagdij", SqlDbType.Decimal, 9)).Value = numVallaltTagd.Text;
            scommand.Parameters.Add(new SqlParameter("@eletcikl_stat", SqlDbType.VarChar, 30)).Value = cbEletciklStatusz.Text;
            scommand.Parameters.Add(new SqlParameter("@megsz_tipusa", SqlDbType.VarChar, 30)).Value = cbMegszunesTipusa.Text;
            scommand.Parameters.Add(new SqlParameter("@portfolio", SqlDbType.VarChar, 1)).Value = cbPortfolio.Text;
            scommand.Parameters.Add(new SqlParameter("@alair_datum", SqlDbType.Date)).Value = DateTime.Parse(dtAlairasDatuma.Text).ToShortDateString();
            scommand.Parameters.Add(new SqlParameter("@erkez_datum", SqlDbType.Date)).Value = DateTime.Parse(dtErkezesDatuma.Text).ToShortDateString();
            scommand.Parameters.Add(new SqlParameter("@felvet_datum", SqlDbType.Date)).Value = DateTime.Parse(dtFelvetelDatuma.Text).ToShortDateString();
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

            // update
            string query;
            try
            {
                if (dtZaradekolasDatuma.Text != string.Empty)
                {
                    query = "update tagsagi_szerzodesek set zaradek_datum='" + dtZaradekolasDatuma.Text.Substring(0, 10) + "' where tszs_id=" + txTszsId.Text;
                    scommand = new SqlCommand(query, sconn);
                    if (sconn.State == ConnectionState.Closed) sconn.Open();
                    scommand.ExecuteNonQuery();
                }
            }
            catch
            {
            }

            try
            {
                if (dtSzamlaLezDatuma.Text != string.Empty)
                {
                    query = "update tagsagi_szerzodesek set szamla_lezar='" + dtSzamlaLezDatuma.Text.Substring(0, 10) + "' where tszs_id=" + txTszsId.Text;
                    scommand = new SqlCommand(query, sconn);
                    if (sconn.State == ConnectionState.Closed) sconn.Open();
                    scommand.ExecuteNonQuery();
                }
            }
            catch
            {
            }

            try
            {
                if (dtKilepesDatuma.Text != string.Empty)
                {
                    query = "update tagsagi_szerzodesek set kilep_datum='" + dtKilepesDatuma.Text.Substring(0, 10) + "' where tszs_id=" + txTszsId.Text;
                    scommand = new SqlCommand(query, sconn);
                    if (sconn.State == ConnectionState.Closed) sconn.Open();
                    scommand.ExecuteNonQuery();
                }
            }
            catch
            {
            }

            // grid frissítése
            scommand = lastScommand;
            da = new SqlDataAdapter(scommand);
            dt = new DataTable();
            Frissit();
            this.dgwOkiratok.DataSource = dt;
            rejtId();

            dgwOkiratok.CurrentCell = dgwOkiratok.Rows[x].Cells[1];

            tsUpdate.Enabled = true;
            tsDelete.Enabled = false;
            tsNew.Enabled = true;
            tsSave.Enabled = false;
            tsFind.Enabled = false;
            tsSearch.Enabled = true;
        }

        public void updateRecord()
        {            
            txPnrid.ReadOnly = true;
            txAdoazon.ReadOnly = false;
            txAllampolg.ReadOnly = false;
            txAnyjaNeve.ReadOnly = false;
            txCim.ReadOnly = false;
            txEmail.ReadOnly = false;
            txHelyseg.ReadOnly = false;
            txIrszam.ReadOnly = false;
            txNev.ReadOnly = false;
            txSzulHely.ReadOnly = false;
            txSzulNev.ReadOnly = false;
            txTelefon.ReadOnly = false;
            txErtCim.ReadOnly = false;
            txErtHelyseg.ReadOnly = false;
            txErtIrszam.ReadOnly = false;

            dtSzulIdo.Visible = true;
            txMegjegyzes.ReadOnly = false;
            txTitulus.ReadOnly = false;
            txOrszKod.ReadOnly = false;
            txErtOrszKod.ReadOnly = false;
            numVallaltTagd.ReadOnly = false;

            cbBelepTipusa.Enabled = true;
            cbEletciklStatusz.Enabled = true;
            cbMegszunesTipusa.Enabled = true;
            cbPortfolio.Enabled = true;
            cbTagdijfizTipusa.Enabled = true;
            cbCsalAll.Enabled = true;
            cbNeme.Enabled = true;
            dtSzulIdo.Enabled = true;

            dtSzulIdo.Visible = true;
            txSzuldat.Visible = false;

            tsNew.Enabled = false;
            tsDelete.Enabled = false;

            tsSave.Enabled = true;
        }

        public override void yellowMode()
        {
            dgwOkiratok.DataSource = null;
            dgwOkiratok.Refresh();

            keresomod = true;
            txNev.ReadOnly = false;

            txPnrid.ReadOnly = false;
            txTszsId.ReadOnly = false;

            txAdoazon.ReadOnly = false;
            txAllampolg.ReadOnly = false;
            txAnyjaNeve.ReadOnly = false;
            txCim.ReadOnly = false;
            txEmail.ReadOnly = false;
            txHelyseg.ReadOnly = false;
            txIrszam.ReadOnly = false;
            txSzulHely.ReadOnly = false;
            txSzulNev.ReadOnly = false;
            txTelefon.ReadOnly = true;
            txErtCim.ReadOnly = false;
            txErtHelyseg.ReadOnly = false;
            txErtIrszam.ReadOnly = false;
            txOrszKod.ReadOnly = false;
            txErtOrszKod.ReadOnly = false;
            txTitulus.ReadOnly = false;
            numVallaltTagd.ReadOnly = false;
            txPenztartag.ReadOnly = false;
            txErvenytelen.ReadOnly = false;
            txKedvezm.ReadOnly = false;
            txTelefon.ReadOnly = false;

            dtBelepDatuma.ReadOnly = false;
            dtAlairasDatuma.ReadOnly = false;
            dtErkezesDatuma.ReadOnly = false;
            dtFelvetelDatuma.ReadOnly = false;
            dtKilepesDatuma.ReadOnly = false;
            dtSzamlaLezDatuma.ReadOnly = false;
            dtZaradekolasDatuma.ReadOnly = false;
            numVallaltTagd.ReadOnly = false;

            cbCsalAll.Enabled = true;
            cbNeme.Enabled = true;
            dtSzulIdo.Enabled = true;

            cbBelepTipusa.Enabled = true;
            cbEletciklStatusz.Enabled = true;
            cbMegszunesTipusa.Enabled = true;
            cbPortfolio.Enabled = true;
            cbTagdijfizTipusa.Enabled = true;

            dtSzulIdo.Visible = false;
            txSzuldat.Visible = true;
            txSzuldat.ReadOnly = false;

            txPnrid.Text = string.Empty;
            txTszsId.Text = string.Empty;

            txAdoazon.Text = string.Empty;
            txAllampolg.Text = string.Empty;
            txAnyjaNeve.Text = string.Empty;
            txCim.Text = string.Empty;
            txEmail.Text = string.Empty;
            txHelyseg.Text = string.Empty;
            txIrszam.Text = string.Empty;
            txNev.Text = string.Empty;
            txPnrid.Text = string.Empty;
            txSzulHely.Text = string.Empty;
            txSzulNev.Text = string.Empty;
            txTelefon.Text = string.Empty;
            txErtCim.Text = string.Empty;
            txErtHelyseg.Text = string.Empty;
            txErtIrszam.Text = string.Empty;
            txOrszKod.Text = string.Empty;
            txErtOrszKod.Text = string.Empty;

            cbNeme.Text = string.Empty;
            cbCsalAll.Text = string.Empty;
            dtBelepDatuma.Text = string.Empty;
            dtAlairasDatuma.Text = string.Empty;
            dtErkezesDatuma.Text = string.Empty;
            dtFelvetelDatuma.Text = string.Empty;
            dtKilepesDatuma.Text = string.Empty;
            dtSzamlaLezDatuma.Text = string.Empty;
            dtZaradekolasDatuma.Text = string.Empty;
            dtSzulIdo.Text = string.Empty;
            numVallaltTagd.Text = string.Empty;
            cbBelepTipusa.Text = string.Empty;
            cbEletciklStatusz.Text = string.Empty;
            cbMegszunesTipusa.Text = string.Empty;
            cbPortfolio.Text = string.Empty;
            cbTagdijfizTipusa.Text = string.Empty;
            txPenztartag.Text = string.Empty;

            tsUpdate.Enabled = false;
            tsDelete.Enabled = false;
            tsNew.Enabled = false;
            tsSave.Enabled = false;
            tsFind.Enabled = true;
            txNev.Focus();
        }

        public override void runQuery()
        {
            if (!keresomod)
            {
                MessageBox.Show("Nem keresőmód!", "Megerősítés", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                this.dgwOkiratok.DataSource = null;

                // A LEKÉRDEZÉS FELÉPÍTÉSE
                querystring = "SELECT pnr.pnr_id,tszs.tszs_id,pnr.nev,pnr.adoazonosito_jel," +
                    "pnr.orszagkod,pnr.ir_szam,pnr.helyseg,pnr.cim," +
                    "pnr.leanykori_nev,pnr.anyja_neve,pnr.szul_dat,pnr.szul_helye," +
                    "CASE pnr.neme WHEN 1 THEN 'FÉRFI' WHEN 2 THEN 'NŐ' END AS neme," +
                    "CASE pnr.csal_allapot WHEN 'NT' THEN 'NŐTLEN' WHEN 'H' THEN 'HAJADON' WHEN 'E' THEN 'ELVÁLT' WHEN 'N' THEN 'NŐS' " +
                    "WHEN 'F' THEN 'FÉRJEZETT' WHEN 'Ö' THEN 'ÖZVEGY' END AS csal_allapot," +
                    "pnr.ert_orszagkod,pnr.ert_irszam,pnr.ert_helyseg,pnr.ert_cim,pnr.telefon,pnr.allampolg,pnr.email," +
                    "pnr.titulus,pnr.penztartag,pnr.kedvezmenyezett,pnr.megjegyzes," +
                    "tszs.belep_tipusa,tszs.belep_datum,tszs.tagdijfiz_tipus,tszs.vallalt_tagdij,tszs.csekket_ker,tszs.eletcikl_stat," +
                    "tszs.megsz_tipusa,tszs.kilep_datum,tszs.alair_datum,tszs.erkez_datum," +
                    "tszs.felvet_datum,tszs.zaradek_datum,tszs.eredeti_tagsag,tszs.szamla_lezar,pnr.ervenytelen,tszs.portfolio " +
                    "FROM partnerek pnr left outer join tagsagi_szerzodesek tszs on pnr.pnr_id=tszs.pnr_id WHERE (tszs.zaradek_datum is null or pnr.ervenytelen='I') and ";
                querystring = txNev.Text != string.Empty ? querystring + "nev like '" + txNev.Text + "' and " : querystring;
                querystring = (txPnrid.Text != string.Empty ? querystring + "pnr.pnr_id like '" + txPnrid.Text + "' and " : querystring);
                querystring = (txTszsId.Text != string.Empty ? querystring + "tszs.tszs_id like '" + txTszsId.Text + "%' and " : querystring);

                querystring = (txSzulHely.Text != string.Empty ? querystring + "szul_helye like '" + txSzulHely.Text + "' and " : querystring);
                querystring = (txSzuldat.Text != string.Empty ? querystring + "szul_dat like '" + txSzuldat.Text + "' and " : querystring);
                querystring = (cbNeme.Text != string.Empty ? querystring + "neme like '" + cbNeme.Text + "' and " : querystring);
                querystring = (cbCsalAll.Text != string.Empty ? querystring + "csal_allapot like '" + cbCsalAll.Text + "' and " : querystring);
                querystring = (txAnyjaNeve.Text != string.Empty ? querystring + "anyja_neve like '" + txAnyjaNeve.Text + "' and " : querystring);
                querystring = (txAllampolg.Text != string.Empty ? querystring + "allampolg like '" + txAllampolg.Text + "' and " : querystring);
                querystring = (txSzulNev.Text != string.Empty ? querystring + "leanykori_nev like '" + txSzulNev.Text + "' and " : querystring);

                querystring = (txAdoazon.Text != string.Empty ? querystring + "adoazonosito_jel like '" + txAdoazon.Text + "' and " : querystring);
                querystring = (txIrszam.Text != string.Empty ? querystring + "ir_szam like '" + txIrszam.Text + "' and " : querystring);
                querystring = (txHelyseg.Text != string.Empty ? querystring + "helyseg like '" + txHelyseg.Text + "' and " : querystring);
                querystring = (txCim.Text != string.Empty ? querystring + "cim like '" + txCim.Text + "' and " : querystring);
                querystring = (txErtIrszam.Text != string.Empty ? querystring + "ert_irszam like '" + txErtIrszam.Text + "' and " : querystring);
                querystring = (txErtHelyseg.Text != string.Empty ? querystring + "ert_helyseg like '" + txErtHelyseg.Text + "' and " : querystring);
                querystring = (txErtCim.Text != string.Empty ? querystring + "ert_cim like '" + txErtCim.Text + "' and " : querystring);
                querystring = (txOrszKod.Text != string.Empty ? querystring + "orszagkod like '" + txOrszKod.Text + "' and " : querystring);
                querystring = (txErtOrszKod.Text != string.Empty ? querystring + "ert_orszagkod like '" + txErtOrszKod.Text + "' and " : querystring);
                querystring = (txTitulus.Text != string.Empty ? querystring + "titulus like '" + txTitulus.Text + "' and " : querystring);
                querystring = (txErvenytelen.Text != string.Empty ? querystring + "ervenytelen like '" + txErvenytelen.Text + "' and " : querystring);
                querystring = (txKedvezm.Text != string.Empty ? querystring + "kedvezmenyezett like '" + txKedvezm.Text + "' and " : querystring);
                querystring = (txPenztartag.Text != string.Empty ? querystring + "penztartag like '" + txPenztartag.Text + "' and " : querystring);

                querystring = (cbBelepTipusa.Text != string.Empty ? querystring + "tszs.belep_tipusa like '" + cbBelepTipusa.Text + "' and " : querystring);
                querystring = (cbEletciklStatusz.Text != string.Empty ? querystring + "tszs.eletcikl_stat like '" + cbEletciklStatusz.Text + "' and " : querystring);
                querystring = (cbMegszunesTipusa.Text != string.Empty ? querystring + "tszs.megsz_tipusa like '" + cbMegszunesTipusa.Text + "' and " : querystring);
                querystring = (cbTagdijfizTipusa.Text != string.Empty ? querystring + "tszs.tagdijfiz_tipus like '" + cbTagdijfizTipusa.Text + "' and " : querystring);
                querystring = (cbPortfolio.Text != string.Empty ? querystring + "tszs.portfolio like '" + cbPortfolio.Text + "' and " : querystring);
                querystring = (dtAlairasDatuma.Text != string.Empty ? querystring + "tszs.alair_datum like '" + dtAlairasDatuma.Text + "%' and " : querystring);
                querystring = (dtBelepDatuma.Text != string.Empty ? querystring + "tszs.belep_datum like '" + dtBelepDatuma.Text + "%' and " : querystring);
                querystring = (dtErkezesDatuma.Text != string.Empty ? querystring + "tszs.erkez_datum like '" + dtErkezesDatuma.Text + "%' and " : querystring);
                querystring = (dtFelvetelDatuma.Text != string.Empty ? querystring + "tszs.felvet_datum like '" + dtFelvetelDatuma.Text + "%' and " : querystring);
                querystring = (dtKilepesDatuma.Text != string.Empty ? querystring + "tszs.kilep_datum like '" + dtKilepesDatuma.Text + "%' and " : querystring);
                querystring = (dtSzamlaLezDatuma.Text != string.Empty ? querystring + "tszs.szamla_lezar like '" + dtSzamlaLezDatuma.Text + "%' and " : querystring);
                querystring = (dtZaradekolasDatuma.Text != string.Empty ? querystring + "tszs.zaradek_datum like '" + dtZaradekolasDatuma.Text + "%' and " : querystring);

                querystring += "pnr_tipus='SZMLY' order by nev;";

                scommand = new SqlCommand(querystring, sconn);
                //lastScommand = scommand;
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
                this.dgwOkiratok.DataSource = dt;
                lastScommand = scommand;
            }

            tsUpdate.Enabled = true;
            tsDelete.Enabled = false;
            tsNew.Enabled = true;
            tsSave.Enabled = false;
            tsFind.Enabled = false;
            tsSearch.Enabled = true;
        }

        public override void tsDelete_Click(object sender, EventArgs e)
        {
            // törlés
            // törölni csak olyan partnert lehet aki nem volt még soha záradékolva, nem pénztártag (érvénytelenített sem) és nem kedvezményezett
            // vagyis csak a záradékolatlan okiratok
            DialogResult dr = MessageBox.Show("Biztosan törölni akarja a kijelölt rekordot?\nPnr Id: " + txPnrid.Text, "Megerősítés", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
            if (dr == DialogResult.OK)
            {
                if (dtZaradekolasDatuma.Text == string.Empty && txErvenytelen.Text == string.Empty && txKedvezm.Text == string.Empty)
                {
                    string query = "delete from partnerek where pnr_id=" + txPnrid.Text;
                    scommand = new SqlCommand(query, sconn);
                    scommand.ExecuteNonQuery();
                    int i = dgwOkiratok.SelectedCells[0].RowIndex;
                    dgwOkiratok.Rows.RemoveAt(i);
                    MessageBox.Show("A partner törlése sikeres.");
                }
                else
                    MessageBox.Show("A partner nem törölhető!");
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

        private void cbNeme_KeyPress(object sender, KeyPressEventArgs e)
        {
            cbNeme.ResetText();
            cbNeme.Refresh();
        }

        private void cbNeme_Leave(object sender, EventArgs e)
        {
            bool ok = false;
            for (int i = 0; i < lista.Count; i++)
            {
                if (cbNeme.Text == lista[i])
                {
                    ok = true;
                    break;
                }
            }
            if (!ok)
            {
                cbNeme.ResetText();
                cbNeme.DataSource = lista;
                cbNeme.Text = lista[0];
                cbNeme.Focus();
            }
        }

        private void cbCsalAll_KeyPress(object sender, KeyPressEventArgs e)
        {
            cbCsalAll.ResetText();
            cbCsalAll.Refresh();
        }

        private void cbCsalAll_Leave(object sender, EventArgs e)
        {
            bool ok = false;
            for (int i = 0; i < csalAllapotLista.Count; i++)
            {
                if (cbCsalAll.Text == csalAllapotLista[i])
                {
                    ok = true;
                    break;
                }
            }
            if (!ok)
            {
                cbCsalAll.ResetText();
                cbCsalAll.DataSource = csalAllapotLista;
                cbCsalAll.Text = csalAllapotLista[0];
                cbCsalAll.Focus();
            }
        }

        public void exit()
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
            myListener.WriteLine("- " + DateTime.Now + ": " + msg);
            myListener.Flush();
            myListener.Close();
        }

        private void panel1_Resize(object sender, EventArgs e)
        {
            dgwOkiratok.Dock = DockStyle.Fill;
        }

        private void OkiratokModositas_Resize(object sender, EventArgs e)
        {
            panel1.Width = OkiratokModositas.ActiveForm.Width - 77;
        }

        private void txSzuldat_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void dgwOkiratok_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                try
                {
                    e.SuppressKeyPress = true;
                    int iColumn = dgwOkiratok.CurrentCell.ColumnIndex;
                    int iRow = dgwOkiratok.CurrentCell.RowIndex;
                    if (iColumn != dgwOkiratok.Columns.Count - 1)
                        dgwOkiratok.CurrentCell = dgwOkiratok[iColumn, iRow];
                }
                catch
                { }
            }
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
        }

        private void dgwOkiratok_MouseMove(object sender, MouseEventArgs e)
        {
            DataGridView.HitTestInfo hit = dgwOkiratok.HitTest(e.X, e.Y);
            if (hit.Type == DataGridViewHitTestType.Cell)
            {
                dgwOkiratok.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                dgwOkiratok.DefaultCellStyle.SelectionBackColor = Color.LightYellow;
                dgwOkiratok.DefaultCellStyle.SelectionForeColor = Color.DarkBlue;
            }
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

        private void txErtIrszam_Leave(object sender, EventArgs e)
        {
            if (txErtIrszam.Text != string.Empty)
            {
                // település beolvasás
                scommand = new SqlCommand("SELECT telepules FROM iranyitoszamok where irszam=" + txIrszam.Text, sconn);
                if (sconn.State == ConnectionState.Closed) sconn.Open();
                SqlDataReader sqlReader = scommand.ExecuteReader();

                while (sqlReader.Read())
                {
                    txErtHelyseg.Text = (sqlReader["telepules"].ToString());
                }

                sqlReader.Close();
            }
        }
    }
}
