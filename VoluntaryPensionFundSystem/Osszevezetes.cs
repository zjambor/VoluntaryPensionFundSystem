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
using System.Timers;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;

namespace VoluntaryPensionFundSystem
{
    public partial class Osszevezetes : VoluntaryPensionFundSystem.BaseForm
    {
        private SqlConnection sconn;
        private SqlCommand scommand;
        private SqlDataReader myReader;
        //System.Timers.Timer myTimer = new System.Timers.Timer();
        //Thread t;
        //private int szamlalo = 0;
        private int counter = 0;
        private int k = 0;
        private int foglid;
        private int darab;
        private string sql;
        private int TranID = 0;
        int hivatkozasi_szam;
        private string sajatsql, munksql, rendszsql, egyszsql, sajatgenykod, munkgenykod, rendszgenykod, egyszgenykod;
        private BackgroundWorker bw = new BackgroundWorker();

        private TextWriterTraceListener myListener = new TextWriterTraceListener(@"LOG\Osszevezetes.log", "myListener");
        Random rnd = new Random();

        private List<string> PnridList = new List<string>();
        private List<string> PnridListbef = new List<string>();
        private List<string> BevList = new List<string>();
        private List<string> BefList = new List<string>();

        public Osszevezetes(SqlConnection SqlConn)
        {
            InitializeComponent();

            this.sconn = SqlConn;

            tsDelete.Enabled = false;
            tsFind.Enabled = false;
            tsNew.Enabled = false;
            tsSave.Enabled = false;
            tsSearch.Enabled = false;
            tsUpdate.Enabled = false;
            tsExit.Enabled = true;
            label1.Visible = false;
            sajatgenykod = "SAJATELOIRAS";
            munkgenykod = "MUNKHELOIRAS";
            rendszgenykod = "RENDSZELOIRAS";
            egyszgenykod = "EGYSZELOIRAS";

            if (sconn.State == ConnectionState.Closed) sconn.Open();

            bw.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(bw_ProgressChanged);
            bw.DoWork += new System.ComponentModel.DoWorkEventHandler(bw_DoWork);
            bw.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(bw_RunWorkerCompleted);
            bw.WorkerSupportsCancellation = true;
            bw.WorkerReportsProgress = true;
            bMegszakit.Enabled = false;
        }

        private void bIndit_Click(object sender, EventArgs e)
        {
            rtbMessage.Text += "Összevezetés indítása: " + DateTime.Now + "\n";
            label1.Visible = true;
            label1.Text = "Összevezetés folyamatban";
            tsExit.Enabled = false;
            bMegszakit.Enabled = true;
            bMegszakit.Focus();
            if (bw.IsBusy != true)
            {
                bw.RunWorkerAsync();
            }
        }

        private void bw_ProgressChanged(object source, ProgressChangedEventArgs e)
        {
            rtbMessage.Text += (e.UserState + "\n");
            rtbMessage.Refresh();
            //Thread.Sleep(100);

            rtbMessage.SelectionStart = rtbMessage.Text.Length;
            rtbMessage.ScrollToCaret();
            label1.Text = (e.ProgressPercentage.ToString() + "%");
            label1.Refresh();
            progressBar1.Value = e.ProgressPercentage;
        }

        private void bw_DoWork(object sender, DoWorkEventArgs e)
        {
            if ((bw.CancellationPending == true))
            {
                e.Cancel = true;
                return;
            }

            Osszevezet();
        }

        private void Osszevezet()
        {
            // FŐ TRANZAKCIÓ indítása
            string Query;
            Query = "BEGIN TRANSACTION main";
            scommand = new SqlCommand(Query, sconn);
            try { scommand.ExecuteNonQuery(); }
            catch (Exception ex) { MessageBox.Show("SQL Hiba " + ex.Message); return; }

            /* 1.	Az érvényes bevallások megkeresése és könyvelése */
            if (Bevallasok() > 0)
            {
                bw.CancelAsync();
                bw.ReportProgress(0, "Összevezetés visszagörgetése");
                if (TranID != 0) TransactionRollback();
                Megszakit();
                return;
            }

            /* 2.	A könyveletlen banki tételek megkeresése és könyvelése */
            if (BankiTetelek() > 0)
            {
                bw.CancelAsync();
                bw.ReportProgress(0, "Összevezetés visszagörgetése");
                if (TranID != 0) TransactionRollback();
                Megszakit();
                return;
            }

            /* 3.	A lekönyvelt bevallás munkáltatójához tartozó könyveletlen pénzek azonosítása és lekönyvelése, a tagdíjak leosztása */
            if (Leosztas() > 0)
            {
                bw.CancelAsync();
                bw.ReportProgress(0, "Összevezetés visszagörgetése");
                if (TranID != 0) TransactionRollback();
                Megszakit();
                return;
            }
            bw.ReportProgress(95, "COMMIT OK");
            bw.ReportProgress(100, "Az összevezetés befejeződött.");
        }

        private int Bevallasok()
        {
            // TRANZAKCIÓ indítása
            if (TranID == 0)
            {
                TranID++;
                TransactionBegin();                // BEGIN TRAN
            }

            bw.ReportProgress(10);
            /* 1.	Az érvényes bevallások megkeresése és könyvelése */
            // -	Partner id-k lekérdezése és betöltése listába
            // select pnr_id from bevallasok where konyvelt is null and ervenyes='I'

            k = 0;
            sql = "select distinct pnr_id from bevallasok where konyvelt is null and ervenyes='I' order by 1";
            scommand = new SqlCommand(sql, sconn);
            myReader = null;
            try
            {
                if (sconn.State == ConnectionState.Closed) sconn.Open();
                myReader = scommand.ExecuteReader();

                if (myReader.HasRows)
                {
                    while (myReader.Read())
                    {
                        PnridList.Add(myReader["pnr_id"].ToString());
                        bw.ReportProgress(10, PnridList[k]);
                        k++;
                    }
                }
                myReader.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("SQL hiba: " + ex);
                TraceBejegyzes("SQL hiba: " + ex);
                return 1;
            }

            bw.ReportProgress(20);
            // -	Adott partnerhez tartozó könyveletlen bevallások lekérdezése és betöltése listába
            // select bev_id from bevallasok where konyvelt is null and ervenyes='I' and pnr_id=@pnr_id

            for (int i = 0; i < PnridList.Count; i++)
            {
                sql = "select bev_id from bevallasok where konyvelt is null and storno='0' and ervenyes='I' and pnr_id=" + PnridList[i] + " order by 1";
                scommand = new SqlCommand(sql, sconn);
                myReader = null;
                k = BevList.Count;              // a k-nak a bev lista épp aktuális elemszámától kell indulnia
                try
                {
                    if (sconn.State == ConnectionState.Closed) sconn.Open();
                    myReader = scommand.ExecuteReader();

                    if (myReader.HasRows)
                    {
                        while (myReader.Read())
                        {
                            BevList.Add(myReader["bev_id"].ToString());
                            bw.ReportProgress(20, BevList[k] + " " + PnridList[i]);
                            k++;
                        }
                    }
                    myReader.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("SQL hiba: " + ex);
                    TraceBejegyzes("SQL hiba: " + ex);
                    return 1;
                }
            }

            bw.ReportProgress(30);
            // -	A könyveletlen bevallások könyvelése, a főkönyvi sorok elkészítése
            counter = 0;
            foglid = 0;
            for (counter = 0; counter < BevList.Count; counter++)
            {
                // bevallás fej rész könyvelése
                // update bevallasok set konyvelt='I' where bev_id=BevList[i]
                sql = "UPDATE bevallasok SET konyvelt='I' WHERE bev_id=" + BevList[counter] +
                    "; SELECT CAST(pnr_id AS int) FROM bevallasok WHERE bev_id=" + BevList[counter];
                scommand = new SqlCommand(sql, sconn);
                try { foglid = (int)scommand.ExecuteScalar(); }
                catch (Exception ex)
                {
                    MessageBox.Show("SQL hiba: " + ex); 
                    TraceBejegyzes("SQL hiba: " + ex);
                    return 1;
                }
                bw.ReportProgress(30, "bev_id=" + BevList[counter] + " pnr_id=" + foglid + " bevallás könyvelve");

                // bevallás fejhez főkönyvi sor beszúrása
                sql = "INSERT INTO fokonyvi_tetelek (pnr_id,idk_id,osszeg,geny_kod,konyveles_kelte,fokonyvi_szam,storno,statusz,rogzit_neve,bev_id) " +
                    "SELECT b.pnr_id,b.idk_id,b.mindosszesen,'FOGL_ELOIR',GETDATE(),'31',0,NULL,'" + User.name + "'," + BevList[counter] +
                    " FROM bevallasok b" +
                    " WHERE b.bev_id=" + BevList[counter] + ";" +
                    " SELECT CAST(pnr_id AS int) FROM fokonyvi_tetelek WHERE ftl_id=(SELECT CAST(scope_identity() AS int));";

                scommand = new SqlCommand(sql, sconn);
                try { foglid = (int)scommand.ExecuteScalar(); }
                catch (Exception ex)
                {
                    MessageBox.Show("SQL hiba: " + ex); TraceBejegyzes("SQL hiba: " + ex);
                    return 1;
                }
                bw.ReportProgress(30, "bev_id=" + BevList[counter] + " pnr_id=" + foglid + " főkönyvi sor könyvelve");

                // forgalom sorokhoz főkönyvi sorok beszúrása

                sajatsql = "INSERT INTO fokonyvi_tetelek (pnr_id,tszs_id,idk_id,osszeg,forg_id,geny_kod,konyveles_kelte,fokonyvi_szam,alap_kod,storno,statusz," +
                "irany,rogzit_neve) " +
                "SELECT f.pnr_id,t.tszs_id,f.idk_id,f.hozzajarulas,f.forg_id,'" + munkgenykod + "',GETDATE(),'31',NULL,0,NULL,NULL,'" + User.name + "'" +
                " FROM bevallasok b, forgalmak f, tagsagi_szerzodesek t" +
                " WHERE f.bev_id=b.bev_id and t.pnr_id=f.pnr_id and b.bev_id=" + BevList[counter] + " and f.hozzajarulas>0";
                scommand = new SqlCommand(sajatsql, sconn);
                try { darab = (int)scommand.ExecuteNonQuery(); }
                catch (Exception ex)
                {
                    MessageBox.Show("SQL hiba: " + ex); TraceBejegyzes("SQL hiba: " + ex);
                    return 1;
                }
                if (darab > 0)
                    bw.ReportProgress(30, "bev_id=" + BevList[counter] + " pnr_id=" + foglid + " " + darab + " tagi főkönyvi sor könyvelve " + munkgenykod);

                munksql = "INSERT INTO fokonyvi_tetelek (pnr_id,tszs_id,idk_id,osszeg,forg_id,geny_kod,konyveles_kelte,fokonyvi_szam,alap_kod,storno,statusz," +
                "irany,rogzit_neve) " +
                "SELECT f.pnr_id,t.tszs_id,f.idk_id,f.sajat,f.forg_id,'" + sajatgenykod + "',GETDATE(),'31',NULL,0,NULL,NULL,'" + User.name + "'" +
                " FROM bevallasok b, forgalmak f, tagsagi_szerzodesek t" +
                " WHERE f.bev_id=b.bev_id and t.pnr_id=f.pnr_id and b.bev_id=" + BevList[counter] + " and f.sajat>0";
                scommand = new SqlCommand(munksql, sconn);
                try { darab = (int)scommand.ExecuteNonQuery(); }
                catch (Exception ex)
                {
                    MessageBox.Show("SQL hiba: " + ex); TraceBejegyzes("SQL hiba: " + ex);
                    return 1;
                }
                if (darab > 0)
                    bw.ReportProgress(30, "bev_id=" + BevList[counter] + " pnr_id=" + foglid + " " + darab + " tagi főkönyvi sor könyvelve " + sajatgenykod);

                rendszsql = "INSERT INTO fokonyvi_tetelek (pnr_id,tszs_id,idk_id,osszeg,forg_id,geny_kod,konyveles_kelte,fokonyvi_szam,alap_kod,storno,statusz," +
                "irany,rogzit_neve) " +
                "SELECT f.pnr_id,t.tszs_id,f.idk_id,f.rend_tamog,f.forg_id,'" + rendszgenykod + "',GETDATE(),'31',NULL,0,NULL,NULL,'" + User.name + "'" +
                " FROM bevallasok b, forgalmak f, tagsagi_szerzodesek t" +
                " WHERE f.bev_id=b.bev_id and t.pnr_id=f.pnr_id and b.bev_id=" + BevList[counter] + " and f.rend_tamog>0";
                scommand = new SqlCommand(rendszsql, sconn);
                try { darab = (int)scommand.ExecuteNonQuery(); }
                catch (Exception ex)
                {
                    MessageBox.Show("SQL hiba: " + ex); TraceBejegyzes("SQL hiba: " + ex);
                    return 1;
                }
                if (darab > 0)
                    bw.ReportProgress(30, "bev_id=" + BevList[counter] + " pnr_id=" + foglid + " " + darab + " tagi főkönyvi sor könyvelve " + rendszgenykod);

                egyszsql = "INSERT INTO fokonyvi_tetelek (pnr_id,tszs_id,idk_id,osszeg,forg_id,geny_kod,konyveles_kelte,fokonyvi_szam,alap_kod,storno,statusz," +
                "irany,rogzit_neve) " +
                "SELECT f.pnr_id,t.tszs_id,f.idk_id,f.egysz_tamog,f.forg_id,'" + egyszgenykod + "',GETDATE(),'31',NULL,0,NULL,NULL,'" + User.name + "'" +
                " FROM bevallasok b, forgalmak f, tagsagi_szerzodesek t" +
                " WHERE f.bev_id=b.bev_id and t.pnr_id=f.pnr_id and b.bev_id=" + BevList[counter] + " and f.egysz_tamog>0";
                scommand = new SqlCommand(egyszsql, sconn);
                try { darab = (int)scommand.ExecuteNonQuery(); }
                catch (Exception ex)
                {
                    MessageBox.Show("SQL hiba: " + ex); TraceBejegyzes("SQL hiba: " + ex);
                    return 1;
                }
                if (darab > 0)
                    bw.ReportProgress(30, "bev_id=" + BevList[counter] + " pnr_id=" + foglid + " " + darab + " tagi főkönyvi sor könyvelve " + egyszgenykod);
                
                // tranzakció commit
                if (TranID != 0)
                {
                    TransactionCommit();
                    bw.ReportProgress(35, "commit ok");
                }
            }

            return 0;
        }

        private int BankiTetelek()
        {
            // TRANZAKCIÓ indítása
            if (TranID == 0)
            {
                TranID++;
                TransactionBegin();                // BEGIN TRAN
            }

            bw.ReportProgress(35);
            /* 2.	A könyveletlen banki tételek megkeresése és könyvelése */
            // -	A banki tételekhez kapcsolódó partner id-k lekérdezése és betöltése listába

            k = 0;
            sql = "select distinct pnr_id from bankkivonat_tetelek where konyveles_datuma is null and pnr_id is not null order by 1";
            scommand = new SqlCommand(sql, sconn);
            myReader = null;
            try
            {
                if (sconn.State == ConnectionState.Closed) sconn.Open();
                myReader = scommand.ExecuteReader();

                if (myReader.HasRows)
                {
                    while (myReader.Read())
                    {
                        PnridListbef.Add(myReader["pnr_id"].ToString());
                        bw.ReportProgress(35, PnridListbef[k]);
                        k++;
                    }
                }
                myReader.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("SQL hiba: " + ex);
                TraceBejegyzes("SQL hiba: " + ex);
                return 1;
            }

            bw.ReportProgress(40);
            // -	Adott partnerhez tartozó könyveletlen banki tételek lekérdezése és betöltése listába
            // select bev_id from bevallasok where konyvelt is null and ervenyes='I' and pnr_id=@pnr_id

            for (int i = 0; i < PnridListbef.Count; i++)
            {
                sql = "select btl_id from bankkivonat_tetelek where konyveles_datuma is null and pnr_id is not null and pnr_id=" + PnridListbef[i] + " order by 1";
                scommand = new SqlCommand(sql, sconn);
                myReader.Close();
                myReader = null;
                k = BefList.Count;              // a k-nak a bef lista épp aktuális elemszámától kell indulnia
                try
                {
                    if (sconn.State == ConnectionState.Closed) sconn.Open();
                    myReader = scommand.ExecuteReader();

                    if (myReader.HasRows)
                    {
                        while (myReader.Read())
                        {
                            BefList.Add(myReader["btl_id"].ToString());
                            bw.ReportProgress(40, "Btl_id: " + BefList[k] + " " + PnridListbef[i]);
                            k++;
                        }
                    }
                    myReader.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("SQL hiba: " + ex);
                    TraceBejegyzes("SQL hiba: " + ex);
                    return 1;
                }
            }

            bw.ReportProgress(50);
            // -	A banki tételek könyvelése, a főkönyvi sorok elkészítése
            counter = 0;
            foglid = 0;
            for (counter = 0; counter < BefList.Count; counter++)
            {
                // banki tételek könyvelése
                // update bankkivonat_tetelek set konyveles_datuma=getdate(), statusz='K' where btl_id=BefList[i]
                sql = "UPDATE bankkivonat_tetelek SET konyveles_datuma=getdate(), statusz='K' WHERE btl_id=" + BefList[counter] +
                    "; SELECT CAST(pnr_id AS int) FROM bankkivonat_tetelek WHERE btl_id=" + BefList[counter];
                scommand = new SqlCommand(sql, sconn);
                try { foglid = (int)scommand.ExecuteScalar(); }
                catch (Exception ex)
                {
                    MessageBox.Show("SQL hiba: " + ex); TraceBejegyzes("SQL hiba: " + ex);
                    return 1;
                }
                bw.ReportProgress(50, "btl_id=" + BefList[counter] + " pnr_id=" + foglid + " banki tétel könyvelve");

                // főkönyvi sor beszúrása
                sql = "INSERT INTO fokonyvi_tetelek (btl_id,pnr_id,osszeg,geny_kod,konyveles_kelte,fokonyvi_szam,bizonylat_kelte,bizonylat_szama,"+
                    "bizonylat_sor,storno,statusz,irany,rogzit_neve) " +
                    "SELECT b.btl_id,b.pnr_id,b.osszeg,'BEFATUTALAS',GETDATE(),'31',bkt.bizonylat_kelte,bkt.kivonat_szama,b.sorszam,0,NULL,bkt.irany,'" + User.name + "'" +
                    " FROM bankkivonat_tetelek b, banki_kivonatok bkt" +
                    " WHERE b.bkt_id=bkt.bkt_id and b.btl_id=" + BefList[counter] + ";" +
                    " SELECT CAST(pnr_id AS int) FROM fokonyvi_tetelek WHERE ftl_id=(SELECT CAST(scope_identity() AS int));";

                scommand = new SqlCommand(sql, sconn);
                try { foglid = (int)scommand.ExecuteScalar(); }
                catch (Exception ex)
                {
                    MessageBox.Show("SQL hiba: " + ex); TraceBejegyzes("SQL hiba: " + ex);
                    return 1;
                }
                bw.ReportProgress(50, "btl_id=" + BefList[counter] + " pnr_id=" + foglid + " főkönyvi sor könyvelve");
            }

            // tranzakció commit
            if (TranID != 0)
            {
                TransactionCommit();
                bw.ReportProgress(50, "commit ok");
            }

            return 0;
        }

        private int Leosztas()
        {
            // TRANZAKCIÓ indítása
            if (TranID == 0)
            {
                TranID++;
                TransactionBegin();                // BEGIN TRAN
            }

            // 3. lépés
            // 
            // A könyvelt bevallásokhoz tartozó még nem könyvelt főkönyvi sorok kikeresése, ahol a partnerhez van hozzá tartozó könyvelt pénz a megfelelő főkönyvi sorral
            // ahol bevallás főkönyvi sor: genykod=FOGL_ELOIR, pénz főkönyvi sor: genykod=BEFATUTALAS ahol a bizonylat kelte a legkorábbi (order by bizonylat_kelte desc)
            // A sorok összepárosítása a hivatkozasi_szam használatával, az összegek összehasonlítása és a szorzó kiszámítása

            bw.ReportProgress(55);
            /*
            select a.pnr_id
            from
                (select distinct pnr_id,osszeg
                 from fokonyvi_tetelek
                 where geny_kod='FOGL_ELOIR' 
                    and forg_id is  null
                    and statusz is null
                    and hivatkozasi_szam is null
                ) a inner join
                (select distinct pnr_id,osszeg,btl_id,konyveles_kelte
                 from fokonyvi_tetelek
                 where geny_kod='BEFATUTALAS'
                    and statusz is null
                    and hivatkozasi_szam is null
                ) b on a.pnr_id=b.pnr_id
            */
            sql = "SELECT a.pnr_id FROM (SELECT DISTINCT pnr_id,osszeg FROM fokonyvi_tetelek WHERE geny_kod='FOGL_ELOIR' AND forg_id IS NULL AND statusz IS NULL" +
            " AND hivatkozasi_szam IS NULL) a INNER JOIN (SELECT DISTINCT pnr_id,osszeg,btl_id,konyveles_kelte FROM fokonyvi_tetelek WHERE geny_kod='BEFATUTALAS'" +
            " AND statusz IS NULL AND hivatkozasi_szam IS NULL ) b ON a.pnr_id=b.pnr_id";

            scommand = new SqlCommand(sql, sconn);
            myReader.Close();
            myReader = null;
            PnridList = new List<string>();
            k = 0;
            try
            {
                if (sconn.State == ConnectionState.Closed) sconn.Open();
                myReader = scommand.ExecuteReader();

                if (myReader.HasRows)
                {
                    while (myReader.Read())
                    {
                        PnridList.Add(myReader["pnr_id"].ToString());
                        bw.ReportProgress(60, "pnr_id: " + PnridList[k] + " összevezetése");
                        k++;
                    }
                }
                myReader.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("SQL hiba: " + ex);
                TraceBejegyzes("SQL hiba: " + ex);
                return 1;
            }

            counter = 0;
            foglid = 0;
            darab = 0;
            bool vanbev = false, vanpenz = false;
            for (counter = 0; counter < PnridList.Count; counter++)
            {
                // érvényes előírások begyűjtése
                sql = "SELECT ftl_id,CAST(osszeg AS int) osszeg FROM fokonyvi_tetelek WHERE geny_kod='FOGL_ELOIR' and forg_id is null and statusz is null and hivatkozasi_szam is null " +
                    "and osszeg>0 and pnr_id=" + PnridList[counter] + " order by ftl_id";
                scommand = new SqlCommand(sql, sconn);
                myReader.Close();
                myReader = null;
                List<Fokonyvi_tetel> FTListb = new List<Fokonyvi_tetel>();
                k = 0;
                string ftl_id, osszeg;
                try
                {
                    if (sconn.State == ConnectionState.Closed) sconn.Open();
                    myReader = scommand.ExecuteReader();

                    if (myReader.HasRows)
                    {
                        while (myReader.Read())
                        {

                            ftl_id = myReader["ftl_id"].ToString();
                            osszeg = myReader["osszeg"].ToString();
                            Fokonyvi_tetel ft = new Fokonyvi_tetel(ftl_id, osszeg);
                            FTListb.Add(ft);
                            bw.ReportProgress(60, "bev: " + ftl_id + " összevezetése");
                            k++;
                        }
                    }
                    myReader.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("SQL hiba: " + ex);
                    TraceBejegyzes("SQL hiba: " + ex);
                    return 1;
                }
                if (FTListb.Count > 0) vanbev = true;

                // érvényes pénzek begyűjtése
                sql = "SELECT DISTINCT ftl_id,CAST(osszeg AS int) osszeg FROM fokonyvi_tetelek WHERE geny_kod='BEFATUTALAS' AND statusz IS NULL AND hivatkozasi_szam IS NULL " +
                    "and pnr_id=" + PnridList[counter] + " order by ftl_id";
                scommand = new SqlCommand(sql, sconn);
                myReader.Close();
                myReader = null;
                List<Fokonyvi_tetel> FTListp = new List<Fokonyvi_tetel>();
                k = 0;
                try
                {
                    if (sconn.State == ConnectionState.Closed) sconn.Open();
                    myReader = scommand.ExecuteReader();

                    if (myReader.HasRows)
                    {
                        while (myReader.Read())
                        {

                            ftl_id = myReader["ftl_id"].ToString();
                            osszeg = myReader["osszeg"].ToString();
                            Fokonyvi_tetel ft = new Fokonyvi_tetel(ftl_id, osszeg);
                            FTListp.Add(ft);
                            bw.ReportProgress(80, "bef: " + ftl_id + " összevezetése");
                            k++;
                        }
                    }
                    myReader.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("SQL hiba: " + ex);
                    TraceBejegyzes("SQL hiba: " + ex);
                    return 1;
                }
                if (FTListp.Count > 0) vanpenz = true;

                // összevezetés
                string bevosszeg, befosszeg;
                double szorzo;
                // az első előírás az első utalással, a második a másodikkal, stb.
                if (vanbev && vanpenz)
                {
                    for (int i = 0; i < FTListb.Count; i++)
                    {
                        // ha FTListp.count>=i+1
                        // bevosszeg=FTListb[i].osszeg, befosszeg=FTListp[i].osszeg, double szorzo=befosszeg/bevosszeg
                        // a szorzó nem lehet nagyobb 1-nél.
                        // update ftl sorok hivatkozasi_szam=random x jegyű szám, statusz='K'
                        // bev_id kell az ftl-be!
                        // bev_id alapján a forg főkönyvi sorok insertálása, geny_kod=xxxx, osszeg=könyvelt főkönyvi előírás összege*szorzó
                        if (FTListp.Count >= (i + 1))
                        {
                            bevosszeg = FTListb[i].osszeg;
                            befosszeg = FTListp[i].osszeg;
                            szorzo = double.Parse(befosszeg) / double.Parse(bevosszeg);
                            bw.ReportProgress(90, "ftl id: " + FTListb[i].ftl_id);
                            bw.ReportProgress(90, "bevösszeg: " + bevosszeg);
                            bw.ReportProgress(90, "befösszeg: " + befosszeg);
                            bw.ReportProgress(90, "szorzó: " + szorzo.ToString());
                            if (szorzo > 1.0) szorzo = 1.0;
                            hivatkozasi_szam = rnd.Next(100000000, 399999999);

                            sql = "UPDATE fokonyvi_tetelek SET statusz='K', hivatkozasi_szam='" + hivatkozasi_szam.ToString() + "' WHERE ftl_id=" + FTListb[i].ftl_id;
                            scommand = new SqlCommand(sql, sconn);
                            try { scommand.ExecuteNonQuery(); }
                            catch (Exception ex)
                            {
                                MessageBox.Show("SQL hiba: " + ex); TraceBejegyzes("SQL hiba: " + ex);
                                return 1;
                            }
                            sql = "UPDATE fokonyvi_tetelek SET statusz='K', hivatkozasi_szam='" + hivatkozasi_szam.ToString() + "' WHERE ftl_id=" + FTListp[i].ftl_id;
                            scommand = new SqlCommand(sql, sconn);
                            scommand.ExecuteNonQuery();
                            try { scommand.ExecuteNonQuery(); }
                            catch (Exception ex)
                            {
                                MessageBox.Show("SQL hiba: " + ex); TraceBejegyzes("SQL hiba: " + ex);
                                return 1;
                            }

                            // főkönyvi sor beszúrása
                            /* INSERT INTO fokonyvi_tetelek 
	                            (pnr_id,tszs_id,osszeg,geny_kod,konyveles_kelte,fokonyvi_szam,
	                            storno,statusz,irany,hivatkozasi_szam,rogzit_neve)
                            SELECT ftl2.pnr_id,ftl2.tszs_id,ftl2.osszeg*szorzo,
	                            (SELECT geny_kod 
	                            FROM gazdasagi_esemenyek 
	                            WHERE geny_kod like SUBSTRING(ftl2.geny_kod,1,5)+'%KONYV'),
	                            GETDATE(),'31',0,'K',NULL,ftl1.hivatkozasi_szam,User.name
                            FROM bevallasok b, fokonyvi_tetelek ftl1, 
	                            fokonyvi_tetelek ftl2, forgalmak f
                            WHERE ftl1.bev_id=b.bev_id
	                            and b.bev_id=f.bev_id
	                            and ftl2.forg_id=f.forg_id
	                            and ftl1.ftl_id=FTListb[i].ftl_id;  */

                            sql = "INSERT INTO fokonyvi_tetelek (pnr_id,tszs_id,osszeg,geny_kod,konyveles_kelte,fokonyvi_szam,storno,statusz,irany,hivatkozasi_szam,rogzit_neve)" +
                            " SELECT ftl2.pnr_id,ftl2.tszs_id,ftl2.osszeg*" + szorzo.ToString() +
                            ",(SELECT geny_kod FROM gazdasagi_esemenyek WHERE geny_kod like SUBSTRING(ftl2.geny_kod,1,5)+'%KONYV')," +
                            " GETDATE(),'31',0,'K',NULL,ftl1.hivatkozasi_szam," + User.name + " FROM bevallasok b, fokonyvi_tetelek ftl1, fokonyvi_tetelek ftl2, forgalmak f" +
                            " WHERE ftl1.bev_id=b.bev_id and b.bev_id=f.bev_id and ftl2.forg_id=f.forg_id and ftl1.ftl_id=" + FTListb[i].ftl_id + ";";

                            scommand = new SqlCommand("spTagFokonyviKonyvInsert", sconn);
                            scommand.CommandType = CommandType.StoredProcedure;
                            scommand.Parameters.Add(new SqlParameter("@ftl_id", SqlDbType.Int)).Value = int.Parse(FTListb[i].ftl_id);
                            scommand.Parameters.Add(new SqlParameter("@szorzo", SqlDbType.Float)).Value = szorzo;
                            scommand.Parameters.Add(new SqlParameter("@modosit_neve", SqlDbType.VarChar, 80)).Value = User.name;

                            try { darab = (int)scommand.ExecuteNonQuery(); }
                            catch (Exception ex)
                            {
                                MessageBox.Show("SQL hiba: " + ex); TraceBejegyzes("SQL hiba: " + ex);
                                return 1;
                            }
                            bw.ReportProgress(90, darab.ToString() + " tagi főkönyvi sor könyvelve");
                        }
                    }
                }
            }
            // tranzakció commit
            if (TranID != 0)
            {
                TransactionCommit();
                bw.ReportProgress(95, "commit ok");
            }

            return 0;
        }

        private void bw_RunWorkerCompleted(object source, RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                MessageBox.Show(e.Error.Message);
            }
            else if (e.Cancelled)
            {
                Megszakit();
                bMegszakit.Enabled = false;
            }
            else
            {
                // FŐ TRANZAKCIÓ
                string Query = "COMMIT TRANSACTION main";
                scommand = new SqlCommand(Query, sconn);
                try { scommand.ExecuteNonQuery(); }
                catch (Exception ex) { MessageBox.Show("SQL Hiba " + ex.Message); return; }
            }
            bMegszakit.Enabled = false;
            //rtbMessage.Text += "\nElkészült.";
            //label1.Text = "Elkészült.";
            tsExit.Enabled = true;
        }

        public override void tsExit_Click(object sender, EventArgs e)
        {
            if (bw.IsBusy)
            {
                MessageBox.Show("Amíg az összevezetés folyamatban van, nem lehet kilépni!");
            }
            else
            {
                Close();
            }
        }

        private void bMegszakit_Click(object sender, EventArgs e)
        {
            if (bw.WorkerSupportsCancellation == true)
            {
                bw.CancelAsync();
                bw.ReportProgress(10, "Összevezetés visszagörgetése");                
            }
        }

        private void Megszakit()
        {
            // TRANZAKCIÓ visszagörgetése
            string Query;
            Query = "ROLLBACK TRANSACTION main";
            scommand = new SqlCommand(Query, sconn);
            try { scommand.ExecuteNonQuery(); }
            catch (Exception ex) { MessageBox.Show("SQL Hiba " + ex.Message); }
            //bw.ReportProgress(0, "Összevezetés visszagörgetve.");
        }

        // Tranzakciók kezelése
        private void TransactionBegin()
        {
            string Query;
            Query = "BEGIN TRAN ov" + TranID.ToString();
            scommand = new SqlCommand(Query, sconn);
            try { scommand.ExecuteNonQuery(); }
            catch (Exception ex) { MessageBox.Show("SQL Hiba " + ex.Message); }
        }

        private void TransactionCommit()
        {
            string Query;
            Query = "COMMIT TRAN ov" + TranID.ToString() + ";";
            scommand = new SqlCommand(Query, sconn);
            try { scommand.ExecuteNonQuery(); }
            catch (Exception ex) { MessageBox.Show("SQL Hiba " + ex.Message); TraceBejegyzes(ex.Message); }
            TranID = 0;
        }

        private void TransactionRollback()
        {
            string Query;
            Query = "ROLLBACK TRAN ov" + TranID.ToString() + ";";
            scommand = new SqlCommand(Query, sconn);
            try { scommand.ExecuteNonQuery(); }
            catch (Exception ex) { MessageBox.Show("SQL Hiba " + ex.Message); TraceBejegyzes(ex.Message); }
            TranID = 0;
        }

        private void TraceBejegyzes(string msg)
        {
            myListener.WriteLine("-" + DateTime.Now + ": " + msg);
            myListener.Flush();
            myListener.Close();
        } 
    }

    public class Fokonyvi_tetel
    {
        public string ftl_id, osszeg;
        public Fokonyvi_tetel(string ftl_id, string osszeg)
        {
            this.ftl_id=ftl_id;
            this.osszeg=osszeg;
        }
    }
}
