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
    public partial class BefizetesekRogz : VoluntaryPensionFundSystem.BaseForm
    {
        private SqlConnection sconn;
        private SqlCommand scommand, scommandT;
        private SqlDataAdapter da, daTetelek;
        private DataTable dt, dtTetelek;
        private bool back, enter = false;
        
        public int kivonatSzama;
        public string erteknap, letrehozas, irany, megjegyzes;
        public int bkt_id;

        private List<string> lista = new List<string>();
        private int Sorindex;
        private const string MatchSzlaszamPattern = @"^[0-9]{24}$";

        // Napló logok:
        TextWriterTraceListener myListener = new TextWriterTraceListener(@"LOG\BefizetesekRogzitese.log", "myListener");

        public BefizetesekRogz(SqlConnection SqlConn)
        {
            InitializeComponent();

            this.sconn = SqlConn;

            lista.Add(string.Empty);
            lista.Add("T");
            lista.Add("K");
            cbIrany.DataSource = lista;

            tsDelete.Enabled = false;
            tsFind.Enabled = true;
            tsNew.Enabled = false;
            tsSave.Enabled = false;
            tsSearch.Enabled = false;
            tsUpdate.Enabled = false;
            bSaveAndNext.Enabled = false;

            if (sconn.State == ConnectionState.Closed) sconn.Open();

            datErteknap.ReadOnly = false;
            datLetrehoz.ReadOnly = false;
            txKivonatSzama.Enabled = true;
            cbIrany.Enabled = true;
            txMegjegyzes.ReadOnly = false;
            datErteknap.Text = string.Empty;
            datLetrehoz.Text = string.Empty;
            cbIrany.Text = string.Empty;
            //cbIrany.Text = lista[0].ToString();
            txMegjegyzes.Text = string.Empty;
            txKivonatSzama.Value = 0;

            txOsszeg.ReadOnly = true;
            txSzamlaszam.ReadOnly = true;
            txNev.ReadOnly = true;
            txKozlemeny.ReadOnly = true;
            txKonyveles.ReadOnly = true;
            txStatus.ReadOnly = true;
            txStorno.ReadOnly = true;
            txMegjegyzes.ReadOnly = true;

            datErteknap.Focus();
        }

        private void BefizetesekRogz_Load(object sender, EventArgs e)
        {

        }

        private void Frissit()
        {
            //dt.Dispose();
            try
            {
                daTetelek.Fill(dtTetelek);
            }
            catch
            {
                //MessageBox.Show(ex.Message);
                //TraceBejegyzes(ex.Message);
            }
            //btl_id,osszeg,pnr_id,erteknap,sorszam,szamlaszam,nev,kozlemeny,konyveles_datuma,statusz,storno,megjegyzes
            dtTetelek.Columns["osszeg"].ColumnName = "Összeg";
            dtTetelek.Columns["erteknap"].ColumnName = "Értéknap";
            dtTetelek.Columns["sorszam"].ColumnName = "Sorsz.";
            dtTetelek.Columns["szamlaszam"].ColumnName = "Számlaszám";
            dtTetelek.Columns["nev"].ColumnName = "Név";
            dtTetelek.Columns["kozlemeny"].ColumnName = "Közlemény";
            dtTetelek.Columns["konyveles_datuma"].ColumnName = "Könyvelés dátuma";
            dtTetelek.Columns["statusz"].ColumnName = "Státusz";
            dtTetelek.Columns["storno"].ColumnName = "Stornó";
            dtTetelek.Columns["megjegyzes"].ColumnName = "Megjegyzés";
        }

        private void RejtId()
        {
            dgvBankKiv.Columns[0].Visible = false;
            dgvBankKiv.Columns[1].Visible = false;
            dgvBankKiv.Columns[2].Width = 60;   // sorszám
            dgvBankKiv.Columns[3].Width = 100;  // ért.nap
            dgvBankKiv.Columns[4].Width = 230;  // szla
            dgvBankKiv.Columns[5].Width = 200;  // név
            dgvBankKiv.Columns[6].Width = 100;  // összeg
            dgvBankKiv.Columns[7].Width = 200;  // közl.
            dgvBankKiv.Columns[8].Width = 100;  // könyv.
            dgvBankKiv.Columns[9].Width = 60;   // statusz
            dgvBankKiv.Columns[10].Width = 60;   // storno
            dgvBankKiv.Columns[11].Width = 200;   // megj.
        }

        public override void tsNew_Click(object sender, EventArgs e)
        {
            NewRec();
        }

        private void NewRec()
        {
            txOsszeg.ReadOnly = false;
            txSzamlaszam.ReadOnly = false;
            txNev.ReadOnly = false;
            txKozlemeny.ReadOnly = false;
            txMegjegyzes.ReadOnly = false;

            txOsszeg.Text = string.Empty;
            txSzamlaszam.Text = string.Empty;
            txNev.Text = string.Empty;
            txKozlemeny.Text = string.Empty;
            txKonyveles.Text = string.Empty;
            txStatus.Text = string.Empty;
            txStorno.Text = string.Empty;
            txMegjegyzes.Text = string.Empty;
            txBtlId.Text = string.Empty;

            int last = dgvBankKiv.RowCount + 1;
            txSorszam.Text = last.ToString();

            tsSave.Enabled = true;
            bSaveAndNext.Enabled = true;
            txOsszeg.Focus();
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

        private void updateRecord()
        {
            txOsszeg.ReadOnly = false;
            txSzamlaszam.ReadOnly = false;
            txNev.ReadOnly = false;
            txKozlemeny.ReadOnly = false;
            txMegjegyzes.ReadOnly = false;
            tsSave.Enabled = true;
            bSaveAndNext.Enabled = true;
        }

        public override void tsSave_Click(object sender, EventArgs e)
        {
            save();
        }

        public override void yellowMode()
        {
            //keresomod = true;

            datErteknap.ReadOnly = false;
            datLetrehoz.ReadOnly = false;
            txKivonatSzama.Enabled = true;
            cbIrany.Enabled = true;
            //txMegjegyzes.ReadOnly = false;
            datErteknap.Text = string.Empty;
            datLetrehoz.Text = string.Empty;
            cbIrany.Text = string.Empty;
            //cbIrany.Text = lista[0].ToString();
            txMegjegyzes.Text = string.Empty;
            txKivonatSzama.Value = 0;

            txOsszeg.Text = string.Empty;
            txSzamlaszam.Text = string.Empty;
            txNev.Text = string.Empty;
            txKozlemeny.Text = string.Empty;
            txKonyveles.Text = string.Empty;
            txStatus.Text = string.Empty;
            txStorno.Text = string.Empty;
            txMegjegyzes.Text = string.Empty;
            txBtlId.Text = string.Empty;

            dt = null;
            dgvBankKiv.DataSource = dt;
            datErteknap.Focus();

            tsFind.Enabled = true;
            tsSearch.Enabled = false;

            tsUpdate.Enabled = false;
            tsNew.Enabled = false;
            tsDelete.Enabled = false;
            tsSave.Enabled = false;
        }

        public override void runQuery()
        {
            DateTime erteknap, letrehozas;
            string erteknapstr = string.Empty, letrehozasstr = string.Empty;
            if (datErteknap.Text != string.Empty)
            {
                try
                {
                    DateTransformA();
                    erteknap = DateTime.Parse(datErteknap.Text);
                    erteknapstr = erteknap.ToShortDateString();
                    erteknapstr = erteknapstr.Substring(0, 4) + "-" + erteknapstr.Substring(5, 2) + "-" + erteknapstr.Substring(8, 2);
                }
                catch
                {
                    MessageBox.Show("Dátum formátum nem megfelelő!");
                    return;
                }
            }
            if (datLetrehoz.Text != string.Empty)
            {
                try
                {
                    DateTransformB();
                    letrehozas = DateTime.Parse(datLetrehoz.Text);
                    letrehozasstr = letrehozas.ToShortDateString();
                    letrehozasstr = letrehozasstr.Substring(0, 4) + "-" + letrehozasstr.Substring(5, 2) + "-" + letrehozasstr.Substring(8, 2);
                }
                catch
                {
                    MessageBox.Show("Dátum formátum nem megfelelő!");
                    return;
                }
            }

            //dt = new DataTable();
            //this.dgvBankKiv.DataSource = null;

            // A LEKÉRDEZÉS FELÉPÍTÉSE
            string querystring1 = "SELECT count(bkt_id) FROM banki_kivonatok WHERE ";
            querystring1 = datErteknap.Text != string.Empty ? querystring1 + "bizonylat_kelte like '" + erteknapstr + "' and " : querystring1;
            querystring1 = (datLetrehoz.Text != string.Empty ? querystring1 + "letrehozas_datuma like '" + letrehozasstr + "' and " : querystring1);
            querystring1 = (txKivonatSzama.Value != 0 ? querystring1 + "kivonat_szama = " + txKivonatSzama.Value + " and " : querystring1);
            querystring1 = (cbIrany.Text != string.Empty ? querystring1 + "irany like '" + cbIrany.Text + "' and " : querystring1);
            querystring1 = (txMegjegyzes.Text != string.Empty ? querystring1 + "megjegyzes like '" + txMegjegyzes.Text + "' and " : querystring1);
            querystring1 += "bizonylat_kelte is not null;";

            int darab = 0;
            try
            {
                scommand = new SqlCommand(querystring1, sconn);
                darab = (int)scommand.ExecuteScalar();
            }
            catch (Exception ex)
            {
                MessageBox.Show("SQL Hiba " + ex.Message);
            }

            string querystring2 = "SELECT bkt_id,bizonylat_kelte,kivonat_szama,irany,letrehozas_datuma,megjegyzes FROM banki_kivonatok WHERE ";
            querystring2 = datErteknap.Text != string.Empty ? querystring2 + "bizonylat_kelte like '" + erteknapstr + "' and " : querystring2;
            querystring2 = (datLetrehoz.Text != string.Empty ? querystring2 + "letrehozas_datuma like '" + letrehozasstr + "' and " : querystring2);
            querystring2 = (txKivonatSzama.Value != 0 ? querystring2 + "kivonat_szama = " + txKivonatSzama.Value + " and " : querystring2);
            querystring2 = (cbIrany.Text != string.Empty ? querystring2 + "irany like '" + cbIrany.Text + "' and " : querystring2);
            querystring2 = (txMegjegyzes.Text != string.Empty ? querystring2 + "megjegyzes like '" + txMegjegyzes.Text + "' and " : querystring2);
            querystring2 += "bizonylat_kelte is not null order by 1,2;";

            if (darab > 1)
            {
                scommand = new SqlCommand(querystring2, sconn);
                //lastScommand = scommand;
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

                BankiKivonKivalaszt bkk = new BankiKivonKivalaszt(dt, this);
                bkk.ShowDialog(this);

                datErteknap.Text = DateTime.Parse(this.erteknap).ToShortDateString();
                txKivonatSzama.Value = this.kivonatSzama;
                cbIrany.Text = this.irany;
                datLetrehoz.Text = DateTime.Parse(this.letrehozas).ToShortDateString();
                txId.Text = this.bkt_id.ToString();
            }
            else
            {
                SqlDataReader myReader = null;
                scommand = new SqlCommand(querystring2, sconn);
                //scommand.CommandType = CommandType.StoredProcedure;
                //scommand.Parameters.Add(new SqlParameter("@id", SqlDbType.Int)).Value = int.Parse(txPnrid.Text);
                try
                {
                    if (sconn.State == ConnectionState.Closed) sconn.Open();
                    myReader = scommand.ExecuteReader();
                    while (myReader.Read())
                    {
                        datErteknap.Text = DateTime.Parse(myReader["bizonylat_kelte"].ToString()).ToShortDateString();
                        txKivonatSzama.Text = myReader["kivonat_szama"].ToString();
                        cbIrany.Text = myReader["irany"].ToString();
                        datLetrehoz.Text = DateTime.Parse(myReader["letrehozas_datuma"].ToString()).ToShortDateString();
                        txId.Text = myReader["bkt_id"].ToString();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("SQL Hiba " + ex.Message);
                    TraceBejegyzes(ex.Message);
                }
                myReader.Close();
            }

            // datagrid betöltése
            dgrLoad();

            //datErteknap.Focus();
            txOsszeg.Focus();
            datErteknap.ReadOnly = true;
            datLetrehoz.ReadOnly = true;
            txKivonatSzama.Enabled = false;
            cbIrany.Enabled = false;

            tsFind.Enabled = false;
            tsSearch.Enabled = true;
            tsNew.Enabled = true;            
            tsSave.Enabled = false;
            bSaveAndNext.Enabled = false;

            if (dgvBankKiv.RowCount > 0)
            {
                tsUpdate.Enabled = true;
                tsDelete.Enabled = true;
            }
        }

        private void dgrLoad()
        {
            dtTetelek = new DataTable();

            scommandT = new SqlCommand("spSelectBankkiv_tetelek2", sconn);
            scommandT.CommandType = CommandType.StoredProcedure;
            scommandT.Parameters.Add(new SqlParameter("@id", SqlDbType.Int)).Value = int.Parse(txId.Text);

            daTetelek = new SqlDataAdapter(scommandT);
            Frissit();

            this.dgvBankKiv.DataSource = dtTetelek;
            RejtId();
            try
            {
                dgvBankKiv.CurrentCell = dgvBankKiv.Rows[dgvBankKiv.RowCount - 1].Cells[2];
            }
            catch
            {
                // üres grid
            }
        }

        public override void save()
        {
            saveOneRecord();
        }

        private void saveOneRecord()
        {
            if (txOsszeg.Text == string.Empty) { MessageBox.Show("Összeg nem lehet üres!"); txOsszeg.Focus(); return; }
            if (txSzamlaszam.Text == string.Empty) { MessageBox.Show("Számlaszám nem lehet üres!"); txSzamlaszam.Focus(); return; }
            if (txNev.Text == string.Empty) { MessageBox.Show("Név nem lehet üres!"); txNev.Focus(); return; }
            int ossz = 0;
            try
            {
                ossz = int.Parse(txOsszeg.Text);
                if (ossz == 0) throw new Exception();
            }
            catch
            {
                MessageBox.Show("Összeg mező hibás értéket tartalmaz!");
                return;
            }

            if (txBtlId.Text == string.Empty)
            {
                scommand = new SqlCommand("spBankkiv_tetelekInsert", sconn);
                scommand.CommandType = CommandType.StoredProcedure;
                scommand.Parameters.Add(new SqlParameter("@id", SqlDbType.Int)).Value = 1;
                scommand.Parameters.Add(new SqlParameter("@bkt_id", SqlDbType.Int)).Value = int.Parse(txId.Text);
                scommand.Parameters.Add(new SqlParameter("@osszeg", SqlDbType.Int)).Value = int.Parse(txOsszeg.Text);
                scommand.Parameters.Add(new SqlParameter("@erteknap", SqlDbType.Date)).Value = DateTime.Parse(datErteknap.Text.ToString());
                scommand.Parameters.Add(new SqlParameter("@sorszam", SqlDbType.Int)).Value = int.Parse(txSorszam.Text);
                scommand.Parameters.Add(new SqlParameter("@szamlaszam", SqlDbType.VarChar, 24)).Value = txSzamlaszam.Text;
                scommand.Parameters.Add(new SqlParameter("@nev", SqlDbType.VarChar, 50)).Value = txNev.Text;
                scommand.Parameters.Add(new SqlParameter("@kozlemeny", SqlDbType.VarChar, 255)).Value = txKozlemeny.Text;
                scommand.Parameters.Add(new SqlParameter("@megjegyzes", SqlDbType.VarChar, 255)).Value = txMegjegyzes.Text;
                scommand.Parameters.Add(new SqlParameter("@modosit_neve", SqlDbType.VarChar, 80)).Value = User.name;

                int idOK = 0;
                try
                {
                    if (sconn.State == ConnectionState.Closed) sconn.Open();
                    idOK = (Int32)scommand.ExecuteScalar();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("SQL Hiba " + ex.Message + ex.Data + "insert");
                    TraceBejegyzes(ex.Message);
                }

                if (idOK == 0)
                {
                    MessageBox.Show("A rekord mentése nem sikerült!");
                    return;
                }
                else 
                    txBtlId.Text = idOK.ToString();
            }
            else
            {
                scommand = new SqlCommand("spBankkiv_tetelekUpdate", sconn);
                scommand.CommandType = CommandType.StoredProcedure;
                scommand.Parameters.Add(new SqlParameter("@id", SqlDbType.Int)).Value = txBtlId.Text;
                scommand.Parameters.Add(new SqlParameter("@osszeg", SqlDbType.Int)).Value = int.Parse(txOsszeg.Text);
                scommand.Parameters.Add(new SqlParameter("@erteknap", SqlDbType.Date)).Value = DateTime.Parse(datErteknap.Text.ToString());
                scommand.Parameters.Add(new SqlParameter("@sorszam", SqlDbType.Int)).Value = int.Parse(txSorszam.Text);
                scommand.Parameters.Add(new SqlParameter("@szamlaszam", SqlDbType.VarChar, 24)).Value = txSzamlaszam.Text;
                scommand.Parameters.Add(new SqlParameter("@nev", SqlDbType.VarChar, 50)).Value = txNev.Text;
                scommand.Parameters.Add(new SqlParameter("@kozlemeny", SqlDbType.VarChar, 255)).Value = txKozlemeny.Text;
                scommand.Parameters.Add(new SqlParameter("@megjegyzes", SqlDbType.VarChar, 255)).Value = txMegjegyzes.Text;
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

            // grid frissítése
            dgrLoad();
            tsSave.Enabled = false;
            bSaveAndNext.Enabled = false;
        }

        public override void tsExit_Click(object sender, EventArgs e)
        {
            if (tsSave.Enabled)
            {
                DialogResult dr = MessageBox.Show("Minden nem mentett adat el fog veszni! Biztosan kilép?", "Megerősítés", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
                if (dr == DialogResult.OK)
                {
                    Close();
                }
            }
            else Close();
        }

        public override void tsDelete_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("Biztos törli a kijelölt rekordot? ",
                "Törlés?", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dr == DialogResult.Yes)
            {
                if (txStatus.Text != "K")
                {
                    // id meghatározása
                    int id = int.Parse(dgvBankKiv.Rows[Sorindex].Cells[0].Value.ToString());
                    string query1 = "delete from bankkivonat_tetelek where btl_id=" + id + ";";
                    scommand = new SqlCommand(query1, sconn);
                    // rekord törlése
                    try { scommand.ExecuteNonQuery(); }
                    catch (Exception ex) { MessageBox.Show("SQL Hiba " + ex.Message); return; }
                    // grid frissítése
                    dgrLoad();
                }
                else
                {
                    MessageBox.Show("A tétel könyvelt ezért nem törölhető!");
                }
            }
        }

        #region Dátum mezők lekezelése
        private void datErteknap_KeyDown(object sender, KeyEventArgs e)
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

        private void datErteknap_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Char.IsDigit(e.KeyChar))
            {
                if (datErteknap.TextLength >= 8) e.Handled = true;
            }
            else if (enter)
            {
                DateTransformA();
            }
            else if (back)
            {
                if (datErteknap.TextLength == 11)
                {
                    string text;
                    text = datErteknap.Text.Substring(0, 4) + datErteknap.Text.Substring(5, 2) + datErteknap.Text.Substring(8, 3);
                    datErteknap.Text = text;
                    datErteknap.Select(datErteknap.Text.Length, 0);
                }
            }
            else
                e.Handled = true;
        }

        private void DateTransformA()
        {
            if (datErteknap.TextLength == 8)
            {
                string text;
                text = datErteknap.Text.Substring(0, 4) + "." + datErteknap.Text.Substring(4, 2) + "." + datErteknap.Text.Substring(6, 2);
                try
                {
                    datErteknap.Text = DateTime.Parse(text).ToShortDateString();
                    tsSave.Enabled = true;
                }
                catch
                {
                    MessageBox.Show("Hibás dátum!");
                    datErteknap.Focus();
                }
            }
            else
            {
                if (datErteknap.TextLength != 11 && datErteknap.TextLength != 0)
                {
                    MessageBox.Show("Hibás dátum!");
                    datErteknap.Focus();
                }
            }
        }

        private void datErteknap_KeyUp(object sender, KeyEventArgs e)
        {
            back = false;
            enter = false;
        }

        private void datErteknap_Leave(object sender, EventArgs e)
        {
            DateTransformA();
            back = false;
            enter = false;
        }

        private void datLetrehoz_KeyDown(object sender, KeyEventArgs e)
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

        private void datLetrehoz_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Char.IsDigit(e.KeyChar))
            {
                if (datLetrehoz.TextLength >= 8) e.Handled = true;
            }
            else if (enter)
            {
                DateTransformA();
            }
            else if (back)
            {
                if (datLetrehoz.TextLength == 11)
                {
                    string text;
                    text = datLetrehoz.Text.Substring(0, 4) + datLetrehoz.Text.Substring(5, 2) + datLetrehoz.Text.Substring(8, 3);
                    datLetrehoz.Text = text;
                    datLetrehoz.Select(datLetrehoz.Text.Length, 0);
                }
            }
            else
                e.Handled = true;
        }

        private void DateTransformB()
        {
            if (datLetrehoz.TextLength == 8)
            {
                string text;
                text = datLetrehoz.Text.Substring(0, 4) + "." + datLetrehoz.Text.Substring(4, 2) + "." + datLetrehoz.Text.Substring(6, 2);
                try
                {
                    datLetrehoz.Text = DateTime.Parse(text).ToShortDateString();
                    tsSave.Enabled = true;
                }
                catch
                {
                    MessageBox.Show("Hibás dátum!");
                    datLetrehoz.Focus();
                }
            }
            else
            {
                if (datLetrehoz.TextLength != 11 && datLetrehoz.TextLength != 0)
                {
                    MessageBox.Show("Hibás dátum!");
                    datLetrehoz.Focus();
                }
            }
        }

        private void datLetrehoz_KeyUp(object sender, KeyEventArgs e)
        {
            back = false;
            enter = false;
        }

        private void datLetrehoz_Leave(object sender, EventArgs e)
        {
            DateTransformB();
            back = false;
            enter = false;
        }
        #endregion Dátum mezők lekezelése

        private void dgvBankKiv_SelectionChanged(object sender, EventArgs e)
        {
            try
            {
                Sorindex = dgvBankKiv.SelectedCells[0].RowIndex;
                txSorszam.Text = dgvBankKiv.Rows[Sorindex].Cells[2].Value.ToString();
                txSzamlaszam.Text = dgvBankKiv.Rows[Sorindex].Cells[4].Value.ToString();
                txNev.Text = dgvBankKiv.Rows[Sorindex].Cells[5].Value.ToString();
                txOsszeg.Text = dgvBankKiv.Rows[Sorindex].Cells[6].Value.ToString();
                txKozlemeny.Text = dgvBankKiv.Rows[Sorindex].Cells[7].Value.ToString();
                string konyv = dgvBankKiv.Rows[Sorindex].Cells[8].Value.ToString();
                txKonyveles.Text = (konyv != string.Empty ? DateTime.Parse(konyv).ToShortDateString() : string.Empty);
                txStatus.Text = dgvBankKiv.Rows[Sorindex].Cells[9].Value.ToString();
                txStorno.Text = dgvBankKiv.Rows[Sorindex].Cells[10].Value.ToString();
                txMegjegyzes.Text = dgvBankKiv.Rows[Sorindex].Cells[11].Value.ToString();
                txBtlId.Text = dgvBankKiv.Rows[Sorindex].Cells[0].Value.ToString();

                txOsszeg.ReadOnly = true;
                txSzamlaszam.ReadOnly = true;
                txNev.ReadOnly = true;
                txKozlemeny.ReadOnly = true;
                txKonyveles.ReadOnly = true;
                txStatus.ReadOnly = true;
                txStorno.ReadOnly = true;
                txMegjegyzes.ReadOnly = true;

                tsDelete.Enabled = true;
                tsFind.Enabled = false;
                tsNew.Enabled = true;
                tsSave.Enabled = false;
                tsSearch.Enabled = true;
                tsUpdate.Enabled = true;
                bSaveAndNext.Enabled = false;
            }
            catch
            {
            }
        }

        private void txKivonatSzama_Enter(object sender, EventArgs e)
        {
            txKivonatSzama.Select(0, txKivonatSzama.Text.Length);
        }

        private void BefizetesekRogz_Resize(object sender, EventArgs e)
        {
            dgvBankKiv.Width = BefizetesekRogz.ActiveForm.Width - 77;
        }

        private void dgvBankKiv_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
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

        private void TraceBejegyzes(string msg)
        {
            myListener.WriteLine("-" + DateTime.Now + ": " + msg);
            myListener.Flush();
            myListener.Close();
        }

        private void bUjkivonat_Click(object sender, EventArgs e)
        {
            Ujkivonat ujkiv = new Ujkivonat(sconn, this);
            ujkiv.ShowDialog(this);
        }

        private void bSaveAndNext_Click(object sender, EventArgs e)
        {
            saveOneRecord();
            // kiürít
            if (txBtlId.Text != string.Empty) NewRec();
        }

        private void dgvBankKiv_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                try
                {
                    e.SuppressKeyPress = true;
                    int iColumn = dgvBankKiv.CurrentCell.ColumnIndex;
                    int iRow = dgvBankKiv.CurrentCell.RowIndex;
                    if (iColumn != dgvBankKiv.Columns.Count - 1)
                        dgvBankKiv.CurrentCell = dgvBankKiv[iColumn, iRow];
                }
                catch
                { }
            }
        }

        private void txSzamlaszam_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Char.IsDigit(e.KeyChar) && !Char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void txSzamlaszam_Leave(object sender, EventArgs e)
        {
            if ((txSzamlaszam.Text != string.Empty) && !(IsSzlaOk(txSzamlaszam.Text))) { MessageBox.Show("Hibás számlaszám!"); txSzamlaszam.Focus(); return; }
        }

        public bool IsSzlaOk(string szamlaszam)
        {
            try
            {
                if (szamlaszam != null) return Regex.IsMatch(szamlaszam, MatchSzlaszamPattern);
                else return false;
            }
            catch
            {
                return false;
            }
        }

        private void txOsszeg_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Char.IsDigit(e.KeyChar) && !Char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void txOsszeg_TextChanged(object sender, EventArgs e)
        {
            bSaveAndNext.Enabled = true;
            tsSave.Enabled = true;
        }

        private void txSzamlaszam_TextChanged(object sender, EventArgs e)
        {
            bSaveAndNext.Enabled = true;
            tsSave.Enabled = true;
        }

        private void txNev_TextChanged(object sender, EventArgs e)
        {
            bSaveAndNext.Enabled = true;
            tsSave.Enabled = true;
        }

        private void dgvBankKiv_MouseMove(object sender, MouseEventArgs e)
        {
            DataGridView.HitTestInfo hit = dgvBankKiv.HitTest(e.X, e.Y);
            if (hit.Type == DataGridViewHitTestType.Cell)
            {
                dgvBankKiv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                dgvBankKiv.DefaultCellStyle.SelectionBackColor = Color.LightYellow;
                dgvBankKiv.DefaultCellStyle.SelectionForeColor = Color.DarkBlue;
            }
        }
    }
}
