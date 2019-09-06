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
    public partial class Beazonositas : VoluntaryPensionFundSystem.BaseForm
    {
        private SqlConnection sconn;
        private SqlCommand scommand, scommandT;
        private SqlDataAdapter da, daTetelek;
        private DataTable dt, dtTetelek;
        private bool back, enter = false;
        private bool KeziAzonositas = false;

        public int kivonatSzama;
        public string erteknap, letrehozas, irany, megjegyzes;
        public int bkt_id;

        public int FoglPnrid;
        public string adoszam, adoazon, megnev, ev, helyseg, cim, irszam;

        private List<string> lista = new List<string>();
        private int Sorindex;
        private int x = 0;
        private const string MatchSzlaszamPattern = @"^[0-9]{24}$";

        // Napló logok:
        TextWriterTraceListener myListener = new TextWriterTraceListener(@"LOG\Beazonositas.log", "myListener");

        public Beazonositas(SqlConnection SqlConn)
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

            if (sconn.State == ConnectionState.Closed) sconn.Open();

            datErteknap.ReadOnly = false;
            datLetrehoz.ReadOnly = false;
            txKivonatSzama.Enabled = true;
            cbIrany.Enabled = true;
            txMegjegyzes.ReadOnly = false;
            datErteknap.Text = string.Empty;
            datLetrehoz.Text = string.Empty;
            cbIrany.Text = string.Empty;
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

        private void Frissit()
        {
            //dt.Dispose();
            try
            {
                daTetelek.Fill(dtTetelek);
            }
            catch
            {
            }
            //btl_id,osszeg,pnr_id,erteknap,sorszam,szamlaszam,nev,kozlemeny,konyveles_datuma,statusz,storno,megjegyzes
            dtTetelek.Columns["osszeg"].ColumnName = "Összeg";
            dtTetelek.Columns["erteknap"].ColumnName = "Értéknap";
            dtTetelek.Columns["sorszam"].ColumnName = "Ssz.";
            dtTetelek.Columns["szamlaszam"].ColumnName = "Számlaszám";
            dtTetelek.Columns["nev"].ColumnName = "Név";
            dtTetelek.Columns["kozlemeny"].ColumnName = "Közlemény";
            dtTetelek.Columns["konyveles_datuma"].ColumnName = "Könyvelés dátuma";
            dtTetelek.Columns["statusz"].ColumnName = "Státusz";
            dtTetelek.Columns["storno"].ColumnName = "Stornó";
            dtTetelek.Columns["megjegyzes"].ColumnName = "Megjegyzés";
            dtTetelek.Columns["evnev"].ColumnName = "EV. Név";
            dtTetelek.Columns["pnr_id"].ColumnName = "Pnr Id";
            dtTetelek.Columns["adoszam"].ColumnName = "Adószám";
            dtTetelek.Columns["adoazonosito_jel"].ColumnName = "Adóazon. jel";
            dtTetelek.Columns["megnevezes"].ColumnName = "Megnevezés";
            dtTetelek.Columns["ir_szam"].ColumnName = "Ir.szám";
            dtTetelek.Columns["helyseg"].ColumnName = "Helység";
            dtTetelek.Columns["cim"].ColumnName = "Cím";
        }

        private void RejtId()
        {
            dgvBankKiv.Columns[0].Visible = false;
            dgvBankKiv.Columns[7].Visible = false;
            dgvBankKiv.Columns[8].Visible = false;
            dgvBankKiv.Columns[9].Visible = false;
            dgvBankKiv.Columns[10].Visible = false;
        }

        public override void tsSearch_Click(object sender, EventArgs e)
        {
            yellowMode();
        }

        public override void tsFind_Click(object sender, EventArgs e)
        {
            runQuery();
        }

        public override void tsSave_Click(object sender, EventArgs e)
        {
            save();
        }

        public override void save()
        {
            // ellenőrzés
            if (txPnrid.Text != string.Empty)
            {
                // Az adatok mentése
                x = Sorindex;
                string query = "UPDATE bankkivonat_tetelek SET pnr_id=" + txPnrid.Text + " WHERE btl_id=" + txBtlId.Text;
                scommand = new SqlCommand(query, sconn);
                try
                {
                    scommand.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    TraceBejegyzes(ex.Message);
                }
                tsSave.Enabled = false;
                txPnrid.ReadOnly = true;
                txAdoszam.ReadOnly = true;
                txAdoazon.ReadOnly = true;
                txMegnev.ReadOnly = true;
                txEV.ReadOnly = true;
                txHelyseg.ReadOnly = true;
                txCim.ReadOnly = true;
                txIrszam.ReadOnly = true;
                // datagrid betöltése
                dgrLoad();
                dgvBankKiv.CurrentCell = dgvBankKiv.Rows[x].Cells[1];
            }
            else
            {
                MessageBox.Show("Nincs menthető adat!");
            }
        }

        public override void yellowMode()
        {
            datErteknap.ReadOnly = false;
            datLetrehoz.ReadOnly = false;
            txKivonatSzama.Enabled = true;
            cbIrany.Enabled = true;
            //txMegjegyzes.ReadOnly = false;
            datErteknap.Text = string.Empty;
            datLetrehoz.Text = string.Empty;
            cbIrany.Text = string.Empty;
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

            txPnrid.Text = string.Empty;
            txAdoszam.Text = string.Empty;
            txAdoazon.Text = string.Empty;
            txMegnev.Text = string.Empty;
            txEV.Text = string.Empty;
            txHelyseg.Text = string.Empty;
            txCim.Text = string.Empty;
            txIrszam.Text = string.Empty;

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
            if (KeziAzonositas)                           // munkáltató rész
            {
                // A LEKÉRDEZÉS FELÉPÍTÉSE
                string query1 = "SELECT count(*)";
                string query2 = "SELECT pnr_id,adoszam,adoazonosito_jel,megnevezes,nev,ir_szam,helyseg,cim";
                string querystring = " FROM partnerek WHERE (pnr_tipus='GTG' or (pnr_tipus='SZMLY' and egyeni_vall='I')) and ";
                querystring = (txPnrid.Text != string.Empty ? querystring + "pnr_id like " + txPnrid.Text + " and " : querystring);
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

                    BeazonFoglKivalaszt bfk = new BeazonFoglKivalaszt(dt, this);
                    bfk.ShowDialog(this);

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

                txPnrid.ReadOnly = true;
                txAdoszam.ReadOnly = true;
                txAdoazon.ReadOnly = true;
                txMegnev.ReadOnly = true;
                txEV.ReadOnly = true;
                txHelyseg.ReadOnly = true;
                txCim.ReadOnly = true;
                txIrszam.ReadOnly = true;

                save();     // automatikus mentés
                KeziAzonositas = false;
            }
            else                                   // banki tétel rész
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

                    BankiKivonKivalasztB bkk = new BankiKivonKivalasztB(dt, this);
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

                txOsszeg.Focus();
                datErteknap.ReadOnly = true;
                datLetrehoz.ReadOnly = true;
                txKivonatSzama.Enabled = false;
                cbIrany.Enabled = false;

                tsFind.Enabled = false;
                tsSearch.Enabled = true;
                tsNew.Enabled = true;
                tsSave.Enabled = false;

                if (dgvBankKiv.RowCount > 0)
                {
                    tsUpdate.Enabled = true;
                    tsDelete.Enabled = true;
                }
            }
        }

        private void dgrLoad()
        {
            dtTetelek = new DataTable();

            scommandT = new SqlCommand("spSelectBankkiv_tetelek3", sconn);
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

        private void bAutoAzon_Click(object sender, EventArgs e)
        {
            // Automatikus azonosítás

            if (txPnrid.Text != string.Empty) { MessageBox.Show("A tétel már azonosításra került. Kézi azonosítással felülbírálható."); return; }
            
            // adószám keresése
            string kozlemenyString = txKozlemeny.Text;
            string adoszamstr = string.Empty;
            int adoszam;
            bool talalt = false;
            foreach (char x in kozlemenyString)
            {
                if (x <= '9' && x >= '0')
                {
                    adoszamstr += x;
                    if (adoszamstr.Length == 8)
                    {
                        talalt = true;
                        break;
                    }
                }
                else
                {
                    // ha nincs 8 egybefüggő számjegy, akkor nem lesz jó
                    if (adoszamstr.Length < 8)
                    {
                        adoszamstr = string.Empty;
                    }
                }
            }

            // ha találtunk egy 8 számjegy hosszú egybefüggő számsort, megpróbálom beazonosítani a munkáltatót
            int PartnerId = 0;
            if (talalt)
            {
                adoszam = int.Parse(adoszamstr);
                // egyértelműen beazonosítható-e?
                string query = "select count(pnr_id) from partnerek where adoszam like '" + adoszamstr + "%';";
                scommand = new SqlCommand(query, sconn);
                try
                {
                    PartnerId = (int)scommand.ExecuteScalar();
                }
                catch (Exception ex)
                {
                    TraceBejegyzes(ex.Message);
                }

                if (PartnerId == 1)
                {
                    // A munkáltatói adatok lekérdezése és betöltése
                    SqlDataReader myReader = null;
                    scommand = new SqlCommand("spFoglAdatai2", sconn);
                    scommand.CommandType = CommandType.StoredProcedure;
                    scommand.Parameters.Add(new SqlParameter("@adoszam", SqlDbType.VarChar, 15)).Value = adoszamstr + "%";
                    try
                    {
                        if (sconn.State == ConnectionState.Closed) sconn.Open();
                        myReader = scommand.ExecuteReader();
                        while (myReader.Read())
                        {
                            txPnrid.Text = myReader["pnr_id"].ToString();
                            //this.FoglId = (int)myReader["pnr_id"];
                            txMegnev.Text = myReader["megnevezes"].ToString();
                            txEV.Text = myReader["nev"].ToString();
                            txAdoszam.Text = myReader["adoszam"].ToString();
                            txEV.Text = myReader["nev"].ToString();
                            txHelyseg.Text = myReader["helyseg"].ToString();
                            txCim.Text = myReader["cim"].ToString();
                            txIrszam.Text = myReader["ir_szam"].ToString();
                        }
                    }
                    catch
                    {
                        MessageBox.Show("Hibás adat!");
                        //TraceBejegyzes(ex.Message);
                    }
                    myReader.Close();
                    // Az adatok mentése
                    // update bankkivonat_tetelek set pnr_id=txPnrid.Text where btl_id=txBtlId.Text
                    query = "UPDATE bankkivonat_tetelek SET pnr_id=" + txPnrid.Text + " WHERE btl_id=" + txBtlId.Text;
                    scommand = new SqlCommand(query, sconn);
                    try
                    {
                        scommand.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        TraceBejegyzes(ex.Message);
                    }
                }
                else if (PartnerId > 1)
                {
                    MessageBox.Show("Partner nem azonosítható egyértelműen!");
                }
                else if (PartnerId == 0)
                {
                    MessageBox.Show("Partner nem azonosítható!");
                }
            }
            else
            {
                MessageBox.Show("Partner nem azonosítható!");
            }

            txPnrid.ReadOnly = true;
            txAdoszam.ReadOnly = true;
            txAdoazon.ReadOnly = true;
            txMegnev.ReadOnly = true;
            txEV.ReadOnly = true;
            txHelyseg.ReadOnly = true;
            txCim.ReadOnly = true;
            txIrszam.ReadOnly = true;
            tsSave.Enabled = false;
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

        private void TraceBejegyzes(string msg)
        {
            myListener.WriteLine("-" + DateTime.Now + ": " + msg);
            myListener.Flush();
            myListener.Close();
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
                txSorszam.Text = dgvBankKiv.Rows[Sorindex].Cells[1].Value.ToString();
                txSzamlaszam.Text = dgvBankKiv.Rows[Sorindex].Cells[3].Value.ToString();
                txNev.Text = dgvBankKiv.Rows[Sorindex].Cells[4].Value.ToString();
                txOsszeg.Text = dgvBankKiv.Rows[Sorindex].Cells[5].Value.ToString();
                txKozlemeny.Text = dgvBankKiv.Rows[Sorindex].Cells[6].Value.ToString();
                string konyv = dgvBankKiv.Rows[Sorindex].Cells[7].Value.ToString();
                txKonyveles.Text = (konyv != string.Empty ? DateTime.Parse(konyv).ToShortDateString() : string.Empty);
                txStatus.Text = dgvBankKiv.Rows[Sorindex].Cells[8].Value.ToString();
                txStorno.Text = dgvBankKiv.Rows[Sorindex].Cells[9].Value.ToString();
                txMegjegyzes.Text = dgvBankKiv.Rows[Sorindex].Cells[10].Value.ToString();
                txBtlId.Text = dgvBankKiv.Rows[Sorindex].Cells[0].Value.ToString();

                txPnrid.Text = dgvBankKiv.Rows[Sorindex].Cells[11].Value.ToString();
                txAdoszam.Text = dgvBankKiv.Rows[Sorindex].Cells[12].Value.ToString();
                txAdoazon.Text = dgvBankKiv.Rows[Sorindex].Cells[13].Value.ToString();
                txMegnev.Text = dgvBankKiv.Rows[Sorindex].Cells[14].Value.ToString();
                txEV.Text = dgvBankKiv.Rows[Sorindex].Cells[15].Value.ToString();
                txHelyseg.Text = dgvBankKiv.Rows[Sorindex].Cells[17].Value.ToString();
                txCim.Text = dgvBankKiv.Rows[Sorindex].Cells[18].Value.ToString();
                txIrszam.Text = dgvBankKiv.Rows[Sorindex].Cells[16].Value.ToString();

                txOsszeg.ReadOnly = true;
                txSzamlaszam.ReadOnly = true;
                txNev.ReadOnly = true;
                txKozlemeny.ReadOnly = true;
                txKonyveles.ReadOnly = true;
                txStatus.ReadOnly = true;
                txStorno.ReadOnly = true;
                txMegjegyzes.ReadOnly = true;

                txPnrid.ReadOnly = true;
                txAdoszam.ReadOnly = true;
                txAdoazon.ReadOnly = true;
                txMegnev.ReadOnly = true;
                txEV.ReadOnly = true;
                txHelyseg.ReadOnly = true;
                txCim.ReadOnly = true;
                txIrszam.ReadOnly = true;

                tsDelete.Enabled = true;
                tsFind.Enabled = false;
                tsNew.Enabled = true;
                tsSave.Enabled = false;
                tsSearch.Enabled = true;
                tsUpdate.Enabled = true;
            }
            catch
            {
            }
        }

        private void Beazonositas_Resize(object sender, EventArgs e)
        {
            dgvBankKiv.Width = Beazonositas.ActiveForm.Width - 77;
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

        private void bKeziAzon_Click(object sender, EventArgs e)
        {
            if (txPnrid.Text != string.Empty)
            {
                DialogResult dr = MessageBox.Show("Újraazonosítja a már beazonosított tételt?", "Megerősítés", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
                if (dr == DialogResult.OK)
                {
                    kaz();
                }
            }
            else
            {
                kaz();
            }
        }

        private void kaz()
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
            KeziAzonositas = true;

            tsFind.Enabled = true;
            tsSearch.Enabled = false;

            tsUpdate.Enabled = false;
            tsNew.Enabled = false;
            tsDelete.Enabled = false;
            tsSave.Enabled = false;
        }

        private void bUjfogl_Click(object sender, EventArgs e)
        {
            FoglUj cn = new FoglUj(sconn);
            cn.ShowDialog();

            try
            {
                if (int.Parse(cn.FoglId) != 0)      // ha a másik formon sikeres a mentés, átveszem az adatokat.
                {
                    txAdoszam.Text = cn.adoszam;
                    txMegnev.Text = cn.nev;
                    txAdoazon.Text = cn.Adoazon;
                    txIrszam.Text = cn.Irszam;
                    txHelyseg.Text = cn.Helyseg;
                    txCim.Text = cn.Cim;
                    txMegjegyzes.Text = cn.Megjegyzes;
                    txPnrid.Text = cn.FoglId;

                    txMegnev.ReadOnly = true;
                    txAdoszam.ReadOnly = true;
                    txCim.ReadOnly = true;
                    txHelyseg.ReadOnly = true;
                    txIrszam.ReadOnly = true;
                    txAdoazon.ReadOnly = true;
                }
            }
            catch
            {
                // nem történt módosítás
            }
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
