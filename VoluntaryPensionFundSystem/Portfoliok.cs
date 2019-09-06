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
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace VoluntaryPensionFundSystem
{
    public partial class Portfoliok : BaseForm
    {
        private SqlConnection sconn;
        private SqlCommand scommand;

        int Sorindex;

        // Napló logok:
        TextWriterTraceListener myListener = new TextWriterTraceListener(@"LOG\Portfoliok.log", "myListener");

        public Portfoliok(SqlConnection SqlConn)
        {
            InitializeComponent();

            this.sconn = SqlConn;

            tsDelete.Enabled = true;
            tsFind.Enabled = false;
            tsNew.Enabled = true;
            tsSave.Enabled = false;
            tsSearch.Enabled = false;
            tsUpdate.Enabled = false;

            if (sconn.State == ConnectionState.Closed) sconn.Open();

            lbVege.Visible = false;
            datVege.Visible = false;

            dgvPortfoliok.Focus();
        }

        public void rejtId()
        {
            dgvPortfoliok.Columns[0].Visible = false;
        }

        private void Portfoliok_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'vPFSDataSet.spPortfoliok' table. You can move, or remove it, as needed.
            Reload();
        }

        private void Reload()
        {
            this.spPortfoliokTableAdapter.Fill(this.vPFSDataSet.spPortfoliok);
            rejtId();
            int Count;
            try
            {
                Count = dgvPortfoliok.RowCount;
            }
            catch
            {
                Count = 0;
            }
            if (Count == 0)
            {
                txLeiras.ReadOnly = true;
                txMegjegyzes.ReadOnly = true;
                txNev.ReadOnly = true;
                txTipus.ReadOnly = true;
                datKezdete.Enabled = false;
                cbErv.Enabled = false;
                txLeiras.Text = string.Empty;
                txMegjegyzes.Text = string.Empty;
                txNev.Text = string.Empty;
                txTipus.Text = string.Empty;
            }
            else
            {
                txLeiras.ReadOnly = false;
                txMegjegyzes.ReadOnly = false;
                txNev.ReadOnly = false;
                txTipus.ReadOnly = false;
                datKezdete.Enabled = true;
                cbErv.Enabled = true;
            }
        }

        public override void tsNew_Click(object sender, EventArgs e)
        {
            UjPortfolio up = new UjPortfolio(sconn);
            up.ShowDialog();

            Reload();
        }

        public override void tsSave_Click(object sender, EventArgs e)
        {
            save();
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

        public override void save()
        {
            // üres-e a Grid
            int Count;
            try
            {
                Count = dgvPortfoliok.RowCount;
            }
            catch
            {
                Count = 0;
            }

            if (Count > 0)
            {
                if (!ell()) return;

                int Letezik = 0;

                for (int j = 0; j < dgvPortfoliok.RowCount; j++)
                {
                    int id = int.Parse(dgvPortfoliok.Rows[j].Cells[0].Value.ToString());
                    scommand = new SqlCommand("spSelectPortfoliok", sconn);
                    scommand.CommandType = CommandType.StoredProcedure;
                    scommand.Parameters.Add(new SqlParameter("@id", SqlDbType.Int)).Value = id;
                    try
                    {
                        Letezik = (Int32)scommand.ExecuteScalar();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("SQL Hiba " + ex.Message + ex.Data + "letezik");
                        TraceBejegyzes(ex.Message);
                    }

                    if (Letezik == 0)
                    {
                        // insert
                        scommand = new SqlCommand("spPortfoliokInsert", sconn);
                        scommand.CommandType = CommandType.StoredProcedure;

                        scommand.Parameters.Add(new SqlParameter("@id", SqlDbType.Int)).Value = 1;
                        scommand.Parameters.Add(new SqlParameter("@tipus", SqlDbType.VarChar, 1)).Value = dgvPortfoliok.Rows[j].Cells[1].Value.ToString();
                        scommand.Parameters.Add(new SqlParameter("@nev", SqlDbType.VarChar, 80)).Value = dgvPortfoliok.Rows[j].Cells[2].Value.ToString();
                        scommand.Parameters.Add(new SqlParameter("@leiras", SqlDbType.VarChar, 255)).Value = dgvPortfoliok.Rows[j].Cells[3].Value.ToString();
                        scommand.Parameters.Add(new SqlParameter("@erv_kezdete", SqlDbType.Date)).Value = DateTime.Parse(dgvPortfoliok.Rows[j].Cells[4].Value.ToString());
                        scommand.Parameters.Add(new SqlParameter("@megjegyzes", SqlDbType.VarChar, 255)).Value = dgvPortfoliok.Rows[j].Cells[6].Value.ToString();
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

                        try
                        {
                            DateTime vege = DateTime.Parse(dgvPortfoliok.Rows[j].Cells[5].Value.ToString());
                            scommand = new SqlCommand("spPortfoliokDateUpdate", sconn);
                            scommand.CommandType = CommandType.StoredProcedure;
                            scommand.Parameters.Add(new SqlParameter("@id", SqlDbType.Int)).Value = id;
                            scommand.Parameters.Add(new SqlParameter("@erv_vege", SqlDbType.Date)).Value = DateTime.Parse(dgvPortfoliok.Rows[j].Cells[5].Value.ToString());

                            try { scommand.ExecuteNonQuery(); }
                            catch (Exception ex) { MessageBox.Show("SQL Hiba " + ex.Message); }
                        }
                        catch
                        {
                            scommand = new SqlCommand("UPDATE befektetesi_kombinaciok set erv_vege=null where bfk_id=" + id.ToString(), sconn);
                            try { scommand.ExecuteNonQuery(); }
                            catch (Exception ex) { MessageBox.Show("SQL Hiba " + ex.Message); }
                        }
                    }
                    else
                    {
                        // update
                        scommand = new SqlCommand("spPortfoliokUpdate", sconn);
                        scommand.CommandType = CommandType.StoredProcedure;

                        scommand.Parameters.Add(new SqlParameter("@id", SqlDbType.Int)).Value = id;
                        scommand.Parameters.Add(new SqlParameter("@tipus", SqlDbType.VarChar, 1)).Value = dgvPortfoliok.Rows[j].Cells[1].Value.ToString();
                        scommand.Parameters.Add(new SqlParameter("@nev", SqlDbType.VarChar, 80)).Value = dgvPortfoliok.Rows[j].Cells[2].Value.ToString();
                        scommand.Parameters.Add(new SqlParameter("@leiras", SqlDbType.VarChar, 255)).Value = dgvPortfoliok.Rows[j].Cells[3].Value.ToString();
                        scommand.Parameters.Add(new SqlParameter("@erv_kezdete", SqlDbType.Date)).Value = DateTime.Parse(dgvPortfoliok.Rows[j].Cells[4].Value.ToString());
                        scommand.Parameters.Add(new SqlParameter("@megjegyzes", SqlDbType.VarChar, 255)).Value = dgvPortfoliok.Rows[j].Cells[6].Value.ToString();
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

                        try
                        {
                            DateTime vege = DateTime.Parse(dgvPortfoliok.Rows[j].Cells[5].Value.ToString());
                            scommand = new SqlCommand("spPortfoliokDateUpdate", sconn);
                            scommand.CommandType = CommandType.StoredProcedure;
                            scommand.Parameters.Add(new SqlParameter("@id", SqlDbType.Int)).Value = id;
                            scommand.Parameters.Add(new SqlParameter("@erv_vege", SqlDbType.Date)).Value = DateTime.Parse(dgvPortfoliok.Rows[j].Cells[5].Value.ToString());

                            try { scommand.ExecuteNonQuery(); }
                            catch (Exception ex) { MessageBox.Show("SQL Hiba " + ex.Message); }
                        }
                        catch
                        {
                            
                        }
                        if (cbErv.Checked == true)
                        {
                            scommand = new SqlCommand("UPDATE befektetesi_kombinaciok set erv_vege=null where bfk_id=" + id.ToString(), sconn);
                            try { scommand.ExecuteNonQuery(); }
                            catch (Exception ex) { MessageBox.Show("SQL Hiba " + ex.Message); }
                        }
                    }
                }
            }
            Reload();
            tsSave.Enabled = false;
        }

        private bool ell()
        {
            // kötelező mezők
            for (int i = 0; i < dgvPortfoliok.RowCount; i++)
            {
                if (dgvPortfoliok.Rows[i].Cells[1].Value.ToString().Length == 0 || dgvPortfoliok.Rows[i].Cells[2].Value.ToString().Length == 0)
                {
                    MessageBox.Show("Típus és név nem lehet üres!");
                    return false;
                }
            }

            // esetleges ismétlődés ellenőrzése
            string adat;
            int sum;

            sum = 0;
            adat = dgvPortfoliok.Rows[Sorindex].Cells[1].Value.ToString();
            for (int i = 0; i < dgvPortfoliok.RowCount; i++)
            {
                if (dgvPortfoliok.Rows[i].Cells[1].Value.ToString() == adat)
                {
                    sum++;
                }
            }
            if (sum > 1)
            {
                MessageBox.Show(adat + " már létezik!");
                return false;
            }
            return true;
        }

        public override void tsDelete_Click(object sender, EventArgs e)
        {
            // üres-e a Grid
            int Count;
            try
            {
                Count = dgvPortfoliok.RowCount;
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
                    // id meghatározása
                    int id = int.Parse(dgvPortfoliok.Rows[Sorindex].Cells[0].Value.ToString());
                    string query1 = "delete from befektetesi_kombinaciok where bfk_id=" + id + ";";
                    scommand = new SqlCommand(query1, sconn);
                    // rekord törlése
                    try { scommand.ExecuteNonQuery(); }
                    catch (Exception ex) { MessageBox.Show("SQL Hiba " + ex.Message); }
                    Reload();
                }
            }
        }

        private void dgvPortfoliok_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
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

        private void Portfoliok_Resize(object sender, EventArgs e)
        {
            dgvPortfoliok.Width = Portfoliok.ActiveForm.Width - 77;
        }

        private void dgvPortfoliok_SelectionChanged(object sender, EventArgs e)
        {
            try
            {
                Sorindex = dgvPortfoliok.SelectedCells[0].RowIndex;
                txId.Text = dgvPortfoliok.Rows[Sorindex].Cells[0].Value.ToString();
                txTipus.Text = dgvPortfoliok.Rows[Sorindex].Cells[1].Value.ToString();
                txNev.Text = dgvPortfoliok.Rows[Sorindex].Cells[2].Value.ToString();
                txLeiras.Text = dgvPortfoliok.Rows[Sorindex].Cells[3].Value.ToString();
                txMegjegyzes.Text = dgvPortfoliok.Rows[Sorindex].Cells[6].Value.ToString();
                datKezdete.Value = DateTime.Parse(dgvPortfoliok.Rows[Sorindex].Cells[4].Value.ToString());
                try
                {
                    DateTime vege = DateTime.Parse(dgvPortfoliok.Rows[Sorindex].Cells[5].Value.ToString());
                    cbErv.Checked = false;
                    lbVege.Visible = true;
                    datVege.Visible = true;
                    datVege.Value = vege;
                }
                catch
                {
                    cbErv.Checked = true;
                    lbVege.Visible = false;
                    datVege.Visible = false;
                }
            }
            catch
            {
            }
        }

        private void dgvPortfoliok_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            TraceBejegyzes(e.ToString());
        }

        private void txTipus_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (txId.Text != string.Empty) dgvPortfoliok.Rows[Sorindex].Cells[1].Value = txTipus.Text;
            }
            catch { }
        }

        private void txNev_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (txId.Text != string.Empty) dgvPortfoliok.Rows[Sorindex].Cells[2].Value = txNev.Text;
            }
            catch { }
        }

        private void txLeiras_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (txId.Text != string.Empty) dgvPortfoliok.Rows[Sorindex].Cells[3].Value = txLeiras.Text;
            }
            catch { }
        }

        private void txMegjegyzes_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (txId.Text != string.Empty) dgvPortfoliok.Rows[Sorindex].Cells[6].Value = txMegjegyzes.Text;
            }
            catch { }
        }

        private void datKezdete_ValueChanged(object sender, EventArgs e)
        {
            if (txId.Text != string.Empty) dgvPortfoliok.Rows[Sorindex].Cells[4].Value = datKezdete.Value;
            //tsSave.Enabled = true;
        }

        private void cbErv_CheckedChanged(object sender, EventArgs e)
        {
            if (cbErv.Checked == true)
            {
                lbVege.Visible = false;
                datVege.Visible = false;
                dgvPortfoliok.Rows[Sorindex].Cells[5].Value = string.Empty;
            }
            else
            {
                lbVege.Visible = true;
                datVege.Visible = true;
            }
        }

        private void TraceBejegyzes(string msg)
        {
            myListener.WriteLine("-" + DateTime.Now + ": " + msg);
            myListener.Flush();
            myListener.Close();
        }

        private void datVege_ValueChanged(object sender, EventArgs e)
        {
            dgvPortfoliok.Rows[Sorindex].Cells[5].Value = datVege.Value.ToShortDateString();
            //tsSave.Enabled = true;
        }

        private void txTipus_KeyPress(object sender, KeyPressEventArgs e)
        {
            tsSave.Enabled = true;
        }

        private void txNev_KeyPress(object sender, KeyPressEventArgs e)
        {
            tsSave.Enabled = true;
        }

        private void txLeiras_KeyPress(object sender, KeyPressEventArgs e)
        {
            tsSave.Enabled = true;
        }

        private void txMegjegyzes_KeyPress(object sender, KeyPressEventArgs e)
        {
            tsSave.Enabled = true;
        }

        private void cbErv_Click(object sender, EventArgs e)
        {
            tsSave.Enabled = true;
        }

        private void dgvPortfoliok_MouseMove(object sender, MouseEventArgs e)
        {
            DataGridView.HitTestInfo hit = dgvPortfoliok.HitTest(e.X, e.Y);
            if (hit.Type == DataGridViewHitTestType.Cell)
            {
                dgvPortfoliok.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                dgvPortfoliok.DefaultCellStyle.SelectionBackColor = Color.LightYellow;
                dgvPortfoliok.DefaultCellStyle.SelectionForeColor = Color.DarkBlue;
            }
        }
    }
}
