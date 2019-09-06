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
    public partial class BevBong : VoluntaryPensionFundSystem.BaseForm
    {
        private SqlConnection sconn;
        private SqlCommand scommand;
        //private int TranID = 0;
        private SqlDataAdapter daBev;
        private DataTable dt, dtBev;
        private DataRow dr;
        private string querystring;
        private int bev_id = 0;
        private int Sorindex = 0;
        private int Tagokszama = 0, TotalSum = 0; //SajatSum = 0, MunkSum = 0, RendszSum = 0, EgyszeriSum = 0,

        // Napló logok:
        private TextWriterTraceListener myListener = new TextWriterTraceListener(@"LOG\BevallasBongeszes.log", "myListener");

        public string Bevid, ErkSorsz, Erkdate, Iktatoszam, IktKelte, Adatkozl, Megjegyzes, Storno, VonIdoszak, Ervenyes, Konyvelt;
        public string Sajat, Munkh, Tagok, Rendszeres, Egyszeri, Total;

        public BevBong(SqlConnection SqlConn)
        {
            InitializeComponent();

            this.sconn = SqlConn;
            if (sconn.State == ConnectionState.Closed) sconn.Open();

            tsDelete.Enabled = false;
            tsFind.Enabled = false;
            tsNew.Enabled = false;
            tsSave.Enabled = false;
            tsSearch.Enabled = true;
            tsUpdate.Enabled = false;
        }

        private void Frissit()
        {
            dt.Columns.Add("forg_id");          // 0
            dt.Columns.Add("adoazonosito");     // 1
            dt.Columns.Add("nev");              // 2
            dt.Columns.Add("ervenyes");         // 3
            dt.Columns.Add("sajat");            // 4
            dt.Columns.Add("hozzajarulas");     // 5
            dt.Columns.Add("rendszeres");       // 6
            dt.Columns.Add("egyszeri");         // 7
            dt.Columns.Add("befizetendo");      // 8
            dt.Columns.Add("storno");           // 9
            dt.Columns.Add("pnr_id");           // 10

            dt.Columns[1].AllowDBNull = false;

            this.dgwForgalmak.DataSource = dt;
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

        public override void yellowMode()
        {
            if (!tsSave.Enabled)
            {
                kiurit();
            }
            else
            {
                DialogResult dr = MessageBox.Show("Minden nem mentett változás el fog veszni! Folytatja?",
                "Folytatja?", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dr == DialogResult.Yes)
                {
                    kiurit();
                }
            }
        }

        public void kiurit()
        {
            txErkSorsz.ReadOnly = false;
            txErkDate.ReadOnly = false;
            txIktatoszam.ReadOnly = false;
            txIktKelte.ReadOnly = false;
            txAdkozlDate.ReadOnly = false;
            txMegjegyzes.ReadOnly = false;
            txVonIdoszak.ReadOnly = false;
            txErv.ReadOnly = false;
            txStorno.ReadOnly = false;

            txPnrid.ReadOnly = false;
            txMegnev.ReadOnly = false;
            txAdoszam.ReadOnly = false;
            txEV.ReadOnly = false;
            txHelyseg.ReadOnly = false;
            txCim.ReadOnly = false;
            txIrszam.ReadOnly = false;

            txPnrid.Text = string.Empty;
            txMegnev.Text = string.Empty;
            txAdoszam.Text = string.Empty;
            txEV.Text = string.Empty;
            txHelyseg.Text = string.Empty;
            txCim.Text = string.Empty;
            txIrszam.Text = string.Empty;

            txErkSorsz.Text = string.Empty;
            txErkDate.Text = string.Empty;
            txIktatoszam.Text = string.Empty;
            txIktKelte.Text = string.Empty;
            txAdkozlDate.Text = string.Empty;
            txMegjegyzes.Text = string.Empty;
            txVonIdoszak.Text = string.Empty;
            txErv.Text = string.Empty;

            txTagokSum.Enabled = true;
            txSajatSum.Enabled = true;
            txMunkSum.Enabled = true;
            txRendszSum.Enabled = true;
            TxEgyszeriSum.Enabled = true;
            TxTotal.Enabled = true;
            //newEvHo();

            //txErv.Text = "I";
            //txErv.BackColor = Color.LightGreen;
            txTagokSum.Value = 0;
            txSajatSum.Value = 0;
            txMunkSum.Value = 0;
            txRendszSum.Value = 0;
            TxEgyszeriSum.Value = 0;
            TxTotal.Value = 0;
            //txIktKelte.Text = Mainap;
            txStorno.Text = string.Empty;

            txTagokSumC.Text = "0";
            txSajatSumC.Text = "0";
            txMunkSumC.Text = "0";
            txRendszSumC.Text = "0";
            TxEgyszeriSumC.Text = "0";
            TxTotalC.Text = "0";

            dt = new DataTable();
            Frissit();
            dgwForgalmak.DataSource = null;
            dgwForgalmak.Refresh();

            sorokKiurit();

            //txErkSorsz.BackColor = Color.Yellow;
            //txErkDate.BackColor = Color.Yellow;
            //txIktatoszam.BackColor = Color.Yellow;
            //txIktKelte.BackColor = Color.Yellow;
            //txAdkozlDate.BackColor = Color.Yellow;
            //txMegjegyzes.BackColor = Color.Yellow;
            //txErv.BackColor = Color.Yellow;
            //txStorno.BackColor = Color.Yellow;
            //txVonIdoszak.BackColor = Color.Yellow;

            //txTagokSum.BackColor = Color.Yellow;
            //txSajatSum.BackColor = Color.Yellow;
            //txMunkSum.BackColor = Color.Yellow;
            //txRendszSum.BackColor = Color.Yellow;
            //TxEgyszeriSum.BackColor = Color.Yellow;
            //TxTotal.BackColor = Color.Yellow;

            tsDelete.Enabled = false;
            tsFind.Enabled = true;
            tsNew.Enabled = false;
            tsSave.Enabled = false;
            tsSearch.Enabled = false;
            tsUpdate.Enabled = false;
        }

        public override void runQuery()
        {
            //dt = new DataTable();
            this.dgwForgalmak.DataSource = null;

            // A LEKÉRDEZÉS FELÉPÍTÉSE
            string query1 = "SELECT count(*)";
            string query2 = "SELECT p.pnr_id,p.megnevezes,p.nev,p.adoszam,p.helyseg,p.cim,p.ir_szam,b.bev_id, i.ev_ho, b.erkez_sorszam, b.erkez_datum, b.iktatoszam, b.iktatas_kelte, b.adatkozles_datum" +
                                    ", b.tagok_osszesen, b.sajat_ossz, b.hozzajarulas_ossz, b.rend_tamog_ossz, b.egysz_tamog_ossz" +
                                    ", b.mindosszesen, b.ervenyes, b.storno, b.megjegyzes, b.konyvelt";
            querystring = " FROM	partnerek p, bevallasok b, idoszakok i " +
                           "WHERE	p.pnr_id=b.pnr_id and b.idk_id=i.idk_id and ";

            querystring = (txPnrid.Text != string.Empty ? querystring + "p.pnr_id like '" + txPnrid.Text + "' and " : querystring);
            querystring = (txMegnev.Text != string.Empty ? querystring + "p.megnevezes like '" + txMegnev.Text + "' and " : querystring);
            querystring = (txAdoszam.Text != string.Empty ? querystring + "p.adoszam like '" + txAdoszam.Text + "' and " : querystring);
            querystring = (txEV.Text != string.Empty ? querystring + "p.nev like '" + txEV.Text + "' and " : querystring);
            querystring = (txHelyseg.Text != string.Empty ? querystring + "p.helyseg like '" + txHelyseg.Text + "' and " : querystring);
            querystring = (txCim.Text != string.Empty ? querystring + "p.cim like '" + txCim.Text + "' and " : querystring);
            querystring = (txIrszam.Text != string.Empty ? querystring + "p.ir_szam like '" + txIrszam.Text + "' and " : querystring);

            querystring = (txVonIdoszak.Text != string.Empty ? querystring + "i.ev_ho like '" + txVonIdoszak.Text + "' and " : querystring);
            querystring = (txErkSorsz.Text != string.Empty ? querystring + "b.erkez_sorszam like '" + txErkSorsz.Text + "' and " : querystring);
            querystring = (txErkDate.Text != string.Empty ? querystring + "b.erkez_datum like '" + txErkDate.Text + "' and " : querystring);
            querystring = (txIktatoszam.Text != string.Empty ? querystring + "b.iktatoszam like '" + txIktatoszam.Text + "' and " : querystring);
            querystring = (txIktKelte.Text != string.Empty ? querystring + "b.iktatas_kelte like '" + txIktKelte.Text + "' and " : querystring);
            querystring = (txAdkozlDate.Text != string.Empty ? querystring + "b.adatkozles_datum like '" + txAdkozlDate.Text + "' and " : querystring);
            querystring = (txMegjegyzes.Text != string.Empty ? querystring + "b.megjegyzes like '" + txMegjegyzes.Text + "' and " : querystring);
            querystring = (txErv.Text != string.Empty ? querystring + "b.ervenyes like '" + txErv.Text + "' and " : querystring);
            querystring = (txStorno.Text != string.Empty ? querystring + "b.storno = '" + txStorno.Text + "' and " : querystring);

            querystring = (txTagokSum.Value != 0 ? querystring + "b.tagok_osszesen = " + txTagokSum.Value + " and " : querystring);
            querystring = (txSajatSum.Value != 0 ? querystring + "b.sajat_ossz = " + txSajatSum.Value + " and " : querystring);
            querystring = (txMunkSum.Value != 0 ? querystring + "b.hozzajarulas_ossz = " + txMunkSum.Value + " and " : querystring);
            querystring = (txRendszSum.Value != 0 ? querystring + "b.rend_tamog_ossz = " + txRendszSum.Value + " and " : querystring);
            querystring = (TxEgyszeriSum.Value != 0 ? querystring + "b.egysz_tamog_ossz = " + TxEgyszeriSum.Value + " and " : querystring);
            querystring = (TxTotal.Value != 0 ? querystring + "b.mindosszesen = " + TxTotal.Value + " and " : querystring);

            querystring += "(pnr_tipus='GTG' or (pnr_tipus='SZMLY' and egyeni_vall='I'))";
            //MessageBox.Show(querystring);

            // mennyiségi ellenőrzés
            query1 += querystring;
            scommand = new SqlCommand(query1, sconn);
            int Mcount = (int)scommand.ExecuteScalar();

            // lekérdezés futtatása
            querystring += " order by p.pnr_id,i.idk_id,b.bev_id;";
            query2 += querystring;
            scommand = new SqlCommand(query2, sconn);
            if (Mcount > 1)
            {
                try { daBev.Dispose(); }
                catch { } // első keresés

                daBev = new SqlDataAdapter(scommand);
                dtBev = new DataTable();

                try
                {
                    daBev.Fill(dtBev);
                }
                catch
                {
                    //TraceBejegyzes(ex.Message);
                }

                TobbBevallasKivalaszt bfk = new TobbBevallasKivalaszt(dtBev, this);
                bfk.ShowDialog(this);

                try
                {
                    txPnrid.Text = bfk.pnrid;
                    txAdoszam.Text = bfk.adoszam;
                    txMegnev.Text = bfk.megnevezes;
                    txEV.Text = bfk.nev;
                    txHelyseg.Text = bfk.helyseg;
                    txCim.Text = bfk.cim;
                    txIrszam.Text = bfk.irszam;
                    string bevidstr = bfk.bevid;
                    bev_id = int.Parse(bevidstr);

                    Erkdate = bfk.erkdate;
                    IktKelte = bfk.iktkelte;
                    Adatkozl = bfk.adatkozl;

                    txErkSorsz.Text = bfk.erksorsz;
                    txErkDate.Text = DateTime.Parse(Erkdate).ToShortDateString();
                    txIktatoszam.Text = bfk.iktato;
                    txIktKelte.Text = DateTime.Parse(IktKelte).ToShortDateString();
                    txAdkozlDate.Text = DateTime.Parse(Adatkozl).ToShortDateString();
                    txMegjegyzes.Text = bfk.megjegyz;
                    txErv.Text = bfk.ervenyes;

                    string tagok, sajat, munk, rendsz, egysz, total;
                    tagok = bfk.tagok;
                    sajat = bfk.sajat;
                    munk = bfk.munkh;
                    rendsz = bfk.rendszeres;
                    egysz = bfk.egyszeri;
                    total = bfk.total;

                    txTagokSum.Value = int.Parse(tagok);
                    txSajatSum.Value = int.Parse(sajat);
                    txMunkSum.Value = int.Parse(munk);
                    txRendszSum.Value = int.Parse(rendsz);
                    TxEgyszeriSum.Value = int.Parse(egysz);
                    TxTotal.Value = int.Parse(total);
                    txStorno.Text = bfk.storno;
                    txVonIdoszak.Text = bfk.vonidoszak;
                    txKonyvelt.Text = bfk.konyvelt;
                }
                catch
                {

                }
            }
            else if (Mcount == 1)
            {
                SqlDataReader myReader = null;
                try
                {
                    if (sconn.State == ConnectionState.Closed) sconn.Open();
                    myReader = scommand.ExecuteReader();
                    while (myReader.Read())
                    {
                        txPnrid.Text = myReader["pnr_id"].ToString();
                        txAdoszam.Text = myReader["adoszam"].ToString();
                        txMegnev.Text = myReader["megnevezes"].ToString();
                        txEV.Text = myReader["nev"].ToString();
                        txHelyseg.Text = myReader["helyseg"].ToString();
                        txCim.Text = myReader["cim"].ToString();
                        txIrszam.Text = myReader["ir_szam"].ToString();
                        
                        bev_id = int.Parse(myReader["bev_id"].ToString());
                        txErkSorsz.Text = myReader["erkez_sorszam"].ToString();
                        txErkDate.Text = DateTime.Parse(myReader["erkez_datum"].ToString()).ToShortDateString();
                        txIktatoszam.Text = myReader["iktatoszam"].ToString();
                        txIktKelte.Text = DateTime.Parse(myReader["iktatas_kelte"].ToString()).ToShortDateString();
                        txAdkozlDate.Text = DateTime.Parse(myReader["adatkozles_datum"].ToString()).ToShortDateString();
                        txMegjegyzes.Text = myReader["megjegyzes"].ToString();
                        txErv.Text = myReader["ervenyes"].ToString();

                        txTagokSum.Value = int.Parse(myReader["tagok_osszesen"].ToString());
                        txSajatSum.Value = int.Parse(myReader["sajat_ossz"].ToString());
                        txMunkSum.Value = int.Parse(myReader["hozzajarulas_ossz"].ToString());
                        txRendszSum.Value = int.Parse(myReader["rend_tamog_ossz"].ToString());
                        TxEgyszeriSum.Value = int.Parse(myReader["egysz_tamog_ossz"].ToString());
                        TxTotal.Value = int.Parse(myReader["mindosszesen"].ToString());
                        txStorno.Text = myReader["storno"].ToString();
                        txVonIdoszak.Text = myReader["ev_ho"].ToString();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("SQL Hiba " + ex.Message);
                    TraceBejegyzes(ex.Message);
                }
                myReader.Close();
            }
            else if (Mcount == 0)
            {
                return;
            }

            SqlDataReader myReader2 = null;
            scommand = new SqlCommand("spForgalmak", sconn);
            scommand.CommandType = CommandType.StoredProcedure;
            scommand.Parameters.Add(new SqlParameter("@bev_id", SqlDbType.Int)).Value = bev_id;

            try
            {
                if (sconn.State == ConnectionState.Closed) sconn.Open();
                myReader2 = scommand.ExecuteReader();
                while (myReader2.Read())
                {
                    dr = this.dt.NewRow();
                    dr["forg_id"] = myReader2["forg_id"].ToString();
                    dr["adoazonosito"] = myReader2["adoazonosito_jel"].ToString();
                    dr["nev"] = myReader2["nev"].ToString();
                    dr["ervenyes"] = myReader2["ervenyes"].ToString();
                    dr["storno"] = myReader2["storno"].ToString();
                    dr["pnr_id"] = myReader2["pnr_id"].ToString();
                    dr["sajat"] = myReader2["sajat"].ToString();
                    dr["hozzajarulas"] = myReader2["hozzajarulas"].ToString();
                    dr["rendszeres"] = myReader2["rend_tamog"].ToString();
                    dr["egyszeri"] = myReader2["egysz_tamog"].ToString();
                    dr["befizetendo"] = myReader2["befizetendo"].ToString();
                    dt.Rows.Add(dr);
                }
                dgwForgalmak.DataSource = dt;
                dgwForgalmak.Refresh();
            }
            catch (Exception ex)
            {
                MessageBox.Show("SQL Hiba " + ex.Message);
                TraceBejegyzes(ex.Message);
            }
            myReader2.Close();
            txSorsz0.Text = "1";
            fSorokFeltoltese();
            SumTagok();
            SumCalc();

            // mezők beállítása
            txPnrid.ReadOnly = true;
            txAdoszam.ReadOnly = true;
            txMegnev.ReadOnly = true;
            txEV.ReadOnly = true;
            txHelyseg.ReadOnly = true;
            txCim.ReadOnly = true;
            txIrszam.ReadOnly = true;

            txErv.ReadOnly = true;
            txErkSorsz.ReadOnly = true;
            txErkDate.ReadOnly = true;
            txIktatoszam.ReadOnly = true;
            txIktKelte.ReadOnly = true;
            txAdkozlDate.ReadOnly = true;
            txMegjegyzes.ReadOnly = true;
            txStorno.ReadOnly = true;
            txVonIdoszak.ReadOnly = true;

            txTagokSum.ReadOnly = true;
            txSajatSum.ReadOnly = true;
            txMunkSum.ReadOnly = true;
            txRendszSum.ReadOnly = true;
            TxEgyszeriSum.ReadOnly = true;
            TxTotal.ReadOnly = true;

            tsDelete.Enabled = true;
            tsFind.Enabled = false;
            tsNew.Enabled = true;
            tsSave.Enabled = false;
            tsSearch.Enabled = true;
            tsUpdate.Enabled = false;
        }

        public void fSorokFeltoltese()
        {
            int max;
            if (int.Parse(txSorsz0.Text) == 1 && dgwForgalmak.RowCount > 5)
            {
                max = 5;
                Sorindex = 0;
            }
            else
            {
                Sorindex = int.Parse(txSorsz0.Text) - 1;
                max = dgwForgalmak.RowCount - Sorindex;
            }
            switch (max)
            {
                case 5:
                    SorFeltolt0();
                    SorFeltolt1();
                    SorFeltolt2();
                    SorFeltolt3();
                    SorFeltolt4();
                    break;
                case 4:
                    SorFeltolt0();
                    SorFeltolt1();
                    SorFeltolt2();
                    SorFeltolt3();
                    break;
                case 3:
                    SorFeltolt0();
                    SorFeltolt1();
                    SorFeltolt2();
                    break;
                case 2:
                    SorFeltolt0();
                    SorFeltolt1();
                    break;
                case 1:
                    SorFeltolt0();
                    break;
                default: break;
            }
        }

        private void SorFeltolt0()
        {
            txForgid0.Text = dgwForgalmak.Rows[Sorindex].Cells[0].Value.ToString();
            txSorsz0.Text = (Sorindex + 1).ToString();
            txAD0.Text = dgwForgalmak.Rows[Sorindex].Cells[1].Value.ToString();
            txNev0.Text = dgwForgalmak.Rows[Sorindex].Cells[2].Value.ToString();
            txErv0.Text = dgwForgalmak.Rows[Sorindex].Cells[3].Value.ToString();
            txSaj0.Text = dgwForgalmak.Rows[Sorindex].Cells[4].Value.ToString();
            txMh0.Text = dgwForgalmak.Rows[Sorindex].Cells[5].Value.ToString();
            txRend0.Text = dgwForgalmak.Rows[Sorindex].Cells[6].Value.ToString();
            txEgysz0.Text = dgwForgalmak.Rows[Sorindex].Cells[7].Value.ToString();
            txBef0.Text = dgwForgalmak.Rows[Sorindex].Cells[8].Value.ToString();
            txStorno0.Text = dgwForgalmak.Rows[Sorindex].Cells[9].Value.ToString();
            txPnrid0.Text = dgwForgalmak.Rows[Sorindex].Cells[10].Value.ToString();
        }

        private void SorFeltolt1()
        {
            txForgid1.Text = dgwForgalmak.Rows[Sorindex + 1].Cells[0].Value.ToString();
            txSorsz1.Text = (Sorindex + 2).ToString();
            txAD1.Text = dgwForgalmak.Rows[Sorindex + 1].Cells[1].Value.ToString();
            txNev1.Text = dgwForgalmak.Rows[Sorindex + 1].Cells[2].Value.ToString();
            txErv1.Text = dgwForgalmak.Rows[Sorindex + 1].Cells[3].Value.ToString();
            txSaj1.Text = dgwForgalmak.Rows[Sorindex + 1].Cells[4].Value.ToString();
            txMh1.Text = dgwForgalmak.Rows[Sorindex + 1].Cells[5].Value.ToString();
            txRend1.Text = dgwForgalmak.Rows[Sorindex + 1].Cells[6].Value.ToString();
            txEgysz1.Text = dgwForgalmak.Rows[Sorindex + 1].Cells[7].Value.ToString();
            txBef1.Text = dgwForgalmak.Rows[Sorindex + 1].Cells[8].Value.ToString();
            txStorno1.Text = dgwForgalmak.Rows[Sorindex + 1].Cells[9].Value.ToString();
            txPnrid1.Text = dgwForgalmak.Rows[Sorindex + 1].Cells[10].Value.ToString();
        }

        private void SorFeltolt2()
        {
            txForgid2.Text = dgwForgalmak.Rows[Sorindex + 2].Cells[0].Value.ToString();
            txSorsz2.Text = (Sorindex + 3).ToString();
            txAD2.Text = dgwForgalmak.Rows[Sorindex + 2].Cells[1].Value.ToString();
            txNev2.Text = dgwForgalmak.Rows[Sorindex + 2].Cells[2].Value.ToString();
            txErv2.Text = dgwForgalmak.Rows[Sorindex + 2].Cells[3].Value.ToString();
            txSaj2.Text = dgwForgalmak.Rows[Sorindex + 2].Cells[4].Value.ToString();
            txMh2.Text = dgwForgalmak.Rows[Sorindex + 2].Cells[5].Value.ToString();
            txRend2.Text = dgwForgalmak.Rows[Sorindex + 2].Cells[6].Value.ToString();
            txEgysz2.Text = dgwForgalmak.Rows[Sorindex + 2].Cells[7].Value.ToString();
            txBef2.Text = dgwForgalmak.Rows[Sorindex + 2].Cells[8].Value.ToString();
            txStorno2.Text = dgwForgalmak.Rows[Sorindex + 2].Cells[9].Value.ToString();
            txPnrid2.Text = dgwForgalmak.Rows[Sorindex + 2].Cells[10].Value.ToString();
        }

        private void SorFeltolt3()
        {
            txForgid3.Text = dgwForgalmak.Rows[Sorindex + 3].Cells[0].Value.ToString();
            txSorsz3.Text = (Sorindex + 4).ToString();
            txAD3.Text = dgwForgalmak.Rows[Sorindex + 3].Cells[1].Value.ToString();
            txNev3.Text = dgwForgalmak.Rows[Sorindex + 3].Cells[2].Value.ToString();
            txErv3.Text = dgwForgalmak.Rows[Sorindex + 3].Cells[3].Value.ToString();
            txSaj3.Text = dgwForgalmak.Rows[Sorindex + 3].Cells[4].Value.ToString();
            txMh3.Text = dgwForgalmak.Rows[Sorindex + 3].Cells[5].Value.ToString();
            txRend3.Text = dgwForgalmak.Rows[Sorindex + 3].Cells[6].Value.ToString();
            txEgysz3.Text = dgwForgalmak.Rows[Sorindex + 3].Cells[7].Value.ToString();
            txBef3.Text = dgwForgalmak.Rows[Sorindex + 3].Cells[8].Value.ToString();
            txStorno3.Text = dgwForgalmak.Rows[Sorindex + 3].Cells[9].Value.ToString();
            txPnrid3.Text = dgwForgalmak.Rows[Sorindex + 3].Cells[10].Value.ToString();
        }

        private void SorFeltolt4()
        {
            txForgid4.Text = dgwForgalmak.Rows[Sorindex + 4].Cells[0].Value.ToString();
            txSorsz4.Text = (Sorindex + 5).ToString();
            txAD4.Text = dgwForgalmak.Rows[Sorindex + 4].Cells[1].Value.ToString();
            txNev4.Text = dgwForgalmak.Rows[Sorindex + 4].Cells[2].Value.ToString();
            txErv4.Text = dgwForgalmak.Rows[Sorindex + 4].Cells[3].Value.ToString();
            txSaj4.Text = dgwForgalmak.Rows[Sorindex + 4].Cells[4].Value.ToString();
            txMh4.Text = dgwForgalmak.Rows[Sorindex + 4].Cells[5].Value.ToString();
            txRend4.Text = dgwForgalmak.Rows[Sorindex + 4].Cells[6].Value.ToString();
            txEgysz4.Text = dgwForgalmak.Rows[Sorindex + 4].Cells[7].Value.ToString();
            txBef4.Text = dgwForgalmak.Rows[Sorindex + 4].Cells[8].Value.ToString();
            txStorno4.Text = dgwForgalmak.Rows[Sorindex + 4].Cells[9].Value.ToString();
            txPnrid4.Text = dgwForgalmak.Rows[Sorindex + 4].Cells[10].Value.ToString();
        }

        private void sorokKiurit()
        {
            txSorsz0.Text = "1";
            txAD0.Text = string.Empty;
            txNev0.Text = string.Empty;
            txErv0.Text = string.Empty;
            txSaj0.Text = string.Empty;
            txMh0.Text = string.Empty;
            txRend0.Text = string.Empty;
            txEgysz0.Text = string.Empty;
            txBef0.Text = string.Empty;
            txStorno0.Text = string.Empty;
            txPnrid0.Text = string.Empty;
            txForgid0.Text = string.Empty;

            txSorsz1.Text = string.Empty;
            txAD1.Text = string.Empty;
            txNev1.Text = string.Empty;
            txErv1.Text = string.Empty;
            txSaj1.Text = string.Empty;
            txMh1.Text = string.Empty;
            txRend1.Text = string.Empty;
            txEgysz1.Text = string.Empty;
            txBef1.Text = string.Empty;
            txStorno1.Text = string.Empty;
            txPnrid1.Text = string.Empty;
            txForgid1.Text = string.Empty;

            txSorsz2.Text = string.Empty;
            txAD2.Text = string.Empty;
            txNev2.Text = string.Empty;
            txErv2.Text = string.Empty;
            txSaj2.Text = string.Empty;
            txMh2.Text = string.Empty;
            txRend2.Text = string.Empty;
            txEgysz2.Text = string.Empty;
            txBef2.Text = string.Empty;
            txStorno2.Text = string.Empty;
            txPnrid2.Text = string.Empty;
            txForgid2.Text = string.Empty;

            txSorsz3.Text = string.Empty;
            txAD3.Text = string.Empty;
            txNev3.Text = string.Empty;
            txErv3.Text = string.Empty;
            txSaj3.Text = string.Empty;
            txMh3.Text = string.Empty;
            txRend3.Text = string.Empty;
            txEgysz3.Text = string.Empty;
            txBef3.Text = string.Empty;
            txStorno3.Text = string.Empty;
            txPnrid3.Text = string.Empty;
            txForgid3.Text = string.Empty;

            txSorsz4.Text = string.Empty;
            txAD4.Text = string.Empty;
            txNev4.Text = string.Empty;
            txErv4.Text = string.Empty;
            txSaj4.Text = string.Empty;
            txMh4.Text = string.Empty;
            txRend4.Text = string.Empty;
            txEgysz4.Text = string.Empty;
            txBef4.Text = string.Empty;
            txStorno4.Text = string.Empty;
            txPnrid4.Text = string.Empty;
            txForgid4.Text = string.Empty;
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

        private void SumTagok()
        {
            // sum tagok száma
            Tagokszama = 0;
            for (int i = 0; i < dgwForgalmak.RowCount; i++)
            {
                Tagokszama++;
            }
            txTagokSumC.Text = Tagokszama.ToString();
        }

        private void SumCalc()
        {
            int SorSajat = 0;
            int SorMH = 0;
            int SorRendsz = 0;
            int SorEgysz = 0;
            int SorBefizetendo = 0;

            for (int i = 0; i < dgwForgalmak.RowCount; i++)
            {
                SorSajat += int.Parse(dgwForgalmak.Rows[i].Cells[4].Value.ToString());
                SorMH += int.Parse(dgwForgalmak.Rows[i].Cells[5].Value.ToString());
                SorRendsz += int.Parse(dgwForgalmak.Rows[i].Cells[6].Value.ToString());
                SorEgysz += int.Parse(dgwForgalmak.Rows[i].Cells[7].Value.ToString());
                SorBefizetendo += int.Parse(dgwForgalmak.Rows[i].Cells[8].Value.ToString());
            }
            txSajatSumC.Text = SorSajat.ToString();
            txMunkSumC.Text = SorMH.ToString();
            txRendszSumC.Text = SorRendsz.ToString();
            TxEgyszeriSumC.Text = SorEgysz.ToString();
            TxTotalC.Text = SorBefizetendo.ToString();
        }

        private void TraceBejegyzes(string msg)
        {
            myListener.WriteLine("-" + DateTime.Now + ": " + msg);
            myListener.Flush();
            myListener.Close();
        }

        private void bUp_Click(object sender, EventArgs e)
        {
            MovesUp();
        }

        private void bDown_Click(object sender, EventArgs e)
        {
            MovesDown();
        }

        private void MovesUp()
        {
            if (txSorsz0.Text != string.Empty)
            {
                if (int.Parse(txSorsz0.Text) > 1)
                {
                    Sorindex = int.Parse(txSorsz0.Text) - 1;
                    txSorsz0.Text = Sorindex.ToString();
                    txAD0.Text = dgwForgalmak.Rows[Sorindex - 1].Cells[1].Value.ToString();
                    txNev0.Text = dgwForgalmak.Rows[Sorindex - 1].Cells[2].Value.ToString();
                    txErv0.Text = dgwForgalmak.Rows[Sorindex - 1].Cells[3].Value.ToString();
                    txSaj0.Text = dgwForgalmak.Rows[Sorindex - 1].Cells[4].Value.ToString();
                    txMh0.Text = dgwForgalmak.Rows[Sorindex - 1].Cells[5].Value.ToString();
                    txRend0.Text = dgwForgalmak.Rows[Sorindex - 1].Cells[6].Value.ToString();
                    txEgysz0.Text = dgwForgalmak.Rows[Sorindex - 1].Cells[7].Value.ToString();
                    txBef0.Text = dgwForgalmak.Rows[Sorindex - 1].Cells[8].Value.ToString();
                    txStorno0.Text = dgwForgalmak.Rows[Sorindex - 1].Cells[9].Value.ToString();
                    txPnrid0.Text = dgwForgalmak.Rows[Sorindex - 1].Cells[10].Value.ToString();
                    txForgid0.Text = dgwForgalmak.Rows[Sorindex - 1].Cells[0].Value.ToString();

                    if (Sorindex <= dgwForgalmak.RowCount)
                    {
                        txSorsz1.Text = (Sorindex + 1).ToString();
                        txAD1.Text = dgwForgalmak.Rows[Sorindex].Cells[1].Value.ToString();
                        txNev1.Text = dgwForgalmak.Rows[Sorindex].Cells[2].Value.ToString();
                        txErv1.Text = dgwForgalmak.Rows[Sorindex].Cells[3].Value.ToString();
                        txSaj1.Text = dgwForgalmak.Rows[Sorindex].Cells[4].Value.ToString();
                        txMh1.Text = dgwForgalmak.Rows[Sorindex].Cells[5].Value.ToString();
                        txRend1.Text = dgwForgalmak.Rows[Sorindex].Cells[6].Value.ToString();
                        txEgysz1.Text = dgwForgalmak.Rows[Sorindex].Cells[7].Value.ToString();
                        txBef1.Text = dgwForgalmak.Rows[Sorindex].Cells[8].Value.ToString();
                        txStorno1.Text = dgwForgalmak.Rows[Sorindex].Cells[9].Value.ToString();
                        txPnrid1.Text = dgwForgalmak.Rows[Sorindex].Cells[10].Value.ToString();
                        txForgid1.Text = dgwForgalmak.Rows[Sorindex].Cells[0].Value.ToString();

                        txSorsz2.Text = (Sorindex + 2).ToString();
                        txAD2.Text = dgwForgalmak.Rows[Sorindex + 1].Cells[1].Value.ToString();
                        txNev2.Text = dgwForgalmak.Rows[Sorindex + 1].Cells[2].Value.ToString();
                        txErv2.Text = dgwForgalmak.Rows[Sorindex + 1].Cells[3].Value.ToString();
                        txSaj2.Text = dgwForgalmak.Rows[Sorindex + 1].Cells[4].Value.ToString();
                        txMh2.Text = dgwForgalmak.Rows[Sorindex + 1].Cells[5].Value.ToString();
                        txRend2.Text = dgwForgalmak.Rows[Sorindex + 1].Cells[6].Value.ToString();
                        txEgysz2.Text = dgwForgalmak.Rows[Sorindex + 1].Cells[7].Value.ToString();
                        txBef2.Text = dgwForgalmak.Rows[Sorindex + 1].Cells[8].Value.ToString();
                        txStorno2.Text = dgwForgalmak.Rows[Sorindex + 1].Cells[9].Value.ToString();
                        txPnrid2.Text = dgwForgalmak.Rows[Sorindex + 1].Cells[10].Value.ToString();
                        txForgid2.Text = dgwForgalmak.Rows[Sorindex + 1].Cells[0].Value.ToString();

                        txSorsz3.Text = (Sorindex + 3).ToString();
                        txAD3.Text = dgwForgalmak.Rows[Sorindex + 2].Cells[1].Value.ToString();
                        txNev3.Text = dgwForgalmak.Rows[Sorindex + 2].Cells[2].Value.ToString();
                        txErv3.Text = dgwForgalmak.Rows[Sorindex + 2].Cells[3].Value.ToString();
                        txSaj3.Text = dgwForgalmak.Rows[Sorindex + 2].Cells[4].Value.ToString();
                        txMh3.Text = dgwForgalmak.Rows[Sorindex + 2].Cells[5].Value.ToString();
                        txRend3.Text = dgwForgalmak.Rows[Sorindex + 2].Cells[6].Value.ToString();
                        txEgysz3.Text = dgwForgalmak.Rows[Sorindex + 2].Cells[7].Value.ToString();
                        txBef3.Text = dgwForgalmak.Rows[Sorindex + 2].Cells[8].Value.ToString();
                        txStorno3.Text = dgwForgalmak.Rows[Sorindex + 2].Cells[9].Value.ToString();
                        txPnrid3.Text = dgwForgalmak.Rows[Sorindex + 2].Cells[10].Value.ToString();
                        txForgid3.Text = dgwForgalmak.Rows[Sorindex + 2].Cells[0].Value.ToString();

                        txSorsz4.Text = (Sorindex + 4).ToString();
                        txAD4.Text = dgwForgalmak.Rows[Sorindex + 3].Cells[1].Value.ToString();
                        txNev4.Text = dgwForgalmak.Rows[Sorindex + 3].Cells[2].Value.ToString();
                        txErv4.Text = dgwForgalmak.Rows[Sorindex + 3].Cells[3].Value.ToString();
                        txSaj4.Text = dgwForgalmak.Rows[Sorindex + 3].Cells[4].Value.ToString();
                        txMh4.Text = dgwForgalmak.Rows[Sorindex + 3].Cells[5].Value.ToString();
                        txRend4.Text = dgwForgalmak.Rows[Sorindex + 3].Cells[6].Value.ToString();
                        txEgysz4.Text = dgwForgalmak.Rows[Sorindex + 3].Cells[7].Value.ToString();
                        txBef4.Text = dgwForgalmak.Rows[Sorindex + 3].Cells[8].Value.ToString();
                        txStorno4.Text = dgwForgalmak.Rows[Sorindex + 3].Cells[9].Value.ToString();
                        txPnrid4.Text = dgwForgalmak.Rows[Sorindex + 3].Cells[10].Value.ToString();
                        txForgid4.Text = dgwForgalmak.Rows[Sorindex + 3].Cells[0].Value.ToString();
                    }
                }
            }
        }

        private void MovesDown()
        {
            if (txSorsz4.Text != string.Empty)
            {
                if (int.Parse(txSorsz4.Text) < dgwForgalmak.RowCount)
                {
                    Sorindex = int.Parse(txSorsz1.Text);
                    txSorsz0.Text = Sorindex.ToString();
                    txAD0.Text = dgwForgalmak.Rows[Sorindex - 1].Cells[1].Value.ToString();
                    txNev0.Text = dgwForgalmak.Rows[Sorindex - 1].Cells[2].Value.ToString();
                    txErv0.Text = dgwForgalmak.Rows[Sorindex - 1].Cells[3].Value.ToString();
                    txSaj0.Text = dgwForgalmak.Rows[Sorindex - 1].Cells[4].Value.ToString();
                    txMh0.Text = dgwForgalmak.Rows[Sorindex - 1].Cells[5].Value.ToString();
                    txRend0.Text = dgwForgalmak.Rows[Sorindex - 1].Cells[6].Value.ToString();
                    txEgysz0.Text = dgwForgalmak.Rows[Sorindex - 1].Cells[7].Value.ToString();
                    txBef0.Text = dgwForgalmak.Rows[Sorindex - 1].Cells[8].Value.ToString();
                    txStorno0.Text = dgwForgalmak.Rows[Sorindex - 1].Cells[9].Value.ToString();
                    txPnrid0.Text = dgwForgalmak.Rows[Sorindex - 1].Cells[10].Value.ToString();
                    txForgid0.Text = dgwForgalmak.Rows[Sorindex - 1].Cells[0].Value.ToString();

                    if (Sorindex <= dgwForgalmak.RowCount)
                    {
                        txSorsz1.Text = (Sorindex + 1).ToString();
                        txAD1.Text = dgwForgalmak.Rows[Sorindex].Cells[1].Value.ToString();
                        txNev1.Text = dgwForgalmak.Rows[Sorindex].Cells[2].Value.ToString();
                        txErv1.Text = dgwForgalmak.Rows[Sorindex].Cells[3].Value.ToString();
                        txSaj1.Text = dgwForgalmak.Rows[Sorindex].Cells[4].Value.ToString();
                        txMh1.Text = dgwForgalmak.Rows[Sorindex].Cells[5].Value.ToString();
                        txRend1.Text = dgwForgalmak.Rows[Sorindex].Cells[6].Value.ToString();
                        txEgysz1.Text = dgwForgalmak.Rows[Sorindex].Cells[7].Value.ToString();
                        txBef1.Text = dgwForgalmak.Rows[Sorindex].Cells[8].Value.ToString();
                        txStorno1.Text = dgwForgalmak.Rows[Sorindex].Cells[9].Value.ToString();
                        txPnrid1.Text = dgwForgalmak.Rows[Sorindex].Cells[10].Value.ToString();
                        txForgid1.Text = dgwForgalmak.Rows[Sorindex].Cells[0].Value.ToString();

                        txSorsz2.Text = (Sorindex + 2).ToString();
                        txAD2.Text = dgwForgalmak.Rows[Sorindex + 1].Cells[1].Value.ToString();
                        txNev2.Text = dgwForgalmak.Rows[Sorindex + 1].Cells[2].Value.ToString();
                        txErv2.Text = dgwForgalmak.Rows[Sorindex + 1].Cells[3].Value.ToString();
                        txSaj2.Text = dgwForgalmak.Rows[Sorindex + 1].Cells[4].Value.ToString();
                        txMh2.Text = dgwForgalmak.Rows[Sorindex + 1].Cells[5].Value.ToString();
                        txRend2.Text = dgwForgalmak.Rows[Sorindex + 1].Cells[6].Value.ToString();
                        txEgysz2.Text = dgwForgalmak.Rows[Sorindex + 1].Cells[7].Value.ToString();
                        txBef2.Text = dgwForgalmak.Rows[Sorindex + 1].Cells[8].Value.ToString();
                        txStorno2.Text = dgwForgalmak.Rows[Sorindex + 1].Cells[9].Value.ToString();
                        txPnrid2.Text = dgwForgalmak.Rows[Sorindex + 1].Cells[10].Value.ToString();
                        txForgid2.Text = dgwForgalmak.Rows[Sorindex + 1].Cells[0].Value.ToString();

                        txSorsz3.Text = (Sorindex + 3).ToString();
                        txAD3.Text = dgwForgalmak.Rows[Sorindex + 2].Cells[1].Value.ToString();
                        txNev3.Text = dgwForgalmak.Rows[Sorindex + 2].Cells[2].Value.ToString();
                        txErv3.Text = dgwForgalmak.Rows[Sorindex + 2].Cells[3].Value.ToString();
                        txSaj3.Text = dgwForgalmak.Rows[Sorindex + 2].Cells[4].Value.ToString();
                        txMh3.Text = dgwForgalmak.Rows[Sorindex + 2].Cells[5].Value.ToString();
                        txRend3.Text = dgwForgalmak.Rows[Sorindex + 2].Cells[6].Value.ToString();
                        txEgysz3.Text = dgwForgalmak.Rows[Sorindex + 2].Cells[7].Value.ToString();
                        txBef3.Text = dgwForgalmak.Rows[Sorindex + 2].Cells[8].Value.ToString();
                        txStorno3.Text = dgwForgalmak.Rows[Sorindex + 2].Cells[9].Value.ToString();
                        txPnrid3.Text = dgwForgalmak.Rows[Sorindex + 2].Cells[10].Value.ToString();
                        txForgid3.Text = dgwForgalmak.Rows[Sorindex + 2].Cells[0].Value.ToString();

                        try
                        {
                            txSorsz4.Text = (Sorindex + 4).ToString();
                            txAD4.Text = dgwForgalmak.Rows[Sorindex + 3].Cells[1].Value.ToString();
                            txNev4.Text = dgwForgalmak.Rows[Sorindex + 3].Cells[2].Value.ToString();
                            txErv4.Text = dgwForgalmak.Rows[Sorindex + 3].Cells[3].Value.ToString();
                            txSaj4.Text = dgwForgalmak.Rows[Sorindex + 3].Cells[4].Value.ToString();
                            txMh4.Text = dgwForgalmak.Rows[Sorindex + 3].Cells[5].Value.ToString();
                            txRend4.Text = dgwForgalmak.Rows[Sorindex + 3].Cells[6].Value.ToString();
                            txEgysz4.Text = dgwForgalmak.Rows[Sorindex + 3].Cells[7].Value.ToString();
                            txBef4.Text = dgwForgalmak.Rows[Sorindex + 3].Cells[8].Value.ToString();
                            txStorno4.Text = dgwForgalmak.Rows[Sorindex + 3].Cells[9].Value.ToString();
                            txPnrid4.Text = dgwForgalmak.Rows[Sorindex + 3].Cells[10].Value.ToString();
                            txForgid4.Text = dgwForgalmak.Rows[Sorindex + 3].Cells[0].Value.ToString();
                        }
                        catch
                        {

                        }
                    }
                }
            }
        }

        private void txErv_TextChanged(object sender, EventArgs e)
        {
            if (txErv.Text == "N")
                txErv.BackColor = Color.IndianRed;
            else
                txErv.BackColor = Color.LightGreen;
        }

        #region nyíl billentyűk kezelése

        #region adóazonosító
        private void txAD0_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Down:
                    if (txAD0.Text != string.Empty)
                    {
                        
                        txAD1.Focus();
                    }
                    break;
                case Keys.Up:
                    if (txAD0.Text != string.Empty)
                    {
                        
                        MovesUp();
                    }
                    break;
                default: break;
            }
        }

        private void txAD1_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Down:
                    if (txAD1.Text != string.Empty)
                    {
                        
                        txAD2.Focus();
                    }
                    break;
                case Keys.Up:
                    if (txAD1.Text != string.Empty)
                    {
                        
                        txAD0.Focus();
                    }
                    else if (txPnrid1.Text == string.Empty)
                    {
                        
                        txAD0.Focus();
                    }
                    break;
                default: break;
            }
        }

        private void txAD2_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Down:
                    if (txAD2.Text != string.Empty)
                    {
                        
                        txAD3.Focus();
                    }
                    break;
                case Keys.Up:
                    if (txAD2.Text != string.Empty)
                    {
                        
                        txAD1.Focus();
                    }
                    else if (txPnrid2.Text == string.Empty)
                    {
                        
                        txAD1.Focus();
                    }
                    break;
                default: break;
            }
        }

        private void txAD3_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Down:
                    if (txAD3.Text != string.Empty)
                    {
                        
                        txAD4.Focus();
                    }
                    break;
                case Keys.Up:
                    if (txAD3.Text != string.Empty)
                    {
                        
                        txAD2.Focus();
                    }
                    else if (txPnrid3.Text == string.Empty)
                    {
                        
                        txAD2.Focus();
                    }
                    break;
                default: break;
            }
        }

        private void txAD4_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Down:
                    try
                    {
                        if ((txAD4.Text != string.Empty) && (int.Parse(txSorsz4.Text) < dgwForgalmak.RowCount))
                        {
                            
                            MovesDown();
                        }
                    }
                    catch
                    { }
                    break;
                case Keys.Up:
                    if (txAD4.Text != string.Empty)
                    {
                        
                        txAD3.Focus();
                    }
                    else if (txPnrid4.Text == string.Empty)
                    {
                        
                        txAD3.Focus();
                    }
                    break;
                default: break;
            }
        }
        #endregion adóazonosító

        #region saját
        private void txSaj0_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Down:
                    if (txAD0.Text != string.Empty)
                    {
                        
                        txSaj1.Focus();
                    }
                    break;
                case Keys.Up:
                    if (txAD0.Text != string.Empty)
                    {
                        
                        MovesUp();
                    }
                    break;
                default: break;
            }
        }

        private void txSaj1_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Down:
                    if (txAD1.Text != string.Empty)
                    {
                        
                        txSaj2.Focus();
                    }
                    break;
                case Keys.Up:
                    if (txAD1.Text != string.Empty)
                    {
                        
                        txSaj0.Focus();
                    }
                    break;
                default: break;
            }
        }

        private void txSaj2_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Down:
                    if (txAD2.Text != string.Empty)
                    {
                        
                        txSaj3.Focus();
                    }
                    break;
                case Keys.Up:
                    if (txAD2.Text != string.Empty)
                    {
                        
                        txSaj1.Focus();
                    }
                    break;
                default: break;
            }
        }

        private void txSaj3_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Down:
                    if (txAD3.Text != string.Empty)
                    {
                        
                        txSaj4.Focus();
                    }
                    break;
                case Keys.Up:
                    if (txAD3.Text != string.Empty)
                    {
                        
                        txSaj2.Focus();
                    }
                    break;
                default: break;
            }
        }

        private void txSaj4_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Down:
                    try
                    {
                        if ((txAD4.Text != string.Empty) && (int.Parse(txSorsz4.Text) < dgwForgalmak.RowCount))
                        {
                            
                            MovesDown();
                        }
                    }
                    catch (Exception)
                    {
                        //
                    }
                    break;
                case Keys.Up:
                    if (txAD4.Text != string.Empty)
                    {
                        
                        txSaj3.Focus();
                    }
                    break;
                default: break;
            }
        }
        #endregion saját

        #region mh
        private void txMh0_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Down:
                    if (txAD0.Text != string.Empty)
                        txMh1.Focus();
                    
                    break;
                case Keys.Up:
                    if (txAD0.Text != string.Empty)
                        MovesUp();
                    
                    break;
                default: break;
            }
        }

        private void txMh1_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Down:
                    if (txAD1.Text != string.Empty)
                        txMh2.Focus();
                    
                    break;
                case Keys.Up:
                    if (txAD1.Text != string.Empty)
                        txMh0.Focus();
                    
                    break;
                default: break;
            }
        }

        private void txMh2_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Down:
                    if (txAD2.Text != string.Empty)
                        txMh3.Focus();
                    
                    break;
                case Keys.Up:
                    if (txAD2.Text != string.Empty)
                        txMh1.Focus();
                    
                    break;
                default: break;
            }
        }

        private void txMh3_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Down:
                    if (txAD3.Text != string.Empty)
                        txMh4.Focus();
                    
                    break;
                case Keys.Up:
                    if (txAD3.Text != string.Empty)
                        txMh2.Focus();
                    
                    break;
                default: break;
            }
        }

        private void txMh4_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Down:
                    try
                    {
                        if ((txAD4.Text != string.Empty) && (int.Parse(txSorsz4.Text) < dgwForgalmak.RowCount))
                            MovesDown();
                    }
                    catch (Exception)
                    {

                        //
                    }
                    
                    break;
                case Keys.Up:
                    if (txAD4.Text != string.Empty)
                        txMh3.Focus();
                    
                    break;
                default: break;
            }
        }
        #endregion mh

        #region rendszeres
        private void txRend0_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Down:
                    if (txAD0.Text != string.Empty)
                        txRend1.Focus();
                    
                    break;
                case Keys.Up:
                    if (txAD0.Text != string.Empty)
                        MovesUp();
                    
                    break;
                default: break;
            }
        }

        private void txRend1_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Down:
                    if (txAD1.Text != string.Empty)
                        txRend2.Focus();
                    
                    break;
                case Keys.Up:
                    if (txAD1.Text != string.Empty)
                        txRend0.Focus();
                    
                    break;
                default: break;
            }
        }

        private void txRend2_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Down:
                    if (txAD2.Text != string.Empty)
                        txRend3.Focus();
                    
                    break;
                case Keys.Up:
                    if (txAD2.Text != string.Empty)
                        txRend1.Focus();
                    
                    break;
                default: break;
            }
        }

        private void txRend3_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Down:
                    if (txAD3.Text != string.Empty)
                        txRend4.Focus();
                    
                    break;
                case Keys.Up:
                    if (txAD3.Text != string.Empty)
                        txRend2.Focus();
                    
                    break;
                default: break;
            }
        }

        private void txRend4_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Down:
                    try
                    {
                        if ((txAD4.Text != string.Empty) && (int.Parse(txSorsz4.Text) < dgwForgalmak.RowCount))
                            MovesDown();
                    }
                    catch (Exception)
                    {

                        //
                    }
                    break;
                case Keys.Up:
                    if (txAD4.Text != string.Empty)
                        txRend3.Focus();
                    break;
                default: break;
            }
        }
        #endregion rendszeres

        #region egyszeri
        private void txEgysz0_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Down:
                    if (txAD0.Text != string.Empty)
                        txEgysz1.Focus();
                    break;
                case Keys.Up:
                    if (txAD0.Text != string.Empty)
                        MovesUp();
                    break;
                default: break;
            }
        }

        private void txEgysz1_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Down:
                    if (txAD1.Text != string.Empty)
                        txEgysz2.Focus();
                    break;
                case Keys.Up:
                    if (txAD1.Text != string.Empty)
                        txEgysz0.Focus();
                    break;
                default: break;
            }
        }

        private void txEgysz2_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Down:
                    if (txAD2.Text != string.Empty)
                        txEgysz3.Focus();
                    break;
                case Keys.Up:
                    if (txAD2.Text != string.Empty)
                        txEgysz1.Focus();
                    break;
                default: break;
            }
        }

        private void txEgysz3_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Down:
                    if (txAD3.Text != string.Empty)
                        txEgysz4.Focus();
                    break;
                case Keys.Up:
                    if (txAD3.Text != string.Empty)
                        txEgysz2.Focus();
                    break;
                default: break;
            }
        }

        private void txEgysz4_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Down:
                    try
                    {
                        if ((txAD4.Text != string.Empty) && (int.Parse(txSorsz4.Text) < dgwForgalmak.RowCount))
                            MovesDown();
                    }
                    catch
                    {
                        //
                    }
                    break;
                case Keys.Up:
                    if (txAD4.Text != string.Empty)
                        txEgysz3.Focus();
                    break;
                default: break;
            }
        }
        #endregion egyszeri

        #region befizetendő
        private void txBef0_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Down:
                    if (txAD0.Text != string.Empty)
                        txBef1.Focus();
                    break;
                case Keys.Up:
                    if (txAD0.Text != string.Empty)
                        MovesUp();
                    break;
                case Keys.Enter:
                    break;
                default: break;
            }
        }

        private void txBef1_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Down:
                    if (txAD1.Text != string.Empty)
                        txBef2.Focus();
                    break;
                case Keys.Up:
                    if (txAD1.Text != string.Empty)
                        txBef0.Focus();
                    break;
                case Keys.Enter:
                    break;
                default: break;
            }
        }

        private void txBef2_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Down:
                    if (txAD2.Text != string.Empty)
                        txBef3.Focus();
                    break;
                case Keys.Up:
                    if (txAD2.Text != string.Empty)
                        txBef1.Focus();
                    break;
                case Keys.Enter:
                    break;
                default: break;
            }
        }

        private void txBef3_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Down:
                    if (txAD3.Text != string.Empty)
                        txBef4.Focus();
                    break;
                case Keys.Up:
                    if (txAD3.Text != string.Empty)
                        txBef2.Focus();
                    break;
                case Keys.Enter:
                    break;
                default: break;
            }
        }

        private void txBef4_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Down:
                    try
                    {
                        if ((txAD4.Text != string.Empty) && (int.Parse(txSorsz4.Text) < dgwForgalmak.RowCount))
                            MovesDown();
                    }
                    catch (Exception)
                    {
                        //
                    }
                    break;
                case Keys.Up:
                    if (txAD4.Text != string.Empty)
                        txBef3.Focus();
                    break;
                case Keys.Enter:
                    Bef4Leave();
                    break;
                default: break;
            }
        }

        private void Bef4Leave()
        {
            if (txAD4.Text != string.Empty)
            {
                int sor = int.Parse(txSorsz4.Text);
                if (txBef4.Text != string.Empty)
                    dgwForgalmak.Rows[sor - 1].Cells[8].Value = txBef4.Text;
                else
                    dgwForgalmak.Rows[sor - 1].Cells[8].Value = "0";
                TotalOsszesit();
                txAD4.Focus();
            }
            else
            {
                MessageBox.Show("Adóazonosító jel nem lehet üres!");
                txAD4.Focus();
            }
        }
        #endregion befizetendő

        #endregion nyíl billentyűk kezelése

        #region érvényes mező BackColor beállításai
        private void txErv0_TextChanged(object sender, EventArgs e)
        {
            if (txErv0.Text == "N")
                txErv0.BackColor = Color.IndianRed;
            else if (txErv0.Text == "I")
                txErv0.BackColor = Color.LightGreen;
            else txErv0.BackColor = SystemColors.Window;
        }

        private void txErv1_TextChanged(object sender, EventArgs e)
        {
            if (txErv1.Text == "N")
                txErv1.BackColor = Color.IndianRed;
            else if (txErv1.Text == "I")
                txErv1.BackColor = Color.LightGreen;
            else txErv1.BackColor = SystemColors.Window;
        }

        private void txErv2_TextChanged(object sender, EventArgs e)
        {
            if (txErv2.Text == "N")
                txErv2.BackColor = Color.IndianRed;
            else if (txErv2.Text == "I")
                txErv2.BackColor = Color.LightGreen;
            else txErv2.BackColor = SystemColors.Window;
        }

        private void txErv3_TextChanged(object sender, EventArgs e)
        {
            if (txErv3.Text == "N")
                txErv3.BackColor = Color.IndianRed;
            else if (txErv3.Text == "I")
                txErv3.BackColor = Color.LightGreen;
            else txErv3.BackColor = SystemColors.Window;
        }

        private void txErv4_TextChanged(object sender, EventArgs e)
        {
            if (txErv4.Text == "N")
                txErv4.BackColor = Color.IndianRed;
            else if (txErv4.Text == "I")
                txErv4.BackColor = Color.LightGreen;
            else txErv4.BackColor = SystemColors.Window;
        }
        #endregion érvényes mező BackColor beállítása

        private void txSorsz0_TextChanged(object sender, EventArgs e)
        {
            if (txSorsz0.Text != string.Empty && int.Parse(txSorsz0.Text) > 1)
            {
                bUp.Enabled = true;
            }
            else
            {
                bUp.Enabled = false;
            }
        }

        private void txSorsz4_TextChanged(object sender, EventArgs e)
        {
            if (txSorsz4.Text != string.Empty && dgwForgalmak.RowCount > 4)
            {
                if (int.Parse(txSorsz4.Text) < dgwForgalmak.RowCount)
                    bDown.Enabled = true;
                else
                    bDown.Enabled = false;
            }
            else
                bDown.Enabled = false;
        }

        private void bHibak_Click(object sender, EventArgs e)
        {
            AnalitikaHibak ah = new AnalitikaHibak(sconn, bev_id);
            ah.ShowDialog();
        }

        private void TotalOsszesit()
        {
            TotalSum = 0;
            for (int i = 0; i < dgwForgalmak.RowCount; i++)
            {
                TotalSum += int.Parse(dgwForgalmak.Rows[i].Cells[8].Value.ToString());
            }
            TxTotalC.Text = TotalSum.ToString();
        }

        private void BevBong_Load(object sender, EventArgs e)
        {

        }
    }
}
