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
    public partial class TamogSzerz : VoluntaryPensionFundSystem.BaseForm
    {
        private SqlConnection sconn;
        private SqlCommand scommand;

        bool InsertTrueUpdateFalse = false;
        public string adoszam, adoazon, megnev, ev, helyseg, cim, irszam;

        private List<string> fizgyak = new List<string>();
        
        private List<string> tamkor = new List<string>();

        FoglKivalasztas fk = null;

        // Napló logok:
        TextWriterTraceListener myListener = new TextWriterTraceListener(@"LOG\TamogSzerzodesek.log", "myListener");

        public TamogSzerz(SqlConnection SqlConn, object Szulo)
        {
            InitializeComponent();

            this.sconn = SqlConn;
            if (sconn.State == ConnectionState.Closed) sconn.Open();
            //évente, félévente, negyedévente, havonta
            fizgyak.Add("1");
            fizgyak.Add("3");
            fizgyak.Add("6");
            fizgyak.Add("12");
            txFizgyak.DataSource = fizgyak;

            // Támogathatósági körök
            scommand = new SqlCommand("SELECT tmkor_id,leiras FROM tamogathato_korok", sconn);
            if (sconn.State == ConnectionState.Closed) sconn.Open();
            SqlDataReader sqlReader = scommand.ExecuteReader();

            while (sqlReader.Read())
            {
                txTamkor.Items.Add(sqlReader["tmkor_id"].ToString() + " | " + sqlReader["leiras"].ToString());
            }

            sqlReader.Close();

            if (Szulo == null)
            {
                txTmszid.Text = string.Empty;
                txADosszeg.Text = string.Empty;
                txEgyszeri.Text = "N";
                dtHataly.Value = DateTime.Now;
                dtAlair.Value = DateTime.Now;
                txFizgyak.Text = string.Empty;
                dtErkezes.Value = DateTime.Now;
                dtKelt.Value = DateTime.Now;
                dtEsedKezd.Value = DateTime.Now;
                dtEsedVege.Value = DateTime.Now;
                //txTamkor.Text = txTamkor.Items[0].ToString();

                tsDelete.Enabled = false;
                tsFind.Enabled = false;
                tsNew.Enabled = false;
                tsSave.Enabled = false;
                tsSearch.Enabled = false;
                tsUpdate.Enabled = false;
                bKivalaszt.Enabled = true;
            }
            else
            {
                if (Szulo.GetType() == typeof(FoglKivalasztas))
                {
                    this.fk = (FoglKivalasztas)Szulo;

                    txPnrid.Text = fk.foglid;
                    txMegnev.Text = fk.megnev;
                    txAdoszam.Text = fk.adoszam;
                    txCim.Text = fk.cim;
                    txHelyseg.Text = fk.helyseg;
                    txIrszam.Text = fk.irszam;
                    txAdoazon.Text = fk.adoazon;

                    SqlDataReader myReader = null;
                    string query = "select tmsz_id,pnr_id,tmkor_id,adomany,egyszeri,gyakorisag,hatalybalepes,alairas_napja," +
                        "erkez_datum,kelt,esedekes_kezd,esedekes_vege from tamogatoi_szerzodesek where pnr_id=" + txPnrid.Text;

                    scommand = new SqlCommand(query, sconn);
                    string tmkid = string.Empty;
                    try
                    {
                        if (sconn.State == ConnectionState.Closed) sconn.Open();
                        myReader = scommand.ExecuteReader();
                        while (myReader.Read())
                        {
                            txTmszid.Text = myReader["tmsz_id"].ToString();
                            txADosszeg.Text = myReader["adomany"].ToString();
                            txEgyszeri.Text = myReader["egyszeri"].ToString();
                            dtHataly.Text = myReader["hatalybalepes"].ToString();
                            dtAlair.Text = myReader["alairas_napja"].ToString();
                            txFizgyak.Text = myReader["gyakorisag"].ToString();
                            dtErkezes.Text = myReader["erkez_datum"].ToString();
                            dtKelt.Text = myReader["kelt"].ToString();
                            dtEsedKezd.Text = myReader["esedekes_kezd"].ToString();
                            dtEsedVege.Text = myReader["esedekes_vege"].ToString();
                            tmkid = myReader["tmkor_id"].ToString();

                            txEgyszeri.Enabled = false;
                            txFizgyak.Enabled = false;
                            txADosszeg.ReadOnly = true;
                            txTmszid.ReadOnly = true;
                            dtHataly.Enabled = false;
                            dtAlair.Enabled = false;
                            dtErkezes.Enabled = false;
                            dtKelt.Enabled = false;
                            dtEsedKezd.Enabled = false;
                            dtEsedVege.Enabled = false;
                        }
                        myReader.Close();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.ToString());
                        TraceBejegyzes(ex.Message);
                    }

                    if (txTmszid.Text != string.Empty)
                    {
                        // van már szerződés
                        tsDelete.Enabled = true;
                        tsFind.Enabled = false;
                        tsNew.Enabled = false;
                        tsSave.Enabled = false;
                        tsSearch.Enabled = false;
                        tsUpdate.Enabled = true;

                        try
                        {
                            query = "select CAST(tmkor_id as varchar(8)) +' I '+leiras from tamogathato_korok where tmkor_id=" + tmkid;
                            scommand = new SqlCommand(query, sconn);

                            txTamkor.Text = scommand.ExecuteScalar().ToString();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.ToString());
                            TraceBejegyzes(ex.Message);
                        }
                    }
                    else
                    {
                        // nincs még szerződés
                        tsDelete.Enabled = false;
                        tsFind.Enabled = false;
                        tsNew.Enabled = true;
                        tsSave.Enabled = false;
                        tsSearch.Enabled = false;
                        tsUpdate.Enabled = false;
                    }
                    bKivalaszt.Enabled = false;
                }
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
            if (txPnrid.Text == string.Empty) return;

            if (txTmszid.Text != string.Empty)
            {
                // van már szerződés
                tsNew.Enabled = false;
                MessageBox.Show("Ennek a munkáltatónak már van támogatói szerződése!");
                return;
            }

            InsertTrueUpdateFalse = true;

            txEgyszeri.Enabled = true;
            txFizgyak.Enabled = true;
            txADosszeg.ReadOnly = false;
            txTamkor.Enabled = true;
            dtHataly.Enabled = true;
            dtAlair.Enabled = true;
            dtErkezes.Enabled = true;
            dtKelt.Enabled = true;
            dtEsedKezd.Enabled = true;
            dtEsedVege.Enabled = true;
            txEgyszeri.Text = "N";

            tsSave.Enabled = true;
            txTamkor.Focus();
        }

        private void updateRecord()
        {
            InsertTrueUpdateFalse = false;

            txEgyszeri.Enabled = true;
            txFizgyak.Enabled = true;
            txADosszeg.ReadOnly = false;
            txTamkor.Enabled = true;
            dtHataly.Enabled = true;
            dtAlair.Enabled = true;
            dtErkezes.Enabled = true;
            dtKelt.Enabled = true;
            dtEsedKezd.Enabled = true;
            dtEsedVege.Enabled = true;

            tsSave.Enabled = true;
            txTamkor.Focus();
        }

        public override void yellowMode()
        {
            // nem használható ezen a felületen
        }

        public override void runQuery()
        {
            // nem használható ezen a felületen
        }

        public override void save()
        {
            // ellenőrzés
            if (txADosszeg.Text != string.Empty)
            {
                if (txADosszeg.Text == "0")
                {
                    MessageBox.Show("Összeg nem lehet 0!");
                    txADosszeg.Focus();
                    return;
                }
            }
            else
            {
                MessageBox.Show("Összeg nem lehet üres!");
                txADosszeg.Focus();
                return;
            }

            if (txTamkor.Text == string.Empty)
            {
                MessageBox.Show("Támogathatósági kör megadása kötelező!");
                txTamkor.Focus();
                return;
            }
            
            int tmszid = 0;

            if (InsertTrueUpdateFalse)
            {
                scommand = new SqlCommand("spTamogSzerzInsert", sconn);
                scommand.CommandType = CommandType.StoredProcedure;
                scommand.Parameters.Add(new SqlParameter("@id", SqlDbType.Int)).Value = 1;
                scommand.Parameters.Add(new SqlParameter("@pnr_id", SqlDbType.Int)).Value = int.Parse(txPnrid.Text);
                string[] t = txTamkor.Text.Split('|');
                scommand.Parameters.Add(new SqlParameter("@tmkor_id", SqlDbType.Int)).Value = int.Parse(t[0]);
                scommand.Parameters.Add(new SqlParameter("@adomany", SqlDbType.Int)).Value = int.Parse(txADosszeg.Text);
                scommand.Parameters.Add(new SqlParameter("@egyszeri", SqlDbType.VarChar, 1)).Value = txEgyszeri.Text;
                scommand.Parameters.Add(new SqlParameter("@hatalybalepes", SqlDbType.Date)).Value = dtHataly.Value;
                scommand.Parameters.Add(new SqlParameter("@alairas_napja", SqlDbType.Date)).Value = dtAlair.Value;
                scommand.Parameters.Add(new SqlParameter("@gyakorisag", SqlDbType.Int)).Value = int.Parse(txFizgyak.Text);
                scommand.Parameters.Add(new SqlParameter("@erkez_datum", SqlDbType.Date)).Value = dtErkezes.Value;
                scommand.Parameters.Add(new SqlParameter("@kelt", SqlDbType.Date)).Value = dtKelt.Value;
                scommand.Parameters.Add(new SqlParameter("@esedekes_kezd", SqlDbType.Date)).Value = dtEsedKezd.Value;
                scommand.Parameters.Add(new SqlParameter("@esedekes_vege", SqlDbType.Date)).Value = dtEsedVege.Value;
                scommand.Parameters.Add(new SqlParameter("@modosit_neve", SqlDbType.VarChar, 80)).Value = User.name;

                try
                {
                    if (sconn.State == ConnectionState.Closed) sconn.Open();
                    tmszid = (int)scommand.ExecuteScalar();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("SQL Hiba " + ex.Message);
                    TraceBejegyzes(ex.Message);
                }

                txTmszid.Text = tmszid.ToString();
            }
            else
            {
                if (txTmszid.Text != string.Empty)
                {
                    scommand = new SqlCommand("spTamogSzerzUpdate", sconn);
                    scommand.CommandType = CommandType.StoredProcedure;
                    scommand.Parameters.Add(new SqlParameter("@id", SqlDbType.Int)).Value = int.Parse(txTmszid.Text);
                    scommand.Parameters.Add(new SqlParameter("@pnr_id", SqlDbType.Int)).Value = int.Parse(txPnrid.Text);
                    string[] t = txTamkor.Text.Split('|');
                    scommand.Parameters.Add(new SqlParameter("@tmkor_id", SqlDbType.Int)).Value = int.Parse(t[0]);
                    scommand.Parameters.Add(new SqlParameter("@adomany", SqlDbType.Int)).Value = int.Parse(txADosszeg.Text);
                    scommand.Parameters.Add(new SqlParameter("@egyszeri", SqlDbType.VarChar, 1)).Value = txEgyszeri.Text;
                    scommand.Parameters.Add(new SqlParameter("@hatalybalepes", SqlDbType.Date)).Value = dtHataly.Value;
                    scommand.Parameters.Add(new SqlParameter("@alairas_napja", SqlDbType.Date)).Value = dtAlair.Value;
                    scommand.Parameters.Add(new SqlParameter("@gyakorisag", SqlDbType.Int)).Value = int.Parse(txFizgyak.Text);
                    scommand.Parameters.Add(new SqlParameter("@erkez_datum", SqlDbType.Date)).Value = dtErkezes.Value;
                    scommand.Parameters.Add(new SqlParameter("@kelt", SqlDbType.Date)).Value = dtKelt.Value;
                    scommand.Parameters.Add(new SqlParameter("@esedekes_kezd", SqlDbType.Date)).Value = dtEsedKezd.Value;
                    scommand.Parameters.Add(new SqlParameter("@esedekes_vege", SqlDbType.Date)).Value = dtEsedVege.Value;
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
                }
            }

            txTamkor.Enabled = false;
            txEgyszeri.Enabled = false;
            txFizgyak.Enabled = false;
            txADosszeg.ReadOnly = true;
            txTmszid.ReadOnly = true;
            dtHataly.Enabled = false;
            dtAlair.Enabled = false;
            dtErkezes.Enabled = false;
            dtKelt.Enabled = false;
            dtEsedKezd.Enabled = false;
            dtEsedVege.Enabled = false;
            tsSave.Enabled = false;
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

        private void TraceBejegyzes(string msg)
        {
            myListener.WriteLine("-" + DateTime.Now + ": " + msg);
            myListener.Flush();
            myListener.Close();
        }

        private void bKivalaszt_Click(object sender, EventArgs e)
        {
            if (fk != null) return;

            txTmszid.Text = string.Empty;
            txADosszeg.Text = string.Empty;
            txEgyszeri.Text = "N";
            dtHataly.Value = DateTime.Now;
            dtAlair.Value = DateTime.Now;
            txFizgyak.Text = string.Empty;
            dtErkezes.Value = DateTime.Now;
            dtKelt.Value = DateTime.Now;
            dtEsedKezd.Value = DateTime.Now;
            dtEsedVege.Value = DateTime.Now;
            txTamkor.Text = string.Empty;

            Fogl_keres fker = new Fogl_keres(sconn);
            fker.ShowDialog();
            int foglpnr = fker.FoglPnrid;
            adoszam = fker.adoszam;
            adoazon = fker.adoazon;
            megnev = fker.megnev;
            ev = fker.ev;
            helyseg = fker.helyseg;
            cim = fker.cim;
            irszam = fker.irszam;

            SqlDataReader myReader = null;

            try
            {
                if (fker.FoglPnrid != 0)
                {
                    this.txPnrid.Text = foglpnr.ToString();

                    txAdoazon.Text = adoazon;
                    txAdoszam.Text = adoszam;
                    txMegnev.Text = megnev;
                    if (megnev == string.Empty) txMegnev.Text = ev;
                    txCim.Text = cim;
                    txHelyseg.Text = helyseg;
                    txIrszam.Text = irszam;

                    string query = "select tmsz_id,pnr_id,tmkor_id,adomany,egyszeri,gyakorisag,hatalybalepes,alairas_napja," +
                        "erkez_datum,kelt,esedekes_kezd,esedekes_vege from tamogatoi_szerzodesek where pnr_id=" + txPnrid.Text;
                    scommand = new SqlCommand(query, sconn);
                    string tmkid = string.Empty;
                    try
                    {
                        if (sconn.State == ConnectionState.Closed) sconn.Open();
                        myReader = scommand.ExecuteReader();
                        while (myReader.Read())
                        {
                            txTmszid.Text = myReader["tmsz_id"].ToString();
                            txADosszeg.Text = myReader["adomany"].ToString();
                            txEgyszeri.Text = myReader["egyszeri"].ToString();
                            dtHataly.Text = myReader["hatalybalepes"].ToString();
                            dtAlair.Text = myReader["alairas_napja"].ToString();
                            txFizgyak.Text = myReader["gyakorisag"].ToString();
                            dtErkezes.Text = myReader["erkez_datum"].ToString();
                            dtKelt.Text = myReader["kelt"].ToString();
                            dtEsedKezd.Text = myReader["esedekes_kezd"].ToString();
                            dtEsedVege.Text = myReader["esedekes_vege"].ToString();
                            tmkid = myReader["tmkor_id"].ToString();

                            txEgyszeri.Enabled = false;
                            txFizgyak.Enabled = false;
                            txADosszeg.ReadOnly = true;
                            txTmszid.ReadOnly = true;
                            dtHataly.Enabled = false;
                            dtAlair.Enabled = false;
                            dtErkezes.Enabled = false;
                            dtKelt.Enabled = false;
                            dtEsedKezd.Enabled = false;
                            dtEsedVege.Enabled = false;

                            tsNew.Enabled = true;
                        }
                        myReader.Close();
                    }
                    catch
                    {
                        MessageBox.Show("Hibás adat!");
                        //TraceBejegyzes(ex.Message);
                    }

                    if (txTmszid.Text == string.Empty)
                    {
                        tsNew.Enabled = true;
                    }
                    else
                    {
                        try
                        {
                            query = "select CAST(tmkor_id as varchar(8)) +' I '+leiras from tamogathato_korok where tmkor_id=" + tmkid;
                            scommand = new SqlCommand(query, sconn);

                            txTamkor.Text = scommand.ExecuteScalar().ToString();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.ToString());
                            TraceBejegyzes(ex.Message);
                        }
                    }
                }
            }
            catch
            {
                // nem történt módosítás
            }
        }

        public override void tsDelete_Click(object sender, EventArgs e)
        {
            if (txTmszid.Text == string.Empty) { MessageBox.Show("Nincs törölhető szerződés!"); return; }

            DialogResult dr = MessageBox.Show("Biztos törli a kijelölt rekordot? ",
                "Törlés?", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dr == DialogResult.Yes)
            {
                string query = "delete from tamogatoi_szerzodesek where tmsz_id=" + txTmszid.Text;
                scommand = new SqlCommand(query, sconn);

                try
                {
                    if (sconn.State == ConnectionState.Closed) sconn.Open();
                    scommand.ExecuteNonQuery();

                    txTmszid.Text = string.Empty;
                    txADosszeg.Text = string.Empty;
                    txEgyszeri.Text = "N";
                    dtHataly.Value = DateTime.Now;
                    dtAlair.Value = DateTime.Now;
                    txFizgyak.Text = string.Empty;
                    dtErkezes.Value = DateTime.Now;
                    dtKelt.Value = DateTime.Now;
                    dtEsedKezd.Value = DateTime.Now;
                    dtEsedVege.Value = DateTime.Now;
                    txTamkor.Text = string.Empty;
                    tsNew.Enabled = true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("SQL Hiba " + ex.Message);
                    TraceBejegyzes(ex.Message);
                }
            }
        }

        private void txADosszeg_KeyPress(object sender, KeyPressEventArgs e)
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

        private void txTamkor_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

        private void txEgyszeri_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

        private void txFizgyak_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

        private void TamogSzerz_Resize(object sender, EventArgs e)
        {
            groupBox2.Width = TamogathatoKorok.ActiveForm.Width - 404;
            txTamkor.Width = TamogathatoKorok.ActiveForm.Width - 587;
        }
    }
}
