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
    public partial class Hibak : VoluntaryPensionFundSystem.BaseForm, IMessageFilter
    {
        private SqlConnection sconn;
        private SqlCommand scommand;

        List<string> ervL = new List<string>();
        int Sorindex;
        bool Mentve = true;

        // Napló logok:
        TextWriterTraceListener myListener = new TextWriterTraceListener(@"LOG\Hibak.log", "myListener");

        public Hibak(SqlConnection SqlConn)
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

            dgwHibak.Focus();
        }

        private void Hibak_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'vPFSDataSet.spHibak' table. You can move, or remove it, as needed.
            Reload();
            Application.AddMessageFilter(this);
        }

        private void Reload()
        {
            this.spHibakTableAdapter.Fill(this.vPFSDataSet.spHibak);
            rejtId();
        }

        public void rejtId()
        {
            //dgwHibak.Columns[0].Visible = false;
        }

        public override void tsNew_Click(object sender, EventArgs e)
        {
            dgwHibak.CurrentCell = dgwHibak.Rows[dgwHibak.RowCount - 1].Cells[1]; 
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
            dgwHibak.EndEdit();                  // szerkesztőmód befejezése
            dgwHibak.CurrentCell = dgwHibak.CurrentRow.Cells[2];

            // üres-e a Grid
            int Count;
            try
            {
                Count = dgwHibak.RowCount;
            }
            catch
            {
                Count = 0;
            }

            if (Count > 0)
            {
                if (!ell()) return;

                int Letezik = 0;

                for (int j = 0; j < dgwHibak.RowCount - 1; j++)
                {
                    int id = int.Parse(dgwHibak.Rows[j].Cells[0].Value.ToString());
                    scommand = new SqlCommand("spSelectHibak", sconn);
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
                        scommand = new SqlCommand("spHibakInsert", sconn);
                        scommand.CommandType = CommandType.StoredProcedure;

                        scommand.Parameters.Add(new SqlParameter("@id", SqlDbType.Int)).Value = 1;
                        scommand.Parameters.Add(new SqlParameter("@leiras", SqlDbType.VarChar, 255)).Value = dgwHibak.Rows[j].Cells[1].Value.ToString();
                        scommand.Parameters.Add(new SqlParameter("@bev_ervenyes", SqlDbType.VarChar, 1)).Value = dgwHibak.Rows[j].Cells[2].Value.ToString();
                        scommand.Parameters.Add(new SqlParameter("@ervenyes", SqlDbType.VarChar, 1)).Value = dgwHibak.Rows[j].Cells[3].Value.ToString();
                        scommand.Parameters.Add(new SqlParameter("@megjegyzes", SqlDbType.VarChar, 255)).Value = dgwHibak.Rows[j].Cells[4].Value.ToString();
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

                        scommand = new SqlCommand("spHibakUpdate", sconn);
                        scommand.CommandType = CommandType.StoredProcedure;

                        scommand.Parameters.Add(new SqlParameter("@id", SqlDbType.Int)).Value = id;
                        scommand.Parameters.Add(new SqlParameter("@leiras", SqlDbType.VarChar, 255)).Value = dgwHibak.Rows[j].Cells[1].Value.ToString();
                        scommand.Parameters.Add(new SqlParameter("@bev_ervenyes", SqlDbType.VarChar, 1)).Value = dgwHibak.Rows[j].Cells[2].Value.ToString();
                        scommand.Parameters.Add(new SqlParameter("@ervenyes", SqlDbType.VarChar, 1)).Value = dgwHibak.Rows[j].Cells[3].Value.ToString();
                        scommand.Parameters.Add(new SqlParameter("@megjegyzes", SqlDbType.VarChar, 255)).Value = dgwHibak.Rows[j].Cells[4].Value.ToString();
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
                for (int j = 0; j < dgwHibak.RowCount - 1; j++)
                {
                    scommand = new SqlCommand("spHibakInsert", sconn);
                    scommand.CommandType = CommandType.StoredProcedure;

                    scommand.Parameters.Add(new SqlParameter("@id", SqlDbType.Int)).Value = 1;
                    scommand.Parameters.Add(new SqlParameter("@leiras", SqlDbType.VarChar, 255)).Value = dgwHibak.Rows[j].Cells[1].Value.ToString();
                    scommand.Parameters.Add(new SqlParameter("@bev_ervenyes", SqlDbType.VarChar, 1)).Value = dgwHibak.Rows[j].Cells[2].Value.ToString();
                    scommand.Parameters.Add(new SqlParameter("@ervenyes", SqlDbType.VarChar, 1)).Value = dgwHibak.Rows[j].Cells[3].Value.ToString();
                    scommand.Parameters.Add(new SqlParameter("@megjegyzes", SqlDbType.VarChar, 255)).Value = dgwHibak.Rows[j].Cells[4].Value.ToString();
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

            dgwHibak.CurrentCell = dgwHibak.Rows[dgwHibak.RowCount - 2].Cells[1];
        }

        private bool ell()
        {
            // esetleges ismétlődés ellenőrzése
            string adat;
            int sum;

            sum = 0;
            adat = dgwHibak.Rows[Sorindex].Cells[1].Value.ToString();
            for (int i = 0; i < dgwHibak.RowCount - 1; i++)
            {
                if (dgwHibak.Rows[i].Cells[1].Value.ToString() == adat)
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
                int id = int.Parse(dgwHibak.Rows[Sorindex].Cells[0].Value.ToString());
                string query1 = "delete from hibak where hiba_id=" + id + ";";
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
                        int iColumn = dgwHibak.CurrentCell.ColumnIndex;
                        int iRow = dgwHibak.CurrentCell.RowIndex;
                        if (iColumn != dgwHibak.Columns.Count - 1)
                            dgwHibak.CurrentCell = dgwHibak[iColumn + 1, iRow];
                        else if (iRow != dgwHibak.RowCount - 1)
                            dgwHibak.CurrentCell = dgwHibak[2, iRow + 1];
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
            dgwHibak.EditingControlShowing +=
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
        }
        #endregion

        private void dgwHibak_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
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

        private void Hibak_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.RemoveMessageFilter(this);
        }

        private void dgwHibak_SelectionChanged(object sender, EventArgs e)
        {
            try
            {
                Sorindex = dgwHibak.SelectedCells[0].RowIndex;
                if (dgwHibak.Rows[Sorindex].Cells[3].Value.ToString() != "I" && dgwHibak.Rows[Sorindex].Cells[3].Value.ToString() != "N")
                {
                    dgwHibak.Rows[Sorindex].Cells[3].Value = "I";
                }
            }
            catch
            {
            }
        }

        private void dgwHibak_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            TraceBejegyzes(e.ToString());
        }

        private void Hibak_Resize(object sender, EventArgs e)
        {
            dgwHibak.Width = Hibak.ActiveForm.Width - 97;
            dgwHibak.Height = Hibak.ActiveForm.Height - 190;
        }

        private void dgwHibak_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            dgwHibak.BeginEdit(false);               // írható cellába belépéskor edit módba lépünk
        }

        private void dgwHibak_MouseMove(object sender, MouseEventArgs e)
        {
            DataGridView.HitTestInfo hit = dgwHibak.HitTest(e.X, e.Y);
            if (hit.Type == DataGridViewHitTestType.Cell)
            {
                dgwHibak.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                dgwHibak.Rows[hit.RowIndex].Selected = true;
                dgwHibak.DefaultCellStyle.SelectionBackColor = Color.LightYellow;
                dgwHibak.DefaultCellStyle.SelectionForeColor = Color.DarkBlue;
            }
        }

        private void dgwHibak_MouseLeave(object sender, EventArgs e)
        {
            dgwHibak.Rows[dgwHibak.CurrentCell.RowIndex].Selected = false;
        }
    }
}
