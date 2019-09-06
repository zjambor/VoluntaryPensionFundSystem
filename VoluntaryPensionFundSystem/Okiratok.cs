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
    public partial class Okiratok : VoluntaryPensionFundSystem.BaseForm
    {
        private SqlConnection sconn;
        private SqlCommand scommand;

        private List<string> lista = new List<string>();
        private List<string> csalAllapotLista = new List<string>();
        private List<string> tagdijfiz = new List<string>();
        private bool irszamHiba = false;
        private string csalAllapot = string.Empty;
        TextWriterTraceListener myListener = new TextWriterTraceListener(@"LOG\TagsagiOkiratokRogzitese.log", "myListener");

        public const string MatchEmailPattern = @"^(([\w-]+\.)+[\w-]+|([a-zA-Z]{1}|[\w-]{2,}))@" + @"(([a-zA-Z]+[\w-]+\.)+[a-zA-Z]{2,4})$";
        public const string MatchTelPattern = @"^([0-9\+][0-9\-]+)|([0-9]+)\/" + @"([0-9\-]+)$";
        public const string MatchAdoszamPattern = @"^[0-9]{10}$";
        public const string MatchSzamlaszamPattern = @"^[0-9]{24}$";

        public int PnrId, FoglId, paj;
        public string megnevezes, irszam, foglcim, helyseg, telszam, adoszam;
        public string nev, adoazon, pnrid;
        public DateTime alair, erkezes;
        public string ptrid, ptrnev;
        public bool zaradekolva = false;
        //bool keresomod = false;


        public Okiratok(SqlConnection SqlConn)
        {
            InitializeComponent();

            lista.Add("FÉRFI");
            lista.Add("NŐ");
            cbNeme.DataSource = lista;
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

            this.sconn = SqlConn;

            tsDelete.Enabled = false;
            tsFind.Enabled = false;
            tsNew.Enabled = true;
            tsSave.Enabled = false;
            tsSearch.Enabled = false;
            tsUpdate.Enabled = false;
            bZaradekolas.Enabled = false;

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

            // portfóliók
            scommand = new SqlCommand("SELECT * FROM befektetesi_kombinaciok", sconn);
            if (sconn.State == ConnectionState.Closed) sconn.Open();
            sqlReader = scommand.ExecuteReader();

            while (sqlReader.Read())
            {
                cbPortfolio.Items.Add(sqlReader["tipus"].ToString());
            }

            sqlReader.Close();
            
        }        

        // Funkciógombok

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

        public override void tsExit_Click(object sender, EventArgs e)
        {
            if (tsSave.Enabled == true)
            {
                DialogResult dr = MessageBox.Show("Minden nem mentett adat el fog veszni! Biztosan kilép?", "Megerősítés", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
                if (dr == DialogResult.OK)
                    Close();
            }
            else Close();
        }

        // metódusok

        public override void createNew()
        {
            if (tsSave.Enabled == true)
            {
                DialogResult dr = MessageBox.Show("Minden nem mentett adat el fog veszni!", "Megerősítés", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
                if (dr == DialogResult.OK)
                {
                    kiurit();
                }
            }
            else
                kiurit();
        }

        public override void save()
        {
            //bool uresmezo = true;

            // ELLENŐRZÉSEK
            if (cbEletciklStatusz.Text == string.Empty) { MessageBox.Show("Életcikl. státusz nincs kiválasztva!"); cbEletciklStatusz.Focus(); return; }
            if (cbPortfolio.Text == string.Empty) { MessageBox.Show("Portfólió nincs kiválasztva!"); cbPortfolio.Focus(); return; }
            if (cbBelepTipusa.Text == string.Empty) { MessageBox.Show("Belépés típusa nincs kiválasztva!"); cbBelepTipusa.Focus(); return; }
            if (cbTagdijfizTipusa.Text == string.Empty) { MessageBox.Show("Tagdíjfizetés típusa nincs kiválasztva!"); cbTagdijfizTipusa.Focus(); return; }
            if (IsSzamlaszam(txSzamlaszam.Text)) { MessageBox.Show("A számlaszám hibás!"); txSzamlaszam.Focus(); return; }
            if (txNev.Text == string.Empty || txAnyjaNeve.Text == string.Empty || txSzulHely.Text == string.Empty || cbCsalAll.Text == string.Empty ||
                txHelyseg.Text == string.Empty || txIrszam.Text == string.Empty || txCim.Text == string.Empty || txAdoazon.Text == string.Empty ||
                txAllampolg.Text == string.Empty)
            {
                MessageBox.Show("Egyik mező sem lehet üres:\n-Név\n-Anyja neve\n-Születési hely\n-Családi állapot\n-Irányítószám\n-Helység\n-Cím\n-Adóazonosító jel\n-Állampolgárság");
                txNev.Focus();
                return;
            }
            if (irszamHiba) { MessageBox.Show("Hibás irányítószám!"); txIrszam.Focus(); return; }         // az ir. szám ellenőrzések külön metódusban
            if (!(txEmail.Text == string.Empty) && !(IsEmail(txEmail.Text))) { MessageBox.Show("Az email cím nem megfelelő!"); txEmail.Focus(); return; }
            if (!(txTelefon.Text == string.Empty) && !(IsTelszam(txTelefon.Text))) { MessageBox.Show("A telefonszám nem megfelelő! Formátum: körzetszám/telefonszám"); txTelefon.Focus(); return; }
            if (!IsAdoszam(txAdoazon.Text)) { MessageBox.Show("Az adóazonosító jel hibás!"); txAdoazon.Focus(); return; }
            if (txFoglId.Text == string.Empty || txFoglId.Text == "0")
            {
                DialogResult dr = MessageBox.Show("Nem adott meg munkáltatót! Biztosan menti az okiratot?", "Megerősítés", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
                if (dr == DialogResult.Cancel)
                {
                    return;
                }
            }

            // partner adatok tárolása
            int newPnrid = 0;

            // INSERT - új rekord létrohozása, az új rekord id-jét adja vissza
            newPnrid = insertNewPartner();
            if (newPnrid == 0) { MessageBox.Show("A rekord mentése nem sikerült!"); txNev.Focus(); return; }
            else
            {
                txPnrid.Text = newPnrid.ToString();
                PnrId = newPnrid;
            }


            // UPDATE
            if (txSzulNev.Text != string.Empty)
            {
                scommand = new SqlCommand("update partnerek set leanykori_nev='" + txSzulNev.Text + "' where pnr_id=" + newPnrid, sconn);
                try { scommand.ExecuteNonQuery(); }
                catch (Exception ex) { MessageBox.Show("SQL Hiba " + ex.Message); }
            }
            if (txErtIrszam.Text != string.Empty)
            {
                scommand = new SqlCommand("update partnerek set ert_irszam='" + int.Parse(txErtIrszam.Text) + "' where pnr_id=" + newPnrid, sconn);
                try { scommand.ExecuteNonQuery(); }
                catch (Exception ex) { MessageBox.Show("SQL Hiba " + ex.Message); }
            }
            if (txErtHelyseg.Text != string.Empty)
            {
                scommand = new SqlCommand("update partnerek set ert_helyseg='" + txErtHelyseg.Text + "' where pnr_id=" + newPnrid, sconn);
                try { scommand.ExecuteNonQuery(); }
                catch (Exception ex) { MessageBox.Show("SQL Hiba " + ex.Message); }
            }
            if (txErtCim.Text != string.Empty)
            {
                scommand = new SqlCommand("update partnerek set ert_cim='" + txErtCim.Text + "' where pnr_id=" + newPnrid, sconn);
                try { scommand.ExecuteNonQuery(); }
                catch (Exception ex) { MessageBox.Show("SQL Hiba " + ex.Message); }
            }
            if (txTelefon.Text != string.Empty)
            {
                scommand = new SqlCommand("update partnerek set telefon='" + txTelefon.Text + "' where pnr_id=" + newPnrid, sconn);
                try { scommand.ExecuteNonQuery(); }
                catch (Exception ex) { MessageBox.Show("SQL Hiba " + ex.Message); }
            }
            if (txEmail.Text != string.Empty)
            {
                scommand = new SqlCommand("update partnerek set email='" + txEmail.Text + "' where pnr_id=" + newPnrid, sconn);
                try { scommand.ExecuteNonQuery(); }
                catch (Exception ex) { MessageBox.Show("SQL Hiba " + ex.Message); }
            }
            if (txErtOrszKod.Text != string.Empty)
            {
                scommand = new SqlCommand("update partnerek set ert_orszagkod='" + txErtOrszKod.Text + "' where pnr_id=" + newPnrid, sconn);
                try { scommand.ExecuteNonQuery(); }
                catch (Exception ex) { MessageBox.Show("SQL Hiba " + ex.Message); }
            }
            if (txTitulus.Text != string.Empty)
            {
                scommand = new SqlCommand("update partnerek set titulus='" + txTitulus.Text + "' where pnr_id=" + newPnrid, sconn);
                try { scommand.ExecuteNonQuery(); }
                catch (Exception ex) { MessageBox.Show("SQL Hiba " + ex.Message); }
            }
            if (txMegjegyzes.Text != string.Empty)
            {
                scommand = new SqlCommand("update partnerek set megjegyzes='" + txMegjegyzes.Text + "' where pnr_id=" + newPnrid, sconn);
                try { scommand.ExecuteNonQuery(); }
                catch (Exception ex) { MessageBox.Show("SQL Hiba " + ex.Message); }
            }


            // tszs adatok tárolása
            int newTszsid = 0;
            // INSERT - új rekord létrohozása, az új rekord id-jét adja vissza
            newTszsid = insertNewContract(newPnrid);
            //txPaj.Text = newTszsid.ToString();
            paj = newTszsid;

            // számlaszám mentés
            if (txSzamlaszam.Text != string.Empty)
            {
                scommand = new SqlCommand("spBszlaInsert", sconn);
                scommand.CommandType = CommandType.StoredProcedure;

                scommand.Parameters.Add(new SqlParameter("@id", SqlDbType.Int)).Value = 1;
                scommand.Parameters.Add(new SqlParameter("@pnr_id", SqlDbType.Int)).Value = newPnrid;
                scommand.Parameters.Add(new SqlParameter("@szamlaszam", SqlDbType.VarChar, 24)).Value = txSzamlaszam.Text;
                scommand.Parameters.Add(new SqlParameter("@ervenyes", SqlDbType.VarChar, 1)).Value = "I";
                scommand.Parameters.Add(new SqlParameter("@modosit_neve", SqlDbType.VarChar, 80)).Value = User.name;

                try
                {
                    if (sconn.State == ConnectionState.Closed) sconn.Open();
                    scommand.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("SQL Hiba " + ex.Message + ex.Data + "insert");
                    TraceBejegyzes(ex.Message);
                }
            }

            // munkaviszony adatok mentése
            if (txFoglId.Text != string.Empty)
            {
                int mvnyid = 0;
                scommand = new SqlCommand("spMvInsert", sconn);
                scommand.CommandType = CommandType.StoredProcedure;
                scommand.Parameters.Add(new SqlParameter("@id", SqlDbType.Int)).Value = 1;
                scommand.Parameters.Add(new SqlParameter("@tszs_id", SqlDbType.Int)).Value = newTszsid;
                scommand.Parameters.Add(new SqlParameter("@pnr_id", SqlDbType.Int)).Value = int.Parse(txPnrid.Text);
                scommand.Parameters.Add(new SqlParameter("@alk_kezdete", SqlDbType.Date)).Value = DateTime.Parse(dtMvnyKezd.Text);
                scommand.Parameters.Add(new SqlParameter("@modosit_neve", SqlDbType.VarChar, 80)).Value = User.name;

                try
                {
                    mvnyid = (int)scommand.ExecuteScalar();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("SQL hiba: " + ex.Message);
                    TraceBejegyzes(ex.Message);
                }

                // UPDATE
                if (mvnyid > 0)
                {
                    if (nHozzajarulasMerteke.Value > 0)
                    {
                        scommand = new SqlCommand("UPDATE munkaviszonyok SET hozzajarulas=" + nHozzajarulasMerteke.Value.ToString() + " where mvny_id=" + mvnyid, sconn);
                        try { scommand.ExecuteNonQuery(); }
                        catch (Exception ex) { MessageBox.Show("SQL hiba: " + ex.Message); TraceBejegyzes(ex.Message); }
                    }

                    if (nTagdijMertek.Value > 0)
                    {
                        scommand = new SqlCommand("UPDATE munkaviszonyok SET tagdij_szazalek=" + nTagdijMertek.Value.ToString() + " where mvny_id=" + mvnyid, sconn);
                        try { scommand.ExecuteNonQuery(); }
                        catch (Exception ex) { MessageBox.Show("SQL hiba: " + ex.Message); TraceBejegyzes(ex.Message); }
                    }

                    if (nGyakorisag.Value > 0)
                    {
                        scommand = new SqlCommand("UPDATE munkaviszonyok SET gyakorisag=" + nGyakorisag.Value.ToString() + " where mvny_id=" + mvnyid, sconn);
                        try { scommand.ExecuteNonQuery(); }
                        catch (Exception ex) { MessageBox.Show("SQL hiba: " + ex.Message); TraceBejegyzes(ex.Message); }
                    }
                }
                else
                {
                    MessageBox.Show("A munkaviszony mentése nem sikerült!");
                }
            }

            tsSave.Enabled = false;

            bFoglalkoztatok.Enabled = false;
            bFoglClear.Enabled = false;

            bZaradekolas.Enabled = true;

            //MentesKesz();
        }               

        private int insertNewPartner()
        {
            Int32 newid = 0;
            scommand = new SqlCommand("spPnrInsert", sconn);
            scommand.CommandType = CommandType.StoredProcedure;
            // NT=NŐTLEN, H=HAJADON, E=ELVÁLT, N=NŐS, F=FÉRJEZETT, Ö=ÖZVEGY
            scommand.Parameters.Add(new SqlParameter("@id", SqlDbType.Int)).Value = 1;
            scommand.Parameters.Add(new SqlParameter("@adoazonosito_jel", SqlDbType.Decimal, 10)).Value = double.Parse(txAdoazon.Text);
            scommand.Parameters.Add(new SqlParameter("@pnr_tipus", SqlDbType.VarChar, 5)).Value = "SZMLY";  //
            scommand.Parameters.Add(new SqlParameter("@nev", SqlDbType.VarChar, 50)).Value = txNev.Text;
            scommand.Parameters.Add(new SqlParameter("@anyja_neve", SqlDbType.VarChar, 50)).Value = txAnyjaNeve.Text;
            scommand.Parameters.Add(new SqlParameter("@szul_helye", SqlDbType.VarChar, 20)).Value = txSzulHely.Text;
            scommand.Parameters.Add(new SqlParameter("@szul_dat", SqlDbType.Date)).Value = dtSzulIdo.Value; // dátum!
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
            scommand.Parameters.Add(new SqlParameter("@ir_szam", SqlDbType.Decimal, 4)).Value = int.Parse(txIrszam.Text);
            scommand.Parameters.Add(new SqlParameter("@helyseg", SqlDbType.VarChar, 20)).Value = txHelyseg.Text;
            scommand.Parameters.Add(new SqlParameter("@cim", SqlDbType.VarChar, 80)).Value = txCim.Text;
            scommand.Parameters.Add(new SqlParameter("@allampolg", SqlDbType.VarChar, 15)).Value = txAllampolg.Text;
            scommand.Parameters.Add(new SqlParameter("@orszagkod", SqlDbType.VarChar, 5)).Value = txOrszKod.Text;
            scommand.Parameters.Add(new SqlParameter("@penztartag", SqlDbType.VarChar, 1)).Value = "I";
            scommand.Parameters.Add(new SqlParameter("@rogzit_neve", SqlDbType.VarChar, 80)).Value = User.name;
            scommand.Parameters.Add(new SqlParameter("@modosit_neve", SqlDbType.VarChar, 80)).Value = User.name;

            try
            {
                if (sconn.State == ConnectionState.Closed) sconn.Open();
                newid = (Int32)scommand.ExecuteScalar();               
            }
            catch (Exception ex)
            {
                MessageBox.Show("SQL Hiba " + ex.Message);
            }
            return (int)newid;
        }

        private int insertNewContract(int partnerid)
        {
            Int32 newid = 0;
            scommand = new SqlCommand("spTszsInsert", sconn);
            scommand.CommandType = CommandType.StoredProcedure;

            scommand.Parameters.Add(new SqlParameter("@id", SqlDbType.Int)).Value = 1;
            scommand.Parameters.Add(new SqlParameter("@pnr_id", SqlDbType.Int)).Value = partnerid;

            scommand.Parameters.Add(new SqlParameter("@belep_tipusa", SqlDbType.VarChar, 30)).Value = cbBelepTipusa.Text;
            scommand.Parameters.Add(new SqlParameter("@belep_datum", SqlDbType.Date)).Value = dtBelep.Value;
            scommand.Parameters.Add(new SqlParameter("@tagdijfiz_tipus", SqlDbType.VarChar, 30)).Value = cbTagdijfizTipusa.Text;
            scommand.Parameters.Add(new SqlParameter("@vallalt_tagdij", SqlDbType.Decimal, 9)).Value = int.Parse(numVallaltTagd.Text);
            scommand.Parameters.Add(new SqlParameter("@eletcikl_stat", SqlDbType.VarChar, 30)).Value = cbEletciklStatusz.Text;
            scommand.Parameters.Add(new SqlParameter("@alair_datum", SqlDbType.Date)).Value = dtAlair.Value;
            scommand.Parameters.Add(new SqlParameter("@erkez_datum", SqlDbType.Date)).Value = dtErkezes.Value;
            scommand.Parameters.Add(new SqlParameter("@felvet_datum", SqlDbType.Date)).Value = dtFelvet.Value;
            scommand.Parameters.Add(new SqlParameter("@csekket_ker", SqlDbType.VarChar, 1)).Value = (chCsekketker.Checked == true ? "I" : string.Empty);
            scommand.Parameters.Add(new SqlParameter("@megjegyzes", SqlDbType.VarChar, 255)).Value = txMegjegyzes.Text;
            scommand.Parameters.Add(new SqlParameter("@portfolio", SqlDbType.VarChar, 1)).Value = cbPortfolio.Text;
            scommand.Parameters.Add(new SqlParameter("@rogzit_neve", SqlDbType.VarChar, 80)).Value = User.name;
            scommand.Parameters.Add(new SqlParameter("@modosit_neve", SqlDbType.VarChar, 80)).Value = User.name;

            try
            {
                if (sconn.State == ConnectionState.Closed) sconn.Open();
                newid = (Int32)scommand.ExecuteScalar();
            }
            catch (Exception ex)
            {
                MessageBox.Show("SQL Hiba " + ex.Message);
            }
            return (int)newid;
        }

        private void MentesKesz()
        {
            txAdoazon.ReadOnly = true;
            txAllampolg.ReadOnly = true;
            txAnyjaNeve.ReadOnly = true;
            txCim.ReadOnly = true;
            txEmail.ReadOnly = true;
            txHelyseg.ReadOnly = true;
            txIrszam.ReadOnly = true;
            txNev.ReadOnly = true;
            txSzulHely.ReadOnly = true;
            txSzulNev.ReadOnly = true;
            txTelefon.ReadOnly = true;

            txErtCim.ReadOnly = true;
            txErtHelyseg.ReadOnly = true;
            txErtIrszam.ReadOnly = true;
            nTagdijMertek.ReadOnly = true;
            nHozzajarulasMerteke.ReadOnly = true;
            nGyakorisag.ReadOnly = true;
            dtSzulIdo.Enabled = false;
            dtMvnyKezd.Enabled = false;
            cbCsalAll.Enabled = false;
            cbNeme.Enabled = false;
            dtAlair.Enabled = false;
            dtErkezes.Enabled = false;

            bFoglalkoztatok.Enabled = false;
            bFoglClear.Enabled = false;
            tsSave.Enabled = false;
            bZaradekolas.Enabled = true;
        }

        public override void update()
        {
            // nincs update
        }        

        private void kiurit()
        {
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
            nTagdijMertek.Value = 0;
            nHozzajarulasMerteke.Value = 0;
            nGyakorisag.Value = 0;
            dtSzulIdo.Value = DateTime.Now;
            dtMvnyKezd.Value = DateTime.Now;

            cbCsalAll.Text = csalAllapotLista[0];
            cbNeme.Text = lista[0];

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
            nTagdijMertek.ReadOnly = false;
            nHozzajarulasMerteke.ReadOnly = false;
            nGyakorisag.ReadOnly = false;
            dtSzulIdo.Enabled = true;
            dtMvnyKezd.Enabled = true;
            cbCsalAll.Enabled = true;
            cbNeme.Enabled = true;
            dtAlair.Enabled = true;
            dtErkezes.Enabled = true;

            txAdoszam.Text = string.Empty;
            txFoglId.Text = string.Empty;
            txFoglIrszam.Text = string.Empty;
            txFoglHelyseg.Text = string.Empty;
            txFoglCim.Text = string.Empty;
            txMegnevezes.Text = string.Empty;
            txTelszam.Text = string.Empty;
            txMegjegyzes.Text = string.Empty;

            dtSzulIdo.ResetText();
            dtErkezes.ResetText();
            dtAlair.ResetText();
            dtMvnyKezd.ResetText();

            bZaradekolas.Enabled = false;
            labelZaradek.Visible = false;
            txPaj.Text = string.Empty;
            txPaj.Visible = false;

            bFoglalkoztatok.Enabled = true;
            bFoglClear.Enabled = true;
            bZaradekolas.Enabled = false;
            tsSave.Enabled = false;
        }

        private void Okiratok_Load(object sender, EventArgs e)
        {
        }

        private void bFoglalkoztatok_Click(object sender, EventArgs e)
        {
            Munkaviszony ujMunkaviszony = new Munkaviszony(this, sconn);
            ujMunkaviszony.ShowDialog(this);

            txFoglId.Text = PnrId.ToString();
            txMegnevezes.Text = megnevezes;
            txFoglIrszam.Text = irszam;
            txFoglHelyseg.Text = helyseg;
            txFoglCim.Text = foglcim;
            txTelszam.Text = telszam;
            txAdoszam.Text = adoszam;

            dtMvnyKezd.Focus();
        }

        private void bFoglClear_Click(object sender, EventArgs e)
        {
            txAdoszam.Text = string.Empty;
            txFoglId.Text = string.Empty;
            txFoglIrszam.Text = string.Empty;
            txFoglHelyseg.Text = string.Empty;
            txFoglCim.Text = string.Empty;
            txMegnevezes.Text = string.Empty;
            txTelszam.Text = string.Empty;
        }

        private void bZaradekolas_Click(object sender, EventArgs e)
        {
            if (tsSave.Enabled == false && txPnrid.Text != string.Empty)
            {
                nev = txNev.Text;
                adoazon = txAdoazon.Text;
                pnrid = txPnrid.Text;
                alair = dtAlair.Value;
                erkezes = dtErkezes.Value;
                Zaradekolas zaradek = new Zaradekolas(this, sconn);
                zaradek.ShowDialog(this);
                if (zaradekolva)
                {
                    txPaj.Text = paj.ToString();
                    bZaradekolas.Enabled = false;
                }
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

        public bool IsSzamlaszam(string szlaszam)
        {
            try
            {
                if (szlaszam != null) return Regex.IsMatch(szlaszam, MatchSzamlaszamPattern);
                else return false;
            }
            catch
            {
                return false;
            }
        }

        private void txSzulHely_Leave(object sender, EventArgs e)
        {
            if (txSzulHely.Text.ToUpper() == "B") txSzulHely.Text = "Budapest";
        }

        private void txAllampolg_Leave(object sender, EventArgs e)
        {
            if (txAllampolg.Text.ToUpper() == "M") txAllampolg.Text = "Magyar";
        }

        private void txNev_TextChanged(object sender, EventArgs e)
        {
            tsSave.Enabled = true;
        }

        private void cbNeme_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
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
            e.Handled = true;
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

        private void TraceBejegyzes(string msg)
        {
            myListener.WriteLine("Tagsági okiratok rögzítése - " + DateTime.Now + ": " + msg);
            myListener.Flush();
            myListener.Close();
        }

        private void txSzulNev_TextChanged(object sender, EventArgs e)
        {
            tsSave.Enabled = true;
        }

        private void txNev_KeyPress(object sender, KeyPressEventArgs e)
        {
            tsSave.Enabled = true;
        }

        private void txTitulus_KeyPress(object sender, KeyPressEventArgs e)
        {
            tsSave.Enabled = true;
        }

        private void txSzulNev_KeyPress(object sender, KeyPressEventArgs e)
        {
            tsSave.Enabled = true;
        }

        private void cbEletciklStatusz_SelectedIndexChanged(object sender, EventArgs e)
        {
            tsSave.Enabled = true;
        }

        private void txSzulHely_TextChanged(object sender, EventArgs e)
        {
            tsSave.Enabled = true;
        }

        private void dtSzulIdo_ValueChanged(object sender, EventArgs e)
        {
            tsSave.Enabled = true;
        }

        private void txAnyjaNeve_TextChanged(object sender, EventArgs e)
        {
            tsSave.Enabled = true;
        }

        private void txAllampolg_TextChanged(object sender, EventArgs e)
        {
            tsSave.Enabled = true;
        }

        private void cbNeme_SelectedIndexChanged(object sender, EventArgs e)
        {
            tsSave.Enabled = true;
        }

        private void cbCsalAll_SelectedIndexChanged(object sender, EventArgs e)
        {
            tsSave.Enabled = true;
        }

        private void cbBelepTipusa_SelectedIndexChanged(object sender, EventArgs e)
        {
            tsSave.Enabled = true;
        }

        private void txAdoazon_TextChanged(object sender, EventArgs e)
        {
            tsSave.Enabled = true;
        }

        private void txTelefon_TextChanged(object sender, EventArgs e)
        {
            tsSave.Enabled = true;
        }

        private void bKedvezm_Click(object sender, EventArgs e)
        {
            if (txPnrid.Text == string.Empty)
            {
                MessageBox.Show("Kedvezményezett rögzítése előtt az okiratot menteni kell!");
            }
            else
            {
                int param = PnrId;
                Kedvezmenyezettek kedv = new Kedvezmenyezettek(sconn, param, this);
                kedv.ShowDialog();
            }
        }

        private void txSzamlaszam_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Char.IsDigit(e.KeyChar) && !Char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
            else
            {
                tsSave.Enabled = true;
            }
        }

        private void bAtadoPenztar_Click(object sender, EventArgs e)
        {
            NypKivalaszt nypk = new NypKivalaszt(sconn, this);
            nypk.ShowDialog();

            txPenztarid.Text = ptrid;
            txPenztarNeve.Text = ptrnev;
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
    }
}
