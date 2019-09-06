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
    public partial class BankiKivonatok : VoluntaryPensionFundSystem.BaseForm
    {
        private SqlConnection sconn;
        private SqlCommand scommand;
        private SqlDataAdapter da;
        private DataTable dt;
        bool back, enter = false;

        List<char> lista = new List<char>();
        int Sorindex;

        // Napló logok:
        TextWriterTraceListener myListener = new TextWriterTraceListener(@"LOG\BankiKivonatok.log", "myListener");

        public BankiKivonatok(SqlConnection SqlConn)
        {
            InitializeComponent();

            this.sconn = SqlConn;

            lista.Add('T');
            lista.Add('K');
            cbIrany.DataSource = lista;

            tsDelete.Enabled = true;
            tsFind.Enabled = false;
            tsNew.Enabled = true;
            tsSave.Enabled = false;
            tsSearch.Enabled = true;
            tsUpdate.Enabled = true;

            if (sconn.State == ConnectionState.Closed) sconn.Open();

            scommand = new SqlCommand("spBanki_kivonatok", sconn);
            scommand.CommandType = CommandType.StoredProcedure;
            //lastScommand = scommand;

            da = new SqlDataAdapter(scommand);
            dt = new DataTable();

            Frissit();
            this.dgvBankKiv.DataSource = dt;
            dgvBankKiv.Columns[1].Width = 100;
            dgvBankKiv.Columns[2].Width = 60;
            dgvBankKiv.Columns[3].Width = 50;
            dgvBankKiv.Columns[4].Width = 100;
            dgvBankKiv.Columns[5].Width = 350;

            dgvBankKiv.Focus();
        }

        private void Frissit()
        {
            //dt.Dispose();
            try
            {
                da.Fill(dt);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                //TraceBejegyzes(ex.Message);
            }

            dt.Columns["bizonylat_kelte"].ColumnName = "Értéknap";
            dt.Columns["kivonat_szama"].ColumnName = "Kivonat száma";
            dt.Columns["irany"].ColumnName = "Irány";
            dt.Columns["letrehozas_datuma"].ColumnName = "Létrehozás dát.";
            dt.Columns["megjegyzes"].ColumnName = "Megjegyzés";
        }

        public void rejtId()
        {
            dgvBankKiv.Columns[0].Visible = false;
        }

        private void Reload()
        {
            int Count;
            try
            {
                Count = dgvBankKiv.RowCount;
            }
            catch
            {
                Count = 0;
            }
            if (Count == 0)
            {
                datErteknap.ReadOnly = true;
                datLetrehoz.ReadOnly = true;
                txKivonatSzama.Enabled = false;
                cbIrany.Enabled = false;
                txMegjegyzes.ReadOnly = true;
                datErteknap.Text = string.Empty;
                datLetrehoz.Text = string.Empty;
                cbIrany.Text = lista[0].ToString();
                txMegjegyzes.Text = string.Empty;
                txKivonatSzama.Value = 0;
            }
            else
            {
                datErteknap.ReadOnly = false;
                datLetrehoz.ReadOnly = false;
                txKivonatSzama.Enabled = true;
                cbIrany.Enabled = true;
                txMegjegyzes.ReadOnly = false;
            }
        }

        public override void tsNew_Click(object sender, EventArgs e)
        {
            datErteknap.ReadOnly = false;
            datLetrehoz.ReadOnly = false;
            txKivonatSzama.Enabled = true;
            cbIrany.Enabled = true;
            txMegjegyzes.ReadOnly = false;
            //datErteknap.Text = DateTime.Now.ToShortDateString();
            datLetrehoz.Text = DateTime.Now.ToShortDateString();
            datErteknap.Text = string.Empty;
            //datLetrehoz.Text = string.Empty;
            cbIrany.Text = lista[0].ToString();
            txMegjegyzes.Text = string.Empty;
            txId.Text = string.Empty;
            txKivonatSzama.Value = 0;
            datErteknap.Focus();
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
            datErteknap.ReadOnly = false;
            datLetrehoz.ReadOnly = false;
            txKivonatSzama.Enabled = true;
            cbIrany.Enabled = true;
            txMegjegyzes.ReadOnly = false;
            datErteknap.Focus();
        }

        public override void tsSave_Click(object sender, EventArgs e)
        {
            save();
        }

        public override void yellowMode()
        {
            datErteknap.ReadOnly = false;
            datLetrehoz.ReadOnly = false;
            txKivonatSzama.Enabled = true;
            cbIrany.Enabled = true;
            txMegjegyzes.ReadOnly = false;
            datErteknap.Text = string.Empty;
            datLetrehoz.Text = string.Empty;
            cbIrany.Text = lista[0].ToString();
            txMegjegyzes.Text = string.Empty;
            txKivonatSzama.Value = 0;

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
            this.dgvBankKiv.DataSource = null;

            // A LEKÉRDEZÉS FELÉPÍTÉSE
            string querystring = "SELECT bkt_id,bizonylat_kelte,kivonat_szama,irany,letrehozas_datuma,megjegyzes FROM banki_kivonatok WHERE ";
            querystring = datErteknap.Text != string.Empty ? querystring + "bizonylat_kelte like '" + erteknapstr + "' and " : querystring;
            querystring = (datLetrehoz.Text != string.Empty ? querystring + "letrehozas_datuma like '" + letrehozasstr + "' and " : querystring);
            querystring = (txKivonatSzama.Value != 0 ? querystring + "kivonat_szama = " + txKivonatSzama.Value + " and " : querystring);
            querystring = (cbIrany.Text != string.Empty ? querystring + "irany like '" + cbIrany.Text + "' and " : querystring);
            querystring = (txMegjegyzes.Text != string.Empty ? querystring + "megjegyzes like '" + txMegjegyzes.Text + "' and " : querystring);
            querystring += "bizonylat_kelte is not null order by 1,2;";

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
            this.dgvBankKiv.DataSource = dt;

            datErteknap.Focus();

            tsFind.Enabled = false;
            tsSearch.Enabled = true;

            tsUpdate.Enabled = true;
            tsNew.Enabled = true;
            tsDelete.Enabled = true;
            tsSave.Enabled = false;
        }

        public override void save()
        {
            if (datErteknap.Text == string.Empty) { MessageBox.Show("Értéknap megadása kötelező!"); datErteknap.Focus(); return; }
            if (txKivonatSzama.Value == 0) { MessageBox.Show("Kivonat száma nem lehet 0!"); txKivonatSzama.Focus(); return; }
            if (cbIrany.Text == string.Empty) { MessageBox.Show("Irány megadása kötelező!"); cbIrany.Focus(); return; }
            if (datLetrehoz.Text == string.Empty) { MessageBox.Show("Létrehozás dátuma megadása kötelező!"); datLetrehoz.Focus(); return; }
            dgvBankKiv.Focus();

            int id;
            try
            {
                id = int.Parse(txId.Text);
            }
            catch
            {
                id = 0;
            }

            if (id == 0)
            {
                // insert
                scommand = new SqlCommand("spBanki_kivonatokInsert", sconn);
                scommand.CommandType = CommandType.StoredProcedure;

                scommand.Parameters.Add(new SqlParameter("@id", SqlDbType.Int)).Value = 1;
                scommand.Parameters.Add(new SqlParameter("@bizonylat_kelte", SqlDbType.Date)).Value = DateTime.Parse(datErteknap.Text);
                scommand.Parameters.Add(new SqlParameter("@kivonat_szama", SqlDbType.VarChar, 20)).Value = txKivonatSzama.Value.ToString();
                scommand.Parameters.Add(new SqlParameter("@irany", SqlDbType.VarChar, 1)).Value = cbIrany.Text;
                scommand.Parameters.Add(new SqlParameter("@letrehozas_datuma", SqlDbType.Date)).Value = DateTime.Parse(datLetrehoz.Text);
                scommand.Parameters.Add(new SqlParameter("@megjegyzes", SqlDbType.VarChar, 255)).Value = txMegjegyzes.Text;
                scommand.Parameters.Add(new SqlParameter("@modosit_neve", SqlDbType.VarChar, 80)).Value = User.name;

                try
                {
                    if (sconn.State == ConnectionState.Closed) sconn.Open();
                    id = (int)scommand.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("SQL Hiba " + ex.Message + ex.Data + "insert");
                    TraceBejegyzes(ex.Message);
                }
                if (id == 0) { MessageBox.Show("A rekord mentése nem sikerült!"); datErteknap.Focus(); return; }
                dgv_re();
                dgvBankKiv.CurrentCell = dgvBankKiv.Rows[dgvBankKiv.RowCount - 1].Cells[1];
            }
            else
            {
                // update
                scommand = new SqlCommand("spBanki_kivonatokUpdate", sconn);
                scommand.CommandType = CommandType.StoredProcedure;

                scommand.Parameters.Add(new SqlParameter("@id", SqlDbType.Int)).Value = id;
                scommand.Parameters.Add(new SqlParameter("@bizonylat_kelte", SqlDbType.Date)).Value = DateTime.Parse(datErteknap.Text);
                scommand.Parameters.Add(new SqlParameter("@kivonat_szama", SqlDbType.VarChar, 20)).Value = txKivonatSzama.Value.ToString();
                scommand.Parameters.Add(new SqlParameter("@irany", SqlDbType.VarChar, 1)).Value = cbIrany.Text;
                scommand.Parameters.Add(new SqlParameter("@letrehozas_datuma", SqlDbType.Date)).Value = DateTime.Parse(datLetrehoz.Text);
                scommand.Parameters.Add(new SqlParameter("@megjegyzes", SqlDbType.VarChar, 255)).Value = txMegjegyzes.Text;
                scommand.Parameters.Add(new SqlParameter("@modosit_neve", SqlDbType.VarChar, 80)).Value = User.name;

                try
                {
                    if (sconn.State == ConnectionState.Closed) sconn.Open();
                    scommand.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("SQL Hiba " + ex.Message + ex.Data + " update");
                    TraceBejegyzes(ex.Message);
                }
                dgv_re();
            }

            datErteknap.Focus();

            tsFind.Enabled = false;
            tsSearch.Enabled = true;

            tsUpdate.Enabled = true;
            tsNew.Enabled = true;
            tsDelete.Enabled = true;
            tsSave.Enabled = false;
        }

        private void dgv_re()
        {
            scommand = new SqlCommand("spBanki_kivonatok", sconn);
            scommand.CommandType = CommandType.StoredProcedure;
            da = new SqlDataAdapter(scommand);
            dt = new DataTable();
            Frissit();
            this.dgvBankKiv.DataSource = dt;
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
            // üres-e a Grid
            int Count;
            try
            {
                Count = dgvBankKiv.RowCount;
            }
            catch
            {
                Count = 0;
            }

            if (Count > 0)
            {
                DialogResult dr = MessageBox.Show("Biztos törli a kijelölt rekordot? ",
                    "Törlés?", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dr == DialogResult.Yes)
                {
                    // idegen kulcsok ellenőrzése
                    int count = 0;
                    // id meghatározása
                    int id = int.Parse(dgvBankKiv.Rows[Sorindex].Cells[0].Value.ToString());

                    scommand = new SqlCommand("SELECT count(*) FROM bankkivonat_tetelek WHERE bkt_id=" + id + ";", sconn);
                    try
                    {
                        if (sconn.State == ConnectionState.Closed) sconn.Open();
                        count = (int)scommand.ExecuteScalar();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("SQL Hiba " + ex.Message);
                        TraceBejegyzes(ex.Message);
                    }

                    if (count == 0)
                    {
                        string query1 = "delete from banki_kivonatok where bkt_id=" + id + ";";
                        scommand = new SqlCommand(query1, sconn);
                        // rekord törlése
                        try { scommand.ExecuteNonQuery(); }
                        catch (Exception ex) { MessageBox.Show("SQL Hiba " + ex.Message); }
                        Reload();
                        dgv_re();
                    }
                    else
                    {
                        MessageBox.Show("A kivonat nem törölhető, mert tétel kapcsolódik hozzá!", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void dgvBankKiv_SelectionChanged(object sender, EventArgs e)
        {
            try
            {
                Sorindex = dgvBankKiv.SelectedCells[0].RowIndex;
                txId.Text = dgvBankKiv.Rows[Sorindex].Cells[0].Value.ToString();
                datErteknap.Text = DateTime.Parse(dgvBankKiv.Rows[Sorindex].Cells[1].Value.ToString()).ToShortDateString();
                txKivonatSzama.Value = int.Parse(dgvBankKiv.Rows[Sorindex].Cells[2].Value.ToString());
                cbIrany.Text = dgvBankKiv.Rows[Sorindex].Cells[3].Value.ToString();
                datLetrehoz.Text = DateTime.Parse(dgvBankKiv.Rows[Sorindex].Cells[4].Value.ToString()).ToShortDateString();
                txMegjegyzes.Text = dgvBankKiv.Rows[Sorindex].Cells[5].Value.ToString();

                datErteknap.ReadOnly = true;
                datLetrehoz.ReadOnly = true;
                txKivonatSzama.Enabled = false;
                cbIrany.Enabled = false;
                txMegjegyzes.ReadOnly = true;

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

        private void txKivonatSzama_ValueChanged(object sender, EventArgs e)
        {
            //tsSave.Enabled = true;
        }

        private void cbIrany_TextChanged(object sender, EventArgs e)
        {
            tsSave.Enabled = true;
        }

        private void datLetrehoz_TextChanged(object sender, EventArgs e)
        {
            //tsSave.Enabled = true;
        }

        private void txMegjegyzes_TextChanged(object sender, EventArgs e)
        {
            //tsSave.Enabled = true;
        }

        private void TraceBejegyzes(string msg)
        {
            myListener.WriteLine("-" + DateTime.Now + ": " + msg);
            myListener.Flush();
            myListener.Close();
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

        private void BankiKivonatok_Resize(object sender, EventArgs e)
        {
            dgvBankKiv.Width = BankiKivonatok.ActiveForm.Width - 77;
        }

        // Dátum mezők lekezelése
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

        private void txKivonatSzama_Enter(object sender, EventArgs e)
        {
            txKivonatSzama.Select(0, txKivonatSzama.Text.Length);
        }

        private void txKivonatSzama_KeyPress(object sender, KeyPressEventArgs e)
        {
            tsSave.Enabled = true;
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

        private void cbIrany_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != 'T' && e.KeyChar != 'K')
            {
                e.Handled = true;
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
