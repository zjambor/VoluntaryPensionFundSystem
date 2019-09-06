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
    public partial class BevRogzUj : VoluntaryPensionFundSystem.BaseForm
    {
        private SqlConnection sconn;
        private SqlCommand scommand;
        private int TranID = 0;
        private SqlDataAdapter daBev;
        private DataTable dt, dtBev;
        private DataRow dr;
        //private int i = 0;
        private string querystring;

        //fejrész
        private int idk_id = 0;
        private int bev_id = 0;
        private int Evho = 0;
        private bool InsertTrueUpdateFalse = true;
        private bool back, enter = false;
        private bool Fejhiba = false, Sorhiba = false;
        //private bool fejresz = true;
        private DateTime mainap = DateTime.Now;
        private string Mainap;

        //forgalom rész
        private int Sorindex = 0;
        private int SajatSum = 0, MunkSum = 0, RendszSum = 0, EgyszeriSum = 0, TotalSum = 0, Tagokszama = 0;
        private string Pnridtext, nev;
        private int sorszam = 0;
        private string nullvalue = "0";
        private bool makeNewRow = false;
        private bool Rowleave = false;

        public string Bevid, ErkSorsz, Erkdate, Iktatoszam, IktKelte, Adatkozl, Megjegyzes, Storno, VonIdoszak, Ervenyes, Konyvelt;
        public string Sajat, Munkh, Tagok, Rendszeres, Egyszeri, Total;

        // Napló logok:
        private TextWriterTraceListener myListener = new TextWriterTraceListener(@"LOG\BevallasRogzites.log", "myListener");

        public BevRogzUj(SqlConnection SqlConn, string Pnridtext, string adoszam, string adoazon, string nev, string helyseg, string cim, string irszam)
        {
            InitializeComponent();

            this.sconn = SqlConn;
            if (sconn.State == ConnectionState.Closed) sconn.Open();

            this.Pnridtext = Pnridtext;
            this.nev = nev;

            txPnrid.Text = Pnridtext;
            txAdoszam.Text = adoszam;
            //txAdoazon.Text = adoazon;
            txMegnev.Text = nev;
            txHelyseg.Text = helyseg;
            txCim.Text = cim;
            txIrszam.Text = irszam;

            newEvHo();

            tsDelete.Enabled = true;
            tsFind.Enabled = false;
            tsNew.Enabled = false;
            tsSave.Enabled = false;
            tsSearch.Enabled = true;
            tsUpdate.Enabled = false;

            txIktKelte.Text = mainap.ToString("yyyy.MM.dd.");

            //scommand = new SqlCommand("spForgalmak", sconn);
            //scommand.CommandType = CommandType.StoredProcedure;
            //scommand.Parameters.Add(new SqlParameter("@bev_id", SqlDbType.Int)).Value = 1;

            //da = new SqlDataAdapter(scommand);
            dt = new DataTable();

            //Frissit();
            //this.dgwForgalmak.DataSource = dt;
            txErv.BackColor = Color.LightGreen;
            txVonIdoszak.Focus();
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

        private void BevRogzUj_Load(object sender, EventArgs e)
        {
            Frissit();
        }

        // Auto betöltés
        private void Reload()
        {
            
        }      

        private void dgwForgalmak_SelectionChanged(object sender, EventArgs e)
        {
            
        }

        // az aktuális vonatkozási időszak megkeresése és betöltése
        private void newEvHo()
        {
            Mainap = mainap.ToString("yyyy.MM.dd");
            scommand = new SqlCommand("select ev_ho from idoszakok where kezdete<='" + Mainap + "' and vege>='" + Mainap + "';", sconn);
            try
            {
                Evho = int.Parse(scommand.ExecuteScalar().ToString());
            }
            catch (Exception ex)
            {
                MessageBox.Show("SQL Hiba " + ex.Message);
                TraceBejegyzes(ex.Message);
            }
            txVonIdoszak.Text = Evho.ToString();
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

        public override void tsDelete_Click(object sender, EventArgs e)
        {
            deleteAll();
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
            InsertTrueUpdateFalse = true;
            txErkSorsz.Text = string.Empty;
            txErkDate.Text = string.Empty;
            txIktatoszam.Text = string.Empty;
            txAdkozlDate.Text = string.Empty;
            txMegjegyzes.Text = string.Empty;
            newEvHo();

            txErv.Text = "I";
            txTagokSum.Value = 0;
            txSajatSum.Value = 0;
            txMunkSum.Value = 0;
            txRendszSum.Value = 0;
            TxEgyszeriSum.Value = 0;
            TxTotal.Value = 0;
            txIktKelte.Text = Mainap;

            txStorno.Text = "0";
            txTagokSumC.Text = "0";
            txSajatSumC.Text = "0";
            txMunkSumC.Text = "0";
            txRendszSumC.Text = "0";
            TxEgyszeriSumC.Text = "0";
            TxTotalC.Text = "0";

            txErkSorsz.ReadOnly = false;
            txErkDate.ReadOnly = false;
            txIktatoszam.ReadOnly = false;
            txIktKelte.ReadOnly = false;
            txAdkozlDate.ReadOnly = false;
            txMegjegyzes.ReadOnly = false;

            txTagokSum.Enabled = true;
            txSajatSum.Enabled = true;
            txMunkSum.Enabled = true;
            txRendszSum.Enabled = true;
            TxEgyszeriSum.Enabled = true;
            TxTotal.Enabled = true;

            dt = new DataTable();
            Frissit();
            dgwForgalmak.DataSource = null;
            dgwForgalmak.Refresh();

            txSorsz0.Text = string.Empty;
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

            tsDelete.Enabled = false;
            tsFind.Enabled = false;
            tsNew.Enabled = false;
            tsSave.Enabled = false;
            tsSearch.Enabled = true;
            tsUpdate.Enabled = false;

            InsertTrueUpdateFalse = true;
        }

        private void updateRecord()
        {
            txErkSorsz.ReadOnly = false;
            txErkDate.ReadOnly = false;
            txIktatoszam.ReadOnly = false;
            txIktKelte.ReadOnly = false;
            txAdkozlDate.ReadOnly = false;
            txMegjegyzes.ReadOnly = false;

            txTagokSum.Enabled = true;
            txSajatSum.Enabled = true;
            txMunkSum.Enabled = true;
            txRendszSum.Enabled = true;
            TxEgyszeriSum.Enabled = true;
            TxTotal.Enabled = true;

            tsDelete.Enabled = false;
            tsFind.Enabled = false;
            tsNew.Enabled = false;
            //tsSave.Enabled = true;
            tsSearch.Enabled = false;
            tsUpdate.Enabled = false;
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

            txErkSorsz.BackColor = Color.Yellow;
            txErkDate.BackColor = Color.Yellow;
            txIktatoszam.BackColor = Color.Yellow;
            txIktKelte.BackColor = Color.Yellow;
            txAdkozlDate.BackColor = Color.Yellow;
            txMegjegyzes.BackColor = Color.Yellow;
            txErv.BackColor = Color.Yellow;
            txStorno.BackColor = Color.Yellow;
            txVonIdoszak.BackColor = Color.Yellow;

            txTagokSum.BackColor = Color.Yellow;
            txSajatSum.BackColor = Color.Yellow;
            txMunkSum.BackColor = Color.Yellow;
            txRendszSum.BackColor = Color.Yellow;
            TxEgyszeriSum.BackColor = Color.Yellow;
            TxTotal.BackColor = Color.Yellow;

            tsDelete.Enabled = false;
            tsFind.Enabled = true;
            tsNew.Enabled = false;
            tsSave.Enabled = false;
            tsSearch.Enabled = false;
            tsUpdate.Enabled = false;
        }

        public override void runQuery()
        {
            InsertTrueUpdateFalse = false;

            //dt = new DataTable();
            this.dgwForgalmak.DataSource = null;

            // A LEKÉRDEZÉS FELÉPÍTÉSE
            string query1 = "SELECT count(*)";
            string query2 = "SELECT b.bev_id, i.ev_ho, b.erkez_sorszam, b.erkez_datum, b.iktatoszam, b.iktatas_kelte, b.adatkozles_datum" +
                                    ", b.tagok_osszesen, b.sajat_ossz, b.hozzajarulas_ossz, b.rend_tamog_ossz, b.egysz_tamog_ossz" +
                                    ", b.mindosszesen, b.ervenyes, b.storno, b.megjegyzes, b.konyvelt";
            querystring = " FROM	bevallasok b, idoszakok i " +
                           "WHERE	b.idk_id=i.idk_id and ";

            querystring = txVonIdoszak.Text != string.Empty ? querystring + "i.ev_ho like '" + txVonIdoszak.Text + "' and " : querystring;
            querystring = txErkSorsz.Text != string.Empty ? querystring + "b.erkez_sorszam like '" + txErkSorsz.Text + "' and " : querystring;
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

            querystring += "b.pnr_id = " + txPnrid.Text;
            //MessageBox.Show(querystring);

            // mennyiségi ellenőrzés
            query1 += querystring;
            scommand = new SqlCommand(query1, sconn);
            int Mcount = (int)scommand.ExecuteScalar();

            // lekérdezés futtatása
            querystring += " order by ev_ho;";
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

                bev_id = int.Parse(Bevid);
                txErkSorsz.Text = ErkSorsz;
                txErkDate.Text = DateTime.Parse(Erkdate).ToShortDateString();
                txIktatoszam.Text = Iktatoszam;
                txIktKelte.Text = DateTime.Parse(IktKelte).ToShortDateString();
                txAdkozlDate.Text = DateTime.Parse(Adatkozl).ToShortDateString();
                txMegjegyzes.Text = Megjegyzes;
                txErv.Text = Ervenyes;

                txTagokSum.Value = int.Parse(Tagok);
                txSajatSum.Value = int.Parse(Sajat);
                txMunkSum.Value = int.Parse(Munkh);
                txRendszSum.Value = int.Parse(Rendszeres);
                TxEgyszeriSum.Value = int.Parse(Egyszeri);
                TxTotal.Value = int.Parse(Total);
                txStorno.Text = Storno;
                txVonIdoszak.Text = VonIdoszak;
                txKonyvelt.Text = Konyvelt;
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
            //txErv.BackColor = Color.LightGreen;
            txErkSorsz.BackColor = SystemColors.Window;
            txErkDate.BackColor = SystemColors.Window;
            txIktatoszam.BackColor = SystemColors.Window;
            txIktKelte.BackColor = SystemColors.Window;
            txAdkozlDate.BackColor = SystemColors.Window;
            txMegjegyzes.BackColor = SystemColors.Window;
            //txErv.BackColor = SystemColors.Window;
            txStorno.BackColor = SystemColors.Window;
            txVonIdoszak.BackColor = SystemColors.Window;

            txTagokSum.BackColor = SystemColors.Window;
            txSajatSum.BackColor = SystemColors.Window;
            txMunkSum.BackColor = SystemColors.Window;
            txRendszSum.BackColor = SystemColors.Window;
            TxEgyszeriSum.BackColor = SystemColors.Window;
            TxTotal.BackColor = SystemColors.Window;

            tsDelete.Enabled = true;
            tsFind.Enabled = false;
            tsNew.Enabled = true;
            tsSave.Enabled = false;
            tsSearch.Enabled = true;
            tsUpdate.Enabled = false;
        }

        private int HibaBeszurasa(int hibaID)
        {
            scommand = new SqlCommand("spAnalitikaHibaInsert", sconn);
            scommand.CommandType = CommandType.StoredProcedure;
            scommand.Parameters.Add(new SqlParameter("@ahb_id", SqlDbType.Int)).Value = 1;
            scommand.Parameters.Add(new SqlParameter("@hiba_id", SqlDbType.Int)).Value = hibaID;
            scommand.Parameters.Add(new SqlParameter("@bev_id", SqlDbType.Int)).Value = bev_id;
            scommand.Parameters.Add(new SqlParameter("@pnr_id", SqlDbType.Int)).Value = int.Parse(txPnrid.Text);

            int ahb_id = 0;
            try
            {
                ahb_id = (int)scommand.ExecuteScalar();
            }
            catch (Exception ex)
            {
                //MessageBox.Show("SQL hiba: " + ex.Message);
                TraceBejegyzes(ex.Message);
            }
            if (ahb_id == 0) { MessageBox.Show("A rekord mentése nem sikerült!"); return 1; }
            else
                return 0;
        }

        private int SorHibaBeszurasa(int hibaID, int forg_id)
        {
            scommand = new SqlCommand("spAnalitikaHibaInsert2", sconn);
            scommand.CommandType = CommandType.StoredProcedure;
            scommand.Parameters.Add(new SqlParameter("@ahb_id", SqlDbType.Int)).Value = 1;
            scommand.Parameters.Add(new SqlParameter("@hiba_id", SqlDbType.Int)).Value = hibaID;
            scommand.Parameters.Add(new SqlParameter("@bev_id", SqlDbType.Int)).Value = bev_id;
            scommand.Parameters.Add(new SqlParameter("@pnr_id", SqlDbType.Int)).Value = int.Parse(txPnrid.Text);
            scommand.Parameters.Add(new SqlParameter("@forg_id", SqlDbType.Int)).Value = forg_id;

            int ahb_id = 0;
            try
            {
                ahb_id = (int)scommand.ExecuteScalar();
            }
            catch (Exception ex)
            {
                //MessageBox.Show("SQL hiba: " + ex.Message);
                TraceBejegyzes(ex.Message);
            }
            if (ahb_id == 0) { MessageBox.Show("A rekord mentése nem sikerült!"); return 1; }
            else
                return 0;
        }

        private void Megoldas(int hibaID, int forg_id)
        {
            scommand = new SqlCommand("spHibaMegoldas", sconn);
            scommand.CommandType = CommandType.StoredProcedure;
            scommand.Parameters.Add(new SqlParameter("@hiba_id", SqlDbType.Int)).Value = hibaID;
            scommand.Parameters.Add(new SqlParameter("@bev_id", SqlDbType.Int)).Value = bev_id;
            scommand.Parameters.Add(new SqlParameter("@forg_id", SqlDbType.Int)).Value = forg_id;

            //int ahb_id = 0;
            try
            {
                scommand.ExecuteScalar();
            }
            catch (Exception ex)
            {
                //MessageBox.Show("SQL hiba: " + ex.Message);
                TraceBejegyzes(ex.Message);
            }
        }

        private int FejEllenorzesek()
        {
            // számszaki ellenőrzések
            int succ = 0;
            if (txTagokSum.Value != int.Parse(txTagokSumC.Text)) { MessageBox.Show("Tagok száma összesen nem egyezik!"); succ += HibaBeszurasa(7100003); Fejhiba = true; }
            else { Megoldas(7100003, 0); }
            if (txSajatSum.Value != int.Parse(txSajatSumC.Text)) { MessageBox.Show("Saját tagi befizetés összesen nem egyezik!"); succ += HibaBeszurasa(7100035); Fejhiba = true; }
            else { Megoldas(7100035, 0); }
            if (txMunkSum.Value != int.Parse(txMunkSumC.Text)) { MessageBox.Show("Munkáltatói hozzájárulás összesen nem egyezik!"); succ += HibaBeszurasa(7100036); Fejhiba = true; }
            else { Megoldas(7100036, 0); }
            if (txRendszSum.Value != int.Parse(txRendszSumC.Text)) { MessageBox.Show("Rendszeres támogatás összesen nem egyezik!"); succ += HibaBeszurasa(7100037); Fejhiba = true; }
            else { Megoldas(7100037, 0); }
            if (TxEgyszeriSum.Value != int.Parse(TxEgyszeriSumC.Text)) { MessageBox.Show("Egyszeri támogatás összesen nem egyezik!"); succ += HibaBeszurasa(7100038); Fejhiba = true; }
            else { Megoldas(7100038, 0); }
            if (TxTotal.Value != int.Parse(TxTotalC.Text)) { MessageBox.Show("Mindösszesen nem egyezik!"); succ += HibaBeszurasa(7100039); Fejhiba = true; }
            else { Megoldas(7100039, 0); }
            
            // Vonatkozási időszak ellenőrzése
            scommand = new SqlCommand("spVonIdEll", sconn);
            scommand.CommandType = CommandType.StoredProcedure;
            scommand.Parameters.Add(new SqlParameter("@idk_id", SqlDbType.Int)).Value = idk_id;
            string idoszakErv = string.Empty;
            try
            {
                idoszakErv = scommand.ExecuteScalar().ToString();
            }
            catch (Exception ex)
            {
                TraceBejegyzes(ex.Message);
            }
            if (idoszakErv == "N") { MessageBox.Show("Vonatkozási időszak még nem érvényes!"); succ += HibaBeszurasa(7100040); Fejhiba = true; }

            if (succ != 0) { return 1; }

            if (Fejhiba)
            {
                txErv.Text = "N";
                txErv.BackColor = Color.IndianRed;
                // ervenyes Update
                string commstring = "update bevallasok set ervenyes='N' where bev_id=" + bev_id.ToString();
                scommand = new SqlCommand(commstring, sconn);
                try
                {
                    scommand.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    TraceBejegyzes(ex.Message);
                }
                // sorok érvénytelenítése
                //for (int i = 0; i < dgwForgalmak.RowCount; i++)
                //{
                //    dgwForgalmak.Rows[i].Cells[3].Value = "N";
                //}
                //fSorokFeltoltese();
            }
            else
            {
                txErv.Text = "I";
                txErv.BackColor = Color.LightGreen;
                try
                {
                    string sql = "update bevallasok set ervenyes='I' where bev_id=" + bev_id.ToString();
                    MessageBox.Show("updatelve");
                    scommand = new SqlCommand(sql, sconn);
                    scommand.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    TraceBejegyzes(ex.Message);
                }
            }
            return 0;
        }

        private void ForgSorEll(int sorszam)
        {
            int fsucc = 0;          // ez a változó tárolja, hogy a hiba beszúrás sikeres volt-e
            int SorOsszesen = 0;
            int SorSajat = 0;
            int SorMH = 0;
            int SorRendsz = 0;
            int SorEgysz = 0;
            int SorBefizetendo = 0;
            Sorhiba = false;

            int forg_id = int.Parse(dgwForgalmak.Rows[sorszam].Cells[0].Value.ToString());
            int tag_id = int.Parse(dgwForgalmak.Rows[sorszam].Cells[10].Value.ToString());

            // munkaviszony ellenőrzése és hiba beszúrása
            int tszs_id = 0;
            try
            {
                scommand = new SqlCommand("spTszsLekerd", sconn);
                scommand.CommandType = CommandType.StoredProcedure;
                scommand.Parameters.Add(new SqlParameter("@pnr_id", SqlDbType.Int)).Value = tag_id;
                tszs_id = int.Parse(scommand.ExecuteScalar().ToString());
            }
            catch (Exception ex)
            {
                MessageBox.Show("SQL Hiba " + ex.Message);
                return;
            }

            MvEll(tszs_id, forg_id);

            SorSajat = int.Parse(dgwForgalmak.Rows[sorszam].Cells[4].Value.ToString());
            SorMH = int.Parse(dgwForgalmak.Rows[sorszam].Cells[5].Value.ToString());
            SorRendsz = int.Parse(dgwForgalmak.Rows[sorszam].Cells[6].Value.ToString());
            SorEgysz = int.Parse(dgwForgalmak.Rows[sorszam].Cells[7].Value.ToString());
            SorBefizetendo = int.Parse(dgwForgalmak.Rows[sorszam].Cells[8].Value.ToString());
            SorOsszesen = SorSajat + SorMH + SorRendsz + SorEgysz;

            int hibaID = 7100002;               // Hibás befizetendő összeg!
            if (SorOsszesen != SorBefizetendo)
            {
                Sorhiba = true;
                fsucc = SorHibaBeszurasa(hibaID, forg_id);
            }
            else
            {
                // ellenőrizzük, hogy volt-e hiba. Ha volt, de már nem áll fent, akkor a megoldás dátumát be kell szúrni.
                // update analitika_hibak set megoldas_dat=getdate() where ahb_id=(select ahb_id from analitika_hibak where forg_id=@forg_id and hiba_id=@hiba_id and megoldas_dat is null)
                Megoldas(hibaID, forg_id);
            }
            
            // Már van erre az időszakra a fogl.-tól, nem stornózott bevallás!  7100027
            hibaID = 7100027;
            int count = 0;
            scommand = new SqlCommand("spForgSorVanE", sconn);
            scommand.CommandType = CommandType.StoredProcedure;
            scommand.Parameters.Add(new SqlParameter("@pnr_id", SqlDbType.Int)).Value = tag_id;
            scommand.Parameters.Add(new SqlParameter("@fogl_id", SqlDbType.Int)).Value = int.Parse(txPnrid.Text);
            scommand.Parameters.Add(new SqlParameter("@ev_ho", SqlDbType.Int)).Value = int.Parse(txVonIdoszak.Text);
            scommand.Parameters.Add(new SqlParameter("@bev_id", SqlDbType.Int)).Value = bev_id;
            try
            {
                count = int.Parse(scommand.ExecuteScalar().ToString());
            }
            catch (Exception ex)
            {
                MessageBox.Show("SQL Hiba " + ex.Message);
            }
            if (count > 0)
            {
                fsucc = SorHibaBeszurasa(hibaID, forg_id);
                Sorhiba = true;
            }
            else
            {
                Megoldas(hibaID, forg_id);
            }

            if (Sorhiba)
                dgwForgalmak.Rows[sorszam].Cells[3].Value = "N";
            else
                dgwForgalmak.Rows[sorszam].Cells[3].Value = "I";
        }

        private void MvEll(int tszs_id, int forg_id)
        {
            int count = 0;
            // FUNCTION futtatása
            scommand = new SqlCommand("SELECT dbo.FuncMvEll2(@tszs_id, @pnr_id, @ev_ho)", sconn);
            scommand.Parameters.Add(new SqlParameter("@tszs_id", SqlDbType.Int)).Value = tszs_id;
            scommand.Parameters.Add(new SqlParameter("@pnr_id", SqlDbType.Int)).Value = int.Parse(txPnrid.Text);
            scommand.Parameters.Add(new SqlParameter("@ev_ho", SqlDbType.Int)).Value = int.Parse(txVonIdoszak.Text);
            try
            {
                count = int.Parse(scommand.ExecuteScalar().ToString());
            }
            catch (Exception ex)
            {
                MessageBox.Show("SQL Hiba " + ex.Message);
            }
            // Az FV kiértékelése
            switch (count)
            {
                case 1: scommand = new SqlCommand("spMvInsert2", sconn);
                    scommand.CommandType = CommandType.StoredProcedure;
                    scommand.Parameters.Add(new SqlParameter("@tszs_id", SqlDbType.Int)).Value = tszs_id;
                    scommand.Parameters.Add(new SqlParameter("@pnr_id", SqlDbType.Int)).Value = int.Parse(txPnrid.Text);
                    scommand.Parameters.Add(new SqlParameter("@ev_ho", SqlDbType.Int)).Value = int.Parse(txVonIdoszak.Text);
                    scommand.Parameters.Add(new SqlParameter("@modosit_neve", SqlDbType.VarChar, 80)).Value = User.name;
                    try
                    {
                        scommand.ExecuteNonQuery();
                    }
                    catch
                    {
                        // üres sor
                    }
                    break;
                case 2: // MessageBox.Show("MV OK"); 
                    break;
                case 3: // MessageBox.Show("mv már van, de nem teljes hónap"); 
                    SorHibaBeszurasa(7100012, forg_id);
                    break;
                case 4: scommand = new SqlCommand("spMvUpdate2", sconn);
                    scommand.CommandType = CommandType.StoredProcedure;
                    scommand.Parameters.Add(new SqlParameter("@tszs_id", SqlDbType.Int)).Value = tszs_id;
                    scommand.Parameters.Add(new SqlParameter("@pnr_id", SqlDbType.Int)).Value = int.Parse(txPnrid.Text);
                    scommand.Parameters.Add(new SqlParameter("@ev_ho", SqlDbType.Int)).Value = int.Parse(txVonIdoszak.Text);
                    scommand.Parameters.Add(new SqlParameter("@modosit_neve", SqlDbType.VarChar, 80)).Value = User.name;
                    try
                    {
                        scommand.ExecuteNonQuery();
                    }
                    catch
                    {
                        // üres sor
                    }
                    //MessageBox.Show("A létező munkaviszony kibővítve a vonatkozási időszak végéig.");
                    SorHibaBeszurasa(7100012, forg_id);
                    break;
                case 5: scommand = new SqlCommand("spMvUpdate3", sconn);
                    scommand.CommandType = CommandType.StoredProcedure;
                    scommand.Parameters.Add(new SqlParameter("@tszs_id", SqlDbType.Int)).Value = tszs_id;
                    scommand.Parameters.Add(new SqlParameter("@pnr_id", SqlDbType.Int)).Value = int.Parse(txPnrid.Text);
                    scommand.Parameters.Add(new SqlParameter("@ev_ho", SqlDbType.Int)).Value = int.Parse(txVonIdoszak.Text);
                    scommand.Parameters.Add(new SqlParameter("@modosit_neve", SqlDbType.VarChar, 80)).Value = User.name;
                    try
                    {
                        scommand.ExecuteNonQuery();
                    }
                    catch
                    {
                        // üres sor
                    }
                    SorHibaBeszurasa(7100012, forg_id);
                    break;
                default: break;
            }
        }

        public override void save()
        {
            IdoszakBeallitasa();
            Fejhiba = false;
            // Fejrész ellenőrzések
            if (txVonIdoszak.Text == string.Empty) { MessageBox.Show("Vonatkozási időszak nem lehet üres!"); txVonIdoszak.Focus(); return; }
            if (txErkSorsz.Text == string.Empty) { MessageBox.Show("Érkezés sorszáma nem lehet üres!"); txErkSorsz.Focus(); return; }
            if (txErkDate.Text == string.Empty) { MessageBox.Show("Érkezés dátuma nem lehet üres!"); txErkDate.Focus(); return; }
            if (txIktatoszam.Text == string.Empty) { MessageBox.Show("Iktatószám nem lehet üres!"); txIktatoszam.Focus(); return; }
            if (txAdkozlDate.Text == string.Empty) { MessageBox.Show("Adatközlés dátuma nem lehet üres!"); txAdkozlDate.Focus(); return; }
            if (txIktKelte.Text == string.Empty) { MessageBox.Show("Iktatás kelte nem lehet üres!"); txIktKelte.Focus(); return; }
            
            // mentés
            // Tranzakció id ellenőrzése, ha még 0, akkor tranzakció indítása
            if (TranID == 0)
            {
                TranID = int.Parse(txPnrid.Text);
                TransactionBegin();                // BEGIN TRAN
            }

            if (InsertTrueUpdateFalse)
            {
                bev_id = 0;
                // INSERT                
                scommand = new SqlCommand("spBevallasokInsert", sconn);
                scommand.CommandType = CommandType.StoredProcedure;
                scommand.Parameters.Add(new SqlParameter("@id", SqlDbType.Int)).Value = 1;
                scommand.Parameters.Add(new SqlParameter("@pnr_id", SqlDbType.Int)).Value = int.Parse(txPnrid.Text);
                scommand.Parameters.Add(new SqlParameter("@iktatoszam", SqlDbType.Int)).Value = int.Parse(txIktatoszam.Text);
                scommand.Parameters.Add(new SqlParameter("@erkez_sorszam", SqlDbType.Int)).Value = int.Parse(txErkSorsz.Text);
                scommand.Parameters.Add(new SqlParameter("@adatkozles_datum", SqlDbType.Date)).Value = DateTime.Parse(txAdkozlDate.Text).ToShortDateString();
                scommand.Parameters.Add(new SqlParameter("@iktatas_kelte", SqlDbType.Date)).Value = DateTime.Parse(txIktKelte.Text).ToShortDateString();
                scommand.Parameters.Add(new SqlParameter("@erkez_datum", SqlDbType.Date)).Value = DateTime.Parse(txErkDate.Text).ToShortDateString();
                scommand.Parameters.Add(new SqlParameter("@ervenyes", SqlDbType.VarChar, 1)).Value = txErv.Text;
                scommand.Parameters.Add(new SqlParameter("@idk_id", SqlDbType.Int)).Value = idk_id;
                scommand.Parameters.Add(new SqlParameter("@megjegyzes", SqlDbType.VarChar, 255)).Value = txMegjegyzes.Text;
                scommand.Parameters.Add(new SqlParameter("@tagok_osszesen", SqlDbType.Int)).Value = txTagokSum.Value;
                scommand.Parameters.Add(new SqlParameter("@sajat_ossz", SqlDbType.Int)).Value = txSajatSum.Value;
                scommand.Parameters.Add(new SqlParameter("@hozzajarulas_ossz", SqlDbType.Int)).Value = txMunkSum.Value;
                scommand.Parameters.Add(new SqlParameter("@rend_tamog_ossz", SqlDbType.Int)).Value = txRendszSum.Value;
                scommand.Parameters.Add(new SqlParameter("@egysz_tamog_ossz", SqlDbType.Int)).Value = TxEgyszeriSum.Value;
                scommand.Parameters.Add(new SqlParameter("@mindosszesen", SqlDbType.Int)).Value = TxTotal.Value;
                scommand.Parameters.Add(new SqlParameter("@modosit_neve", SqlDbType.VarChar, 80)).Value = User.name;

                try
                {
                    bev_id = int.Parse(scommand.ExecuteScalar().ToString());
                }
                catch (Exception ex)
                {
                    MessageBox.Show("SQL hiba: " + ex.Message);
                    TraceBejegyzes(ex.Message);
                }
                MessageBox.Show(bev_id.ToString());
                if (bev_id == 0) { MessageBox.Show("A rekord mentése nem sikerült!"); return; }

                // Ellenőrzések és a hibák beszúrása
                FejEllenorzesek();

                // forgalom sorok mentése
                int forg_id = 0;
                for (int i = 0; i < dgwForgalmak.RowCount; i++)
                {
                    forg_id = (int)ForgalmakInsert(i);
                    if (forg_id == 0)
                    {
                        MessageBox.Show("A rekord mentése nem sikerült!");
                        // tranzakció visszagörgetés
                        if (TranID != 0)
                        {
                            TransactionRollback();
                        }
                        return;
                    }
                    dgwForgalmak.Rows[i].Cells[0].Value = forg_id.ToString();
                    dgwForgalmak.Refresh();
                    // sorok ellenőrzése
                    ForgSorEll(i);
                }
            }
            else
            {
                // UPDATE
                int newBevId = bev_id;
                int idOK = BevallasokUpdate(newBevId);
                if (idOK == 0) { MessageBox.Show("A rekord mentése nem sikerült!"); return; }

                // Ellenőrzések és a hibák beszúrása
                FejEllenorzesek();

                // forgalom sorok mentése
                if (bev_id == 0) { MessageBox.Show("A rekord mentése nem sikerült!"); return; }
                for (int i = 0; i < dgwForgalmak.RowCount; i++)
                {
                    // Ha van forg id, akkor nem új sor, ha nincs, akkor új sor.
                    if (int.Parse(dgwForgalmak.Rows[i].Cells[0].Value.ToString()) > 0)
                    {
                        // sorok ellenőrzése
                        ForgSorEll(i);

                        idOK = (int)ForgalmakUpdate(i);
                        if (idOK == 0)
                        {
                            MessageBox.Show("A rekord mentése nem sikerült!");
                            // tranzakció visszagörgetés
                            if (TranID != 0)
                            {
                                TransactionRollback();
                            }
                            return;
                        }
                    }
                    else
                    {
                        // sorok ellenőrzése
                        ForgSorEll(i);

                        int forg_id = 0;
                        forg_id = (int)ForgalmakInsert(i);
                        if (forg_id == 0)
                        {
                            MessageBox.Show("A rekord mentése nem sikerült!");
                            // tranzakció visszagörgetés
                            if (TranID != 0)
                            {
                                TransactionRollback();
                            }
                            return;
                        }
                        dgwForgalmak.Rows[i].Cells[0].Value = forg_id.ToString();
                    }
                }
            }

            // ha minden sor érvénytelen a fej is érvénytelen
            bool nemerv = true;
            for (int i = 0; i < dgwForgalmak.RowCount; i++)
            {
                if (dgwForgalmak.Rows[i].Cells[3].Value.ToString() == "I")
                {
                    nemerv = false;
                    break;
                }
            }
            if (nemerv)
            {
                txErv.Text = "N";
                try
                {
                    string sql = "update bevallasok set ervenyes='N' where bev_id=" + bev_id.ToString();
                    scommand = new SqlCommand(sql, sconn);
                    scommand.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("SQL hiba: " + ex.Message);
                    TraceBejegyzes(ex.Message);
                }
            }

            // tranzakció commit
            if (TranID != 0)
            {
                TransactionCommit();
            }

            fSorokFeltoltese();
            InsertTrueUpdateFalse = false;
            tsSave.Enabled = false;
        }

        private int BevallasokUpdate(int id)
        {
            scommand = new SqlCommand("spBevallasokUpdate", sconn);
            scommand.CommandType = CommandType.StoredProcedure;
            scommand.Parameters.Add(new SqlParameter("@id", SqlDbType.Int)).Value = id;
            scommand.Parameters.Add(new SqlParameter("@pnr_id", SqlDbType.Int)).Value = int.Parse(txPnrid.Text);
            scommand.Parameters.Add(new SqlParameter("@iktatoszam", SqlDbType.Int)).Value = int.Parse(txIktatoszam.Text);
            scommand.Parameters.Add(new SqlParameter("@erkez_sorszam", SqlDbType.Int)).Value = int.Parse(txErkSorsz.Text);
            scommand.Parameters.Add(new SqlParameter("@adatkozles_datum", SqlDbType.Date)).Value = DateTime.Parse(txAdkozlDate.Text).ToShortDateString();
            scommand.Parameters.Add(new SqlParameter("@iktatas_kelte", SqlDbType.Date)).Value = DateTime.Parse(txIktKelte.Text).ToShortDateString();
            scommand.Parameters.Add(new SqlParameter("@erkez_datum", SqlDbType.Date)).Value = DateTime.Parse(txErkDate.Text).ToShortDateString();
            scommand.Parameters.Add(new SqlParameter("@ervenyes", SqlDbType.VarChar, 1)).Value = txErv.Text;
            scommand.Parameters.Add(new SqlParameter("@idk_id", SqlDbType.Int)).Value = idk_id;
            scommand.Parameters.Add(new SqlParameter("@megjegyzes", SqlDbType.VarChar, 255)).Value = txMegjegyzes.Text;
            scommand.Parameters.Add(new SqlParameter("@tagok_osszesen", SqlDbType.Int)).Value = txTagokSum.Value;
            scommand.Parameters.Add(new SqlParameter("@sajat_ossz", SqlDbType.Int)).Value = txSajatSum.Value;
            scommand.Parameters.Add(new SqlParameter("@hozzajarulas_ossz", SqlDbType.Int)).Value = txMunkSum.Value;
            scommand.Parameters.Add(new SqlParameter("@rend_tamog_ossz", SqlDbType.Int)).Value = txRendszSum.Value;
            scommand.Parameters.Add(new SqlParameter("@egysz_tamog_ossz", SqlDbType.Int)).Value = TxEgyszeriSum.Value;
            scommand.Parameters.Add(new SqlParameter("@mindosszesen", SqlDbType.Int)).Value = TxTotal.Value;
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
                return 0;
            }
            return 1;
        }

        private int ForgalmakInsert(int row)
        {
            int fid = 0;
            scommand = new SqlCommand("spForgalmakInsert", sconn);
            scommand.CommandType = CommandType.StoredProcedure;
            scommand.Parameters.Add(new SqlParameter("@id", SqlDbType.Int)).Value = 1;
            scommand.Parameters.Add(new SqlParameter("@idk_id", SqlDbType.Int)).Value = idk_id;
            scommand.Parameters.Add(new SqlParameter("@bev_id", SqlDbType.Int)).Value = bev_id;
            scommand.Parameters.Add(new SqlParameter("@pnr_id", SqlDbType.Int)).Value = int.Parse(dgwForgalmak.Rows[row].Cells[10].Value.ToString());
            scommand.Parameters.Add(new SqlParameter("@ervenyes", SqlDbType.VarChar, 1)).Value = dgwForgalmak.Rows[row].Cells[3].Value.ToString();
            scommand.Parameters.Add(new SqlParameter("@sajat", SqlDbType.Int)).Value = int.Parse(dgwForgalmak.Rows[row].Cells[4].Value.ToString());
            scommand.Parameters.Add(new SqlParameter("@hozzajarulas", SqlDbType.Int)).Value = int.Parse(dgwForgalmak.Rows[row].Cells[5].Value.ToString());
            scommand.Parameters.Add(new SqlParameter("@rend_tamog", SqlDbType.Int)).Value = int.Parse(dgwForgalmak.Rows[row].Cells[6].Value.ToString());
            scommand.Parameters.Add(new SqlParameter("@egysz_tamog", SqlDbType.Int)).Value = int.Parse(dgwForgalmak.Rows[row].Cells[7].Value.ToString());
            scommand.Parameters.Add(new SqlParameter("@befizetendo", SqlDbType.Int)).Value = int.Parse(dgwForgalmak.Rows[row].Cells[8].Value.ToString());
            //scommand.Parameters.Add(new SqlParameter("@storno", SqlDbType.VarChar, 1)).Value = int.Parse(dgwForgalmak.Rows[row].Cells[9].Value.ToString());
            scommand.Parameters.Add(new SqlParameter("@modosit_neve", SqlDbType.VarChar, 80)).Value = User.name;

            try
            {
                if (sconn.State == ConnectionState.Closed) sconn.Open();
                fid = Int32.Parse(scommand.ExecuteScalar().ToString());
            }
            catch (Exception ex)
            {
                MessageBox.Show("SQL Hiba " + ex.Message);
                TraceBejegyzes(ex.Message);
                return 0;
            }
            return (int)fid;
        }

        private int ForgalmakUpdate(int row)
        {
            scommand = new SqlCommand("spForgalmakUpdate", sconn);
            scommand.CommandType = CommandType.StoredProcedure;
            scommand.Parameters.Add(new SqlParameter("@id", SqlDbType.Int)).Value = int.Parse(dgwForgalmak.Rows[row].Cells[0].Value.ToString());
            scommand.Parameters.Add(new SqlParameter("@idk_id", SqlDbType.Int)).Value = idk_id;
            scommand.Parameters.Add(new SqlParameter("@bev_id", SqlDbType.Int)).Value = bev_id;
            scommand.Parameters.Add(new SqlParameter("@pnr_id", SqlDbType.Int)).Value = int.Parse(dgwForgalmak.Rows[row].Cells[10].Value.ToString());
            scommand.Parameters.Add(new SqlParameter("@ervenyes", SqlDbType.VarChar, 1)).Value = dgwForgalmak.Rows[row].Cells[3].Value.ToString();
            scommand.Parameters.Add(new SqlParameter("@sajat", SqlDbType.Int)).Value = int.Parse(dgwForgalmak.Rows[row].Cells[4].Value.ToString());
            scommand.Parameters.Add(new SqlParameter("@hozzajarulas", SqlDbType.Int)).Value = int.Parse(dgwForgalmak.Rows[row].Cells[5].Value.ToString());
            scommand.Parameters.Add(new SqlParameter("@rend_tamog", SqlDbType.Int)).Value = int.Parse(dgwForgalmak.Rows[row].Cells[6].Value.ToString());
            scommand.Parameters.Add(new SqlParameter("@egysz_tamog", SqlDbType.Int)).Value = int.Parse(dgwForgalmak.Rows[row].Cells[7].Value.ToString());
            scommand.Parameters.Add(new SqlParameter("@befizetendo", SqlDbType.Int)).Value = int.Parse(dgwForgalmak.Rows[row].Cells[8].Value.ToString());
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
                return 0;
            }
            return 1;
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
            nextRow1();
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
            nextRow2();
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
            nextRow3();
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
            nextRow4();
        }

        public void deleteRecord(string sorszam)
        {
            DialogResult dr = MessageBox.Show("Biztos törli a kijelölt rekordot? ",
                "Törlés?", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dr == DialogResult.Yes)
            {
                if (txKonyvelt.Text != string.Empty)
                {
                    MessageBox.Show("A bevallás könyvelt, nem törölhető!");
                }
                else
                {
                    int index = int.Parse(sorszam) - 1;
                    // ha grid sorok száma > index
                    if (dgwForgalmak.RowCount > index)
                    {
                        dgwForgalmak.Rows.RemoveAt(index);
                    }
                    sorokKiurit();
                    fSorokFeltoltese();
                }
            }
        }

        public void deleteAll()
        {
            DialogResult dr = MessageBox.Show("Biztos törli a bevallást? ",
                "Törlés?", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dr == DialogResult.Yes)
            {
                if (txKonyvelt.Text != string.Empty)
                {
                    MessageBox.Show("A bevallás könyvelt, nem törölhető!");
                }
                else
                {
                    scommand = new SqlCommand("spBevallasokDelete", sconn);
                    scommand.CommandType = CommandType.StoredProcedure;
                    if (bev_id != 0)
                    {
                        scommand.Parameters.Add(new SqlParameter("@id", SqlDbType.Int)).Value = bev_id;
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
                    createNew();
                }
            }
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

        private void BevRogz_Resize(object sender, EventArgs e)
        {
        }

        private void TagEll(int index)
        {
            // tag ellenőrzése
            long adoazon = 0;
            try
            {
                switch (index)
                {
                    case 0: adoazon = long.Parse(txAD0.Text);
                        break;
                    case 1: adoazon = long.Parse(txAD1.Text);
                        break;
                    case 2: adoazon = long.Parse(txAD2.Text);
                        break;
                    case 3: adoazon = long.Parse(txAD3.Text);
                        break;
                    case 4: adoazon = long.Parse(txAD4.Text);
                        break;
                    default: break;
                }
            }
            catch
            {
                MessageBox.Show("Hibás adat!");
                return;
            }
            if (adoazon != 0)
            {
                //SqlDataReader myReader = null;
                scommand = new SqlCommand("spTagEll", sconn);
                scommand.CommandType = CommandType.StoredProcedure;
                scommand.Parameters.Add(new SqlParameter("@adoazonosito_jel", SqlDbType.BigInt)).Value = adoazon;
                try
                {
                    if (sconn.State == ConnectionState.Closed) sconn.Open();
                    SqlDataReader myReader = scommand.ExecuteReader();
                    int db = 0;
                    while (myReader.Read())
                    {
                        IDataRecord record = (IDataRecord)myReader;
                        switch (index)
                        {
                            case 0:
                                txPnrid0.Text = record[0].ToString();  // pnr_id
                                txNev0.Text = record[1].ToString();    // név
                                db++;
                                break;
                            case 1: 
                                txPnrid1.Text = record[0].ToString();  // pnr_id
                                txNev1.Text = record[1].ToString();    // név
                                db++;
                                break;
                            case 2:
                                txPnrid2.Text = record[0].ToString();  // pnr_id
                                txNev2.Text = record[1].ToString();    // név
                                db++;
                                break;
                            case 3:
                                txPnrid3.Text = record[0].ToString();  // pnr_id
                                txNev3.Text = record[1].ToString();    // név
                                db++;
                                break;
                            case 4:
                                txPnrid4.Text = record[0].ToString();  // pnr_id
                                txNev4.Text = record[1].ToString();    // név
                                db++;
                                break;
                            default: break;
                        }
                    }
                    if (db == 0)
                    {
                        MessageBox.Show("Ezen az adószámon nem található tag!");
                        myReader.Close();
                        return;
                    }
                    else
                    {
                        myReader.Close();
                        SumTagok();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("SQL Hiba " + ex.Message);
                    TraceBejegyzes(ex.Message);
                }
            }
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

        private void dgwForgalmak_KeyPress(object sender, KeyPressEventArgs e)
        {

        }

        // numerikus mezők kezelése
        private void txPnrid_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Char.IsDigit(e.KeyChar) && !Char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void txVonIdoszak_KeyPress(object sender, KeyPressEventArgs e)
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

        private void txIktatoszam_KeyPress(object sender, KeyPressEventArgs e)
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

        private void txErkSorsz_KeyPress(object sender, KeyPressEventArgs e)
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

        private void IdoszakBeallitasa()
        {
            if (txVonIdoszak.TextLength == 6)
            {
                Evho = int.Parse(txVonIdoszak.Text);
                scommand = new SqlCommand("spIdoszak1", sconn);
                scommand.CommandType = CommandType.StoredProcedure;
                scommand.Parameters.Add(new SqlParameter("@ev_ho", SqlDbType.Int)).Value = Evho;
                try
                {
                    if (sconn.State == ConnectionState.Closed) sconn.Open();
                    string idkid2 = scommand.ExecuteScalar().ToString();
                    idk_id = int.Parse(idkid2);
                }
                catch
                {
                    MessageBox.Show("Hibás időszak!");
                    txVonIdoszak.Focus();
                }
            }
            else if (txVonIdoszak.TextLength != 0)
            {
                MessageBox.Show("Hibás időszak!");
                txVonIdoszak.Focus();
            }
        }

        private void txVonIdoszak_Leave(object sender, EventArgs e)
        {
            IdoszakBeallitasa();
        }

        #region DÁTUM MEZŐK KEZELÉSE

        // az érkezés dátuma kezelése
        private void txErkDate_KeyDown(object sender, KeyEventArgs e)
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

        private void txErkDate_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (txErkDate.SelectionLength == 0)
            {
                if (Char.IsDigit(e.KeyChar))
                {
                    if (txErkDate.TextLength >= 8) e.Handled = true;
                }
                else if (enter)
                {
                    DateTransformErkDate();
                }
                else if (back)
                {
                    if (txErkDate.TextLength == 11)
                    {
                        string text;
                        text = txErkDate.Text.Substring(0, 4) + txErkDate.Text.Substring(5, 2) + txErkDate.Text.Substring(8, 3);
                        txErkDate.Text = text;
                        txErkDate.Select(txErkDate.Text.Length, 0);
                    }
                }
                else
                    e.Handled = true;
            }
        }

        private void DateTransformErkDate()
        {
            if (txErkDate.TextLength == 8)
            {
                string text;
                text = txErkDate.Text.Substring(0, 4) + "." + txErkDate.Text.Substring(4, 2) + "." + txErkDate.Text.Substring(6, 2);
                try
                {
                    txErkDate.Text = DateTime.Parse(text).ToShortDateString();
                }
                catch
                {
                    MessageBox.Show("Hibás dátum!");
                    txErkDate.Focus();
                }
            }
            else
            {
                if (txErkDate.TextLength != 11 && txErkDate.TextLength != 0)
                {
                    MessageBox.Show("Hibás dátum!");
                    txErkDate.Focus();
                }
            }
        }

        private void txErkDate_KeyUp(object sender, KeyEventArgs e)
        {
            back = false;
            enter = false;
        }

        private void txErkDate_Leave(object sender, EventArgs e)
        {
            DateTransformErkDate();
            back = false;
            enter = false;

            try
            {
                if (txErkDate.Text != string.Empty)
                {
                    if (DateTime.Parse(txErkDate.Text) > DateTime.Parse(txIktKelte.Text))
                    {
                        MessageBox.Show("Az érkezés dátuma nem lehet nagyobb az iktatás dátumánál!");
                        txErkDate.Focus();
                    }
                    else if (txAdkozlDate.Text != string.Empty)
                    {
                        if (DateTime.Parse(txErkDate.Text) < DateTime.Parse(txAdkozlDate.Text))
                        {
                            MessageBox.Show("Az érkezés dátuma nem lehet kisebb, mint az adatközlés dátuma!");
                            txErkDate.Focus();
                        }
                    }
                }
            }
            catch
            {
                txErkDate.Focus();
            }
        }

        // az adatközlés dátuma kezelése
        private void txAdkozlDate_KeyDown(object sender, KeyEventArgs e)
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

        private void txAdkozlDate_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (txAdkozlDate.SelectionLength == 0)
            {
                if (Char.IsDigit(e.KeyChar))
                {
                    if (txAdkozlDate.TextLength >= 8) e.Handled = true;
                }
                else if (enter)
                {
                    DateTransformAdatkozlDate();
                }
                else if (back)
                {
                    if (txAdkozlDate.TextLength == 11)
                    {
                        string text;
                        text = txAdkozlDate.Text.Substring(0, 4) + txAdkozlDate.Text.Substring(5, 2) + txAdkozlDate.Text.Substring(8, 3);
                        txAdkozlDate.Text = text;
                        txAdkozlDate.Select(txAdkozlDate.Text.Length, 0);
                    }
                }
                else
                    e.Handled = true;
            }
        }

        private void DateTransformAdatkozlDate()
        {
            if (txAdkozlDate.TextLength == 8)
            {
                string text;
                text = txAdkozlDate.Text.Substring(0, 4) + "." + txAdkozlDate.Text.Substring(4, 2) + "." + txAdkozlDate.Text.Substring(6, 2);
                try
                {
                    txAdkozlDate.Text = DateTime.Parse(text).ToShortDateString();
                }
                catch
                {
                    MessageBox.Show("Hibás dátum!");
                    txAdkozlDate.Focus();
                }
            }
            else
            {
                if (txAdkozlDate.TextLength != 11 && txAdkozlDate.TextLength != 0)
                {
                    MessageBox.Show("Hibás dátum!");
                    txAdkozlDate.Focus();
                }
            }
        }

        private void txAdkozlDate_KeyUp(object sender, KeyEventArgs e)
        {
            back = false;
            enter = false;
        }

        private void txAdkozlDate_Leave(object sender, EventArgs e)
        {
            DateTransformAdatkozlDate();
            back = false;
            enter = false;

            try
            {
                if (txAdkozlDate.Text != string.Empty)
                {
                    if (DateTime.Parse(txAdkozlDate.Text) > DateTime.Parse(txErkDate.Text))
                    {
                        MessageBox.Show("Az adatközlés dátuma nem lehet nagyobb az érkezés dátumánál!");
                        txAdkozlDate.Focus();
                    }
                }
            }
            catch
            {
                txAdkozlDate.Focus();
            }
        }

        // az iktatás kelte dátum kezelése
        private void txIktKelte_KeyDown(object sender, KeyEventArgs e)
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

        private void txIktKelte_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (txIktKelte.SelectionLength == 0)
            {
                if (Char.IsDigit(e.KeyChar))
                {
                    if (txIktKelte.TextLength >= 8) e.Handled = true;
                }
                else if (enter)
                {
                    DateTransformIktKelteDate();
                }
                else if (back)
                {
                    if (txIktKelte.TextLength == 11)
                    {
                        string text;
                        text = txIktKelte.Text.Substring(0, 4) + txIktKelte.Text.Substring(5, 2) + txIktKelte.Text.Substring(8, 3);
                        txIktKelte.Text = text;
                        txIktKelte.Select(txIktKelte.Text.Length, 0);
                    }
                }
                else
                    e.Handled = true;
            }
        }

        private void DateTransformIktKelteDate()
        {
            if (txIktKelte.TextLength == 8)
            {
                string text;
                text = txIktKelte.Text.Substring(0, 4) + "." + txIktKelte.Text.Substring(4, 2) + "." + txIktKelte.Text.Substring(6, 2);
                try
                {
                    txIktKelte.Text = DateTime.Parse(text).ToShortDateString();
                }
                catch
                {
                    MessageBox.Show("Hibás dátum!");
                    txIktKelte.Focus();
                }
            }
            else
            {
                if (txIktKelte.TextLength != 11 && txIktKelte.TextLength != 0)
                {
                    MessageBox.Show("Hibás dátum!");
                    txIktKelte.Focus();
                }
            }
        }

        private void txIktKelte_KeyUp(object sender, KeyEventArgs e)
        {
            back = false;
            enter = false;
        }

        private void txIktKelte_Leave(object sender, EventArgs e)
        {
            DateTransformIktKelteDate();
            back = false;
            enter = false;

            try
            {
                if (txIktKelte.Text != string.Empty)
                {
                    if (DateTime.Parse(txIktKelte.Text) > DateTime.Now)
                    {
                        MessageBox.Show("Az iktatás dátuma nem lehet nagyobb az aktuális dátumnál!");
                        txIktKelte.Focus();
                    }
                    else if (txErkDate.Text != string.Empty)
                    {
                        if (DateTime.Parse(txIktKelte.Text) < DateTime.Parse(txErkDate.Text))
                        {
                            MessageBox.Show("Az iktatás kelte nem lehet kisebb, mint az érkezés dátuma!");
                            txIktKelte.Focus();
                        }
                    }
                }
            }
            catch
            {
                txIktKelte.Focus();
            }
        }
        #endregion

        #region numericupdown mezők kezelése

        private void txTagokSum_Enter(object sender, EventArgs e)
        {
            txTagokSum.Select(0, txTagokSum.Text.Length);
        }

        private void txSajatSum_Enter(object sender, EventArgs e)
        {
            txSajatSum.Select(0, txSajatSum.Text.Length);
        }

        private void txMunkSum_Enter(object sender, EventArgs e)
        {
            txMunkSum.Select(0, txMunkSum.Text.Length);
        }

        private void txRendszSum_Enter(object sender, EventArgs e)
        {
            txRendszSum.Select(0, txRendszSum.Text.Length);
        }

        private void TxEgyszeriSum_Enter(object sender, EventArgs e)
        {
            TxEgyszeriSum.Select(0, TxEgyszeriSum.Text.Length);
        }

        private void TxTotal_Enter(object sender, EventArgs e)
        {
            TxTotal.Select(0, TxTotal.Text.Length);
        }
        #endregion

        private void txErkSorsz_Enter(object sender, EventArgs e)
        {
            txErkSorsz.Select(0, txErkSorsz.Text.Length);
        }

        private void txErkDate_Enter(object sender, EventArgs e)
        {
            txErkDate.Select(0, txErkDate.Text.Length);
        }

        private void txIktatoszam_Enter(object sender, EventArgs e)
        {
            txIktatoszam.Select(0, txIktatoszam.Text.Length);
        }

        private void txAdkozlDate_Enter(object sender, EventArgs e)
        {
            txAdkozlDate.Select(0, txAdkozlDate.Text.Length);
        }

        private void txIktKelte_Enter(object sender, EventArgs e)
        {
            txIktKelte.Select(0, txIktKelte.Text.Length);
        }

        private void TraceBejegyzes(string msg)
        {
            myListener.WriteLine("-" + DateTime.Now + ": " + msg);
            myListener.Flush();
            myListener.Close();
        }

        private void BevRogz_Activated(object sender, EventArgs e)
        {
            BevRogz.ActiveForm.Text = "Bevallás rögzítés - Fogl.: " + nev + " ID: " + Pnridtext;
        }

        #region RowVerification
        private void RowVerification(int index)
        {
            int tag = 0;
            int rn = 0;

            switch (index)
            {
                case 0:
                    tag = int.Parse(txPnrid0.Text);
                    rn = int.Parse(txSorsz0.Text);
                    break;
                case 1:
                    tag = int.Parse(txPnrid1.Text);
                    rn = int.Parse(txSorsz1.Text);
                    break;
                case 2:
                    tag = int.Parse(txPnrid2.Text);
                    rn = int.Parse(txSorsz2.Text);
                    break;
                case 3:
                    tag = int.Parse(txPnrid3.Text);
                    rn = int.Parse(txSorsz3.Text);
                    break;
                case 4:
                    tag = int.Parse(txPnrid4.Text);
                    rn = int.Parse(txSorsz4.Text);
                    break;
                default: break;
            }

            // Sor ellenőrzések
            Sorhiba = false;

            // ismétlődés
            // van-e már a tagnak ennél a foglalkoztatónál ugyanerre az időszakra bevallása
            // ha igen, a sor érvénytelen lesz

            int SorOsszesen = 0;
            int SorSajat = 0;
            int SorMH = 0;
            int SorRendsz = 0;
            int SorEgysz = 0;
            int SorBefizetendo = 0;
            switch (index)
            {
                case 0:
                    SorSajat = (txSaj0.Text == string.Empty ? 0 : int.Parse(txSaj0.Text));
                    SorMH = (txMh0.Text == string.Empty ? 0 : int.Parse(txMh0.Text));
                    SorRendsz = (txRend0.Text == string.Empty ? 0 : int.Parse(txRend0.Text));
                    SorEgysz = (txEgysz0.Text == string.Empty ? 0 : int.Parse(txEgysz0.Text));
                    SorBefizetendo = (txBef0.Text == string.Empty ? 0 : int.Parse(txBef0.Text));
                    break;
                case 1:
                    SorSajat = (txSaj1.Text == string.Empty ? 0 : int.Parse(txSaj1.Text));
                    SorMH = (txMh1.Text == string.Empty ? 0 : int.Parse(txMh1.Text));
                    SorRendsz = (txRend1.Text == string.Empty ? 0 : int.Parse(txRend1.Text));
                    SorEgysz = (txEgysz1.Text == string.Empty ? 0 : int.Parse(txEgysz1.Text));
                    SorBefizetendo = (txBef1.Text == string.Empty ? 0 : int.Parse(txBef1.Text));
                    break;
                case 2:
                    SorSajat = (txSaj2.Text == string.Empty ? 0 : int.Parse(txSaj2.Text));
                    SorMH = (txMh2.Text == string.Empty ? 0 : int.Parse(txMh2.Text));
                    SorRendsz = (txRend2.Text == string.Empty ? 0 : int.Parse(txRend2.Text));
                    SorEgysz = (txEgysz2.Text == string.Empty ? 0 : int.Parse(txEgysz2.Text));
                    SorBefizetendo = (txBef2.Text == string.Empty ? 0 : int.Parse(txBef2.Text));
                    break;
                case 3:
                    SorSajat = (txSaj3.Text == string.Empty ? 0 : int.Parse(txSaj3.Text));
                    SorMH = (txMh3.Text == string.Empty ? 0 : int.Parse(txMh3.Text));
                    SorRendsz = (txRend3.Text == string.Empty ? 0 : int.Parse(txRend3.Text));
                    SorEgysz = (txEgysz3.Text == string.Empty ? 0 : int.Parse(txEgysz3.Text));
                    SorBefizetendo = (txBef3.Text == string.Empty ? 0 : int.Parse(txBef3.Text));
                    break;
                case 4:
                    SorSajat = (txSaj4.Text == string.Empty ? 0 : int.Parse(txSaj4.Text));
                    SorMH = (txMh4.Text == string.Empty ? 0 : int.Parse(txMh4.Text));
                    SorRendsz = (txRend4.Text == string.Empty ? 0 : int.Parse(txRend4.Text));
                    SorEgysz = (txEgysz4.Text == string.Empty ? 0 : int.Parse(txEgysz4.Text));
                    SorBefizetendo = (txBef4.Text == string.Empty ? 0 : int.Parse(txBef4.Text));
                    break;
                default: break;
            }

            SorOsszesen = SorSajat + SorMH + SorRendsz + SorEgysz;
            if (SorOsszesen != SorBefizetendo)
            {
                MessageBox.Show("Hibás befizetendő összeg!");
                Sorhiba = true;
            }

            rn--;
            switch (index)
            {
                case 0:
                    if (Sorhiba)
                        txErv0.Text = "N";
                    else
                        txErv0.Text = "I";
                    dgwForgalmak.Rows[rn].Cells[3].Value = txErv0.Text;
                    break;
                case 1:
                    if (Sorhiba)
                        txErv1.Text = "N";
                    else
                        txErv1.Text = "I";
                    dgwForgalmak.Rows[rn].Cells[3].Value = txErv1.Text;
                    break;
                case 2:
                    if (Sorhiba)
                        txErv2.Text = "N";
                    else
                        txErv2.Text = "I";
                    dgwForgalmak.Rows[rn].Cells[3].Value = txErv2.Text;
                    break;
                case 3:
                    if (Sorhiba)
                        txErv3.Text = "N";
                    else
                        txErv3.Text = "I";
                    dgwForgalmak.Rows[rn].Cells[3].Value = txErv3.Text;
                    break;
                case 4:
                    if (Sorhiba)
                        txErv4.Text = "N";
                    else
                        txErv4.Text = "I";
                    dgwForgalmak.Rows[rn].Cells[3].Value = txErv4.Text;
                    break;
                default: break;
            }

            // sum tagok száma
            Tagokszama = 0;
            for (int i = 0; i < dgwForgalmak.RowCount; i++)
            {
                //if (int.Parse() > 9999)
                Tagokszama++;
            }
            txTagokSumC.Text = Tagokszama.ToString();
        } 
        #endregion

        private void txMegjegyzes_Leave(object sender, EventArgs e)
        {
            //txAD0.Focus();
        }

        // Tranzakciók kezelése
        private void TransactionBegin()
        {
            string Query;
            Query = "BEGIN TRAN BevRogz" + TranID.ToString();
            scommand = new SqlCommand(Query, sconn);
            try { scommand.ExecuteNonQuery(); }
            catch (Exception ex) { MessageBox.Show("SQL Hiba " + ex.Message); }
        }

        private void TransactionCommit()
        {
            string Query;
            Query = "COMMIT TRAN BevRogz" + TranID.ToString() + ";";
            scommand = new SqlCommand(Query, sconn);
            try { scommand.ExecuteNonQuery(); }
            catch (Exception ex) { MessageBox.Show("SQL Hiba " + ex.Message); TraceBejegyzes(ex.Message); }
            TranID = 0;
        }

        private void TransactionRollback()
        {
            string Query;
            Query = "ROLLBACK TRAN BevRogz" + TranID.ToString() + ";";
            scommand = new SqlCommand(Query, sconn);
            try { scommand.ExecuteNonQuery(); }
            catch (Exception ex) { MessageBox.Show("SQL Hiba " + ex.Message); TraceBejegyzes(ex.Message); }
            TranID = 0;
        }

        #region OSZLOPOK

        #region adószámok
        private void txAD0_Leave(object sender, EventArgs e)
        {
            if (txAD0.Text != string.Empty)
            { 
                try
                {
                    sorszam = 0;
                    txSorsz0.Text = (txSorsz0.Text == string.Empty ? (sorszam + 1).ToString() : txSorsz0.Text);
                    if (dgwForgalmak.RowCount < int.Parse(txSorsz0.Text))
                    {
                        TagEll(0);
                        if (txPnrid0.Text == string.Empty) { txAD0.Focus(); return; }       // ha nincs beazonosítva a tag akkor nem lépünk tovább
                        txErv0.Text = "I";
                        txErv0.BackColor = Color.LightGreen;
                        txSaj0.Text = txMh0.Text = txRend0.Text = txEgysz0.Text = txBef0.Text = txStorno0.Text = nullvalue;

                        dr = this.dt.NewRow();
                        dr["adoazonosito"] = txAD0.Text;
                        dr["nev"] = txNev0.Text;
                        dr["ervenyes"] = txErv0.Text;
                        dr["pnr_id"] = txPnrid0.Text;          // pnr_id
                        dr["sajat"] = nullvalue;
                        dr["hozzajarulas"] = nullvalue;
                        dr["rendszeres"] = nullvalue;
                        dr["egyszeri"] = nullvalue;
                        dr["befizetendo"] = nullvalue;
                        dr["storno"] = nullvalue;
                        dt.Rows.Add(dr);

                        sorszam = int.Parse(txSorsz0.Text) - 1;
                        dgwForgalmak.Rows[sorszam].Cells[3].Value = txErv0.Text;
                        dgwForgalmak.Rows[sorszam].Cells[9].Value = txStorno0.Text;
                    }
                    else
                    {
                        TagEll(0);
                        if (txPnrid0.Text == string.Empty) { txAD0.Focus(); return; }       // ha nincs beazonosítva a tag akkor nem lépünk tovább
                        sorszam = int.Parse(txSorsz0.Text) - 1;
                        //txErv0.Text = "I";
                        //sorszam = 0;
                        //txSorsz0.Text = (txSorsz0.Text == string.Empty ? (sorszam + 1).ToString() : txSorsz0.Text);
                    }
                    dgwForgalmak.Rows[sorszam].Cells[1].Value = txAD0.Text;
                    dgwForgalmak.Rows[sorszam].Cells[2].Value = txNev0.Text;
                    dgwForgalmak.Rows[sorszam].Cells[10].Value = txPnrid0.Text;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                if (Rowleave)
                {
                    RowVerification(0);
                    Rowleave = false;
                }
            }
        }

        private void txAD1_Leave(object sender, EventArgs e)
        {
            if (txAD1.Text != string.Empty)
            {
                try
                {
                    sorszam = int.Parse(txSorsz0.Text);
                    txSorsz1.Text = (txSorsz1.Text == string.Empty ? (sorszam + 1).ToString() : txSorsz1.Text);
                    if (dgwForgalmak.RowCount < int.Parse(txSorsz1.Text))
                    {
                        TagEll(1);
                        if (txPnrid1.Text == string.Empty) { txAD1.Focus(); return; }       // ha nincs beazonosítva a tag akkor nem lépünk tovább
                        txErv1.Text = "I";
                        txErv1.BackColor = Color.LightGreen;
                        txSaj1.Text = txMh1.Text = txRend1.Text = txEgysz1.Text = txBef1.Text = txStorno1.Text = nullvalue;

                        dr = this.dt.NewRow();
                        dr["adoazonosito"] = txAD1.Text;
                        dr["nev"] = txNev1.Text;
                        dr["ervenyes"] = txErv1.Text;
                        dr["pnr_id"] = txPnrid1.Text;          // pnr_id
                        dr["sajat"] = nullvalue;
                        dr["hozzajarulas"] = nullvalue;
                        dr["rendszeres"] = nullvalue;
                        dr["egyszeri"] = nullvalue;
                        dr["befizetendo"] = nullvalue;
                        dr["storno"] = nullvalue;
                        dt.Rows.Add(dr);

                        sorszam = int.Parse(txSorsz1.Text) - 1;
                        dgwForgalmak.Rows[sorszam].Cells[3].Value = txErv1.Text;
                        dgwForgalmak.Rows[sorszam].Cells[9].Value = txStorno1.Text;
                    }
                    else
                    {
                        TagEll(1);
                        if (txPnrid1.Text == string.Empty) { txAD1.Focus(); return; }       // ha nincs beazonosítva a tag akkor nem lépünk tovább
                        sorszam = int.Parse(txSorsz1.Text) - 1;
                    }
                    dgwForgalmak.Rows[sorszam].Cells[1].Value = txAD1.Text;
                    dgwForgalmak.Rows[sorszam].Cells[2].Value = txNev1.Text;
                    dgwForgalmak.Rows[sorszam].Cells[10].Value = txPnrid1.Text;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                if (Rowleave)
                {
                    RowVerification(1);
                    Rowleave = false;
                }
            }
        }

        private void txAD2_Leave(object sender, EventArgs e)
        {
            if (txAD2.Text != string.Empty)
            {
                try
                {
                    sorszam = int.Parse(txSorsz1.Text);
                    txSorsz2.Text = (txSorsz2.Text == string.Empty ? (sorszam + 1).ToString() : txSorsz2.Text);
                    if (dgwForgalmak.RowCount < int.Parse(txSorsz2.Text))
                    {
                        TagEll(2);
                        if (txPnrid2.Text == string.Empty) { txAD2.Focus(); return; }       // ha nincs beazonosítva a tag akkor nem lépünk tovább
                        txErv2.Text = "I";
                        txErv2.BackColor = Color.LightGreen;
                        txSaj2.Text = txMh2.Text = txRend2.Text = txEgysz2.Text = txBef2.Text = txStorno2.Text = nullvalue;

                        dr = this.dt.NewRow();
                        dr["adoazonosito"] = txAD2.Text;
                        dr["nev"] = txNev2.Text;
                        dr["ervenyes"] = txErv2.Text;
                        dr["pnr_id"] = txPnrid2.Text;          // pnr_id
                        dr["sajat"] = nullvalue;
                        dr["hozzajarulas"] = nullvalue;
                        dr["rendszeres"] = nullvalue;
                        dr["egyszeri"] = nullvalue;
                        dr["befizetendo"] = nullvalue;
                        dr["storno"] = nullvalue;
                        dt.Rows.Add(dr);

                        sorszam = int.Parse(txSorsz2.Text) - 1;
                        dgwForgalmak.Rows[sorszam].Cells[3].Value = txErv2.Text;
                        dgwForgalmak.Rows[sorszam].Cells[9].Value = txStorno2.Text;
                    }
                    else
                    {
                        TagEll(2);
                        if (txPnrid2.Text == string.Empty) { txAD2.Focus(); return; }       // ha nincs beazonosítva a tag akkor nem lépünk tovább
                        sorszam = int.Parse(txSorsz2.Text) - 1;
                    }
                    dgwForgalmak.Rows[sorszam].Cells[1].Value = txAD2.Text;
                    dgwForgalmak.Rows[sorszam].Cells[2].Value = txNev2.Text;
                    dgwForgalmak.Rows[sorszam].Cells[10].Value = txPnrid2.Text;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                if (Rowleave)
                {
                    RowVerification(2);
                    Rowleave = false;
                }
            }
        }

        private void txAD3_Leave(object sender, EventArgs e)
        {
            if (txAD3.Text != string.Empty)
            {
                try
                {
                    sorszam = int.Parse(txSorsz2.Text);
                    txSorsz3.Text = (txSorsz3.Text == string.Empty ? (sorszam + 1).ToString() : txSorsz3.Text);
                    if (dgwForgalmak.RowCount < int.Parse(txSorsz3.Text))
                    {
                        TagEll(3);
                        if (txPnrid3.Text == string.Empty) { txAD3.Focus(); return; }       // ha nincs beazonosítva a tag akkor nem lépünk tovább
                        txErv3.Text = "I";
                        txErv3.BackColor = Color.LightGreen;
                        txSaj3.Text = txMh3.Text = txRend3.Text = txEgysz3.Text = txBef3.Text = txStorno3.Text = nullvalue;

                        dr = this.dt.NewRow();
                        dr["adoazonosito"] = txAD3.Text;
                        dr["nev"] = txNev3.Text;
                        dr["ervenyes"] = txErv3.Text;
                        dr["pnr_id"] = txPnrid3.Text;          // pnr_id
                        dr["sajat"] = nullvalue;
                        dr["hozzajarulas"] = nullvalue;
                        dr["rendszeres"] = nullvalue;
                        dr["egyszeri"] = nullvalue;
                        dr["befizetendo"] = nullvalue;
                        dr["storno"] = nullvalue;
                        dt.Rows.Add(dr);

                        sorszam = int.Parse(txSorsz3.Text) - 1;
                        dgwForgalmak.Rows[sorszam].Cells[3].Value = txErv3.Text;
                        dgwForgalmak.Rows[sorszam].Cells[9].Value = txStorno3.Text;
                    }
                    else
                    {
                        TagEll(3);
                        if (txPnrid3.Text == string.Empty) { txAD3.Focus(); return; }       // ha nincs beazonosítva a tag akkor nem lépünk tovább
                        sorszam = int.Parse(txSorsz3.Text) - 1;
                    }
                    dgwForgalmak.Rows[sorszam].Cells[1].Value = txAD3.Text;
                    dgwForgalmak.Rows[sorszam].Cells[2].Value = txNev3.Text;
                    dgwForgalmak.Rows[sorszam].Cells[10].Value = txPnrid3.Text;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                if (Rowleave)
                {
                    RowVerification(3);
                    Rowleave = false;
                }
            }
        }

        private void txAD4_Leave(object sender, EventArgs e)
        {
            if (txAD4.Text != string.Empty)
            {
                try
                {
                    sorszam = int.Parse(txSorsz3.Text);
                    txSorsz4.Text = (txSorsz4.Text == string.Empty ? (sorszam + 1).ToString() : txSorsz4.Text);
                    if (dgwForgalmak.RowCount < int.Parse(txSorsz4.Text))
                    {
                        TagEll(4);
                        if (txPnrid4.Text == string.Empty) { txAD4.Focus(); return; }       // ha nincs beazonosítva a tag akkor nem lépünk tovább
                        txErv4.Text = "I";
                        txErv4.BackColor = Color.LightGreen;
                        txSaj4.Text = txMh4.Text = txRend4.Text = txEgysz4.Text = txBef4.Text = txStorno4.Text = nullvalue;

                        dr = this.dt.NewRow();
                        dr["adoazonosito"] = txAD4.Text;
                        dr["nev"] = txNev4.Text;
                        dr["ervenyes"] = txErv4.Text;
                        dr["pnr_id"] = txPnrid4.Text;          // pnr_id
                        dr["sajat"] = nullvalue;
                        dr["hozzajarulas"] = nullvalue;
                        dr["rendszeres"] = nullvalue;
                        dr["egyszeri"] = nullvalue;
                        dr["befizetendo"] = nullvalue;
                        dr["storno"] = nullvalue;
                        dt.Rows.Add(dr);

                        sorszam = int.Parse(txSorsz4.Text) - 1;
                        dgwForgalmak.Rows[sorszam].Cells[3].Value = txErv4.Text;
                        dgwForgalmak.Rows[sorszam].Cells[9].Value = txStorno4.Text;
                    }
                    else
                    {
                        TagEll(4);
                        if (txPnrid4.Text == string.Empty) { txAD4.Focus(); return; }       // ha nincs beazonosítva a tag akkor nem lépünk tovább
                        sorszam = int.Parse(txSorsz4.Text) - 1;
                    }
                    dgwForgalmak.Rows[sorszam].Cells[1].Value = txAD4.Text;
                    dgwForgalmak.Rows[sorszam].Cells[2].Value = txNev4.Text;
                    dgwForgalmak.Rows[sorszam].Cells[10].Value = txPnrid4.Text;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                if (Rowleave)
                {
                    RowVerification(4);
                    Rowleave = false;
                }
            }
        }
        #endregion adószámok

        # region saját
        private void txSaj0_Leave(object sender, EventArgs e)
        {
            if (txAD0.Text != string.Empty)
            {
                int sor = int.Parse(txSorsz0.Text);
                if (txSaj0.Text != string.Empty)
                    dgwForgalmak.Rows[sor - 1].Cells[4].Value = txSaj0.Text;
                else
                    dgwForgalmak.Rows[sor - 1].Cells[4].Value = "0";
                SajatOsszesit();
                if (Rowleave)
                {
                    RowVerification(0);
                    Rowleave = false;
                }
            }
        }

        private void txSaj1_Leave(object sender, EventArgs e)
        {
            if (txAD1.Text != string.Empty)
            {
                int sor = int.Parse(txSorsz1.Text);
                if (txSaj1.Text != string.Empty)
                    dgwForgalmak.Rows[sor - 1].Cells[4].Value = txSaj1.Text;
                else
                    dgwForgalmak.Rows[sor - 1].Cells[4].Value = "0";
                SajatOsszesit();
                if (Rowleave)
                {
                    RowVerification(1);
                    Rowleave = false;
                }
            }
        }

        private void txSaj2_Leave(object sender, EventArgs e)
        {
            if (txAD2.Text != string.Empty)
            {
                int sor = int.Parse(txSorsz2.Text);
                if (txSaj2.Text != string.Empty)
                    dgwForgalmak.Rows[sor - 1].Cells[4].Value = txSaj2.Text;
                else
                    dgwForgalmak.Rows[sor - 1].Cells[4].Value = "0";
                SajatOsszesit();
                if (Rowleave)
                {
                    RowVerification(2);
                    Rowleave = false;
                }
            }
        }

        private void txSaj3_Leave(object sender, EventArgs e)
        {
            if (txAD3.Text != string.Empty)
            {
                int sor = int.Parse(txSorsz3.Text);
                if (txSaj3.Text != string.Empty)
                    dgwForgalmak.Rows[sor - 1].Cells[4].Value = txSaj3.Text;
                else
                    dgwForgalmak.Rows[sor - 1].Cells[4].Value = "0";
                SajatOsszesit();
                if (Rowleave)
                {
                    RowVerification(3);
                    Rowleave = false;
                }
            }
        }

        private void txSaj4_Leave(object sender, EventArgs e)
        {
            if (txAD4.Text!=string.Empty)
            {
                int sor = int.Parse(txSorsz4.Text);
                if (txSaj4.Text != string.Empty)
                    dgwForgalmak.Rows[sor - 1].Cells[4].Value = txSaj4.Text;
                else
                    dgwForgalmak.Rows[sor - 1].Cells[4].Value = "0";
                SajatOsszesit();
                if (Rowleave)
                {
                    RowVerification(4);
                    Rowleave = false;
                }
            }
        }

        private void SajatOsszesit()
        {
            SajatSum = 0;
            for (int i = 0; i < dgwForgalmak.RowCount; i++)
            {
                SajatSum += int.Parse(dgwForgalmak.Rows[i].Cells[4].Value.ToString());
            }
            txSajatSumC.Text = SajatSum.ToString();
        }

        #endregion saját

        # region mh
        private void txMh0_Leave(object sender, EventArgs e)
        {
            if (txAD0.Text != string.Empty)
            {
                int sor = int.Parse(txSorsz0.Text);
                if (txMh0.Text != string.Empty)
                    dgwForgalmak.Rows[sor - 1].Cells[5].Value = txMh0.Text;
                else
                    dgwForgalmak.Rows[sor - 1].Cells[5].Value = "0";
                MhOsszesit();
                if (Rowleave)
                {
                    RowVerification(0);
                    Rowleave = false;
                }
            }
        }

        private void txMh1_Leave(object sender, EventArgs e)
        {
            if (txAD1.Text != string.Empty)
            {
                int sor = int.Parse(txSorsz1.Text);
                if (txMh1.Text != string.Empty)
                    dgwForgalmak.Rows[sor - 1].Cells[5].Value = txMh1.Text;
                else
                    dgwForgalmak.Rows[sor - 1].Cells[5].Value = "0";
                MhOsszesit();
                if (Rowleave)
                {
                    RowVerification(1);
                    Rowleave = false;
                }
            }
        }

        private void txMh2_Leave(object sender, EventArgs e)
        {
            if (txAD2.Text != string.Empty)
            {
                int sor = int.Parse(txSorsz2.Text);
                if (txMh2.Text != string.Empty)
                    dgwForgalmak.Rows[sor - 1].Cells[5].Value = txMh2.Text;
                else
                    dgwForgalmak.Rows[sor - 1].Cells[5].Value = "0";
                MhOsszesit();
                if (Rowleave)
                {
                    RowVerification(2);
                    Rowleave = false;
                }
            }
        }

        private void txMh3_Leave(object sender, EventArgs e)
        {
            if (txAD3.Text != string.Empty)
            {
                int sor = int.Parse(txSorsz3.Text);
                if (txMh3.Text != string.Empty)
                    dgwForgalmak.Rows[sor - 1].Cells[5].Value = txMh3.Text;
                else
                    dgwForgalmak.Rows[sor - 1].Cells[5].Value = "0";
                MhOsszesit();
                if (Rowleave)
                {
                    RowVerification(3);
                    Rowleave = false;
                }
            }
        }

        private void txMh4_Leave(object sender, EventArgs e)
        {
            if (txAD4.Text != string.Empty)
            {
                int sor = int.Parse(txSorsz4.Text);
                if (txMh4.Text != string.Empty)
                    dgwForgalmak.Rows[sor - 1].Cells[5].Value = txMh4.Text;
                else
                    dgwForgalmak.Rows[sor - 1].Cells[5].Value = "0";
                MhOsszesit();
                if (Rowleave)
                {
                    RowVerification(4);
                    Rowleave = false;
                }
            }
        }

        private void MhOsszesit()
        {
            MunkSum = 0;
            for (int i = 0; i < dgwForgalmak.RowCount; i++)
            {
                MunkSum += int.Parse(dgwForgalmak.Rows[i].Cells[5].Value.ToString());
            }
            txMunkSumC.Text = MunkSum.ToString();
        }

        # endregion mh

        # region rendszeres
        private void txRend0_Leave(object sender, EventArgs e)
        {
            if (txAD0.Text != string.Empty)
            {
                int sor = int.Parse(txSorsz0.Text);
                if (txRend0.Text != string.Empty)
                    dgwForgalmak.Rows[sor - 1].Cells[6].Value = txRend0.Text;
                else
                    dgwForgalmak.Rows[sor - 1].Cells[6].Value = "0";
                RendszeresOsszesit();
                if (Rowleave)
                {
                    RowVerification(0);
                    Rowleave = false;
                }
            }
        }

        private void txRend1_Leave(object sender, EventArgs e)
        {
            if (txAD1.Text != string.Empty)
            {
                int sor = int.Parse(txSorsz1.Text);
                if (txRend1.Text != string.Empty)
                    dgwForgalmak.Rows[sor - 1].Cells[6].Value = txRend1.Text;
                else
                    dgwForgalmak.Rows[sor - 1].Cells[6].Value = "0";
                RendszeresOsszesit();
                if (Rowleave)
                {
                    RowVerification(1);
                    Rowleave = false;
                }
            }
        }

        private void txRend2_Leave(object sender, EventArgs e)
        {
            if (txAD2.Text != string.Empty)
            {
                int sor = int.Parse(txSorsz2.Text);
                if (txRend2.Text != string.Empty)
                    dgwForgalmak.Rows[sor - 1].Cells[6].Value = txRend2.Text;
                else
                    dgwForgalmak.Rows[sor - 1].Cells[6].Value = "0";
                RendszeresOsszesit();
                if (Rowleave)
                {
                    RowVerification(2);
                    Rowleave = false;
                }
            }
        }

        private void txRend3_Leave(object sender, EventArgs e)
        {
            if (txAD3.Text != string.Empty)
            {
                int sor = int.Parse(txSorsz3.Text);
                if (txRend3.Text != string.Empty)
                    dgwForgalmak.Rows[sor - 1].Cells[6].Value = txRend3.Text;
                else
                    dgwForgalmak.Rows[sor - 1].Cells[6].Value = "0";
                RendszeresOsszesit();
                if (Rowleave)
                {
                    RowVerification(3);
                    Rowleave = false;
                }
            }
        }

        private void txRend4_Leave(object sender, EventArgs e)
        {
            if (txAD4.Text != string.Empty)
            {
                int sor = int.Parse(txSorsz4.Text);
                if (txRend4.Text != string.Empty)
                    dgwForgalmak.Rows[sor - 1].Cells[6].Value = txRend4.Text;
                else
                    dgwForgalmak.Rows[sor - 1].Cells[6].Value = "0";
                RendszeresOsszesit();
                if (Rowleave)
                {
                    RowVerification(4);
                    Rowleave = false;
                }
            }
        }

        private void RendszeresOsszesit()
        {
            RendszSum = 0;
            for (int i = 0; i < dgwForgalmak.RowCount; i++)
            {
                RendszSum += int.Parse(dgwForgalmak.Rows[i].Cells[6].Value.ToString());
            }
            txRendszSumC.Text = RendszSum.ToString();
        }

        #endregion rendszeres

        #region egyszeri
        private void txEgysz0_Leave(object sender, EventArgs e)
        {
            if (txAD0.Text != string.Empty)
            {
                int sor = int.Parse(txSorsz0.Text);
                if (txEgysz0.Text != string.Empty)
                    dgwForgalmak.Rows[sor - 1].Cells[7].Value = txEgysz0.Text;
                else
                    dgwForgalmak.Rows[sor - 1].Cells[7].Value = "0";
                EgyszeriOsszesit();
                if (Rowleave)
                {
                    RowVerification(0);
                    Rowleave = false;
                }
            }
        }

        private void txEgysz1_Leave(object sender, EventArgs e)
        {
            if (txAD1.Text != string.Empty)
            {
                int sor = int.Parse(txSorsz1.Text);
                if (txEgysz1.Text != string.Empty)
                    dgwForgalmak.Rows[sor - 1].Cells[7].Value = txEgysz1.Text;
                else
                    dgwForgalmak.Rows[sor - 1].Cells[7].Value = "0";
                EgyszeriOsszesit();
                if (Rowleave)
                {
                    RowVerification(1);
                    Rowleave = false;
                }
            }
        }

        private void txEgysz2_Leave(object sender, EventArgs e)
        {
            if (txAD2.Text != string.Empty)
            {
                int sor = int.Parse(txSorsz2.Text);
                if (txEgysz2.Text != string.Empty)
                    dgwForgalmak.Rows[sor - 1].Cells[7].Value = txEgysz2.Text;
                else
                    dgwForgalmak.Rows[sor - 1].Cells[7].Value = "0";
                EgyszeriOsszesit();
                if (Rowleave)
                {
                    RowVerification(2);
                    Rowleave = false;
                }
            }
        }

        private void txEgysz3_Leave(object sender, EventArgs e)
        {
            if (txAD3.Text != string.Empty)
            {
                int sor = int.Parse(txSorsz3.Text);
                if (txEgysz3.Text != string.Empty)
                    dgwForgalmak.Rows[sor - 1].Cells[7].Value = txEgysz3.Text;
                else
                    dgwForgalmak.Rows[sor - 1].Cells[7].Value = "0";
                EgyszeriOsszesit();
                if (Rowleave)
                {
                    RowVerification(3);
                    Rowleave = false;
                }
            }
        }

        private void txEgysz4_Leave(object sender, EventArgs e)
        {
            if (txAD4.Text != string.Empty)
            {
                int sor = int.Parse(txSorsz4.Text);
                if (txEgysz4.Text != string.Empty)
                    dgwForgalmak.Rows[sor - 1].Cells[7].Value = txEgysz4.Text;
                else
                    dgwForgalmak.Rows[sor - 1].Cells[7].Value = "0";
                EgyszeriOsszesit();
                if (Rowleave)
                {
                    RowVerification(4);
                    Rowleave = false;
                }
            }
        }

        private void EgyszeriOsszesit()
        {
            EgyszeriSum = 0;
            for (int i = 0; i < dgwForgalmak.RowCount; i++)
            {
                EgyszeriSum += int.Parse(dgwForgalmak.Rows[i].Cells[7].Value.ToString());
            }
            TxEgyszeriSumC.Text = EgyszeriSum.ToString();
        }

        #endregion egyszeri

        #region befiz
        private void txBef0_Leave(object sender, EventArgs e)
        {
            if (txAD0.Text != string.Empty)
            {
                int sor = int.Parse(txSorsz0.Text);
                if (txBef0.Text != string.Empty)
                    dgwForgalmak.Rows[sor - 1].Cells[8].Value = txBef0.Text;
                else
                    dgwForgalmak.Rows[sor - 1].Cells[8].Value = "0";
                TotalOsszesit();
                RowVerification(0);
                //nextRow1();
                if (Rowleave)
                {
                    Rowleave = false;
                }
            }
            else
            {
                MessageBox.Show("Adóazonosító jel nem lehet üres!");
                txAD0.Focus();
            }
        }

        private void txBef1_Leave(object sender, EventArgs e)
        {
            if (txAD1.Text != string.Empty)
            {
                int sor = int.Parse(txSorsz1.Text);
                if (txBef1.Text != string.Empty)
                    dgwForgalmak.Rows[sor - 1].Cells[8].Value = txBef1.Text;
                else
                    dgwForgalmak.Rows[sor - 1].Cells[8].Value = "0";
                TotalOsszesit();
                RowVerification(1);
                nextRow2();
                if (Rowleave)
                {
                    Rowleave = false;
                }
            }
            else
            {
                MessageBox.Show("Adóazonosító jel nem lehet üres!");
                txAD1.Focus();
            }
        }

        private void txBef2_Leave(object sender, EventArgs e)
        {
            if (txAD2.Text != string.Empty)
            {
                int sor = int.Parse(txSorsz2.Text);
                if (txBef2.Text != string.Empty)
                    dgwForgalmak.Rows[sor - 1].Cells[8].Value = txBef2.Text;
                else
                    dgwForgalmak.Rows[sor - 1].Cells[8].Value = "0";
                TotalOsszesit();
                RowVerification(2);
                nextRow3();
                if (Rowleave)
                {
                    Rowleave = false;
                }
            }
            else
            {
                MessageBox.Show("Adóazonosító jel nem lehet üres!");
                txAD2.Focus();
            }
        }

        private void txBef3_Leave(object sender, EventArgs e)
        {
            if (txAD3.Text != string.Empty)
            {
                int sor = int.Parse(txSorsz3.Text);
                if (txBef3.Text != string.Empty)
                    dgwForgalmak.Rows[sor - 1].Cells[8].Value = txBef3.Text;
                else
                    dgwForgalmak.Rows[sor - 1].Cells[8].Value = "0";
                TotalOsszesit();
                RowVerification(3);
                nextRow4();
                if (Rowleave)
                {
                    Rowleave = false;
                }
            }
            else
            {
                MessageBox.Show("Adóazonosító jel nem lehet üres!");
                txAD3.Focus();
            }
        }

        private void txBef4_Leave(object sender, EventArgs e)
        {
            if (Rowleave)
            {
                if (txAD4.Text != string.Empty)
                {
                    int sor = int.Parse(txSorsz4.Text);
                    if (txBef4.Text != string.Empty)
                        dgwForgalmak.Rows[sor - 1].Cells[8].Value = txBef4.Text;
                    else
                        dgwForgalmak.Rows[sor - 1].Cells[8].Value = "0";
                    //TotalOsszesit();
                    //RowVerification(4);
                    Rowleave = false;
                    //CreateNewRow();
                    //txAD4.Focus();
                }
                else
                {
                    //MessageBox.Show("Adóazonosító jel nem lehet üres!");
                    //txAD4.Focus();
                }
            }
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

        #endregion befiz

        #endregion OSZLOPOK

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
            if(txSorsz4.Text!=string.Empty)
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

        private void CreateNewRow()
        {
            //if (makeNewRow) Sorindex = int.Parse(txSorsz1.Text);
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

        #region nyíl billentyűk kezelése

        #region adóazonosító
        private void txAD0_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Down:
                    if (txAD0.Text != string.Empty)
                    {
                        Rowleave = true;
                        txAD1.Focus();
                    }
                    break;
                case Keys.Up:
                    if (txAD0.Text != string.Empty)
                    {
                        Rowleave = true;
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
                        Rowleave = true;
                        txAD2.Focus();
                    }
                    break;
                case Keys.Up:
                    if (txAD1.Text != string.Empty)
                    {
                        Rowleave = true;
                        txAD0.Focus();
                    }
                    else if (txPnrid1.Text == string.Empty)
                    {
                        Rowleave = true;
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
                        Rowleave = true;
                        txAD3.Focus();
                    }
                    break;
                case Keys.Up:
                    if (txAD2.Text != string.Empty)
                    {
                        Rowleave = true;
                        txAD1.Focus();
                    }
                    else if (txPnrid2.Text == string.Empty)
                    {
                        Rowleave = true;
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
                        Rowleave = true;
                        txAD4.Focus();
                    }
                    break;
                case Keys.Up:
                    if (txAD3.Text != string.Empty)
                    {
                        Rowleave = true;
                        txAD2.Focus();
                    }
                    else if (txPnrid3.Text == string.Empty)
                    {
                        Rowleave = true;
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
                            Rowleave = true;
                            MovesDown();
                        }
                    }
                    catch
                    { }
                    break;
                case Keys.Up:
                    if (txAD4.Text != string.Empty)
                    {
                        Rowleave = true;
                        txAD3.Focus();
                    }
                    else if (txPnrid4.Text == string.Empty)
                    {
                        Rowleave = true;
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
                        Rowleave = true;
                        txSaj1.Focus();
                    }
                    break;
                case Keys.Up:
                    if (txAD0.Text != string.Empty)
                    {
                        Rowleave = true;
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
                        Rowleave = true;
                        txSaj2.Focus();
                    }
                    break;
                case Keys.Up:
                    if (txAD1.Text != string.Empty)
                    {
                        Rowleave = true;
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
                        Rowleave = true;
                        txSaj3.Focus();
                    }
                    break;
                case Keys.Up:
                    if (txAD2.Text != string.Empty)
                    {
                        Rowleave = true;
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
                        Rowleave = true;
                        txSaj4.Focus();
                    }
                    break;
                case Keys.Up:
                    if (txAD3.Text != string.Empty)
                    {
                        Rowleave = true;
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
                            Rowleave = true;
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
                        Rowleave = true;
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
                    Rowleave = true;
                    break;
                case Keys.Up: 
                    if (txAD0.Text != string.Empty) 
                        MovesUp();
                    Rowleave = true;
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
                    Rowleave = true;
                    break;
                case Keys.Up: 
                    if (txAD1.Text != string.Empty) 
                        txMh0.Focus();
                    Rowleave = true;
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
                    Rowleave = true;
                    break;
                case Keys.Up: 
                    if (txAD2.Text != string.Empty) 
                        txMh1.Focus();
                    Rowleave = true;
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
                    Rowleave = true;
                    break;
                case Keys.Up: 
                    if (txAD3.Text != string.Empty) 
                        txMh2.Focus();
                    Rowleave = true;
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
                    Rowleave = true;
                    break;
                case Keys.Up: 
                    if (txAD4.Text != string.Empty) 
                        txMh3.Focus();
                    Rowleave = true;
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
                    Rowleave = true;
                    break;
                case Keys.Up: 
                    if (txAD0.Text != string.Empty) 
                        MovesUp();
                    Rowleave = true;
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
                    Rowleave = true;
                    break;
                case Keys.Up: 
                    if (txAD1.Text != string.Empty) 
                        txRend0.Focus();
                    Rowleave = true;
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
                    Rowleave = true;
                    break;
                case Keys.Up: 
                    if (txAD2.Text != string.Empty) 
                        txRend1.Focus();
                    Rowleave = true;
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
                    Rowleave = true;
                    break;
                case Keys.Up: 
                    if (txAD3.Text != string.Empty) 
                        txRend2.Focus();
                    Rowleave = true;
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
                    Rowleave = true;
                    break;
                case Keys.Up: 
                    if (txAD4.Text != string.Empty) 
                        txRend3.Focus();
                    Rowleave = true;
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
                    Rowleave = true;
                    break;
                case Keys.Up: 
                    if (txAD0.Text != string.Empty) 
                        MovesUp();
                    Rowleave = true;
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
                    Rowleave = true;
                    break;
                case Keys.Up: 
                    if (txAD1.Text != string.Empty) 
                        txEgysz0.Focus();
                    Rowleave = true;
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
                    Rowleave = true;
                    break;
                case Keys.Up:
                    if (txAD2.Text != string.Empty) 
                        txEgysz1.Focus();
                    Rowleave = true;
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
                    Rowleave = true;
                    break;
                case Keys.Up:
                    if (txAD3.Text != string.Empty) 
                        txEgysz2.Focus();
                    Rowleave = true;
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
                    Rowleave = true;
                    break;
                case Keys.Up:
                    if (txAD4.Text != string.Empty) 
                        txEgysz3.Focus();
                    Rowleave = true;
                    break;
                default: break;
            }
        }
        #endregion egyszeri

        #region befizetendő
        private void txBef0_KeyDown(object sender, KeyEventArgs e)
        {
            //e.SuppressKeyPress = false;
            switch (e.KeyCode)
            {
                case Keys.Down:
                    if (txAD0.Text != string.Empty)
                    {
                        txBef1.Focus();
                    }
                    Rowleave = true;
                    break;
                case Keys.Up: 
                    if (txAD0.Text != string.Empty) 
                        MovesUp();
                    Rowleave = true;
                    break;
                case Keys.Enter:
                    //RowVerification(0);
                    Rowleave = false;
                    //nextRow1();
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
                    {
                        txBef2.Focus();
                    }
                    Rowleave = true;
                    break;
                case Keys.Up: 
                    if (txAD1.Text != string.Empty) 
                        txBef0.Focus();
                    Rowleave = true;
                    break;
                case Keys.Enter:
                    Rowleave = true;
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
                    {
                        txBef3.Focus();
                    }
                    Rowleave = true;
                    break;
                case Keys.Up: 
                    if (txAD2.Text != string.Empty) 
                        txBef1.Focus();
                    Rowleave = true;
                    break;
                case Keys.Enter:
                    Rowleave = true;
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
                    {
                        txBef4.Focus();
                    }
                    Rowleave = true;
                    break;
                case Keys.Up: 
                    if (txAD3.Text != string.Empty) 
                        txBef2.Focus();
                    Rowleave = true;
                    break;
                case Keys.Enter:
                    Rowleave = true;
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
                    Rowleave = true;
                    makeNewRow = false;
                    break;
                case Keys.Up: 
                    if (txAD4.Text != string.Empty) 
                        txBef3.Focus();
                    Rowleave = true;
                    makeNewRow = false;
                    break;
                case Keys.Enter:
                    //Rowleave = true;
                    //makeNewRow = true;
                    //Bef4Leave();
                    break;
                default: break;
            }
        }

        private void Bef4Leave()
        {
            //if (txAD4.Text != string.Empty)
            //{
                int sor = int.Parse(txSorsz4.Text);
                if (txBef4.Text != string.Empty)
                    dgwForgalmak.Rows[sor - 1].Cells[8].Value = txBef4.Text;
                else
                    dgwForgalmak.Rows[sor - 1].Cells[8].Value = "0";
                TotalOsszesit();
                RowVerification(4);
                Rowleave = false;
                CreateNewRow();
                makeNewRow = false;
                txAD4.Focus();
            //}
            //else
            //{
            //    MessageBox.Show("Adóazonosító jel nem lehet üres!");
            //    txAD4.Focus();
            //}
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

        private void txErv_TextChanged(object sender, EventArgs e)
        {
            if (txErv.Text == "N")
                txErv.BackColor = Color.IndianRed;
            else
                txErv.BackColor = Color.LightGreen;
        }

        private void txTagokSum_ValueChanged(object sender, EventArgs e)
        {
            //tsSave.Enabled = true;
        }

        private void bDel1_Click(object sender, EventArgs e)
        {
            if (txForgid0.Text != string.Empty)
            {
                if (txKonyvelt.Text != string.Empty)
                {
                    MessageBox.Show("A bevallás könyvelt, nem törölhető!");
                }
                else
                {
                    scommand = new SqlCommand("spForgalmakDelete", sconn);
                    scommand.CommandType = CommandType.StoredProcedure;
                    scommand.Parameters.Add(new SqlParameter("@id", SqlDbType.Int)).Value = int.Parse(txForgid0.Text);
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
            if (txSorsz0.Text != string.Empty)
                deleteRecord(txSorsz0.Text);
        }

        private void bDel2_Click(object sender, EventArgs e)
        {
            if (txForgid1.Text != string.Empty)
            {
                if (txKonyvelt.Text != string.Empty)
                {
                    MessageBox.Show("A bevallás könyvelt, nem törölhető!");
                }
                else
                {
                    scommand = new SqlCommand("spForgalmakDelete", sconn);
                    scommand.CommandType = CommandType.StoredProcedure;
                    scommand.Parameters.Add(new SqlParameter("@id", SqlDbType.Int)).Value = int.Parse(txForgid1.Text);
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
            if (txSorsz1.Text != string.Empty)
                deleteRecord(txSorsz1.Text);
        }

        private void bDel3_Click(object sender, EventArgs e)
        {
            if (txForgid2.Text != string.Empty)
            {
                if (txKonyvelt.Text != string.Empty)
                {
                    MessageBox.Show("A bevallás könyvelt, nem törölhető!");
                }
                else
                {
                    scommand = new SqlCommand("spForgalmakDelete", sconn);
                    scommand.CommandType = CommandType.StoredProcedure;
                    scommand.Parameters.Add(new SqlParameter("@id", SqlDbType.Int)).Value = int.Parse(txForgid2.Text);
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
            if (txSorsz2.Text != string.Empty)
                deleteRecord(txSorsz2.Text);
        }

        private void bDel4_Click(object sender, EventArgs e)
        {
            if (txForgid3.Text != string.Empty)
            {
                if (txKonyvelt.Text != string.Empty)
                {
                    MessageBox.Show("A bevallás könyvelt, nem törölhető!");
                }
                else
                {
                    scommand = new SqlCommand("spForgalmakDelete", sconn);
                    scommand.CommandType = CommandType.StoredProcedure;
                    scommand.Parameters.Add(new SqlParameter("@id", SqlDbType.Int)).Value = int.Parse(txForgid3.Text);
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
            if (txSorsz3.Text != string.Empty)
                deleteRecord(txSorsz3.Text);
        }

        private void bDel5_Click(object sender, EventArgs e)
        {
            if (txForgid4.Text != string.Empty)
            {
                if (txKonyvelt.Text != string.Empty)
                {
                    MessageBox.Show("A bevallás könyvelt, nem törölhető!");
                }
                else
                {
                    scommand = new SqlCommand("spForgalmakDelete", sconn);
                    scommand.CommandType = CommandType.StoredProcedure;
                    scommand.Parameters.Add(new SqlParameter("@id", SqlDbType.Int)).Value = int.Parse(txForgid4.Text);
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
            if (txSorsz4.Text != string.Empty)
                deleteRecord(txSorsz4.Text);
        }

        private void bStorno_Click(object sender, EventArgs e)
        {
            if (!tsSave.Enabled && txKonyvelt.Text == "I" && bev_id != 0 && txStorno.Text == "0")
            {
                // update1
                string sql = "update fokonyvi_tetelek set statusz='S', storno='1'" +
                " where (hivatkozasi_szam in (select distinct hivatkozasi_szam" +
                " from fokonyvi_tetelek" +
                " where bev_id=" + bev_id.ToString() + ")) or (bev_id=" + bev_id.ToString() + " and hivatkozasi_szam is null);";
                scommand = new SqlCommand(sql, sconn);
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
                // update2
                sql = "update bevallasok set storno='1' where bev_id=" + bev_id.ToString();
                scommand = new SqlCommand(sql, sconn);
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
                txStorno.Text = "1";
            }
            else
            {
                MessageBox.Show("A bevallást nem lehet sztornózni.");
            }
        }

        private void nextRow1()
        {
            txAD1.ReadOnly = false;
            txSaj1.ReadOnly = false;
            txMh1.ReadOnly = false;
            txRend1.ReadOnly = false;
            txEgysz1.ReadOnly = false;
            txBef1.ReadOnly = false;
        }

        private void nextRow2()
        {
            txAD2.ReadOnly = false;
            txSaj2.ReadOnly = false;
            txMh2.ReadOnly = false;
            txRend2.ReadOnly = false;
            txEgysz2.ReadOnly = false;
            txBef2.ReadOnly = false;
        }

        private void nextRow3()
        {
            txAD3.ReadOnly = false;
            txSaj3.ReadOnly = false;
            txMh3.ReadOnly = false;
            txRend3.ReadOnly = false;
            txEgysz3.ReadOnly = false;
            txBef3.ReadOnly = false;
        }

        private void nextRow4()
        {
            txAD4.ReadOnly = false;
            txSaj4.ReadOnly = false;
            txMh4.ReadOnly = false;
            txRend4.ReadOnly = false;
            txEgysz4.ReadOnly = false;
            txBef4.ReadOnly = false;
        }

        private void txBef0_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                RowVerification(0);
                Rowleave = false;
                nextRow1();
            }
        }

        private void txBef4_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (txAD4.Text != string.Empty)
                {
                    Bef4Leave();
                    txAD4.Focus();
                }
                else
                {
                    MessageBox.Show("Adóazonosító jel nem lehet üres!");
                    txAD4.Focus();
                }
                //Bef4Leave();
            }
        }
    }
}
