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

namespace VoluntaryPensionFundSystem
{
    public partial class Tag_keres : VoluntaryPensionFundSystem.BaseForm
    {
        private SqlConnection sconn;
        private SqlCommand scommand;
        private SqlDataAdapter da;
        private DataTable dt;
        public string adoazon, nev, szulnev, helyseg, cim, irszam;
        public int Paj, PnrId;

        public Tag_keres(SqlConnection SqlConn)
        {
            InitializeComponent();

            this.sconn = SqlConn;

            tsDelete.Enabled = false;
            tsFind.Enabled = true;
            tsNew.Enabled = false;
            tsSave.Enabled = false;
            tsSearch.Enabled = true;
            tsUpdate.Enabled = false;

            if (sconn.State == ConnectionState.Closed) sconn.Open();
        }

        public override void tsFind_Click(object sender, EventArgs e)
        {
            runQuery();
        }

        public override void runQuery()
        {
            // A LEKÉRDEZÉS FELÉPÍTÉSE
            string query1 = "SELECT count(*)";
            string query2 = "SELECT p.pnr_id,t.tszs_id,p.adoazonosito_jel,p.nev,p.leanykori_nev,p.ir_szam,p.helyseg,p.cim";
            string querystring = " FROM partnerek p, tagsagi_szerzodesek t WHERE p.pnr_id=t.pnr_id and p.pnr_tipus='SZMLY' and ";
            querystring = (txPnrId.Text != string.Empty ? querystring + "p.pnr_id like '" + txPnrId.Text + "' and " : querystring);
            querystring = (txPaj.Text != string.Empty ? querystring + "t.tszs_id like '" + txPaj.Text + "' and " : querystring);
            querystring = (txAdoazon.Text != string.Empty ? querystring + "p.adoazonosito_jel like '" + txAdoazon.Text + "' and " : querystring);
            querystring = txNev.Text != string.Empty ? querystring + "p.nev like '" + txNev.Text + "' and " : querystring;
            querystring = txSzulNev.Text != string.Empty ? querystring + "p.leanykori_nev like '" + txSzulNev.Text + "' and " : querystring;
            querystring = (txIrszam.Text != string.Empty ? querystring + "p.ir_szam like '" + txIrszam.Text + "' and " : querystring);
            querystring = (txHelyseg.Text != string.Empty ? querystring + "p.helyseg like '" + txHelyseg.Text + "' and " : querystring);
            querystring = (txCim.Text != string.Empty ? querystring + "p.cim like '" + txCim.Text + "' and " : querystring);
            querystring += "(p.ervenytelen is null or p.ervenytelen!='I')";

            // mennyiségi ellenőrzés
            query1 += querystring;
            scommand = new SqlCommand(query1, sconn);
            int Mcount = (int)scommand.ExecuteScalar();

            // A tagi adatok lekérdezése és betöltése
            querystring += " order by nev;";
            query2 += querystring;
            scommand = new SqlCommand(query2, sconn);

            if (Mcount > 0)
            {
                try { da.Dispose(); }
                catch { } // első keresés

                da = new SqlDataAdapter(scommand);
                dt = new DataTable();

                try
                {
                    da.Fill(dt);
                }
                catch
                {
                    //TraceBejegyzes(ex.Message);
                }

                TagEgyenirepKivalaszt fmk = new TagEgyenirepKivalaszt(dt, this);
                fmk.ShowDialog(this);

                txPnrId.Text = this.PnrId.ToString();
                txPaj.Text = this.Paj.ToString();
                txAdoazon.Text = this.adoazon;
                txNev.Text = this.nev;
                txSzulNev.Text = this.szulnev;
                txHelyseg.Text = this.helyseg;
                txCim.Text = this.cim;
                txIrszam.Text = this.irszam;
            }
            else
            {
                SqlDataReader myReader = null;
                try
                {
                    if (sconn.State == ConnectionState.Closed) sconn.Open();
                    myReader = scommand.ExecuteReader();
                    if (myReader.HasRows)
                    {
                        while (myReader.Read())
                        {
                            txPnrId.Text = myReader["pnr_id"].ToString();
                            txPaj.Text = myReader["tszs_id"].ToString();
                            txNev.Text = myReader["nev"].ToString();
                            txAdoazon.Text = myReader["adoazonosito_jel"].ToString();
                            txSzulNev.Text = myReader["leanykori_nev"].ToString();
                            txHelyseg.Text = myReader["helyseg"].ToString();
                            txCim.Text = myReader["cim"].ToString();
                            txIrszam.Text = myReader["ir_szam"].ToString();
                        }
                    }
                    else
                    {
                        MessageBox.Show("Nincs találat!");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Hibás adat!" + ex.Message);
                    //TraceBejegyzes(ex.Message);
                }
                myReader.Close();
            }
            txPnrId.ReadOnly = true;
            txPaj.ReadOnly = true;
            txAdoazon.ReadOnly = true;
            txNev.ReadOnly = true;
            txSzulNev.ReadOnly = true;
            txHelyseg.ReadOnly = true;
            txCim.ReadOnly = true;
            txIrszam.ReadOnly = true;

            save();     // automatikus mentés
        }

        private void bOk_Click(object sender, EventArgs e)
        {
            if (txPaj.Text != string.Empty)
            {
                Paj = int.Parse(txPaj.Text);
                PnrId = int.Parse(txPnrId.Text);
                adoazon = txAdoazon.Text;
                nev = txNev.Text;
                szulnev = txSzulNev.Text;
                irszam = txIrszam.Text;
                helyseg = txHelyseg.Text;
                cim = txCim.Text;

                Close();
            }
            else
                MessageBox.Show("Nincs kiválasztott adat!");
        }

        public override void tsSearch_Click(object sender, EventArgs e)
        {
            yellowMode();
        }

        public override void yellowMode()
        {
            txPnrId.ReadOnly = false;
            txPaj.ReadOnly = false;
            txAdoazon.ReadOnly = false;
            txNev.ReadOnly = false;
            txSzulNev.ReadOnly = false;
            txHelyseg.ReadOnly = false;
            txCim.ReadOnly = false;
            txIrszam.ReadOnly = false;

            txPnrId.Text = string.Empty;
            txPaj.Text = string.Empty;
            txAdoazon.Text = string.Empty;
            txNev.Text = string.Empty;
            txSzulNev.Text = string.Empty;
            txHelyseg.Text = string.Empty;
            txCim.Text = string.Empty;
            txIrszam.Text = string.Empty;

            tsFind.Enabled = true;
        }
    }
}
