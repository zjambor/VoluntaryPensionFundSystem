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
    public partial class FoglMrepKivalaszt : BaseForm
    {
        private DataTable dt;
        int Sorindex;

        Fogl_keres ba;

        public FoglMrepKivalaszt(DataTable dt, Fogl_keres szulo)
        {
            InitializeComponent();

            this.dt = dt;
            this.ba = szulo;

            tsDelete.Enabled = false;
            tsFind.Enabled = false;
            tsNew.Enabled = false;
            tsSave.Enabled = false;
            tsSearch.Enabled = false;
            tsUpdate.Enabled = false;
            tsExit.Enabled = false;

            Frissit();
            dgvBankKiv.DataSource = dt;
        }

        private void Frissit()
        {
            dt.Columns["pnr_id"].ColumnName = "Pnr Id";
            dt.Columns["adoszam"].ColumnName = "Adószám";
            dt.Columns["adoazonosito_jel"].ColumnName = "Adóazon. jel";
            dt.Columns["megnevezes"].ColumnName = "Megnevezés";
            dt.Columns["nev"].ColumnName = "EV név";
            dt.Columns["ir_szam"].ColumnName = "Ir.szám";
            dt.Columns["helyseg"].ColumnName = "Helység";
            dt.Columns["cim"].ColumnName = "Cím";
            // pnr_id,adoszam,adoazonosito_jel,megnevezes,nev,ir_szam,helyseg,cim
        }

        private void bKivalaszt_Click(object sender, EventArgs e)
        {
            kivalaszt();
        }

        private void dgvBankKiv_SelectionChanged(object sender, EventArgs e)
        {
            
        }

        private void dgvBankKiv_DoubleClick(object sender, EventArgs e)
        {
            kivalaszt();
        }

        private void kivalaszt()
        {
            try
            {
                Sorindex = dgvBankKiv.SelectedCells[0].RowIndex;
                ba.FoglPnrid = int.Parse(dgvBankKiv.Rows[Sorindex].Cells[0].Value.ToString());
                ba.adoszam = dgvBankKiv.Rows[Sorindex].Cells[1].Value.ToString();
                ba.adoazon = dgvBankKiv.Rows[Sorindex].Cells[2].Value.ToString();
                ba.megnev = dgvBankKiv.Rows[Sorindex].Cells[3].Value.ToString();
                ba.ev = dgvBankKiv.Rows[Sorindex].Cells[4].Value.ToString();
                ba.helyseg = dgvBankKiv.Rows[Sorindex].Cells[6].Value.ToString();
                ba.cim = dgvBankKiv.Rows[Sorindex].Cells[7].Value.ToString();
                ba.irszam = dgvBankKiv.Rows[Sorindex].Cells[5].Value.ToString();
                //pnr_id,adoszam,adoazonosito_jel,megnevezes,nev,ir_szam,helyseg,cim
                Close();
            }
            catch
            {
                MessageBox.Show("Nincs kiválasztott sor!");
            }
        }

        private void dgvBankKiv_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                kivalaszt();
            }
        }

        private void FoglMrepKivalaszt_Resize(object sender, EventArgs e)
        {
            dgvBankKiv.Width = FoglMrepKivalaszt.ActiveForm.Width - 40;
            dgvBankKiv.Height = FoglMrepKivalaszt.ActiveForm.Height - 200;
        }

        private void dgvBankKiv_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
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

        private void dgvBankKiv_MouseMove(object sender, MouseEventArgs e)
        {
            DataGridView.HitTestInfo hit = dgvBankKiv.HitTest(e.X, e.Y);
            if (hit.Type == DataGridViewHitTestType.Cell)
            {
                dgvBankKiv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                dgvBankKiv.DefaultCellStyle.SelectionBackColor = Color.LightYellow;
                dgvBankKiv.DefaultCellStyle.SelectionForeColor = Color.DarkBlue;
            }
        }
    }
}
