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
    public partial class BankiKivonKivalasztB : BaseForm
    {
        private DataTable dt;
        int Sorindex;

        Beazonositas ba;

        public BankiKivonKivalasztB(DataTable dt, Beazonositas szulo)
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
            dt.Columns["bizonylat_kelte"].ColumnName = "Értéknap";
            dt.Columns["kivonat_szama"].ColumnName = "Kivonat száma";
            dt.Columns["irany"].ColumnName = "Irány";
            dt.Columns["letrehozas_datuma"].ColumnName = "Létrehozás dát.";
            dt.Columns["megjegyzes"].ColumnName = "Megjegyzés";
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
                ba.erteknap = dgvBankKiv.Rows[Sorindex].Cells[1].Value.ToString();
                ba.kivonatSzama = int.Parse(dgvBankKiv.Rows[Sorindex].Cells[2].Value.ToString());
                ba.irany = dgvBankKiv.Rows[Sorindex].Cells[3].Value.ToString();
                ba.letrehozas = dgvBankKiv.Rows[Sorindex].Cells[4].Value.ToString();
                ba.bkt_id = int.Parse(dgvBankKiv.Rows[Sorindex].Cells[0].Value.ToString());

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

        private void BankiKivonKivalasztB_Resize(object sender, EventArgs e)
        {
            dgvBankKiv.Width = BankiKivonKivalasztB.ActiveForm.Width - 40;
            dgvBankKiv.Height = BankiKivonKivalasztB.ActiveForm.Height - 200;
        }
    }
}
