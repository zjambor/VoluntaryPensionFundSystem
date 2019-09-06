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
    public partial class Idoszakok : VoluntaryPensionFundSystem.BaseForm, IMessageFilter
    {
        private SqlConnection sconn;
        private SqlCommand scommand;

        List<string> ervL = new List<string>();
        int Sorindex;
        bool Mentve = true;

        // Napló logok:
        TextWriterTraceListener myListener = new TextWriterTraceListener(@"LOG\Idoszakok.log", "myListener");

        public Idoszakok(SqlConnection SqlConn)
        {
            InitializeComponent();

            this.sconn = SqlConn;

            ervL.Add("I");
            ervL.Add("N");
            ervL.Add(string.Empty);

            tsDelete.Enabled = true;
            tsFind.Enabled = false;
            tsNew.Enabled = true;
            tsSave.Enabled = true;
            tsSearch.Enabled = false;
            tsUpdate.Enabled = false;

            if (sconn.State == ConnectionState.Closed) sconn.Open();

            Adatcimke.Text = "Mentve: " + (Mentve ? "OK" : "nem");

            dgwIdoszakok.Focus();
        }

        private void Idoszakok_Load(object sender, EventArgs e)
        {
            Reload();
            Application.AddMessageFilter(this);
        }

        private void Reload()
        {
            this.spIdoszakokTableAdapter.Fill(this.vPFSDataSet.spIdoszakok);
        }

        public override void tsNew_Click(object sender, EventArgs e)
        {
            dgwIdoszakok.CurrentCell = dgwIdoszakok.Rows[dgwIdoszakok.RowCount-1].Cells[1];
        }

        public override void tsSave_Click(object sender, EventArgs e)
        {
            save();
        }

        public override void tsUpdate_Click(object sender, EventArgs e)
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
            exit();
        }

        public override void tsDelete_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("Biztos törli a kijelölt rekordot? ",
                "Törlés?", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dr == DialogResult.Yes)
            {
                // id meghatározása
                int id = int.Parse(dgwIdoszakok.Rows[Sorindex].Cells[0].Value.ToString());
                string query1 = "delete from idoszakok where idk_id=" + id + ";";
                scommand = new SqlCommand(query1, sconn);
                // rekord törlése
                try { scommand.ExecuteNonQuery(); }
                catch (Exception ex) { MessageBox.Show("SQL Hiba " + ex.Message); }
                Reload();
            }
        }

        public override void save()
        {
            dgwIdoszakok.EndEdit();                  // szerkesztőmód befejezése
            dgwIdoszakok.CurrentCell = dgwIdoszakok.CurrentRow.Cells[2];
            
            // üres-e a Grid
            int Count;
            try
            {
                Count = dgwIdoszakok.RowCount;
            }
            catch
            {
                Count = 0;
            }

            if (Count > 0)
            {
                if (!ell()) return;

                int Letezik = 0;
                string Kezdete;
                string Vege;
                for (int j = 0; j < dgwIdoszakok.RowCount - 1; j++)
                {
                    int id = int.Parse(dgwIdoszakok.Rows[j].Cells[0].Value.ToString());
                    scommand = new SqlCommand("spSelectIdoszak", sconn);
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

                    // A kezdete és vége dátumok előállítása az ev_ho alapján
                    try
                    {
                        int year = int.Parse(dgwIdoszakok.Rows[j].Cells[1].Value.ToString().Substring(0, 4));
                        int month = int.Parse(dgwIdoszakok.Rows[j].Cells[1].Value.ToString().Substring(4, 2));
                        int day = DateTime.DaysInMonth(year, month);
                        Kezdete = year.ToString() + "." + month.ToString() + ".01";
                        Vege = year.ToString() + "." + month.ToString() + "." + day.ToString();
                    }
                    catch
                    {
                        MessageBox.Show("Hibás időszak!");
                        return;
                    }

                    if (Letezik == 0)
                    {
                        // insert
                        scommand = new SqlCommand("spIdoszakInsert", sconn);
                        scommand.CommandType = CommandType.StoredProcedure;

                        scommand.Parameters.Add(new SqlParameter("@id", SqlDbType.Int)).Value = 1;
                        scommand.Parameters.Add(new SqlParameter("@ev_ho", SqlDbType.Int)).Value = dgwIdoszakok.Rows[j].Cells[1].Value.ToString();
                        scommand.Parameters.Add(new SqlParameter("@kezdete", SqlDbType.Date)).Value = DateTime.Parse(Kezdete);
                        scommand.Parameters.Add(new SqlParameter("@vege", SqlDbType.Date)).Value = DateTime.Parse(Vege);
                        scommand.Parameters.Add(new SqlParameter("@ervenyes", SqlDbType.VarChar, 1)).Value = dgwIdoszakok.Rows[j].Cells[4].Value.ToString();
                        scommand.Parameters.Add(new SqlParameter("@megjegyzes", SqlDbType.VarChar, 255)).Value = dgwIdoszakok.Rows[j].Cells[5].Value.ToString();
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

                        scommand = new SqlCommand("spIdoszakUpdate", sconn);
                        scommand.CommandType = CommandType.StoredProcedure;

                        scommand.Parameters.Add(new SqlParameter("@id", SqlDbType.Int)).Value = id;
                        scommand.Parameters.Add(new SqlParameter("@ev_ho", SqlDbType.Int)).Value = dgwIdoszakok.Rows[j].Cells[1].Value.ToString();
                        scommand.Parameters.Add(new SqlParameter("@kezdete", SqlDbType.Date)).Value = DateTime.Parse(Kezdete);
                        scommand.Parameters.Add(new SqlParameter("@vege", SqlDbType.Date)).Value = DateTime.Parse(Vege);
                        scommand.Parameters.Add(new SqlParameter("@ervenyes", SqlDbType.VarChar, 1)).Value = dgwIdoszakok.Rows[j].Cells[4].Value.ToString();
                        scommand.Parameters.Add(new SqlParameter("@megjegyzes", SqlDbType.VarChar, 255)).Value = dgwIdoszakok.Rows[j].Cells[5].Value.ToString();
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
                string Kezdete;
                string Vege;

                for (int j = 0; j < dgwIdoszakok.RowCount - 1; j++)
                {
                    // A kezdete és vége dátumok előállítása az ev_ho alapján
                    int year = int.Parse(dgwIdoszakok.Rows[j].Cells[1].Value.ToString().Substring(0, 4));
                    int month = int.Parse(dgwIdoszakok.Rows[j].Cells[1].Value.ToString().Substring(4, 2));
                    int day = DateTime.DaysInMonth(year, month);
                    Kezdete = year.ToString() + "." + month.ToString() + ".01";
                    Vege = year.ToString() + "." + month.ToString() + "." + day.ToString();
                    
                    scommand = new SqlCommand("spIdoszakInsert", sconn);
                    scommand.CommandType = CommandType.StoredProcedure;

                    scommand.Parameters.Add(new SqlParameter("@id", SqlDbType.Int)).Value = 1;
                    scommand.Parameters.Add(new SqlParameter("@ev_ho", SqlDbType.Int)).Value = dgwIdoszakok.Rows[j].Cells[1].Value.ToString();
                    scommand.Parameters.Add(new SqlParameter("@kezdete", SqlDbType.Date)).Value = DateTime.Parse(Kezdete);
                    scommand.Parameters.Add(new SqlParameter("@vege", SqlDbType.Date)).Value = DateTime.Parse(Vege);
                    scommand.Parameters.Add(new SqlParameter("@ervenyes", SqlDbType.VarChar, 1)).Value = dgwIdoszakok.Rows[j].Cells[4].Value.ToString();
                    scommand.Parameters.Add(new SqlParameter("@megjegyzes", SqlDbType.VarChar, 255)).Value = dgwIdoszakok.Rows[j].Cells[5].Value.ToString();
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

            dgwIdoszakok.CurrentCell = dgwIdoszakok.Rows[dgwIdoszakok.RowCount - 2].Cells[1];
        }

        private bool ell()
        {
            // esetleges ismétlődés ellenőrzése
            string adat;
            int sum;

            sum = 0;
            adat = dgwIdoszakok.Rows[Sorindex].Cells[1].Value.ToString();
            for (int i = 0; i < dgwIdoszakok.RowCount - 1; i++)
            {
                if (dgwIdoszakok.Rows[i].Cells[1].Value.ToString() == adat)
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

        public void exit()
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

        private void Bankszamlak_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.RemoveMessageFilter(this);
        }

        private void dgwIdoszakok_SelectionChanged(object sender, EventArgs e)
        {
            try
            {
                Sorindex = dgwIdoszakok.SelectedCells[0].RowIndex;
                if (dgwIdoszakok.Rows[Sorindex].Cells[4].Value.ToString() != "I" && dgwIdoszakok.Rows[Sorindex].Cells[4].Value.ToString() != "N")
                {
                    dgwIdoszakok.Rows[Sorindex].Cells[4].Value = "I";
                }
            }
            catch
            {
            }
        }

        private void dgwIdoszakok_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            dgwIdoszakok.BeginEdit(false);               // írható cellába belépéskor edit módba lépünk
        }

        private void dgwIdoszakok_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.ColumnIndex == 0 || e.ColumnIndex == 2 || e.ColumnIndex == 3)
            {
                // a readonly mezők szürke színűek
                e.CellStyle.BackColor = SystemColors.Control;
            }
            else
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
        }

        private void dgwIdoszakok_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            TraceBejegyzes(e.ToString());
        }

        private void TraceBejegyzes(string msg)
        {
            myListener.WriteLine("-" + DateTime.Now + ": " + msg);
            myListener.Flush();
            myListener.Close();
        }

        private void Idoszakok_Resize(object sender, EventArgs e)
        {
            dgwIdoszakok.Width = Idoszakok.ActiveForm.Width - 83;
            dgwIdoszakok.Height = Idoszakok.ActiveForm.Height - 196;
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
                        int iColumn = dgwIdoszakok.CurrentCell.ColumnIndex;
                        int iRow = dgwIdoszakok.CurrentCell.RowIndex;
                        if (iColumn != dgwIdoszakok.Columns.Count - 1)
                            dgwIdoszakok.CurrentCell = dgwIdoszakok[iColumn + 1, iRow];
                        else if (iRow != dgwIdoszakok.RowCount - 1)
                            dgwIdoszakok.CurrentCell = dgwIdoszakok[2, iRow + 1];
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
            dgwIdoszakok.EditingControlShowing +=
        new DataGridViewEditingControlShowingEventHandler(dataGridView_EditingControlShowing);
        }

        private void dataGridView_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            e.Control.KeyPress +=
                new KeyPressEventHandler(Control_KeyPress);
        }

        private void Control_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (((char.IsNumber(e.KeyChar) || char.IsControl(e.KeyChar)) && this.dgwIdoszakok.CurrentCell.ColumnIndex == 1) || (this.dgwIdoszakok.CurrentCell.ColumnIndex != 1))
            {
                // engedjük a beírást, ha az időszakhoz számjegyeket írunk vagy máshol vagyunk
                Mentve = false;
                Adatcimke.Text = "Mentve: " + (Mentve ? "OK" : "nem");
            }
            else
            {
                e.Handled = true;
            }
        }
        #endregion

        private void dgwIdoszakok_MouseMove(object sender, MouseEventArgs e)
        {
            DataGridView.HitTestInfo hit = dgwIdoszakok.HitTest(e.X, e.Y);
            if (hit.Type == DataGridViewHitTestType.Cell)
            {
                dgwIdoszakok.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                dgwIdoszakok.Rows[hit.RowIndex].Selected = true;
                dgwIdoszakok.DefaultCellStyle.SelectionBackColor = Color.LightYellow;
                dgwIdoszakok.DefaultCellStyle.SelectionForeColor = Color.DarkBlue;
            }
        }

        private void dgwIdoszakok_MouseLeave(object sender, EventArgs e)
        {
            dgwIdoszakok.Rows[dgwIdoszakok.CurrentCell.RowIndex].Selected = false;
        }
    }
}
