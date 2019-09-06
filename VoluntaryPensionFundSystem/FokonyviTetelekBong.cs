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
    public partial class FokonyviTetelekBong : VoluntaryPensionFundSystem.BaseForm
    {
        private SqlConnection sconn;
        private SqlCommand scommand;
        private SqlDataAdapter da;
        private DataTable dt;

        // Napló logok:
        TextWriterTraceListener myListener = new TextWriterTraceListener(@"LOG\FokonyviTetelek.log", "myListener");

        public FokonyviTetelekBong(SqlConnection SqlConn)
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
            string query = "select * from fokonyvi_tetelek";
            scommand = new SqlCommand(query, sconn);

            da = new SqlDataAdapter(scommand);
            dt = new DataTable();

            Frissit();
            this.dgvFtl.DataSource = dt;

            dgvFtl.Focus();
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

            //btl_id,osszeg,pnr_id,erteknap,sorszam,szamlaszam,nev,kozlemeny,konyveles_datuma,statusz,storno,megjegyzes
            //dt.Columns["osszeg"].ColumnName = "Összeg";
            //dt.Columns["erteknap"].ColumnName = "Értéknap";
            //dt.Columns["sorszam"].ColumnName = "Ssz.";
            //dt.Columns["szamlaszam"].ColumnName = "Számlaszám";
            //dt.Columns["nev"].ColumnName = "Név";
            //dt.Columns["kozlemeny"].ColumnName = "Közlemény";
            //dt.Columns["konyveles_datuma"].ColumnName = "Könyvelés dátuma";
            //dt.Columns["statusz"].ColumnName = "Státusz";
            //dt.Columns["storno"].ColumnName = "Stornó";
            //dt.Columns["megjegyzes"].ColumnName = "Megjegyzés";
            //dt.Columns["evnev"].ColumnName = "EV. Név";
            //dt.Columns["pnr_id"].ColumnName = "Pnr Id";
            //dt.Columns["adoszam"].ColumnName = "Adószám";
            //dt.Columns["adoazonosito_jel"].ColumnName = "Adóazon. jel";
            //dt.Columns["megnevezes"].ColumnName = "Megnevezés";
            //dt.Columns["ir_szam"].ColumnName = "Ir.szám";
            //dt.Columns["helyseg"].ColumnName = "Helység";
            //dt.Columns["cim"].ColumnName = "Cím";
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

        private void FokonyviTetelekBong_Resize(object sender, EventArgs e)
        {
            dgvFtl.Width = Idoszakok.ActiveForm.Width - 40;
            dgvFtl.Height = Idoszakok.ActiveForm.Height - 186;
        }

        private void dgvFtl_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
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

        private void dgvFtl_MouseMove(object sender, MouseEventArgs e)
        {
            DataGridView.HitTestInfo hit = dgvFtl.HitTest(e.X, e.Y);
            if (hit.Type == DataGridViewHitTestType.Cell)
            {
                dgvFtl.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                dgvFtl.Rows[hit.RowIndex].Selected = true;
                dgvFtl.DefaultCellStyle.SelectionBackColor = Color.LightYellow;
                dgvFtl.DefaultCellStyle.SelectionForeColor = Color.DarkBlue;
            }
        }
    }
}
