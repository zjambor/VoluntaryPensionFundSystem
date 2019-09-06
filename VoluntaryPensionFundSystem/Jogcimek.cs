using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Configuration;
using System.Data.SqlClient;
using System.Diagnostics;

namespace VoluntaryPensionFundSystem
{
    public partial class Jogcimek : VoluntaryPensionFundSystem.BaseForm, IMessageFilter
    {
        private SqlConnection sconn;
        private SqlCommand scommand; 
        
        int Sorindex;
        bool Mentve = true;

        // Napló logok:
        TextWriterTraceListener myListener = new TextWriterTraceListener(@"LOG\Jogcimek.log", "myListener");

        public Jogcimek(SqlConnection SqlConn)
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

            dgvJogcimek.Focus();
        }

        private void Jogcimek_Load(object sender, EventArgs e)
        {
            Reload();
            Application.AddMessageFilter(this);
        }

        private void Reload()
        {
            this.spJogcimekTableAdapter.Fill(this.vPFSDataSet.spJogcimek);
        }

        public override void tsNew_Click(object sender, EventArgs e)
        {
            dgvJogcimek.CurrentCell = dgvJogcimek.Rows[dgvJogcimek.RowCount - 1].Cells[1]; 
        }

        public override void tsSave_Click(object sender, EventArgs e)
        {
            save();
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

        public override void save()
        {
            dgvJogcimek.EndEdit();                  // szerkesztőmód befejezése
            dgvJogcimek.CurrentCell = dgvJogcimek.CurrentRow.Cells[2];

            // üres-e a Grid
            int Count;
            try
            {
                Count = dgvJogcimek.RowCount;
            }
            catch
            {
                Count = 0;
            }

            if (Count > 0)
            {
                if (!ell()) return;

                int Letezik = 0;

                for (int j = 0; j < dgvJogcimek.RowCount - 1; j++)
                {
                    int id = int.Parse(dgvJogcimek.Rows[j].Cells[0].Value.ToString());
                    scommand = new SqlCommand("spJogcimekSelect", sconn);
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
                        scommand = new SqlCommand("spJogcimekInsert", sconn);
                        scommand.CommandType = CommandType.StoredProcedure;

                        scommand.Parameters.Add(new SqlParameter("@id", SqlDbType.Int)).Value = 1;
                        scommand.Parameters.Add(new SqlParameter("@tipus", SqlDbType.VarChar, 1)).Value = dgvJogcimek.Rows[j].Cells[1].Value.ToString();
                        scommand.Parameters.Add(new SqlParameter("@megnevezes", SqlDbType.VarChar, 100)).Value = dgvJogcimek.Rows[j].Cells[2].Value.ToString();
                        scommand.Parameters.Add(new SqlParameter("@ervenyes", SqlDbType.VarChar, 1)).Value = dgvJogcimek.Rows[j].Cells[3].Value.ToString();
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

                        scommand = new SqlCommand("spJogcimekUpdate", sconn);
                        scommand.CommandType = CommandType.StoredProcedure;

                        scommand.Parameters.Add(new SqlParameter("@id", SqlDbType.Int)).Value = id;
                        scommand.Parameters.Add(new SqlParameter("@tipus", SqlDbType.VarChar, 1)).Value = dgvJogcimek.Rows[j].Cells[1].Value.ToString();
                        scommand.Parameters.Add(new SqlParameter("@megnevezes", SqlDbType.VarChar, 100)).Value = dgvJogcimek.Rows[j].Cells[2].Value.ToString();
                        scommand.Parameters.Add(new SqlParameter("@ervenyes", SqlDbType.VarChar, 1)).Value = dgvJogcimek.Rows[j].Cells[3].Value.ToString();
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
                for (int j = 0; j < dgvJogcimek.RowCount - 1; j++)
                {
                    scommand = new SqlCommand("spJogcimekInsert", sconn);
                    scommand.CommandType = CommandType.StoredProcedure;

                    scommand.Parameters.Add(new SqlParameter("@id", SqlDbType.Int)).Value = 1;
                    scommand.Parameters.Add(new SqlParameter("@tipus", SqlDbType.VarChar, 1)).Value = dgvJogcimek.Rows[j].Cells[1].Value.ToString();
                    scommand.Parameters.Add(new SqlParameter("@megnevezes", SqlDbType.VarChar, 100)).Value = dgvJogcimek.Rows[j].Cells[2].Value.ToString();
                    scommand.Parameters.Add(new SqlParameter("@ervenyes", SqlDbType.VarChar, 1)).Value = dgvJogcimek.Rows[j].Cells[3].Value.ToString();
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

            dgvJogcimek.CurrentCell = dgvJogcimek.Rows[dgvJogcimek.RowCount - 2].Cells[0];
        }

        private bool ell()
        {
            // kötelező mezők
            for (int i = 0; i < dgvJogcimek.RowCount - 1; i++)
            {
                if (dgvJogcimek.Rows[i].Cells[1].Value.ToString().Length == 0 || dgvJogcimek.Rows[i].Cells[2].Value.ToString().Length == 0 ||
                    dgvJogcimek.Rows[i].Cells[3].Value.ToString().Length == 0)
                {
                    MessageBox.Show("Típus, megnevezés és érvényes mező nem lehet üres!");
                    return false;
                }
            }

            // esetleges ismétlődés ellenőrzése
            string adat;
            int sum;

            sum = 0;
            adat = dgvJogcimek.Rows[Sorindex].Cells[2].Value.ToString();
            for (int i = 0; i < dgvJogcimek.RowCount - 1; i++)
            {
                if (dgvJogcimek.Rows[i].Cells[2].Value.ToString() == adat)
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
            DialogResult dr = MessageBox.Show("Biztos törli a kijelölt rekordot? ",
                "Törlés?", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dr == DialogResult.Yes)
            {
                // id meghatározása
                int id = int.Parse(dgvJogcimek.Rows[Sorindex].Cells[0].Value.ToString());
                string query1 = "delete from jogcimek where jcm_id=" + id + ";";
                scommand = new SqlCommand(query1, sconn);
                // rekord törlése
                try { scommand.ExecuteNonQuery(); }
                catch (Exception ex) { MessageBox.Show("SQL Hiba " + ex.Message); }
                Reload();
            }
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
                        int iColumn = dgvJogcimek.CurrentCell.ColumnIndex;
                        int iRow = dgvJogcimek.CurrentCell.RowIndex;
                        if (iColumn != dgvJogcimek.Columns.Count - 1)
                            dgvJogcimek.CurrentCell = dgvJogcimek[iColumn + 1, iRow];
                        else if (iRow != dgvJogcimek.RowCount - 1)
                            dgvJogcimek.CurrentCell = dgvJogcimek[2, iRow + 1];
                        return true;
                    }
                    catch
                    {
                        //TraceBejegyzes(ex.Message);
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
            dgvJogcimek.EditingControlShowing +=
        new DataGridViewEditingControlShowingEventHandler(dataGridView_EditingControlShowing);
        }

        private void dataGridView_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            e.Control.KeyPress +=
                new KeyPressEventHandler(Control_KeyPress);
        }

        private void Control_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (((char.IsNumber(e.KeyChar) || char.IsControl(e.KeyChar)) && this.dgvJogcimek.CurrentCell.ColumnIndex == 2) || (this.dgvJogcimek.CurrentCell.ColumnIndex != 2))
            {
                // engedjük a beírást, ha a számlaszámhoz számjegyeket írunk vagy máshol vagyunk
                Mentve = false;
                Adatcimke.Text = "Mentve: " + (Mentve ? "OK" : "nem");
            }
            else
            {
                e.Handled = true;               // a számlaszámhoz mást nem engedünk beírni
            }
        }
        #endregion

        private void TraceBejegyzes(string msg)
        {
            myListener.WriteLine("-" + DateTime.Now + ": " + msg);
            myListener.Flush();
            myListener.Close();
        }

        private void dgvJogcimek_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
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

        private void Jogcimek_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.RemoveMessageFilter(this);
        }

        private void Jogcimek_Resize(object sender, EventArgs e)
        {
            dgvJogcimek.Width = Jogcimek.ActiveForm.Width - 93;
            dgvJogcimek.Height = Jogcimek.ActiveForm.Height - 206;
        }

        private void dgvJogcimek_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            TraceBejegyzes(e.ToString());
        }

        private void dgvJogcimek_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            dgvJogcimek.BeginEdit(false);               // írható cellába belépéskor edit módba lépünk
        }

        private void dgvJogcimek_SelectionChanged(object sender, EventArgs e)
        {
            try
            {
                Sorindex = dgvJogcimek.SelectedCells[0].RowIndex;
            }
            catch
            {
            }
        }

        private void dgvJogcimek_MouseMove(object sender, MouseEventArgs e)
        {
            DataGridView.HitTestInfo hit = dgvJogcimek.HitTest(e.X, e.Y);
            if (hit.Type == DataGridViewHitTestType.Cell)
            {
                dgvJogcimek.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                dgvJogcimek.Rows[hit.RowIndex].Selected = true;
                dgvJogcimek.DefaultCellStyle.SelectionBackColor = Color.LightYellow;
                dgvJogcimek.DefaultCellStyle.SelectionForeColor = Color.DarkBlue;
            }
        }

        private void dgvJogcimek_MouseLeave(object sender, EventArgs e)
        {
            dgvJogcimek.Rows[dgvJogcimek.CurrentCell.RowIndex].Selected = false;
        }
    }
}
