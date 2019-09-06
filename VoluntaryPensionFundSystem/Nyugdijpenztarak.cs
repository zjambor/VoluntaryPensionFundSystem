using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Configuration;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace VoluntaryPensionFundSystem
{
    public partial class Nyugdijpenztarak : BaseForm, IMessageFilter
    {
        private SqlConnection sconn;
        private SqlCommand scommand;
        
        List<string> ervL = new List<string>();
        int Sorindex;
        bool Mentve = true;

        // Napló logok:
        TextWriterTraceListener myListener = new TextWriterTraceListener(@"LOG\Nyugdijpenztarak.log", "myListener");

        public Nyugdijpenztarak(SqlConnection SqlConn)
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

            dgvNyugdijpenzt.Focus();
            dgvNyugdijpenzt.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        }

        public void rejtId()
        {
            //dgvNyugdijpenzt.Columns[0].Visible = false;
        }

        private void Nyugdijpenztarak_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'vPFSDataSet.spNyugdijpenztarak' table. You can move, or remove it, as needed.
            Reload();
            Application.AddMessageFilter(this);
        }

        private void Reload()
        {
            this.spNyugdijpenztarakTableAdapter.Fill(this.vPFSDataSet.spNyugdijpenztarak);
            rejtId();
        }

        public override void tsNew_Click(object sender, EventArgs e)
        {
            dgvNyugdijpenzt.CurrentCell = dgvNyugdijpenzt.Rows[dgvNyugdijpenzt.RowCount - 1].Cells[1]; 
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
            dgvNyugdijpenzt.EndEdit();                  // szerkesztőmód befejezése
            dgvNyugdijpenzt.CurrentCell = dgvNyugdijpenzt.CurrentRow.Cells[2];

            // üres-e a Grid
            int Count;
            try
            {
                Count = dgvNyugdijpenzt.RowCount;
            }
            catch
            {
                Count = 0;
            }

            if (Count > 0)
            {
                if (!ell()) return;

                int Letezik = 0;

                for (int j = 0; j < dgvNyugdijpenzt.RowCount - 1; j++)
                {
                    int id = int.Parse(dgvNyugdijpenzt.Rows[j].Cells[0].Value.ToString());
                    scommand = new SqlCommand("spSelectNyugdijpenztarak", sconn);
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
                        scommand = new SqlCommand("spNyugdijpenztarakInsert", sconn);
                        scommand.CommandType = CommandType.StoredProcedure;

                        scommand.Parameters.Add(new SqlParameter("@id", SqlDbType.Int)).Value = 1;
                        scommand.Parameters.Add(new SqlParameter("@nev", SqlDbType.VarChar, 100)).Value = dgvNyugdijpenzt.Rows[j].Cells[1].Value.ToString();
                        scommand.Parameters.Add(new SqlParameter("@ir_szam", SqlDbType.Decimal, 4)).Value = int.Parse(dgvNyugdijpenzt.Rows[j].Cells[2].Value.ToString());
                        scommand.Parameters.Add(new SqlParameter("@helyseg", SqlDbType.VarChar, 20)).Value = dgvNyugdijpenzt.Rows[j].Cells[3].Value.ToString();
                        scommand.Parameters.Add(new SqlParameter("@cim", SqlDbType.VarChar, 80)).Value = dgvNyugdijpenzt.Rows[j].Cells[4].Value.ToString();
                        scommand.Parameters.Add(new SqlParameter("@megjegyzes", SqlDbType.VarChar, 255)).Value = dgvNyugdijpenzt.Rows[j].Cells[5].Value.ToString();
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

                        scommand = new SqlCommand("spNyugdijpenztarakUpdate", sconn);
                        scommand.CommandType = CommandType.StoredProcedure;

                        scommand.Parameters.Add(new SqlParameter("@id", SqlDbType.Int)).Value = id;
                        scommand.Parameters.Add(new SqlParameter("@nev", SqlDbType.VarChar, 100)).Value = dgvNyugdijpenzt.Rows[j].Cells[1].Value.ToString();
                        scommand.Parameters.Add(new SqlParameter("@ir_szam", SqlDbType.Decimal, 4)).Value = int.Parse(dgvNyugdijpenzt.Rows[j].Cells[2].Value.ToString());
                        scommand.Parameters.Add(new SqlParameter("@helyseg", SqlDbType.VarChar, 20)).Value = dgvNyugdijpenzt.Rows[j].Cells[3].Value.ToString();
                        scommand.Parameters.Add(new SqlParameter("@cim", SqlDbType.VarChar, 80)).Value = dgvNyugdijpenzt.Rows[j].Cells[4].Value.ToString();
                        scommand.Parameters.Add(new SqlParameter("@megjegyzes", SqlDbType.VarChar, 255)).Value = dgvNyugdijpenzt.Rows[j].Cells[5].Value.ToString();
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
                for (int j = 0; j < dgvNyugdijpenzt.RowCount - 1; j++)
                {
                    scommand = new SqlCommand("spNyugdijpenztarakInsert", sconn);
                    scommand.CommandType = CommandType.StoredProcedure;

                    scommand.Parameters.Add(new SqlParameter("@id", SqlDbType.Int)).Value = 1;
                    scommand.Parameters.Add(new SqlParameter("@nev", SqlDbType.VarChar, 100)).Value = dgvNyugdijpenzt.Rows[j].Cells[1].Value.ToString();
                    scommand.Parameters.Add(new SqlParameter("@ir_szam", SqlDbType.Decimal, 4)).Value = int.Parse(dgvNyugdijpenzt.Rows[j].Cells[2].Value.ToString());
                    scommand.Parameters.Add(new SqlParameter("@helyseg", SqlDbType.VarChar, 20)).Value = dgvNyugdijpenzt.Rows[j].Cells[3].Value.ToString();
                    scommand.Parameters.Add(new SqlParameter("@cim", SqlDbType.VarChar, 80)).Value = dgvNyugdijpenzt.Rows[j].Cells[4].Value.ToString();
                    scommand.Parameters.Add(new SqlParameter("@megjegyzes", SqlDbType.VarChar, 255)).Value = dgvNyugdijpenzt.Rows[j].Cells[5].Value.ToString();
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

            dgvNyugdijpenzt.CurrentCell = dgvNyugdijpenzt.Rows[dgvNyugdijpenzt.RowCount - 2].Cells[0];
        }

        private bool ell()
        {
            // kötelező mezők
            for (int i = 0; i < dgvNyugdijpenzt.RowCount - 1; i++)
            {
                if (dgvNyugdijpenzt.Rows[i].Cells[1].Value.ToString().Length == 0 || dgvNyugdijpenzt.Rows[i].Cells[2].Value.ToString().Length == 0 ||
                    dgvNyugdijpenzt.Rows[i].Cells[3].Value.ToString().Length == 0 || dgvNyugdijpenzt.Rows[i].Cells[4].Value.ToString().Length == 0)
                {
                    MessageBox.Show("Név, irányítószám, helység, cím nem lehet üres!");
                    return false;
                }
            }

            // esetleges ismétlődés ellenőrzése
            string adat;
            int sum;

            sum = 0;
            adat = dgvNyugdijpenzt.Rows[Sorindex].Cells[1].Value.ToString();
            for (int i = 0; i < dgvNyugdijpenzt.RowCount - 1; i++)
            {
                if (dgvNyugdijpenzt.Rows[i].Cells[1].Value.ToString() == adat)
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
                int id = int.Parse(dgvNyugdijpenzt.Rows[Sorindex].Cells[0].Value.ToString());
                string query1 = "delete from nyugdijpenztarak where nyp_id=" + id + ";";
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
                        int iColumn = dgvNyugdijpenzt.CurrentCell.ColumnIndex;
                        int iRow = dgvNyugdijpenzt.CurrentCell.RowIndex;
                        if (iColumn != dgvNyugdijpenzt.Columns.Count - 1)
                            dgvNyugdijpenzt.CurrentCell = dgvNyugdijpenzt[iColumn + 1, iRow];
                        else if (iRow != dgvNyugdijpenzt.RowCount - 1)
                            dgvNyugdijpenzt.CurrentCell = dgvNyugdijpenzt[2, iRow + 1];
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
            dgvNyugdijpenzt.EditingControlShowing +=
        new DataGridViewEditingControlShowingEventHandler(dataGridView_EditingControlShowing);
        }

        private void dataGridView_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            e.Control.KeyPress +=
                new KeyPressEventHandler(Control_KeyPress);
        }

        private void Control_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (((char.IsNumber(e.KeyChar) || char.IsControl(e.KeyChar)) && this.dgvNyugdijpenzt.CurrentCell.ColumnIndex == 2) || (this.dgvNyugdijpenzt.CurrentCell.ColumnIndex != 2))
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

        private void dgvNyugdijpenzt_SelectionChanged(object sender, EventArgs e)
        {
            try
            {
                Sorindex = dgvNyugdijpenzt.SelectedCells[0].RowIndex;
            }
            catch
            {
            }
        }

        private void dgvNyugdijpenzt_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
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

        private void dgvNyugdijpenzt_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            TraceBejegyzes(e.ToString());
        }

        private void Nyugdijpenztarak_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.RemoveMessageFilter(this);
        }

        private void dgvNyugdijpenzt_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            dgvNyugdijpenzt.BeginEdit(false);               // írható cellába belépéskor edit módba lépünk
        }

        private void Nyugdijpenztarak_Resize(object sender, EventArgs e)
        {
            dgvNyugdijpenzt.Width = Nyugdijpenztarak.ActiveForm.Width - 83;
            dgvNyugdijpenzt.Height = Nyugdijpenztarak.ActiveForm.Height - 177;
        }

        private void dgvNyugdijpenzt_MouseMove(object sender, MouseEventArgs e)
        {
            DataGridView.HitTestInfo hit = dgvNyugdijpenzt.HitTest(e.X, e.Y);
            if (hit.Type == DataGridViewHitTestType.Cell)
            {
                dgvNyugdijpenzt.Rows[hit.RowIndex].Selected = true;
                dgvNyugdijpenzt.DefaultCellStyle.SelectionBackColor = Color.LightYellow;
                dgvNyugdijpenzt.DefaultCellStyle.SelectionForeColor = Color.DarkBlue;
            }
        }

        private void dgvNyugdijpenzt_MouseLeave(object sender, EventArgs e)
        {
            dgvNyugdijpenzt.Rows[dgvNyugdijpenzt.CurrentCell.RowIndex].Selected = false;
        }
    }
}
