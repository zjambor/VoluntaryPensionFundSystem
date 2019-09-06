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
    public partial class BankszamlakM : VoluntaryPensionFundSystem.BaseForm
    {
        private SqlConnection sconn;
        private SqlCommand scommand;
        private SqlDataAdapter da;
        private DataTable dt;
        bool Mentve = true;
        int i = 0;

        // Napló logok:
        TextWriterTraceListener myListener = new TextWriterTraceListener(@"LOG\BankszamlakM.log", "myListener");

        public BankszamlakM(SqlConnection SqlConn)
        {
            InitializeComponent();

            this.sconn = SqlConn;
            //this.Pnrid = pnrid;

            //ervL.Add("I");
            //ervL.Add("N");
            //ervL.Add(string.Empty);

            tsDelete.Enabled = false;
            tsFind.Enabled = false;
            tsNew.Enabled = false;
            tsSave.Enabled = false;
            tsSearch.Enabled = false;
            tsUpdate.Enabled = false;

            this.dgwBankszla.DataSource = dt;
            if (sconn.State == ConnectionState.Closed) sconn.Open();

            dgwBankszla.Focus();
        }

        private void bSzures_Click(object sender, EventArgs e)
        {
            scommand = new SqlCommand("spBankszlaM", sconn);
            scommand.CommandType = CommandType.StoredProcedure;
            scommand.Parameters.Add(new SqlParameter("@szamlaszam", SqlDbType.VarChar, 24)).Value = txSzamlaszam.Text;

            da = new SqlDataAdapter(scommand);
            dt = new DataTable();

            if (sconn.State == ConnectionState.Closed) sconn.Open();

            try
            {
                da.Fill(dt);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                TraceBejegyzes(ex.Message);
            }
            this.dgwBankszla.DataSource = dt;

            dt.Columns["bszla_id"].ColumnName = "Bszla id";
            dt.Columns["pnr_id"].ColumnName = "Pnr id";
            dt.Columns["nev"].ColumnName = "Név";
            dt.Columns["adoszam"].ColumnName = "Adószám";
            dt.Columns["cim"].ColumnName = "Cím";
            dt.Columns["ir_szam"].ColumnName = "Ir. szám";
            dt.Columns["helyseg"].ColumnName = "Helység";
            dt.Columns["pnr_tipus"].ColumnName = "Pnr típus";
            dt.Columns["egyeni_vall"].ColumnName = "EV.";
            dt.Columns["orszagkod"].ColumnName = "Orsz.";
            dt.Columns["szamlaszam"].ColumnName = "Számlaszám";
            dt.Columns["erv_kezdete"].ColumnName = "Érv. kezd.";
            dt.Columns["erv_vege"].ColumnName = "Érv. vége";
            dt.Columns["ervenyes"].ColumnName = "Érv.";
            dt.Columns["megjegyzes"].ColumnName = "Megjegyzés";
            dgwBankszla.Columns[0].Visible = false;
        }

        private void txSzamlaszam_Leave(object sender, EventArgs e)
        {
            
        }

        private void dgwBankszla_SelectionChanged(object sender, EventArgs e)
        {
            try
            {
                i = dgwBankszla.SelectedCells[0].RowIndex;                
                txFoglId.Text = dgwBankszla.Rows[i].Cells[5].Value.ToString();
                txMegnevezes.Text = dgwBankszla.Rows[i].Cells[6].Value.ToString();
                txEgyeniVall.Text = dgwBankszla.Rows[i].Cells[12].Value.ToString();
                txAdoszam.Text = dgwBankszla.Rows[i].Cells[7].Value.ToString();
            }            
            catch
            {
                // kezdeti hiba
            }
        }

        public override void tsExit_Click(object sender, EventArgs e)
        {
            exit();
        }

        public void exit()
        {
            if (!Mentve)
            {
                DialogResult dr = MessageBox.Show("Minden nem mentett adat el fog veszni! Biztosan kilép?", "Megerősítés", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
                if (dr == DialogResult.OK)
                {
                    Close();
                }
            }
            else Close();
        }

        private void TraceBejegyzes(string msg)
        {
            myListener.WriteLine("-" + DateTime.Now + ": " + msg);
            myListener.Flush();
            myListener.Close();
        }

        private void Bankszamlak_Resize(object sender, EventArgs e)
        {
            dgwBankszla.Width = Bankszamlak.ActiveForm.Width - 40;
            dgwBankszla.Height = Bankszamlak.ActiveForm.Height - 272;
        }

        private void dgwBankszla_KeyDown(object sender, KeyEventArgs e)
        {
        }

        private void dgwBankszla_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            e.CellStyle.BackColor = SystemColors.Control;
        }

        private void dgwBankszla_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            TraceBejegyzes(e.ToString());
        }

        private void dgwBankszla_DoubleClick(object sender, EventArgs e)
        {

        }

        private void dgwBankszla_MouseMove(object sender, MouseEventArgs e)
        {
            DataGridView.HitTestInfo hit = dgwBankszla.HitTest(e.X, e.Y);
            if (hit.Type == DataGridViewHitTestType.Cell)
            {
                dgwBankszla.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                dgwBankszla.DefaultCellStyle.SelectionBackColor = Color.LightYellow;
                dgwBankszla.DefaultCellStyle.SelectionForeColor = Color.DarkBlue;
            }
        }

        private void txSzamlaszam_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Char.IsDigit(e.KeyChar) && !Char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
        }
    }
}
