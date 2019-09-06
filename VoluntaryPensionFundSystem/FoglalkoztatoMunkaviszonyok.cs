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
    public partial class FoglalkoztatoMunkaviszonyok : VoluntaryPensionFundSystem.BaseForm
    {
        private SqlConnection sconn;
        private SqlCommand scommand;
        private SqlDataAdapter da;
        private DataTable dt;

        // Napló logok:
        TextWriterTraceListener myListener = new TextWriterTraceListener(@"LOG\FoglalkoztatoMunkaviszonyok.log", "myListener");

        public FoglalkoztatoMunkaviszonyok(SqlConnection SqlConn, int pnrid)
        {
            InitializeComponent();

            this.sconn = SqlConn;

            tsDelete.Enabled = false;
            tsFind.Enabled = false;
            tsNew.Enabled = false;
            tsSave.Enabled = false;
            tsSearch.Enabled = false;
            tsUpdate.Enabled = false;

            if (sconn.State == ConnectionState.Closed) sconn.Open();
            scommand = new SqlCommand("spFoglMunkaviszonyok", sconn);
            scommand.CommandType = CommandType.StoredProcedure;
            scommand.Parameters.Add(new SqlParameter("@id", SqlDbType.Int)).Value = pnrid;
            
            da = new SqlDataAdapter(scommand);
            dt = new DataTable();

            Frissit();
            this.dgvMunk.DataSource = dt;

            dgvMunk.Focus();
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
                MessageBox.Show("SQL Hiba " + ex.Message);
                TraceBejegyzes(ex.Message);
            }

            dt.Columns["tszs_id"].ColumnName = "PAJ";
            dt.Columns["nev"].ColumnName = "Név";
            dt.Columns["adoazonosito_jel"].ColumnName = "Adóazonosító jel";
            dt.Columns["alk_kezdete"].ColumnName = "Alk.kezdete";
            dt.Columns["alk_vege"].ColumnName = "Alk.vége";
            dt.Columns["ervenyes"].ColumnName = "Érvényes";
        }

        public override void tsExit_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void TraceBejegyzes(string msg)
        {
            myListener.WriteLine("-" + DateTime.Now + ": " + msg);
            myListener.Flush();
            myListener.Close();
        }

        private void FoglalkoztatoMunkaviszonyok_Resize(object sender, EventArgs e)
        {
            dgvMunk.Width = Idoszakok.ActiveForm.Width - 40;
            dgvMunk.Height = Idoszakok.ActiveForm.Height - 174;
        }

        private void dgvMunk_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex / 2 * 2 != e.RowIndex)
            {
                e.CellStyle.BackColor = Color.LightGray;
            }
            else
            {
                e.CellStyle.BackColor = Color.LightGray;
            }
        }

        private void dgvMunk_MouseMove(object sender, MouseEventArgs e)
        {
            DataGridView.HitTestInfo hit = dgvMunk.HitTest(e.X, e.Y);
            if (hit.Type == DataGridViewHitTestType.Cell)
            {
                dgvMunk.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                dgvMunk.Rows[hit.RowIndex].Selected = true;
                dgvMunk.DefaultCellStyle.SelectionBackColor = Color.LightYellow;
                dgvMunk.DefaultCellStyle.SelectionForeColor = Color.DarkBlue;
            }
        }
    }
}
