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

namespace VoluntaryPensionFundSystem
{
    public partial class Korcsoportok : VoluntaryPensionFundSystem.BaseForm, IMessageFilter
    {
        private SqlConnection sconn;
        private SqlCommand scommand;
        private SqlDataAdapter da;
        private DataTable dt;
        public string adoszam, adoazon, megnev, ev;
        int Sorindex = 0;
        // Napló logok:
        TextWriterTraceListener myListener = new TextWriterTraceListener(@"LOG\Korcsoportok.log", "myListener");

        public Korcsoportok(SqlConnection SqlConn)
        {
            InitializeComponent();

            this.sconn = SqlConn;

            tsDelete.Enabled = false;
            tsFind.Enabled = true;
            tsNew.Enabled = false;
            tsSave.Enabled = false;
            tsSearch.Enabled = true;
            tsUpdate.Enabled = false;

            if (sconn.State == ConnectionState.Closed) sconn.Open();
        }

        private void Korcsoportok_Load(object sender, EventArgs e)
        {
            Application.AddMessageFilter(this);
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

            //dt.Columns["fho_id"].ColumnName = "Fho id";
            //dt.Columns["fho_nev"].ColumnName = "Felhasználónév";
            //dt.Columns["teljes_nev"].ColumnName = "Teljes név";
            //dt.Columns["erv_kezdete"].ColumnName = "Érv. kezdete";
            //dt.Columns["erv_vege"].ColumnName = "Érv. vége";
            //dt.Columns["megjegyzes"].ColumnName = "Megjegyzés";
        }

        public void rejtId()
        {
            dgvKorcsoport.Columns[0].Visible = false;
        }

        private void bKivalaszt_Click(object sender, EventArgs e)
        {
            Fogl_keres fker = new Fogl_keres(sconn);
            fker.ShowDialog();
            int foglpnr = fker.FoglPnrid;
            adoszam = fker.adoszam;
            adoazon = fker.adoazon;
            megnev = fker.megnev;
            ev = fker.ev;
            
            try
            {
                if (fker.FoglPnrid != 0)
                {
                    this.txPnrid.Text = foglpnr.ToString();

                    txAdoazon.Text = adoazon;
                    txAdoszam.Text = adoszam;
                    txMegnev.Text = megnev;
                    if (megnev == string.Empty) txMegnev.Text = ev;

                    string query = "select k.kcsp_id,k.pnr_id,k.mksz_id,m.hozzajarulas,m.szazalek,m.egys_tagd,m.hatalybalepes,m.alairas_napja,m.fiz_gyakorisag," +
                        "m.kelt,k.evek_szama,k.kcsp_osszeg " +
                        "from korcsoportok k, munkaltatoi_szerzodesek m where k.mksz_id=m.mksz_id " +
                        "order by kcsp_id";
                    scommand = new SqlCommand(query, sconn);
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
                    this.dgvKorcsoport.DataSource = dt;
                    //lastScommand = scommand;

                    tsUpdate.Enabled = false;
                    tsDelete.Enabled = false;
                    tsNew.Enabled = false;
                    tsSave.Enabled = false;
                    tsFind.Enabled = false;
                    tsSearch.Enabled = true;
                }
            }
            catch
            {
                // nem történt módosítás
            }
        }

        public override void tsNew_Click(object sender, EventArgs e)
        {
            dgvKorcsoport.CurrentCell = dgvKorcsoport.Rows[dgvKorcsoport.RowCount - 1].Cells[1];
        }

        public override void tsSave_Click(object sender, EventArgs e)
        {
            save();
        }

        public override void tsExit_Click(object sender, EventArgs e)
        {
            if (tsNew.Enabled)
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
            dgvKorcsoport.EndEdit();                  // szerkesztőmód befejezése
            dgvKorcsoport.CurrentCell = dgvKorcsoport.CurrentRow.Cells[2];

            // üres-e a Grid
            int Count;
            try
            {
                Count = dgvKorcsoport.RowCount;
            }
            catch
            {
                Count = 0;
            }

            if (Count > 0)
            {
                if (!ell()) return;

                int Letezik = 0;

                for (int j = 0; j < dgvKorcsoport.RowCount - 1; j++)
                {
                    int id = int.Parse(dgvKorcsoport.Rows[j].Cells[0].Value.ToString());
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
                        scommand.Parameters.Add(new SqlParameter("@nev", SqlDbType.VarChar, 100)).Value = dgvKorcsoport.Rows[j].Cells[1].Value.ToString();
                        scommand.Parameters.Add(new SqlParameter("@ir_szam", SqlDbType.Decimal, 4)).Value = int.Parse(dgvKorcsoport.Rows[j].Cells[2].Value.ToString());
                        scommand.Parameters.Add(new SqlParameter("@helyseg", SqlDbType.VarChar, 20)).Value = dgvKorcsoport.Rows[j].Cells[3].Value.ToString();
                        scommand.Parameters.Add(new SqlParameter("@cim", SqlDbType.VarChar, 80)).Value = dgvKorcsoport.Rows[j].Cells[4].Value.ToString();
                        scommand.Parameters.Add(new SqlParameter("@megjegyzes", SqlDbType.VarChar, 255)).Value = dgvKorcsoport.Rows[j].Cells[5].Value.ToString();
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
                        scommand.Parameters.Add(new SqlParameter("@nev", SqlDbType.VarChar, 100)).Value = dgvKorcsoport.Rows[j].Cells[1].Value.ToString();
                        scommand.Parameters.Add(new SqlParameter("@ir_szam", SqlDbType.Decimal, 4)).Value = int.Parse(dgvKorcsoport.Rows[j].Cells[2].Value.ToString());
                        scommand.Parameters.Add(new SqlParameter("@helyseg", SqlDbType.VarChar, 20)).Value = dgvKorcsoport.Rows[j].Cells[3].Value.ToString();
                        scommand.Parameters.Add(new SqlParameter("@cim", SqlDbType.VarChar, 80)).Value = dgvKorcsoport.Rows[j].Cells[4].Value.ToString();
                        scommand.Parameters.Add(new SqlParameter("@megjegyzes", SqlDbType.VarChar, 255)).Value = dgvKorcsoport.Rows[j].Cells[5].Value.ToString();
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
                for (int j = 0; j < dgvKorcsoport.RowCount - 1; j++)
                {
                    scommand = new SqlCommand("spNyugdijpenztarakInsert", sconn);
                    scommand.CommandType = CommandType.StoredProcedure;

                    scommand.Parameters.Add(new SqlParameter("@id", SqlDbType.Int)).Value = 1;
                    scommand.Parameters.Add(new SqlParameter("@nev", SqlDbType.VarChar, 100)).Value = dgvKorcsoport.Rows[j].Cells[1].Value.ToString();
                    scommand.Parameters.Add(new SqlParameter("@ir_szam", SqlDbType.Decimal, 4)).Value = int.Parse(dgvKorcsoport.Rows[j].Cells[2].Value.ToString());
                    scommand.Parameters.Add(new SqlParameter("@helyseg", SqlDbType.VarChar, 20)).Value = dgvKorcsoport.Rows[j].Cells[3].Value.ToString();
                    scommand.Parameters.Add(new SqlParameter("@cim", SqlDbType.VarChar, 80)).Value = dgvKorcsoport.Rows[j].Cells[4].Value.ToString();
                    scommand.Parameters.Add(new SqlParameter("@megjegyzes", SqlDbType.VarChar, 255)).Value = dgvKorcsoport.Rows[j].Cells[5].Value.ToString();
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

            dgvKorcsoport.CurrentCell = dgvKorcsoport.Rows[dgvKorcsoport.RowCount - 2].Cells[0];
        }

        private bool ell()
        {
            // kötelező mezők
            for (int i = 0; i < dgvKorcsoport.RowCount - 1; i++)
            {
                if (dgvKorcsoport.Rows[i].Cells[1].Value.ToString().Length == 0 || dgvKorcsoport.Rows[i].Cells[2].Value.ToString().Length == 0 ||
                    dgvKorcsoport.Rows[i].Cells[3].Value.ToString().Length == 0 || dgvKorcsoport.Rows[i].Cells[4].Value.ToString().Length == 0)
                {
                    MessageBox.Show("Név, irányítószám, helység, cím nem lehet üres!");
                    return false;
                }
            }

            // esetleges ismétlődés ellenőrzése
            string adat;
            int sum;

            sum = 0;
            adat = dgvKorcsoport.Rows[Sorindex].Cells[1].Value.ToString();
            for (int i = 0; i < dgvKorcsoport.RowCount - 1; i++)
            {
                if (dgvKorcsoport.Rows[i].Cells[1].Value.ToString() == adat)
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
                int id = int.Parse(dgvKorcsoport.Rows[Sorindex].Cells[0].Value.ToString());
                string query1 = "delete from nyugdijpenztarak where nyp_id=" + id + ";";
                scommand = new SqlCommand(query1, sconn);
                // rekord törlése
                try { scommand.ExecuteNonQuery(); }
                catch (Exception ex) { MessageBox.Show("SQL Hiba " + ex.Message); }
                //Reload();
            }
        }

        private void dgvKorcsoport_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
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

        private void dgvKorcsoport_MouseMove(object sender, MouseEventArgs e)
        {
            DataGridView.HitTestInfo hit = dgvKorcsoport.HitTest(e.X, e.Y);
            if (hit.Type == DataGridViewHitTestType.Cell)
            {
                dgvKorcsoport.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                //dgvKorcsoport.Rows[hit.RowIndex].Selected = true;
                dgvKorcsoport.DefaultCellStyle.SelectionBackColor = Color.LightYellow;
                dgvKorcsoport.DefaultCellStyle.SelectionForeColor = Color.DarkBlue;
            }
        }

        private void dgvKorcsoport_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {

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
                        int iColumn = dgvKorcsoport.CurrentCell.ColumnIndex;
                        int iRow = dgvKorcsoport.CurrentCell.RowIndex;
                        if (iColumn != dgvKorcsoport.Columns.Count - 1)
                            dgvKorcsoport.CurrentCell = dgvKorcsoport[iColumn + 1, iRow];
                        else if (iRow != dgvKorcsoport.RowCount - 1)
                            dgvKorcsoport.CurrentCell = dgvKorcsoport[2, iRow + 1];
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
            dgvKorcsoport.EditingControlShowing +=
        new DataGridViewEditingControlShowingEventHandler(dataGridView_EditingControlShowing);
        }

        private void dataGridView_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            e.Control.KeyPress +=
                new KeyPressEventHandler(Control_KeyPress);
        }

        private void Control_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (((char.IsNumber(e.KeyChar) || char.IsControl(e.KeyChar)) && this.dgvKorcsoport.CurrentCell.ColumnIndex == 2) || (this.dgvKorcsoport.CurrentCell.ColumnIndex != 2))
            {
                // engedjük a beírást, ha a számlaszámhoz számjegyeket írunk vagy máshol vagyunk
            }
            else
            {
                e.Handled = true;               // a számlaszámhoz mást nem engedünk beírni
            }
        }
        #endregion

        private void Korcsoportok_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.RemoveMessageFilter(this);
        }

        private void Korcsoportok_Resize(object sender, EventArgs e)
        {
            dgvKorcsoport.Width = Korcsoportok.ActiveForm.Width - 40;
            dgvKorcsoport.Height = Korcsoportok.ActiveForm.Height - 252;
        }

        private void dgvKorcsoport_MouseLeave(object sender, EventArgs e)
        {

        }
    }
}
