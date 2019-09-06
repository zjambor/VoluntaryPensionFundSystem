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
using System.Security.Cryptography;
using System.Text.RegularExpressions;

namespace VoluntaryPensionFundSystem
{
    public partial class Felhasznalok : VoluntaryPensionFundSystem.BaseForm
    {
        //private int Fhoid;
        private SqlConnection sconn;
        private SqlCommand scommand, lastScommand;
        private SqlDataAdapter da;
        private DataTable dt;
        private bool back, enter = false;
        private bool keresomod;
        private int index = 0;
        //bool InsertTrueUpdateFalse;

        TextWriterTraceListener myListener = new TextWriterTraceListener(@"LOG\Felhasznalok.log", "myListener");
        public const string MatchDatePattern = @"^[0-9]{4}.[0-9]{2}.[0-9]{2}.$";
        public const string MatchDatePattern2 = @"^[1-2]{1}[0-9]{7}$";

        public Felhasznalok(SqlConnection SqlConn)
        {
            InitializeComponent();

            this.sconn = SqlConn;
            if (sconn.State == ConnectionState.Closed) sconn.Open();

            // adattábla
            dt = new DataTable();

            scommand = new SqlCommand("spFelhasznalok", sconn);
            scommand.CommandType = CommandType.StoredProcedure;
            lastScommand = scommand;
            da = new SqlDataAdapter(scommand);
            Frissit();

            this.dgwMunk.DataSource = dt;
            //rejtId();

            tsUpdate.Enabled = true;
            tsNew.Enabled = true;
            tsDelete.Enabled = true;
            tsFind.Enabled = false;
            tsSearch.Enabled = true;
            tsSave.Enabled = false;
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

            dt.Columns["fho_id"].ColumnName = "Fho id";
            dt.Columns["fho_nev"].ColumnName = "Felhasználónév";
            dt.Columns["teljes_nev"].ColumnName = "Teljes név";
            dt.Columns["erv_kezdete"].ColumnName = "Érv. kezdete";
            dt.Columns["erv_vege"].ColumnName = "Érv. vége";
            dt.Columns["megjegyzes"].ColumnName = "Megjegyzés";
        }

        public void rejtId()
        {
            dgwMunk.Columns[0].Visible = false;
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

        public override void tsExit_Click(object sender, EventArgs e)
        {
            exit();
        }

        public override void createNew()
        {
            Newuser ujfh = new Newuser(sconn);
            ujfh.ShowDialog();

            scommand = lastScommand;
            da = new SqlDataAdapter(scommand);
            dt = new DataTable();
            Frissit();
            this.dgwMunk.DataSource = dt;
            tsSave.Enabled = false;
        }

        private void updateRecord()
        {
            txFhnev.ReadOnly = false;
            txTeljesnev.ReadOnly = false;
            txErvKezdete.ReadOnly = false;
            txErvVege.ReadOnly = false;
            tsSave.Enabled = true;
        }

        private void exit()
        {
            if (tsSave.Enabled == true)
            {
                DialogResult dr = MessageBox.Show("Menti a változtatásokat?", "Megerősítés", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);
                if (dr == DialogResult.No)
                {
                    Close();
                }
                else if (dr == DialogResult.Yes)           // mentés
                {
                    save();
                    Close();
                }
            }
            else
                Close();
        }

        private void TraceBejegyzes(string msg)
        {
            myListener.WriteLine("-" + DateTime.Now + ": " + msg);
            myListener.Flush();
            myListener.Close();
        }

        #region Dátum mezők lekezelése
        private void txAlkKezdete_KeyDown(object sender, KeyEventArgs e)
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

        private void txAlkVege_KeyDown(object sender, KeyEventArgs e)
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

        private void txAlkKezdete_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Char.IsDigit(e.KeyChar))
            {
                if (txErvKezdete.TextLength >= 8) e.Handled = true;
            }
            else if (enter)
            {
                DateTransformA();
            }
            else if (back)
            {
                if (txErvKezdete.TextLength == 11)
                {
                    string text;
                    text = txErvKezdete.Text.Substring(0, 4) + txErvKezdete.Text.Substring(5, 2) + txErvKezdete.Text.Substring(8, 3);
                    txErvKezdete.Text = text;
                    txErvKezdete.Select(txErvKezdete.Text.Length, 0);
                }
            }
            else
                e.Handled = true;
        }

        private void txAlkVege_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Char.IsDigit(e.KeyChar))
            {
                if (txErvVege.TextLength >= 8) e.Handled = true;
            }
            else if (enter)
            {
                DateTransformB();
            }
            else if (back)
            {
                if (txErvVege.TextLength == 11)
                {
                    string text;
                    text = txErvVege.Text.Substring(0, 4) + txErvVege.Text.Substring(5, 2) + txErvVege.Text.Substring(8, 3);
                    txErvVege.Text = text;
                    txErvVege.Select(txErvVege.Text.Length, 0);
                }
            }
            else
                e.Handled = true;
        }

        private void DateTransformA()
        {
            if (txErvKezdete.TextLength == 8)
            {
                string text;
                text = txErvKezdete.Text.Substring(0, 4) + "." + txErvKezdete.Text.Substring(4, 2) + "." + txErvKezdete.Text.Substring(6, 2);
                try
                {
                    txErvKezdete.Text = DateTime.Parse(text).ToShortDateString();
                }
                catch
                {
                    MessageBox.Show("Hibás dátum!");
                    txErvKezdete.Focus();
                }
            }
            else
            {
                if (txErvKezdete.TextLength != 11 && txErvKezdete.TextLength != 0)
                {
                    MessageBox.Show("Hibás dátum!");
                    txErvKezdete.Focus();
                }
            }
        }

        private void DateTransformB()
        {
            if (txErvVege.TextLength == 8)
            {
                string text;
                text = txErvVege.Text.Substring(0, 4) + "." + txErvVege.Text.Substring(4, 2) + "." + txErvVege.Text.Substring(6, 2);
                try
                {
                    txErvVege.Text = DateTime.Parse(text).ToShortDateString();
                }
                catch
                {
                    MessageBox.Show("Hibás dátum!");
                    txErvVege.Focus();
                }
            }
            else
            {
                if (txErvVege.TextLength != 11 && txErvVege.TextLength != 0)
                {
                    MessageBox.Show("Hibás dátum!");
                    txErvVege.Focus();
                }
            }
        }

        private void txAlkKezdete_KeyUp(object sender, KeyEventArgs e)
        {
            back = false;
            enter = false;
        }

        private void txAlkVege_KeyUp(object sender, KeyEventArgs e)
        {
            back = false;
            enter = false;
        }

        private void txAlkKezdete_Leave(object sender, EventArgs e)
        {
            DateTransformA();
            back = false;
            enter = false;
        }

        private void txAlkVege_Leave(object sender, EventArgs e)
        {
            DateTransformB();
            back = false;
            enter = false;
        }
        #endregion

        private void bJelszo_Click(object sender, EventArgs e)
        {
            UjJelszo pw = new UjJelszo(sconn);
            pw.fhoid = this.txFhoid.Text;
            pw.fhonev = this.txFhnev.Text;
            pw.teljesnev = this.txTeljesnev.Text;
            pw.ShowDialog();
        }

        private void dgwMunk_SelectionChanged(object sender, EventArgs e)
        {
            if (!keresomod)
            {
                try
                {
                    int i = dgwMunk.SelectedCells[0].RowIndex;
                    index = dgwMunk.SelectedCells[0].RowIndex;
                    txFhoid.Text = dgwMunk.Rows[i].Cells[0].Value.ToString();
                    txFhnev.Text = dgwMunk.Rows[i].Cells[1].Value.ToString();
                    txTeljesnev.Text = dgwMunk.Rows[i].Cells[2].Value.ToString();

                    try { txErvKezdete.Text = DateTime.Parse(dgwMunk.Rows[i].Cells[3].Value.ToString()).ToShortDateString(); }
                    catch { txErvKezdete.Text = string.Empty; }
                    try { txErvVege.Text = DateTime.Parse(dgwMunk.Rows[i].Cells[4].Value.ToString()).ToShortDateString(); }
                    catch { txErvVege.Text = string.Empty; }
                }
                catch
                {
                }
            }
        }

        public override void save()
        {
            int x = index;

            // ellenőrzés
            if (txFhnev.Text == string.Empty) { label9.Text = "A mező nem lehet üres!"; label9.Visible = true; txFhnev.Focus(); return; }
            if (txTeljesnev.Text == string.Empty) { label9.Text = "A mező nem lehet üres!"; label9.Visible = true; txTeljesnev.Focus(); return; }

            if (!(txErvKezdete.Text == string.Empty) && !(IsDate(txErvKezdete.Text)))
            {
                if (IsDate2(txErvKezdete.Text))
                {
                    DateTransformA();
                }
                else
                {
                    MessageBox.Show("Az érv. kezdete dátum nem megfelelő!");
                    txErvKezdete.Focus();
                    return;
                }
            }

            if (!(txErvVege.Text == string.Empty) && !(IsDate(txErvVege.Text)))
            {
                if (IsDate2(txErvVege.Text))
                {
                    DateTransformB();
                }
                else
                {
                    MessageBox.Show("Az érv. vége dátum nem megfelelő!");
                    txErvVege.Focus();
                    return;
                }
            }

            scommand = new SqlCommand("spFelhasznalokUpdate", sconn);
            scommand.CommandType = CommandType.StoredProcedure;
            scommand.Parameters.Add(new SqlParameter("@id", SqlDbType.Int)).Value = int.Parse(txFhoid.Text);
            scommand.Parameters.Add(new SqlParameter("@fho_nev", SqlDbType.VarChar, 30)).Value = txFhnev.Text;
            scommand.Parameters.Add(new SqlParameter("@teljes_nev", SqlDbType.VarChar, 80)).Value = txTeljesnev.Text;
            scommand.Parameters.Add(new SqlParameter("@erv_kezdete", SqlDbType.Date)).Value = DateTime.Parse(txErvKezdete.Text).ToShortDateString();
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
            }

            // update
            string query;
            try
            {
                if (txErvVege.Text != string.Empty)
                {
                    query = "update felhasznalok set erv_vege='" + txErvVege.Text.Substring(0,10) + "' where fho_id=" + txFhoid.Text;
                }
                else
                {
                    query = "update felhasznalok set erv_vege=NULL where fho_id=" + txFhoid.Text;
                }
                scommand = new SqlCommand(query, sconn);
                if (sconn.State == ConnectionState.Closed) sconn.Open();
                scommand.ExecuteNonQuery();
            }
            catch
            {
            }

            // grid frissítése
            scommand = lastScommand;
            da = new SqlDataAdapter(scommand);
            dt = new DataTable();
            Frissit();
            this.dgwMunk.DataSource = dt;
            dgwMunk.CurrentCell = dgwMunk.Rows[x].Cells[0];
        }

        public override void tsSearch_Click(object sender, EventArgs e)
        {
            yellowMode();
        }

        public override void tsFind_Click(object sender, EventArgs e)
        {
            runQuery();
        }

        public override void yellowMode()
        {
            dgwMunk.DataSource = null;
            dgwMunk.Refresh();

            txFhnev.ReadOnly = false;
            txTeljesnev.ReadOnly = false;
            txErvKezdete.ReadOnly = false;
            txErvVege.ReadOnly = false;

            txFhoid.Text = string.Empty;
            txFhnev.Text = string.Empty;
            txTeljesnev.Text = string.Empty;
            txErvKezdete.Text = string.Empty;
            txErvVege.Text = string.Empty;

            tsUpdate.Enabled = false;
            tsDelete.Enabled = false;
            tsNew.Enabled = true;
            tsSave.Enabled = false;
            tsFind.Enabled = true;
            keresomod = true;
            txFhnev.Focus();
        }

        public override void runQuery()
        {
            this.dgwMunk.DataSource = null;

            // A LEKÉRDEZÉS FELÉPÍTÉSE
            string querystring = "SELECT fho_id,fho_nev,teljes_nev,erv_kezdete,erv_vege,megjegyzes "+
                "FROM felhasznalok WHERE ";
            querystring = txFhnev.Text != string.Empty ? querystring + "fho_nev like '" + txFhnev.Text + "' and " : querystring;
            querystring = (txTeljesnev.Text != string.Empty ? querystring + "teljes_nev like '" + txTeljesnev.Text + "' and " : querystring);
            querystring = (txErvKezdete.Text != string.Empty ? querystring + "erv_kezdete like '" + txErvKezdete.Text + "%' and " : querystring);
            querystring = (txErvVege.Text != string.Empty ? querystring + "erv_vege like '" + txErvVege.Text + "%' and " : querystring);
            querystring += "password is not null order by fho_nev;";

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
            this.dgwMunk.DataSource = dt;
            lastScommand = scommand;

            tsUpdate.Enabled = false;
            tsDelete.Enabled = false;
            tsNew.Enabled = false;
            tsSave.Enabled = false;
            tsFind.Enabled = false;
            tsSearch.Enabled = true;
        }

        private void txTeljesnev_KeyUp(object sender, KeyEventArgs e)
        {
            if (keresomod)
            {
                this.dgwMunk.DataSource = null;

                if (txTeljesnev.Text != string.Empty)
                {
                    // A LEKÉRDEZÉS FELÉPÍTÉSE
                    string querystring = "SELECT fho_id,fho_nev,teljes_nev,erv_kezdete,erv_vege,megjegyzes " +
                        "FROM felhasznalok WHERE ";
                    querystring = (txTeljesnev.Text != string.Empty ? querystring + "teljes_nev like '" + txTeljesnev.Text + "%' and " : querystring);
                    querystring += "password is not null order by fho_nev;";

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
                    this.dgwMunk.DataSource = dt;
                    lastScommand = scommand;

                    tsFind.Enabled = false;
                    tsSearch.Enabled = true;
                }
            }
            dgwMunk.Refresh();
        }

        private void txTeljesnev_Leave(object sender, EventArgs e)
        {
            keresomod = false;
            lastScommand = scommand;
        }

        public override void tsDelete_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("Biztos törli a kijelölt rekordot? ",
                "Törlés?", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dr == DialogResult.Yes)
            {
                string query = "delete from felhasznalok where fho_id=" + txFhoid.Text;
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

                scommand = lastScommand;
                da = new SqlDataAdapter(scommand);
                dt = new DataTable();
                Frissit();
                this.dgwMunk.DataSource = dt;
            }
        }

        private void dgwMunk_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
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

        private void Felhasznalok_Resize(object sender, EventArgs e)
        {
            panel1.Width = Felhasznalok.ActiveForm.Width - 70;
        }

        private void panel1_Resize(object sender, EventArgs e)
        {
            dgwMunk.Dock = DockStyle.Fill;
        }

        public static bool IsDate(string adat)
        {
            try
            {
                if (adat != null) return Regex.IsMatch(adat, MatchDatePattern);
                else return false;
            }
            catch
            {
                return false;
            }
        }

        public static bool IsDate2(string adat)
        {
            try
            {
                if (adat != null) return Regex.IsMatch(adat, MatchDatePattern2);
                else return false;
            }
            catch
            {
                return false;
            }
        }

        private void txFhnev_KeyUp(object sender, KeyEventArgs e)
        {
            if (keresomod)
            {
                this.dgwMunk.DataSource = null;

                if (txFhnev.Text != string.Empty)
                {
                    // A LEKÉRDEZÉS FELÉPÍTÉSE
                    string querystring = "SELECT fho_id,fho_nev,teljes_nev,erv_kezdete,erv_vege,megjegyzes " +
                        "FROM felhasznalok WHERE ";
                    querystring = (txFhnev.Text != string.Empty ? querystring + "fho_nev like '" + txFhnev.Text + "%' and " : querystring);
                    querystring += "password is not null order by fho_nev;";

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
                    this.dgwMunk.DataSource = dt;
                    lastScommand = scommand;

                    tsFind.Enabled = false;
                    tsSearch.Enabled = true;
                }
            }
            dgwMunk.Refresh();
        }

        private void dgwMunk_MouseMove(object sender, MouseEventArgs e)
        {
            DataGridView.HitTestInfo hit = dgwMunk.HitTest(e.X, e.Y);
            if (hit.Type == DataGridViewHitTestType.Cell)
            {
                dgwMunk.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                dgwMunk.DefaultCellStyle.SelectionBackColor = Color.LightYellow;
                dgwMunk.DefaultCellStyle.SelectionForeColor = Color.DarkBlue;
            }
        }
    }
}
