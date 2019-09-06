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
    public partial class TamogathatoKorok : BaseForm, IMessageFilter
    {
        private SqlConnection sconn;
        private SqlCommand scommand;

        int Sorindex;
        bool Mentve = true;

        // Napló logok:
        TextWriterTraceListener myListener = new TextWriterTraceListener(@"LOG\TamKorok.log", "myListener");

        public TamogathatoKorok(SqlConnection SqlConn)
        {
            InitializeComponent();

            this.sconn = SqlConn;

            tsDelete.Enabled = true;
            tsFind.Enabled = false;
            tsNew.Enabled = true;
            tsSave.Enabled = true;
            tsSearch.Enabled = false;
            tsUpdate.Enabled = false;

            if (sconn.State == ConnectionState.Closed) sconn.Open();

            Adatcimke.Text = "Mentve: " + (Mentve ? "OK" : "nem");

            dgvTamogKorok.Focus();
        }

        private void TamogathatoKorok_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'vPFSDataSet.spTamogathato_korok' table. You can move, or remove it, as needed.
            Reload();
            Application.AddMessageFilter(this);
        }

        private void Reload()
        {
            this.spTamogathato_korokTableAdapter.Fill(this.vPFSDataSet.spTamogathato_korok);
        }

        public override void tsNew_Click(object sender, EventArgs e)
        {
            dgvTamogKorok.CurrentCell = dgvTamogKorok.Rows[dgvTamogKorok.RowCount - 1].Cells[1];
        }

        public override void tsSave_Click(object sender, EventArgs e)
        {
            save();
        }

        public override void save()
        {
            dgvTamogKorok.EndEdit();                  // szerkesztőmód befejezése
            dgvTamogKorok.CurrentCell = dgvTamogKorok.CurrentRow.Cells[1];

            // üres-e a Grid
            int Count;
            try
            {
                Count = dgvTamogKorok.RowCount;
            }
            catch
            {
                Count = 0;
            }

            if (Count > 0)
            {
                if (!ell()) return;

                int Letezik = 0;

                for (int j = 0; j < dgvTamogKorok.RowCount - 1; j++)
                {
                    int id = int.Parse(dgvTamogKorok.Rows[j].Cells[0].Value.ToString());
                    scommand = new SqlCommand("spSelectTamogathato_korok", sconn);
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
                        scommand = new SqlCommand("spTamogathato_korokInsert", sconn);
                        scommand.CommandType = CommandType.StoredProcedure;

                        scommand.Parameters.Add(new SqlParameter("@id", SqlDbType.Int)).Value = 1;
                        scommand.Parameters.Add(new SqlParameter("@leiras", SqlDbType.VarChar, 255)).Value = dgvTamogKorok.Rows[j].Cells[1].Value.ToString();
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
                    else
                    {
                        // update

                        scommand = new SqlCommand("spTamogathato_korokUpdate", sconn);
                        scommand.CommandType = CommandType.StoredProcedure;

                        scommand.Parameters.Add(new SqlParameter("@id", SqlDbType.Int)).Value = id;
                        scommand.Parameters.Add(new SqlParameter("@leiras", SqlDbType.VarChar, 255)).Value = dgvTamogKorok.Rows[j].Cells[1].Value.ToString();
                        scommand.Parameters.Add(new SqlParameter("@modosit_neve", SqlDbType.VarChar, 80)).Value = User.name;

                        try
                        {
                            if (sconn.State == ConnectionState.Closed) sconn.Open();
                            scommand.ExecuteNonQuery();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("SQL Hiba " + ex.Message + ex.Data + "update");
                            TraceBejegyzes(ex.Message);
                        }
                    }
                }
            }
            else
            {
                //ha üres a Grid akkor csak Insert lehet
                for (int j = 0; j < dgvTamogKorok.RowCount - 1; j++)
                {
                    scommand = new SqlCommand("spTamogathato_korokInsert", sconn);
                    scommand.CommandType = CommandType.StoredProcedure;

                    scommand.Parameters.Add(new SqlParameter("@id", SqlDbType.Int)).Value = 1;
                    scommand.Parameters.Add(new SqlParameter("@leiras", SqlDbType.VarChar, 255)).Value = dgvTamogKorok.Rows[j].Cells[1].Value.ToString();
                    scommand.Parameters.Add(new SqlParameter("@modosit_neve", SqlDbType.VarChar, 80)).Value = User.name;

                    try
                    {
                        if (sconn.State == ConnectionState.Closed) sconn.Open();
                        scommand.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("SQL Hiba " + ex.Message + ex.Data + "insert2");
                        TraceBejegyzes(ex.Message);
                    }
                }
            }
            Reload();
            Mentve = true;
            Adatcimke.Text = "Mentve: " + (Mentve ? "OK" : "nem");

            dgvTamogKorok.CurrentCell = dgvTamogKorok.Rows[dgvTamogKorok.RowCount - 2].Cells[0];
        }

        private bool ell()
        {
            // kötelező mezők
            for (int i = 0; i < dgvTamogKorok.RowCount - 1; i++)
            {
                if (dgvTamogKorok.Rows[i].Cells[1].Value.ToString().Length == 0)
                {
                    MessageBox.Show("A leírás nem lehet üres!");
                    return false;
                }
            }

            // esetleges ismétlődés ellenőrzése
            string adat;
            int sum;

            sum = 0;
            adat = dgvTamogKorok.Rows[Sorindex].Cells[1].Value.ToString();
            for (int i = 0; i < dgvTamogKorok.RowCount - 1; i++)
            {
                if (dgvTamogKorok.Rows[i].Cells[1].Value.ToString() == adat)
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

        public override void tsExit_Click(object sender, EventArgs e)
        {
            if (!Mentve)
            {
                DialogResult dr = MessageBox.Show("Minden nem mentett adat el fog veszni! Biztosan kilép?", "Megerősítés", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
                if (dr == DialogResult.OK)
                {
                    Close();
                    Application.RemoveMessageFilter(this);
                }
            }
            else Close();
            Application.RemoveMessageFilter(this);
        }

        public override void tsDelete_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("Biztos törli a kijelölt rekordot? ",
                "Törlés?", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dr == DialogResult.Yes)
            {
                // id meghatározása
                int id = int.Parse(dgvTamogKorok.Rows[Sorindex].Cells[0].Value.ToString());
                string query1 = "delete from tamogathato_korok where tmkor_id=" + id + ";";
                scommand = new SqlCommand(query1, sconn);
                // rekord törlése
                try { scommand.ExecuteNonQuery(); }
                catch (Exception ex) { MessageBox.Show("SQL Hiba " + ex.Message); }
                Reload();
            }
        }

        private void TraceBejegyzes(string msg)
        {
            myListener.WriteLine("-" + DateTime.Now + ": " + msg);
            myListener.Flush();
            myListener.Close();
        }

        // A Gridben leütött billentyűknek a lekezelése
        #region IMessageFilter Members
        private const UInt32 WM_KEYDOWN = 0x0100;

        // 1: az Enter kezelése
        public bool PreFilterMessage(ref Message m)
        {
            if (m.Msg == WM_KEYDOWN)
            {
                Keys keyCode = (Keys)(int)m.WParam & Keys.KeyCode;

                if (keyCode == Keys.Enter)
                {
                    try
                    {
                        int iColumn = dgvTamogKorok.CurrentCell.ColumnIndex;
                        int iRow = dgvTamogKorok.CurrentCell.RowIndex;
                        if (iColumn != dgvTamogKorok.Columns.Count - 1)
                            dgvTamogKorok.CurrentCell = dgvTamogKorok[iColumn + 1, iRow];
                        else if (iRow != dgvTamogKorok.RowCount - 1)
                            dgvTamogKorok.CurrentCell = dgvTamogKorok[2, iRow + 1];
                        return true;
                    }
                    catch (Exception ex)
                    {
                        TraceBejegyzes(ex.Message);
                        return false;
                    }
                }
                return false;
            }
            return false;
        }

        // 2: A számjegyek és betűk kezelése
        private void MyDataGridViewInitializationMethod()
        {
            dgvTamogKorok.EditingControlShowing +=
        new DataGridViewEditingControlShowingEventHandler(dataGridView_EditingControlShowing);
        }

        private void dataGridView_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            e.Control.KeyPress +=
                new KeyPressEventHandler(Control_KeyPress);
        }

        private void Control_KeyPress(object sender, KeyPressEventArgs e)
        {

        }
        #endregion

        private void dgvTamogKorok_SelectionChanged(object sender, EventArgs e)
        {
            try
            {
                Sorindex = dgvTamogKorok.SelectedCells[0].RowIndex;
            }
            catch
            {
            }
        }

        private void dgvTamogKorok_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
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

        private void dgvTamogKorok_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            TraceBejegyzes(e.ToString());
        }

        private void dgvTamogKorok_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            dgvTamogKorok.BeginEdit(false);               // írható cellába belépéskor edit módba lépünk
        }

        private void TamogathatoKorok_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.RemoveMessageFilter(this);
        }

        private void TamogathatoKorok_Resize(object sender, EventArgs e)
        {
            dgvTamogKorok.Width = TamogathatoKorok.ActiveForm.Width - 74;
            dgvTamogKorok.Height = TamogathatoKorok.ActiveForm.Height - 200;
        }

        private void dgvTamogKorok_MouseMove(object sender, MouseEventArgs e)
        {
            DataGridView.HitTestInfo hit = dgvTamogKorok.HitTest(e.X, e.Y);
            if (hit.Type == DataGridViewHitTestType.Cell)
            {
                dgvTamogKorok.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                dgvTamogKorok.Rows[hit.RowIndex].Selected = true;
                dgvTamogKorok.DefaultCellStyle.SelectionBackColor = Color.LightYellow;
                dgvTamogKorok.DefaultCellStyle.SelectionForeColor = Color.DarkBlue;
            }
        }

        private void dgvTamogKorok_MouseLeave(object sender, EventArgs e)
        {
            dgvTamogKorok.Rows[dgvTamogKorok.CurrentCell.RowIndex].Selected = false;
        }
    }
}
