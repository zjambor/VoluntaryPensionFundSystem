using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows;
using System.Configuration;
using System.Data.SqlClient;
using System.Drawing.Printing;

namespace VoluntaryPensionFundSystem
{
    public partial class Foablak : VoluntaryPensionFundSystem.BaseForm
    {
        private SqlConnection sconn;
        private SqlCommand scommand;
        //private SqlDataAdapter da;
        //private DataTable dt;
        private PrintDocument printDocument1 = new PrintDocument();
        Bitmap memoryImage;

        // MOBJAMBORZ1\SQLEXPRESS

        List<Form> openForms = new List<Form>();
        
        public Foablak()
        {
            InitializeComponent();

            printDocument1.PrintPage += new PrintPageEventHandler(printDocument1_PrintPage);

            // connectionstring
            ConnectionStringSettings settings = System.Configuration.ConfigurationManager.ConnectionStrings["Vpfs"];
            
            if (settings == null) return;

            // sql kapcsolat
            sconn = new SqlConnection(settings.ConnectionString);

            // adattábla
            //dt = new DataTable();
            scommand = new SqlCommand("spTagsagiOkiratokSelect", sconn);
            scommand.CommandType = CommandType.StoredProcedure;

            try
            {
                if (sconn.State == ConnectionState.Closed) sconn.Open();
                scommand.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show("SQL Hiba " + ex.Message);
            }

            Fomenu.Visible = false;
            tsIkonsor.Visible = false;

            // felhasználónév és jelszó bekérése, ellenőrzése 
            Azonositas a = new Azonositas(sconn);
            a.ShowDialog();

            this.Text += " (" + User.teljesnev + ")";

            if (User.name != "admin")
            {
                this.felhasználókKezeléseToolStripMenuItem.Enabled = false;
                this.sqlScriptFuttatásaToolStripMenuItem.Enabled = false;
            }
        }

        private void CaptureScreen()
        {
            Graphics myGraphics = this.CreateGraphics();
            Size s = this.Size;
            memoryImage = new Bitmap(s.Width, s.Height, myGraphics);
            Graphics memoryGraphics = Graphics.FromImage(memoryImage);
            memoryGraphics.CopyFromScreen(this.Location.X, this.Location.Y, 0, 0, s);
        }

        private void printDocument1_PrintPage(System.Object sender,
               System.Drawing.Printing.PrintPageEventArgs e)
        {
            e.Graphics.DrawImage(memoryImage, 0, 0);
        }

        /// <summary>
        /// Törzsadatok menü
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tagságiOkiratokRögzítéseToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            Okiratok newContract = new Okiratok(sconn);
            newContract.Show();
        }

        private void tagságiOkiratokToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OkiratokModositas updateContract = new OkiratokModositas(sconn);
            updateContract.Show();
        }

        private void záradékolásToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            ZaradekolasMenubol clauseFromMenu = new ZaradekolasMenubol(sconn);
            clauseFromMenu.Show();
        }

        private void partnerTörzsadatokToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PartnerTorzsadatok pnr = new PartnerTorzsadatok(sconn);
            pnr.Show();
        }

        /// <summary>
        /// Szerződések menü
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void foglalkoztatókKarbantartásaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
        }

        private void foglalkoztatókToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FoglKivalasztas updateEmployer = new FoglKivalasztas(sconn);
            updateEmployer.Show();
        }

        /// <summary>
        /// File menü
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private new void bezárToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private new void mindentBezárToolStripMenuItem_Click(object sender, EventArgs e)
        {            
            foreach (Form f in Application.OpenForms)
                openForms.Add(f);

            foreach (Form f in openForms)
            {
                if (f.Name != "Foablak")
                    f.Close();
            }
        }

        private new void kilépésToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private new void nyomtatásToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CaptureScreen();
            printDocument1.DefaultPageSettings.Landscape = true;
            printDocument1.Print();
        }

        private void bKilep_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void Foablak_Load(object sender, EventArgs e)
        {

        }

        private void bankszámlákToolStripMenuItem_Click(object sender, EventArgs e)
        {
            BankszamlakM browseAccountNumbers = new BankszamlakM(sconn);
            browseAccountNumbers.Show();
        }

        private void adatszolgáltatásRögzítéseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            BevRogzFoglKivalasztas Rogzites = new BevRogzFoglKivalasztas(sconn);
            Rogzites.Show();
        }

        private void nyomtatásiKépToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CaptureScreen();
            printDocument1.DefaultPageSettings.Landscape = true;

            printPreviewDialog1.Document = printDocument1;
            printPreviewDialog1.ShowDialog();
        }

        private void elektronikusBevallásFeldolgozásToolStripMenuItem_Click(object sender, EventArgs e)
        {
            BevRogzLemezes newBevRogzLemezes = new BevRogzLemezes(sconn);
            newBevRogzLemezes.Show();
        }

        private void törzsadatokToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void időszakokToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Idoszakok browseIdoszakok = new Idoszakok(sconn);
            browseIdoszakok.Show();
        }

        private void hibákToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Hibak newHibakForm = new Hibak(sconn);
            newHibakForm.Show();
        }

        private void nyugdíjpénztárakToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Nyugdijpenztarak nyp = new Nyugdijpenztarak(sconn);
            nyp.Show();
        }

        private void támogathatóságiKörökToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            TamogathatoKorok tmk = new TamogathatoKorok(sconn);
            tmk.Show();
        }

        private void gazdaságiEseményekToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            GazdasagiEsemenyek ge = new GazdasagiEsemenyek(sconn);
            ge.Show();
        }

        private void befektetésiKombinációkToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Portfoliok pf = new Portfoliok(sconn);
            pf.Show();
        }

        private void bankiKivonatokToolStripMenuItem_Click(object sender, EventArgs e)
        {
            BankiKivonatok bk = new BankiKivonatok(sconn);
            bk.Show();
        }

        private void pénzügyiTételekRögzítéseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            BefizetesekRogz bR = new BefizetesekRogz(sconn);
            bR.Show();
        }

        private void pénzügyiTételekBeazonosításaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Beazonositas bea = new Beazonositas(sconn);
            bea.Show();
        }

        private void összevezetésToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Osszevezetes ov = new Osszevezetes(sconn);
            ov.Show();
        }

        private void sqlScriptFuttatásaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RunSql rs = new RunSql(sconn);
            rs.Show();
        }

        private void főkönyviTételekBöngészéseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FokonyviTetelekBong ftb = new FokonyviTetelekBong(sconn);
            ftb.Show();
        }

        private void tagiSzámlaegyenlegToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TagiEgyeniSzamlaReportIndito ter = new TagiEgyeniSzamlaReportIndito(sconn);
            ter.Show();
        }

        private void munkáltatóiSzámlaegyenlegToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MunkFolyoszamlaReportIndito mfr = new MunkFolyoszamlaReportIndito(sconn);
            mfr.Show();
        }

        private void felhasználókKezeléseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Felhasznalok fho = new Felhasznalok(sconn);
            fho.Show();
        }

        private void jogcímekToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Jogcimek jcm = new Jogcimek(sconn);
            jcm.Show();
        }

        private void sajátJelszóMódosításaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            UjJelszo newpw = new UjJelszo(sconn);
            newpw.fhoid = User.userid.ToString();
            newpw.fhonev = User.name;
            newpw.teljesnev = User.teljesnev;
            newpw.ShowDialog();
        }

        private void korcsoportokToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Korcsoportok kcst = new Korcsoportok(sconn);
            kcst.Show();
        }

        private void bevallásBöngészőToolStripMenuItem_Click(object sender, EventArgs e)
        {
            BevBong bb = new BevBong(sconn);
            bb.Show();
        }

        private void munkáltatóiSzerződésToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MunkSzerzodesek msz = new MunkSzerzodesek(sconn, null);
            msz.Show();
        }

        private void támogatóiSzerződésToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TamogSzerz tsz = new TamogSzerz(sconn, null);
            tsz.Show();
        }

        private void névjegyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Nevjegy nj = new Nevjegy();
            nj.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Azonositas a = new Azonositas(sconn);
            a.ShowDialog();
        }
    }

    public static class User
    {
        public static int userid;
        public static string name;
        public static string password;
        public static string teljesnev;

        public static void Construct()
        {

        }
    }
}
