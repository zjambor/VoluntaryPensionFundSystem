using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Windows;
using System.Configuration;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Threading;

namespace VoluntaryPensionFundSystem
{
    public partial class BevRogzLemezes : VoluntaryPensionFundSystem.BaseForm
    {
        private SqlConnection sconn;
        private SqlCommand scommand;
        private Stream myStream = null;
        private StreamReader s;

        public int PnrId, FoglId;
        public string megnevezes, irszam, foglcim, helyseg, telszam, adoszam;
        public string nev, adoazon, pnrid;

        private int Tagid, Ev_ho, idk_id, Bev_id;
        private bool back, enter = false;
        private int SajatSum = 0, MunkSum = 0, RendszSum = 0, EgyszeriSum = 0, TotalSum = 0;
        private int TranID = 0;
        private bool Fejhiba = false, Sorhiba = false;
        private DateTime mainap = DateTime.Now;
        
        private string sor;
        private string[] t;
        private int rownum = 0;
        private List<Tag> tagok = new List<Tag>();

        private TextWriterTraceListener myListener = new TextWriterTraceListener(@"LOG\LemezesFeldolgozas.log", "myListener");

        public BevRogzLemezes(SqlConnection SqlConn)
        {
            InitializeComponent();

            this.sconn = SqlConn;
            if (sconn.State == ConnectionState.Closed) sconn.Open();

            txIktKelte.Text = mainap.ToString("yyyy.MM.dd.");
            bFeldolg.Enabled = false;

            tsDelete.Enabled = false;
            tsFind.Enabled = false;
            tsNew.Enabled = false;
            tsSave.Enabled = false;
            tsSearch.Enabled = false;
            tsUpdate.Enabled = false;
        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {

        }

        public override void tsNew_Click(object sender, EventArgs e)
        {
            createNew();
        }

        public override void tsSave_Click(object sender, EventArgs e)
        {
            
        }

        public override void tsUpdate_Click(object sender, EventArgs e)
        {
            
        }

        public override void tsDelete_Click(object sender, EventArgs e)
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
            Close();
        }

        private void bFileOpen_Click(object sender, EventArgs e)
        {
            openFileDialog1.InitialDirectory = "c:\\";
            //openFileDialog1.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            openFileDialog1.Filter = "txt files (*.txt)|*.txt";
            openFileDialog1.FilterIndex = 2;
            openFileDialog1.RestoreDirectory = true;
            openFileDialog1.FileName = string.Empty;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    if ((myStream = openFileDialog1.OpenFile()) != null)
                    {
                        txPath.Text = openFileDialog1.FileName;
                        bFeldolg.Enabled = true;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Hiba: A fájl nem olvasható! " + ex.Message);
                }
            }
        }

        private int HibaBeszurasa(int hibaID)
        {
            scommand = new SqlCommand("spAnalitikaHibaInsert", sconn);
            scommand.CommandType = CommandType.StoredProcedure;
            scommand.Parameters.Add(new SqlParameter("@ahb_id", SqlDbType.Int)).Value = 1;
            scommand.Parameters.Add(new SqlParameter("@hiba_id", SqlDbType.Int)).Value = hibaID;
            scommand.Parameters.Add(new SqlParameter("@bev_id", SqlDbType.Int)).Value = Bev_id;
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
            scommand.Parameters.Add(new SqlParameter("@bev_id", SqlDbType.Int)).Value = Bev_id;
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
            scommand.Parameters.Add(new SqlParameter("@bev_id", SqlDbType.Int)).Value = Bev_id;
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

        private void bFeldolg_Click(object sender, EventArgs e)
        {
            // Form ellenőrzések
            Fejhiba = false;

            if (txVonIdoszak.Text == string.Empty) { MessageBox.Show("Vonatkozási időszak nem lehet üres!"); txVonIdoszak.Focus(); return; }
            if (txErkSorsz.Text == string.Empty) { MessageBox.Show("Érkezés sorszáma nem lehet üres!"); txErkSorsz.Focus(); return; }
            if (txErkDate.Text == string.Empty) { MessageBox.Show("Érkezés dátuma nem lehet üres!"); txErkDate.Focus(); return; }
            if (txIktatoszam.Text == string.Empty) { MessageBox.Show("Iktatószám nem lehet üres!"); txIktatoszam.Focus(); return; }
            if (txAdkozlDate.Text == string.Empty) { MessageBox.Show("Adatközlés dátuma nem lehet üres!"); txAdkozlDate.Focus(); return; }
            if (txIktKelte.Text == string.Empty) { MessageBox.Show("Iktatás kelte nem lehet üres!"); txIktKelte.Focus(); return; }

            // File feldolgozás indul
            try
            {
                if (myStream != null)
                {
                    using (myStream)
                    {
                        s = new StreamReader(myStream, false);
                        while (s.Peek() > 0)
                        {
                            sor = s.ReadLine();
                            
                            if (rownum < 1)
                            {
                                FejreszFeldolgozasa();
                                if (this.Bev_id == 0)
                                {
                                    MessageBox.Show("A rekord mentése nem sikerült!");
                                    return;
                                }
                                rownum++;
                            }
                            else
                            {
                                SorokFeldolgozasa();
                                BevRogzLemezes.ActiveForm.Refresh();
                                rownum++;
                            }
                        }

                        // Bevallás érvényesség ellenőrzése
                        BevEllenorzese();
                        // tranzakció commit
                        if (TranID != 0)
                        {
                            TransactionCommit();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hiba: A fájl nem olvasható! " + ex.Message);
            }
        }

        private void FejreszFeldolgozasa()
        {
            t = sor.Split(';');
            adoszam = t[2].ToString();
            SqlDataReader myReader = null;
            // Fogl azonosítása
            string query = "select count(pnr_id) from partnerek where adoszam like '" + adoszam.Substring(0, 8) + "%';";
            scommand = new SqlCommand(query, sconn);

            int rowCount = (int)scommand.ExecuteScalar();
            if (rowCount != 1) { MessageBox.Show("Partner nem azonosítható!"); bFeldolg.Enabled = false; return; }

            scommand = new SqlCommand("spFoglAdatai2", sconn);
            scommand.CommandType = CommandType.StoredProcedure;
            scommand.Parameters.Add(new SqlParameter("@adoszam", SqlDbType.VarChar, 15)).Value = adoszam.Substring(0, 8) + "%";
            try
            {
                if (sconn.State == ConnectionState.Closed) sconn.Open();
                myReader = scommand.ExecuteReader();
                while (myReader.Read())
                {
                    txPnrid.Text = myReader["pnr_id"].ToString();
                    this.FoglId = int.Parse(myReader["pnr_id"].ToString());
                    txMegnev.Text = myReader["megnevezes"].ToString();
                    txEV.Text = myReader["nev"].ToString();
                    txAdoszam.Text = myReader["adoszam"].ToString();
                    txAdoazonJel.Text = myReader["adoazonosito_jel"].ToString();
                    txHelyseg.Text = myReader["helyseg"].ToString();
                    txCim.Text = myReader["cim"].ToString();
                    txIrszam.Text = myReader["ir_szam"].ToString();
                }
            }
            catch
            {
                MessageBox.Show("Hibás adat az 1. sorban!");
                //TraceBejegyzes(ex.Message);
            }

            txVonIdoszak.Text = t[3].ToString();
            txTagokSum.Value = int.Parse(t[4].ToString());
            txSajatSum.Value = int.Parse(t[5].ToString());
            txMunkSum.Value = int.Parse(t[6].ToString());
            TxEgyszeriSum.Value = int.Parse(t[7].ToString());
            txRendszSum.Value = int.Parse(t[8].ToString());
            TxTotal.Value = txSajatSum.Value + txMunkSum.Value + TxEgyszeriSum.Value + txRendszSum.Value;
            myReader.Close();

            // adatok ellenőrzése
            FejEll(t[3].ToString());

            // tranzakció start
            if (TranID == 0)
            {
                TranID = int.Parse(txPnrid.Text);
                TransactionBegin();                // BEGIN TRAN
            }

            // db insert
            this.Bev_id = (int)InsertFejresz();
        }

        private void FejEll(string Vonidoszak)
        {
            // vonatkozási időszak
            if (txVonIdoszak.TextLength == 6)
            {
                this.Ev_ho = int.Parse(Vonidoszak);
                scommand = new SqlCommand("spIdoszak1", sconn);
                scommand.CommandType = CommandType.StoredProcedure;
                scommand.Parameters.Add(new SqlParameter("@ev_ho", SqlDbType.Int)).Value = this.Ev_ho;
                try
                {
                    if (sconn.State == ConnectionState.Closed) sconn.Open();
                    string idkid2 = scommand.ExecuteScalar().ToString();
                    this.idk_id = int.Parse(idkid2);
                }
                catch
                {
                    MessageBox.Show("Hibás időszak!");
                    txVonIdoszak.Focus();
                    return;
                }
            }
            else
            {
                MessageBox.Show("Hibás időszak!");
                txVonIdoszak.Focus();
                return;
            }
        }

        private void SorokFeldolgozasa()
        {
            Sorhiba = false;
            t = sor.Split(';');
            if (t[0].ToString() == string.Empty) { MessageBox.Show("Adóazonosító jel hiányzik! (" + rownum + ". sor)"); bFeldolg.Enabled = false; return; }

            Tag ujtag = new Tag();
            ujtag.adoazon = t[0].ToString();
            ujtag.tag_pnr_id = TagEll(ujtag.adoazon);
            if (ujtag.tag_pnr_id == 0)
            {
                // visszagörgetés
                if (TranID != 0)
                {
                    TransactionRollback();
                }
                // alaphelyzet
                openFileDialog1.FileName = string.Empty;
                bFeldolg.Enabled = false;
                // leáll
                return;
            }

            try
            {
                ujtag.sajat = int.Parse(t[1].ToString());
                SajatSum += ujtag.sajat;
                txSajatSumC.Text = SajatSum.ToString();

                ujtag.mh = int.Parse(t[2].ToString());
                MunkSum += ujtag.mh;
                txMunkSumC.Text = MunkSum.ToString();

                ujtag.egyszeri = int.Parse(t[3].ToString());
                EgyszeriSum += ujtag.egyszeri;
                TxEgyszeriSumC.Text = EgyszeriSum.ToString();

                ujtag.rendszeres = int.Parse(t[4].ToString());
                RendszSum += ujtag.rendszeres;
                txRendszSumC.Text = RendszSum.ToString();
            }
            catch
            {
                MessageBox.Show("Hibás összeg: " + rownum + ". sor!");
                // visszagörgetés
                if (TranID != 0)
                {
                    TransactionRollback();
                }
                // alaphelyzet
                openFileDialog1.FileName = string.Empty;
                bFeldolg.Enabled = false;
                // leáll
                return;
            }

            TotalSum = SajatSum + MunkSum + EgyszeriSum + RendszSum;
            TxTotalC.Text = TotalSum.ToString();

            tagok.Add(ujtag);
            txTagokSumC.Text = tagok.Count.ToString();

            // db insert
            int forgid = InsertSor(ujtag);
            if (forgid == 0)
            {
                MessageBox.Show("Sor rögzítése sikertelen: " + rownum + ". sor!");
                // visszagörgetés
                if (TranID != 0)
                {
                    TransactionRollback();
                }
                // alaphelyzet
                openFileDialog1.FileName = string.Empty;
                bFeldolg.Enabled = false;
                // leáll
                return;
            }
            
            RowVerification(ujtag, forgid);

            // szerződési feltételek ellenőrzése
            //munkáltatói hj
            if (ujtag.mh > 0)
            {
                mhEll(ujtag, forgid);
                // 
            }
            // támogatás
            if (ujtag.rendszeres > 0)
            {
                mhEll(ujtag, forgid);
                // 
            }
            if (Sorhiba)
            {
                ujtag.ervenyes = "N";
                UpdateSor(ujtag, forgid);
            }
        }

        private int TagEll(string adoazonosito)
        {
            // tag ellenőrzése
            scommand = new SqlCommand("spTagEll", sconn);
            scommand.CommandType = CommandType.StoredProcedure;
            scommand.Parameters.Add(new SqlParameter("@adoazonosito_jel", SqlDbType.BigInt)).Value = adoazonosito;
            try
            {
                if (sconn.State == ConnectionState.Closed) sconn.Open();
                SqlDataReader myReader = scommand.ExecuteReader();
                int db = 0;
                while (myReader.Read())
                {
                    IDataRecord record = (IDataRecord)myReader;
                    Tagid = int.Parse(record[0].ToString());
                    db++;
                }
                if (db == 0)
                {
                    MessageBox.Show("Pénztártag nem azonosítható: " + rownum + ".sor!");
                    myReader.Close();
                    return 0;
                }
                myReader.Close();
                return Tagid;
            }
            catch (Exception ex)
            {
                MessageBox.Show("SQL Hiba " + ex.Message);
                TraceBejegyzes(ex.Message);
                // visszagörgetés
                if (TranID != 0)
                {
                    TransactionRollback();
                }
                // alaphelyzet
                openFileDialog1.FileName = string.Empty;
                bFeldolg.Enabled = false;
                return 0;
            }
        }

        private void RowVerification(Tag ujtag, int forg_id)
        {
            // Munkaviszony ellenőrzés
            int fsucc = 0;      // ez a változó tárolja, hogy a hiba beszúrás sikeres volt-e
            int count = 0;
            int tszs_id = 0;
            int tag_pnr_id = ujtag.tag_pnr_id;
            this.Ev_ho = int.Parse(txVonIdoszak.Text);
            this.Tagid = tag_pnr_id;
            Sorhiba = false;
            try
            {
                scommand = new SqlCommand("spTszsLekerd", sconn);               // kell a tagsagi szerzodes id a munkaviszony ellenőrzéshez
                scommand.CommandType = CommandType.StoredProcedure;
                scommand.Parameters.Add(new SqlParameter("@pnr_id", SqlDbType.Int)).Value = tag_pnr_id;
                tszs_id = int.Parse(scommand.ExecuteScalar().ToString());
            }
            catch (Exception ex)
            {
                MessageBox.Show("SQL Hiba " + ex.Message);
                TraceBejegyzes(ex.Message);
                // visszagörgetés
                if (TranID != 0)
                {
                    TransactionRollback();
                }
                // alaphelyzet
                openFileDialog1.FileName = string.Empty;
                bFeldolg.Enabled = false;
                // hiba esetén nem megy tovább
                return;
            }

            // FUNCTION futtatása

            scommand = new SqlCommand("SELECT dbo.FuncMvEll2(@tszs_id, @pnr_id, @ev_ho)", sconn);
            scommand.Parameters.Add(new SqlParameter("@tszs_id", SqlDbType.Int)).Value = tszs_id;
            scommand.Parameters.Add(new SqlParameter("@pnr_id", SqlDbType.Int)).Value = this.FoglId;
            scommand.Parameters.Add(new SqlParameter("@ev_ho", SqlDbType.Int)).Value = this.Ev_ho;
            try
            {
                count = int.Parse(scommand.ExecuteScalar().ToString());
            }
            catch (Exception ex)
            {
                MessageBox.Show("SQL Hiba " + ex.Message);
                TraceBejegyzes(ex.Message);
                // visszagörgetés
                if (TranID != 0)
                {
                    TransactionRollback();
                }
                // alaphelyzet
                openFileDialog1.FileName = string.Empty;
                bFeldolg.Enabled = false;
            }
            // Az FV kiértékelése
            switch (count)
            {
                case 1: scommand = new SqlCommand("spMvInsert2", sconn);
                    scommand.CommandType = CommandType.StoredProcedure;
                    scommand.Parameters.Add(new SqlParameter("@tszs_id", SqlDbType.Int)).Value = tszs_id;
                    scommand.Parameters.Add(new SqlParameter("@pnr_id", SqlDbType.Int)).Value = this.FoglId;
                    scommand.Parameters.Add(new SqlParameter("@ev_ho", SqlDbType.Int)).Value = this.Ev_ho;
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
                    scommand.Parameters.Add(new SqlParameter("@pnr_id", SqlDbType.Int)).Value = this.FoglId;
                    scommand.Parameters.Add(new SqlParameter("@ev_ho", SqlDbType.Int)).Value = this.Ev_ho;
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

            // Sor ellenőrzések
            // Már van erre az időszakra a fogl.-tól, nem stornózott bevallás!  7100027
            int hibaID = 7100027;
            scommand = new SqlCommand("spForgSorVanE", sconn);
            scommand.CommandType = CommandType.StoredProcedure;
            scommand.Parameters.Add(new SqlParameter("@fogl_id", SqlDbType.Int)).Value = this.FoglId;
            scommand.Parameters.Add(new SqlParameter("@ev_ho", SqlDbType.Int)).Value = this.Ev_ho;
            scommand.Parameters.Add(new SqlParameter("@pnr_id", SqlDbType.Int)).Value = this.Tagid;
            scommand.Parameters.Add(new SqlParameter("@bev_id", SqlDbType.Int)).Value = this.Bev_id;
            // select count(f.forg_id) from forgalmak f, idoszakok i, bevallasok b where f.pnr_id=@pnr_id and f.idk_id=i.idk_id and b.bev_id=f.bev_id and i.ev_ho=@ev_ho and f.ervenyes='I'
            // and b.pnr_id=@fogl_id
            int check = 0;
            try
            {
                check = int.Parse(scommand.ExecuteScalar().ToString());
            }
            catch (Exception ex)
            {
                MessageBox.Show("SQL Hiba " + ex.Message);
                TraceBejegyzes(ex.Message);
                // visszagörgetés
                if (TranID != 0)
                {
                    TransactionRollback();
                }
                // alaphelyzet
                openFileDialog1.FileName = string.Empty;
                bFeldolg.Enabled = false;
            }
            if (check > 0)
            {
                fsucc = SorHibaBeszurasa(hibaID, forg_id);
                Sorhiba = true;
            }
            else
            {
                Megoldas(hibaID, forg_id);
            }
        }

        private int InsertFejresz()
        {
            this.Bev_id = 0; 
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
            scommand.Parameters.Add(new SqlParameter("@idk_id", SqlDbType.Int)).Value = this.idk_id;
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
                this.Bev_id = (int)scommand.ExecuteScalar();
            }
            catch (Exception ex)
            {
                MessageBox.Show("SQL hiba: " + ex.Message);
                TraceBejegyzes(ex.Message);
                // visszagörgetés
                if (TranID != 0)
                {
                    TransactionRollback();
                }
                // alaphelyzet
                openFileDialog1.FileName = string.Empty;
                bFeldolg.Enabled = false;
            }

            return this.Bev_id;
        }

        private int InsertSor(Tag ujtag)
        {
            scommand = new SqlCommand("spForgalmakInsert", sconn);
            scommand.CommandType = CommandType.StoredProcedure;
            scommand.Parameters.Add(new SqlParameter("@id", SqlDbType.Int)).Value = 1;
            scommand.Parameters.Add(new SqlParameter("@idk_id", SqlDbType.Int)).Value = this.idk_id;
            scommand.Parameters.Add(new SqlParameter("@bev_id", SqlDbType.Int)).Value = this.Bev_id;
            scommand.Parameters.Add(new SqlParameter("@pnr_id", SqlDbType.Int)).Value = ujtag.tag_pnr_id;
            scommand.Parameters.Add(new SqlParameter("@ervenyes", SqlDbType.VarChar, 1)).Value = ujtag.ervenyes;
            scommand.Parameters.Add(new SqlParameter("@sajat", SqlDbType.Int)).Value = ujtag.sajat;
            scommand.Parameters.Add(new SqlParameter("@hozzajarulas", SqlDbType.Int)).Value = ujtag.mh;
            scommand.Parameters.Add(new SqlParameter("@rend_tamog", SqlDbType.Int)).Value = ujtag.rendszeres;
            scommand.Parameters.Add(new SqlParameter("@egysz_tamog", SqlDbType.Int)).Value = ujtag.egyszeri;
            scommand.Parameters.Add(new SqlParameter("@befizetendo", SqlDbType.Int)).Value = ujtag.sajat + ujtag.mh + ujtag.rendszeres + ujtag.egyszeri;
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
                // visszagörgetés
                if (TranID != 0)
                {
                    TransactionRollback();
                }
                // alaphelyzet
                openFileDialog1.FileName = string.Empty;
                bFeldolg.Enabled = false;
                return 0;
            }
            return 1;
        }

        private int UpdateSor(Tag ujtag, int fsid)
        {
            scommand = new SqlCommand("spForgalmakUpdate", sconn);
            scommand.CommandType = CommandType.StoredProcedure;
            scommand.Parameters.Add(new SqlParameter("@id", SqlDbType.Int)).Value = fsid;
            scommand.Parameters.Add(new SqlParameter("@idk_id", SqlDbType.Int)).Value = this.idk_id;
            scommand.Parameters.Add(new SqlParameter("@bev_id", SqlDbType.Int)).Value = this.Bev_id;
            scommand.Parameters.Add(new SqlParameter("@pnr_id", SqlDbType.Int)).Value = ujtag.tag_pnr_id;
            scommand.Parameters.Add(new SqlParameter("@ervenyes", SqlDbType.VarChar, 1)).Value = ujtag.ervenyes;
            scommand.Parameters.Add(new SqlParameter("@sajat", SqlDbType.Int)).Value = ujtag.sajat;
            scommand.Parameters.Add(new SqlParameter("@hozzajarulas", SqlDbType.Int)).Value = ujtag.mh;
            scommand.Parameters.Add(new SqlParameter("@rend_tamog", SqlDbType.Int)).Value = ujtag.rendszeres;
            scommand.Parameters.Add(new SqlParameter("@egysz_tamog", SqlDbType.Int)).Value = ujtag.egyszeri;
            scommand.Parameters.Add(new SqlParameter("@befizetendo", SqlDbType.Int)).Value = ujtag.sajat + ujtag.mh + ujtag.rendszeres + ujtag.egyszeri;
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
                // visszagörgetés
                if (TranID != 0)
                {
                    TransactionRollback();
                }
                // alaphelyzet
                openFileDialog1.FileName = string.Empty;
                bFeldolg.Enabled = false;
                return 0;
            }
            return 1;
        }

        private int mhEll(Tag ujtag, int forg_id)
        {
            int hozzajarulas = 0;
            int szazalek;
            char cafeteria;
            int min_munkaviszony;
            int hiba_id;
            DateTime hatalybalepes;

            // select hozzajarulas,szazalek,cafeteria,min_munkaviszony,hatalybalepes from munkaltatoi_szerzodesek where pnr_id=@pnr_id
            scommand = new SqlCommand("spMunkSzerzSelect", sconn);
            scommand.CommandType = CommandType.StoredProcedure;
            scommand.Parameters.Add(new SqlParameter("@pnr_id", SqlDbType.Int)).Value = this.FoglId;    // ??
            try
            {
                if (sconn.State == ConnectionState.Closed) sconn.Open();
                SqlDataReader myReadermh = scommand.ExecuteReader();
                int db = 0;
                while (myReadermh.Read())
                {
                    IDataRecord record = (IDataRecord)myReadermh;
                    hozzajarulas = int.Parse(record[0].ToString());
                    szazalek = int.Parse(record[0].ToString());
                    cafeteria = char.Parse(record[0].ToString());
                    min_munkaviszony = int.Parse(record[0].ToString());
                    hatalybalepes = DateTime.Parse(record[0].ToString());
                    db++;
                }
                if (db == 0)
                {
                    // nincs munkáltatói szerződés
                    hiba_id = 7100046;
                    SorHibaBeszurasa(hiba_id, forg_id);
                    Sorhiba = true;
                    myReadermh.Close();
                    return 1;
                }

                // összegszerű ellenőrzés !!
                if (hozzajarulas > 0 && ujtag.mh != hozzajarulas)
                {
                    Sorhiba = true;
                }
                myReadermh.Close();
                return Tagid;
            }
            catch (Exception ex)
            {
                MessageBox.Show("SQL Hiba " + ex.Message);
                TraceBejegyzes(ex.Message);
                // visszagörgetés
                if (TranID != 0)
                {
                    TransactionRollback();
                }
                // alaphelyzet
                openFileDialog1.FileName = string.Empty;
                bFeldolg.Enabled = false;
                return 0;
            }
        }

        private void BevEllenorzese()
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

            if (Fejhiba)
            {
                txErv.Text = "N";
                txErv.BackColor = Color.Red;
            }
            else
            {
                txErv.Text = "I";
                txErv.BackColor = Color.LightGreen;
            }


            // Bevallás érvényesség módosítása spBevErvUpdate
            // update bevallasok set ervenyes=txErv.Text where bev_id=@bev_id
            scommand = new SqlCommand("spBevErvUpdate", sconn);
            scommand.CommandType = CommandType.StoredProcedure;
            scommand.Parameters.Add(new SqlParameter("@bev_id", SqlDbType.Int)).Value = this.Bev_id;
            scommand.Parameters.Add(new SqlParameter("@ervenyes", SqlDbType.VarChar, 1)).Value = txErv.Text;
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
                // visszagörgetés
                if (TranID != 0)
                {
                    TransactionRollback();
                }
                // alaphelyzet
                openFileDialog1.FileName = string.Empty;
                bFeldolg.Enabled = false;
                return;
            }
        }

        private void TraceBejegyzes(string msg)
        {
            myListener.WriteLine("-" + DateTime.Now + ": " + msg);
            myListener.Flush();
            myListener.Close();
        }

        private void txIktatoszam_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Char.IsDigit(e.KeyChar) && !Char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void txErkSorsz_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Char.IsDigit(e.KeyChar) && !Char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        #region Tranzakciók kezelése
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
        #endregion

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

            if (txAdkozlDate.Text != string.Empty)
            {
                if (DateTime.Parse(txAdkozlDate.Text) > DateTime.Parse(txErkDate.Text))
                {
                    MessageBox.Show("Az adatközlés dátuma nem lehet nagyobb az érkezés dátumánál!");
                    txAdkozlDate.Focus();
                }
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
        #endregion

        private void bHibak_Click(object sender, EventArgs e)
        {
            AnalitikaHibak ah = new AnalitikaHibak(sconn, Bev_id);
            ah.ShowDialog();
        }
    }

    public class Tag
    {
        public string adoazon;
        public int tag_pnr_id;
        public int sajat;
        public int mh;
        public int egyszeri;
        public int rendszeres;
        public string ervenyes;

        public Tag()
        {
            adoazon = string.Empty;
            sajat = 0;
            mh = 0;
            egyszeri = 0;
            rendszeres = 0;
            ervenyes = "I";
        }
    }
}
