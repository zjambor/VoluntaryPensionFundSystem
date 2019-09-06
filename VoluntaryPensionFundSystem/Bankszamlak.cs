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
    public partial class Bankszamlak : VoluntaryPensionFundSystem.BaseForm, IMessageFilter
    {
        private SqlConnection sconn;
        private SqlCommand scommand;
        //private SqlDataAdapter da;
        //private DataTable dt;
        private int Pnrid;
        List<string> ervL = new List<string>();
        int Sorindex;
        //string Foglneve;
        bool Mentve = true;

        // Napló logok:
        TextWriterTraceListener myListener = new TextWriterTraceListener(@"LOG\Bankszamlak.log", "myListener");

        public Bankszamlak(SqlConnection SqlConn, int pnrid)
        {
            InitializeComponent();

            this.sconn = SqlConn;
            this.Pnrid = pnrid;

            ervL.Add("I");
            ervL.Add("N");
            ervL.Add(string.Empty);

            tsDelete.Enabled = true;
            tsFind.Enabled = false;
            tsNew.Enabled = false;
            tsSave.Enabled = true;
            tsSearch.Enabled = false;
            tsUpdate.Enabled = false;

            //scommand = new SqlCommand("spBankszla", sconn);
            //scommand.CommandType = CommandType.StoredProcedure;
            //scommand.Parameters.Add(new SqlParameter("@pnr_id", SqlDbType.Int)).Value = Pnrid;

            //da = new SqlDataAdapter(scommand);
            //dt = new DataTable();

            //Frissit();
            //this.dgwBankszla.DataSource = dt;
            if (sconn.State == ConnectionState.Closed) sconn.Open();

            Adatcimke.Text = "Foglalkoztató: id: " + Pnrid + " Mentve: " + (Mentve ? "OK" : "nem");

            dgwBankszla.Focus();
        }

        private void Bankszamlak_Load(object sender, EventArgs e)
        {
            Reload();
            Application.AddMessageFilter(this);
        }

        // Auto betöltés
        private void Reload()
        {            
            this.spBankszlaTableAdapter.Fill(this.vPFSDataSetBankszla.spBankszla, new System.Nullable<int>(((int)(System.Convert.ChangeType(Pnrid, typeof(int))))));
        }

        private void dgwBankszla_SelectionChanged(object sender, EventArgs e)
        {
            // automatikus kiegészítése az érvényes=I és a Pnrid celláknak
            try
            {
                Sorindex = dgwBankszla.SelectedCells[0].RowIndex;

                if (dgwBankszla.Rows[Sorindex].Cells[5].Value.ToString() != "I" && dgwBankszla.Rows[Sorindex].Cells[5].Value.ToString() != "N")
                {
                    dgwBankszla.Rows[Sorindex].Cells[5].Value = "I";
                }
                dgwBankszla.Rows[Sorindex].Cells[1].Value = Pnrid;
            }
            catch
            {
            }
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

        public override void tsSearch_Click(object sender, EventArgs e)
        {
            yellowMode();
        }

        public override void tsFind_Click(object sender, EventArgs e)
        {
            runQuery();
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
                int id = int.Parse(dgwBankszla.Rows[dgwBankszla.CurrentCell.RowIndex].Cells[0].Value.ToString());
                string query1 = "delete from bankszamlak where bszla_id=" + id + ";";
                scommand = new SqlCommand(query1, sconn);
                // rekord törlése
                try { scommand.ExecuteNonQuery(); }
                catch (Exception ex) { MessageBox.Show("SQL Hiba " + ex.Message); }
                Reload();
            }
        }

        public override void createNew()
        {
        }

        public override void save()
        {
            dgwBankszla.EndEdit();                  // szerkesztőmód befejezése

            // üres-e a Grid
            string query1 = "select count(*) from bankszamlak where pnr_id=" + Pnrid + ";";
            scommand = new SqlCommand(query1, sconn);
            int Count = (Int32)scommand.ExecuteScalar();

            if (Count > 0)
            {
                int Letezik = 0;
                for (int j = 0; j < dgwBankszla.RowCount - 1; j++)
                {
                    int id = int.Parse(dgwBankszla.Rows[j].Cells[0].Value.ToString());
                    scommand = new SqlCommand("spSelectBankszla", sconn);
                    scommand.CommandType = CommandType.StoredProcedure;
                    scommand.Parameters.Add(new SqlParameter("@id", SqlDbType.Int)).Value = id;
                    scommand.Parameters.Add(new SqlParameter("@pnr_id", SqlDbType.Int)).Value = Pnrid;
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
                        scommand = new SqlCommand("spBszlaInsert", sconn);
                        scommand.CommandType = CommandType.StoredProcedure;

                        scommand.Parameters.Add(new SqlParameter("@id", SqlDbType.Int)).Value = 1;
                        scommand.Parameters.Add(new SqlParameter("@pnr_id", SqlDbType.Int)).Value = Pnrid;
                        scommand.Parameters.Add(new SqlParameter("@szamlaszam", SqlDbType.VarChar, 24)).Value = dgwBankszla.Rows[j].Cells[2].Value.ToString();
                        //scommand.Parameters.Add(new SqlParameter("@erv_kezdete", SqlDbType.Date)).Value = DateTime.Now;
                        scommand.Parameters.Add(new SqlParameter("@ervenyes", SqlDbType.VarChar, 1)).Value = dgwBankszla.Rows[j].Cells[5].Value.ToString();
                        //scommand.Parameters.Add(new SqlParameter("@megjegyzes", SqlDbType.VarChar, 255)).Value = dgwBankszla.Rows[j].Cells[6].Value.ToString();
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

                        scommand = new SqlCommand("spBszlaUpdate", sconn);
                        scommand.CommandType = CommandType.StoredProcedure;

                        scommand.Parameters.Add(new SqlParameter("@id", SqlDbType.Int)).Value = id;
                        scommand.Parameters.Add(new SqlParameter("@szamlaszam", SqlDbType.VarChar, 24)).Value = dgwBankszla.Rows[j].Cells[2].Value.ToString();
                        //scommand.Parameters.Add(new SqlParameter("@erv_kezdete", SqlDbType.Date)).Value = DateTime.Now;
                        scommand.Parameters.Add(new SqlParameter("@ervenyes", SqlDbType.VarChar, 1)).Value = dgwBankszla.Rows[j].Cells[5].Value.ToString();
                        scommand.Parameters.Add(new SqlParameter("@megjegyzes", SqlDbType.VarChar, 255)).Value = dgwBankszla.Rows[j].Cells[6].Value.ToString();
                        scommand.Parameters.Add(new SqlParameter("@modosit_neve", SqlDbType.VarChar, 80)).Value = User.name;

                        try
                        {
                            if (sconn.State == ConnectionState.Closed) sconn.Open();
                            scommand.ExecuteNonQuery();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("SQL Hiba " + ex.Message+ex.Data+"update");
                            TraceBejegyzes(ex.Message);
                        }
                    }
                }
            }
            else
            {
                //ha üres a Grid akkor csak Insert lehet
                for (int j = 0; j < dgwBankszla.RowCount - 1; j++)
                {
                    scommand = new SqlCommand("spBszlaInsert", sconn);
                    scommand.CommandType = CommandType.StoredProcedure;

                    scommand.Parameters.Add(new SqlParameter("@id", SqlDbType.Int)).Value = 1;
                    scommand.Parameters.Add(new SqlParameter("@pnr_id", SqlDbType.Int)).Value = Pnrid;
                    scommand.Parameters.Add(new SqlParameter("@szamlaszam", SqlDbType.VarChar, 24)).Value = dgwBankszla.Rows[j].Cells[2].Value.ToString();
                    //scommand.Parameters.Add(new SqlParameter("@erv_kezdete", SqlDbType.Date)).Value = DateTime.Now;
                    scommand.Parameters.Add(new SqlParameter("@ervenyes", SqlDbType.VarChar, 1)).Value = dgwBankszla.Rows[j].Cells[5].Value.ToString();
                    //scommand.Parameters.Add(new SqlParameter("@megjegyzes", SqlDbType.VarChar, 255)).Value = dgwBankszla.Rows[j].Cells[6].Value.ToString();
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
            Adatcimke.Text = "Foglalkoztató: id: " + Pnrid + " Mentve: " + (Mentve ? "OK" : "nem");
        }
        
        public override void yellowMode()
        {
            //dgwBankszla.DefaultCellStyle.BackColor = Color.Yellow;
        }

        public override void runQuery()
        {
        }

        public void updateRecord()
        {
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

        private void TraceBejegyzes(string msg)
        {
            myListener.WriteLine("-" + DateTime.Now + ": " + msg);
            myListener.Flush();
            myListener.Close();
        }

        private void Bankszamlak_Resize(object sender, EventArgs e)
        {
            dgwBankszla.Width = Bankszamlak.ActiveForm.Width - 77;
            dgwBankszla.Height = Bankszamlak.ActiveForm.Height - 230;
        }

        private void dgwBankszla_KeyDown(object sender, KeyEventArgs e)
        {
        }

        private void dgwBankszla_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.ColumnIndex == 0 || e.ColumnIndex == 1 || e.ColumnIndex == 3 || e.ColumnIndex == 4)
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

        private void dgwBankszla_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            TraceBejegyzes(e.ToString());
        }

        private void dgwBankszla_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            dgwBankszla.BeginEdit(false);               // írható cellába belépéskor edit módba lépünk
        }

        private void dgwBankszla_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            
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
                    int iColumn = dgwBankszla.CurrentCell.ColumnIndex;
                    int iRow = dgwBankszla.CurrentCell.RowIndex;
                    if (iColumn != dgwBankszla.Columns.Count - 1)
                        dgwBankszla.CurrentCell = dgwBankszla[iColumn + 1, iRow];
                    else if (iRow != dgwBankszla.RowCount - 1)
                        dgwBankszla.CurrentCell = dgwBankszla[2, iRow + 1];
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
            dgwBankszla.EditingControlShowing +=
        new DataGridViewEditingControlShowingEventHandler(dataGridView_EditingControlShowing);
        }

        private void dataGridView_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            e.Control.KeyPress +=
                new KeyPressEventHandler(Control_KeyPress);
        }

        private void Control_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (((char.IsNumber(e.KeyChar) || char.IsControl(e.KeyChar)) && this.dgwBankszla.CurrentCell.ColumnIndex == 2) || (this.dgwBankszla.CurrentCell.ColumnIndex != 2))
            {
                // engedjük a beírást, ha a számlaszámhoz számjegyeket írunk vagy máshol vagyunk
                Mentve = false;
                Adatcimke.Text = "Foglalkoztató: id: " + Pnrid + " Mentve: " + (Mentve ? "OK" : "nem");
                dgwBankszla.Rows[dgwBankszla.CurrentCell.RowIndex].Cells[5].Value = "I"; 
            }
            else
            {
                e.Handled = true;               // a számlaszámhoz mást nem engedünk beírni
            }
        }
        #endregion

        private void dgwBankszla_MouseMove(object sender, MouseEventArgs e)
        {
            DataGridView.HitTestInfo hit = dgwBankszla.HitTest(e.X, e.Y);
            if (hit.Type == DataGridViewHitTestType.Cell)
            {
                dgwBankszla.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                dgwBankszla.Rows[hit.RowIndex].Selected = true;
                dgwBankszla.DefaultCellStyle.SelectionBackColor = Color.LightYellow;
                dgwBankszla.DefaultCellStyle.SelectionForeColor = Color.DarkBlue;
            }
        }

        private void dgwBankszla_MouseLeave(object sender, EventArgs e)
        {
            dgwBankszla.Rows[dgwBankszla.CurrentCell.RowIndex].Selected = false;
        }
    }
}
