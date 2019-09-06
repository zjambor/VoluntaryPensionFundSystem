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
    public partial class FoglUj : VoluntaryPensionFundSystem.BaseForm
    {
        private SqlConnection sconn;
        private SqlCommand scommand;

        public const string MatchAdoszamPattern = @"^[0-9]{11}$";
        public const string MatchKshszamPattern = @"^[0-9]{17}$";
        public const string MatchEmailPattern = @"^(([\w-]+\.)+[\w-]+|([a-zA-Z]{1}|[\w-]{2,}))@" + @"(([a-zA-Z]+[\w-]+\.)+[a-zA-Z]{2,4})$";
        public const string MatchTelPattern = @"^([0-9\+][0-9\-]+)|([0-9]+)\/" + @"([0-9\-]+)$";
        TextWriterTraceListener myListener = new TextWriterTraceListener(@"LOG\FoglUj.log", "myListener");
        List<string> tipusL = new List<string>();

        public string nev, adoszam, FoglTipusa, EgyeniVall, Adoazon, OrszKod, Irszam, Helyseg, Cim, ErtOrszKod, ErtIrszam, ErtHelyseg, ErtCim, KSH, Telszam;
        public string Email, Fax, Titulus, Megjegyzes, FoglId;
        bool irszamHiba = false;
        private bool insertTrueUpdateFalse = true;

        public FoglUj(SqlConnection SqlConn)
        {
            InitializeComponent();

            this.sconn = SqlConn;

            tsDelete.Enabled = false;
            tsFind.Enabled = false;
            tsNew.Enabled = false;
            tsSave.Enabled = false;
            tsSearch.Enabled = false;
            tsUpdate.Enabled = false;

            tipusL.Add("");
            tipusL.Add("GTG");
            tipusL.Add("SZMLY");
            cbFoglTipusa.DataSource = tipusL;
        }

        public override void tsNew_Click(object sender, EventArgs e)
        {
            createNew();
        }

        public override void tsSave_Click(object sender, EventArgs e)
        {
            save();
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

        public override void createNew()
        {
            insertTrueUpdateFalse = true;
            tsSave.Enabled = true;

            txAdoazon.Text = string.Empty;
            txAdoszam.Text = string.Empty;
            txCim.Text = string.Empty;
            txEgyeniVall.Text = string.Empty;
            txEmail.Text = string.Empty;
            txErtCim.Text = string.Empty;
            txErtHelyseg.Text = string.Empty;
            txErtIrszam.Text = string.Empty;
            txErtOrszKod.Text = string.Empty;
            txFax.Text = string.Empty;
            txHelyseg.Text = string.Empty;
            txIrszam.Text = string.Empty;
            txKSH.Text = string.Empty;
            txMegjegyzes.Text = string.Empty;
            txMegnevezes.Text = string.Empty;
            txOrszKod.Text = string.Empty;
            txPnrId.Value = 0;
            txTelszam.Text = string.Empty;
            txTitulus.Text = string.Empty;            
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

            if (insertTrueUpdateFalse)
            {
                // partner adatok tárolása
                int newPnrid = 0;

                // INSERT - új rekord létrohozása, az új rekord id-jét adja vissza
                newPnrid = insertNewPartner();
                if (newPnrid == 0) { MessageBox.Show("A rekord mentése nem sikerült!"); txMegnevezes.Focus(); return; }

                txPnrId.Text = newPnrid.ToString();

                // UPDATE

                if (cbFoglTipusa.Text == tipusL[1])
                {
                    scommand = new SqlCommand("update partnerek set megnevezes='" + txMegnevezes.Text + "' where pnr_id=" + newPnrid, sconn);
                    try { scommand.ExecuteNonQuery(); }
                    catch (Exception ex) { MessageBox.Show("SQL Hiba " + ex.Message); }
                }
                else
                {
                    scommand = new SqlCommand("update partnerek set nev='" + txMegnevezes.Text + "' where pnr_id=" + newPnrid, sconn);
                    try { scommand.ExecuteNonQuery(); }
                    catch (Exception ex) { MessageBox.Show("SQL Hiba " + ex.Message); }
                } 

                if (txAdoazon.Text != string.Empty)
                {
                    scommand = new SqlCommand("update partnerek set adoazonosito_jel='" + long.Parse(txAdoazon.Text) + "' where pnr_id=" + newPnrid, sconn);
                    try { scommand.ExecuteNonQuery(); }
                    catch (Exception ex) { MessageBox.Show("SQL Hiba " + ex.Message); }
                }
                if (txFax.Text != string.Empty)
                {
                    scommand = new SqlCommand("update partnerek set faxszam='" + txFax.Text + "' where pnr_id=" + newPnrid, sconn);
                    try { scommand.ExecuteNonQuery(); }
                    catch (Exception ex) { MessageBox.Show("SQL Hiba " + ex.Message); }
                }
                if (txEgyeniVall.Text != string.Empty)
                {
                    scommand = new SqlCommand("update partnerek set egyeni_vall='" + txEgyeniVall.Text + "' where pnr_id=" + newPnrid, sconn);
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
                if (txTelszam.Text != string.Empty)
                {
                    scommand = new SqlCommand("update partnerek set telefon='" + txTelszam.Text + "' where pnr_id=" + newPnrid, sconn);
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

                insertTrueUpdateFalse = false;
                tsSave.Enabled = false;
            }
            else
            {
                // Már létező rekord módosítása

                // UPDATE
                int Pnrid = (int)txPnrId.Value;

                UpdateCurrentPartner(Pnrid);

                if (cbFoglTipusa.Text == tipusL[1])
                {
                    scommand = new SqlCommand("update partnerek set megnevezes='" + int.Parse(txMegnevezes.Text) + "' where pnr_id=" + Pnrid, sconn);
                    try { scommand.ExecuteNonQuery(); }
                    catch (Exception ex) { MessageBox.Show("SQL Hiba " + ex.Message); }
                    scommand = new SqlCommand("update partnerek set nev=null where pnr_id=" + Pnrid, sconn);
                    try { scommand.ExecuteNonQuery(); }
                    catch (Exception ex) { MessageBox.Show("SQL Hiba " + ex.Message); }
                }
                else
                {
                    scommand = new SqlCommand("update partnerek set nev='" + int.Parse(txMegnevezes.Text) + "' where pnr_id=" + Pnrid, sconn);
                    try { scommand.ExecuteNonQuery(); }
                    catch (Exception ex) { MessageBox.Show("SQL Hiba " + ex.Message); }
                    scommand = new SqlCommand("update partnerek set megnevezes=null where pnr_id=" + Pnrid, sconn);
                    try { scommand.ExecuteNonQuery(); }
                    catch (Exception ex) { MessageBox.Show("SQL Hiba " + ex.Message); }
                }


                if (txAdoazon.Text != string.Empty) scommand = new SqlCommand("update partnerek set adoazonosito_jel='" + int.Parse(txAdoazon.Text) + "' where pnr_id=" + Pnrid, sconn);
                else scommand = new SqlCommand("update partnerek set adoazonosito_jel=null where pnr_id=" + Pnrid, sconn);
                try { scommand.ExecuteNonQuery(); }
                catch (Exception ex) { MessageBox.Show("SQL Hiba " + ex.Message); }

                if (txFax.Text != string.Empty)
                {
                    scommand = new SqlCommand("update partnerek set faxszam='" + txFax.Text + "' where pnr_id=" + Pnrid, sconn);
                }
                else scommand = new SqlCommand("update partnerek set faxszam=null where pnr_id=" + Pnrid, sconn);
                try { scommand.ExecuteNonQuery(); }
                catch (Exception ex) { MessageBox.Show("SQL Hiba " + ex.Message); }

                if (txEgyeniVall.Text != string.Empty)
                {
                    scommand = new SqlCommand("update partnerek set egyeni_vall='" + txEgyeniVall.Text + "' where pnr_id=" + Pnrid, sconn);
                }
                else scommand = new SqlCommand("update partnerek set egyeni_vall=null where pnr_id=" + Pnrid, sconn);
                try { scommand.ExecuteNonQuery(); }
                catch (Exception ex) { MessageBox.Show("SQL Hiba " + ex.Message); }

                if (txErtIrszam.Text != string.Empty)
                {
                    scommand = new SqlCommand("update partnerek set ert_irszam='" + int.Parse(txErtIrszam.Text) + "' where pnr_id=" + Pnrid, sconn);
                }
                else scommand = new SqlCommand("update partnerek set ert_irszam=null where pnr_id=" + Pnrid, sconn);
                try { scommand.ExecuteNonQuery(); }
                catch (Exception ex) { MessageBox.Show("SQL Hiba " + ex.Message); }

                if (txErtHelyseg.Text != string.Empty)
                {
                    scommand = new SqlCommand("update partnerek set ert_helyseg='" + txErtHelyseg.Text + "' where pnr_id=" + Pnrid, sconn);
                }
                else scommand = new SqlCommand("update partnerek set ert_helyseg=null where pnr_id=" + Pnrid, sconn);
                try { scommand.ExecuteNonQuery(); }
                catch (Exception ex) { MessageBox.Show("SQL Hiba " + ex.Message); }

                if (txErtCim.Text != string.Empty)
                {
                    scommand = new SqlCommand("update partnerek set ert_cim='" + txErtCim.Text + "' where pnr_id=" + Pnrid, sconn);
                }
                else scommand = new SqlCommand("update partnerek set ert_cim=null where pnr_id=" + Pnrid, sconn);
                try { scommand.ExecuteNonQuery(); }
                catch (Exception ex) { MessageBox.Show("SQL Hiba " + ex.Message); }

                if (txTelszam.Text != string.Empty)
                {
                    scommand = new SqlCommand("update partnerek set telefon='" + txTelszam.Text + "' where pnr_id=" + Pnrid, sconn);
                }
                else scommand = new SqlCommand("update partnerek set telefon=null where pnr_id=" + Pnrid, sconn);
                try { scommand.ExecuteNonQuery(); }
                catch (Exception ex) { MessageBox.Show("SQL Hiba " + ex.Message); }

                if (txEmail.Text != string.Empty)
                {
                    scommand = new SqlCommand("update partnerek set email='" + txEmail.Text + "' where pnr_id=" + Pnrid, sconn);
                }
                else scommand = new SqlCommand("update partnerek set email=null where pnr_id=" + Pnrid, sconn);
                try { scommand.ExecuteNonQuery(); }
                catch (Exception ex) { MessageBox.Show("SQL Hiba " + ex.Message); }

                if (txErtOrszKod.Text != string.Empty)
                {
                    scommand = new SqlCommand("update partnerek set ert_orszagkod='" + txErtOrszKod.Text + "' where pnr_id=" + Pnrid, sconn);
                }
                else scommand = new SqlCommand("update partnerek set ert_orszagkod=null where pnr_id=" + Pnrid, sconn);
                try { scommand.ExecuteNonQuery(); }
                catch (Exception ex) { MessageBox.Show("SQL Hiba " + ex.Message); }

                if (txTitulus.Text != string.Empty)
                {
                    scommand = new SqlCommand("update partnerek set titulus='" + txTitulus.Text + "' where pnr_id=" + Pnrid, sconn);
                }
                else scommand = new SqlCommand("update partnerek set titulus=null where pnr_id=" + Pnrid, sconn);
                try { scommand.ExecuteNonQuery(); }
                catch (Exception ex) { MessageBox.Show("SQL Hiba " + ex.Message); }

                if (txMegjegyzes.Text != string.Empty)
                {
                    scommand = new SqlCommand("update partnerek set megjegyzes='" + txMegjegyzes.Text + "' where pnr_id=" + Pnrid, sconn);
                }
                else scommand = new SqlCommand("update partnerek set megjegyzes=null where pnr_id=" + Pnrid, sconn);
                try { scommand.ExecuteNonQuery(); }
                catch (Exception ex) { MessageBox.Show("SQL Hiba " + ex.Message); }

                tsSave.Enabled = false;
            }
        }

        private int insertNewPartner()
        {
            Int32 newid = 0;
            scommand = new SqlCommand("spFoglalkoztatokInsert", sconn);
            scommand.CommandType = CommandType.StoredProcedure;

            scommand.Parameters.Add(new SqlParameter("@id", SqlDbType.Int)).Value = 1;
            scommand.Parameters.Add(new SqlParameter("@pnr_tipus", SqlDbType.VarChar, 5)).Value = cbFoglTipusa.Text;            
            scommand.Parameters.Add(new SqlParameter("@adoszam", SqlDbType.VarChar, 15)).Value = txAdoszam.Text;
            scommand.Parameters.Add(new SqlParameter("@ksh_torzsszam", SqlDbType.VarChar, 15)).Value = txKSH.Text;  
            scommand.Parameters.Add(new SqlParameter("@ir_szam", SqlDbType.Decimal, 4)).Value = int.Parse(txIrszam.Text);
            scommand.Parameters.Add(new SqlParameter("@helyseg", SqlDbType.VarChar, 20)).Value = txHelyseg.Text;
            scommand.Parameters.Add(new SqlParameter("@cim", SqlDbType.VarChar, 80)).Value = txCim.Text;
            scommand.Parameters.Add(new SqlParameter("@orszagkod", SqlDbType.VarChar, 5)).Value = txOrszKod.Text;
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

        private void UpdateCurrentPartner(int id)
        {
            scommand = new SqlCommand("spFoglalkoztatokUpdate", sconn);
            scommand.CommandType = CommandType.StoredProcedure;

            scommand.Parameters.Add(new SqlParameter("@id", SqlDbType.Int)).Value = id;
            scommand.Parameters.Add(new SqlParameter("@pnr_tipus", SqlDbType.VarChar, 5)).Value = cbFoglTipusa.Text;
            scommand.Parameters.Add(new SqlParameter("@adoszam", SqlDbType.VarChar, 15)).Value = txAdoszam.Text;
            scommand.Parameters.Add(new SqlParameter("@ksh_torzsszam", SqlDbType.VarChar, 15)).Value = txKSH.Text;
            scommand.Parameters.Add(new SqlParameter("@ir_szam", SqlDbType.Decimal, 4)).Value = int.Parse(txIrszam.Text);
            scommand.Parameters.Add(new SqlParameter("@helyseg", SqlDbType.VarChar, 20)).Value = txHelyseg.Text;
            scommand.Parameters.Add(new SqlParameter("@cim", SqlDbType.VarChar, 80)).Value = txCim.Text;
            scommand.Parameters.Add(new SqlParameter("@orszagkod", SqlDbType.VarChar, 5)).Value = txOrszKod.Text;
            scommand.Parameters.Add(new SqlParameter("@modosit_neve", SqlDbType.VarChar, 80)).Value = User.name;

            try
            {
                if (sconn.State == ConnectionState.Closed) sconn.Open();
                scommand.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show("SQL Hiba " + ex.Message);
            }
        }

        public bool IsEmail(string email)
        {
            TraceBejegyzes("Email Regexp");
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
            TraceBejegyzes("Adóazonosító jel Regexp");
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

        private void TraceBejegyzes(string msg)
        {
            myListener.WriteLine("- " + DateTime.Now + ": " + msg);
            myListener.Flush();
            myListener.Close();
        }

        private bool irszamEllenoriz(string irsz)
        {
            TraceBejegyzes("Ir. szám ellenőrzése");
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

        private void cbFoglTipusa_TextChanged(object sender, EventArgs e)
        {
            if (cbFoglTipusa.Text == tipusL[2]) txEgyeniVall.Text = "I";
            else txEgyeniVall.Text = string.Empty;
        }

        private void bEnter_Click(object sender, EventArgs e)
        {
            // adatátadás

            if (tsSave.Enabled) { MessageBox.Show("Az adatok nincsenek elmentve!"); cbFoglTipusa.Focus(); return; }

            adoszam = txAdoszam.Text;
            nev = txMegnevezes.Text;
            FoglTipusa = cbFoglTipusa.Text;
            EgyeniVall = txEgyeniVall.Text;
            Adoazon = txAdoazon.Text;
            OrszKod = txOrszKod.Text;
            Irszam = txIrszam.Text;
            Helyseg = txHelyseg.Text;
            Cim = txCim.Text;
            ErtOrszKod = txErtOrszKod.Text;
            ErtIrszam = txErtIrszam.Text;
            ErtHelyseg = txErtHelyseg.Text;
            ErtCim = txErtCim.Text;
            KSH = txKSH.Text;
            Telszam = txTelszam.Text;
            Email = txEmail.Text;
            Fax = txFax.Text;
            Titulus = txTitulus.Text;
            Megjegyzes = txMegjegyzes.Text;
            FoglId = txPnrId.Value.ToString();

            Close();
        }

        private void cbFoglTipusa_SelectedIndexChanged(object sender, EventArgs e)
        {
            //tsSave.Enabled = true;
        }

        private void txTitulus_TextChanged(object sender, EventArgs e)
        {
            tsSave.Enabled = true;
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

        private void txKSH_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Char.IsDigit(e.KeyChar) && !Char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
        }
    }
}
