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

namespace VoluntaryPensionFundSystem
{
    public partial class AnalitikaHibak : BaseForm
    {
        private SqlConnection sconn;
        private SqlCommand scommand;
        private SqlDataAdapter da;
        private DataTable dt;

        // Napló logok:
        private TextWriterTraceListener myListener = new TextWriterTraceListener(@"LOG\AnalitikaHibak.log", "myListener");

        public AnalitikaHibak(SqlConnection SqlConn, int bev_id)
        {
            InitializeComponent();

            tsDelete.Enabled = false;
            tsFind.Enabled = false;
            tsNew.Enabled = false;
            tsSave.Enabled = false;
            tsSearch.Enabled = false;
            tsUpdate.Enabled = false;

            this.sconn = SqlConn;
            if (sconn.State == ConnectionState.Closed) sconn.Open();

            scommand = new SqlCommand("spAnHibSel", sconn);
            scommand.CommandType = CommandType.StoredProcedure;
            scommand.Parameters.Add(new SqlParameter("@bev_id", SqlDbType.Int)).Value = bev_id;

            da = new SqlDataAdapter(scommand);
            dt = new DataTable();

            try
            {
                da.Fill(dt);
            }
            catch (Exception ex)
            {
                TraceBejegyzes(ex.Message);
            }

            dt.Columns["hiba_id"].ColumnName = "Hibakód";
            dt.Columns["ado"].ColumnName = "Adóazon. jel";
            dt.Columns["nev"].ColumnName = "Név";
            dt.Columns["leiras"].ColumnName = "Leírás";
            dt.Columns["jelzes_datum"].ColumnName = "Jelzés dátuma";
            dt.Columns["megoldas_dat"].ColumnName = "Megoldás dátuma";

            this.dgvAnalitikaHibak.DataSource = dt;
            rejtId();
        }

        public void rejtId()
        {
            dgvAnalitikaHibak.Columns[0].Visible = false;
        }

        private void TraceBejegyzes(string msg)
        {
            myListener.WriteLine("-" + DateTime.Now + ": " + msg);
            myListener.Flush();
            myListener.Close();
        }

        public override void tsExit_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void dgvAnalitikaHibak_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
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

        private void dgvAnalitikaHibak_MouseMove(object sender, MouseEventArgs e)
        {
            DataGridView.HitTestInfo hit = dgvAnalitikaHibak.HitTest(e.X, e.Y);
            if (hit.Type == DataGridViewHitTestType.Cell)
            {
                dgvAnalitikaHibak.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                dgvAnalitikaHibak.Rows[hit.RowIndex].Selected = true;
                dgvAnalitikaHibak.DefaultCellStyle.SelectionBackColor = Color.LightYellow;
                dgvAnalitikaHibak.DefaultCellStyle.SelectionForeColor = Color.DarkBlue;
            }
        }
    }
}
