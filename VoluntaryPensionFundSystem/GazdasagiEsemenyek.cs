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
    public partial class GazdasagiEsemenyek : BaseForm, IMessageFilter
    {
        private SqlConnection sconn;
        private SqlCommand scommand;

        int Sorindex;
        bool Mentve = true;

        // Napló logok:
        TextWriterTraceListener myListener = new TextWriterTraceListener(@"LOG\GazdasagiEsemenyek.log", "myListener");

        public GazdasagiEsemenyek(SqlConnection SqlConn)
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

            dgvGenykod.Focus();
        }

        public void rejtId()
        {
            dgvGenykod.Columns[0].Visible = false;
        }

        private void GazdasagiEsemenyek_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'vPFSDataSet.spGazdasagi_esemenyek' table. You can move, or remove it, as needed.
            Reload();
            Application.AddMessageFilter(this);
        }

        private void Reload()
        {
            this.spGazdasagi_esemenyekTableAdapter.Fill(this.vPFSDataSet.spGazdasagi_esemenyek);
            rejtId();
        }

        public override void tsNew_Click(object sender, EventArgs e)
        {
            dgvGenykod.CurrentCell = dgvGenykod.Rows[dgvGenykod.RowCount - 1].Cells[1];
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
            dgvGenykod.EndEdit();                  // szerkesztőmód befejezése
            dgvGenykod.CurrentCell = dgvGenykod.CurrentRow.Cells[2];

            // üres-e a Grid
            int Count;
            try
            {
                Count = dgvGenykod.RowCount;
            }
            catch
            {
                Count = 0;
            }

            if (Count > 0)
            {
                if (!ell()) return;

                int Letezik = 0;

                for (int j = 0; j < dgvGenykod.RowCount - 1; j++)
                {
                    int id = int.Parse(dgvGenykod.Rows[j].Cells[0].Value.ToString());
                    scommand = new SqlCommand("spSelectGazdasagi_esemenyek", sconn);
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
                        scommand = new SqlCommand("spGazdasagi_esemenyekInsert", sconn);
                        scommand.CommandType = CommandType.StoredProcedure;

                        scommand.Parameters.Add(new SqlParameter("@id", SqlDbType.Int)).Value = 1;
                        scommand.Parameters.Add(new SqlParameter("@geny_kod", SqlDbType.VarChar, 25)).Value = dgvGenykod.Rows[j].Cells[1].Value.ToString();
                        scommand.Parameters.Add(new SqlParameter("@leiras", SqlDbType.VarChar, 255)).Value = dgvGenykod.Rows[j].Cells[2].Value.ToString();
                        scommand.Parameters.Add(new SqlParameter("@megjegyzes", SqlDbType.VarChar, 255)).Value = dgvGenykod.Rows[j].Cells[3].Value.ToString();
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

                        scommand = new SqlCommand("spGazdasagi_esemenyekUpdate", sconn);
                        scommand.CommandType = CommandType.StoredProcedure;

                        scommand.Parameters.Add(new SqlParameter("@id", SqlDbType.Int)).Value = id;
                        scommand.Parameters.Add(new SqlParameter("@geny_kod", SqlDbType.VarChar, 25)).Value = dgvGenykod.Rows[j].Cells[1].Value.ToString();
                        scommand.Parameters.Add(new SqlParameter("@leiras", SqlDbType.VarChar, 255)).Value = dgvGenykod.Rows[j].Cells[2].Value.ToString();
                        scommand.Parameters.Add(new SqlParameter("@megjegyzes", SqlDbType.VarChar, 255)).Value = dgvGenykod.Rows[j].Cells[3].Value.ToString();
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
                for (int j = 0; j < dgvGenykod.RowCount - 1; j++)
                {
                    scommand = new SqlCommand("spGazdasagi_esemenyekInsert", sconn);
                    scommand.CommandType = CommandType.StoredProcedure;

                    scommand.Parameters.Add(new SqlParameter("@id", SqlDbType.Int)).Value = 1;
                    scommand.Parameters.Add(new SqlParameter("@geny_kod", SqlDbType.VarChar, 25)).Value = dgvGenykod.Rows[j].Cells[1].Value.ToString();
                    scommand.Parameters.Add(new SqlParameter("@leiras", SqlDbType.VarChar, 255)).Value = dgvGenykod.Rows[j].Cells[2].Value.ToString();
                    scommand.Parameters.Add(new SqlParameter("@megjegyzes", SqlDbType.VarChar, 255)).Value = dgvGenykod.Rows[j].Cells[3].Value.ToString();
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

            dgvGenykod.CurrentCell = dgvGenykod.Rows[dgvGenykod.RowCount - 2].Cells[1];
        }

        private bool ell()
        {
            // kötelező mezők
            for (int i = 0; i < dgvGenykod.RowCount - 1; i++)
            {
                if (dgvGenykod.Rows[i].Cells[1].Value.ToString().Length == 0)
                {
                    MessageBox.Show("Gazd. esemény kód nem lehet üres!");
                    return false;
                }
            }

            // esetleges ismétlődés ellenőrzése
            string adat;
            int sum;

            sum = 0;
            adat = dgvGenykod.Rows[Sorindex].Cells[1].Value.ToString();
            for (int i = 0; i < dgvGenykod.RowCount - 1; i++)
            {
                if (dgvGenykod.Rows[i].Cells[1].Value.ToString() == adat)
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
                int id = int.Parse(dgvGenykod.Rows[Sorindex].Cells[0].Value.ToString());
                string query1 = "delete from gazdasagi_esemenyek where geny_id=" + id + ";";
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
                        int iColumn = dgvGenykod.CurrentCell.ColumnIndex;
                        int iRow = dgvGenykod.CurrentCell.RowIndex;
                        if (iColumn != dgvGenykod.Columns.Count - 1)
                            dgvGenykod.CurrentCell = dgvGenykod[iColumn + 1, iRow];
                        else if (iRow != dgvGenykod.RowCount - 1)
                            dgvGenykod.CurrentCell = dgvGenykod[2, iRow + 1];
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
            dgvGenykod.EditingControlShowing +=
        new DataGridViewEditingControlShowingEventHandler(dataGridView_EditingControlShowing);
        }

        private void dataGridView_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            e.Control.KeyPress +=
                new KeyPressEventHandler(Control_KeyPress);
        }

        private void Control_KeyPress(object sender, KeyPressEventArgs e)
        {
            Mentve = false;
            Adatcimke.Text = "Mentve: " + (Mentve ? "OK" : "nem");
            //if (((char.IsNumber(e.KeyChar) || char.IsControl(e.KeyChar)) && this.dgvGenykod.CurrentCell.ColumnIndex == 2) || (this.dgvGenykod.CurrentCell.ColumnIndex != 2))
            //{
            //    // engedjük a beírást, ha a számlaszámhoz számjegyeket írunk vagy máshol vagyunk
            //    Mentve = false;
            //    Adatcimke.Text = "Mentve: " + (Mentve ? "OK" : "nem");
            //}
            //else
            //{
            //    e.Handled = true;               // a számlaszámhoz mást nem engedünk beírni
            //}
        }
        #endregion

        private void dgvGenykod_SelectionChanged(object sender, EventArgs e)
        {
            try
            {
                Sorindex = dgvGenykod.SelectedCells[0].RowIndex;
            }
            catch
            {
            }
        }

        private void dgvGenykod_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
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

        private void dgvGenykod_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            TraceBejegyzes(e.ToString());
        }

        private void dgvGenykod_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            dgvGenykod.BeginEdit(false);               // írható cellába belépéskor edit módba lépünk
        }

        private void GazdasagiEsemenyek_Resize(object sender, EventArgs e)
        {
            dgvGenykod.Width = Nyugdijpenztarak.ActiveForm.Width - 83;
            dgvGenykod.Height = Nyugdijpenztarak.ActiveForm.Height - 193;
        }

        private void GazdasagiEsemenyek_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.RemoveMessageFilter(this);
        }

        private void dgvGenykod_MouseMove(object sender, MouseEventArgs e)
        {
            DataGridView.HitTestInfo hit = dgvGenykod.HitTest(e.X, e.Y);
            if (hit.Type == DataGridViewHitTestType.Cell)
            {
                dgvGenykod.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                dgvGenykod.Rows[hit.RowIndex].Selected = true;
                dgvGenykod.DefaultCellStyle.SelectionBackColor = Color.LightYellow;
                dgvGenykod.DefaultCellStyle.SelectionForeColor = Color.DarkBlue;
            }
        }

        private void dgvGenykod_MouseLeave(object sender, EventArgs e)
        {
            dgvGenykod.Rows[dgvGenykod.CurrentCell.RowIndex].Selected = false;
        }
    }
}
