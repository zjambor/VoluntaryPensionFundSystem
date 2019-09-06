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
    public partial class UjPortfolio : BaseForm
    {
        private SqlConnection sconn;
        private SqlCommand scommand;
        // Napló logok:
        TextWriterTraceListener myListener = new TextWriterTraceListener(@"LOG\UjPortfolio.log", "myListener");
        
        public UjPortfolio(SqlConnection SqlConn)
        {
            InitializeComponent();

            this.sconn = SqlConn;

            tsDelete.Enabled = false;
            tsFind.Enabled = false;
            tsNew.Enabled = true;
            tsSave.Enabled = true;
            tsSearch.Enabled = false;
            tsUpdate.Enabled = false;
        }

        public override void tsNew_Click(object sender, EventArgs e)
        {
            tsSave.Enabled = true;
            txLeiras.Text = string.Empty;
            txMegjegyzes.Text = string.Empty;
            txNev.Text = string.Empty;
            txTipus.Text = string.Empty;
            txTipus.Focus();
        }

        public override void tsSave_Click(object sender, EventArgs e)
        {
            save();
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
            else Close();
        }

        public override void save()
        {
            if (!ell()) return;
            int Letezik = 0;

            scommand = new SqlCommand("SELECT count(*) FROM befektetesi_kombinaciok WHERE tipus='" + txTipus.Text+"'", sconn);
            try
            {
                Letezik = (Int32)scommand.ExecuteScalar();
            }
            catch (Exception ex)
            {
                MessageBox.Show("SQL Hiba " + ex.Message);
                TraceBejegyzes(ex.Message);
            }

            if (Letezik == 0)
            {
                scommand = new SqlCommand("spPortfoliokInsert", sconn);
                scommand.CommandType = CommandType.StoredProcedure;

                scommand.Parameters.Add(new SqlParameter("@id", SqlDbType.Int)).Value = 1;
                scommand.Parameters.Add(new SqlParameter("@tipus", SqlDbType.VarChar, 1)).Value = txTipus.Text;
                scommand.Parameters.Add(new SqlParameter("@nev", SqlDbType.VarChar, 80)).Value = txNev.Text;
                scommand.Parameters.Add(new SqlParameter("@leiras", SqlDbType.VarChar, 255)).Value = txLeiras.Text;
                scommand.Parameters.Add(new SqlParameter("@erv_kezdete", SqlDbType.Date)).Value = datKezdete.Value;
                scommand.Parameters.Add(new SqlParameter("@megjegyzes", SqlDbType.VarChar, 255)).Value = txMegjegyzes.Text;
                scommand.Parameters.Add(new SqlParameter("@modosit_neve", SqlDbType.VarChar, 80)).Value = User.name;

                try { scommand.ExecuteNonQuery(); }
                catch (Exception ex) { MessageBox.Show("SQL Hiba " + ex.Message); }
                tsSave.Enabled = false;
            }
            else
            {
                MessageBox.Show("Ez a portfólió típus már létezik!");
            }
        }

        private bool ell()
        {
            return true;
        }

        private void TraceBejegyzes(string msg)
        {
            myListener.WriteLine("-" + DateTime.Now + ": " + msg);
            myListener.Flush();
            myListener.Close();
        }
    }
}
