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
    public partial class ZaradekolasMenubol : VoluntaryPensionFundSystem.BaseForm
    {
        private SqlConnection sconn;
        private SqlCommand scommand, lastScommand;
        private SqlDataAdapter da;
        private DataTable dt;

        //Okiratok tag;
        string querystring;
        private bool keresomod;
        private string tszsid;

        TextWriterTraceListener myListener = new TextWriterTraceListener(@"LOG\Zaradekolas.log", "myListener");

        public ZaradekolasMenubol(SqlConnection SqlConn)
        {
            InitializeComponent();

            this.sconn = SqlConn;

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
            keresomod = true;
            bZaradek.Enabled = false;
            txNev.Focus();
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

            dt.Columns["pnr_id"].ColumnName = "Pnr Id";
            dt.Columns["adoazonosito_jel"].ColumnName = "Adóazonosító jel";
            dt.Columns["nev"].ColumnName = "Név";
            dt.Columns["orszagkod"].ColumnName = "Orsz.kód";
            dt.Columns["ir_szam"].ColumnName = "Ir.szám";
            dt.Columns["helyseg"].ColumnName = "Helység";
            dt.Columns["cim"].ColumnName = "Cím";
            dt.Columns["leanykori_nev"].ColumnName = "Születési Név";
            dt.Columns["anyja_neve"].ColumnName = "Anyja Neve";
            dt.Columns["szul_helye"].ColumnName = "Születési Hely";
            dt.Columns["szul_dat"].ColumnName = "Szül. Idő";
            dt.Columns["neme"].ColumnName = "Neme";
            dt.Columns["belep_tipusa"].ColumnName = "Belép. típ.";
            dt.Columns["belep_datum"].ColumnName = "Belép. dátum";
            dt.Columns["tagdijfiz_tipus"].ColumnName = "Tagdíjfiz. típ.";
            dt.Columns["vallalt_tagdij"].ColumnName = "Vállalt tagdíj";
            dt.Columns["felvet_datum"].ColumnName = "Felvét dátuma";
            dt.Columns["zaradek_datum"].ColumnName = "Záradékolás";
            dt.Columns["eletcikl_stat"].ColumnName = "Életcikl. stát.";
            dt.Columns["csekket_ker"].ColumnName = "Csekket kér";
            dt.Columns["alair_datum"].ColumnName = "Aláírás dátuma";
            dt.Columns["erkez_datum"].ColumnName = "Érkezés dátuma";
            dt.Columns["megsz_tipusa"].ColumnName = "Megsz. típusa";
            dt.Columns["kilep_datum"].ColumnName = "Kilépés dátuma";
            dt.Columns["eredeti_tagsag"].ColumnName = "Eredeti tagság";
            dt.Columns["szamla_lezar"].ColumnName = "Számla lez. dát.";
            dt.Columns["tszs_id"].ColumnName = "PAJ";
        }

        private void ZaradekolasMenubol_Load(object sender, EventArgs e)
        {

        }

        public override void tsSave_Click(object sender, EventArgs e)
        {

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
            dgwOkiratok.DataSource = null;
            dgwOkiratok.Refresh();
            txNev.ReadOnly = false;
            txPnrid.ReadOnly = false;
            txAdoazon.ReadOnly = false;

            txPnrid.Text = string.Empty;
            txAdoazon.Text = string.Empty;
            txNev.Text = string.Empty;

            tsUpdate.Enabled = false;
            tsDelete.Enabled = false;
            tsNew.Enabled = false;
            tsSave.Enabled = false;
            tsFind.Enabled = true;
            keresomod = true;
            bZaradek.Enabled = false;
            txNev.Focus();
        }

        public override void runQuery()
        {
            this.dgwOkiratok.DataSource = null;

            // A LEKÉRDEZÉS FELÉPÍTÉSE
            querystring = "SELECT pnr.pnr_id,pnr.nev,pnr.adoazonosito_jel," +
                "pnr.orszagkod,pnr.ir_szam,pnr.helyseg,pnr.cim," +
                "pnr.leanykori_nev,pnr.anyja_neve,pnr.szul_dat,pnr.szul_helye," +
                "CASE pnr.neme WHEN 1 THEN 'FÉRFI' WHEN 2 THEN 'NŐ' END AS neme," +
                "tszs.belep_tipusa,tszs.belep_datum,tszs.tagdijfiz_tipus,tszs.vallalt_tagdij,tszs.csekket_ker,tszs.eletcikl_stat," +
                "tszs.megsz_tipusa,tszs.kilep_datum,tszs.alair_datum,tszs.erkez_datum," +
                "tszs.felvet_datum,tszs.zaradek_datum,tszs.eredeti_tagsag,tszs.szamla_lezar,tszs.tszs_id " +
                "FROM partnerek pnr, tagsagi_szerzodesek tszs WHERE pnr.pnr_id=tszs.pnr_id and pnr_tipus='SZMLY' and ";
            querystring = txNev.Text != string.Empty ? querystring + "pnr.nev like '" + txNev.Text + "' and " : querystring;
            querystring = (txPnrid.Text != string.Empty ? querystring + "pnr.pnr_id like '" + txPnrid.Text + "' and " : querystring);
            querystring = (txAdoazon.Text != string.Empty ? querystring + "pnr.adoazonosito_jel like '" + txAdoazon.Text + "' and " : querystring);
            querystring += "(pnr.ervenytelen is null or pnr.ervenytelen!='I') and tszs.zaradek_datum is null order by nev;";

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

            tsUpdate.Enabled = false;
            tsDelete.Enabled = false;
            tsNew.Enabled = false;
            tsSave.Enabled = false;
            tsFind.Enabled = false;
            tsSearch.Enabled = true;
        }

        private void MentesKesz()
        {
            dtBelepes.Enabled = false;
            dtFelvetel.Enabled = false;
            dtZaradek.Enabled = false;
            cbEletcikl.Enabled = false;
            //tag.txPaj.Text = this.txPaj.Text;
            //tag.txPaj.Visible = true;
            //tag.labelZaradek.Visible = true;
        }

        private void TraceBejegyzes(string msg)
        {
            myListener.WriteLine("- " + DateTime.Now + ": " + msg);
            myListener.Flush();
            myListener.Close();
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

        private void txNev_KeyUp(object sender, KeyEventArgs e)
        {
            if (keresomod)
            {
                this.dgwOkiratok.DataSource = null;

                if (txNev.Text != string.Empty)
                {
                    // A LEKÉRDEZÉS FELÉPÍTÉSE
                    querystring = "SELECT pnr.pnr_id,pnr.nev,pnr.adoazonosito_jel," +
                        "pnr.orszagkod,pnr.ir_szam,pnr.helyseg,pnr.cim," +
                        "pnr.leanykori_nev,pnr.anyja_neve,pnr.szul_dat,pnr.szul_helye," +
                        "CASE pnr.neme WHEN 1 THEN 'FÉRFI' WHEN 2 THEN 'NŐ' END AS neme," +
                        "tszs.belep_tipusa,tszs.belep_datum,tszs.tagdijfiz_tipus,tszs.vallalt_tagdij,tszs.csekket_ker,tszs.eletcikl_stat," +
                        "tszs.megsz_tipusa,tszs.kilep_datum,tszs.alair_datum,tszs.erkez_datum," +
                        "tszs.felvet_datum,tszs.zaradek_datum,tszs.eredeti_tagsag,tszs.szamla_lezar,tszs.tszs_id " +
                        "FROM partnerek pnr, tagsagi_szerzodesek tszs WHERE pnr.pnr_id=tszs.pnr_id and pnr_tipus='SZMLY' and ";
                    querystring = txNev.Text != string.Empty ? querystring + "pnr.nev like '" + txNev.Text + "%' and " : querystring;
                    querystring += "(pnr.ervenytelen is null or pnr.ervenytelen!='I') and tszs.zaradek_datum is null order by nev;"; //and tszs.zaradek_datum is null

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

                    tsFind.Enabled = false;
                    tsSearch.Enabled = true;
                }
                dgwOkiratok.Refresh();
            }
        }

        private void bZaradek_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("Biztosan záradékolható a tag a megadott adatokkal?\n" +
                "Aláírás dátuma: " + dtAlair.Value.ToShortDateString() + "\nBelépés dátuma: " + dtBelepes.Value.ToShortDateString() + 
                "\nÉrkezés dátuma: " + dtErkezes.Value.ToShortDateString() + "\nFelvétel dátuma: " + 
                dtFelvetel.Value.ToShortDateString(), "Megerősítés", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
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
                //scommand = lastScommand;
                //da = new SqlDataAdapter(scommand);
                //dt = new DataTable();
                //Frissit();
                //this.dgwOkiratok.DataSource = dt;
                txPaj.Text = tszsid;

                MessageBox.Show("A záradékolás sikeres, dátum: " + dtZaradek.Text + " PAJ: " + tszsid);
            }
        }

        private void dgwOkiratok_SelectionChanged(object sender, EventArgs e)
        {
            if (!keresomod)
            {
                try
                {
                    int i = dgwOkiratok.SelectedCells[0].RowIndex;
                    txPnrid.Text = dgwOkiratok.Rows[i].Cells[0].Value.ToString();
                    txNev.Text = dgwOkiratok.Rows[i].Cells[1].Value.ToString();
                    txAdoazon.Text = dgwOkiratok.Rows[i].Cells[2].Value.ToString();
                    dtAlair.Text = DateTime.Parse(dgwOkiratok.Rows[i].Cells[20].Value.ToString()).ToShortDateString();
                    dtBelepes.Text = DateTime.Parse(dgwOkiratok.Rows[i].Cells[13].Value.ToString()).ToShortDateString();
                    dtErkezes.Text = DateTime.Parse(dgwOkiratok.Rows[i].Cells[21].Value.ToString()).ToShortDateString();
                    dtFelvetel.Text = DateTime.Parse(dgwOkiratok.Rows[i].Cells[22].Value.ToString()).ToShortDateString();
                    cbEletcikl.Text = dgwOkiratok.Rows[i].Cells[17].Value.ToString();
                    tszsid = dgwOkiratok.Rows[i].Cells[26].Value.ToString();
                    bZaradek.Enabled = true;
                }
                catch
                {
                    // Kezdeti probléma...
                }
            }
        }

        private void txNev_Leave(object sender, EventArgs e)
        {
            keresomod = false;
            lastScommand = scommand;
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

        private void ZaradekolasMenubol_Resize(object sender, EventArgs e)
        {
            dgwOkiratok.Width = ZaradekolasMenubol.ActiveForm.Width - 60;
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
    }
}
