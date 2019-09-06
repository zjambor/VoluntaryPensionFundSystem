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
using System.Security.Cryptography;

namespace VoluntaryPensionFundSystem
{
    public partial class Newuser : VoluntaryPensionFundSystem.BaseForm
    {
        private SqlConnection sconn;
        private SqlCommand scommand;

        public string fhoid, fhonev, teljesnev;

        TextWriterTraceListener myListener = new TextWriterTraceListener(@"LOG\NewUser.log", "myListener");

        public Newuser(SqlConnection SqlConn)
        {
            InitializeComponent();

            this.sconn = SqlConn;
            if (sconn.State == ConnectionState.Closed) sconn.Open();

            tsUpdate.Enabled = false;
            tsNew.Enabled = false;
            tsDelete.Enabled = false;
            tsFind.Enabled = false;
            tsSearch.Enabled = false;
            tsSave.Enabled = false;
        }

        private void Newuser_Load(object sender, EventArgs e)
        {
        }

        private void bOK_Click(object sender, EventArgs e)
        {
            // ellenőrzés
            if (txFhnev.Text == string.Empty) { label8.Text = "A mező nem lehet üres!"; label8.Visible = true; txFhnev.Focus(); return; }
            if (txTeljesnev.Text == string.Empty) { label9.Text = "A mező nem lehet üres!"; label9.Visible = true; txTeljesnev.Focus(); return; }
            if (txJelszo.Text == string.Empty) { label6.Text = "A mező nem lehet üres!"; label6.Visible = true; txJelszo.Focus(); return; }
            if (txJelszo2.Text == string.Empty) { label7.Text = "A mező nem lehet üres!"; label7.Visible = true; txJelszo2.Focus(); return; }

            if ((txJelszo.Text != string.Empty && txJelszo2.Text != string.Empty) && txJelszo.Text == txJelszo2.Text)
            {
                string kodoltjelszo = EncryptData(txJelszo.Text);

                scommand = new SqlCommand("spFelhasznalokInsert", sconn);
                scommand.CommandType = CommandType.StoredProcedure;
                scommand.Parameters.Add(new SqlParameter("@id", SqlDbType.Int)).Value = 1;
                scommand.Parameters.Add(new SqlParameter("@fho_nev", SqlDbType.VarChar, 30)).Value = txFhnev.Text;
                scommand.Parameters.Add(new SqlParameter("@teljes_nev", SqlDbType.VarChar, 80)).Value = txTeljesnev.Text;
                scommand.Parameters.Add(new SqlParameter("@password", SqlDbType.VarChar, 512)).Value = kodoltjelszo;
                scommand.Parameters.Add(new SqlParameter("@erv_kezdete", SqlDbType.Date)).Value = DateTime.Now.ToShortDateString();
                scommand.Parameters.Add(new SqlParameter("@modosit_neve", SqlDbType.VarChar, 80)).Value = User.name;

                try
                {
                    if (sconn.State == ConnectionState.Closed) sconn.Open();
                    scommand.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("SQL Hiba " + ex.Message);
                    TraceBejegyzes(ex.Message);
                }

                MessageBox.Show("Felhasználó létrehozása sikeres!");
                Close();
            }
            else
            {
                label7.Text = "A megadott jelszavak nem egyeznek!";
                label7.Visible = true;
                txJelszo2.Focus();
            }
        }

        private void bCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        public string EncryptData(string stringToEncrypt)
        {
            string EntropyValue = txFhnev.Text;
            byte[] encryptedData = ProtectedData.Protect(Encoding.Unicode.GetBytes(stringToEncrypt), Encoding.Unicode.GetBytes(EntropyValue), DataProtectionScope.LocalMachine);
            return Convert.ToBase64String(encryptedData);
        }

        private void txJelszo_Leave(object sender, EventArgs e)
        {
            if (txJelszo.Text.Length < 6)
            {
                label6.Text = "A jelszó túl rövid!";
                label6.Visible = true;
                txJelszo.Focus();
            }
            else
                label6.Visible = false;
        }

        private void txJelszo2_Leave(object sender, EventArgs e)
        {
            if (txJelszo2.Text.Length < 6)
            {
                label7.Text = "A jelszó túl rövid!";
                label7.Visible = true;
                txJelszo2.Focus();
            }
            else
                label7.Visible = false;
        }

        public override void tsSave_Click(object sender, EventArgs e)
        {
            save();
        }

        public override void save()
        {

        }

        private void TraceBejegyzes(string msg)
        {
            myListener.WriteLine("- " + DateTime.Now + ": " + msg);
            myListener.Flush();
            myListener.Close();
        }

        private void txFhnev_Leave(object sender, EventArgs e)
        {
            if (txFhnev.Text == string.Empty)
            {
                label8.Text = "A mező nem lehet üres!";
                label8.Visible = true;
                txFhnev.Focus();
            }
            else
                label8.Visible = false;
        }

        private void txTeljesnev_Leave(object sender, EventArgs e)
        {
            if (txTeljesnev.Text == string.Empty)
            {
                label9.Text = "A mező nem lehet üres!";
                label9.Visible = true;
                txTeljesnev.Focus();
            }
            else
                label9.Visible = false;
        }
    }
}
