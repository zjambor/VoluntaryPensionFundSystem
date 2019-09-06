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
    public partial class MunkFolyoszamlaReportIndito : VoluntaryPensionFundSystem.BaseForm
    {
        private SqlConnection sconn;
        private SqlCommand scommand;
        TextWriterTraceListener myListener = new TextWriterTraceListener(@"LOG\MunkFSzlaRepIndito.log", "myListener");
        public string PnrId;

        public MunkFolyoszamlaReportIndito(SqlConnection SqlConn)
        {
            InitializeComponent();

            this.sconn = SqlConn;

            tsDelete.Enabled = false;
            tsFind.Enabled = false;
            tsNew.Enabled = false;
            tsSave.Enabled = false;
            tsSearch.Enabled = false;
            tsUpdate.Enabled = false;

            txPnrid.Text = "1000661";
            dtKezdete.Value = DateTime.Parse("1998.01.01");

            if (sconn.State == ConnectionState.Closed) sconn.Open();
        }

        private void bIndit_Click(object sender, EventArgs e)
        {
            // riport adatok letárolása: pnr id, típus, kezdete, vége, dátum, adóazonosító
            scommand = new SqlCommand("spMunkReportInsert", sconn);
            scommand.CommandType = CommandType.StoredProcedure;

            scommand.Parameters.Add(new SqlParameter("@id", SqlDbType.Int)).Value = 1;
            scommand.Parameters.Add(new SqlParameter("@pnr_id", SqlDbType.Int)).Value = int.Parse(txPnrid.Text);
            scommand.Parameters.Add(new SqlParameter("@kezdete", SqlDbType.Date)).Value = DateTime.Parse(dtKezdete.Text);
            scommand.Parameters.Add(new SqlParameter("@vege", SqlDbType.Date)).Value = DateTime.Parse(dtVege.Text);
            scommand.Parameters.Add(new SqlParameter("@kelt", SqlDbType.Date)).Value = DateTime.Parse(DateTime.Now.ToShortDateString());
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

            MunkFolyoszamlaReportW mfr = new MunkFolyoszamlaReportW(sconn);
            mfr.pnrid = txPnrid.Text;
            mfr.kezdete = dtKezdete.Text;
            mfr.vege = dtVege.Text;
            mfr.Show();
            Close();
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

        private void bFoglKeres_Click(object sender, EventArgs e)
        {
            Fogl_keres fker = new Fogl_keres(sconn);
            fker.ShowDialog();
            int foglpnr = fker.FoglPnrid;

            try
            {
                if (fker.FoglPnrid != 0)
                {
                    this.txPnrid.Text = foglpnr.ToString();
                }
            }
            catch
            {
                // nem történt módosítás
            }
        }
    }
}
