using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Configuration;
using System.Data.SqlClient;
using System.Diagnostics;

namespace VoluntaryPensionFundSystem
{
    public partial class NypKivalaszt : VoluntaryPensionFundSystem.BaseForm
    {
        private SqlConnection sconn;
        //private SqlCommand scommand;
        int Sorindex;

        Okiratok ok;

        public NypKivalaszt(SqlConnection SqlConn, Okiratok ok)
        {
            InitializeComponent();

            this.sconn = SqlConn;
            if (sconn.State == ConnectionState.Closed) sconn.Open();
            this.ok = ok;

            tsDelete.Enabled = false;
            tsFind.Enabled = false;
            tsNew.Enabled = false;
            tsSave.Enabled = false;
            tsSearch.Enabled = false;
            tsUpdate.Enabled = false;

            dgvNyugdijpenzt.Focus();
        }

        private void NypKivalaszt_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'vPFSDataSet.spNyugdijpenztarak' table. You can move, or remove it, as needed.
            this.spNyugdijpenztarakTableAdapter.Fill(this.vPFSDataSet.spNyugdijpenztarak);

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

        private void NypKivalaszt_Resize(object sender, EventArgs e)
        {
            dgvNyugdijpenzt.Width = NypKivalaszt.ActiveForm.Width - 83;
            dgvNyugdijpenzt.Height = NypKivalaszt.ActiveForm.Height - 177;
        }

        private void dgvNyugdijpenzt_DoubleClick(object sender, EventArgs e)
        {
            kivalaszt();
        }

        private void dgvNyugdijpenzt_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                kivalaszt();
            }
        }

        private void kivalaszt()
        {
            try
            {
                Sorindex = dgvNyugdijpenzt.SelectedCells[0].RowIndex;

                ok.ptrid = dgvNyugdijpenzt.Rows[Sorindex].Cells[0].Value.ToString();
                ok.ptrnev = dgvNyugdijpenzt.Rows[Sorindex].Cells[1].Value.ToString();

                Close();
            }
            catch
            {
                MessageBox.Show("Nincs kiválasztott sor!");
            }
        }

        public override void tsExit_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void dgvNyugdijpenzt_MouseMove(object sender, MouseEventArgs e)
        {
            DataGridView.HitTestInfo hit = dgvNyugdijpenzt.HitTest(e.X, e.Y);
            if (hit.Type == DataGridViewHitTestType.Cell)
            {
                dgvNyugdijpenzt.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                dgvNyugdijpenzt.Rows[hit.RowIndex].Selected = true;
                dgvNyugdijpenzt.DefaultCellStyle.SelectionBackColor = Color.LightYellow;
                dgvNyugdijpenzt.DefaultCellStyle.SelectionForeColor = Color.DarkBlue;
            }
        }
    }
}
