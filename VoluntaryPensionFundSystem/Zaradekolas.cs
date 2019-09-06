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
    public partial class Zaradekolas : VoluntaryPensionFundSystem.BaseForm
    {
        private SqlConnection sconn;
        private SqlCommand scommand;

        Okiratok tag;
        private string tszsid;

        TextWriterTraceListener myListener = new TextWriterTraceListener(@"LOG\Zaradekolas.log", "myListener");

        public Zaradekolas(Okiratok ujOkirat, SqlConnection SqlConn)
        {
            InitializeComponent();

            tag = ujOkirat;

            this.sconn = SqlConn;
            if (sconn.State == ConnectionState.Closed) sconn.Open();

            txAdoazon.Text = tag.adoazon;
            txNev.Text = tag.nev;
            txPnrid.Text = tag.pnrid;
            dtAlair.Value = tag.alair;
            dtErkezes.Value = tag.erkezes;
            int pajszam = tag.paj;
            tszsid = pajszam.ToString();

            // Életciklus jogcímek
            scommand = new SqlCommand("SELECT * FROM jogcimek WHERE tipus='E' AND ervenyes='I'", sconn);
            if (sconn.State == ConnectionState.Closed) sconn.Open();
            SqlDataReader sqlReader = scommand.ExecuteReader();

            while (sqlReader.Read())
            {
                cbEletcikl.Items.Add(sqlReader["megnevezes"].ToString());
            }

            sqlReader.Close();

            tsDelete.Enabled = false;
            tsFind.Enabled = true;
            tsNew.Enabled = false;
            tsSave.Enabled = false;
            tsSearch.Enabled = false;
            tsUpdate.Enabled = false;
            bZaradek.Enabled = true;
            txNev.Focus();
        }

        public override void tsSave_Click(object sender, EventArgs e)
        {

        }

        public override void tsSearch_Click(object sender, EventArgs e)
        {
            
        }

        public override void tsFind_Click(object sender, EventArgs e)
        {
            
        }

        public override void tsExit_Click(object sender, EventArgs e)
        {
            exit();
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

        private void Zaradekolas_Load(object sender, EventArgs e)
        {

        }

        private void TraceBejegyzes(string msg)
        {
            myListener.WriteLine("- " + DateTime.Now + ": " + msg);
            myListener.Flush();
            myListener.Close();
        }

        private void bZaradek_Click(object sender, EventArgs e)
        {
            if (cbEletcikl.Text == string.Empty) { MessageBox.Show("Életciklus státusz nem lehet üres!"); cbEletcikl.Focus(); return; }
            DialogResult dr = MessageBox.Show("Biztosan záradékolható a tag a megadott adatokkal?\n" +
                "Aláírás dátuma: " + dtAlair.Value.ToShortDateString() + "\nBelépés dátuma: " + dtBelepes.Value.ToShortDateString() +
                "\nÉrkezés dátuma: " + dtErkezes.Value.ToShortDateString() + "\nFelvétel dátuma: " +
                dtFelvetel.Value.ToShortDateString(), "Megerősítés", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
            if (dr == DialogResult.OK)
            {
                // ellenőrzés
                if (dtAlair.Value > dtErkezes.Value) { MessageBox.Show("Az érkezés dátuma nem lehet kisebb, mint az aláírás dátuma!"); dtAlair.Focus(); return; }
                if (dtAlair.Value > dtFelvetel.Value) { MessageBox.Show("Az felvétel dátuma nem lehet kisebb, mint az aláírás dátuma!"); dtAlair.Focus(); return; }
                if (dtErkezes.Value > dtFelvetel.Value) { MessageBox.Show("Az felvétel dátuma nem lehet kisebb, mint az érkezés dátuma!"); dtErkezes.Focus(); return; }
                if (dtBelepes.Text.Substring(8, 2) != "01") { MessageBox.Show("A belépés dátuma csak a hónap első napja lehet!"); dtAlair.Focus(); return; }

                dtZaradek.Text = DateTime.Now.ToShortDateString();
                string query = "update tagsagi_szerzodesek set zaradek_datum='" + dtZaradek.Text.Substring(0, 10) + "' where tszs_id=" + tszsid;
                scommand = new SqlCommand(query, sconn);
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
                txPaj.Text = tszsid;
                tag.zaradekolva = true;
                MessageBox.Show("A záradékolás sikeres, dátum: " + dtZaradek.Text + " PAJ: " + tszsid);
            }
        }
    }
}
