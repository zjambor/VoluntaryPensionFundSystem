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
    public partial class Fogl_keres : VoluntaryPensionFundSystem.BaseForm
    {
        private SqlConnection sconn;
        private SqlCommand scommand;
        private SqlDataAdapter da;
        private DataTable dt;
        public string adoszam, adoazon, megnev, ev, helyseg, cim, irszam;
        public int FoglPnrid;

        public Fogl_keres(SqlConnection SqlConn)
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
            string query2 = "SELECT pnr_id,adoszam,adoazonosito_jel,megnevezes,nev,ir_szam,helyseg,cim";
            string querystring = " FROM partnerek WHERE (pnr_tipus='GTG' or (pnr_tipus='SZMLY' and egyeni_vall='I')) and ";
            querystring = (txPnrid.Text != string.Empty ? querystring + "pnr_id like '" + txPnrid.Text + "' and " : querystring);
            querystring = (txAdoszam.Text != string.Empty ? querystring + "adoszam like '" + txAdoszam.Text + "' and " : querystring);
            querystring = (txAdoazon.Text != string.Empty ? querystring + "adoazonosito_jel like '" + txAdoazon.Text + "' and " : querystring);
            querystring = txMegnev.Text != string.Empty ? querystring + "megnevezes like '" + txMegnev.Text + "' and " : querystring;
            querystring = txEV.Text != string.Empty ? querystring + "nev like '" + txEV.Text + "' and " : querystring;
            querystring = (txIrszam.Text != string.Empty ? querystring + "ir_szam like '" + txIrszam.Text + "' and " : querystring);
            querystring = (txHelyseg.Text != string.Empty ? querystring + "helyseg like '" + txHelyseg.Text + "' and " : querystring);
            querystring = (txCim.Text != string.Empty ? querystring + "cim like '" + txCim.Text + "' and " : querystring);
            querystring += "(ervenytelen is null or ervenytelen!='I')";

            // mennyiségi ellenőrzés
            query1 += querystring;
            scommand = new SqlCommand(query1, sconn);
            int Mcount = (int)scommand.ExecuteScalar();

            // A munkáltatói adatok lekérdezése és betöltése
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

                FoglMrepKivalaszt fmk = new FoglMrepKivalaszt(dt, this);
                fmk.ShowDialog(this);

                txPnrid.Text = this.FoglPnrid.ToString();
                txAdoszam.Text = this.adoszam;
                txAdoazon.Text = this.adoazon;
                txMegnev.Text = this.megnev;
                txEV.Text = this.ev;
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
                            txPnrid.Text = myReader["pnr_id"].ToString();
                            txMegnev.Text = myReader["megnevezes"].ToString();
                            txEV.Text = myReader["nev"].ToString();
                            txAdoszam.Text = myReader["adoszam"].ToString();
                            txEV.Text = myReader["nev"].ToString();
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
            txPnrid.ReadOnly = true;
            txAdoszam.ReadOnly = true;
            txAdoazon.ReadOnly = true;
            txMegnev.ReadOnly = true;
            txEV.ReadOnly = true;
            txHelyseg.ReadOnly = true;
            txCim.ReadOnly = true;
            txIrszam.ReadOnly = true;

            save();     // automatikus mentés
        }

        private void bOk_Click(object sender, EventArgs e)
        {
            if (txPnrid.Text != string.Empty)
            {
                FoglPnrid = int.Parse(txPnrid.Text);
                adoszam = txAdoszam.Text;
                megnev = txMegnev.Text;
                adoazon = txAdoazon.Text;
                irszam = txIrszam.Text;
                helyseg = txHelyseg.Text;
                cim = txCim.Text;
                ev = txEV.Text;

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
            txPnrid.ReadOnly = false;
            txAdoszam.ReadOnly = false;
            txAdoazon.ReadOnly = false;
            txMegnev.ReadOnly = false;
            txEV.ReadOnly = false;
            txHelyseg.ReadOnly = false;
            txCim.ReadOnly = false;
            txIrszam.ReadOnly = false;

            txPnrid.Text = string.Empty;
            txAdoszam.Text = string.Empty;
            txAdoazon.Text = string.Empty;
            txMegnev.Text = string.Empty;
            txEV.Text = string.Empty;
            txHelyseg.Text = string.Empty;
            txCim.Text = string.Empty;
            txIrszam.Text = string.Empty;

            tsFind.Enabled = true;
        }

        public override void tsExit_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
