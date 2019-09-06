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
    public partial class Ujkivonat : VoluntaryPensionFundSystem.BaseForm
    {
        private SqlConnection sconn;
        private SqlCommand scommand;
        BefizetesekRogz br;
        private bool saveok = false;
        private List<string> lista = new List<string>();

        // Napló logok:
        TextWriterTraceListener myListener = new TextWriterTraceListener(@"LOG\UjKivonat.log", "myListener");

        public Ujkivonat(SqlConnection SqlConn, BefizetesekRogz szulo)
        {
            InitializeComponent();

            this.sconn = SqlConn;
            this.br = szulo;

            lista.Add("T");
            lista.Add("K");
            cbIrany.DataSource = lista;

            tsDelete.Enabled = false;
            tsFind.Enabled = false;
            tsNew.Enabled = false;
            tsSave.Enabled = true;
            tsSearch.Enabled = false;
            tsUpdate.Enabled = false;
        }

        private void Ujkivonat_Load(object sender, EventArgs e)
        {

        }

        public override void tsSave_Click(object sender, EventArgs e)
        {
            saving();
            tsSave.Enabled = false;
        }

        public override void tsExit_Click(object sender, EventArgs e)
        {
            if (tsSave.Enabled)
            {
                DialogResult dr = MessageBox.Show("Minden nem mentett adat el fog veszni! Biztosan kilép?", "Megerősítés", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
                if (dr == DialogResult.OK)
                {
                    Close();
                }
            }
            else
            {
                // adatátadás
                br.erteknap = datErteknap.Text;
                br.kivonatSzama = (int)txKivonatSzama.Value;
                br.irany = cbIrany.Text;
                br.letrehozas = datLetrehoz.Text;
                br.bkt_id = int.Parse(txId.Text);

                Close();
            }
        }

        private void bSave_Click(object sender, EventArgs e)
        {
            saving();

            if (saveok)
            {
                // adatátadás
                br.erteknap = datErteknap.Text;
                br.kivonatSzama = (int)txKivonatSzama.Value;
                br.irany = cbIrany.Text;
                br.letrehozas = datLetrehoz.Text;
                br.bkt_id = int.Parse(txId.Text);

                Close();
            }
        }

        private void saving()
        {
            // mentés
            if (datErteknap.Text == string.Empty) { MessageBox.Show("Értéknap megadása kötelező!"); datErteknap.Focus(); return; }
            if (txKivonatSzama.Value == 0) { MessageBox.Show("Kivonat száma nem lehet 0!"); txKivonatSzama.Focus(); return; }
            if (cbIrany.Text == string.Empty) { MessageBox.Show("Irány megadása kötelező!"); cbIrany.Focus(); return; }
            if (datLetrehoz.Text == string.Empty) { MessageBox.Show("Létrehozás dátuma megadása kötelező!"); datLetrehoz.Focus(); return; }

            int id;
            try
            {
                id = int.Parse(txId.Text);
            }
            catch
            {
                id = 0;
            }

            if (id == 0)
            {
                // insert
                scommand = new SqlCommand("spBanki_kivonatokInsert", sconn);
                scommand.CommandType = CommandType.StoredProcedure;

                scommand.Parameters.Add(new SqlParameter("@id", SqlDbType.Int)).Value = 1;
                scommand.Parameters.Add(new SqlParameter("@bizonylat_kelte", SqlDbType.Date)).Value = DateTime.Parse(datErteknap.Text);
                scommand.Parameters.Add(new SqlParameter("@kivonat_szama", SqlDbType.VarChar, 20)).Value = txKivonatSzama.Value.ToString();
                scommand.Parameters.Add(new SqlParameter("@irany", SqlDbType.VarChar, 1)).Value = cbIrany.Text;
                scommand.Parameters.Add(new SqlParameter("@letrehozas_datuma", SqlDbType.Date)).Value = DateTime.Parse(datLetrehoz.Text);
                scommand.Parameters.Add(new SqlParameter("@megjegyzes", SqlDbType.VarChar, 255)).Value = txMegjegyzes.Text;
                scommand.Parameters.Add(new SqlParameter("@modosit_neve", SqlDbType.VarChar, 80)).Value = User.name;

                try
                {
                    if (sconn.State == ConnectionState.Closed) sconn.Open();
                    id = (int)scommand.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("SQL Hiba " + ex.Message + ex.Data + "insert");
                    TraceBejegyzes(ex.Message);
                }
                if (id == 0) { MessageBox.Show("A rekord mentése nem sikerült!"); datErteknap.Focus(); return; }
            }
            else
            {
                // update
                scommand = new SqlCommand("spBanki_kivonatokUpdate", sconn);
                scommand.CommandType = CommandType.StoredProcedure;

                scommand.Parameters.Add(new SqlParameter("@id", SqlDbType.Int)).Value = id;
                scommand.Parameters.Add(new SqlParameter("@bizonylat_kelte", SqlDbType.Date)).Value = DateTime.Parse(datErteknap.Text);
                scommand.Parameters.Add(new SqlParameter("@kivonat_szama", SqlDbType.VarChar, 20)).Value = txKivonatSzama.Value.ToString();
                scommand.Parameters.Add(new SqlParameter("@irany", SqlDbType.VarChar, 1)).Value = cbIrany.Text;
                scommand.Parameters.Add(new SqlParameter("@letrehozas_datuma", SqlDbType.Date)).Value = DateTime.Parse(datLetrehoz.Text);
                scommand.Parameters.Add(new SqlParameter("@megjegyzes", SqlDbType.VarChar, 255)).Value = txMegjegyzes.Text;
                scommand.Parameters.Add(new SqlParameter("@modosit_neve", SqlDbType.VarChar, 80)).Value = User.name;

                try
                {
                    if (sconn.State == ConnectionState.Closed) sconn.Open();
                    scommand.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("SQL Hiba " + ex.Message + ex.Data + " update");
                    TraceBejegyzes(ex.Message);
                }
            }
            saveok = true;
        }

        private void bCancel_Click(object sender, EventArgs e)
        {
            if (tsSave.Enabled)
            {
                DialogResult dr = MessageBox.Show("Minden nem mentett adat el fog veszni! Biztosan kilép?", "Megerősítés", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
                if (dr == DialogResult.OK)
                {
                    Close();
                }
            }
            else
            {
                Close();
            }
        }

        private void TraceBejegyzes(string msg)
        {
            myListener.WriteLine("-" + DateTime.Now + ": " + msg);
            myListener.Flush();
            myListener.Close();
        }

        private void datErteknap_TextChanged(object sender, EventArgs e)
        {
            tsSave.Enabled = true;
        }

        private void txKivonatSzama_ValueChanged(object sender, EventArgs e)
        {
            tsSave.Enabled = true;
        }

        private void cbIrany_TextChanged(object sender, EventArgs e)
        {
            tsSave.Enabled = true;
        }

        private void cbIrany_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != 'T' && e.KeyChar != 'K')
            {
                e.Handled = true;
            }
        }
    }
}
