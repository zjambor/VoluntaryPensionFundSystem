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
    public partial class Fogl_munkaviszony : VoluntaryPensionFundSystem.BaseForm
    {
        private int Tszsid;
        private SqlConnection sconn;
        private SqlCommand scommand;
        private SqlDataAdapter da;
        private DataTable dt;
        int i = 0;
        const int ColumnCount = 12;
        bool back, enter = false;
        bool InsertTrueUpdateFalse;
        FoglKivalasztas fk = null;
        public string adoszam, adoazon, megnev, ev, helyseg, cim, irszam;

        TextWriterTraceListener myListener = new TextWriterTraceListener(@"LOG\Fogl-Munkaviszony.log", "myListener");
        
        public Fogl_munkaviszony(SqlConnection SqlConn, int tszsid)
        {
            InitializeComponent();

            this.Tszsid = tszsid;

            this.sconn = SqlConn;
            if (sconn.State == ConnectionState.Closed) sconn.Open();

            // adattábla
            dt = new DataTable();

            scommand = new SqlCommand("spMunkaviszony", sconn);
            scommand.CommandType = CommandType.StoredProcedure;
            scommand.Parameters.Add(new SqlParameter("@tszs_id", SqlDbType.Int)).Value = Tszsid;

            da = new SqlDataAdapter(scommand);
            Frissit();            

            this.dgwMunk.DataSource = dt;
            rejtId();

            tsUpdate.Enabled = true;
            tsNew.Enabled = true;
            tsDelete.Enabled = true;
            tsFind.Enabled = false;
            tsSearch.Enabled = false;
            tsSave.Enabled = false;

            dgwMunk.Focus();
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

            dt.Columns["pnr_id"].ColumnName = "Pnr id";
            dt.Columns["megnevezes"].ColumnName = "Munk. neve";
            dt.Columns["nev"].ColumnName = "EV neve";
            dt.Columns["helyseg"].ColumnName = "Helység";
            dt.Columns["cim"].ColumnName = "Cím";
            dt.Columns["ir_szam"].ColumnName = "Ir.szám";
            dt.Columns["alk_kezdete"].ColumnName = "Alk. kezdete";
            dt.Columns["alk_vege"].ColumnName = "Alk. vége";
            dt.Columns["megjegyzes"].ColumnName = "Megjegyzés";
            dt.Columns["hozzajarulas"].ColumnName = "Hozzájár.";
            dt.Columns["tagdij_szazalek"].ColumnName = "Tagd. %";
            dt.Columns["gyakorisag"].ColumnName = "Gyakoriság";            
        }

        public void rejtId()
        {
            dgwMunk.Columns[0].Visible = false;
        }

        private void dgwMunk_SelectionChanged(object sender, EventArgs e)
        {
            try
            {
                i = dgwMunk.SelectedCells[0].RowIndex;
                txMvnyid.Text = dgwMunk.Rows[i].Cells[0].Value.ToString();
                txPnrid.Text = dgwMunk.Rows[i].Cells[1].Value.ToString();
                txMegnev.Text = dgwMunk.Rows[i].Cells[2].Value.ToString();
                txEV.Text = dgwMunk.Rows[i].Cells[3].Value.ToString();
                txHelyseg.Text = dgwMunk.Rows[i].Cells[4].Value.ToString();
                txCim.Text = dgwMunk.Rows[i].Cells[5].Value.ToString();
                txIrszam.Text = dgwMunk.Rows[i].Cells[6].Value.ToString();
                txAlkKezdete.Text = (dgwMunk.Rows[i].Cells[7].Value.ToString() == string.Empty ? string.Empty : DateTime.Parse(dgwMunk.Rows[i].Cells[7].Value.ToString()).ToShortDateString());
                txAlkVege.Text = (dgwMunk.Rows[i].Cells[8].Value.ToString() == string.Empty ? string.Empty : DateTime.Parse(dgwMunk.Rows[i].Cells[8].Value.ToString()).ToShortDateString());
                txMegjegyzes.Text = dgwMunk.Rows[i].Cells[9].Value.ToString();
                txMH.Text = dgwMunk.Rows[i].Cells[10].Value.ToString();
                txMHszazalek.Text = dgwMunk.Rows[i].Cells[11].Value.ToString();
                txGyak.Text = dgwMunk.Rows[i].Cells[12].Value.ToString();
            }
            catch
            {
                // kezdeti hiba
            }

            //txPnrid.ReadOnly = true;
            //txMegnev.ReadOnly = true;
            //txEV.ReadOnly = true;
            //txHelyseg.ReadOnly = true;
            //txCim.ReadOnly = true;
            //txIrszam.ReadOnly = true;
            
            //txAlkKezdete.ReadOnly = true;
            //txAlkVege.ReadOnly = true;
            //txMegjegyzes.ReadOnly = true;
            //txMH.ReadOnly = true;
            //txMHszazalek.ReadOnly = true;
            //txGyak.ReadOnly = true;

            //tsUpdate.Enabled = false;
            //tsNew.Enabled = false;
            //tsDelete.Enabled = false;
            //tsFind.Enabled = false;
            //tsSearch.Enabled = false;
            //tsSave.Enabled = false;
        }

        private void SaveTheRecord()
        {
            // Ellenőrzések

            if (txPnrid.Text == string.Empty) { MessageBox.Show("Foglalkoztató id nem lehet üres!"); txPnrid.Focus(); return; }
            if (txHelyseg.Text == string.Empty) { MessageBox.Show("Foglalkoztatót meg kell adni!"); txPnrid.Focus(); return; }
            if (txAlkKezdete.Text == string.Empty) { MessageBox.Show("Alkalmazás kezdete nem lehet üres!"); txPnrid.Focus(); return; }

            if (InsertTrueUpdateFalse)
            {
                // INSERT
                int mvnyid = 0;
                scommand = new SqlCommand("spMvInsert", sconn);
                scommand.CommandType = CommandType.StoredProcedure;
                scommand.Parameters.Add(new SqlParameter("@id", SqlDbType.Int)).Value = 1;
                scommand.Parameters.Add(new SqlParameter("@tszs_id", SqlDbType.Int)).Value = Tszsid;
                scommand.Parameters.Add(new SqlParameter("@pnr_id", SqlDbType.Int)).Value = int.Parse(txPnrid.Text);
                scommand.Parameters.Add(new SqlParameter("@alk_kezdete", SqlDbType.Date)).Value = DateTime.Parse(txAlkKezdete.Text);
                scommand.Parameters.Add(new SqlParameter("@modosit_neve", SqlDbType.VarChar, 80)).Value = User.name;

                try
                {
                    mvnyid = (int)scommand.ExecuteScalar();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("SQL hiba: " + ex.Message);
                    TraceBejegyzes(ex.Message);
                }

                // UPDATE
                if (txAlkVege.Text != string.Empty)
                {
                    scommand = new SqlCommand("UPDATE munkaviszonyok SET alk_vege='" + DateTime.Parse(txAlkVege.Text).ToShortDateString().Substring(0, 10) + "' where mvny_id=" + mvnyid, sconn);
                    try { scommand.ExecuteNonQuery(); }
                    catch (Exception ex) { MessageBox.Show("SQL hiba: " + ex.Message); TraceBejegyzes(ex.Message); }
                }

                if (txMegjegyzes.Text != string.Empty)
                {
                    scommand = new SqlCommand("UPDATE munkaviszonyok SET megjegyzes='" + txMegjegyzes.Text + "' where mvny_id=" + mvnyid, sconn);
                    try { scommand.ExecuteNonQuery(); }
                    catch (Exception ex) { MessageBox.Show("SQL hiba: " + ex.Message); TraceBejegyzes(ex.Message); }
                }

                if (txMH.Text != string.Empty)
                {
                    scommand = new SqlCommand("UPDATE munkaviszonyok SET hozzajarulas=" + int.Parse(txMH.Text) + " where mvny_id=" + mvnyid, sconn);
                    try { scommand.ExecuteNonQuery(); }
                    catch (Exception ex) { MessageBox.Show("SQL hiba: " + ex.Message); TraceBejegyzes(ex.Message); }
                }

                if (txMHszazalek.Text != string.Empty)
                {
                    scommand = new SqlCommand("UPDATE munkaviszonyok SET tagdij_szazalek=" + int.Parse(txMHszazalek.Text) + " where mvny_id=" + mvnyid, sconn);
                    try { scommand.ExecuteNonQuery(); }
                    catch (Exception ex) { MessageBox.Show("SQL hiba: " + ex.Message); TraceBejegyzes(ex.Message); }
                }

                if (txGyak.Text != string.Empty)
                {
                    scommand = new SqlCommand("UPDATE munkaviszonyok SET gyakorisag=" + int.Parse(txGyak.Text) + " where mvny_id=" + mvnyid, sconn);
                    try { scommand.ExecuteNonQuery(); }
                    catch (Exception ex) { MessageBox.Show("SQL hiba: " + ex.Message); TraceBejegyzes(ex.Message); }
                }
            }
            else
            {
                // UPDATE                
                int mvnyid = int.Parse(txMvnyid.Text);
                scommand = new SqlCommand("UPDATE munkaviszonyok SET pnr_id='" + int.Parse(txPnrid.Text) + "', alk_kezdete='" + DateTime.Parse(txAlkKezdete.Text).ToShortDateString().Substring(0, 10) + "' where mvny_id=" + mvnyid, sconn);
                try { scommand.ExecuteNonQuery(); }
                catch (Exception ex) { MessageBox.Show("SQL hiba: " + ex.Message); TraceBejegyzes(ex.Message); }
                
                if (txAlkVege.Text != string.Empty)
                {
                    scommand = new SqlCommand("UPDATE munkaviszonyok SET alk_vege='" + DateTime.Parse(txAlkVege.Text).ToShortDateString().Substring(0, 10) + "' where mvny_id=" + mvnyid, sconn);
                    try { scommand.ExecuteNonQuery(); }
                    catch (Exception ex) { MessageBox.Show("SQL hiba: " + ex.Message); TraceBejegyzes(ex.Message); }
                }

                if (txMegjegyzes.Text != string.Empty)
                {
                    scommand = new SqlCommand("UPDATE munkaviszonyok SET megjegyzes='" + txMegjegyzes.Text + "' where mvny_id=" + mvnyid, sconn);
                    try { scommand.ExecuteNonQuery(); }
                    catch (Exception ex) { MessageBox.Show("SQL hiba: " + ex.Message); TraceBejegyzes(ex.Message); }
                }

                if (txMH.Text != string.Empty)
                {
                    scommand = new SqlCommand("UPDATE munkaviszonyok SET hozzajarulas=" + int.Parse(txMH.Text) + " where mvny_id=" + mvnyid, sconn);
                    try { scommand.ExecuteNonQuery(); }
                    catch (Exception ex) { MessageBox.Show("SQL hiba: " + ex.Message); TraceBejegyzes(ex.Message); }
                }

                if (txMHszazalek.Text != string.Empty)
                {
                    scommand = new SqlCommand("UPDATE munkaviszonyok SET tagdij_szazalek=" + int.Parse(txMHszazalek.Text) + " where mvny_id=" + mvnyid, sconn);
                    try { scommand.ExecuteNonQuery(); }
                    catch (Exception ex) { MessageBox.Show("SQL hiba: " + ex.Message); TraceBejegyzes(ex.Message); }
                }

                if (txGyak.Text != string.Empty)
                {
                    scommand = new SqlCommand("UPDATE munkaviszonyok SET gyakorisag=" + int.Parse(txGyak.Text) + " where mvny_id=" + mvnyid, sconn);
                    try { scommand.ExecuteNonQuery(); }
                    catch (Exception ex) { MessageBox.Show("SQL hiba: " + ex.Message); TraceBejegyzes(ex.Message); }
                }
            }
        }

        private void Reload()
        {
            dt = new DataTable();
            scommand = new SqlCommand("spMunkaviszony", sconn);
            scommand.CommandType = CommandType.StoredProcedure;
            scommand.Parameters.Add(new SqlParameter("@tszs_id", SqlDbType.Int)).Value = Tszsid;

            da = new SqlDataAdapter(scommand);
            Frissit();
            //rejtId();

            this.dgwMunk.DataSource = dt;
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
            InsertTrueUpdateFalse = true;
            tsSave.Enabled = true;

            txPnrid.Text = string.Empty;
            txAlkKezdete.Text = string.Empty;
            txAlkVege.Text = string.Empty;
            txMegjegyzes.Text = string.Empty;
            txMH.Text = string.Empty;
            txMHszazalek.Text = string.Empty;
            txGyak.Text = string.Empty;
            txMegnev.Text = string.Empty;
            txEV.Text = string.Empty;
            txHelyseg.Text = string.Empty;
            txCim.Text = string.Empty;
            txIrszam.Text = string.Empty;

            txPnrid.ReadOnly = false;
            txAlkKezdete.ReadOnly = false;
            txAlkVege.ReadOnly = false;
            txMegjegyzes.ReadOnly = false;
            txMH.ReadOnly = false;
            txMHszazalek.ReadOnly = false;
            txGyak.ReadOnly = false;
        }

        private void updateRecord()
        {
            InsertTrueUpdateFalse = false;
            tsSave.Enabled = true;

            txPnrid.ReadOnly = false;
            txAlkKezdete.ReadOnly = false;
            txAlkVege.ReadOnly = false;
            txMegjegyzes.ReadOnly = false;
            txMH.ReadOnly = false;
            txMHszazalek.ReadOnly = false;
            txGyak.ReadOnly = false;
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

        public override void save()
        {
            SaveTheRecord();
            Reload();

            tsUpdate.Enabled = true;
            tsNew.Enabled = true;
            tsDelete.Enabled = true;
            tsSave.Enabled = false;

            txPnrid.ReadOnly = true;
            txAlkKezdete.ReadOnly = true;
            txAlkVege.ReadOnly = true;
            txMegjegyzes.ReadOnly = true;
            txMH.ReadOnly = true;
            txMHszazalek.ReadOnly = true;
            txGyak.ReadOnly = true;
            txMegnev.ReadOnly = true;
            txEV.ReadOnly = true;
            txHelyseg.ReadOnly = true;
            txCim.ReadOnly = true;
            txIrszam.ReadOnly = true;
        }

        private void TraceBejegyzes(string msg)
        {
            myListener.WriteLine("-" + DateTime.Now + ": " + msg);
            myListener.Flush();
            myListener.Close();
        }

        // A partner adatok betöltése
        private void txPnrid_Leave(object sender, EventArgs e)
        {
            if (txPnrid.Text != string.Empty)
            {
                SqlDataReader myReader = null;
                scommand = new SqlCommand("spFoglAdatai", sconn);
                scommand.CommandType = CommandType.StoredProcedure;
                scommand.Parameters.Add(new SqlParameter("@id", SqlDbType.Int)).Value = int.Parse(txPnrid.Text);
                try
                {
                    if (sconn.State == ConnectionState.Closed) sconn.Open();
                    myReader = scommand.ExecuteReader();
                    while (myReader.Read())
                    {
                        txPnrid.Text = myReader["pnr_id"].ToString();
                        txMegnev.Text = myReader["megnevezes"].ToString();
                        txEV.Text = myReader["nev"].ToString();
                        txHelyseg.Text = myReader["helyseg"].ToString();
                        txCim.Text = myReader["cim"].ToString();
                        txIrszam.Text = myReader["ir_szam"].ToString();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("SQL Hiba " + ex.Message);
                    TraceBejegyzes(ex.Message);
                }
                myReader.Close();
                txAlkKezdete.Focus();
            }
        }

        // Dátum mezők lekezelése

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
                if (txAlkKezdete.TextLength >= 8) e.Handled = true;
            }
            else if (enter)
            {
                DateTransformA();
            }
            else if (back)
            {
                if (txAlkKezdete.TextLength == 11)
                {
                    string text;                    
                    text = txAlkKezdete.Text.Substring(0, 4) + txAlkKezdete.Text.Substring(5, 2) + txAlkKezdete.Text.Substring(8, 3);
                    txAlkKezdete.Text = text;
                    txAlkKezdete.Select(txAlkKezdete.Text.Length, 0);
                }
            }
            else
                e.Handled = true;
        }

        private void txAlkVege_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Char.IsDigit(e.KeyChar))
            {
                if (txAlkVege.TextLength >= 8) e.Handled = true;
            }
            else if (enter)
            {
                DateTransformB();
            }
            else if (back)
            {
                if (txAlkVege.TextLength == 11)
                {
                    string text;
                    text = txAlkVege.Text.Substring(0, 4) + txAlkVege.Text.Substring(5, 2) + txAlkVege.Text.Substring(8, 3);
                    txAlkVege.Text = text;
                    txAlkVege.Select(txAlkVege.Text.Length, 0);
                }
            }
            else
                e.Handled = true;
        }

        private void DateTransformA()
        {
            if (txAlkKezdete.TextLength == 8)
            {
                string text;
                text = txAlkKezdete.Text.Substring(0, 4) + "." + txAlkKezdete.Text.Substring(4, 2) + "." + txAlkKezdete.Text.Substring(6, 2);
                try
                {
                    txAlkKezdete.Text = DateTime.Parse(text).ToShortDateString();
                }
                catch
                {
                    MessageBox.Show("Hibás dátum!");
                    txAlkKezdete.Focus();
                }
            }
            else
            {
                if (txAlkKezdete.TextLength != 11 && txAlkKezdete.TextLength != 0)
                {
                    MessageBox.Show("Hibás dátum!");
                    txAlkKezdete.Focus();
                }
            }
        }

        private void DateTransformB()
        {
            if (txAlkVege.TextLength == 8)
            {
                string text;
                text = txAlkVege.Text.Substring(0, 4) + "." + txAlkVege.Text.Substring(4, 2) + "." + txAlkVege.Text.Substring(6, 2);
                try
                {
                    txAlkVege.Text = DateTime.Parse(text).ToShortDateString();
                }
                catch
                {
                    MessageBox.Show("Hibás dátum!");
                    txAlkVege.Focus();
                }
            }
            else
            {
                if (txAlkVege.TextLength != 11 && txAlkVege.TextLength != 0)
                {
                    MessageBox.Show("Hibás dátum!");
                    txAlkVege.Focus();
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

        // numerikus mezők lekezelése
        private void txPnrid_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Char.IsDigit(e.KeyChar) && !Char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void txMH_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Char.IsDigit(e.KeyChar) && !Char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void txMHszazalek_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Char.IsDigit(e.KeyChar) && !Char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void txGyak_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Char.IsDigit(e.KeyChar) && !Char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        // panel kezelése
        private void panel1_Resize(object sender, EventArgs e)
        {
            dgwMunk.Dock = DockStyle.Fill;
        }

        private void Fogl_munkaviszony_Resize(object sender, EventArgs e)
        {
            panel1.Width = Fogl_munkaviszony.ActiveForm.Width - 47;
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

        private void Fogl_munkaviszony_Load(object sender, EventArgs e)
        {

        }

        private void bKivalaszt_Click(object sender, EventArgs e)
        {
            if (fk != null) return;

            Fogl_keres fker = new Fogl_keres(sconn);
            fker.ShowDialog();
            int foglpnr = fker.FoglPnrid;
            adoszam = fker.adoszam;
            adoazon = fker.adoazon;
            megnev = fker.megnev;
            ev = fker.ev;
            helyseg = fker.helyseg;
            cim = fker.cim;
            irszam = fker.irszam;

            try
            {
                if (fker.FoglPnrid != 0)
                {
                    this.txPnrid.Text = foglpnr.ToString();

                    txMegnev.Text = megnev;
                    if (megnev == string.Empty) txMegnev.Text = ev;
                    txCim.Text = cim;
                    txHelyseg.Text = helyseg;
                    txIrszam.Text = irszam;
                }
            }
            catch
            {
                // nem történt módosítás
            }
        }       
    }
}
