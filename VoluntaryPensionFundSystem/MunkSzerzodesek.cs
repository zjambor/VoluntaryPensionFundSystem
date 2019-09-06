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
    public partial class MunkSzerzodesek : VoluntaryPensionFundSystem.BaseForm
    {
        private SqlConnection sconn;
        private SqlCommand scommand;

        bool InsertTrueUpdateFalse = false;
        public string adoszam, adoazon, megnev, ev, helyseg, cim, irszam;

        private List<string> fizgyak = new List<string>();
        private List<string> fizmod = new List<string>();

        FoglKivalasztas fk = null;

        // Napló logok:
        TextWriterTraceListener myListener = new TextWriterTraceListener(@"LOG\MunkSzerzodesek.log", "myListener");

        public MunkSzerzodesek(SqlConnection SqlConn, object Szulo)
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

            //előre utal, utólag utal
            fizmod.Add("előre utal");
            fizmod.Add("utólag utal");
            txFizmod.DataSource = fizmod;

            txMinmunk.Text = "0";

            if (Szulo == null)
            {
                txMkszid.Text = string.Empty;
                txHJosszeg.Text = string.Empty;
                txSzazalek.Text = string.Empty;
                txEgystagd.Text = string.Empty;
                txCafeteria.Text = string.Empty;
                txMinmunk.Text = string.Empty;
                dtHataly.Value = DateTime.Now;
                dtAlair.Value = DateTime.Now;
                txSzuneteltet.Text = string.Empty;
                txFizmod.Text = string.Empty;
                txFizgyak.Text = string.Empty;
                txHatarozatlan.Text = string.Empty;
                txIdotartam.Text = string.Empty;
                dtErkezes.Value = DateTime.Now;
                dtKelt.Value = DateTime.Now;

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
                    string query = "select mksz_id,pnr_id,hozzajarulas,szazalek,egys_tagd,cafeteria,min_munkaviszony,hatalybalepes,alairas_napja,szuneteltetes,fiz_modja,fiz_gyakorisag,hatarozatlan," +
                        "hat_idotartam,erkez_datum,kelt from munkaltatoi_szerzodesek where pnr_id=" + txPnrid.Text;

                    scommand = new SqlCommand(query, sconn);

                    //scommand = new SqlCommand("spFoglAdatai2", sconn);
                    //scommand.CommandType = CommandType.StoredProcedure;
                    //scommand.Parameters.Add(new SqlParameter("@adoszam", SqlDbType.VarChar, 15)).Value = adoszam.Substring(0, 8) + "%";
                    try
                    {
                        if (sconn.State == ConnectionState.Closed) sconn.Open();
                        myReader = scommand.ExecuteReader();
                        while (myReader.Read())
                        {
                            txMkszid.Text = myReader["mksz_id"].ToString();
                            txHJosszeg.Text = myReader["hozzajarulas"].ToString();
                            txSzazalek.Text = myReader["szazalek"].ToString();
                            txEgystagd.Text = myReader["egys_tagd"].ToString();
                            txCafeteria.Text = myReader["cafeteria"].ToString();
                            txMinmunk.Text = myReader["min_munkaviszony"].ToString();
                            dtHataly.Text = myReader["hatalybalepes"].ToString();
                            dtAlair.Text = myReader["alairas_napja"].ToString();
                            txSzuneteltet.Text = myReader["szuneteltetes"].ToString();
                            txFizmod.Text = myReader["fiz_modja"].ToString();
                            txFizgyak.Text = myReader["fiz_gyakorisag"].ToString();
                            txHatarozatlan.Text = myReader["hatarozatlan"].ToString();
                            txIdotartam.Text = myReader["hat_idotartam"].ToString();
                            dtErkezes.Text = myReader["erkez_datum"].ToString();
                            dtKelt.Text = myReader["kelt"].ToString();

                            txCafeteria.Enabled = false;
                            txEgystagd.Enabled = false;
                            txFizgyak.Enabled = false;
                            txFizmod.Enabled = false;
                            txHatarozatlan.Enabled = false;
                            txHJosszeg.ReadOnly = true;
                            txIdotartam.ReadOnly = true;
                            txMinmunk.ReadOnly = true;
                            txMkszid.ReadOnly = true;
                            txSzazalek.ReadOnly = true;
                            txSzuneteltet.Enabled = false;
                            dtHataly.Enabled = false;
                            dtAlair.Enabled = false;
                            dtErkezes.Enabled = false;
                            dtKelt.Enabled = false;
                        }
                        myReader.Close();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.ToString());
                        //TraceBejegyzes(ex.Message);
                    }

                    if (txMkszid.Text != string.Empty)
                    {
                        // van már szerződés
                        tsDelete.Enabled = true;
                        tsFind.Enabled = false;
                        tsNew.Enabled = false;
                        tsSave.Enabled = false;
                        tsSearch.Enabled = false;
                        tsUpdate.Enabled = true;
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
            
            if (txMkszid.Text != string.Empty)
            {
                // van már szerződés
                tsNew.Enabled = false;
                MessageBox.Show("Ennek a munkáltatónak már van munkáltatói szerződése!");
                return;
            }

            InsertTrueUpdateFalse = true;
            txCafeteria.Enabled = true;
            txEgystagd.Enabled = true;
            txFizgyak.Enabled = true;
            txFizmod.Enabled = true;
            txHatarozatlan.Enabled = true;
            txHJosszeg.ReadOnly = false;
            txIdotartam.ReadOnly = false;
            txMinmunk.ReadOnly = false;
            txSzazalek.ReadOnly = false;
            txSzuneteltet.Enabled = true;
            dtHataly.Enabled = true;
            dtAlair.Enabled = true;
            dtErkezes.Enabled = true;
            dtKelt.Enabled = true;

            txEgystagd.Text = "I";
            txCafeteria.Text = "I";
            txSzuneteltet.Text = "N";
            txHatarozatlan.Text = "I";

            tsSave.Enabled = true;
        }

        public override void save()
        {
            // ellenőrzés
            if (txHJosszeg.Text == string.Empty && txEgystagd.Text == "N" && txSzazalek.Text == string.Empty && txCafeteria.Text == "N")
            {
                MessageBox.Show("A munkáltatói hozzájárulás típusának vagy összegének valamelyikét kötelező megadni!");
                txHJosszeg.Focus();
                return;
            }

            if (txHJosszeg.Text != string.Empty)
            {
                if (txEgystagd.Text != "N" || txSzazalek.Text != string.Empty || txCafeteria.Text != "N")
                {
                    MessageBox.Show("Ha összeg kitöltött, akkor százalékos mérték, egységes tagdíj vagy cafeteria nem lehet!");
                    txHJosszeg.Focus();
                    return;
                }
                else
                {
                    if (txHJosszeg.Text == "0")
                    {
                        MessageBox.Show("Összeg nem lehet 0!");
                        txHJosszeg.Focus();
                        return;
                    }
                }
            }

            if (txSzazalek.Text != string.Empty)
            {
                if (txEgystagd.Text != "N" || txHJosszeg.Text != string.Empty || txCafeteria.Text != "N")
                {
                    MessageBox.Show("Ha százalékos mérték kitöltött, akkor összeg, egységes tagdíj vagy cafeteria nem lehet!");
                    txHJosszeg.Focus();
                    return;
                }
                else
                {
                    if (txSzazalek.Text == "0")
                    {
                        MessageBox.Show("Százalék mérték nem lehet 0!");
                        txSzazalek.Focus();
                        return;
                    }
                }
            }

            if (txEgystagd.Text != "N")
            {
                if (txSzazalek.Text != string.Empty || txHJosszeg.Text != string.Empty || txCafeteria.Text != "N")
                {
                    MessageBox.Show("Ha egységes tagdíj kitöltött, akkor összeg, százalékos mérték vagy cafeteria nem lehet!");
                    txHJosszeg.Focus();
                    return;
                }
            }

            if (txCafeteria.Text != "N")
            {
                if (txSzazalek.Text != string.Empty || txHJosszeg.Text != string.Empty || txEgystagd.Text != "N")
                {
                    MessageBox.Show("Ha cafeteria kitöltött, akkor összeg, százalékos mérték vagy egységes tagdíj nem lehet!");
                    txHJosszeg.Focus();
                    return;
                }
            }

            if (txHatarozatlan.Text == "N" && txIdotartam.Text == string.Empty)
            {
                MessageBox.Show("Ha a szerződés időtartama határozott, az időtartam kitöltése kötelező!");
                txIdotartam.Focus();
                return;
            }

            int mkszid = 0;

            if (InsertTrueUpdateFalse)
            {
                scommand = new SqlCommand("spMunkszerzInsert", sconn);
                scommand.CommandType = CommandType.StoredProcedure;
                scommand.Parameters.Add(new SqlParameter("@id", SqlDbType.Int)).Value = 1;
                scommand.Parameters.Add(new SqlParameter("@pnr_id", SqlDbType.Int)).Value = int.Parse(txPnrid.Text);
                if (txHJosszeg.Text == string.Empty)
                    scommand.Parameters.Add(new SqlParameter("@hozzajarulas", SqlDbType.Int)).Value = 0;
                else
                    scommand.Parameters.Add(new SqlParameter("@hozzajarulas", SqlDbType.Int)).Value = int.Parse(txHJosszeg.Text);
                scommand.Parameters.Add(new SqlParameter("@egys_tagd", SqlDbType.VarChar, 1)).Value = txEgystagd.Text;
                scommand.Parameters.Add(new SqlParameter("@cafeteria", SqlDbType.VarChar, 1)).Value = txCafeteria.Text;
                scommand.Parameters.Add(new SqlParameter("@hatalybalepes", SqlDbType.Date)).Value = dtHataly.Value;
                scommand.Parameters.Add(new SqlParameter("@alairas_napja", SqlDbType.Date)).Value = dtAlair.Value;
                scommand.Parameters.Add(new SqlParameter("@szuneteltetes", SqlDbType.VarChar, 1)).Value = txSzuneteltet.Text;
                scommand.Parameters.Add(new SqlParameter("@fiz_modja", SqlDbType.VarChar, 10)).Value = txFizmod.Text;
                scommand.Parameters.Add(new SqlParameter("@fiz_gyakorisag", SqlDbType.Int)).Value = int.Parse(txFizgyak.Text);
                scommand.Parameters.Add(new SqlParameter("@hatarozatlan", SqlDbType.VarChar, 1)).Value = txHatarozatlan.Text;
                scommand.Parameters.Add(new SqlParameter("@erkez_datum", SqlDbType.Date)).Value = dtErkezes.Value;
                scommand.Parameters.Add(new SqlParameter("@kelt", SqlDbType.Date)).Value = dtKelt.Value;
                scommand.Parameters.Add(new SqlParameter("@modosit_neve", SqlDbType.VarChar, 80)).Value = User.name;

                try
                {
                    if (sconn.State == ConnectionState.Closed) sconn.Open();
                    mkszid = (int)scommand.ExecuteScalar();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("SQL Hiba " + ex.Message);
                    TraceBejegyzes(ex.Message);
                }

                if (txMinmunk.Text != string.Empty)
                {
                    scommand = new SqlCommand("update munkaltatoi_szerzodesek set min_munkaviszony=" + txMinmunk.Text + " where mksz_id=" + mkszid.ToString(), sconn);
                    try { scommand.ExecuteNonQuery(); }
                    catch (Exception ex) { MessageBox.Show("SQL Hiba " + ex.Message); }
                }

                if (txSzazalek.Text != string.Empty)
                {
                    scommand = new SqlCommand("update munkaltatoi_szerzodesek set szazalek=" + txSzazalek.Text + " where mksz_id=" + mkszid.ToString(), sconn);
                    try { scommand.ExecuteNonQuery(); }
                    catch (Exception ex) { MessageBox.Show("SQL Hiba " + ex.Message); }
                }

                if (txIdotartam.Text != string.Empty)
                {
                    scommand = new SqlCommand("update munkaltatoi_szerzodesek set hat_idotartam=" + txIdotartam.Text + " where mksz_id=" + mkszid.ToString(), sconn);
                    try { scommand.ExecuteNonQuery(); }
                    catch (Exception ex) { MessageBox.Show("SQL Hiba " + ex.Message); }
                }
                txMkszid.Text = mkszid.ToString();
            }
            else
            {
                if (txMkszid.Text != string.Empty)
                {
                    scommand = new SqlCommand("spMunkszerzUpdate", sconn);
                    scommand.CommandType = CommandType.StoredProcedure;
                    scommand.Parameters.Add(new SqlParameter("@id", SqlDbType.Int)).Value = int.Parse(txMkszid.Text);
                    scommand.Parameters.Add(new SqlParameter("@pnr_id", SqlDbType.Int)).Value = int.Parse(txPnrid.Text);
                    if (txHJosszeg.Text == string.Empty)
                        scommand.Parameters.Add(new SqlParameter("@hozzajarulas", SqlDbType.Int)).Value = 0;
                    else
                        scommand.Parameters.Add(new SqlParameter("@hozzajarulas", SqlDbType.Int)).Value = int.Parse(txHJosszeg.Text);
                    //scommand.Parameters.Add(new SqlParameter("@szazalek", SqlDbType.Int)).Value = int.Parse(txSzazalek.Text);
                    scommand.Parameters.Add(new SqlParameter("@egys_tagd", SqlDbType.VarChar, 1)).Value = txEgystagd.Text;
                    scommand.Parameters.Add(new SqlParameter("@cafeteria", SqlDbType.VarChar, 1)).Value = txCafeteria.Text;
                    //scommand.Parameters.Add(new SqlParameter("@min_munkaviszony", SqlDbType.Int)).Value = int.Parse(txMinmunk.Text);
                    scommand.Parameters.Add(new SqlParameter("@hatalybalepes", SqlDbType.Date)).Value = dtHataly.Value;
                    scommand.Parameters.Add(new SqlParameter("@alairas_napja", SqlDbType.Date)).Value = dtAlair.Value;
                    scommand.Parameters.Add(new SqlParameter("@szuneteltetes", SqlDbType.VarChar, 1)).Value = txSzuneteltet.Text;
                    scommand.Parameters.Add(new SqlParameter("@fiz_modja", SqlDbType.VarChar, 10)).Value = txFizmod.Text;
                    scommand.Parameters.Add(new SqlParameter("@fiz_gyakorisag", SqlDbType.Int)).Value = int.Parse(txFizgyak.Text);
                    scommand.Parameters.Add(new SqlParameter("@hatarozatlan", SqlDbType.VarChar, 1)).Value = txHatarozatlan.Text;
                    //scommand.Parameters.Add(new SqlParameter("@hat_idotartam", SqlDbType.Int)).Value = int.Parse(txIdotartam.Text);
                    scommand.Parameters.Add(new SqlParameter("@erkez_datum", SqlDbType.Date)).Value = dtErkezes.Value;
                    scommand.Parameters.Add(new SqlParameter("@kelt", SqlDbType.Date)).Value = dtKelt.Value;
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

                    if (txMinmunk.Text != string.Empty)
                    {
                        scommand = new SqlCommand("update munkaltatoi_szerzodesek set min_munkaviszony=" + txMinmunk.Text + " where mksz_id=" + txMkszid.Text, sconn);
                        try { scommand.ExecuteNonQuery(); }
                        catch (Exception ex) { MessageBox.Show("SQL Hiba " + ex.Message); }
                    }

                    if (txSzazalek.Text != string.Empty)
                    {
                        scommand = new SqlCommand("update munkaltatoi_szerzodesek set szazalek=" + txSzazalek.Text + " where mksz_id=" + txMkszid.Text, sconn);
                        try { scommand.ExecuteNonQuery(); }
                        catch (Exception ex) { MessageBox.Show("SQL Hiba " + ex.Message); }
                    }

                    if (txIdotartam.Text != string.Empty)
                    {
                        scommand = new SqlCommand("update munkaltatoi_szerzodesek set hat_idotartam=" + txIdotartam.Text + " where mksz_id=" + txMkszid.Text, sconn);
                        try { scommand.ExecuteNonQuery(); }
                        catch (Exception ex) { MessageBox.Show("SQL Hiba " + ex.Message); }
                    }
                }
            }

            txCafeteria.Enabled = false;
            txEgystagd.Enabled = false;
            txFizgyak.Enabled = false;
            txFizmod.Enabled = false;
            txHatarozatlan.Enabled = false;
            txHJosszeg.ReadOnly = true;
            txIdotartam.ReadOnly = true;
            txMinmunk.ReadOnly = true;
            txMkszid.ReadOnly = true;
            txSzazalek.ReadOnly = true;
            txSzuneteltet.Enabled = false;
            dtHataly.Enabled = false;
            dtAlair.Enabled = false;
            dtErkezes.Enabled = false;
            dtKelt.Enabled = false;
            tsSave.Enabled = false;
        }

        private void updateRecord()
        {
            InsertTrueUpdateFalse = false;
            txCafeteria.Enabled = true;
            txEgystagd.Enabled = true;
            txFizgyak.Enabled = true;
            txFizmod.Enabled = true;
            txHatarozatlan.Enabled = true;
            txHJosszeg.ReadOnly = false;
            txIdotartam.ReadOnly = false;
            txMinmunk.ReadOnly = false;
            txSzazalek.ReadOnly = false;
            txSzuneteltet.Enabled = true;
            dtHataly.Enabled = true;
            dtAlair.Enabled = true;
            dtErkezes.Enabled = true;
            dtKelt.Enabled = true;
            tsSave.Enabled = true;
        }

        public override void yellowMode()
        {
            // nem használható ezen a felületen
        }

        public override void runQuery()
        {
            // nem használható ezen a felületen
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

            txMkszid.Text = string.Empty;
            txHJosszeg.Text = string.Empty;
            txSzazalek.Text = string.Empty;
            txEgystagd.Text = string.Empty;
            txCafeteria.Text = string.Empty;
            txMinmunk.Text = string.Empty;
            dtHataly.Value = DateTime.Now;
            dtAlair.Value = DateTime.Now;
            txSzuneteltet.Text = string.Empty;
            txFizmod.Text = string.Empty;
            txFizgyak.Text = string.Empty;
            txHatarozatlan.Text = string.Empty;
            txIdotartam.Text = string.Empty;
            dtErkezes.Value = DateTime.Now;
            dtKelt.Value = DateTime.Now;

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

                    string query = "select mksz_id,pnr_id,hozzajarulas,szazalek,egys_tagd,cafeteria,min_munkaviszony,hatalybalepes,alairas_napja,szuneteltetes,fiz_modja,fiz_gyakorisag,hatarozatlan," +
                        "hat_idotartam,erkez_datum,kelt from munkaltatoi_szerzodesek where pnr_id=" + txPnrid.Text;
                    scommand = new SqlCommand(query, sconn);

                    //scommand = new SqlCommand("spFoglAdatai2", sconn);
                    //scommand.CommandType = CommandType.StoredProcedure;
                    //scommand.Parameters.Add(new SqlParameter("@adoszam", SqlDbType.VarChar, 15)).Value = adoszam.Substring(0, 8) + "%";
                    try
                    {
                        if (sconn.State == ConnectionState.Closed) sconn.Open();
                        myReader = scommand.ExecuteReader();
                        while (myReader.Read())
                        {
                            txMkszid.Text = myReader["mksz_id"].ToString();
                            txHJosszeg.Text = myReader["hozzajarulas"].ToString();
                            txSzazalek.Text = myReader["szazalek"].ToString();
                            txEgystagd.Text = myReader["egys_tagd"].ToString();
                            txCafeteria.Text = myReader["cafeteria"].ToString();
                            txMinmunk.Text = myReader["min_munkaviszony"].ToString();
                            dtHataly.Text = myReader["hatalybalepes"].ToString();
                            dtAlair.Text = myReader["alairas_napja"].ToString();
                            txSzuneteltet.Text = myReader["szuneteltetes"].ToString();
                            txFizmod.Text = myReader["fiz_modja"].ToString();
                            txFizgyak.Text = myReader["fiz_gyakorisag"].ToString();
                            txHatarozatlan.Text = myReader["hatarozatlan"].ToString();
                            txIdotartam.Text = myReader["hat_idotartam"].ToString();
                            dtErkezes.Text = myReader["erkez_datum"].ToString();
                            dtKelt.Text = myReader["kelt"].ToString();

                            txCafeteria.Enabled = false;
                            txEgystagd.Enabled = false;
                            txFizgyak.Enabled = false;
                            txFizmod.Enabled = false;
                            txHatarozatlan.Enabled = false;
                            txHJosszeg.ReadOnly = true;
                            txIdotartam.ReadOnly = true;
                            txMinmunk.ReadOnly = true;
                            txMkszid.ReadOnly = true;
                            txSzazalek.ReadOnly = true;
                            txSzuneteltet.Enabled = false;
                            dtHataly.Enabled = false;
                            dtAlair.Enabled = false;
                            dtErkezes.Enabled = false;
                            dtKelt.Enabled = false;

                            tsNew.Enabled = true;
                        }
                        myReader.Close();
                    }
                    catch
                    {
                        MessageBox.Show("Hibás adat!");
                        //TraceBejegyzes(ex.Message);
                    }

                    if (txMkszid.Text == string.Empty)
                    {
                        tsNew.Enabled = true;
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
            if (txMkszid.Text == string.Empty) { MessageBox.Show("Nincs törölhető szerződés!"); return; }

            DialogResult dr = MessageBox.Show("Biztos törli a kijelölt rekordot? ",
                "Törlés?", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dr == DialogResult.Yes)
            {
                string query = "delete from munkaltatoi_szerzodesek where mksz_id=" + txMkszid.Text;
                scommand = new SqlCommand(query, sconn);

                try
                {
                    if (sconn.State == ConnectionState.Closed) sconn.Open();
                    scommand.ExecuteNonQuery();

                    txMkszid.Text = string.Empty;
                    txFizgyak.Text = fizgyak[0];
                    txFizmod.Text = fizmod[0];
                    txHJosszeg.Text = string.Empty;
                    txIdotartam.Text = string.Empty;
                    txMinmunk.Text = string.Empty;
                    txSzazalek.Text = string.Empty;
                    dtHataly.Value = DateTime.Now;
                    dtAlair.Value = DateTime.Now;
                    dtErkezes.Value = DateTime.Now;
                    dtKelt.Value = DateTime.Now;

                    txEgystagd.Text = "I";
                    txCafeteria.Text = "I";
                    txSzuneteltet.Text = "N";
                    txHatarozatlan.Text = "I";
                    tsNew.Enabled = true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("SQL Hiba " + ex.Message);
                    TraceBejegyzes(ex.Message);
                }
            }
        }

        private void txHJosszeg_KeyPress(object sender, KeyPressEventArgs e)
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

        private void txSzazalek_KeyPress(object sender, KeyPressEventArgs e)
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

        private void txMinmunk_KeyPress(object sender, KeyPressEventArgs e)
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

        private void txIdotartam_KeyPress(object sender, KeyPressEventArgs e)
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

        private void txHatarozatlan_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (txHatarozatlan.Text == "I") txIdotartam.ReadOnly = true;
            else txIdotartam.ReadOnly = false;
        }

        private void txEgystagd_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

        private void txCafeteria_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

        private void txSzuneteltet_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

        private void txFizmod_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

        private void txFizgyak_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

        private void txHatarozatlan_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }
    }
}
